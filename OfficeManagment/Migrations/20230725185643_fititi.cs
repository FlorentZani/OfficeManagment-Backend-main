using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeManagment.Migrations
{
    /// <inheritdoc />
    public partial class fititi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProjects_Positions_PositionId",
                table: "UserProjects");

            migrationBuilder.DropIndex(
                name: "IX_UserProjects_PositionId",
                table: "UserProjects");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "UserProjects");

            migrationBuilder.AddColumn<string>(
                name: "PositionIds",
                table: "UserProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionIds",
                table: "UserProjects");

            migrationBuilder.AddColumn<Guid>(
                name: "PositionId",
                table: "UserProjects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserProjects_PositionId",
                table: "UserProjects",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProjects_Positions_PositionId",
                table: "UserProjects",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
