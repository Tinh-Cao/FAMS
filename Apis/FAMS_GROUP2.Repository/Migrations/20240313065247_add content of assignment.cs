using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAMS_GROUP2.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class addcontentofassignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "StudentCertificate_Certificate_CertificateId_fk",
                table: "StudentCertificate");

            migrationBuilder.AlterColumn<int>(
                name: "CertificateId",
                table: "StudentCertificate",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Assignment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "StudentCertificate_Certificate_CertificateId_fk",
                table: "StudentCertificate",
                column: "CertificateId",
                principalTable: "Certificate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "StudentCertificate_Certificate_CertificateId_fk",
                table: "StudentCertificate");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Assignment");

            migrationBuilder.AlterColumn<int>(
                name: "CertificateId",
                table: "StudentCertificate",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "StudentCertificate_Certificate_CertificateId_fk",
                table: "StudentCertificate",
                column: "CertificateId",
                principalTable: "Certificate",
                principalColumn: "Id");
        }
    }
}
