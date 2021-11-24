CREATE OR ALTER PROCEDURE [dbo].[GetActivities] (@Date Date)
AS
BEGIN
	IF OBJECT_ID('tempdb..#tempData') IS NOT NULL DROP TABLE #tempData

	Select * into #tempData from(
	SELECT 
		   a.name + ' Shift' as Name, 
		   3 as [Priority], 
		   e.name as Employee, 
		   s.StartDateTime, 
		   s.EndDateTime
	FROM schedule.Shifts s
			INNER JOIN dbo.Activities a 
				ON a.id = s.ActivityID
			INNER JOIN Employees e 
				ON e.id = s.EmployeeID
			WHERE 
			CAST(s.StartDateTime as Date) = @Date OR 
			CAST(s.EndDateTime as Date) = @Date
	UNION 

	SELECT 
		  'Break' as Name, 
		  2 as [Priority],
		  e.name as Employee, 
		  s.StartDateTime, 
		  s.EndDateTime
	FROM schedule.Breaks s
			INNER JOIN dbo.Activities a 
				ON a.id = s.ActivityID
			INNER JOIN Employees e 
				ON e.id = s.EmployeeID
	WHERE 
		CAST(s.StartDateTime as Date) = @Date OR 
		CAST(s.EndDateTime as Date) = @Date
	UNION 

	SELECT 
		 'Leave' as Name, 
		 1 as [Priority], 
		 e.name as Employee, 
		 s.StartDateTime, 
		 s.EndDateTime
	FROM schedule.Leaves s
			INNER JOIN dbo.Activities a 
				ON a.id = s.ActivityID
			INNER JOIN Employees e 
				ON e.id = s.EmployeeID
	WHERE 
		CAST(s.StartDateTime as Date) = @Date OR 
		CAST(s.EndDateTime as Date) = @Date	
	) temp 

	Select * from #tempData order by Employee asc,[Priority] desc,StartDateTime asc,EndDateTime asc
END
GO