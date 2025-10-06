using BE_SWP391.Data;
using BE_SWP391.Repositories.Implementations;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Implementations;
using BE_SWP391.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BE_SWP391 API",
        Version = "v1",
        Description = "API documentation for EV Market Project"
    });
});
builder.Services.AddDbContext<EvMarketContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EvMarketContext")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategorySevice>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<IDataPackageRepository, DataPackageRepository>();
builder.Services.AddScoped<IDataPackageService, DataPackageService>();
builder.Services.AddScoped<IMetaDataRepository, MetaDataRepository>();
builder.Services.AddScoped<IMetaDataService, MetaDataService>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();    
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IRevenueShareRepository, RevenueShareRepository>();
builder.Services.AddScoped<IRevenueShareService, RevenueShareService>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IPricingPlanRepository, PricingPlanRepository>();
builder.Services.AddScoped<IPricingPlanService, PricingPlanService>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IDownloadRepository, DownloadRepository>();
builder.Services.AddScoped<IDownloadService, DownloadService>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
