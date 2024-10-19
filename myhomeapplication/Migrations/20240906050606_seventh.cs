using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myhomeapplication.Migrations
{
    /// <inheritdoc />
    public partial class seventh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "StaticPages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "StaticPages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaTile",
                table: "StaticPages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "StaticPages");

            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "StaticPages");

            migrationBuilder.DropColumn(
                name: "MetaTile",
                table: "StaticPages");
        }
    }
}
