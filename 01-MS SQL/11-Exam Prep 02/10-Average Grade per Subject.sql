SELECT s.[Name], AVG(ss.Grade) FROM Subjects AS s
JOIN StudentsSubjects AS ss ON ss.SubjectId = s.Id
GROUP BY s.[Name], s.Id
ORDER BY s.Id

