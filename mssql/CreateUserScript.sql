DECLARE @CREATE_LOGIN_SQL	AS VARCHAR(max);
DECLARE @CREATE_USER_SQL	AS VARCHAR(max);
DECLARE @DATABASE			AS VARCHAR(50);
DECLARE @LOGIN				AS VARCHAR(50);
DECLARE @PASSWORD			AS VARCHAR(64);
--------------------------------------------------------------
-- Set your variable below
--------------------------------------------------------------
SET @DATABASE	= 'YourDB';
SET @LOGIN		= 'Username';
SET @PASSWORD	= 'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx';
--------------------------------------------------------------

SET @CREATE_LOGIN_SQL = '
CREATE LOGIN [{LOGIN}] WITH PASSWORD = ''{PASSWORD}''
';
SET @CREATE_USER_SQL = '
USE {DATABASE};

CREATE USER [{LOGIN}] FOR LOGIN [{LOGIN}];
EXEC sp_addrolemember ''db_datareader'', ''{LOGIN}'';
EXEC sp_addrolemember ''db_datawriter'', ''{LOGIN}'';
--EXEC sp_addrolemember ''db_owner'', ''{LOGIN}'';
';

SET @CREATE_LOGIN_SQL = REPLACE(@CREATE_LOGIN_SQL, '{LOGIN}', @LOGIN);
SET @CREATE_LOGIN_SQL = REPLACE(@CREATE_LOGIN_SQL, '{PASSWORD}', @PASSWORD);
SET @CREATE_USER_SQL = REPLACE(@CREATE_USER_SQL, '{DATABASE}', @DATABASE);
SET @CREATE_USER_SQL = REPLACE(@CREATE_USER_SQL, '{LOGIN}', @LOGIN);

EXECUTE (@CREATE_LOGIN_SQL)
EXECUTE (@CREATE_USER_SQL)
