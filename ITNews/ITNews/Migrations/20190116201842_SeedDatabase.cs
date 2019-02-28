using Microsoft.EntityFrameworkCore.Migrations;

namespace ITNews.Migrations
{
    public partial class SeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Categories (Name) VALUES ('C#')");
            migrationBuilder.Sql("INSERT INTO Categories (Name) VALUES ('JavaScript')");
            migrationBuilder.Sql("INSERT INTO Categories (Name) VALUES ('Linux')");
            migrationBuilder.Sql("INSERT INTO Categories (Name) VALUES ('PHP')");
            migrationBuilder.Sql("INSERT INTO Categories (Name) VALUES ('Java')");
            migrationBuilder.Sql("INSERT INTO Categories (Name) VALUES ('MySQL')");

            migrationBuilder.Sql("INSERT INTO Languages (Name) VALUES ('rus')");
            migrationBuilder.Sql("INSERT INTO Languages (Name) VALUES ('eng')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categories WHERE Name IN ('C#', 'JavaScript', 'Linux', 'PHP', 'Java', 'MySQL')");
            migrationBuilder.Sql("DELETE FROM Languages WHERE Name IN ('rus', 'eng')");
        }
    }
}
