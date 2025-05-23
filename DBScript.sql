USE [master]
GO
/****** Object:  Database [HybridDB]    Script Date: 5/22/2025 1:07:31 PM ******/
CREATE DATABASE [HybridDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HybridDB', FILENAME = N'D:\Tools\SQL\MSSQL15.MSSQLSERVER\MSSQL\DATA\HybridDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'HybridDB_log', FILENAME = N'D:\Tools\SQL\MSSQL15.MSSQLSERVER\MSSQL\DATA\HybridDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [HybridDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HybridDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HybridDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HybridDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HybridDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HybridDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HybridDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [HybridDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HybridDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HybridDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HybridDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HybridDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HybridDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HybridDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HybridDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HybridDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HybridDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [HybridDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HybridDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HybridDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HybridDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HybridDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HybridDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HybridDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HybridDB] SET RECOVERY FULL 
GO
ALTER DATABASE [HybridDB] SET  MULTI_USER 
GO
ALTER DATABASE [HybridDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HybridDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HybridDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HybridDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [HybridDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [HybridDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'HybridDB', N'ON'
GO
ALTER DATABASE [HybridDB] SET QUERY_STORE = OFF
GO
USE [HybridDB]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[CourseId] [char](10) NOT NULL,
	[CourseName] [nvarchar](max) NOT NULL,
	[DataText] [nvarchar](max) NULL,
	[LevelId] [char](1) NOT NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Level]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Level](
	[LevelId] [char](1) NOT NULL,
	[LevelName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Level] PRIMARY KEY CLUSTERED 
(
	[LevelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Minigame]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Minigame](
	[MinigameId] [char](10) NOT NULL,
	[MinigameName] [nvarchar](255) NOT NULL,
	[TeacherId] [char](10) NOT NULL,
	[ThumbnailImage] [varchar](max) NULL,
	[DataText] [nvarchar](max) NULL,
	[Duration] [int] NOT NULL,
	[ParticipantsCount] [int] NULL,
	[RatingScore] [float] NULL,
	[TemplateId] [char](10) NOT NULL,
	[CourseId] [char](10) NOT NULL,
 CONSTRAINT [PK_Minigame] PRIMARY KEY CLUSTERED 
(
	[MinigameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MinigameTemplate]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MinigameTemplate](
	[TemplateId] [char](10) NOT NULL,
	[TemplateName] [nvarchar](255) NOT NULL,
	[Image] [varchar](max) NULL,
	[Summary] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_MinigameTemplate] PRIMARY KEY CLUSTERED 
(
	[TemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethod]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethod](
	[MethodId] [char](1) NOT NULL,
	[MethodName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_PaymentMethod] PRIMARY KEY CLUSTERED 
(
	[MethodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rating]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rating](
	[StudentId] [char](10) NOT NULL,
	[MinigameId] [char](10) NOT NULL,
	[Score] [float] NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Rating] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC,
	[MinigameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [char](1) NOT NULL,
	[RoleName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[UserId] [char](10) NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[Address] [nvarchar](255) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[YearOfBirth] [int] NOT NULL,
	[TierId] [char](1) NOT NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentAccomplisment]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentAccomplisment](
	[StudentId] [char](10) NOT NULL,
	[MinigameId] [char](10) NOT NULL,
	[Score] [float] NOT NULL,
	[Duration] [int] NOT NULL,
	[TakenDate] [datetime] NOT NULL,
 CONSTRAINT [PK_StudentAccomplisment] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC,
	[MinigameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentSupscription]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentSupscription](
	[StudentId] [char](10) NOT NULL,
	[TierId] [char](1) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_StudentSupscription] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC,
	[TierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentTier]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentTier](
	[TierId] [char](1) NOT NULL,
	[TierName] [nchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_StudentTier] PRIMARY KEY CLUSTERED 
(
	[TierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupscriptionExtentionOrder]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupscriptionExtentionOrder](
	[UserId] [char](10) NOT NULL,
	[TierId] [char](1) NOT NULL,
	[TransactionId] [char](10) NOT NULL,
	[Days] [int] NOT NULL,
 CONSTRAINT [PK_SupscriptionExtentionOrder_1] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[TierId] ASC,
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teacher]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teacher](
	[UserId] [char](10) NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[Address] [nvarchar](255) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[YearOfBirth] [int] NOT NULL,
	[TierId] [char](1) NOT NULL,
 CONSTRAINT [PK_Teacher] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeacherSupscription]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeacherSupscription](
	[TeacherId] [char](10) NOT NULL,
	[TierId] [char](1) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_TeacherSupscription] PRIMARY KEY CLUSTERED 
(
	[TeacherId] ASC,
	[TierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeacherTier]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeacherTier](
	[TierId] [char](1) NOT NULL,
	[TierName] [nchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_TeacherTier] PRIMARY KEY CLUSTERED 
(
	[TierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionHistory]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionHistory](
	[TransactionId] [char](10) NOT NULL,
	[Amount] [float] NOT NULL,
	[MethodId] [char](1) NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_TransactionHistory] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 5/22/2025 1:07:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [char](10) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[RoleId] [char](1) NOT NULL,
	[ResetCode] [nchar](100) NULL,
	[RefreshToken] [nvarchar](512) NULL,
	[RefreshTokenExpiryTime] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (N'1', N'admin')
INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (N'2', N'student')
INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (N'3', N'teacher')
GO
INSERT [dbo].[StudentTier] ([TierId], [TierName], [Description]) VALUES (N'1', N'Basic                                             ', N'Basic Description')
INSERT [dbo].[StudentTier] ([TierId], [TierName], [Description]) VALUES (N'2', N'Premium                                           ', N'Premium Description')
GO
INSERT [dbo].[TeacherTier] ([TierId], [TierName], [Description]) VALUES (N'1', N'Basic                                             ', N'Basic Description')
INSERT [dbo].[TeacherTier] ([TierId], [TierName], [Description]) VALUES (N'2', N'Premium                                           ', N'Premium Description')
GO
INSERT [dbo].[User] ([UserId], [Email], [Password], [CreatedDate], [RoleId], [ResetCode], [RefreshToken], [RefreshTokenExpiryTime], [IsActive]) VALUES (N'HU0       ', N'abc@gmail.com', N'83d4536114b0c317a3e63709a2ad844b1b28981f10c8fecc4dfab7299567c145', CAST(N'2025-05-22T00:00:00.000' AS DateTime), N'2', NULL, N'2eJ+ugPdD1LGt9BI+5n/mf+nInJCbOhgCgW3G7Ox+jI=', CAST(N'2025-05-29T06:03:15.243' AS DateTime), 1)
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Level] FOREIGN KEY([LevelId])
REFERENCES [dbo].[Level] ([LevelId])
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Level]
GO
ALTER TABLE [dbo].[Minigame]  WITH CHECK ADD  CONSTRAINT [FK_Minigame_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([CourseId])
GO
ALTER TABLE [dbo].[Minigame] CHECK CONSTRAINT [FK_Minigame_Course]
GO
ALTER TABLE [dbo].[Minigame]  WITH CHECK ADD  CONSTRAINT [FK_Minigame_MinigameTemplate] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[MinigameTemplate] ([TemplateId])
GO
ALTER TABLE [dbo].[Minigame] CHECK CONSTRAINT [FK_Minigame_MinigameTemplate]
GO
ALTER TABLE [dbo].[Minigame]  WITH CHECK ADD  CONSTRAINT [FK_Minigame_Teacher] FOREIGN KEY([TeacherId])
REFERENCES [dbo].[Teacher] ([UserId])
GO
ALTER TABLE [dbo].[Minigame] CHECK CONSTRAINT [FK_Minigame_Teacher]
GO
ALTER TABLE [dbo].[Rating]  WITH CHECK ADD  CONSTRAINT [FK_Rating_Minigame] FOREIGN KEY([MinigameId])
REFERENCES [dbo].[Minigame] ([MinigameId])
GO
ALTER TABLE [dbo].[Rating] CHECK CONSTRAINT [FK_Rating_Minigame]
GO
ALTER TABLE [dbo].[Rating]  WITH CHECK ADD  CONSTRAINT [FK_Rating_Student] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([UserId])
GO
ALTER TABLE [dbo].[Rating] CHECK CONSTRAINT [FK_Rating_Student]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_StudentTier] FOREIGN KEY([TierId])
REFERENCES [dbo].[StudentTier] ([TierId])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_StudentTier]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_User]
GO
ALTER TABLE [dbo].[StudentAccomplisment]  WITH CHECK ADD  CONSTRAINT [FK_StudentAccomplisment_Minigame] FOREIGN KEY([MinigameId])
REFERENCES [dbo].[Minigame] ([MinigameId])
GO
ALTER TABLE [dbo].[StudentAccomplisment] CHECK CONSTRAINT [FK_StudentAccomplisment_Minigame]
GO
ALTER TABLE [dbo].[StudentAccomplisment]  WITH CHECK ADD  CONSTRAINT [FK_StudentAccomplisment_Student] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([UserId])
GO
ALTER TABLE [dbo].[StudentAccomplisment] CHECK CONSTRAINT [FK_StudentAccomplisment_Student]
GO
ALTER TABLE [dbo].[StudentSupscription]  WITH CHECK ADD  CONSTRAINT [FK_StudentSupscription_Student] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([UserId])
GO
ALTER TABLE [dbo].[StudentSupscription] CHECK CONSTRAINT [FK_StudentSupscription_Student]
GO
ALTER TABLE [dbo].[StudentSupscription]  WITH CHECK ADD  CONSTRAINT [FK_StudentSupscription_StudentTier] FOREIGN KEY([TierId])
REFERENCES [dbo].[StudentTier] ([TierId])
GO
ALTER TABLE [dbo].[StudentSupscription] CHECK CONSTRAINT [FK_StudentSupscription_StudentTier]
GO
ALTER TABLE [dbo].[SupscriptionExtentionOrder]  WITH CHECK ADD  CONSTRAINT [FK_SupscriptionExtentionOrder_StudentSupscription] FOREIGN KEY([UserId], [TierId])
REFERENCES [dbo].[StudentSupscription] ([StudentId], [TierId])
GO
ALTER TABLE [dbo].[SupscriptionExtentionOrder] CHECK CONSTRAINT [FK_SupscriptionExtentionOrder_StudentSupscription]
GO
ALTER TABLE [dbo].[SupscriptionExtentionOrder]  WITH CHECK ADD  CONSTRAINT [FK_SupscriptionExtentionOrder_TeacherSupscription] FOREIGN KEY([UserId], [TierId])
REFERENCES [dbo].[TeacherSupscription] ([TeacherId], [TierId])
GO
ALTER TABLE [dbo].[SupscriptionExtentionOrder] CHECK CONSTRAINT [FK_SupscriptionExtentionOrder_TeacherSupscription]
GO
ALTER TABLE [dbo].[SupscriptionExtentionOrder]  WITH CHECK ADD  CONSTRAINT [FK_SupscriptionExtentionOrder_TransactionHistory] FOREIGN KEY([TransactionId])
REFERENCES [dbo].[TransactionHistory] ([TransactionId])
GO
ALTER TABLE [dbo].[SupscriptionExtentionOrder] CHECK CONSTRAINT [FK_SupscriptionExtentionOrder_TransactionHistory]
GO
ALTER TABLE [dbo].[Teacher]  WITH CHECK ADD  CONSTRAINT [FK_Teacher_TeacherTier] FOREIGN KEY([TierId])
REFERENCES [dbo].[TeacherTier] ([TierId])
GO
ALTER TABLE [dbo].[Teacher] CHECK CONSTRAINT [FK_Teacher_TeacherTier]
GO
ALTER TABLE [dbo].[Teacher]  WITH CHECK ADD  CONSTRAINT [FK_Teacher_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Teacher] CHECK CONSTRAINT [FK_Teacher_User]
GO
ALTER TABLE [dbo].[TeacherSupscription]  WITH CHECK ADD  CONSTRAINT [FK_TeacherSupscription_Teacher] FOREIGN KEY([TeacherId])
REFERENCES [dbo].[Teacher] ([UserId])
GO
ALTER TABLE [dbo].[TeacherSupscription] CHECK CONSTRAINT [FK_TeacherSupscription_Teacher]
GO
ALTER TABLE [dbo].[TeacherSupscription]  WITH CHECK ADD  CONSTRAINT [FK_TeacherSupscription_TeacherTier] FOREIGN KEY([TierId])
REFERENCES [dbo].[TeacherTier] ([TierId])
GO
ALTER TABLE [dbo].[TeacherSupscription] CHECK CONSTRAINT [FK_TeacherSupscription_TeacherTier]
GO
ALTER TABLE [dbo].[TransactionHistory]  WITH CHECK ADD  CONSTRAINT [FK_TransactionHistory_PaymentMethod] FOREIGN KEY([MethodId])
REFERENCES [dbo].[PaymentMethod] ([MethodId])
GO
ALTER TABLE [dbo].[TransactionHistory] CHECK CONSTRAINT [FK_TransactionHistory_PaymentMethod]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
USE [master]
GO
ALTER DATABASE [HybridDB] SET  READ_WRITE 
GO
