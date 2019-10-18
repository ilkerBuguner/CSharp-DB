SELECT s.FirstName, s.LastName, COUNT(st.TeacherId) FROM Students AS s
JOIN StudentsTeachers AS st ON s.Id = st.StudentId
GROUP BY s.FirstName, s.LastName
