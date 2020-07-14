using System.Collections.Generic;

namespace Website.Models.Shared
{
    public class PostViewModel
    {
        public List<PostPanel> PublicSections { get; set; }
        public List<PostPanel> PrivateSections { get; set; }
        public object NoticeSection { get; set; }
    }
}
