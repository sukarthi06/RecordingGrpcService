using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecordingGrpcService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChunkStartAndEndTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Duration",
                schema: "recording",
                table: "RecordingChunks",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                schema: "recording",
                table: "RecordingChunks",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                schema: "recording",
                table: "RecordingChunks",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                schema: "recording",
                table: "RecordingChunks");

            migrationBuilder.DropColumn(
                name: "EndTime",
                schema: "recording",
                table: "RecordingChunks");

            migrationBuilder.DropColumn(
                name: "StartTime",
                schema: "recording",
                table: "RecordingChunks");
        }
    }
}
