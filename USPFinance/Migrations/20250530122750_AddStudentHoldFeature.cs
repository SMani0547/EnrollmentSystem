using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace USPFinance.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentHoldFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "HoldEndDate",
                table: "StudentFinances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoldPlacedBy",
                table: "StudentFinances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HoldReason",
                table: "StudentFinances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "HoldStartDate",
                table: "StudentFinances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnHold",
                table: "StudentFinances",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoldEndDate",
                table: "StudentFinances");

            migrationBuilder.DropColumn(
                name: "HoldPlacedBy",
                table: "StudentFinances");

            migrationBuilder.DropColumn(
                name: "HoldReason",
                table: "StudentFinances");

            migrationBuilder.DropColumn(
                name: "HoldStartDate",
                table: "StudentFinances");

            migrationBuilder.DropColumn(
                name: "IsOnHold",
                table: "StudentFinances");
        }
    }
}
