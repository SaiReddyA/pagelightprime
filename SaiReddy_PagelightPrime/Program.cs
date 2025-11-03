using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
builder.Services.AddDbContext<PageLightPrimeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<PageLightPrimeContext>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Location}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
