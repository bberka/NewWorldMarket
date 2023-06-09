using EasMe;
using EasMe.Extensions;
using EasMe.Logging;
using EasMe.Result;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewWorldMarket.Web;
using NewWorldMarket.Web.Filters;
using NewWorldMarket.Web.Middleware;
using WebMarkupMin.AspNetCore6;

var isArgsContainDbCreate = args.Any(x => x == "dbcreate");
if (isArgsContainDbCreate)
{
    MarketDbContext.EnsureCreated();
    Environment.Exit(0);
}
#if DEBUG
AssemblyActionExtractor.ExtractEnumFromControllerActions();
#endif

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = c =>
    {
        var errors = c.ModelState.Values
            .Where(v => v.Errors.Count > 0)
            .SelectMany(v => v.Errors)
            .Select(v => v.ErrorMessage)
            .ToArray();
        var firstError = errors.FirstOrDefault();
        return new OkObjectResult(Result.Warn(firstError));
    };
});

builder.Services.AddInfrastructureDependencies();
builder.Services.AddBusinessDependencies();

#if !DEBUG
builder.Services.AddControllersWithViews(x =>
{
    x.Filters.Add(new ExceptionHandleFilter());
});
#endif
builder.Services.AddHttpContextAccessor();


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services
    .AddAuthentication(op =>
    {
        op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer("Bearer", token =>
    {
#if DEBUG
        token.RequireHttpsMetadata = false;
#endif
        token.SaveToken = true;
        token.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(EasConfig.GetString("JwtToken")?.ConvertToByteArray()),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddResponseCaching();
builder.Services.AddAuthorization();

builder.Services.AddMemoryCache();
builder.Services.AddDataProtection();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".Session.NewWorldBiSMarket";
});

builder.Services.AddHttpClient("NW_BISM").ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
});

builder.Services.AddWebMarkupMin(
        options =>
        {
            options.AllowMinificationInDevelopmentEnvironment = true;
            options.AllowCompressionInDevelopmentEnvironment = true;
        })
    .AddHtmlMinification(
        options =>
        {
            options.MinificationSettings.RemoveRedundantAttributes = true;
            options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
            options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
        })
    .AddHttpCompression();

var app = builder.Build();


#if !DEBUG
app.UseWebMarkupMin(); // FOR HTML Minify
#endif

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseMiddleware<AuthMiddleware>();


//app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

EasLogFactory.Configure(x =>
{
    x.WebInfoLogging = true;
    x.MinimumLogLevel = EasLogLevel.Information;
    x.LogFileName = "Log_";
    x.ExceptionHideSensitiveInfo = false;
});
AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) =>
{
    try
    {
        EasLogFactory.StaticLogger
            .Exception((Exception?)e.ExceptionObject ??
                       new Exception("FATAL|EXCEPTION IS NULL"), "UnhandledException");
    }
    catch (Exception)
    {
        EasLogFactory.StaticLogger.Fatal(e.ToJsonString(), "UnhandledException");
    }
};


app.Run();