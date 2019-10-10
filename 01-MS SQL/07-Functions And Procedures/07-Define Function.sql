CREATE FUNCTION ufn_IsWordComprised(@setOfLetters NVARCHAR(MAX), @word NVARCHAR(MAX)) 
RETURNS BIT
AS
BEGIN
	DECLARE @count INT = 1
	DECLARE @result INT

	WHILE @count <= LEN(@word)
	BEGIN
		DECLARE @currentChar NVARCHAR(10) = SUBSTRING(@word, @count, 1)

		IF(CHARINDEX(@currentChar, @setOfLetters) > 0)
			BEGIN
				SET @count += 1
				SET @result = 1
			END
		ELSE
			BEGIN
				SET @result = 0
				BREAK
			END
	END

	RETURN @result

END