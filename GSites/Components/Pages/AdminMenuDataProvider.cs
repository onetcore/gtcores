using GSites.Extensions.Components;
using GSites.Extensions.Menus;

namespace GSites.Components.Pages
{
    public class AdminMenuDataProvider : MenuDataProvider
    {
        public override void Initialized(NavMenuItemCollection menu)
        {
            menu.AddMenu("计算器", "counter", Extensions.IconName.Clock, item => item.AddMenu("Overview1", "Counter/Overview1")
            .AddMenu("Analytics1", "Counter/Analytics1"));
            menu.AddOrUpdate("/", item => item.AddMenu("Overview", "Home/Overview", Extensions.IconName.Speedometer)
            .AddMenu("Analytics", "Home/Analytics", Extensions.IconName.BarChart));
            menu.AddMenu("Weather", "Weather", Extensions.IconName.Gear);
            menu.AddMenu("Auth Required", "auth", Extensions.IconName.Lock);
        }
    }
}
