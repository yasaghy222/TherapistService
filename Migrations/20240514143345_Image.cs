using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TherapistService.Migrations
{
    /// <inheritdoc />
    public partial class Image : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MedicalSysCode",
                table: "Therapists",
                newName: "CounselingSysCode");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Therapists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Specialties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Specialties");

            migrationBuilder.RenameColumn(
                name: "CounselingSysCode",
                table: "Therapists",
                newName: "MedicalSysCode");
        }
    }
}
