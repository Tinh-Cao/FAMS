using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAMS_GROUP2.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class initDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EmailTemplate_pk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Module",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleCode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Module_pk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
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
                    table.PrimaryKey("Role_pk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DOB = table.Column<DateTime>(type: "date", nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: true),
                    University = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Major = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Skill = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    YearOfGraduation = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    GPA = table.Column<double>(type: "float", nullable: true),
                    StudentCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EnrollmentArea = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RECruiter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Student_pk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingProgram",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProgramName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TrainingProgram_pk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: true),
                    AssignmentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    AssignmentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Assignment_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Assignment_Module_ModuleId_fk",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lesson",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: true),
                    LessonName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LessonCode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Lesson_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Lesson_Module_ModuleId_fk",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnsignFullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    DOB = table.Column<DateTime>(type: "date", nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: true),
                    UnsignAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Account_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Account_Role_fk",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Certificate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    CertificatedName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CertificatedCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Certificate_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Certificate_Student_StudentId_fk",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Score",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    Quiz1 = table.Column<double>(type: "float", nullable: true),
                    Quiz2 = table.Column<double>(type: "float", nullable: true),
                    Quiz3 = table.Column<double>(type: "float", nullable: true),
                    Quiz4 = table.Column<double>(type: "float", nullable: true),
                    Quiz5 = table.Column<double>(type: "float", nullable: true),
                    Quiz6 = table.Column<double>(type: "float", nullable: true),
                    QuizAvg = table.Column<double>(type: "float", nullable: true, comment: "Average off Q1 - Q6"),
                    QuizFinal = table.Column<double>(type: "float", nullable: true),
                    Asm1 = table.Column<double>(type: "float", nullable: true),
                    Asm2 = table.Column<double>(type: "float", nullable: true),
                    Asm3 = table.Column<double>(type: "float", nullable: true),
                    Asm4 = table.Column<double>(type: "float", nullable: true),
                    Asm5 = table.Column<double>(type: "float", nullable: true),
                    AsmAvg = table.Column<double>(type: "float", nullable: true, comment: "Avarage of Assignment"),
                    PracticeFinal = table.Column<double>(type: "float", nullable: true),
                    Audit = table.Column<double>(type: "float", nullable: true),
                    GPAModule = table.Column<double>(type: "float", nullable: true),
                    LevelModule = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Score_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Score_Student_StudentId_fk",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ProgramId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Class_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Class_TrainingProgram_ProgramId_fk",
                        column: x => x.ProgramId,
                        principalTable: "TrainingProgram",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProgramModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramId = table.Column<int>(type: "int", nullable: true),
                    ModuleId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("ProgramModule_pk", x => x.Id);
                    table.ForeignKey(
                        name: "ProgramModule_Module_ModuleId_fk",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "ProgramModule_TrainingProgram_ProgramId_fk",
                        column: x => x.ProgramId,
                        principalTable: "TrainingProgram",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DocumentLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Document_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Document_Lesson_LessonId_fk",
                        column: x => x.LessonId,
                        principalTable: "Lesson",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmailSend",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: true),
                    TemplateId = table.Column<int>(type: "int", nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EmailSend_pk", x => x.Id);
                    table.ForeignKey(
                        name: "EmailSend_Account_AccountId_fk",
                        column: x => x.SenderId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "EmailSend_EmailTemplate_TemplateId_fk",
                        column: x => x.TemplateId,
                        principalTable: "EmailTemplate",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClassAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: true),
                    AdminId = table.Column<int>(type: "int", nullable: true),
                    TrainerId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
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
                    table.PrimaryKey("ClassAccount_pk", x => x.Id);
                    table.ForeignKey(
                        name: "ClassAccount_Account_AccountId_fk",
                        column: x => x.AdminId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "ClassAccount_Account_AccountId_fk_2",
                        column: x => x.TrainerId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "ClassAccount_Class_ClassId_fk",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    ClassId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("StudentClass_pk", x => x.Id);
                    table.ForeignKey(
                        name: "StudentClass_Class_ClassId_fk",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "StudentClass_Student_StudentId_fk",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmailSendStudent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiveId = table.Column<int>(type: "int", nullable: true),
                    EmailSendId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("EmailSendStudent_pk", x => x.Id);
                    table.ForeignKey(
                        name: "EmailSendStudent_EmailSend_EmailSendId_fk",
                        column: x => x.EmailSendId,
                        principalTable: "EmailSend",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "EmailSendStudent_Student_StudentId_fk",
                        column: x => x.ReceiveId,
                        principalTable: "Student",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AttendanceClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentClassId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("AttendanceClass_pk", x => x.Id);
                    table.ForeignKey(
                        name: "AttendanceClass_StudentClass_StudentClassId_fk",
                        column: x => x.StudentClassId,
                        principalTable: "StudentClass",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_RoleId",
                table: "Account",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_ModuleId",
                table: "Assignment",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceClass_StudentClassId",
                table: "AttendanceClass",
                column: "StudentClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_StudentId",
                table: "Certificate",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Class_ProgramId",
                table: "Class",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassAccount_AdminId",
                table: "ClassAccount",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassAccount_ClassId",
                table: "ClassAccount",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassAccount_TrainerId",
                table: "ClassAccount",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_LessonId",
                table: "Document",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailSend_SenderId",
                table: "EmailSend",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailSend_TemplateId",
                table: "EmailSend",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailSendStudent_EmailSendId",
                table: "EmailSendStudent",
                column: "EmailSendId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailSendStudent_ReceiveId",
                table: "EmailSendStudent",
                column: "ReceiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_ModuleId",
                table: "Lesson",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramModule_ModuleId",
                table: "ProgramModule",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramModule_ProgramId",
                table: "ProgramModule",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Score_StudentId",
                table: "Score",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClass_ClassId",
                table: "StudentClass",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClass_StudentId",
                table: "StudentClass",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assignment");

            migrationBuilder.DropTable(
                name: "AttendanceClass");

            migrationBuilder.DropTable(
                name: "Certificate");

            migrationBuilder.DropTable(
                name: "ClassAccount");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "EmailSendStudent");

            migrationBuilder.DropTable(
                name: "ProgramModule");

            migrationBuilder.DropTable(
                name: "Score");

            migrationBuilder.DropTable(
                name: "StudentClass");

            migrationBuilder.DropTable(
                name: "Lesson");

            migrationBuilder.DropTable(
                name: "EmailSend");

            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Module");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "EmailTemplate");

            migrationBuilder.DropTable(
                name: "TrainingProgram");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
