# Wordle Practice Project

This project is for exploring new libraries, framworks and languages. 

## Setting up

### .NET API

#### IDE

To get the project ready the first thing needed is to get the backend API up an running. The simplest way of doing this is by installing Visual Studios. The IDE comes with everything we need already built in. Just make sure to include the Web option when installing. 

[Download Visual Studios](https://visualstudio.microsoft.com/downloads/)

#### Project

Once you have Visual Studio is installed you can navigate to the project folder. Inside the folder called "dotnet" you'll find a .sln file. Open this file using Visual Studio and the project will be ready. 

#### Database

For the API to function it requires a database connection. The simplest way of doing this is using the built in SQL server in Visual Studio. To do this simply select View -> SQL Server Object Explorer in the top toolbar. 

![alt text](https://i.imgur.com/yZaFVKR.png "View menu option inside Visual Studio")

This will open a new window where you should have a local SQL Server. Expand it until you see a directory called "Databases". Right click this and select "Add new database". This will prompt you with a window where you can name the database for the project. Once you click "OK" you'll have a database setup and ready to use. 

Finally we also need to add in a connection string to the app settings file so the API knows what database to connect to. To do this, expand the API project in your solution explorer. Then select the appsettings.json and make a duplicate. Rename this duplocate to "appsettings.Development.json" Once you're done your solution explorer should look like below. 

![alt text](https://i.imgur.com/3b3rMKY.png "Solution explorer")

Next you'll want to open the "appsettings.Development.json" and make sure it looks like below. Replace [Name of database] with what ever you entered as the name of the database you created earlier. If you used another authentication method this string will be different. 

![alt text](https://i.imgur.com/TJsHwNS.png "appsettings.Development.json")

The last the needed before we're ready is to create the required tables within the database. To do this, once again select view in the Visual Studio menu bar but this time choose "Terminal" 

![alt text](https://i.imgur.com/UN9n4py.png "Menu for terminal")

With the terminal open you'll want to navigate into the API folder within the solution. Once you're inside the API folder the command below will setup the database with everything you need to run the project. 

```
dotnet ef database update --context WordleDatabaseContex
```

Once the command has run you can start the project. Make sure you have API as the selected project in the drop down and hit the green arrow in the toolbar. A page with the projects Swagger doc should open and the project is good to go. If you want to test to see if everything is working,  expand the POST /api/Wordle/start endpoint and run it with the default parameters. It should return a 200 OK with a GUID of the started game. 







