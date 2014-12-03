using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace FSWActions.Core.Config
{
    [XmlRoot("watchers")]
    public class WatchersConfiguration
    {
        [XmlElement("watcher")]
        public Collection<WatcherConfig> Watchers { get; set; }

        public static WatchersConfiguration LoadConfigFromPath(string configPath)
        {
            FileStream fs = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(WatchersConfiguration));
                fs = new FileStream(configPath, FileMode.Open, FileAccess.Read);
                return (WatchersConfiguration)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
    }
}