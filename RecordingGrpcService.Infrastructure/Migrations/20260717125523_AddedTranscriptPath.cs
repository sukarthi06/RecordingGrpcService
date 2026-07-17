using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecordingGrpcService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedTranscriptPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TranscriptPath",
                schema: "recording",
                table: "RecordingChunks",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TranscriptPath",
                schema: "recording",
                table: "RecordingChunks");
        }
    }
}
