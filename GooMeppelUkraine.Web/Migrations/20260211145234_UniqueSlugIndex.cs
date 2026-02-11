using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GooMeppelUkraine.Web.Migrations
{
    /// <inheritdoc />
    public partial class UniqueSlugIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Articles_Slug_Language",
                table: "Articles",
                columns: new[] { "Slug", "Language" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_Slug_Language",
                table: "Articles");
        }
    }
}
