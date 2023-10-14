using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShapeShift.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainerNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrainerName",
                table: "ShiftRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrainerName",
                table: "ShiftRequests");
        }
    }
}
