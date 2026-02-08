using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GooMeppelUkraine.Web.Migrations
{
    /// <inheritdoc />
    public partial class ArticleSeoSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "Articles",
                type: "nvarchar(160)",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle",
                table: "Articles",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "MetaTitle",
                table: "Articles");
        }
    }
}
