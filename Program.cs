using TeamProjectPlanner.Components;
using TeamProjectPlanner.Data;
using TeamProjectPlanner.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MongoDB settings
var mongoDbSettings = new MongoDbSettings
{
    ConnectionString = builder.Configuration["MongoDB:ConnectionString"] ?? string.Empty,
    DatabaseName = builder.Configuration["MongoDB:DatabaseName"] ?? string.Empty
};

builder.Services.AddSingleton(mongoDbSettings);

// Add project service
builder.Services.AddSingleton<ProjectService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();