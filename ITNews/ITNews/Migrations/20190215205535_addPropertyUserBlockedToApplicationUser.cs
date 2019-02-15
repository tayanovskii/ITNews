using Microsoft.EntityFrameworkCore.Migrations;

namespace ITNews.Migrations
{
    public partial class addPropertyUserBlockedToApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UserBlocked",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserBlocked",
                table: "AspNetUsers");
        }
    }
}
