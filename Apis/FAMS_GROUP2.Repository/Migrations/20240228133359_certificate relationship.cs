using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAMS_GROUP2.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class certificaterelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Certificate");

            migrationBuilder.RenameColumn(
                name: "CertificatedCode",
                table: "Certificate",
                newName: "CertificatedType");

            migrationBuilder.AddColumn<string>(
                name: "CertificateCode",
                table: "StudentCertificate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProviderDate",
                table: "StudentCertificate",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateCode",
                table: "StudentCertificate");

            migrationBuilder.DropColumn(
                name: "ProviderDate",
                table: "StudentCertificate");

            migrationBuilder.RenameColumn(
                name: "CertificatedType",
                table: "Certificate",
                newName: "CertificatedCode");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Certificate",
                type: "int",
                nullable: true);
        }
    }
}
