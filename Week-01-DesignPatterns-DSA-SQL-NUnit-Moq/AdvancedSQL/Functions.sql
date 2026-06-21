CREATE FUNCTION dbo.GetBonus
(
    @Salary DECIMAL(10,2)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @Bonus DECIMAL(10,2);

    SET @Bonus = @Salary * 0.10;

    RETURN @Bonus;
END;
GO

-- Example Usage

SELECT dbo.GetBonus(50000) AS BonusAmount;
GO