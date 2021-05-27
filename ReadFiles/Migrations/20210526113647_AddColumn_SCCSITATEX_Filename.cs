using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadFiles.Migrations
{
    public partial class AddColumn_SCCSITATEX_Filename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleChangeMessages");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "SITATEX_FILES",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SCMessages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SCC_SITATEXID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCMessages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SCMessages_SITATEX_FILES_SCC_SITATEXID",
                        column: x => x.SCC_SITATEXID,
                        principalTable: "SITATEX_FILES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SITATEX_FILES_FileName",
                table: "SITATEX_FILES",
                column: "FileName",
                unique: true,
                filter: "[FileName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SCMessages_SCC_SITATEXID",
                table: "SCMessages",
                column: "SCC_SITATEXID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SCMessages");

            migrationBuilder.DropIndex(
                name: "IX_SITATEX_FILES_FileName",
                table: "SITATEX_FILES");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "SITATEX_FILES");

            migrationBuilder.CreateTable(
                name: "ScheduleChangeMessages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SCC_SITATEXID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleChangeMessages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ScheduleChangeMessages_SITATEX_FILES_SCC_SITATEXID",
                        column: x => x.SCC_SITATEXID,
                        principalTable: "SITATEX_FILES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleChangeMessages_SCC_SITATEXID",
                table: "ScheduleChangeMessages",
                column: "SCC_SITATEXID");
        }
    }
}
