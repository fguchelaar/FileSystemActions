using System.Collections.ObjectModel;

namespace FSWActions.Core.Config
{
    public class ActionConfigCollection : KeyedCollection<string, ActionConfig>
    {
        protected override string GetKeyForItem(ActionConfig item)
        {
            return item.Event;
        }
    }
}