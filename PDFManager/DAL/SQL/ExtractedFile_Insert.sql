-- CREATED BY: Nathan Townsend
-- CREATED DATE: 10/29/2014
-- DO NOT MODIFY THIS CODE
-- CHANGES WILL BE LOST WHEN THE GENERATOR IS RUN AGAIN
-- GENERATION TOOL: Dalapi Code Generator (DalapiPro.com)



USE [PDFManager]

-- Drop the procedure if it exists.
If OBJECT_ID('[dbo].[ExtractedFile_Insert]') IS NOT NULL
    BEGIN
    DROP PROCEDURE [dbo].[ExtractedFile_Insert]
    END
GO

CREATE PROCEDURE [dbo].[ExtractedFile_Insert]
    @ParentFileId Int = null,
    @FileName VarChar(500),
    @FilePath VarChar(MAX),
    @Extracted Bit
AS

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    INSERT INTO [dbo].[ExtractedFile]
	(
        [ParentFileId],
        [FileName],
        [FilePath],
        [Extracted]
    ) VALUES (
        @ParentFileId,
        @FileName,
        @FilePath,
        @Extracted
	)

	-- return the new identity value
	SELECT SCOPE_IDENTITY()

END