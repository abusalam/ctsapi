using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using CTS_BE.PensionEnum;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.BAL.Services.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DAL.Repositories.Pension;
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
using CTS_BE.DAL.Interfaces.stamp;
using CTS_BE.DAL.Repositories.stamp;
using CTS_BE.BAL.Interfaces.stamp;
using CTS_BE.BAL.Services.stamp;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using CTS_BE.Enum;
using CTS_BE.Helper;
using System.Collections;
using CTS_BE.DAL.Interfaces.stampRequisition;
using CTS_BE.DAL.Repositories.stampRequisition;
using CTS_BE.BAL.Interfaces.stampRequisition;
using CTS_BE.BAL.Services.stampRequisition;

var builder = WebApplication.CreateBuilder(args);

//Database Connection
builder.Services.AddDbContext<CTSDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection"),
    //options => options.CommandTimeout(999)                   
    options => options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null)
), ServiceLifetime.Transient);

//Pension Database Connection
// https://www.npgsql.org/efcore/mapping/enum.html?tabs=with-datasource
var dataSourceBuilder = new NpgsqlDataSourceBuilder(
        builder.Configuration.GetConnectionString("DBConnection")
    );
dataSourceBuilder.MapEnum<PensionStatusFlag>();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<PensionDbContext>(
    options => {
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("DBConnection"),
            //options => options.CommandTimeout(999)                   
            options => options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null)
        );
        options.UseNpgsql(dataSource);
    },
    ServiceLifetime.Transient
);


// Hide non OpenAPI Conventions from Swagger.
// builder.Services.AddMvc(c =>
//     c.Conventions.Add(new OpenApiConvention())
// );

builder.Services.AddSwaggerGen(c =>
{
    c.AddServer(new (){
        Url = "http://api.docker.test"
    });
    c.AddServer(new (){
        Url = "https://localhost:7249"
    });
    c.AddServer(new (){
        Url = "http://localhost:7249"
    });
    // Use method name as operationId
    c.CustomOperationIds(apiDesc =>
    {
        return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
    });
});

//Pension Repositories
builder.Services.AddTransient<IManualPpoReceiptRepository, ManualPpoReceiptRepository>();
builder.Services.AddTransient<IReceiptSequenceRepository, ReceiptSequenceRepository>();
builder.Services.AddTransient<IPensionStatusRepository, PensionStatusRepository>();
builder.Services.AddTransient<IPensionerDetailsRepository, PensionerDetailsRepository>();
builder.Services.AddTransient<IPpoIdSequenceRepository, PpoIdSequenceRepository>();
builder.Services.AddTransient<IPensionerBankAccountRepository, PensionerBankAccountRepository>();
builder.Services.AddTransient<IPrimaryCategoryRepository, PrimaryCategoryRepository>();
builder.Services.AddTransient<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IBreakupRepository, BreakupRepository>();
builder.Services.AddTransient<IComponentRateRepository, ComponentRateRepository>();
builder.Services.AddTransient<IPpoComponentRevisionRepository, PpoComponentRevisionRepository>();


// Pension Services
builder.Services.AddTransient<IPpoReceiptService, PpoReceiptService>();
builder.Services.AddTransient<IReceiptSequenceService, ReceiptSequenceService>();
builder.Services.AddTransient<IPensionStatusService, PensionStatusService>();
builder.Services.AddTransient<IPensionerDetailsService, PensionerDetailsService>();
builder.Services.AddTransient<IPensionerBankAccountService, PensionerBankAccountService>();
builder.Services.AddTransient<IPensionBillService, PensionBillService>();
builder.Services.AddTransient<IPensionCategoryService, PensionCategoryService>();
builder.Services.AddTransient<IPensionBreakupService, PensionBreakupService>();
builder.Services.AddTransient<IComponentRateService, ComponentRateService>();
builder.Services.AddTransient<IPpoComponentRevisionService, PpoComponentRevisionService>();
// builder.Services.AddTransient<IPensionCategoryService, PensionCategoryService>();



//Automapper
builder.Services.AddAutoMapper(typeof(Program));

// Add services to the container.
//Repositories
builder.Services.AddTransient<IBillBtdetailRepository, BillBtdetailRepository>();
builder.Services.AddTransient<IDdoAllotmentBookedBillRepository, DdoAllotmentBookedBillRepository>();
builder.Services.AddTransient<IChequeCountRepository, ChequeCountRepository>();
builder.Services.AddTransient<IEcsNeftDetailRepository, EcsNeftDetailRepository>();
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
builder.Services.AddTransient<ITransactionLotRepository, TransactionLotRepository>();

