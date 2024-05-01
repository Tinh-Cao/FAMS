using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAMS_GROUP2.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class fixcertificaterelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProviderDate",
                table: "StudentCertificate",
                newName: "ProvidedDate");

            migrationBuilder.RenameColumn(
                name: "CertificatedType",
                table: "Certificate",
                newName: "CertificateType");

            migrationBuilder.RenameColumn(
                name: "CertificatedName",
                table: "Certificate",
                newName: "CertificateName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProvidedDate",
                table: "StudentCertificate",
                newName: "ProviderDate");

            migrationBuilder.RenameColumn(
                name: "CertificateType",
                table: "Certificate",
                newName: "CertificatedType");

            migrationBuilder.RenameColumn(
                name: "CertificateName",
                table: "Certificate",
                newName: "CertificatedName");
        }
    }
}
