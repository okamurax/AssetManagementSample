using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class ini : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestTable33",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false),
                    管理番号 = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    関連番号 = table.Column<int>(type: "INTEGER", unicode: false, maxLength: 50, nullable: false),
                    導入年月 = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    担当者１ = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    担当者２ = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    一般名称 = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    モデル名 = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    メーカー = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    設置保存場所 = table.Column<string>(name: "設置/保存場所", type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    所有形態 = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    IDユーザー名 = table.Column<string>(name: "ID/ユーザー名", type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    メール = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    パスワード = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: true),
                    備考 = table.Column<string>(type: "TEXT", unicode: false, maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestTable33", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestTable33");
        }
    }
}
