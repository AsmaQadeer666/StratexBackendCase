PROBLEM STATEMENT:
The challenge was to retrieve this raw schedule from the database and return it as a flat schedule as response from an API call.
I added following functionalities stated below:
1-	Add a swagger for testing.
2-	Adjusting a overlapping according to 'Time Interval' 
3-	Add a validation for a flat schedule that a leave doesn't not exceed the beginning or ending of a shift.
4-	Create a stored procedure [dbo].[GetActivities]. You can find it in : DbScript/GetActivities.sql

OUTCOMES:
At the end user is able to perform all the required tasks. I had to think from user’s perspective. I tried to add swagger for user convenience, easily testing and debugging. 

QUESTIONS:
Q 1- How long did you spend on this assignment? What would you have done different if you had more time?
Ans: I spent 3.5 hours on this assignment. I tried to focus on the best approach possible (Using the following: Asp.net MVC WebApi, Entity Framework[Database First]). Understood all the requirements first and then started coding. If I had more time 
1.	I implement more architectural wise solution. Like Interface, Dependency Injection, Integration Test or Unit Test.
2.	I had spent more time on Adjust Overlapping.
3.	Following point is partially functional (exceed the beginning or end of a shift for a flat schedule (i.e. : shift ends at 18:00, leave is from 17:00 till 19:00, then leave should 	end at 18:00 for the flat schedule)) .i.e. Validation is applied correctly but the overlapped part of leave on shift is not being calculated accordingly. If I had sometime, it would 	have been fixed then

Q 2- What are focus areas if your code would have to be part of an actual application? 
I would focus on:
1.	Naming Conventions
2.	Coding pattern
3.	Performance
4.	Code Optimization


URL:
Here is the link for swagger: https://localhost:44328/swagger

TESTING:

Input1:
Date = 2022-01-16

Output:
[
  {
    "Name": "Leave",
    "Priority": 1,
    "Employee": "Employee C",
    "StartDateTime": "2022-01-09T00:15:00",
    "EndDateTime": "2022-01-16T00:00:00"
  }
]


Input:
Date = 2022-01-01

Output:
[
  {
    "Name": "Telephone Shift",
    "Priority": 3,
    "Employee": "Employee A",
    "StartDateTime": "2022-01-01T08:00:00",
    "EndDateTime": "2022-01-01T10:30:00"
  },
  {
    "Name": "Break",
    "Priority": 2,
    "Employee": "Employee A",
    "StartDateTime": "2022-01-01T10:30:00",
    "EndDateTime": "2022-01-01T10:45:00"
  },
  {
    "Name": "Telephone Shift",
    "Priority": 3,
    "Employee": "Employee A",
    "StartDateTime": "2022-01-01T10:45:00",
    "EndDateTime": "2022-01-01T14:30:00"
  },
  {
    "Name": "Break",
    "Priority": 2,
    "Employee": "Employee A",
    "StartDateTime": "2022-01-01T14:30:00",
    "EndDateTime": "2022-01-01T14:45:00"
  },
  {
    "Name": "Telephone Shift",
    "Priority": 3,
    "Employee": "Employee A",
    "StartDateTime": "2022-01-01T14:45:00",
    "EndDateTime": "2022-01-01T17:00:00"
  },
  {
    "Name": "Telephone Shift",
    "Priority": 3,
    "Employee": "Employee B",
    "StartDateTime": "2022-01-01T09:00:00",
    "EndDateTime": "2022-01-01T12:30:00"
  },
  {
    "Name": "Break",
    "Priority": 2,
    "Employee": "Employee B",
    "StartDateTime": "2022-01-01T12:30:00",
    "EndDateTime": "2022-01-01T13:00:00"
  },
  {
    "Name": "Telephone Shift",
    "Priority": 3,
    "Employee": "Employee B",
    "StartDateTime": "2022-01-01T13:00:00",
    "EndDateTime": "2021-12-31T22:00:00"
  }
]




