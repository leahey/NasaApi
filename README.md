# NasaApi
Started as a coding assessment for a job, will continue as a labor of love.

### Purpose
Built to access and display images from the NASA API portal at https://api.nasa.gov/.

### Warning
The console application which is built is intended to pass a specific set of acceptance criteria, including retreival of photos from specific dates. 
Since the criteria do not specify a rover, a camera, or a number of pages, all photos for all 3 rovers are returned. 
This includes the 392 photos for Curiosity on July 13, 2016.

There is currently no cancel token, so if you are executing the application to verify functional satisfaction of the acceptance criteria, you will have to wait through a long download process.

When you run the application, you'll see a set of menu items you can use to specify rover and/or date for the queries. 
Item #4 meets the acceptance criteria by loading the dates.txt file and executing photo pulls for four differnt specified dates. 
But to see the application in action you could choose one of the other menu items to narrow your search. 

### Next
- Add a test case in the console application to allow specification of camera.
- Add a test case in the console application to allow specification of page size. 
- Next steps include creating an Angular-based web UI to satisfy some of the bonus criteria. 

Since I hope to expand this in the future, to access other NASA APIs, I have de-coupled the architecture, including support for _Dependency Injection_ in the **Program** and **ConsoleApplication** classes. 