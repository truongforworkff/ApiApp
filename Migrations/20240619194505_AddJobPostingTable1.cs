using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FptJobBack.Migrations
{
    /// <inheritdoc />
    public partial class AddJobPostingTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_Users_CompanyId",
                table: "JobPostings");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "JobPostings",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_JobPostings_CompanyId",
                table: "JobPostings",
                newName: "IX_JobPostings_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_Users_UserId",
                table: "JobPostings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_Users_UserId",
                table: "JobPostings");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "JobPostings",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_JobPostings_UserId",
                table: "JobPostings",
                newName: "IX_JobPostings_CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_Users_CompanyId",
                table: "JobPostings",
                column: "CompanyId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
