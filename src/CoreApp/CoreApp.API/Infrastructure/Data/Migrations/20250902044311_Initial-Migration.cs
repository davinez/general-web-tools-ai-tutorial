using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoreApp.API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "coreapp");

            migrationBuilder.CreateTable(
                name: "BookmarkFolders",
                schema: "coreapp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AddDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ParentFolderId = table.Column<int>(type: "integer", nullable: true),
                    createdAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookmarkFolders", x => x.id);
                    table.ForeignKey(
                        name: "FK_BookmarkFolders_BookmarkFolders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalSchema: "coreapp",
                        principalTable: "BookmarkFolders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobEvents",
                schema: "coreapp",
                columns: table => new
                {
                    JobEventId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    EventTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Workflow = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobEvents", x => x.JobEventId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                schema: "coreapp",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "Bookmarks",
                schema: "coreapp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    AddDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Icon = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    BookmarkFolderId = table.Column<int>(type: "integer", nullable: false),
                    createdAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmarks", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bookmarks_BookmarkFolders_BookmarkFolderId",
                        column: x => x.BookmarkFolderId,
                        principalSchema: "coreapp",
                        principalTable: "BookmarkFolders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookmarkFolders_ParentFolderId",
                schema: "coreapp",
                table: "BookmarkFolders",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_BookmarkFolderId",
                schema: "coreapp",
                table: "Bookmarks",
                column: "BookmarkFolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookmarks",
                schema: "coreapp");

            migrationBuilder.DropTable(
                name: "JobEvents",
                schema: "coreapp");

            migrationBuilder.DropTable(
                name: "Persons",
                schema: "coreapp");

            migrationBuilder.DropTable(
                name: "BookmarkFolders",
                schema: "coreapp");
        }
    }
}
