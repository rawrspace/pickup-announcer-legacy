CREATE FUNCTION [dbo].[STRING_SPLIT]
(
    @string    nvarchar(MAX), 
    @separator nvarchar(MAX)
)
RETURNS TABLE WITH SCHEMABINDING 
AS RETURN
   WITH X(N) AS (SELECT 'Table1' FROM (VALUES (0),(0),(0),(0),(0),(0),(0),(0),(0),(0),(0),(0),(0),(0),(0),(0)) T(C)),
        Y(N) AS (SELECT 'Table2' FROM X A1, X A2, X A3, X A4, X A5, X A6, X A7, X A8) , -- Up to 16^8 = 4 billion
        T(N) AS (SELECT TOP(ISNULL(LEN(@string),0)) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) -1 N FROM Y),
        Delim(Pos) AS (SELECT t.N FROM T WHERE (SUBSTRING(@string, t.N, LEN(@separator+'x')-1) LIKE @separator OR t.N = 0)),
        Separated(value) AS (SELECT SUBSTRING(@string, d.Pos + LEN(@separator+'x')-1, LEAD(d.Pos,1,2147483647) OVER (ORDER BY (SELECT NULL)) - d.Pos - LEN(@separator)) 
                               FROM Delim d
                              WHERE @string IS NOT NULL)
       SELECT s.value
         FROM Separated s
        WHERE s.value <> @separator
GO