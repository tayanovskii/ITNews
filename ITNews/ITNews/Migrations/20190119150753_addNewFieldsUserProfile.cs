using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITNews.Migrations
{
    public partial class addNewFieldsUserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDay",
                table: "UserProfile",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "UserProfile",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDay",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "UserProfile");
        }
    }
}
