using System;
using System.Collections.Generic;
using FAMS_GROUP2.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace FAMS_GROUP2.Repositories;

public partial class FamsDbContext : DbContext
{
    public FamsDbContext()
    {
    }

    public FamsDbContext(DbContextOptions<FamsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<AttendanceClass> AttendanceClasses { get; set; }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassAccount> ClassAccounts { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<EmailSend> EmailSends { get; set; }

    public virtual DbSet<EmailSendStudent> EmailSendStudents { get; set; }

    public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<ProgramModule> ProgramModules { get; set; }

    public virtual DbSet<Score> Scores { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentClass> StudentClasses { get; set; }

    public virtual DbSet<TrainingProgram> TrainingPrograms { get; set; }
    public virtual DbSet<StudentCertificate> StudentCertificates {  get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=(local);uid=sa;pwd=12345;database=FAMS_DB;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");
            entity.HasKey(e => e.Id).HasName("Account_pk");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            //entity.Property(e => e.AccountId)
            //    .HasMaxLength(30)
            //    .HasDefaultValueSql("(newid())");

            entity.Property(e => e.UnsignAddress).HasMaxLength(500);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.DeletedBy).HasMaxLength(100);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.UnsignFullName).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RefreshToken).IsRequired(false);
            entity.Property(e => e.RefreshTokenExpiryTime).HasColumnType("datetime").IsRequired(false);
            entity.Property(e => e.Image).HasMaxLength(500);


            entity.HasOne(e => e.Role).WithMany(e => e.Account)
                .HasForeignKey(e => e.RoleId).HasConstraintName("Account_Role_fk")
                .IsRequired(false);

        });

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Assignment_pk");

            entity.ToTable("Assignment");

            entity.Property(e => e.AssignmentName).HasMaxLength(100);
            entity.Property(e => e.AssignmentType).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedBy).HasMaxLength(100);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Module).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("Assignment_Module_ModuleId_fk");
        });

        modelBuilder.Entity<AttendanceClass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AttendanceClass_pk");

            entity.ToTable("AttendanceClass");

            entity.Property(e => e.Comment).HasMaxLength(100);
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.StudentClass).WithMany(p => p.AttendanceClasses)
                .HasForeignKey(d => d.StudentClassId)
                .HasConstraintName("AttendanceClass_StudentClass_StudentClassId_fk");
        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Certificate_pk");

            entity.ToTable("Certificate");

            entity.Property(e => e.CertificateType).HasMaxLength(20);
            entity.Property(e => e.CertificateName).HasMaxLength(150);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedBy).HasMaxLength(100);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Class_pk");

            entity.ToTable("Class");

            entity.Property(e => e.ClassName).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedBy).HasMaxLength(100);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Location).HasMaxLength(250);
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Program).WithMany(p => p.Classes)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("Class_TrainingProgram_ProgramId_fk");
        });

        modelBuilder.Entity<ClassAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ClassAccount_pk");

            entity.ToTable("ClassAccount");

            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Admin).WithMany(p => p.ClassAccountAdmins)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("ClassAccount_Account_AccountId_fk");

            entity.HasOne(d => d.Class).WithMany(p => p.ClassAccounts)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("ClassAccount_Class_ClassId_fk");

            entity.HasOne(d => d.Trainer).WithMany(p => p.ClassAccountTrainers)
                .HasForeignKey(d => d.TrainerId)
                .HasConstraintName("ClassAccount_Account_AccountId_fk_2");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Document_pk");

            entity.ToTable("Document");

            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedBy).HasMaxLength(100);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.DocumentLink).HasMaxLength(250);
            entity.Property(e => e.DocumentName).HasMaxLength(150);
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Documents)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Document_Lesson_LessonId_fk");
        });

        modelBuilder.Entity<EmailSend>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EmailSend_pk");

            entity.ToTable("EmailSend");
            entity.Property(e => e.Subject).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedBy).HasMaxLength(100);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.SendDate).HasColumnType("datetime");

            entity.HasOne(d => d.Sender).WithMany(p => p.EmailSends)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("EmailSend_Account_AccountId_fk");

            entity.HasOne(d => d.Template).WithMany(p => p.EmailSends)
                .HasForeignKey(d => d.TemplateId)
                .HasConstraintName("EmailSend_EmailTemplate_TemplateId_fk");
        });

        modelBuilder.Entity<EmailSendStudent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EmailSendStudent_pk");

            entity.ToTable("EmailSendStudent");

            entity.HasOne(d => d.EmailSend).WithMany(p => p.EmailSendStudents)
                .HasForeignKey(d => d.EmailSendId)
                .HasConstraintName("EmailSendStudent_EmailSend_EmailSendId_fk");

            entity.HasOne(d => d.Receive).WithMany(p => p.EmailSendStudents)
                .HasForeignKey(d => d.ReceiveId)
                .HasConstraintName("EmailSendStudent_Student_StudentId_fk");
        });

        modelBuilder.Entity<EmailTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EmailTemplate_pk");

            entity.ToTable("EmailTemplate");

            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedBy).HasMaxLength(100);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Lesson_pk");

            entity.ToTable("Lesson");

            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedBy).HasMaxLength(100);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.LessonCode).HasMaxLength(25);
            entity.Property(e => e.LessonName).HasMaxLength(100);
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Module).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("Lesson_Module_ModuleId_fk");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Module_pk");

            entity.ToTable("Module");

            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedBy).HasMaxLength(100);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.ModuleCode).HasMaxLength(25);
            entity.Property(e => e.ModuleName).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(20);
        });

        modelBuilder.Entity<ProgramModule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ProgramModule_pk");

            entity.ToTable("ProgramModule");

            entity.HasOne(d => d.Module).WithMany(p => p.ProgramModules)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("ProgramModule_Module_ModuleId_fk");

            entity.HasOne(d => d.Program).WithMany(p => p.ProgramModules)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("ProgramModule_TrainingProgram_ProgramId_fk");
        });

        modelBuilder.Entity<Score>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Score_pk");

            entity.ToTable("Score");

            entity.Property(e => e.AsmAvg).HasComment("Avarage of Assignment");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Gpamodule).HasColumnName("GPAModule");
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.QuizAvg).HasComment("Average off Q1 - Q6");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Student).WithMany(p => p.Scores)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("Score_Student_StudentId_fk");

            entity.HasOne(d => d.Class).WithMany(p => p.Scores)
                .HasForeignKey(d => d.ClassId).HasConstraintName("Score_Class_ClassId_fk");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Student_pk");

            entity.ToTable("Student");

            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedBy).HasMaxLength(100);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EnrollmentArea).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gpa).HasColumnName("GPA");
            entity.Property(e => e.Major).HasMaxLength(100);
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Recruiter)
                .HasMaxLength(50)
                .HasColumnName("RECruiter");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.StudentCode).HasMaxLength(20);
            entity.Property(e => e.University).HasMaxLength(100);
            entity.Property(e => e.YearOfGraduation).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Skill).HasMaxLength(30);
            entity.Property(e => e.RefreshToken).IsRequired(false);
            entity.Property(e => e.RefreshTokenExpiryTime).HasColumnType("datetime").IsRequired(false);
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.UnsignFullName).HasMaxLength(100);
        });

        modelBuilder.Entity<StudentCertificate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StudentCertificate_pk");

            entity.ToTable("StudentCertificate");

            entity.HasOne(c => c.Certificate).WithMany(p => p.StudentCertificates)
                .HasForeignKey(c => c.CertificateId)
                .HasConstraintName("StudentCertificate_Certificate_CertificateId_fk");

            entity.HasOne(s => s.Student).WithMany(p => p.StudentCertificates)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("StudentCertificate_Student_StudentId_fk");
        });

        modelBuilder.Entity<StudentClass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StudentClass_pk");

            entity.ToTable("StudentClass");

            entity.HasOne(d => d.Class).WithMany(p => p.StudentClasses)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("StudentClass_Class_ClassId_fk");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentClasses)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("StudentClass_Student_StudentId_fk");
        });

        modelBuilder.Entity<TrainingProgram>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TrainingProgram_pk");

            entity.ToTable("TrainingProgram");

            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedBy).HasMaxLength(100);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.Duration).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.ProgramCode).HasMaxLength(50);
            entity.Property(e => e.ProgramName).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(20);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Role_pk");
            entity.ToTable("Role");
            entity.Property(e => e.RoleName).HasMaxLength(20);

        });

        //OnModelCreatingPartial(modelBuilder);

        }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
