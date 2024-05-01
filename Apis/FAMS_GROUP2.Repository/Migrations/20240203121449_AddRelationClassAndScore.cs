using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAMS_GROUP2.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationClassAndScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Account");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Score",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Score_ClassId",
                table: "Score",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "Score_Class_ClassId_fk",
                table: "Score",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Score_Class_ClassId_fk",
                table: "Score");

            migrationBuilder.DropIndex(
                name: "IX_Score_ClassId",
                table: "Score");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Score");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Account",
                type: "datetime",
                nullable: true);
        }
    }
}
