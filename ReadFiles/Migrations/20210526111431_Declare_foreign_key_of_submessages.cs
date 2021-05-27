using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadFiles.Migrations
{
    public partial class Declare_foreign_key_of_submessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleChangeMessages_SITATEX_FILES_SCC_SITATEXID",
                table: "ScheduleChangeMessages");

            migrationBuilder.AlterColumn<int>(
                name: "SCC_SITATEXID",
                table: "ScheduleChangeMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleChangeMessages_SITATEX_FILES_SCC_SITATEXID",
                table: "ScheduleChangeMessages",
                column: "SCC_SITATEXID",
                principalTable: "SITATEX_FILES",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleChangeMessages_SITATEX_FILES_SCC_SITATEXID",
                table: "ScheduleChangeMessages");

            migrationBuilder.AlterColumn<int>(
                name: "SCC_SITATEXID",
                table: "ScheduleChangeMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleChangeMessages_SITATEX_FILES_SCC_SITATEXID",
                table: "ScheduleChangeMessages",
                column: "SCC_SITATEXID",
                principalTable: "SITATEX_FILES",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
