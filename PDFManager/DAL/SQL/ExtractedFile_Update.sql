-- CREATED BY: Nathan Townsend
-- CREATED DATE: 10/29/2014
-- DO NOT MODIFY THIS CODE
-- CHANGES WILL BE LOST WHEN THE GENERATOR IS RUN AGAIN
-- GENERATION TOOL: Dalapi Code Generator (DalapiPro.com)



USE [PDFManager]

-- Drop the procedure if it exists.
If OBJECT_ID('[dbo].[ExtractedFile_Update]') IS NOT NULL
    BEGIN
    DROP PROCEDURE [dbo].[ExtractedFile_Update]
    END
GO

CREATE PROCEDURE [dbo].[ExtractedFile_Update]
    @ExtractedFileId Int,
    @ParentFileId Int = null,
    @FileName VarChar(500),
    @FilePath VarChar(MAX),
    @Extracted Bit
AS

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    UPDATE [dbo].[ExtractedFile]
    SET
        [ParentFileId] = @ParentFileId,
        [FileName] = @FileName,
        [FilePath] = @FilePath,
        [Extracted] = @Extracted
    WHERE
        [ExtractedFileId] = @ExtractedFileId

    SELECT @@ROWCOUNT AS UPDATED; 
END