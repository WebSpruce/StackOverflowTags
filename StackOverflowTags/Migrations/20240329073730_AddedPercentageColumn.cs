using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverflowTags.Migrations
{
    /// <inheritdoc />
    public partial class AddedPercentageColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Percentage",
                table: "Tag",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "Tag");
        }
    }
}
