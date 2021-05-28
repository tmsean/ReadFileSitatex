using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadFiles.Migrations
{
    public partial class ChangeTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SCMessages");

            migrationBuilder.DropTable(
                name: "SITATEX_FILES");

            migrationBuilder.CreateTable(
                name: "SC_SITATEXes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destinations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SMI = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageEnd = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SC_SITATEXes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SCSubMessages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SC_SITATEXID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCSubMessages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SCSubMessages_SC_SITATEXes_SC_SITATEXID",
                        column: x => x.SC_SITATEXID,
                        principalTable: "SC_SITATEXes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SC_SITATEXes_FileName",
                table: "SC_SITATEXes",
                column: "FileName",
                unique: true,
                filter: "[FileName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SCSubMessages_SC_SITATEXID",
                table: "SCSubMessages",
                column: "SC_SITATEXID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SCSubMessages");

            migrationBuilder.DropTable(
                name: "SC_SITATEXes");

            migrationBuilder.CreateTable(
                name: "SITATEX_FILES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Destinations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageEnd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SMI = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SITATEX_FILES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SCMessages",
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
                    table.PrimaryKey("PK_SCMessages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SCMessages_SITATEX_FILES_SCC_SITATEXID",
                        column: x => x.SCC_SITATEXID,
                        principalTable: "SITATEX_FILES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SCMessages_SCC_SITATEXID",
                table: "SCMessages",
                column: "SCC_SITATEXID");

            migrationBuilder.CreateIndex(
                name: "IX_SITATEX_FILES_FileName",
                table: "SITATEX_FILES",
                column: "FileName",
                unique: true,
                filter: "[FileName] IS NOT NULL");
        }
    }
}
