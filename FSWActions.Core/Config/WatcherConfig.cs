using System;
using System.Xml.Serialization;

namespace FSWActions.Core.Config
{
    public class WatcherConfig
    {
        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("filter")]
        public string Filter { get; set; }

        [XmlAttribute("timeout")]
        public int Timeout { get; set; }

        [XmlAttribute("debounceOnFolder")]
        public bool DebounceOnFolder { get; set; }
    
        [XmlElement("action")]
        public ActionConfigCollection ActionsConfig { get; set; }
    }
}