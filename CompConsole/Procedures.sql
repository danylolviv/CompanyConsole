-- noinspection SqlNoDataSourceInspectionForFile

-- noinspection SqlDialectInspectionForFile

ALTER Procedure [dbo].[usp_CreateDepartment](@DName varchar(50), @MgrSSN numeric(9,0))
AS
BEGIN
	IF NOT EXISTS (SELECT Employee.SSN FROM Employee WHERE Employee.SSN = @MgrSSN)
		THROW 50001, 'Employee does not exist',1

	IF EXISTS (SELECT Department.MgrSSN FROM Department WHERE Department.MgrSSN = @MgrSSN)
		THROW 50001, 'Employee Is already a manager',1

	DECLARE @highestId INT = (SELECT MAX(DNumber) FROM Department);
	INSERT INTO Department (DName, MgrSSN, MgrStartDate, DNumber) VALUES (@DName, @MgrSSN, GETDATE(), @highestId + 1)
	RETURN SELECT MAX(DNumber) FROM Department
END



ALTER Procedure [dbo].[usp_DeleteDepartment](@DNumber INT)
AS
BEGIN
		SET NOCOUNT ON;
    
    -- Check if department exists
    IF NOT EXISTS (SELECT * FROM Department WHERE DNumber = @DNumber)
    	THROW 50001, 'Department does not exist',1

    BEGIN TRY
        -- Delete from Dept_Locations
        DELETE FROM Dept_Locations WHERE DNumber = @DNumber;

        -- Delete related projects from Project
        DELETE FROM Project WHERE DNum = @DNumber;

        -- Delete related tuples from Works_on
        DELETE FROM Works_on WHERE Essn IN (SELECT SSN FROM Employee WHERE Dno = @DNumber);

        -- Update Employee table
        UPDATE Employee SET Dno = NULL WHERE Dno = @DNumber;

        -- Delete department
        DELETE FROM Department WHERE DNumber = @DNumber;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END


ALTER Procedure [dbo].[usp_GetAllDepartments]
AS
BEGIN
		SELECT Department.DNumber, Department.DName, Department.MgrSSN, Department.MgrStartDate, COUNT(Employee.SSN) AS EmpCount
		FROM Department LEFT JOIN Employee ON Department.DNumber = Employee.DNo
		GROUP BY Department.DNumber, Department.DName, Department.MgrSSN, Department.MgrStartDate
		ORDER BY Department.DNumber;
END

ALTER Procedure [dbo].[usp_GetDepartment](@DNumber INT)
AS
BEGIN
		SELECT D.DNumber, D.DName, D.MgrSSN, COUNT(E.SSN) AS EmpCount
		FROM DEPARTMENT D
		LEFT JOIN EMPLOYEE E ON D.DNumber = E.DNo
		WHERE D.DNumber = @DNumber
		GROUP BY D.DNumber, D.DName, D.MgrSSN;
END

ALTER Procedure [dbo].[usp_UpdateDepartmentManager](@DNumber INT, @MgrSSN numeric(9,0))
AS
BEGIN
	IF EXISTS (SELECT Department.DNumber FROM Department WHERE Department.MgrSSN = @MgrSSN)
		THROW 50001, 'MgrSSN is already in use',1

		UPDATE Department SET MgrSSN = @MgrSSN, MgrStartDate = GETDATE() WHERE Department.DNumber = @DNumber;
		UPDATE Employee SET SuperSSN = @MgrSSN WHERE Employee.SuperSSN = @MgrSSN;
END

ALTER Procedure [dbo].[usp_UpdateDepartmentName](@DNumber INT, @DName varchar(50))
AS
BEGIN
	IF EXISTS (SELECT Department.DNumber FROM Department WHERE Department.DName = @DName)
		THROW 50001, 'Department name is already in use',1

		UPDATE Department SET DName = @DName WHERE Department.DNumber = @DNumber;
END