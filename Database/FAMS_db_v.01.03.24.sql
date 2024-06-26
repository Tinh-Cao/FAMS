CREATE DATABASE [FAMS_DB]
USE [FAMS_DB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UnsignFullName] [nvarchar](100) NULL,
	[FullName] [nvarchar](100) NULL,
	[Email] [nvarchar](250) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[DOB] [date] NULL,
	[Gender] [bit] NULL,
	[UnsignAddress] [nvarchar](500) NULL,
	[Address] [nvarchar](500) NULL,
	[RefreshToken] [nvarchar](max) NULL,
	[RefreshTokenExpiryTime] [datetime] NULL,
	[RoleId] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[IsDelete] [bit] NULL,
	[Image] [nvarchar](500) NULL,
 CONSTRAINT [Account_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Assignment]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Assignment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NULL,
	[AssignmentName] [nvarchar](100) NULL,
	[Status] [nvarchar](20) NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[AssignmentType] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [Assignment_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AttendanceClass]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttendanceClass](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentClassId] [int] NULL,
	[Date] [date] NULL,
	[Status] [nvarchar](20) NULL,
	[Comment] [nvarchar](100) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [AttendanceClass_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Certificate]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Certificate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CertificateName] [nvarchar](150) NULL,
	[CertificateType] [nvarchar](20) NULL,
	[Content] [nvarchar](max) Null,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [Certificate_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Class]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClassName] [nvarchar](100) NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[Location] [nvarchar](250) NULL,
	[ProgramId] [int] NULL,
	[Status] [nvarchar](20) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [Class_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassAccount]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassAccount](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClassId] [int] NULL,
	[AdminId] [int] NULL,
	[TrainerId] [int] NULL,
	[Status] [nvarchar](20) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [ClassAccount_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Document]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Document](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LessonId] [int] NOT NULL,
	[DocumentName] [nvarchar](150) NULL,
	[DocumentLink] [nvarchar](250) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [Document_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailSend]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailSend](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SenderId] [int] NULL,
	[TemplateId] [int] NULL,
	[SendDate] [datetime] NULL,
	[Subject] [nvarchar](100) Null,
	[Content] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [EmailSend_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailSendStudent]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailSendStudent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReceiveId] [int] NULL,
	[EmailSendId] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [EmailSendStudent_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailTemplate]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTemplate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NULL,
	[Name] [nvarchar](100) NULL,
	[Content] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [EmailTemplate_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lesson]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lesson](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NULL,
	[LessonName] [nvarchar](100) NULL,
	[LessonCode] [nvarchar](25) NULL,
	[Status] [nvarchar](20) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [Lesson_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Module]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Module](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModuleCode] [nvarchar](25) NOT NULL,
	[ModuleName] [nvarchar](100) NULL,
	[Status] [nvarchar](20) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [Module_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProgramModule]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProgramModule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProgramId] [int] NULL,
	[ModuleId] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [ProgramModule_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](20) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [Role_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Score]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Score](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NULL,
	[Quiz1] [float] NULL,
	[Quiz2] [float] NULL,
	[Quiz3] [float] NULL,
	[Quiz4] [float] NULL,
	[Quiz5] [float] NULL,
	[Quiz6] [float] NULL,
	[QuizAvg] [float] NULL,
	[QuizFinal] [float] NULL,
	[Asm1] [float] NULL,
	[Asm2] [float] NULL,
	[Asm3] [float] NULL,
	[Asm4] [float] NULL,
	[Asm5] [float] NULL,
	[AsmAvg] [float] NULL,
	[PracticeFinal] [float] NULL,
	[Audit] [float] NULL,
	[GPAModule] [float] NULL,
	[LevelModule] [int] NULL,
	[Status] [nvarchar](20) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[IsDelete] [bit] NULL,
	[ClassId] [int] NULL,
 CONSTRAINT [Score_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[DOB] [date] NULL,
	[Gender] [bit] NULL,
	[University] [nvarchar](100) NULL,
	[Major] [nvarchar](100) NULL,
	[Skill] [nvarchar](30) NULL,
	[YearOfGraduation] [decimal](18, 0) NULL,
	[GPA] [float] NULL,
	[StudentCode] [nvarchar](20) NULL,
	[EnrollmentArea] [nvarchar](100) NULL,
	[RECruiter] [nvarchar](50) NULL,
	[Status] [nvarchar](20) NULL,
	[RefreshToken] [nvarchar](max) NULL,
	[RefreshTokenExpiryTime] [datetime] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[IsDelete] [bit] NULL,
	[Image] [nvarchar](500) NULL,
	[UnsignFullName] [nvarchar](100) NULL,
 CONSTRAINT [Student_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentCertificate]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentCertificate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NULL,
	[CertificateId] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[IsDelete] [bit] NULL,
	[CertificateCode] [nvarchar](max) NULL,
	[ProvidedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [StudentCertificate_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentClass]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentClass](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NULL,
	[ClassId] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [StudentClass_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrainingProgram]    Script Date: 03/01/2024 9:48:23 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainingProgram](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProgramCode] [nvarchar](50) NOT NULL,
	[ProgramName] [nvarchar](100) NULL,
	[Duration] [nvarchar](50) NULL,
	[Status] [nvarchar](20) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [TrainingProgram_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StudentCertificate] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [ProvidedDate]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [Account_Role_fk] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [Account_Role_fk]
GO
ALTER TABLE [dbo].[Assignment]  WITH CHECK ADD  CONSTRAINT [Assignment_Module_ModuleId_fk] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([Id])
GO
ALTER TABLE [dbo].[Assignment] CHECK CONSTRAINT [Assignment_Module_ModuleId_fk]
GO
ALTER TABLE [dbo].[AttendanceClass]  WITH CHECK ADD  CONSTRAINT [AttendanceClass_StudentClass_StudentClassId_fk] FOREIGN KEY([StudentClassId])
REFERENCES [dbo].[StudentClass] ([Id])
GO
ALTER TABLE [dbo].[AttendanceClass] CHECK CONSTRAINT [AttendanceClass_StudentClass_StudentClassId_fk]
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [Class_TrainingProgram_ProgramId_fk] FOREIGN KEY([ProgramId])
REFERENCES [dbo].[TrainingProgram] ([Id])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [Class_TrainingProgram_ProgramId_fk]
GO
ALTER TABLE [dbo].[ClassAccount]  WITH CHECK ADD  CONSTRAINT [ClassAccount_Account_AccountId_fk] FOREIGN KEY([AdminId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[ClassAccount] CHECK CONSTRAINT [ClassAccount_Account_AccountId_fk]
GO
ALTER TABLE [dbo].[ClassAccount]  WITH CHECK ADD  CONSTRAINT [ClassAccount_Account_AccountId_fk_2] FOREIGN KEY([TrainerId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[ClassAccount] CHECK CONSTRAINT [ClassAccount_Account_AccountId_fk_2]
GO
ALTER TABLE [dbo].[ClassAccount]  WITH CHECK ADD  CONSTRAINT [ClassAccount_Class_ClassId_fk] FOREIGN KEY([ClassId])
REFERENCES [dbo].[Class] ([Id])
GO
ALTER TABLE [dbo].[ClassAccount] CHECK CONSTRAINT [ClassAccount_Class_ClassId_fk]
GO
ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [Document_Lesson_LessonId_fk] FOREIGN KEY([LessonId])
REFERENCES [dbo].[Lesson] ([Id])
GO
ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [Document_Lesson_LessonId_fk]
GO
ALTER TABLE [dbo].[EmailSend]  WITH CHECK ADD  CONSTRAINT [EmailSend_Account_AccountId_fk] FOREIGN KEY([SenderId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[EmailSend] CHECK CONSTRAINT [EmailSend_Account_AccountId_fk]
GO
ALTER TABLE [dbo].[EmailSend]  WITH CHECK ADD  CONSTRAINT [EmailSend_EmailTemplate_TemplateId_fk] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[EmailTemplate] ([Id])
GO
ALTER TABLE [dbo].[EmailSend] CHECK CONSTRAINT [EmailSend_EmailTemplate_TemplateId_fk]
GO
ALTER TABLE [dbo].[EmailSendStudent]  WITH CHECK ADD  CONSTRAINT [EmailSendStudent_EmailSend_EmailSendId_fk] FOREIGN KEY([EmailSendId])
REFERENCES [dbo].[EmailSend] ([Id])
GO
ALTER TABLE [dbo].[EmailSendStudent] CHECK CONSTRAINT [EmailSendStudent_EmailSend_EmailSendId_fk]
GO
ALTER TABLE [dbo].[EmailSendStudent]  WITH CHECK ADD  CONSTRAINT [EmailSendStudent_Student_StudentId_fk] FOREIGN KEY([ReceiveId])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[EmailSendStudent] CHECK CONSTRAINT [EmailSendStudent_Student_StudentId_fk]
GO
ALTER TABLE [dbo].[Lesson]  WITH CHECK ADD  CONSTRAINT [Lesson_Module_ModuleId_fk] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([Id])
GO
ALTER TABLE [dbo].[Lesson] CHECK CONSTRAINT [Lesson_Module_ModuleId_fk]
GO
ALTER TABLE [dbo].[ProgramModule]  WITH CHECK ADD  CONSTRAINT [ProgramModule_Module_ModuleId_fk] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([Id])
GO
ALTER TABLE [dbo].[ProgramModule] CHECK CONSTRAINT [ProgramModule_Module_ModuleId_fk]
GO
ALTER TABLE [dbo].[ProgramModule]  WITH CHECK ADD  CONSTRAINT [ProgramModule_TrainingProgram_ProgramId_fk] FOREIGN KEY([ProgramId])
REFERENCES [dbo].[TrainingProgram] ([Id])
GO
ALTER TABLE [dbo].[ProgramModule] CHECK CONSTRAINT [ProgramModule_TrainingProgram_ProgramId_fk]
GO
ALTER TABLE [dbo].[Score]  WITH CHECK ADD  CONSTRAINT [Score_Class_ClassId_fk] FOREIGN KEY([ClassId])
REFERENCES [dbo].[Class] ([Id])
GO
ALTER TABLE [dbo].[Score] CHECK CONSTRAINT [Score_Class_ClassId_fk]
GO
ALTER TABLE [dbo].[Score]  WITH CHECK ADD  CONSTRAINT [Score_Student_StudentId_fk] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[Score] CHECK CONSTRAINT [Score_Student_StudentId_fk]
GO
ALTER TABLE [dbo].[StudentCertificate]  WITH CHECK ADD  CONSTRAINT [StudentCertificate_Certificate_CertificateId_fk] FOREIGN KEY([CertificateId])
REFERENCES [dbo].[Certificate] ([Id])
GO
ALTER TABLE [dbo].[StudentCertificate] CHECK CONSTRAINT [StudentCertificate_Certificate_CertificateId_fk]
GO
ALTER TABLE [dbo].[StudentCertificate]  WITH CHECK ADD  CONSTRAINT [StudentCertificate_Student_StudentId_fk] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[StudentCertificate] CHECK CONSTRAINT [StudentCertificate_Student_StudentId_fk]
GO
ALTER TABLE [dbo].[StudentClass]  WITH CHECK ADD  CONSTRAINT [StudentClass_Class_ClassId_fk] FOREIGN KEY([ClassId])
REFERENCES [dbo].[Class] ([Id])
GO
ALTER TABLE [dbo].[StudentClass] CHECK CONSTRAINT [StudentClass_Class_ClassId_fk]
GO
ALTER TABLE [dbo].[StudentClass]  WITH CHECK ADD  CONSTRAINT [StudentClass_Student_StudentId_fk] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[StudentClass] CHECK CONSTRAINT [StudentClass_Student_StudentId_fk]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Average off Q1 - Q6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Score', @level2type=N'COLUMN',@level2name=N'QuizAvg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Avarage of Assignment' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Score', @level2type=N'COLUMN',@level2name=N'AsmAvg'
GO
