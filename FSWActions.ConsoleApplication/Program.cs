using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FSWActions.Core;
using FSWActions.Core.Config;

namespace FSWActions.ConsoleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var configuration = WatchersConfiguration.LoadConfigFromPath("watchers.xml");

            if (ValidateWatchers(configuration.Watchers))
            {
                StartWatching(configuration.Watchers);
            }
        }

        private static bool ValidateWatchers(Collection<WatcherConfig> watchers)
        {
            bool valid = true;
            foreach (var watcherConfig in watchers)
            {
                if (!Directory.Exists(watcherConfig.Path))
                {
                    valid = false;
                    Console.WriteLine("Watcher path not found: {0}", watcherConfig.Path);
                }

                foreach (var action in watcherConfig.ActionsConfig)
                {
                    if (!File.Exists(action.Command))
                    {
                        valid = false;
                        Console.WriteLine("Action command not found: {0}", action.Command);
                    }
                }
            }

            if (!valid)
            {
                // Wait for the user to quit the program.
                Console.WriteLine("\n\nPress any key to continue\n");
                Console.ReadKey(true);
            }

            return valid;
        }

        private static void StartWatching(Collection<WatcherConfig> watchers)
        {
            var listOfWatchers = new List<Watcher>(watchers.Count);

            foreach (var watcherConfig in watchers)
            {
                foreach (var action in watcherConfig.ActionsConfig)
                {
                    Console.WriteLine("{0} [{1}]\t{2}: {3}", watcherConfig.Path, watcherConfig.Filter, action.Event,
                        action.Command);
                }
                var watcher = new Watcher(watcherConfig);
                listOfWatchers.Add(watcher);
                watcher.StartWatching();
            }

            // Wait for the user to quit the program.
            Console.WriteLine("\n\nEnter \'q\' to quit\n");
            while (Console.Read() != 'q') ;

            // Stop all watchers, not really necessary though...
            foreach (var watcher in listOfWatchers)
            {
                watcher.StopWatching();
            }
        }
    }
}