using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymApplication.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedEmailAndPhoneCC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "EmailCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "EmailCheck",
                table: "Members");

            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck",
                table: "Members");

            migrationBuilder.AddCheckConstraint(
                name: "EmailCheck1",
                table: "Trainers",
                sql: "Email LIKE '%@%.%'");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers",
                sql: "Phone LIKE '010________'");

            migrationBuilder.AddCheckConstraint(
                name: "EmailCheck",
                table: "Members",
                sql: "Email LIKE '%@%.%'");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck",
                table: "Members",
                sql: "Phone LIKE '010________'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "EmailCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "EmailCheck",
                table: "Members");

            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck",
                table: "Members");

            migrationBuilder.AddCheckConstraint(
                name: "EmailCheck1",
                table: "Trainers",
                sql: "Email LIKE '__@___'");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers",
                sql: "Phone like '010@'");

            migrationBuilder.AddCheckConstraint(
                name: "EmailCheck",
                table: "Members",
                sql: "Email LIKE '__@___'");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck",
                table: "Members",
                sql: "Phone like '010@'");
        }
    }
}
