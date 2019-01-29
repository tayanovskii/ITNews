using Microsoft.EntityFrameworkCore.Migrations;

namespace ITNews.Migrations
{
    public partial class addFieldModifiedByToNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "News",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "News");
        }
    }
}
