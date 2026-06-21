-- ==========================================
-- Create Employee Table
-- ==========================================

CREATE TABLE Employees
(
    EmployeeId INT PRIMARY KEY,
    EmployeeName VARCHAR(100),
    Department VARCHAR(50),
    Salary DECIMAL(10,2)
);
GO


INSERT INTO Employees VALUES
(1,'John','IT',50000),
(2,'David','IT',70000),
(3,'Smith','HR',60000);
GO


CREATE PROCEDURE GetEmployees
AS
BEGIN
    SELECT *
    FROM Employees;
END;
GO

EXEC GetEmployees;
GO