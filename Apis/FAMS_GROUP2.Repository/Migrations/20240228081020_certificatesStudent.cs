using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAMS_GROUP2.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class certificatesStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Certificate_Student_StudentId_fk",
                table: "Certificate");

            migrationBuilder.DropIndex(
                name: "IX_Certificate_StudentId",
                table: "Certificate");

            migrationBuilder.CreateTable(
                name: "StudentCertificate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    CertificateId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("StudentCertificate_pk", x => x.Id);
                    table.ForeignKey(
                        name: "StudentCertificate_Certificate_CertificateId_fk",
                        column: x => x.CertificateId,
                        principalTable: "Certificate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "StudentCertificate_Student_StudentId_fk",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentCertificate_CertificateId",
                table: "StudentCertificate",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCertificate_StudentId",
                table: "StudentCertificate",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentCertificate");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_StudentId",
                table: "Certificate",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "Certificate_Student_StudentId_fk",
                table: "Certificate",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id");
        }
    }
}
