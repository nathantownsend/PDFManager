-- CREATED BY: Nathan Townsend
-- CREATED DATE: 10/29/2014
-- DO NOT MODIFY THIS CODE
-- CHANGES WILL BE LOST WHEN THE GENERATOR IS RUN AGAIN
-- GENERATION TOOL: Dalapi Code Generator (DalapiPro.com)



USE [PDFManager]

-- Drop the procedure if it exists.
If OBJECT_ID('[dbo].[ExtractedFile_Delete]') IS NOT NULL
    BEGIN
    DROP PROCEDURE [dbo].[ExtractedFile_Delete]
    END
GO

CREATE PROCEDURE [dbo].[ExtractedFile_Delete]
    @ExtractedFileId Int
AS

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DELETE 
        [dbo].[ExtractedFile]
    WHERE
        [ExtractedFileId] = @ExtractedFileId

    SELECT @@ROWCOUNT AS DELETED; 
END