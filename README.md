# ContractMonthlyClaimSystem project
---

## ASP.net Core is running .net 6.0 and packages installed also run their respective 6.0 versions like the following
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.ReportingServices.ReportViewerControl.WinForms [ Install latest version ]
- FluentValidation [ Install version 11 or greater ]

## For testing purposes
- On the DbInitializer.cs, there 2 users that are used for testing to simulation logging in as a Coordinator or a Lecturer
- For lecturer: 
	- Email : lecturer@example.com
	- Passowrd : Password123!
	
- For Coordinator:
	- Email : coordinator@example.com
	- Password : Password123!

## Packages to install in this project
- RDLC Report Designer for the HR Automation (found in extensions)
	-	Steps on how to add a report in Report Designer:
		- Add a New RDLC Report:
		- Right-click on the project.
		- Add > New Item.
		- Select Reporting > Report.
		- Name it ApprovedClaimsReport.rdlc.	
- FluentValidation for Coordinators and Academic Managers
