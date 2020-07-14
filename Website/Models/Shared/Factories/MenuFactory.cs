using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;

namespace Website.Models.Shared.Factories
{
    public class MenuFactory
    {
        private IActionContextAccessor _actionContext;

        public MenuFactory(IActionContextAccessor actionContext)
        {
            _actionContext = actionContext;
        }

        public MenuViewModel Create()
        {
            MenuViewModel model = new MenuViewModel
            {
                MenuItems = TieredMenu()
            };
            
            return model;
        }

        private List<MenuItem> TieredMenu()
        {
            return new List<MenuItem>();
        }
    }
}
