using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AddDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsPersonalToolbarFolder = table.Column<bool>(type: "bit", nullable: false),
                    ParentFolderId = table.Column<int>(type: "int", nullable: true),
                    createdAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
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
                name: "Persons",
                schema: "coreapp",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Salt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "coreapp",
                columns: table => new
                {
                    TagId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "Bookmarks",
                schema: "coreapp",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    AddDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    BookmarkFolderId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Articles",
                schema: "coreapp",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorPersonId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ArticleId);
                    table.ForeignKey(
                        name: "FK_Articles_Persons_AuthorPersonId",
                        column: x => x.AuthorPersonId,
                        principalSchema: "coreapp",
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateTable(
                name: "FollowedPeople",
                schema: "coreapp",
                columns: table => new
                {
                    ObserverId = table.Column<int>(type: "int", nullable: false),
                    TargetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowedPeople", x => new { x.ObserverId, x.TargetId });
                    table.ForeignKey(
                        name: "FK_FollowedPeople_Persons_ObserverId",
                        column: x => x.ObserverId,
                        principalSchema: "coreapp",
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowedPeople_Persons_TargetId",
                        column: x => x.TargetId,
                        principalSchema: "coreapp",
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleFavorites",
                schema: "coreapp",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleFavorites", x => new { x.ArticleId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_ArticleFavorites_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "coreapp",
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleFavorites_Persons_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "coreapp",
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTags",
                schema: "coreapp",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTags", x => new { x.ArticleId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ArticleTags_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "coreapp",
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTags_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "coreapp",
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                schema: "coreapp",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "coreapp",
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Persons_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "coreapp",
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleFavorites_PersonId",
                schema: "coreapp",
                table: "ArticleFavorites",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorPersonId",
                schema: "coreapp",
                table: "Articles",
                column: "AuthorPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTags_TagId",
                schema: "coreapp",
                table: "ArticleTags",
                column: "TagId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId",
                schema: "coreapp",
                table: "Comments",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                schema: "coreapp",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowedPeople_TargetId",
                schema: "coreapp",
                table: "FollowedPeople",
                column: "TargetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleFavorites",
                schema: "coreapp");

            migrationBuilder.DropTable(
                name: "ArticleTags",
                schema: "coreapp");

            migrationBuilder.DropTable(
                name: "Bookmarks",
                schema: "coreapp");

            migrationBuilder.DropTable(
                name: "Comments",
                schema: "coreapp");

            migrationBuilder.DropTable(
                name: "FollowedPeople",
                schema: "coreapp");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "coreapp");

            migrationBuilder.DropTable(
                name: "BookmarkFolders",
                schema: "coreapp");

            migrationBuilder.DropTable(
                name: "Articles",
                schema: "coreapp");

            migrationBuilder.DropTable(
                name: "Persons",
                schema: "coreapp");
        }
    }
}
