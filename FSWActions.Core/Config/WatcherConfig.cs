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

        [XmlElement("action")]
        public ActionConfigCollection ActionsConfig { get; set; }
    }
}