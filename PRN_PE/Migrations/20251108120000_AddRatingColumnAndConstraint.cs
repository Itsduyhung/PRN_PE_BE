using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN_PE.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingColumnAndConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Rating column if it doesn't exist
            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "posts",
                type: "numeric(3,2)",
                nullable: true);

            // Add check constraint for rating range (1.00 - 5.00)
            migrationBuilder.AddCheckConstraint(
                name: "CK_Posts_Rating_Range",
                table: "posts",
                sql: "(\"Rating\" IS NULL) OR (\"Rating\" >= 1.00 AND \"Rating\" <= 5.00)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop check constraint first
            migrationBuilder.DropCheckConstraint(
                name: "CK_Posts_Rating_Range",
                table: "posts");

            // Drop Rating column
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "posts");
        }
    }
}

