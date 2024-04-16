using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Infrastructure.Persistence.Migrations
{
    public partial class UpdateRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProperties_Properties_PropertyId",
                table: "FavoriteProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_ImprovementProperties_Properties_PropertyId",
                table: "ImprovementProperties");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProperties_Properties_PropertyId",
                table: "FavoriteProperties",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImprovementProperties_Properties_PropertyId",
                table: "ImprovementProperties",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProperties_Properties_PropertyId",
                table: "FavoriteProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_ImprovementProperties_Properties_PropertyId",
                table: "ImprovementProperties");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProperties_Properties_PropertyId",
                table: "FavoriteProperties",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImprovementProperties_Properties_PropertyId",
                table: "ImprovementProperties",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id");
        }
    }
}
