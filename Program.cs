using System.Collections;
using System.Reflection;
using System.Text.Json.Serialization;
using CTS_BE.Adapters;
using CTS_BE.BAL.Interfaces;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.BAL.Services;
using CTS_BE.BAL.Services.Pension;
using CTS_BE.DAL;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DAL.Repositories.Pension;
using CTS_BE.Enum;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.Middlewares;
using CTS_BE.PensionEnum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Npgsql;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

//Pension Database Connection
// https://www.npgsql.org/efcore/mapping/enum.html?tabs=with-datasource
var dataSourceBuilder = new NpgsqlDataSourceBuilder(
    builder.Configuration.GetConnectionString("DBConnection")
);
dataSourceBuilder.MapEnum<PensionStatusFlag>();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<PensionDbContext>(
    options =>
    {
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("DBConnection"),
            //options => options.CommandTimeout(999)
            options => options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null)
        );
        options.UseNpgsql(dataSource);
        options.EnableSensitiveDataLogging();
    },
    ServiceLifetime.Transient
);

// Hide non OpenAPI Conventions from Swagger.
builder.Services.AddMvc(c => c.Conventions.Add(new OpenApiConvention()));

builder.Services.AddSwaggerGen(c =>
{
    c.AddServer(new() { Url = "http://api.docker.test" });
    c.AddServer(new() { Url = "https://localhost:7249" });
    c.AddServer(new() { Url = "http://localhost:7249" });
    // Use method name as operationId
    c.CustomOperationIds(apiDesc =>
    {
        return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
    });
});

// Add MessageQueue services to the container with specified configurations.
RabbitMqOptions rabbitMqOptions = new();
builder.Configuration.GetSection(RabbitMqOptions.RabbitMq).Bind(rabbitMqOptions);
try
{
    IMqService mqService = new MqService(
        new RabbitMqAdapter(rabbitMqOptions)
    // new MockMqAdapter()
    );
    builder.Services.AddSingleton(mqService);
}
catch (Exception ex)
{
    Console.WriteLine($"RabbitMQ connection failed: {ex.GetType()} {ex.Message}");
    string jsonRabbitMqOptions = JsonConvert.SerializeObject(rabbitMqOptions);
    Console.WriteLine($"RabbitMQ Options: {jsonRabbitMqOptions}");

    // Start RabbitMQ with default configurations
    builder.Services.AddSingleton(rabbitMqOptions);
    builder.Services.AddSingleton<MqAdapter, RabbitMqAdapter>();
    builder.Services.AddSingleton<IMqService, MqService>();
}

//Pension Repositories
builder.Services.AddTransient<IFileStorageRepository, FileStorageRepository>();
builder.Services.AddTransient<IManualPpoReceiptRepository, ManualPpoReceiptRepository>();
builder.Services.AddTransient<IPensionStatusRepository, PensionStatusRepository>();
builder.Services.AddTransient<IPensionerDetailsRepository, PensionerDetailsRepository>();
builder.Services.AddTransient<IPpoIdSequenceRepository, PpoIdSequenceRepository>();
builder.Services.AddTransient<IPrimaryCategoryRepository, PrimaryCategoryRepository>();
builder.Services.AddTransient<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IBreakupRepository, BreakupRepository>();
builder.Services.AddTransient<IComponentRateRepository, ComponentRateRepository>();
builder.Services.AddTransient<IPpoComponentRevisionRepository, PpoComponentRevisionRepository>();
builder.Services.AddTransient<IPpoBillRepository, PpoBillRepository>();
builder.Services.AddTransient<IBankBranchRepository, BankBranchRepository>();
builder.Services.AddTransient<IPpoSanctionDetailsRepository, PpoSanctionDetailsRepository>();
builder.Services.AddTransient<INomineeRepository, NomineeRepository>();
builder.Services.AddTransient<ILifeCertificateRepository, LifeCertificateRepository>();
builder.Services.AddTransient<IEPpoReceiptRepository, EPpoReceiptRepository>();
builder.Services.AddTransient<ITreasuryRepository, TreasuryRepository>();
builder.Services.AddTransient<IFinancialYearRepository, FinancialYearRepository>();

// Pension Services
builder.Services.AddTransient<IFileStorageService, FileStorageService>();
builder.Services.AddTransient<IPpoReceiptService, PpoReceiptService>();
builder.Services.AddTransient<IPensionStatusService, PensionStatusService>();
builder.Services.AddTransient<IPensionerDetailsService, PensionerDetailsService>();
builder.Services.AddTransient<IPensionBillService, PensionBillService>();
builder.Services.AddTransient<IPensionCategoryService, PensionCategoryService>();
builder.Services.AddTransient<IPensionBreakupService, PensionBreakupService>();
builder.Services.AddTransient<IComponentRateService, ComponentRateService>();
builder.Services.AddTransient<IPpoComponentRevisionService, PpoComponentRevisionService>();
builder.Services.AddTransient<IPpoBillService, PpoBillService>();
builder.Services.AddTransient<IBankBranchService, BankBranchService>();
builder.Services.AddTransient<IPpoSanctionDetailsService, PpoSanctionDetailsService>();
builder.Services.AddTransient<INomineeService, NomineeService>();
builder.Services.AddTransient<ILifeCertificateService, LifeCertificateService>();
builder.Services.AddTransient<IEPpoReceiptService, EPpoReceiptService>();
builder.Services.AddTransient<IConvertToFamilyPensionService, ConvertToFamilyPensionService>();

//Automapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<ITokenHelper, TokenHelper>();

// builder.Services.AddSingleton<ITokencache, Tokencache>();

builder.Services.AddTransient<IClaimService, ClaimService>();

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // options.JsonSerializerOptions.Converters.Add(new JsonStringDateOnlyConverter("yyyy-MM-dd"));
        // options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CTS-BE", Version = "v1" });
    c.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date-only" });

    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme.",
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                Array.Empty<string>()
            },
        }
    );
});

builder.Services.AddHttpContextAccessor();
builder.Services.Configure<ApiBehaviorOptions>(config =>
{
    config.InvalidModelStateResponseFactory = ctx => new BadRequestObjectResult(
        new JsonAPIResponse<IEnumerable>()
        {
            ApiResponseStatus = APIResponseStatus.Error,
            Result = ctx.ModelState.Values,
            Message = "DTO validation error :: result field specifies error location.",
        }
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

app.UseAuthTokenMiddleware();
app.UseFinancialYear();

app.MapControllers();

app.Run();

public partial class Program { }
