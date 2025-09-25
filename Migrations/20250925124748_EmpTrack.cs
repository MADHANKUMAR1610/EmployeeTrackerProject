using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeTracker.Migrations
{
    /// <inheritdoc />
    public partial class EmpTrack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "profile_pictureUrl",
                table: "Employees",
                newName: "Profile_pictureUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Profile_pictureUrl",
                table: "Employees",
                newName: "profile_pictureUrl");
        }
    }
}
