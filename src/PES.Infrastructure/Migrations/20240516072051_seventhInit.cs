using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PES.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seventhInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "ProductRatings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "ProductRatings");
        }
    }
}
