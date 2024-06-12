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
using Microsoft.OpenApi.Models;

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
builder.Services.AddTransient<IChequeCountRepository, ChequeCountRepository>();
builder.Services.AddTransient<ITreasuryRepository, TreasuryRepository>();
builder.Services.AddTransient<IChequeInvoiceRepository, ChequeInvoiceRepository>();
builder.Services.AddTransient<IBranchRepository, BranchRepository>();
builder.Services.AddTransient<IBankRepository, BankRepository>();
builder.Services.AddTransient<IChequeIndentRepository, ChequeIndentRepository>();
builder.Services.AddTransient<IChequeEntryRepository, ChequeEntryRepository>();
builder.Services.AddTransient<IVoucherRepository, VoucherRepository>();
builder.Services.AddTransient<ITokenFlowRepository, TokenFlowRepository>();
builder.Services.AddTransient<ITokenHasObjectionsRepository, TokenHasObjectionRepository>();
builder.Services.AddTransient<ILocalObjectionRepository, LocalObjectionRepository>();
builder.Services.AddTransient<IGobalObjectionRepository, GobalObjectionRepository>();
builder.Services.AddTransient<IDdoRepository, DdoRepository>();
builder.Services.AddTransient<ITpBillRepository, TpBillRepository>();
builder.Services.AddTransient<ITokenRepository, TokenRepository>();

builder.Services.AddTransient<IChequeInvoiceDetailRepository, ChequeInvoiceDetailRepository>();
builder.Services.AddTransient<IChequeReceivedRepository, ChequeReceivedRepository>();

builder.Services.AddTransient<IChequeDistributionRepository, ChequeDistributionRepository>();




//Services
builder.Services.AddTransient<IChequeCountService, ChequeCountService>();
builder.Services.AddTransient<ITreasuryService, TreasuryService>();
builder.Services.AddTransient<IChequeInvoiceService, ChequeInvoiceService>();
builder.Services.AddTransient<IBranchService, BranchService>();
builder.Services.AddTransient<IBankService, BankService>();
builder.Services.AddTransient<IChequeIndentService, ChequeIndentService>();
builder.Services.AddTransient<IChequeEntryService, ChequeEntryService>();
builder.Services.AddTransient<IVoucherService, VoucherService>();
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

builder.Services.AddTransient<IChequeReceivedService, ChequeReceivedService>();
builder.Services.AddTransient<IChequeDistributionService, ChequeDistributionService>();

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
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
   c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test01", Version = "v1" });

   c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
   {
       Name = "Authorization",
       Type = SecuritySchemeType.ApiKey,
       Scheme = "Bearer",
       BearerFormat = "JWT",
       In = ParameterLocation.Header,
       Description = "JWT Authorization header using the Bearer scheme."

   });
   c.AddSecurityRequirement(new OpenApiSecurityRequirement
               {
                   {
                         new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = "Bearer"
                             }
                         },
                        new string[] {}
                   }
               });
});

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
 app.UseAuthTokenMiddleware();

app.UseAuthTokenMiddleware();

app.MapControllers();

app.Run();
