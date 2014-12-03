using System;
using FSWActions.Core.Config;

namespace FSWActions.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            WatchersConfiguration configuration = WatchersConfiguration.LoadConfigFromPath("watchers.xml");

            foreach (WatcherConfig watcher in configuration.Watchers)
            {
                foreach (ActionConfig action in watcher.ActionsConfig)
                {
                    Console.WriteLine("[{0}] {1}: {2}", watcher, action.Event, action.Command);
                }
            }

            Console.ReadKey(true);
        }
    }
}
