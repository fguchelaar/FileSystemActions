using System.Collections.Generic;
using System.ServiceProcess;
using FSWActions.Core;
using FSWActions.Core.Config;

namespace FSWActions.WindowsService
{
    public partial class Service : ServiceBase
    {
        private List<Watcher> Watchers { get; set; }

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WatchersConfiguration configuration = WatchersConfiguration.LoadConfigFromPath("watchers.xml");
            Watchers = new List<Watcher>(configuration.Watchers.Count);

            foreach (WatcherConfig watcherConfig in configuration.Watchers)
            {
                Watcher watcher = new Watcher(watcherConfig);
                Watchers.Add(watcher);
                watcher.StartWatching();
            }
        }

        protected override void OnStop()
        {
            foreach (Watcher watcher in Watchers)
            {
                watcher.StopWatching();
            }
        }
    }
}
