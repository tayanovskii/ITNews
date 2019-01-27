using Microsoft.EntityFrameworkCore.Migrations;

namespace ITNews.Migrations
{
    public partial class AddMarkdownToNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MarkDown",
                table: "News",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkDown",
                table: "News");
        }
    }
}
