﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;
using FSWActions.Core.Config;

namespace FSWActions.Core
{
    public class Watcher
    {
        public Watcher(WatcherConfig config)
        {
            Config = config;
        }

        private WatcherConfig Config { get; set; }
        private FileSystemWatcher FileSystemWatcher { get; set; }
        private IDictionary<string, Timer> DebounceTimers { get; set; }

        public void StartWatching()
        {
            FileSystemWatcher = new FileSystemWatcher(Config.Path);

            if (!string.IsNullOrEmpty(Config.Filter))
            {
                FileSystemWatcher.Filter = Config.Filter;
            }

            foreach (var actionConfig in Config.ActionsConfig)
            {
                var config = actionConfig;
                if (string.Equals(actionConfig.Event, "onCreated"))
                {
                    FileSystemWatcher.Created +=
                        (sender, args) => Debounce(DebounceKeyForArgs(args), delegate { ProcessEvent(args, config); });
                }
                else if (string.Equals(actionConfig.Event, "onChanged"))
                {
                    FileSystemWatcher.Changed +=
                        (sender, args) => Debounce(DebounceKeyForArgs(args), delegate { ProcessEvent(args, config); });
                }
                else if (string.Equals(actionConfig.Event, "onRenamed"))
                {
                    FileSystemWatcher.Renamed +=
                        (sender, args) => Debounce(DebounceKeyForArgs(args), delegate { ProcessRenamedEvent(args, config); });
                }
                else if (string.Equals(actionConfig.Event, "onDeleted"))
                {
                    FileSystemWatcher.Deleted +=
                        (sender, args) => Debounce(DebounceKeyForArgs(args), delegate { ProcessEvent(args, config); });
                }

                if (actionConfig.RunOnStartup)
                {
                    Console.WriteLine("Run on startup: {0}", actionConfig.Command);
                    RunAction(actionConfig);
                }
            }

            FileSystemWatcher.EnableRaisingEvents = true;
            Console.WriteLine("Started watching '{0}'{1}{2}", Config.Path,
                Config.Timeout != 0 ? string.Format(" timeout={0}", Config.Timeout) : string.Empty,
                Config.DebounceOnFolder ? " debounce on folder" : string.Empty);
        }

        private string DebounceKeyForArgs(FileSystemEventArgs args)
        {
            return Config.DebounceOnFolder ? new FileInfo(args.FullPath).DirectoryName : args.FullPath;
        }

        public void StopWatching()
        {
            FileSystemWatcher.EnableRaisingEvents = false;
            Console.WriteLine("Stopped watching '{0}'", Config.Path);
        }

        private void Debounce(string key, Action action)
        {
            if (Config.Timeout <= 0)
            {
                action();
                return;
            }
            if (DebounceTimers == null) DebounceTimers = new Dictionary<string, Timer>();

            if (DebounceTimers.ContainsKey(key))
            {
                DebounceTimers[key].Stop();
                DebounceTimers.Remove(key);
            }

            var debounceTimer = new Timer(Config.Timeout) {AutoReset = false};
            debounceTimer.Elapsed += delegate
            {
                DebounceTimers.Remove(key);
                action();
            };
            DebounceTimers.Add(key, debounceTimer);
            debounceTimer.Start();
        }

        private void ProcessRenamedEvent(RenamedEventArgs renamedEventArgs, ActionConfig actionConfig)
        {
            Console.WriteLine("[{0}] Command: {1}", renamedEventArgs.ChangeType, actionConfig.Command);

            RunAction(actionConfig);
        }

        private void ProcessEvent(FileSystemEventArgs fileSystemEventArgs, ActionConfig actionConfig)
        {
            Console.WriteLine("[{0}] Command: {1}", fileSystemEventArgs.ChangeType, actionConfig.Command);
            RunAction(actionConfig);
        }

        private static void RunAction(ActionConfig actionConfig)
        {
            var processStartInfo = new ProcessStartInfo(actionConfig.Command)
            {
                WorkingDirectory = new FileInfo(actionConfig.Command).DirectoryName
            };
            Process.Start(processStartInfo);
        }
    }
}