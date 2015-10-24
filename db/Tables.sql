CREATE DATABASE GummyBears

GO

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
	[ProfilePicturePath] [nvarchar](MAX) NULL
)

GO