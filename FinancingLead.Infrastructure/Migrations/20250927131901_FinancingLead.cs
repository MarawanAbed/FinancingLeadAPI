using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancingLead.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinancingLead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancingLeads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Contact_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Contact_Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Contact_PhoneE164 = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Contact_PreferredContactMethod = table.Column<int>(type: "int", nullable: true),
                    Business_TypeOfActivity = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Business_CommercialRegisterType = table.Column<int>(type: "int", nullable: false),
                    Business_AnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewStatus = table.Column<int>(type: "int", nullable: false),
                    ReviewReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancingLeads", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancingLeads_Contact_Email",
                table: "FinancingLeads",
                column: "Contact_Email");

            migrationBuilder.CreateIndex(
                name: "IX_FinancingLeads_Contact_PhoneE164",
                table: "FinancingLeads",
                column: "Contact_PhoneE164");

            migrationBuilder.CreateIndex(
                name: "IX_FinancingLeads_CreatedAt",
                table: "FinancingLeads",
                column: "CreatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_FinancingLeads_CreatedAt_ReviewStatus",
                table: "FinancingLeads",
                columns: new[] { "CreatedAt", "ReviewStatus" },
                descending: new[] { true, false });

            migrationBuilder.CreateIndex(
                name: "IX_FinancingLeads_ReviewStatus",
                table: "FinancingLeads",
                column: "ReviewStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancingLeads");
        }
    }
}
