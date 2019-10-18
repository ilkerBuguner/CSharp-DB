DELETE FROM StudentsTeachers
WHERE TeacherId IN (
					SELECT t.Id
					FROM Teachers AS t
					WHERE Phone LIKE '%72%')

DELETE FROM Teachers
WHERE Phone LIKE '%72%'