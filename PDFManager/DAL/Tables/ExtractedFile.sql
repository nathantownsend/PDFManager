/****** Object:  Table [dbo].[ExtractedFile]    Script Date: 10/29/2014 10:47:13 PM ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[ExtractedFile](
	[ExtractedFileId] [int] IDENTITY(1,1) NOT NULL,
	[ParentFileId] [int] NULL,
	[FileName] [varchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[FilePath] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Extracted] [bit] NOT NULL,
 CONSTRAINT [PK_ExtractedFile] PRIMARY KEY CLUSTERED 
(
	[ExtractedFileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