builder.Services.AddTransient<IChequeInvoiceDetailRepository, ChequeInvoiceDetailRepository>();
builder.Services.AddTransient<IChequeReceivedRepository, ChequeReceivedRepository>();

builder.Services.AddTransient<IChequeDistributionRepository, ChequeDistributionRepository>();


builder.Services.AddTransient<IStampLabelRepository, StampLabelRepository>();
builder.Services.AddTransient<IStampCategoryRepository, StampCategoryRepository>();
builder.Services.AddTransient<IStampVendorRepository, StampVendorRepository>();
builder.Services.AddTransient<IStampTypeRepository, StampTypeRepository>();
builder.Services.AddTransient<IDiscountDetailsRepository, DiscountDetailsRepository>();
builder.Services.AddTransient<IStampVendorTypeRepository, StampVendorTypeRepository>();
// builder.Services.AddTransient<IStampCategoryTypeRepository, StampCateroryTypeRepository>();
builder.Services.AddTransient<IStampCombinationRepository, StampCombinationRepository>();
builder.Services.AddTransient<IStampIndentRepository, StampIndentRepository>();
builder.Services.AddTransient<IStampInvoiceRepository, StampInvoiceRepository>();
builder.Services.AddTransient<IStampWalletRepository, StampWalletRepository>();
builder.Services.AddTransient<IStampRequisitionRepository, StampRequisitionRepository>();
builder.Services.AddTransient<IStampRequisitionApproveRepository, StampRequisitionApproveRepository>();
builder.Services.AddTransient<IStampRequisitionChallanGenerateRepository, StampRequisitionChallanGenerateRepository>();
builder.Services.AddTransient<IStampRequisitionStagingRepository, StampRequisitionStagingRepository>();




//Services
builder.Services.AddTransient<IBillBtdetailService, BillBtdetailService>();
builder.Services.AddTransient<IDdoAllotmentBookedBillService, DdoAllotmentBookedBillService>();
builder.Services.AddTransient<IChequeCountService, ChequeCountService>();
builder.Services.AddTransient<IEcsNeftDetailService, EcsNeftDetailService>();
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
builder.Services.AddTransient<IStampMasterService, StampMasterService>();
builder.Services.AddTransient<IStampService, StampService>();
builder.Services.AddTransient<IStampWalletService, StampWalletService>();
builder.Services.AddTransient<IStampRequisitionService, StampRequisitionService>();


builder.Services.AddTransient<IPaymandateService, PaymandateService>();
builder.Services.AddTransient<ITransactionLotService, TransactionLotService>();

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
   c.SwaggerDoc("v1", new OpenApiInfo { Title = "CTS-BE", Version = "v1" });

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
builder.Services.Configure<ApiBehaviorOptions>(config =>
{

    config.InvalidModelStateResponseFactory = ctx => new BadRequestObjectResult(
        new APIResponse<IEnumerable>()
    {
        apiResponseStatus = APIResponseStatus.Error,
        result = ctx.ModelState.Values,
        Message = "DTO validation error :: result field specifies error location." + ctx.ModelState.Values
    }


  // new BaseResponse(
  // success: ctx.ModelState.IsValid,
  // errors: ctx.ModelState.Values
  //     .Where(v => v.ValidationState == ModelValidationState.Invalid)
  //     .SelectMany(v => v.Errors)
  //     .Select(e => new ErrorDetails
  //     {
  //         Code = "ModelError",
  //         Description = e.ErrorMessage
  //     })
  //     .ToList()
  // )

    );
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.EnableFilter();
        options.EnablePersistAuthorization();
        options.EnableValidator();
        options.EnableDeepLinking();
        options.DisplayRequestDuration();
        options.ShowExtensions();
        options.DocExpansion(DocExpansion.None);
    });
}
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseStaticFiles();
// app.UseDirectoryBrowser(new DirectoryBrowserOptions
// {
//     FileProvider = new PhysicalFileProvider(
//         Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
//     RequestPath = "/uploads"
// });

app.UseAuthTokenMiddleware();

app.MapControllers();

app.Run();

public partial class Program { }