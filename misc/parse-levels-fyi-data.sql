-- URL: https://www.levels.fyi/js/salaryData.json


ALTER FUNCTION dbo.ParseSalary(@str varchar(50))
RETURNS decimal(18,2) AS
BEGIN
	DECLARE @value decimal = ISNULL(TRY_PARSE(@str AS decimal(18,2)), 0);

    RETURN 
	CASE 
		WHEN @value <= 999 THEN @value * 1000
		ELSE @value END
END
-----------------------------------------------------------------------------------
SELECT 
	Id								
	,Company							
	,Title							
	,Level							
	,dbo.ParseSalary(BaseSalary						) AS BaseSalary
	,dbo.ParseSalary(StockGrantValue				) AS StockGrantValue	
	,dbo.ParseSalary(Bonus							) AS Bonus
	,dbo.ParseSalary(TotalYearlyCompensation		) AS TotalYearlyCompensation	
	,TRY_PARSE(YearsOfExperience				AS DECIMAL(18,2)) AS YearsOfExperience
	,TRY_PARSE(YearsAtCompany					AS DECIMAL(18,2)) AS YearsAtCompany
	,Location						
	,Timestamp						
	,Tag								
	,Gender							
	,Notes							
	,CityId							
	--,Dmaid		
FROM 
	OPENROWSET (BULK 'C:\Users\JLessard\Downloads\salaryData.json', SINGLE_CLOB) AS [Json]
	CROSS APPLY OPENJSON ( BulkColumn, '$' )
    WITH  (
            Id								int					'$.rowNumber',
            Company							varchar(50)			'$.company', 
            Title							varchar(100)		'$.title',
            Level							varchar(100)		'$.level',
			      BaseSalary						varchar(50)			'$.basesalary',
            StockGrantValue					varchar(50)			'$.stockgrantvalue',
            Bonus							varchar(50)			'$.bonus',
            TotalYearlyCompensation			varchar(50)			'$.totalyearlycompensation',
            YearsOfExperience				varchar(50)			'$.yearsofexperience',
            YearsAtCompany					varchar(50)			'$.yearsatcompany',
            Location						varchar(100)		'$.location',
            Timestamp						DATETIME2			'$.timestamp',  
            Tag								varchar(100)		'$.tag',
            Gender							varchar(100)		'$.gender',
            Notes							varchar(100)		'$.otherdetails',
            CityId							int					'$.cityid',
            Dmaid							int					'$.dmaid'
        ) AS [Salaries]
