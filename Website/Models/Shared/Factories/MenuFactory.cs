using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;

namespace Website.Models.Shared.Factories
{
    public class MenuFactory
    {
        public static MenuViewModel Create()
        {
            MenuViewModel model = new MenuViewModel
            {
                MenuItems = TieredMenu()
            };

            return model;
        }

        private static List<MenuItem> TieredMenu()
        {
            List<MenuItem> result = new List<MenuItem>();

            result.Add(new MenuItem() { Controller = "home", Action = "index", Name = "Home", Icon = "home" });

            result.Add(new MenuItem()
            {
                Name = "Minecraft",
                Icon = "language",
                Children = new List<MenuItem>() {
                    new MenuItem() { Controller = "spotify", Action = "index", Name = "Overviewer"},
                    new MenuItem() { Controller = "spotify", Action = "index", Name = "Papyri"}
                }
            });

            result.Add(new MenuItem()
            {
                Name = "Toys",
                Icon = "favorite",
                Children = new List<MenuItem>() {
                    new MenuItem() { Controller = "spotify", Action = "index", Name = "Spotify"}
                }
            });

            return result;
        }
    }
}
