
DROP DATABASE GummyBears

CREATE DATABASE GummyBears

GO
 
USE GummyBears

CREATE TABLE [dbo].[Users]
(
	[Id] [int] NOT NULL PRIMARY KEY IDENTITY,
	[UserName] [nvarchar](100) NOT NULL UNIQUE,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL UNIQUE,
	[TelephoneNumber] [nvarchar](100) NULL,
	[DateOfBirth] DATETIME NULL,
	[Description] [nvarchar](MAX) NULL,
	[Country] [nvarchar](100) NULL,
	[ProfilePicturePath] [nvarchar](MAX) NULL,
	[Role] [nvarchar](50) NULL
)

GO

CREATE TABLE [dbo].[Authentications](
	[Token] [nvarchar](150) NOT NULL PRIMARY KEY,
	[UserId] [int] NOT NULL  FOREIGN KEY REFERENCES Users(Id),
	[LastSeen] [datetime] NOT NULL)

GO

CREATE TABLE [dbo].[Feeds](
	[id] [int] NOT NULL PRIMARY KEY IDENTITY,
	[text] [ntext] NOT NULL,
	[author_id] [int] NOT NULL,
	[created_at] [datetime] NOT NULL)
	
GO