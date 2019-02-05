WITH CTE (col1, col2, CPT)
AS
(
  SELECT 
    col1, 
    col2,
    ROW_NUMBER() OVER (PARTITION BY col1, col2 ORDER BY col1) AS CPT
  FROM 
    [dbo].[Table]
)
DELETE FROM CTE
WHERE CPT > 1
GO
