CREATE TABLE #test (ID VARCHAR(MAX), Result VARCHAR(MAX));
CREATE TABLE #test2 (ID VARCHAR(MAX));
GO
--------------------------------------------
INSERT INTO #test
(ID)
VALUES
	('ABCD')
	,('FGHI');
INSERT INTO #test2
(ID)
VALUES
	('ABCD')
	,('1234');
GO
--------------------------------------------
MERGE #test target
USING (SELECT Id FROM #test2)  AS source (Id)
ON target.Id = source.Id
WHEN MATCHED THEN
	UPDATE SET Result = 'Matched'
WHEN NOT MATCHED BY TARGET THEN
	INSERT (ID, Result)
	VALUES (source.Id, 'NOT MATCHED BY TARGET')
WHEN NOT MATCHED BY SOURCE THEN
	DELETE
;
GO
--------------------------------------------
SELECT * FROM #test
SELECT * FROM #test2
GO
---------------------------------------------
DROP TABLE #test
DROP TABLE #test2
GO
