using GSites.Extensions.Menus;

namespace GSites.Components.Pages
{
    public class MenuDataProvider : MenuDataProviderBase
    {
        public override void Initialized(NavMenuItemCollection menu)
        {
            menu.AddMenu("counter", "计算器", Extensions.IconName.Clock, item => item
                .AddMenu("Counter/Overview1", "Overview1", action: item => item.Authorize("Admin"))
                .AddMenu("Counter/Analytics1", "Analytics1", action: item => item.Authorize("Admin")));
            menu.AddMenu("/", item => item
                .AddMenu("Home/Overview", "Overview", Extensions.IconName.Speedometer)
                .AddMenu("Home/Analytics", "Analytics", Extensions.IconName.BarChart));
            menu.AddMenu("Weather", "Weather", Extensions.IconName.Gear);
            menu.AddMenu("auth", "Auth Required", Extensions.IconName.Lock);
        }
    }
}
