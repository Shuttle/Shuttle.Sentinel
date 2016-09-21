using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;

namespace Shuttle.Sentinel.Query
{
    public class Role
    {
        public Role()
        {
            Permissions = new EditableList<string>();
        }

        public string Name { get; set; }
        public List<string> Permissions { get; set; }
    }
}