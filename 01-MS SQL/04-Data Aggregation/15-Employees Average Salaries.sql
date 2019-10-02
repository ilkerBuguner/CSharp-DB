SELECT * INTO NewEmployeeTable
 FROM Employees
 WHERE Salary > 30000
 DELETE FROM NewEmployeeTable
 WHERE ManagerID = 42
 UPDATE NewEmployeeTable
 SET Salary += 5000
 WHERE DepartmentID = 1

 SELECT DepartmentID, AVG(Salary)
  FROM NewEmployeeTable
  GROUP BY DepartmentID