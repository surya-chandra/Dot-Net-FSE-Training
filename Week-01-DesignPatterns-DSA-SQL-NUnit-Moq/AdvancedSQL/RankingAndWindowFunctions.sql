
CREATE TABLE Employees
(
    EmployeeId INT PRIMARY KEY,
    EmployeeName VARCHAR(100),
    Department VARCHAR(50),
    Salary DECIMAL(10,2)
);

INSERT INTO Employees VALUES
(1,'John','IT',50000),
(2,'David','IT',70000),
(3,'Smith','HR',60000),
(4,'Alice','HR',80000);

-- RANK()

SELECT
    EmployeeName,
    Salary,
    RANK() OVER(ORDER BY Salary DESC) AS SalaryRank
FROM Employees;

-- ROW_NUMBER()

SELECT
    EmployeeName,
    Salary,
    ROW_NUMBER() OVER(ORDER BY Salary DESC) AS RowNumber
FROM Employees;


SELECT
    EmployeeName,
    Salary,
    DENSE_RANK() OVER(ORDER BY Salary DESC) AS DenseRank
FROM Employees;


SELECT
    EmployeeName,
    Department,
    Salary,
    RANK() OVER
    (
        PARTITION BY Department
        ORDER BY Salary DESC
    ) AS DepartmentRank
FROM Employees;