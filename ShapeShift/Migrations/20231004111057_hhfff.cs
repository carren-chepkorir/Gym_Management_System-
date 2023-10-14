using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShapeShift.Migrations
{
    /// <inheritdoc />
    public partial class hhfff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MemberName",
                table: "ShiftRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberName",
                table: "ShiftRequests");
        }
    }
}
