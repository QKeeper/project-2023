using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using yaDirectParser.Middlewares;
using Swashbuckle.Swagger;
using yaDirectParser;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var assembly = Assembly.GetAssembly(typeof(MappingProfile));
builder.Services.AddAutoMapper(assembly);
builder.Services.AddSession();
builder.Services.AddScoped<yaDirectService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "Yandex";
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.Name = "DirectParser";
        options.LoginPath = "/Account/Login";
    })
    .AddOAuth("Yandex", options =>
{
    options.ClientId = "31723453426541b99e7cadec90ae4d15";
    options.ClientSecret = "a8d7438ae65546d8b94d1038097b5194";
    options.CallbackPath = "/home/index";

    options.AuthorizationEndpoint = "https://oauth.yandex.ru/authorize";
    options.TokenEndpoint = "https://oauth.yandex.ru/token";
    options.Events = new OAuthEvents
    {
        OnCreatingTicket = async context =>
        {
            var httpContextAccessor = context.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext;
            var accessToken = context.TokenResponse?.AccessToken;
            var clientLogin = httpContext.Session.GetString("ClientLogin");

            if (accessToken != null)
            {
                httpContext.Session.SetString("AccessToken", accessToken);
            }
        
            await Task.CompletedTask;
        }
    };
});
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();