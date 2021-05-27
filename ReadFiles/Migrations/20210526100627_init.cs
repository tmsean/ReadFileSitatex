using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadFiles.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Destionations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SITATEX_FILES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destinations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageEnd = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SITATEX_FILES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleChangeMessages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SCC_SITATEXID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleChangeMessages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ScheduleChangeMessages_SITATEX_FILES_SCC_SITATEXID",
                        column: x => x.SCC_SITATEXID,
                        principalTable: "SITATEX_FILES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleChangeMessages_SCC_SITATEXID",
                table: "ScheduleChangeMessages",
                column: "SCC_SITATEXID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "ScheduleChangeMessages");

            migrationBuilder.DropTable(
                name: "SITATEX_FILES");
        }
    }
}
