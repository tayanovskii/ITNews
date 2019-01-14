using Microsoft.EntityFrameworkCore.Migrations;

namespace ITNews.Migrations
{
    public partial class SeedingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Categories (Name) VALUES ('C#')");
            migrationBuilder.Sql("INSERT INTO Categories (Name) VALUES ('JavaScript')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categories WHERE Name IN ('C#', 'JavaScript')");
        }
    }
}
