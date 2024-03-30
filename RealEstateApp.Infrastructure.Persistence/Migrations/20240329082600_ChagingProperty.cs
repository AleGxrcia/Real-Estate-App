using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Infrastructure.Persistence.Migrations
{
    public partial class ChagingProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "PropertyImages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PropertyImages");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "PropertyImages");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "PropertyImages");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PropertyImages",
                newName: "id");

            migrationBuilder.AddColumn<string>(
                name: "AgentId",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "PropertyImages",
                newName: "Id");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "PropertyImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PropertyImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "PropertyImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "PropertyImages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
