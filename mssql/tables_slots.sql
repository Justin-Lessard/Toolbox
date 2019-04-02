INSERT INTO [dbo].[Work_Table] ([Name]) VALUES ('New Data');
INSERT INTO [dbo].[Work_Table] ([Name]) VALUES ('New Data');
-- ...

BEGIN TRANSACTION
	TRUNCATE TABLE [dbo].[Prod_Table];
	ALTER TABLE [dbo].[Work_Table] SWITCH TO [dbo].[Prod_Table];
COMMIT TRANSACTION;
