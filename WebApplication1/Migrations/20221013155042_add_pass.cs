using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class add_pass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "パスワード１",
                table: "TestTable33",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "パスワード２",
                table: "TestTable33",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "パスワード３",
                table: "TestTable33",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "パスワード１",
                table: "TestTable33");

            migrationBuilder.DropColumn(
                name: "パスワード２",
                table: "TestTable33");

            migrationBuilder.DropColumn(
                name: "パスワード３",
                table: "TestTable33");
        }
    }
}
