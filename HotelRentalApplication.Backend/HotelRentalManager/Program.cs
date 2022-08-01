using Hellang.Middleware.ProblemDetails;
using HotelRentalManager.BusinessLayer.Extensions;
using HotelRentalManager.BusinessLayer.Services;
using HotelRentalManager.BusinessLayer.Services.Interfaces;
using HotelRentalManager.BusinessLayer.Settings;
using HotelRentalManager.Contracts;
using HotelRentalManager.Extensions;
using HotelRentalManager.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = Configure<JwtSettings>(nameof(JwtSettings));

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddProblemDetails();
builder.Services.AddMapperProfiles();
builder.Services.AddValidators();
builder.Services.AddSwaggerSettings();

string connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDataContext(connectionString);

builder.Services.AddIdentitySettings(jwtSettings);

builder.Services.AddScoped<IUserService, HttpUserService>();
builder.Services.AddScoped<IAuthenticatedService, AuthenticatedService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

T Configure<T>(string sectionName) where T : class
{
    var section = builder.Configuration.GetSection(sectionName);
    var settings = section.Get<T>();
    builder.Services.Configure<T>(section);
    return settings;
}

var app = builder.Build();
app.UseProblemDetails();
app.UseSwaggerSettings();
app.UseSerilogRequestLogging(options =>
{
    options.IncludeQueryInRequestPath = true;
});
app.UseHttpsRedirection();
app.UseRouting();
app.UseIdentitySettings();
app.MapControllers();
await app.RunAsync();