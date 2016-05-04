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
	[ProfilePicturePath] [nvarchar](MAX) NULL,
	Role [nvarchar](50) NULL
)

GO

CREATE TABLE [dbo].[Authentications](
	[Token] [nvarchar](150) NOT NULL PRIMARY KEY,
	[UserId] [int] NOT NULL  FOREIGN KEY REFERENCES Users(Id),
	[LastSeen] [datetime] NOT NULL)
	
CREATE TABLE [dbo].[Feeds]
(
	[Id] [int] NOT NULL PRIMARY KEY IDENTITY,
	[Text] [nvarchar](MAX) NOT NULL,
	[AuthorId] [int] NOT NULL,
	[CreatedAt] DATETIME NOT NULL
)

GO	

CREATE TABLE [dbo].[Creations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[FilePath] [nvarchar](max) NOT NULL,
	[Signiture] [nvarchar](max) NOT NULL,
	[CreationFootprint] [nvarchar](max) NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Creations]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

CREATE TABLE [dbo].[Groups]
(
	[Id] [int] NOT NULL PRIMARY KEY IDENTITY,
	[Name] [nvarchar](MAX) NOT NULL,
	[AuthorId] [int] NOT NULL
)

GO	

CREATE TABLE [dbo].[GroupCreations]
(
	[Id] [int] NOT NULL PRIMARY KEY IDENTITY,
	[GroupId] [int] NOT NULL FOREIGN KEY(Id) REFERENCES Groups(Id),
	[CreationId] [int] NOT NULL FOREIGN KEY(Id) REFERENCES Creations(Id)
)

GO

CREATE TABLE [dbo].[GroupMessages]
(
	[Id] [int] NOT NULL PRIMARY KEY IDENTITY,
	[GroupId] [int] NOT NULL FOREIGN KEY(Id) REFERENCES Groups(Id),
	[UserId] [int] NOT NULL FOREIGN KEY(Id) REFERENCES Users(Id),
	[Message] [nvarchar](MAX) NOT NULL,
	[SendDate] DATETIME NOT NULL
)

GO

CREATE TABLE [dbo].[UserGroups]
(
	[Id] [int] NOT NULL PRIMARY KEY IDENTITY,
	[GroupId] [int] NOT NULL FOREIGN KEY(Id) REFERENCES Groups(Id),
	[UserId] [int] NOT NULL FOREIGN KEY(Id) REFERENCES Users(Id),
	[IsAdmin] [bit] NOT NULL
)

GO