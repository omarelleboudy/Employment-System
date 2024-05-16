using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class updaterelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VacancyApplication_ApplicantId",
                table: "VacancyApplication");

            migrationBuilder.DropIndex(
                name: "IX_Vacancy_EmployerId",
                table: "Vacancy");

            migrationBuilder.CreateIndex(
                name: "IX_VacancyApplication_ApplicantId",
                table: "VacancyApplication",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancy_EmployerId",
                table: "Vacancy",
                column: "EmployerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VacancyApplication_ApplicantId",
                table: "VacancyApplication");

            migrationBuilder.DropIndex(
                name: "IX_Vacancy_EmployerId",
                table: "Vacancy");

            migrationBuilder.CreateIndex(
                name: "IX_VacancyApplication_ApplicantId",
                table: "VacancyApplication",
                column: "ApplicantId",
                unique: true,
                filter: "[ApplicantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancy_EmployerId",
                table: "Vacancy",
                column: "EmployerId",
                unique: true,
                filter: "[EmployerId] IS NOT NULL");
        }
    }
}
