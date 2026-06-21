-- ==========================================
-- SQL Index Example
-- ==========================================

CREATE TABLE Employees
(
    EmployeeId INT PRIMARY KEY,
    EmployeeName VARCHAR(100),
    Department VARCHAR(50),
    Salary DECIMAL(10,2)
);

CREATE INDEX IX_EmployeeName
ON Employees(EmployeeName);

SELECT *
FROM Employees
WHERE EmployeeName = 'John';