CREATE PROCEDURE [dbo].[spTruncateTable]
	@TableName VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @reseed int = CASE WHEN (IDENT_CURRENT(@TableName) = 1) THEN 1 ELSE 0 END;
		DECLARE @Sql NVARCHAR(MAX) = 'DELETE FROM ' + @TableName;
		EXECUTE sp_executesql @Sql;

		DBCC CHECKIDENT (@TableName, RESEED, @reseed);
	
		COMMIT TRAN
	END TRY
	BEGIN CATCH

		IF @@TRANCOUNT > 0
			ROLLBACK TRAN 

		DECLARE @ErrorMessage NVARCHAR(4000);  
		DECLARE @ErrorSeverity INT;  
		DECLARE @ErrorState INT;  

		SELECT   @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();  
		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);  

	END CATCH
END
GO
