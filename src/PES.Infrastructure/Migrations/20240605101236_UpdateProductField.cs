using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PES.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductRatings_ProductId",
                table: "ProductRatings");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRatings_ProductId",
                table: "ProductRatings",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductRatings_ProductId",
                table: "ProductRatings");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRatings_ProductId",
                table: "ProductRatings",
                column: "ProductId",
                unique: true);
        }
    }
}
