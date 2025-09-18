using GtCores;
using GSites.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGtCores(builder.Configuration);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseRequestLocalization(options =>
{
    var supportedCultures = new[] { "en-US", "zh-CN" };
    options.SetDefaultCulture("zh-CN")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
});
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
