using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChapsDotNET.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDotNetAuditsTable_SecondAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DotNetAudits",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Object = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjectPrimaryKey = table.Column<int>(type: "int", nullable: true),
                    RootPrimaryKey = table.Column<int>(type: "int", nullable: true),
                    ActionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DotNetAudits", x => x.AuditId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DotNetAudits");
        }
    }
}
