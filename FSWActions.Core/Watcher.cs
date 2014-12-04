using System;
using System.Diagnostics;
using System.IO;
using FSWActions.Core.Config;

namespace FSWActions.Core
{
    public class Watcher
    {
        private WatcherConfig Config { get; set; }

        public Watcher(WatcherConfig config)
        {
            Config = config;
        }

        public void StartWatching()
        {
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(Config.Path);

            if (!string.IsNullOrEmpty(Config.Filter))
            {
                fileSystemWatcher.Filter = Config.Filter;
            }

            foreach (ActionConfig actionConfig in Config.ActionsConfig)
            {
                ActionConfig config = actionConfig;
                if (string.Equals(actionConfig.Event, "onCreated"))
                {
                    fileSystemWatcher.Created += (sender, args) => ProcessEvent(args, config);
                }
                else if (string.Equals(actionConfig.Event, "onChanged"))
                {
                    fileSystemWatcher.Changed += (sender, args) => ProcessEvent(args, config);
                }
                else if (string.Equals(actionConfig.Event, "onRenamed"))
                {
                    fileSystemWatcher.Renamed += (sender, args) => ProcessRenamedEvent(args, config);
                }
                else if (string.Equals(actionConfig.Event, "onDeleted"))
                {
                    fileSystemWatcher.Deleted += (sender, args) => ProcessEvent(args, config);
                }
            }

            fileSystemWatcher.EnableRaisingEvents = true;
            Console.WriteLine("Started watching '{0}'", Config.Path);
        }

        private static void ProcessRenamedEvent(RenamedEventArgs renamedEventArgs, ActionConfig actionConfig)
        {
            Console.WriteLine("[{0}] Command: {1}", renamedEventArgs.ChangeType, actionConfig.Command);

            ProcessStartInfo processStartInfo = new ProcessStartInfo(actionConfig.Command);
            Process.Start(processStartInfo);
        }

        private static void ProcessEvent(FileSystemEventArgs fileSystemEventArgs, ActionConfig actionConfig)
        {
            Console.WriteLine("[{0}] Command: {1}", fileSystemEventArgs.ChangeType, actionConfig.Command);

            ProcessStartInfo processStartInfo = new ProcessStartInfo(actionConfig.Command);
            Process.Start(processStartInfo);
        }
    }
}
