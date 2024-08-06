using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("GraduationDatabase")));
builder.Services.AddCors();
builder.Services.AddTransient<IFarmerServices, FarmerServices>();
builder.Services.AddTransient<IProductOrderServices, ProductOrderServices>();
builder.Services.AddTransient<ILandOrderServices, LandOrderServices>();
builder.Services.AddTransient<IFarmerAccountServices, FarmerAccountServices>();
builder.Services.AddTransient<IBankServices, BankServices>();
builder.Services.AddTransient<IRepresintorServices, RepresintorServices>();
builder.Services.AddTransient<ICompanyAccountServices, CompanyAccountServices>();
builder.Services.AddTransient<ICompanyServices, CompanyServices>();
builder.Services.AddTransient<IEngineerCompanyServices, EngineerCompanyServices>();
builder.Services.AddTransient<IEngineerAccountServices, EngineerAccountServices>();
builder.Services.AddTransient<IEngineerFarmerServices, EngineerFarmerServices>();
builder.Services.AddTransient<IEngineerServices, EngineerServices>();
builder.Services.AddTransient<IProductServices, ProductServices>();
builder.Services.AddTransient<ILandServices, LandServices>();
builder.Services.AddTransient<IEquipmentServices, EquipmentServices>();
builder.Services.AddTransient<IFarmerEquipmentServices, FarmerEquipmentServices>();
builder.Services.AddTransient<ILogingUserServices, LogingUserServices>();
builder.Services.AddTransient<IFileServices, FileServices>();
builder.Services.AddTransient<IFarmerLandOrderServices, FarmerLandOrderServices>();
builder.Services.AddTransient<IFarmerProductOrderServices, FarmerProductOrderServices>();
builder.Services.AddTransient<IBuyerFarmerServices, BuyerFarmerServices>();

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
