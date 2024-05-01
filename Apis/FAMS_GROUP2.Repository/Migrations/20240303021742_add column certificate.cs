using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAMS_GROUP2.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class addcolumncertificate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "StudentCertificate",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "StudentCertificate");
        }
    }
}
