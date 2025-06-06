using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WITFamilyArmory;
using WITFamilyArmory.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<DatabaseRepo>(); 
builder.Services.AddTransient<APIRepo>();
builder.Services.AddSingleton<StampHolder>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; 
        options.AccessDeniedPath = "/AccessDenied"; 
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);

    });

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
app.UseAuthentication();
app.Use(async (context, next) =>
{
    var stampHolder = context.RequestServices.GetRequiredService<StampHolder>();

    if (context.User.Identity.IsAuthenticated)
    {
        var claimStamp = context.User.FindFirst("SecurityStamp")?.Value;

        if (claimStamp != stampHolder.Stamp)
        {
            // Stamp mismatch – log brugeren ud
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.Response.Redirect("/Login");
            return;
        }
    }

    await next();
});
app.UseAuthorization();

app.MapRazorPages();

app.Run();
