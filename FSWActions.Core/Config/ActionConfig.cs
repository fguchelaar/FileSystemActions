using System.Xml.Serialization;

namespace FSWActions.Core.Config
{
    public class ActionConfig
    {
        [XmlAttribute("event")]
        public string Event { get; set; }

        [XmlAttribute("command")]
        public string Command { get; set; }

        [XmlAttribute("runOnStartup")]
        public bool RunOnStartup { get; set; }
    }
}