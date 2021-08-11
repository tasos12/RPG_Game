using Microsoft.EntityFrameworkCore.Migrations;

namespace _NET_Course.Migrations
{
    public partial class SkillSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "ID", "Damage", "Name" },
                values: new object[] { 1, 30, "Fireball" });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "ID", "Damage", "Name" },
                values: new object[] { 2, 30, "Icicle" });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "ID", "Damage", "Name" },
                values: new object[] { 3, 30, "Breeze" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "ID",
                keyValue: 3);
        }
    }
}
