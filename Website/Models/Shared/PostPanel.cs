using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Website.Models.Shared
{
    public class PostPanel
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public string Heading { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDeleted { get; set; }
        public int? OwnerID { get; set; }
        public bool IsClosable { get; set; } = true;
        public bool IsEditable { get; set; } = true;
        public bool IsShrinkable { get; set; } = true;
        public bool IsFulscreen { get; set; }
        public bool IsReloadable { get; set; }

        public List<SelectListItem> Characters { get; set; }
    }
}
