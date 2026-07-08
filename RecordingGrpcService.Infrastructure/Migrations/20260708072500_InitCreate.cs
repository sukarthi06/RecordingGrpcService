using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecordingGrpcService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "recording");

            migrationBuilder.CreateTable(
                name: "RecordingSessions",
                schema: "recording",
                columns: table => new
                {
                    RecordingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StoppedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StoragePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    WavPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SampleRate = table.Column<int>(type: "integer", nullable: true),
                    ChannelCount = table.Column<int>(type: "integer", nullable: true),
                    BitsPerSample = table.Column<int>(type: "integer", nullable: true),
                    MimeType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    RecordStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordingSessions", x => x.RecordingId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecordingSessions",
                schema: "recording");
        }
    }
}
