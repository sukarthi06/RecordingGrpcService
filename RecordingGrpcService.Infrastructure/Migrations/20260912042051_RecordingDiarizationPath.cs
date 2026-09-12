using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecordingGrpcService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RecordingDiarizationPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiarizationPath",
                schema: "recording",
                table: "RecordingSessions",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiarizationPath",
                schema: "recording",
                table: "RecordingSessions");
        }
    }
}
