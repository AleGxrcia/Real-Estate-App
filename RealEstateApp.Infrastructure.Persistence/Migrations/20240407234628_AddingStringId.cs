using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Infrastructure.Persistence.Migrations
{
    public partial class AddingStringId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "FavoriteProperties",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "FavoriteProperties",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
