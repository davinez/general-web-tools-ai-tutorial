using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookmarkIsPersonalField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPersonalToolbarFolder",
                schema: "coreapp",
                table: "BookmarkFolders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPersonalToolbarFolder",
                schema: "coreapp",
                table: "BookmarkFolders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
