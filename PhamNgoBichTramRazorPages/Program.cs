var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages(options =>
{
    // Default UI is Login
    options.Conventions.AddPageRoute("/Login", "");
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(2);
});

builder.Services.AddSignalR();

// Configure Data layer singleton factory (connection string from appsettings.json)
var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
             ?? throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection in appsettings.json");
FUNewsManagement.Data.DbContextFactorySingleton.Instance.Configure(connStr);

// Repositories
builder.Services.AddScoped<FUNewsManagement.Repository.Interfaces.IAccountRepository, FUNewsManagement.Repository.Implementations.AccountRepository>();
builder.Services.AddScoped<FUNewsManagement.Repository.Interfaces.ICategoryRepository, FUNewsManagement.Repository.Implementations.CategoryRepository>();
builder.Services.AddScoped<FUNewsManagement.Repository.Interfaces.INewsRepository, FUNewsManagement.Repository.Implementations.NewsRepository>();
builder.Services.AddScoped<FUNewsManagement.Repository.Interfaces.ITagRepository, FUNewsManagement.Repository.Implementations.TagRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseMiddleware<PhamNgoBichTramRazorPages.Security.SimpleAuthMiddleware>();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<PhamNgoBichTramRazorPages.Hubs.NewsHub>("/hubs/news");

app.Run();
