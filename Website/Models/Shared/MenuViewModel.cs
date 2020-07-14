using System;
using System.Collections.Generic;
using System.Linq;

namespace Website.Models
{
    public class MenuViewModel
    {
        public List<MenuItem> MenuItems { get; set; }
    }

    public class MenuItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }

        // Relationships
        public MenuItem Parent { get; set; }
        public int? ParentID { get; internal set; }
        public List<MenuItem> Children { get; set; }

        // Reference Mapping
        public bool HasChildren => Children?.Any() ?? false;

        public bool IsActive(string area, string controller)
        {
            return (area.Equals(Area, StringComparison.CurrentCultureIgnoreCase) && controller.Equals(Controller, StringComparison.CurrentCultureIgnoreCase))
                || (Children != null && Children.Any(x => x.IsActive(area, controller)));
        }
    }
}