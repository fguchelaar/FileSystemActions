using System;
using System.Collections.Generic;
using FSWActions.Core;
using FSWActions.Core.Config;

namespace FSWActions.ConsoleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var configuration = WatchersConfiguration.LoadConfigFromPath("watchers.xml");
            var watchers = new List<Watcher>(configuration.Watchers.Count);

            foreach (var watcherConfig in configuration.Watchers)
            {
                foreach (var action in watcherConfig.ActionsConfig)
                {
                    Console.WriteLine("{0} [{1}]\t{2}: {3}", watcherConfig.Path, watcherConfig.Filter, action.Event,
                        action.Command);
                }
                var watcher = new Watcher(watcherConfig);
                watchers.Add(watcher);
                watcher.StartWatching();
            }

            // Wait for the user to quit the program.
            Console.WriteLine("\n\nEnter \'q\' to quit\n");
            while (Console.Read() != 'q') ;

            // Stop all watchers, not really necessary though...
            foreach (var watcher in watchers)
            {
                watcher.StopWatching();
            }
        }
    }
}