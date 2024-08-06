using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationApi.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    BankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankId);
                });

            migrationBuilder.CreateTable(
                name: "BuyerFarmers",
                columns: table => new
                {
                    BuyerFarmerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FarmerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarmerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarmerAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarmerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarmerPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyerFarmers", x => x.BuyerFarmerId);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Engineers",
                columns: table => new
                {
                    EngineerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EngineerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EngineerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EngineerAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EngineerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EngineerPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engineers", x => x.EngineerId);
                });

            migrationBuilder.CreateTable(
                name: "Farmers",
                columns: table => new
                {
                    FarmerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FarmerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarmerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarmerAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarmerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarmerPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farmers", x => x.FarmerId);
                });

            migrationBuilder.CreateTable(
                name: "LogingUsers",
                columns: table => new
                {
                    LogingUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRole = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogingUsers", x => x.LogingUserId);
                });

            migrationBuilder.CreateTable(
                name: "CompanyAccounts",
                columns: table => new
                {
                    CompanyAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountBalance = table.Column<double>(type: "float", nullable: false),
                    CvvNumber = table.Column<int>(type: "int", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAccounts", x => x.CompanyAccountId);
                    table.ForeignKey(
                        name: "FK_CompanyAccounts_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "BankId");
                    table.ForeignKey(
                        name: "FK_CompanyAccounts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                });

            migrationBuilder.CreateTable(
                name: "Represintors",
                columns: table => new
                {
                    RepresintorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepresintorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepresintorPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepresintorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Represintors", x => x.RepresintorId);
                    table.ForeignKey(
                        name: "FK_Represintors_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                });

            migrationBuilder.CreateTable(
                name: "EngineerAccounts",
                columns: table => new
                {
                    EngineerAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountBalance = table.Column<double>(type: "float", nullable: false),
                    CvvNumber = table.Column<int>(type: "int", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    EngineerId = table.Column<int>(type: "int", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineerAccounts", x => x.EngineerAccountId);
                    table.ForeignKey(
                        name: "FK_EngineerAccounts_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "BankId");
                    table.ForeignKey(
                        name: "FK_EngineerAccounts_Engineers_EngineerId",
                        column: x => x.EngineerId,
                        principalTable: "Engineers",
                        principalColumn: "EngineerId");
                });

            migrationBuilder.CreateTable(
                name: "EngineerCompanies",
                columns: table => new
                {
                    EngineerCompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServicePrice = table.Column<double>(type: "float", nullable: false),
                    ServiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    EngineerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineerCompanies", x => x.EngineerCompanyId);
                    table.ForeignKey(
                        name: "FK_EngineerCompanies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_EngineerCompanies_Engineers_EngineerId",
                        column: x => x.EngineerId,
                        principalTable: "Engineers",
                        principalColumn: "EngineerId");
                });

            migrationBuilder.CreateTable(
                name: "EngineerFarmers",
                columns: table => new
                {
                    EngineerFarmerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServicePrice = table.Column<double>(type: "float", nullable: false),
                    ServiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FarmerId = table.Column<int>(type: "int", nullable: false),
                    EngineerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineerFarmers", x => x.EngineerFarmerId);
                    table.ForeignKey(
                        name: "FK_EngineerFarmers_Engineers_EngineerId",
                        column: x => x.EngineerId,
                        principalTable: "Engineers",
                        principalColumn: "EngineerId");
                    table.ForeignKey(
                        name: "FK_EngineerFarmers_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "FarmerId");
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EquipmentPrice = table.Column<double>(type: "float", nullable: false),
                    EquipmentDescribtion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarmerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.EquipmentId);
                    table.ForeignKey(
                        name: "FK_Equipments_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "FarmerId");
                });

            migrationBuilder.CreateTable(
                name: "FarmerAccounts",
                columns: table => new
                {
                    FarmerAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountBalance = table.Column<double>(type: "float", nullable: false),
                    CvvNumber = table.Column<int>(type: "int", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    FarmerId = table.Column<int>(type: "int", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmerAccounts", x => x.FarmerAccountId);
                    table.ForeignKey(
                        name: "FK_FarmerAccounts_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "BankId");
                    table.ForeignKey(
                        name: "FK_FarmerAccounts_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "FarmerId");
                });

            migrationBuilder.CreateTable(
                name: "FarmerProductOrders",
                columns: table => new
                {
                    FarmerProductOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderPrice = table.Column<double>(type: "float", nullable: false),
                    OrderWeight = table.Column<double>(type: "float", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductOffersStatus = table.Column<int>(type: "int", nullable: false),
                    FarmerId = table.Column<int>(type: "int", nullable: false),
                    BuyerFarmerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmerProductOrders", x => x.FarmerProductOrderId);
                    table.ForeignKey(
                        name: "FK_FarmerProductOrders_BuyerFarmers_BuyerFarmerId",
                        column: x => x.BuyerFarmerId,
                        principalTable: "BuyerFarmers",
                        principalColumn: "BuyerFarmerId");
                    table.ForeignKey(
                        name: "FK_FarmerProductOrders_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "FarmerId");
                });

            migrationBuilder.CreateTable(
                name: "Lands",
                columns: table => new
                {
                    LandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LandLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LandSize = table.Column<double>(type: "float", nullable: false),
                    LandType = table.Column<int>(type: "int", nullable: false),
                    LandDescribtion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarmerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lands", x => x.LandId);
                    table.ForeignKey(
                        name: "FK_Lands_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "FarmerId");
                });

            migrationBuilder.CreateTable(
                name: "ProductOrders",
                columns: table => new
                {
                    ProductOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderPrice = table.Column<double>(type: "float", nullable: false),
                    OrderWeight = table.Column<double>(type: "float", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductOffersStatus = table.Column<int>(type: "int", nullable: false),
                    FarmerId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOrders", x => x.ProductOrderId);
                    table.ForeignKey(
                        name: "FK_ProductOrders_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_ProductOrders_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "FarmerId");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductWeight = table.Column<double>(type: "float", nullable: false),
                    ProductPrice = table.Column<double>(type: "float", nullable: false),
                    ProductQuality = table.Column<int>(type: "int", nullable: false),
                    ProductDescribtion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarmerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "FarmerId");
                });

            migrationBuilder.CreateTable(
                name: "FarmerEquipments",
                columns: table => new
                {
                    FarmerEquipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentPrice = table.Column<double>(type: "float", nullable: false),
                    RentStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RentEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EquipmentRentStatus = table.Column<int>(type: "int", nullable: false),
                    FarmerId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    BuyerFarmerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmerEquipments", x => x.FarmerEquipmentId);
                    table.ForeignKey(
                        name: "FK_FarmerEquipments_BuyerFarmers_BuyerFarmerId",
                        column: x => x.BuyerFarmerId,
                        principalTable: "BuyerFarmers",
                        principalColumn: "BuyerFarmerId");
                    table.ForeignKey(
                        name: "FK_FarmerEquipments_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId");
                    table.ForeignKey(
                        name: "FK_FarmerEquipments_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "FarmerId");
                });

            migrationBuilder.CreateTable(
                name: "FarmerLandOrders",
                columns: table => new
                {
                    FarmerLandOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderPrice = table.Column<double>(type: "float", nullable: false),
                    LandSize = table.Column<double>(type: "float", nullable: false),
                    OrderStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LandRentStatus = table.Column<int>(type: "int", nullable: false),
                    FarmerId = table.Column<int>(type: "int", nullable: false),
                    LandId = table.Column<int>(type: "int", nullable: false),
                    BuyerFarmerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmerLandOrders", x => x.FarmerLandOrderId);
                    table.ForeignKey(
                        name: "FK_FarmerLandOrders_BuyerFarmers_BuyerFarmerId",
                        column: x => x.BuyerFarmerId,
                        principalTable: "BuyerFarmers",
                        principalColumn: "BuyerFarmerId");
                    table.ForeignKey(
                        name: "FK_FarmerLandOrders_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "FarmerId");
                    table.ForeignKey(
                        name: "FK_FarmerLandOrders_Lands_LandId",
                        column: x => x.LandId,
                        principalTable: "Lands",
                        principalColumn: "LandId");
                });

            migrationBuilder.CreateTable(
                name: "LandOrders",
                columns: table => new
                {
                    LandOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderPrice = table.Column<double>(type: "float", nullable: false),
                    LandSize = table.Column<double>(type: "float", nullable: false),
                    OrderStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LandRentStatus = table.Column<int>(type: "int", nullable: false),
                    FarmerId = table.Column<int>(type: "int", nullable: false),
                    LandId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandOrders", x => x.LandOrderId);
                    table.ForeignKey(
                        name: "FK_LandOrders_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_LandOrders_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "FarmerId");
                    table.ForeignKey(
                        name: "FK_LandOrders_Lands_LandId",
                        column: x => x.LandId,
                        principalTable: "Lands",
                        principalColumn: "LandId");
                });

            migrationBuilder.CreateTable(
                name: "FileInformations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransformedFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarmerId = table.Column<int>(type: "int", nullable: true),
                    BuyerFarmerId = table.Column<int>(type: "int", nullable: true),
                    EngineerId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    RepresintorId = table.Column<int>(type: "int", nullable: true),
                    LandId = table.Column<int>(type: "int", nullable: true),
                    EquipmentId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileInformations_BuyerFarmers_BuyerFarmerId",
                        column: x => x.BuyerFarmerId,
                        principalTable: "BuyerFarmers",
                        principalColumn: "BuyerFarmerId");
                    table.ForeignKey(
                        name: "FK_FileInformations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_FileInformations_Engineers_EngineerId",
                        column: x => x.EngineerId,
                        principalTable: "Engineers",
                        principalColumn: "EngineerId");
                    table.ForeignKey(
                        name: "FK_FileInformations_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId");
                    table.ForeignKey(
                        name: "FK_FileInformations_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "FarmerId");
                    table.ForeignKey(
                        name: "FK_FileInformations_Lands_LandId",
                        column: x => x.LandId,
                        principalTable: "Lands",
                        principalColumn: "LandId");
                    table.ForeignKey(
                        name: "FK_FileInformations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK_FileInformations_Represintors_RepresintorId",
                        column: x => x.RepresintorId,
                        principalTable: "Represintors",
                        principalColumn: "RepresintorId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAccounts_BankId",
                table: "CompanyAccounts",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAccounts_CompanyId",
                table: "CompanyAccounts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineerAccounts_BankId",
                table: "EngineerAccounts",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineerAccounts_EngineerId",
                table: "EngineerAccounts",
                column: "EngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineerCompanies_CompanyId",
                table: "EngineerCompanies",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineerCompanies_EngineerId",
                table: "EngineerCompanies",
                column: "EngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineerFarmers_EngineerId",
                table: "EngineerFarmers",
                column: "EngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineerFarmers_FarmerId",
                table: "EngineerFarmers",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_FarmerId",
                table: "Equipments",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmerAccounts_BankId",
                table: "FarmerAccounts",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmerAccounts_FarmerId",
                table: "FarmerAccounts",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmerEquipments_BuyerFarmerId",
                table: "FarmerEquipments",
                column: "BuyerFarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmerEquipments_EquipmentId",
                table: "FarmerEquipments",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmerEquipments_FarmerId",
                table: "FarmerEquipments",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmerLandOrders_BuyerFarmerId",
                table: "FarmerLandOrders",
                column: "BuyerFarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmerLandOrders_FarmerId",
                table: "FarmerLandOrders",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmerLandOrders_LandId",
                table: "FarmerLandOrders",
                column: "LandId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmerProductOrders_BuyerFarmerId",
                table: "FarmerProductOrders",
                column: "BuyerFarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmerProductOrders_FarmerId",
                table: "FarmerProductOrders",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_FileInformations_BuyerFarmerId",
                table: "FileInformations",
                column: "BuyerFarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_FileInformations_CompanyId",
                table: "FileInformations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_FileInformations_EngineerId",
                table: "FileInformations",
                column: "EngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_FileInformations_EquipmentId",
                table: "FileInformations",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FileInformations_FarmerId",
                table: "FileInformations",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_FileInformations_LandId",
                table: "FileInformations",
                column: "LandId");

            migrationBuilder.CreateIndex(
                name: "IX_FileInformations_ProductId",
                table: "FileInformations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FileInformations_RepresintorId",
                table: "FileInformations",
                column: "RepresintorId");

            migrationBuilder.CreateIndex(
                name: "IX_LandOrders_CompanyId",
                table: "LandOrders",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_LandOrders_FarmerId",
                table: "LandOrders",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_LandOrders_LandId",
                table: "LandOrders",
                column: "LandId");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_FarmerId",
                table: "Lands",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrders_CompanyId",
                table: "ProductOrders",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrders_FarmerId",
                table: "ProductOrders",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_FarmerId",
                table: "Products",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_Represintors_CompanyId",
                table: "Represintors",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyAccounts");

            migrationBuilder.DropTable(
                name: "EngineerAccounts");

            migrationBuilder.DropTable(
                name: "EngineerCompanies");

            migrationBuilder.DropTable(
                name: "EngineerFarmers");

            migrationBuilder.DropTable(
                name: "FarmerAccounts");

            migrationBuilder.DropTable(
                name: "FarmerEquipments");

            migrationBuilder.DropTable(
                name: "FarmerLandOrders");

            migrationBuilder.DropTable(
                name: "FarmerProductOrders");

            migrationBuilder.DropTable(
                name: "FileInformations");

            migrationBuilder.DropTable(
                name: "LandOrders");

            migrationBuilder.DropTable(
                name: "LogingUsers");

            migrationBuilder.DropTable(
                name: "ProductOrders");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "BuyerFarmers");

            migrationBuilder.DropTable(
                name: "Engineers");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Represintors");

            migrationBuilder.DropTable(
                name: "Lands");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Farmers");
        }
    }
}
