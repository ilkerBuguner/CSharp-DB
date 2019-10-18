CREATE FUNCTION udf_ExamGradesToUpdate(@studentId INT, @grade DECIMAL(15,2))
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @targetStudentName NVARCHAR(100) = (SELECT s.FirstName
												FROM Students AS s
												WHERE s.Id = @studentId)

	IF(@targetStudentName IS NULL)
	BEGIN
		RETURN 'The student with provided id does not exist in the school!';
	END

	IF(@grade > 6.00)
	BEGIN
		RETURN 'Grade cannot be above 6.00!';
	END

	DECLARE @countOfGrades INT = (SELECT COUNT(*) 
								  FROM Students AS s
								  JOIN StudentsExams AS se ON s.Id = se.StudentId
								  WHERE s.Id = @studentId AND (se.Grade > @grade AND se.Grade <= @grade + 0.50))

	RETURN CONCAT('You have to update ',@countOfGrades,' grades for the student ', @targetStudentName)
END