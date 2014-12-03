using System.Xml.Serialization;

namespace FSWActions.Core.Config
{
    public class WatcherConfig
    {
        [XmlElement("action")]
        public ActionConfigCollection ActionsConfig { get; set; }
    }
}