# WorkloadCalculator
The end-user can select which courses from the list they want to take and they’ll get their workload (expected study hours per week) to finish their learning journey on time.

This is the first and the simpliest variant of the Application. 
This is a Console Application, but it easily can be extended to WEB Application - we just need to add new project to the solution.

##How it works
1. You need to build Application using MS Visual Studio. 
2. WorkloadCalculator.exe-file will be created in [Project Folder]\bin\Debug\net6.0 folder.
4. After starting exe-file App creates In-Memory Database to store needed data. After closing the application the database will be removed. 
I created in-memory database for this task because I don't know which database you use in the company, and in-memory database is some kind of "portable" variant, which will work on any workstation. For working with data I use IDataManager, so if you want to use Oracle, MS SQL or even Elasticsearch or MongoDB database, you just need to create new DataManager which will implement this interface - and all code will work.
I would recommend to use SQL database, because data doesn't contain any documents with complex structure. If it contained, for example, agenda or list of lessons for each course where we needed to do some search requests, it would be better to use Elasticsearch or MongoDB.
3. Calculator has 2 options: C - means "calculate", H - means "show history of previous calculations". User can type these letters into the Console App in any case.
4. If user types "C", then Calculator asks to type numbers of chosen Courses from the list and Start and End date of studying in format DD/MM/YYYY. It checks the date format and also checks that start date should be greater than end date and both date shouldn't be in the past.
5. If dates were typed correctly, then Calculator asks to type "Y" to continue calculation. If user types "N", Calculator will clear all data which they typed before and asks user to type new data.
6. If user chose "Y", then Calculator sums all hours from chosen courses and calculates workload as sum_of_hours_of_selected_courses/((endDate-startDate)/7). Then it saves calculation into table in the database.
7. If user choose option "H", Calculator selects informtation about all previous calculations from the database and shows it on the Console.