USE [master]
GO
/****** Object:  Database [USPEducation]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE DATABASE [USPEducation]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'USPEducation_Data', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\USPEducation.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'USPEducation_Log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\USPEducation.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [USPEducation] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [USPEducation].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [USPEducation] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [USPEducation] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [USPEducation] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [USPEducation] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [USPEducation] SET ARITHABORT OFF 
GO
ALTER DATABASE [USPEducation] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [USPEducation] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [USPEducation] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [USPEducation] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [USPEducation] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [USPEducation] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [USPEducation] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [USPEducation] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [USPEducation] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [USPEducation] SET  ENABLE_BROKER 
GO
ALTER DATABASE [USPEducation] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [USPEducation] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [USPEducation] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [USPEducation] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [USPEducation] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [USPEducation] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [USPEducation] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [USPEducation] SET RECOVERY FULL 
GO
ALTER DATABASE [USPEducation] SET  MULTI_USER 
GO
ALTER DATABASE [USPEducation] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [USPEducation] SET DB_CHAINING OFF 
GO
ALTER DATABASE [USPEducation] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [USPEducation] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [USPEducation] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [USPEducation] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'USPEducation', N'ON'
GO
ALTER DATABASE [USPEducation] SET QUERY_STORE = OFF
GO
USE [USPEducation]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 23/03/2025 6:21:14 PM ******/
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
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[AdmissionYear] [int] NOT NULL,
	[MajorType] [int] NOT NULL,
	[MajorI] [nvarchar](10) NOT NULL,
	[MajorII] [nvarchar](10) NULL,
	[MinorI] [nvarchar](10) NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[StudentId] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CoursePrerequisites]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CoursePrerequisites](
	[IsPrerequisiteForId] [int] NOT NULL,
	[PrerequisitesId] [int] NOT NULL,
 CONSTRAINT [PK_CoursePrerequisites] PRIMARY KEY CLUSTERED 
(
	[IsPrerequisiteForId] ASC,
	[PrerequisitesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Courses]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Courses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[CreditPoints] [int] NOT NULL,
	[Level] [int] NOT NULL,
	[Semester] [int] NOT NULL,
	[SubjectAreaId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Fees] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProgramCoreCourses]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProgramCoreCourses](
	[CoreCoursesId] [int] NOT NULL,
	[IsCoreCourseForId] [int] NOT NULL,
 CONSTRAINT [PK_ProgramCoreCourses] PRIMARY KEY CLUSTERED 
(
	[CoreCoursesId] ASC,
	[IsCoreCourseForId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProgramElectiveCourses]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProgramElectiveCourses](
	[ElectiveCoursesId] [int] NOT NULL,
	[IsElectiveCourseForId] [int] NOT NULL,
 CONSTRAINT [PK_ProgramElectiveCourses] PRIMARY KEY CLUSTERED 
(
	[ElectiveCoursesId] ASC,
	[IsElectiveCourseForId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProgramRequirementCourses]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProgramRequirementCourses](
	[ProgramRequirementId] [int] NOT NULL,
	[RequiredCoursesId] [int] NOT NULL,
 CONSTRAINT [PK_ProgramRequirementCourses] PRIMARY KEY CLUSTERED 
(
	[ProgramRequirementId] ASC,
	[RequiredCoursesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProgramRequirements]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProgramRequirements](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProgramId] [int] NOT NULL,
	[SubjectAreaId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[CreditPointsRequired] [int] NOT NULL,
	[MinimumGrade] [nvarchar](2) NULL,
	[Description] [nvarchar](500) NULL,
	[Notes] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[SubjectAreaId1] [int] NULL,
 CONSTRAINT [PK_ProgramRequirements] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Programs]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Programs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[CreditPoints] [int] NOT NULL,
	[Duration] [int] NOT NULL,
	[Level] [int] NOT NULL,
	[OfferingYear] [int] NOT NULL,
	[MajorCreditsRequired] [int] NOT NULL,
	[MinorCreditsRequired] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Programs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentEnrollments]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentEnrollments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [nvarchar](450) NOT NULL,
	[CourseId] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[Semester] [int] NOT NULL,
	[Grade] [nvarchar](2) NULL,
	[Status] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[AcademicProgramId] [int] NULL,
 CONSTRAINT [PK_StudentEnrollments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubjectAreas]    Script Date: 23/03/2025 6:21:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubjectAreas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](3) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[CanBeMajor] [bit] NOT NULL,
	[CanBeMinor] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SubjectAreas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250322053758_InitialCreate', N'8.0.3')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250322072732_AddStudentIdToUsers', N'8.0.3')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250323002902_SeedCourses', N'8.0.3')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250323004047_SeedCoursesData', N'8.0.3')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250323004749_USPCourses', N'8.0.3')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250323005508_RevertUSPCourses', N'8.0.3')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250323005547_AddUSPCourses', N'8.0.3')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250323005848_UpdatedUSPCourses', N'8.0.3')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250323010301_UpdatedCourseCodes', N'8.0.3')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250323031500_AddCourseFees', N'8.0.3')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250323032011_AddFeesColumn', N'8.0.3')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250323032111_AddCourseFeesColumn', N'8.0.3')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'969faf61-73e4-4a92-b237-be1bff2b3a86', N'Manager', N'MANAGER', NULL)
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'bff80819-e586-41b0-b143-64abc1e4b873', N'Student', N'STUDENT', NULL)
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'f3389e34-345c-4097-9b87-82032a2087cc', N'969faf61-73e4-4a92-b237-be1bff2b3a86')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'007851bb-5989-4147-88fc-6fb3fe51e9a5', N'bff80819-e586-41b0-b143-64abc1e4b873')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'4a7d2991-5870-45a3-998c-df70290c6cbc', N'bff80819-e586-41b0-b143-64abc1e4b873')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'ae9919cb-c3f8-41c1-b5f2-9c923118ed9d', N'bff80819-e586-41b0-b143-64abc1e4b873')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e06fdbe8-a841-4d33-9f06-273bfc42056f', N'bff80819-e586-41b0-b143-64abc1e4b873')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e4f48fd0-0a4e-4230-89fa-a8b26810b4e6', N'bff80819-e586-41b0-b143-64abc1e4b873')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e54bba5c-4956-42e8-af4c-c683f73edc2b', N'bff80819-e586-41b0-b143-64abc1e4b873')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'eeaab3fe-d720-448d-a72d-f42dcd223b6f', N'bff80819-e586-41b0-b143-64abc1e4b873')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'f7bfccf9-ebce-475c-990e-51ca74b4c003', N'bff80819-e586-41b0-b143-64abc1e4b873')
GO
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AdmissionYear], [MajorType], [MajorI], [MajorII], [MinorI], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [StudentId]) VALUES (N'007851bb-5989-4147-88fc-6fb3fe51e9a5', N'Jane', N'Smith', 2024, 0, N'ITC', NULL, NULL, N'S12345679', N'S12345679', N'jane.smith@usp.ac.fj', N'JANE.SMITH@USP.AC.FJ', 1, N'AQAAAAIAAYagAAAAELRdGk6+z/AOz+6Stik1G84pAPAn91/5SrbLfY9gcgX2+oQV6RvsxnZAtIEFD9TM9A==', N'IYUXQOSN7W64RY75AI4VL42WYHCO7AYB', N'40c8a40e-b9b8-401c-b0d3-dcee5839378b', NULL, 0, 0, NULL, 1, 0, N'S12345679')
GO
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AdmissionYear], [MajorType], [MajorI], [MajorII], [MinorI], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [StudentId]) VALUES (N'4a7d2991-5870-45a3-998c-df70290c6cbc', N'John', N'Doe', 2025, 0, N'ITC', NULL, NULL, N'S12345678', N'S12345678', N'john.doe@usp.ac.fj', N'JOHN.DOE@USP.AC.FJ', 1, N'AQAAAAIAAYagAAAAEGT/aSCUbA3TI6biIp7vAEmL5uz2iKMYFxjZQ2n+yAV97ldbQQh4dHhL0pG7GwUTjg==', N'WR4QL2QGVGCDP7LCPAZWEH3QSVSVUCUJ', N'8cc4d130-698a-4ac8-8982-a8e7820c83a5', NULL, 0, 0, NULL, 1, 0, N'S12345678')
GO
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AdmissionYear], [MajorType], [MajorI], [MajorII], [MinorI], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [StudentId]) VALUES (N'ae9919cb-c3f8-41c1-b5f2-9c923118ed9d', N'Sarah', N'Davis', 2025, 0, N'CHE', NULL, NULL, N'S12345683', N'S12345683', N'sarah.davis@usp.ac.fj', N'SARAH.DAVIS@USP.AC.FJ', 1, N'AQAAAAIAAYagAAAAEK+pOCoxfCicZXOCt2v6uQWIoP7loskO8xFqCLGFWO3eaKUCfFP8u6Aac/IllSNQzQ==', N'AWE2LIMJNG6CDPS4KJHHJDDWVQTRGZMY', N'3c7dfba9-de42-4d1a-9250-95631cf09da9', NULL, 0, 0, NULL, 1, 0, N'S12345683')
GO
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AdmissionYear], [MajorType], [MajorI], [MajorII], [MinorI], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [StudentId]) VALUES (N'e06fdbe8-a841-4d33-9f06-273bfc42056f', N'Mike', N'Taylor', 2025, 1, N'ECO', N'MGT', NULL, N'S12345684', N'S12345684', N'mike.taylor@usp.ac.fj', N'MIKE.TAYLOR@USP.AC.FJ', 1, N'AQAAAAIAAYagAAAAEOiHhIZqahO0O9+5CPw11+tQAqXPwBnsDjCSx8CuPU1YG7rV2TgoRmD7qQ2tbD06dg==', N'AJ5WQOROE7N7JP6T4C7PZTZRGOPJLCRQ', N'c94466d8-4c7c-4822-885b-fe58c595c033', NULL, 0, 0, NULL, 1, 0, N'S12345684')
GO
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AdmissionYear], [MajorType], [MajorI], [MajorII], [MinorI], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [StudentId]) VALUES (N'e4f48fd0-0a4e-4230-89fa-a8b26810b4e6', N'Alex', N'Brown', 2024, 0, N'BIO', NULL, NULL, N'S12345682', N'S12345682', N'alex.brown@usp.ac.fj', N'ALEX.BROWN@USP.AC.FJ', 1, N'AQAAAAIAAYagAAAAEAiIujiFGw2I1e09dTvaVxhevp+GXEeE334zBYh6RGrDI2nlmiIqoPwhBTa6u9Ak0A==', N'2COUHGNANCTXVUGMASCMS7U7UF4OCOPP', N'fa92e70d-141e-4bfa-b8c7-57107bb37a1d', NULL, 0, 0, NULL, 1, 0, N'S12345682')
GO
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AdmissionYear], [MajorType], [MajorI], [MajorII], [MinorI], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [StudentId]) VALUES (N'e54bba5c-4956-42e8-af4c-c683f73edc2b', N'Lisa', N'Anderson', 2023, 0, N'ECO', NULL, NULL, N'S12345685', N'S12345685', N'lisa.anderson@usp.ac.fj', N'LISA.ANDERSON@USP.AC.FJ', 1, N'AQAAAAIAAYagAAAAEOaO3rDjXjEdNE3U2DjFGkIS8QT8ovEEpzZi5hhqTnCfx0PMuVQvHt8FivJ82dxZvQ==', N'KV6NPYEC72R6FBICS4GI4BMHYB6SMYBV', N'10329a9e-12e0-4927-b229-6abf1e0586a8', NULL, 0, 0, NULL, 1, 0, N'S12345685')
GO
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AdmissionYear], [MajorType], [MajorI], [MajorII], [MinorI], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [StudentId]) VALUES (N'eeaab3fe-d720-448d-a72d-f42dcd223b6f', N'Bob', N'Wilson', 2025, 1, N'ACC', N'FIN', NULL, N'S12345680', N'S12345680', N'bob.wilson@usp.ac.fj', N'BOB.WILSON@USP.AC.FJ', 1, N'AQAAAAIAAYagAAAAEF6VKn1/DchIA5HRFNhgNDn9K1QQXmJ32y+ZYpf9MBsOnkPO90nFTHcaNE8yP58jrA==', N'3PSZ4HETYCSQ3W7GJKW3ZNDZW34BQIJV', N'cce96a40-2b0d-459d-996f-46ae5e20e993', NULL, 0, 0, NULL, 1, 0, N'S12345680')
GO
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AdmissionYear], [MajorType], [MajorI], [MajorII], [MinorI], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [StudentId]) VALUES (N'f3389e34-345c-4097-9b87-82032a2087cc', N'Program', N'Manager', 2025, 0, N'MGT', NULL, NULL, N'manager@usp.ac.fj', N'MANAGER@USP.AC.FJ', N'manager@usp.ac.fj', N'MANAGER@USP.AC.FJ', 1, N'AQAAAAIAAYagAAAAELhS9clCddiVOT1MMN2pG4UGNXEjqBL5qwEcxXhmFprA4VFD0eaSS7uzZKvFWy2PuQ==', N'FKYCL2AZVLIA7TW5NIIC7VBVRW73GXX6', N'23d176ce-617e-48b3-9778-552564506bc4', NULL, 0, 0, NULL, 1, 0, N'MNGR0000')
GO
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AdmissionYear], [MajorType], [MajorI], [MajorII], [MinorI], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [StudentId]) VALUES (N'f7bfccf9-ebce-475c-990e-51ca74b4c003', N'Mary', N'Jones', 2025, 0, N'MGT', NULL, N'ACC', N'S12345681', N'S12345681', N'mary.jones@usp.ac.fj', N'MARY.JONES@USP.AC.FJ', 1, N'AQAAAAIAAYagAAAAEJthpe4dcfvtbT2WVZ5o9jAYROF+5wrbRB6yka5x+msq8KzpwxpC5eiMv4VhzCXo3w==', N'YPFNWWV5HKUJAWLYDRZIU4CZVYBH3BZH', N'f399e1a6-bf30-431f-9c24-109c0a9e826f', NULL, 0, 0, NULL, 1, 0, N'S12345681')
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (2, 1)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (3, 1)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (4, 1)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (3, 2)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (4, 2)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (5, 3)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (6, 3)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (5, 4)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (6, 4)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (9, 7)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (10, 7)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (9, 8)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (10, 8)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (11, 9)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (12, 9)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (11, 10)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (12, 10)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (15, 13)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (16, 13)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (15, 14)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (16, 14)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (17, 15)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (18, 15)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (17, 16)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (18, 16)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (21, 19)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (22, 19)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (21, 20)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (22, 20)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (23, 21)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (24, 21)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (23, 22)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (24, 22)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (27, 25)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (28, 25)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (27, 26)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (28, 26)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (29, 27)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (30, 27)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (29, 28)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (30, 28)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (33, 31)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (34, 31)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (33, 32)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (34, 32)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (35, 33)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (36, 33)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (35, 34)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (36, 34)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (39, 37)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (40, 37)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (39, 38)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (40, 38)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (41, 39)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (42, 39)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (41, 40)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (42, 40)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (45, 43)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (46, 43)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (45, 44)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (46, 44)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (47, 45)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (48, 45)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (47, 46)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (48, 46)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (51, 49)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (52, 49)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (51, 50)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (52, 50)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (53, 51)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (54, 51)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (53, 52)
GO
INSERT [dbo].[CoursePrerequisites] ([IsPrerequisiteForId], [PrerequisitesId]) VALUES (54, 52)
GO
SET IDENTITY_INSERT [dbo].[Courses] ON 
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (1, N'ACC101', N'Introduction to Accounting', N'First-year introduction to accounting', 12, 0, 0, 1, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (2, N'ACC102', N'Foundations of Accounting', N'First-year foundations of accounting', 12, 0, 1, 1, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (3, N'ACC201', N'Intermediate Accounting I', N'Second-year intermediate studies in accounting', 12, 1, 0, 1, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (4, N'ACC202', N'Intermediate Accounting II', N'Second-year advanced studies in accounting', 12, 1, 1, 1, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (5, N'ACC301', N'Advanced Accounting I', N'Third-year advanced studies in accounting', 12, 2, 0, 1, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (6, N'ACC302', N'Advanced Accounting II', N'Third-year specialized studies in accounting', 12, 2, 1, 1, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (7, N'ECO101', N'Introduction to Economics', N'First-year introduction to economics', 12, 0, 0, 2, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (8, N'ECO102', N'Foundations of Economics', N'First-year foundations of economics', 12, 0, 1, 2, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (9, N'ECO201', N'Intermediate Economics I', N'Second-year intermediate studies in economics', 12, 1, 0, 2, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (10, N'ECO202', N'Intermediate Economics II', N'Second-year advanced studies in economics', 12, 1, 1, 2, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (11, N'ECO301', N'Advanced Economics I', N'Third-year advanced studies in economics', 12, 2, 0, 2, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (12, N'ECO302', N'Advanced Economics II', N'Third-year specialized studies in economics', 12, 2, 1, 2, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (13, N'MGT101', N'Introduction to Management', N'First-year introduction to management', 12, 0, 0, 3, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (14, N'MGT102', N'Foundations of Management', N'First-year foundations of management', 12, 0, 1, 3, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (15, N'MGT201', N'Intermediate Management I', N'Second-year intermediate studies in management', 12, 1, 0, 3, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (16, N'MGT202', N'Intermediate Management II', N'Second-year advanced studies in management', 12, 1, 1, 3, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (17, N'MGT301', N'Advanced Management I', N'Third-year advanced studies in management', 12, 2, 0, 3, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (18, N'MGT302', N'Advanced Management II', N'Third-year specialized studies in management', 12, 2, 1, 3, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (19, N'BIO101', N'Introduction to Biology', N'First-year introduction to biology', 12, 0, 0, 4, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (20, N'BIO102', N'Foundations of Biology', N'First-year foundations of biology', 12, 0, 1, 4, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (21, N'BIO201', N'Intermediate Biology I', N'Second-year intermediate studies in biology', 12, 1, 0, 4, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (22, N'BIO202', N'Intermediate Biology II', N'Second-year advanced studies in biology', 12, 1, 1, 4, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (23, N'BIO301', N'Advanced Biology I', N'Third-year advanced studies in biology', 12, 2, 0, 4, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (24, N'BIO302', N'Advanced Biology II', N'Third-year specialized studies in biology', 12, 2, 1, 4, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (25, N'CHE101', N'Introduction to Chemistry', N'First-year introduction to chemistry', 12, 0, 0, 5, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (26, N'CHE102', N'Foundations of Chemistry', N'First-year foundations of chemistry', 12, 0, 1, 5, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (27, N'CHE201', N'Intermediate Chemistry I', N'Second-year intermediate studies in chemistry', 12, 1, 0, 5, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (28, N'CHE202', N'Intermediate Chemistry II', N'Second-year advanced studies in chemistry', 12, 1, 1, 5, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (29, N'CHE301', N'Advanced Chemistry I', N'Third-year advanced studies in chemistry', 12, 2, 0, 5, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (30, N'CHE302', N'Advanced Chemistry II', N'Third-year specialized studies in chemistry', 12, 2, 1, 5, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (31, N'PHY101', N'Introduction to Physics', N'First-year introduction to physics', 12, 0, 0, 6, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (32, N'PHY102', N'Foundations of Physics', N'First-year foundations of physics', 12, 0, 1, 6, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (33, N'PHY201', N'Intermediate Physics I', N'Second-year intermediate studies in physics', 12, 1, 0, 6, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (34, N'PHY202', N'Intermediate Physics II', N'Second-year advanced studies in physics', 12, 1, 1, 6, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (35, N'PHY301', N'Advanced Physics I', N'Third-year advanced studies in physics', 12, 2, 0, 6, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (36, N'PHY302', N'Advanced Physics II', N'Third-year specialized studies in physics', 12, 2, 1, 6, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (37, N'HIS101', N'Introduction to History', N'First-year introduction to history', 12, 0, 0, 7, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (38, N'HIS102', N'Foundations of History', N'First-year foundations of history', 12, 0, 1, 7, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (39, N'HIS201', N'Intermediate History I', N'Second-year intermediate studies in history', 12, 1, 0, 7, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (40, N'HIS202', N'Intermediate History II', N'Second-year advanced studies in history', 12, 1, 1, 7, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (41, N'HIS301', N'Advanced History I', N'Third-year advanced studies in history', 12, 2, 0, 7, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (42, N'HIS302', N'Advanced History II', N'Third-year specialized studies in history', 12, 2, 1, 7, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (43, N'PSY101', N'Introduction to Psychology', N'First-year introduction to psychology', 12, 0, 0, 8, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (44, N'PSY102', N'Foundations of Psychology', N'First-year foundations of psychology', 12, 0, 1, 8, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (45, N'PSY201', N'Intermediate Psychology I', N'Second-year intermediate studies in psychology', 12, 1, 0, 8, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (46, N'PSY202', N'Intermediate Psychology II', N'Second-year advanced studies in psychology', 12, 1, 1, 8, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (47, N'PSY301', N'Advanced Psychology I', N'Third-year advanced studies in psychology', 12, 2, 0, 8, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (48, N'PSY302', N'Advanced Psychology II', N'Third-year specialized studies in psychology', 12, 2, 1, 8, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (49, N'SOC101', N'Introduction to Sociology', N'First-year introduction to sociology', 12, 0, 0, 9, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (50, N'SOC102', N'Foundations of Sociology', N'First-year foundations of sociology', 12, 0, 1, 9, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (51, N'SOC201', N'Intermediate Sociology I', N'Second-year intermediate studies in sociology', 12, 1, 0, 9, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (52, N'SOC202', N'Intermediate Sociology II', N'Second-year advanced studies in sociology', 12, 1, 1, 9, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (53, N'SOC301', N'Advanced Sociology I', N'Third-year advanced studies in sociology', 12, 2, 0, 9, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (54, N'SOC302', N'Advanced Sociology II', N'Third-year specialized studies in sociology', 12, 2, 1, 9, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (58, N'CS101', N'Introduction to Computer Science', N'Basic concepts of computer science', 3, 0, 0, 17, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (59, N'CS102', N'Programming Fundamentals', N'Introduction to programming concepts', 3, 0, 1, 17, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (60, N'MA101', N'Calculus I', N'Introduction to calculus', 4, 0, 0, 18, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (61, N'AC101', N'Financial Accounting', N'Basic accounting principles', 3, 0, 0, 19, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (62, N'FI101', N'Introduction to Finance', N'Basic financial concepts', 3, 0, 1, 20, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (63, N'MG101', N'Principles of Management', N'Basic management concepts', 3, 0, 0, 21, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (64, N'EC101', N'Microeconomics', N'Introduction to microeconomics', 3, 0, 0, 22, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (65, N'BI101', N'General Biology', N'Introduction to biology', 4, 0, 0, 23, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (66, N'CH101', N'General Chemistry', N'Introduction to chemistry', 4, 0, 0, 24, 1, NULL)
GO
INSERT [dbo].[Courses] ([Id], [Code], [Name], [Description], [CreditPoints], [Level], [Semester], [SubjectAreaId], [IsActive], [Fees]) VALUES (67, N'PH101', N'General Physics', N'Introduction to physics', 4, 0, 0, 25, 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[Courses] OFF
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (1, 1)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (2, 1)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (1, 2)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (2, 2)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (1, 3)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (1, 4)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (1, 5)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (1, 6)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (3, 7)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (4, 7)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (3, 8)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (4, 8)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (3, 9)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (3, 10)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (3, 11)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (3, 12)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (5, 13)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (6, 13)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (5, 14)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (6, 14)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (5, 15)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (5, 16)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (5, 17)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (5, 18)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (7, 19)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (8, 19)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (7, 20)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (8, 20)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (7, 21)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (7, 22)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (7, 23)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (7, 24)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (9, 25)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (10, 25)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (9, 26)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (10, 26)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (9, 27)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (9, 28)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (9, 29)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (9, 30)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (11, 31)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (12, 31)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (11, 32)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (12, 32)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (11, 33)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (11, 34)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (11, 35)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (11, 36)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (13, 37)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (14, 37)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (13, 38)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (14, 38)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (13, 39)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (13, 40)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (13, 41)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (13, 42)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (15, 43)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (16, 43)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (15, 44)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (16, 44)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (15, 45)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (15, 46)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (15, 47)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (15, 48)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (17, 49)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (18, 49)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (17, 50)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (18, 50)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (17, 51)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (17, 52)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (17, 53)
GO
INSERT [dbo].[ProgramRequirementCourses] ([ProgramRequirementId], [RequiredCoursesId]) VALUES (17, 54)
GO
SET IDENTITY_INSERT [dbo].[ProgramRequirements] ON 
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (1, 1, 1, 0, 2024, 48, NULL, N'Core courses required for Accounting major', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (2, 1, 1, 2, 2024, 24, NULL, N'Courses required for Accounting minor', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (3, 1, 2, 0, 2024, 48, NULL, N'Core courses required for Economics major', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (4, 1, 2, 2, 2024, 24, NULL, N'Courses required for Economics minor', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (5, 1, 3, 0, 2024, 48, NULL, N'Core courses required for Management major', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (6, 1, 3, 2, 2024, 24, NULL, N'Courses required for Management minor', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (7, 2, 4, 0, 2024, 48, NULL, N'Core courses required for Biology major', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (8, 2, 4, 2, 2024, 24, NULL, N'Courses required for Biology minor', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (9, 2, 5, 0, 2024, 48, NULL, N'Core courses required for Chemistry major', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (10, 2, 5, 2, 2024, 24, NULL, N'Courses required for Chemistry minor', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (11, 2, 6, 0, 2024, 48, NULL, N'Core courses required for Physics major', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (12, 2, 6, 2, 2024, 24, NULL, N'Courses required for Physics minor', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (13, 3, 7, 0, 2024, 48, NULL, N'Core courses required for History major', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (14, 3, 7, 2, 2024, 24, NULL, N'Courses required for History minor', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (15, 3, 8, 0, 2024, 48, NULL, N'Core courses required for Psychology major', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (16, 3, 8, 2, 2024, 24, NULL, N'Courses required for Psychology minor', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (17, 3, 9, 0, 2024, 48, NULL, N'Core courses required for Sociology major', NULL, 1, NULL)
GO
INSERT [dbo].[ProgramRequirements] ([Id], [ProgramId], [SubjectAreaId], [Type], [Year], [CreditPointsRequired], [MinimumGrade], [Description], [Notes], [IsActive], [SubjectAreaId1]) VALUES (18, 3, 9, 2, 2024, 24, NULL, N'Courses required for Sociology minor', NULL, 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[ProgramRequirements] OFF
GO
SET IDENTITY_INSERT [dbo].[Programs] ON 
GO
INSERT [dbo].[Programs] ([Id], [Code], [Name], [Description], [CreditPoints], [Duration], [Level], [OfferingYear], [MajorCreditsRequired], [MinorCreditsRequired], [IsActive]) VALUES (1, N'BCOM', N'Bachelor of Commerce', N'A comprehensive business degree focusing on commerce, finance, and management', 360, 3, 1, 2024, 48, 24, 1)
GO
INSERT [dbo].[Programs] ([Id], [Code], [Name], [Description], [CreditPoints], [Duration], [Level], [OfferingYear], [MajorCreditsRequired], [MinorCreditsRequired], [IsActive]) VALUES (2, N'BSC', N'Bachelor of Science', N'A degree program focusing on scientific principles and research', 360, 3, 2, 2024, 48, 24, 1)
GO
INSERT [dbo].[Programs] ([Id], [Code], [Name], [Description], [CreditPoints], [Duration], [Level], [OfferingYear], [MajorCreditsRequired], [MinorCreditsRequired], [IsActive]) VALUES (3, N'BA', N'Bachelor of Arts', N'A flexible degree program in humanities and social sciences', 360, 3, 0, 2024, 48, 24, 1)
GO
SET IDENTITY_INSERT [dbo].[Programs] OFF
GO
SET IDENTITY_INSERT [dbo].[SubjectAreas] ON 
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (1, N'ACC', N'Accounting', N'Study of accounting principles, financial analysis, and business reporting', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (2, N'ECO', N'Economics', N'Study of economic theories, market analysis, and policy development', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (3, N'MGT', N'Management', N'Study of organizational management, leadership, and business strategy', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (4, N'BIO', N'Biology', N'Study of living organisms, their structure, function, and evolution', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (5, N'CHE', N'Chemistry', N'Study of matter, its properties, structure, and transformations', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (6, N'PHY', N'Physics', N'Study of matter, energy, and their interactions', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (7, N'HIS', N'History', N'Study of past events, societies, and civilizations', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (8, N'PSY', N'Psychology', N'Study of human behavior and mental processes', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (9, N'SOC', N'Sociology', N'Study of human society, social relationships, and institutions', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (17, N'CS', N'Computer Science', N'Computer Science courses', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (18, N'MA', N'Mathematics', N'Mathematics courses', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (19, N'AC', N'Accounting', N'Accounting courses', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (20, N'FI', N'Finance', N'Finance courses', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (21, N'MG', N'Management', N'Management courses', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (22, N'EC', N'Economics', N'Economics courses', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (23, N'BI', N'Biology', N'Biology courses', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (24, N'CH', N'Chemistry', N'Chemistry courses', 1, 1, 1)
GO
INSERT [dbo].[SubjectAreas] ([Id], [Code], [Name], [Description], [CanBeMajor], [CanBeMinor], [IsActive]) VALUES (25, N'PH', N'Physics', N'Physics courses', 1, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[SubjectAreas] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CoursePrerequisites_PrerequisitesId]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_CoursePrerequisites_PrerequisitesId] ON [dbo].[CoursePrerequisites]
(
	[PrerequisitesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Courses_SubjectAreaId]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_Courses_SubjectAreaId] ON [dbo].[Courses]
(
	[SubjectAreaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProgramCoreCourses_IsCoreCourseForId]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProgramCoreCourses_IsCoreCourseForId] ON [dbo].[ProgramCoreCourses]
(
	[IsCoreCourseForId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProgramElectiveCourses_IsElectiveCourseForId]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProgramElectiveCourses_IsElectiveCourseForId] ON [dbo].[ProgramElectiveCourses]
(
	[IsElectiveCourseForId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProgramRequirementCourses_RequiredCoursesId]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProgramRequirementCourses_RequiredCoursesId] ON [dbo].[ProgramRequirementCourses]
(
	[RequiredCoursesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProgramRequirements_ProgramId]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProgramRequirements_ProgramId] ON [dbo].[ProgramRequirements]
(
	[ProgramId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProgramRequirements_SubjectAreaId]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProgramRequirements_SubjectAreaId] ON [dbo].[ProgramRequirements]
(
	[SubjectAreaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProgramRequirements_SubjectAreaId1]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProgramRequirements_SubjectAreaId1] ON [dbo].[ProgramRequirements]
(
	[SubjectAreaId1] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_StudentEnrollments_AcademicProgramId]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_StudentEnrollments_AcademicProgramId] ON [dbo].[StudentEnrollments]
(
	[AcademicProgramId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_StudentEnrollments_CourseId]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_StudentEnrollments_CourseId] ON [dbo].[StudentEnrollments]
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_StudentEnrollments_StudentId]    Script Date: 23/03/2025 6:21:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_StudentEnrollments_StudentId] ON [dbo].[StudentEnrollments]
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT (N'') FOR [StudentId]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[CoursePrerequisites]  WITH CHECK ADD  CONSTRAINT [FK_CoursePrerequisites_Courses_IsPrerequisiteForId] FOREIGN KEY([IsPrerequisiteForId])
REFERENCES [dbo].[Courses] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CoursePrerequisites] CHECK CONSTRAINT [FK_CoursePrerequisites_Courses_IsPrerequisiteForId]
GO
ALTER TABLE [dbo].[CoursePrerequisites]  WITH CHECK ADD  CONSTRAINT [FK_CoursePrerequisites_Courses_PrerequisitesId] FOREIGN KEY([PrerequisitesId])
REFERENCES [dbo].[Courses] ([Id])
GO
ALTER TABLE [dbo].[CoursePrerequisites] CHECK CONSTRAINT [FK_CoursePrerequisites_Courses_PrerequisitesId]
GO
ALTER TABLE [dbo].[Courses]  WITH CHECK ADD  CONSTRAINT [FK_Courses_SubjectAreas_SubjectAreaId] FOREIGN KEY([SubjectAreaId])
REFERENCES [dbo].[SubjectAreas] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Courses] CHECK CONSTRAINT [FK_Courses_SubjectAreas_SubjectAreaId]
GO
ALTER TABLE [dbo].[ProgramCoreCourses]  WITH CHECK ADD  CONSTRAINT [FK_ProgramCoreCourses_Courses_CoreCoursesId] FOREIGN KEY([CoreCoursesId])
REFERENCES [dbo].[Courses] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProgramCoreCourses] CHECK CONSTRAINT [FK_ProgramCoreCourses_Courses_CoreCoursesId]
GO
ALTER TABLE [dbo].[ProgramCoreCourses]  WITH CHECK ADD  CONSTRAINT [FK_ProgramCoreCourses_Programs_IsCoreCourseForId] FOREIGN KEY([IsCoreCourseForId])
REFERENCES [dbo].[Programs] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProgramCoreCourses] CHECK CONSTRAINT [FK_ProgramCoreCourses_Programs_IsCoreCourseForId]
GO
ALTER TABLE [dbo].[ProgramElectiveCourses]  WITH CHECK ADD  CONSTRAINT [FK_ProgramElectiveCourses_Courses_ElectiveCoursesId] FOREIGN KEY([ElectiveCoursesId])
REFERENCES [dbo].[Courses] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProgramElectiveCourses] CHECK CONSTRAINT [FK_ProgramElectiveCourses_Courses_ElectiveCoursesId]
GO
ALTER TABLE [dbo].[ProgramElectiveCourses]  WITH CHECK ADD  CONSTRAINT [FK_ProgramElectiveCourses_Programs_IsElectiveCourseForId] FOREIGN KEY([IsElectiveCourseForId])
REFERENCES [dbo].[Programs] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProgramElectiveCourses] CHECK CONSTRAINT [FK_ProgramElectiveCourses_Programs_IsElectiveCourseForId]
GO
ALTER TABLE [dbo].[ProgramRequirementCourses]  WITH CHECK ADD  CONSTRAINT [FK_ProgramRequirementCourses_Courses_RequiredCoursesId] FOREIGN KEY([RequiredCoursesId])
REFERENCES [dbo].[Courses] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProgramRequirementCourses] CHECK CONSTRAINT [FK_ProgramRequirementCourses_Courses_RequiredCoursesId]
GO
ALTER TABLE [dbo].[ProgramRequirementCourses]  WITH CHECK ADD  CONSTRAINT [FK_ProgramRequirementCourses_ProgramRequirements_ProgramRequirementId] FOREIGN KEY([ProgramRequirementId])
REFERENCES [dbo].[ProgramRequirements] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProgramRequirementCourses] CHECK CONSTRAINT [FK_ProgramRequirementCourses_ProgramRequirements_ProgramRequirementId]
GO
ALTER TABLE [dbo].[ProgramRequirements]  WITH CHECK ADD  CONSTRAINT [FK_ProgramRequirements_Programs_ProgramId] FOREIGN KEY([ProgramId])
REFERENCES [dbo].[Programs] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProgramRequirements] CHECK CONSTRAINT [FK_ProgramRequirements_Programs_ProgramId]
GO
ALTER TABLE [dbo].[ProgramRequirements]  WITH CHECK ADD  CONSTRAINT [FK_ProgramRequirements_SubjectAreas_SubjectAreaId] FOREIGN KEY([SubjectAreaId])
REFERENCES [dbo].[SubjectAreas] ([Id])
GO
ALTER TABLE [dbo].[ProgramRequirements] CHECK CONSTRAINT [FK_ProgramRequirements_SubjectAreas_SubjectAreaId]
GO
ALTER TABLE [dbo].[ProgramRequirements]  WITH CHECK ADD  CONSTRAINT [FK_ProgramRequirements_SubjectAreas_SubjectAreaId1] FOREIGN KEY([SubjectAreaId1])
REFERENCES [dbo].[SubjectAreas] ([Id])
GO
ALTER TABLE [dbo].[ProgramRequirements] CHECK CONSTRAINT [FK_ProgramRequirements_SubjectAreas_SubjectAreaId1]
GO
ALTER TABLE [dbo].[StudentEnrollments]  WITH CHECK ADD  CONSTRAINT [FK_StudentEnrollments_AspNetUsers_StudentId] FOREIGN KEY([StudentId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StudentEnrollments] CHECK CONSTRAINT [FK_StudentEnrollments_AspNetUsers_StudentId]
GO
ALTER TABLE [dbo].[StudentEnrollments]  WITH CHECK ADD  CONSTRAINT [FK_StudentEnrollments_Courses_CourseId] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Courses] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StudentEnrollments] CHECK CONSTRAINT [FK_StudentEnrollments_Courses_CourseId]
GO
ALTER TABLE [dbo].[StudentEnrollments]  WITH CHECK ADD  CONSTRAINT [FK_StudentEnrollments_Programs_AcademicProgramId] FOREIGN KEY([AcademicProgramId])
REFERENCES [dbo].[Programs] ([Id])
GO
ALTER TABLE [dbo].[StudentEnrollments] CHECK CONSTRAINT [FK_StudentEnrollments_Programs_AcademicProgramId]
GO
USE [master]
GO
ALTER DATABASE [USPEducation] SET  READ_WRITE 
GO
