using HalloDocBAL.Interfaces;
using HalloDocBAL.Services;
using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Model;
using HalloDocDAL.Repositories;
using HelloDoc_BAL.Services;
using Newtonsoft.Json.Serialization;
using Rotativa.AspNetCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});
var smtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton(smtpSettings);
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddSession();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
builder.Services.AddScoped<IConciergeRepository, ConciergeRepository>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IPhysicianRepository, PhysicianRepository>();
builder.Services.AddScoped<IRequestClientRepository, RequestClientRepository>();
builder.Services.AddScoped<IRequestConciergeRepository, RequestConciergeRepository>();
builder.Services.AddScoped<IRequestBusinessRepository, RequestBusinessRepository>();
builder.Services.AddScoped<IRequestWiseFilesRepository, RequestWiseFilesRepository>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IRecordsRepository, RecordsRepository>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
});
builder.Services.AddSignalR();
ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseRotativa();
app.MapHub<ChatHub>("/chathub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
