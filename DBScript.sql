USE [ContactDB]
GO

/****** Object:  Table [dbo].[Contact]    Script Date: 24/2/2021 6:33:42 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Contact](
	[ContactID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[PhoneNo] [varchar](50) NULL,
	[Address] [varchar](150) NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[ContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
GO

/****** Object:  StoredProcedure [dbo].[ContactViewAllOrSearch]    Script Date: 24/2/2021 6:32:25 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[ContactViewAllOrSearch]
@SearchText varchar(50)
AS BEGIN
SELECT * FROM Contact WHERE @SearchText='' OR NAME LIKE '%' +@SearchText+ '%'
END
GO
GO

/****** Object:  StoredProcedure [dbo].[ContactAddOrEdit]    Script Date: 24/2/2021 6:31:49 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[ContactAddOrEdit]
@ContactID int,
@Name varchar(50),
@PhoneNo varchar(50),
@Address varchar(250)
AS
BEGIN
IF(@ContactID = 0)
BEGIN
	INSERT INTO Contact
	(
		Name,
		PhoneNo,
		Address
	)
	VALUES
	(
		@Name,
		@PhoneNo,
		@Address
	)
END
ELSE
BEGIN
	UPDATE Contact
	SET
	Name = @Name,
	PhoneNo = @PhoneNo,
	Address = @Address
	WHERE ContactID = @ContactID
END
END
GO
GO

/****** Object:  StoredProcedure [dbo].[ContactDeleteByID]    Script Date: 24/2/2021 6:32:17 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[ContactDeleteByID]
@ContactID int
AS BEGIN
DELETE FROM Contact
WHERE ContactID = @ContactID
END
GO