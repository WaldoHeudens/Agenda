using Agenda_Models;
using Agenda_Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AgendaDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AgendaDbContextConnection' not found.");;

//Add the dbContext service
builder.Services.AddDbContext<Agenda_Models.AgendaDbContext>();

builder.Services.AddDefaultIdentity<AgendaUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AgendaDbContext>();

//// Toevoegen van UserManager / SignInManager voor AgendaUser
//builder.Services.AddScoped(typeof(SignInManager<Microsoft.AspNetCore.Identity.IdentityUser>),
//    sp => sp.GetRequiredService<SignInManager<AgendaUser>>());

// Add services to the container.
builder.Services.AddControllersWithViews();

// Voor de configuratie van Restful API's
builder.Services.AddControllers();

// Voor het gebruik van Swagger
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Agenda_Web", Version = "v1" }); });


// Configureer logging via de databank
builder.Logging.AddDbLogger(options =>
    {
        builder.Configuration
            .GetSection("Logging");
    }
);

// Toevoegen van localisatie services
builder.Services.AddLocalization(options => options.ResourcesPath = "Translations");
builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Hier kunnen we initialisatiecode plaatsen, zoals het seeden van de database
        Agenda_Models.AgendaDbContext context = services.GetRequiredService<Agenda_Models.AgendaDbContext>();
        Agenda_Models.AgendaDbContext.Seeder(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agenda_Web v1"));
}

// Toevoegen van middleware voor localisatie
var supportedCultures = new[] { "nl", "en", "fr" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


app.Run();
