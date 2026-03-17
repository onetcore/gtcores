using GSites.Extensions.Components;

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
        }
    }
}
