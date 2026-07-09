using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymApplication.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedEmailConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "EmailCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "EmailCheck",
                table: "Members");

            migrationBuilder.AddCheckConstraint(
                name: "EmailCheck1",
                table: "Trainers",
                sql: "Email LIKE '__@___'");

            migrationBuilder.AddCheckConstraint(
                name: "EmailCheck",
                table: "Members",
                sql: "Email LIKE '__@___'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "EmailCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "EmailCheck",
                table: "Members");

            migrationBuilder.AddCheckConstraint(
                name: "EmailCheck1",
                table: "Trainers",
                sql: "Email like '--@---'");

            migrationBuilder.AddCheckConstraint(
                name: "EmailCheck",
                table: "Members",
                sql: "Email like '--@---'");
        }
    }
}
