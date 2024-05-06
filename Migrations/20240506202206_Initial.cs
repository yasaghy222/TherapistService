using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TherapistService.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitudes = table.Column<float>(type: "real", nullable: false),
                    Longitudes = table.Column<float>(type: "real", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clinics_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Therapists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneValidate = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailValidate = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<byte>(type: "tinyint", nullable: false),
                    MedicalSysCode = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialtyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rate = table.Column<float>(type: "real", nullable: false),
                    RaterCount = table.Column<int>(type: "int", nullable: false),
                    SuccessConsolationCount = table.Column<int>(type: "int", nullable: false),
                    SuccessReservationCount = table.Column<int>(type: "int", nullable: false),
                    HasTelCounseling = table.Column<bool>(type: "bit", nullable: false),
                    HasTextCounseling = table.Column<bool>(type: "bit", nullable: false),
                    AcceptVisit = table.Column<bool>(type: "bit", nullable: false),
                    ClinicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LineStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Therapists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Therapists_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Therapists_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnlinePlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    DayOfWeek = table.Column<byte>(type: "tinyint", nullable: false),
                    TherapistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlinePlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlinePlans_Therapists_TherapistId",
                        column: x => x.TherapistId,
                        principalTable: "Therapists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    TherapistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitPlans_Therapists_TherapistId",
                        column: x => x.TherapistId,
                        principalTable: "Therapists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_LocationId",
                table: "Clinics",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlinePlans_TherapistId",
                table: "OnlinePlans",
                column: "TherapistId");

            migrationBuilder.CreateIndex(
                name: "IX_Therapists_ClinicId",
                table: "Therapists",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Therapists_SpecialtyId",
                table: "Therapists",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitPlans_TherapistId",
                table: "VisitPlans",
                column: "TherapistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnlinePlans");

            migrationBuilder.DropTable(
                name: "VisitPlans");

            migrationBuilder.DropTable(
                name: "Therapists");

            migrationBuilder.DropTable(
                name: "Clinics");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
