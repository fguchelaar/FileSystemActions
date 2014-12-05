using System;
using System.Collections.Generic;
using FSWActions.Core;
using FSWActions.Core.Config;

namespace FSWActions.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            WatchersConfiguration configuration = WatchersConfiguration.LoadConfigFromPath("watchers.xml");
            List<Watcher> watchers = new List<Watcher>(configuration.Watchers.Count);

            foreach (WatcherConfig watcherConfig in configuration.Watchers)
            {
                foreach (ActionConfig action in watcherConfig.ActionsConfig)
                {
                    Console.WriteLine("{0} [{1}]\t{2}: {3}", watcherConfig.Path, watcherConfig.Filter, action.Event, action.Command);
                }
                Watcher watcher = new Watcher(watcherConfig);
                watchers.Add(watcher);
                watcher.StartWatching();
            }

            // Wait for the user to quit the program.
            Console.WriteLine("\n\nPress \'q\' to quit\n");
            while (Console.Read() != 'q') ;

            // Stop all watchers, not really necessary though...
            foreach (Watcher watcher in watchers)
            {
                watcher.StopWatching();
            }
        }
    }
}
