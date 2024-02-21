using Microsoft.EntityFrameworkCore;
using CTS_BE.DAL;
using CTS_BE.BAL.Interfaces.billing;
using CTS_BE.BAL.Services.billing;
using CTS_BE.DAL.Repositories.billing;
using CTS_BE.DAL.Interfaces.billing;
using CTS_BE.DAL.Repositories;
using CTS_BE.DAL.Interfaces;
using CTS_BE.BAL.Services;
using CTS_BE.BAL.Interfaces;
using CTS_BE.BAL;
using CTS_BE.BAL.Services.master;
using CTS_BE.BAL.Interfaces.master;
using CTS_BE.DAL.Repositories.master;
using CTS_BE.DAL.Interfaces.master;
using CTS_BE.Middlewares;
using CTS_BE.Helper.Authentication;
using CTS_BE.BAL.Services.paymandate;
using CTS_BE.BAL.Interfaces.paymandate;

var builder = WebApplication.CreateBuilder(args);

//Database Connection
builder.Services.AddDbContext<CTSDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CTS_BEDBConnection"),
    //options => options.CommandTimeout(999)                   
    options => options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null)
), ServiceLifetime.Transient);

//Automapper
builder.Services.AddAutoMapper(typeof(Program));

// Add services to the container.
//Repositories
builder.Services.AddTransient<ITokenFlowRepository, TokenFlowRepository>();
builder.Services.AddTransient<ITokenHasObjectionsRepository, TokenHasObjectionRepository>();
builder.Services.AddTransient<ILocalObjectionRepository, LocalObjectionRepository>();
builder.Services.AddTransient<IGobalObjectionRepository, GobalObjectionRepository>();
builder.Services.AddTransient<IDdoRepository, DdoRepository>();
builder.Services.AddTransient<ITpBillRepository, TpBillRepository>();
builder.Services.AddTransient<ITokenRepository, TokenRepository>();

//Services
builder.Services.AddTransient<ITokenFlowService, TokenFlowService>();
builder.Services.AddTransient<ITokenHasObjectionService, TokenHasObjectionService>();
builder.Services.AddTransient<ILocalObjectionService, LocalObjectionService>();
builder.Services.AddTransient<IGobalObjectionService, GobalObjectionService>();
builder.Services.AddTransient<IDdoService, DdoService>();
builder.Services.AddTransient<ITpBillService, TpBillService>();
builder.Services.AddTransient<ITokenService, TokenService>();

builder.Services.AddTransient<IPaymandateService, PaymandateService>();

builder.Services.AddTransient<ITokenHelper, TokenHelper>();
builder.Services.AddSingleton<ITokencache, Tokencache>();

builder.Services.AddTransient<IClaimService, ClaimService>();

//builder.Services.AddTransient<ITokenHelper, TokenHelper>();
//builder.Services.AddSingleton<ITokencache, Tokencache>();

//builder.Services.AddTransient<ISixLaborsCaptchaModule, SixLaborsCaptchaModule>();
//builder.Services.AddSixLabCaptcha(x =>
//{
//    // x.FontFamilies = new string[] { "Marlboro" };
//    x.DrawLines = 3;
//    x.FontSize = 35;
//    x.Width = 150;  
//    x.Height = 50;
//    x.NoiseRate = 500;
//});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();

//app.UseAuthorization();

//app.UseAuthTokenMiddleware();

app.MapControllers();

app.Run();
