CREATE PROC usp_ExcludeFromSchool(@StudentId INT)
AS
BEGIN
	DECLARE @targetStudentId INT = (SELECT s.Id 
									FROM Students AS s
									WHERE s.Id = @StudentId)

	IF(@targetStudentId IS NULL)
	BEGIN
		RAISERROR('This school has no student with the provided id!', 16, 2)
		RETURN
	END

	DELETE FROM StudentsTeachers
	WHERE StudentId = @StudentId

	DELETE FROM StudentsExams
	WHERE StudentId = @StudentId

	DELETE FROM StudentsSubjects
	WHERE StudentId = @StudentId

	DELETE FROM Students
	WHERE Id = @StudentId
END