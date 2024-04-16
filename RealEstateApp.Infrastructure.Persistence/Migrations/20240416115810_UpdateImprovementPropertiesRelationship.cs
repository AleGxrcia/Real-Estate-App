using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Infrastructure.Persistence.Migrations
{
    public partial class UpdateImprovementPropertiesRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImprovementProperties_Improvements_ImprovementId",
                table: "ImprovementProperties");

            migrationBuilder.AddForeignKey(
                name: "FK_ImprovementProperties_Improvements_ImprovementId",
                table: "ImprovementProperties",
                column: "ImprovementId",
                principalTable: "Improvements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImprovementProperties_Improvements_ImprovementId",
                table: "ImprovementProperties");

            migrationBuilder.AddForeignKey(
                name: "FK_ImprovementProperties_Improvements_ImprovementId",
                table: "ImprovementProperties",
                column: "ImprovementId",
                principalTable: "Improvements",
                principalColumn: "Id");
        }
    }
}
