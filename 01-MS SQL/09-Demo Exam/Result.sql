--2--
INSERT INTO Files ([Name], Size, ParentId, CommitId) VALUES
('Trade.idk', 2598.0, 1, 1),
('menu.net', 9238.31, 2, 2),
('Administrate.soshy', 1246.93, 3, 3),
('Controller.php', 7353.15, 4, 4),
('Find.java', 9957.86, 5, 5),
('Controller.json', 14034.87, 3, 6),
('Operate.xix', 7662.92, 7, 7)

INSERT INTO Issues (Title, IssueStatus, RepositoryId, AssigneeId) VALUES
('Critical Problem with HomeController.cs file', 'open', 1, 4),
('Typo fix in Judge.html', 'open', 4, 3),
('Implement documentation for UsersService.cs', 'closed', 8, 2),
('Unreachable code in Index.cs', 'open', 9, 8)

--3--
UPDATE Issues
SET IssueStatus = 'closed'
WHERE AssigneeId = 6

--4--
DELETE FROM RepositoriesContributors
WHERE RepositoryId IN (
					   SELECT Id 
					   FROM Repositories 
					   WHERE [Name] = 'Softuni-Teamwork')

DELETE FROM Issues
WHERE RepositoryId IN (
					   SELECT Id 
					   FROM Repositories 
					   WHERE [Name] = 'Softuni-Teamwork')

--5--
SELECT c.Id, c.[Message], c.RepositoryId, c.ContributorId FROM Commits AS c
ORDER BY c.Id, c.[Message], c.RepositoryId, c.ContributorId

--6--
SELECT f.Id, f.[Name], f.Size FROM Files AS f
WHERE f.Size > 1000 AND f.[Name] LIKE '%html%'
ORDER BY f.Size DESC, f.[Name]

--7--
SELECT i.Id, CONCAT(u.Username, ' : ', i.Title) AS [IssueAssignee] FROM Issues AS i
JOIN Users AS u ON i.AssigneeId = u.Id
ORDER BY i.Id DESC, i.AssigneeId

--8--
SELECT fParent.Id, fParent.[Name], CONCAT(CAST(fParent.Size AS VARCHAR(40)), 'KB') AS 'Size' FROM Files AS fParent
LEFT JOIN Files AS fChild ON fParent.Id = fChild.ParentId
WHERE fChild.ParentId IS NULL
ORDER BY fParent.Id, fParent.[Name], fParent.Size DESC

--9--
SELECT  TOP(5) r.Id, r.[Name], COUNT(rc.ContributorId) AS [Commits] FROM Repositories AS r
JOIN Commits AS c ON c.RepositoryId = r.Id
JOIN RepositoriesContributors AS rc ON rc.RepositoryId = r.Id
GROUP BY r.Id, r.[Name]
ORDER BY COUNT(rc.ContributorId) DESC, r.Id, r.[Name]


--10--
SELECT u.Username, AVG(f.Size) FROM Users AS u
LEFT JOIN Commits AS c ON c.ContributorId = u.Id
JOIN Files AS f ON f.CommitId = c.Id
WHERE c.Id IS NOT NULL
GROUP BY u.Username
ORDER BY AVG(f.Size) DESC, u.Username

--11--
CREATE FUNCTION udf_UserTotalCommits(@username NVARCHAR(100))
RETURNS INT
AS
BEGIN
	 DECLARE @countOfCommits INT = (SELECT TOP(1) COUNT(c.Id)
									FROM Users AS u
									JOIN Commits AS c ON c.ContributorId = u.Id
									WHERE u.Username = @username)

	 RETURN @countOfCommits
END

--12--
CREATE PROC usp_FindByExtension(@extension NVARCHAR(100))
AS
BEGIN
	SELECT f.Id, f.[Name], CONCAT(CAST(f.[Size] AS NVARCHAR(50)), 'KB') AS [Size] 
	FROM Files AS f
	WHERE f.[Name] LIKE '%' + @extension
	ORDER BY f.Id, f.[Name], Size DESC

END

