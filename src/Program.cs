using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using projekt_io.Data;
using projekt_io.Entities;
using projekt_io.Services;
using projekt_io.Hubs;
using QuestPDF.Infrastructure;


namespace projekt_io;

public class Program {
    public static async Task Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        QuestPDF.Settings.License = LicenseType.Community;

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        // potem mozemy to usunac jak cos, tymczasowo zeby walidacja byla prostsza 
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;

            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        });
        
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IGeocodingService, GeocodingService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ISightingService, SightingService>();
        builder.Services.AddScoped<IMapService, MapService>();
        builder.Services.AddScoped<ILostReportService, LostReportService>();
        builder.Services.AddScoped<IChatService, ChatService>();
        builder.Services.AddTransient<IEmailService, EmailService>();
        builder.Services.AddSignalR();
        builder.Services.AddScoped<IPosterPdfService, PosterPdfService>();

        builder.Services.AddHttpClient("Nominatim", client => {
            client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
            client.DefaultRequestHeaders.Add("User-Agent", "projekt-io/1.0 (kontakt@email.pl)");
        });
        
        builder.Services.ConfigureApplicationCookie(options => {
            options.LoginPath = "/login";
            options.LogoutPath = "/logout";
            options.AccessDeniedPath = "/access-denied";
        });
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapHub<ChatHub>("/hubs/chat");

        using (var scope = app.Services.CreateScope()) {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            List<string> roles = new List<string> { "Admin", "Owner", "Seeker", "User" };

            foreach (var role in roles) {
                var exists = await roleManager.RoleExistsAsync(role);

                if (!exists) {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        
        await app.RunAsync();
    }
}