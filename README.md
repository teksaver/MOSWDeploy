# MOSWDeploy

MOSWDeploy is a .NET CORE console application that automates the generation of a FileZilla configuration file and MySQL/MariaDB database creation scripts. This allows for mass creation of credentials for FTP-based deployment of PHP/MySQL web sites. This is primary designed for ESIGELEC's "web programming" class

The core part of the application is the "StudentGroup" class. For each group of student, a StudentGroup object is constructed by providing
* a group identifier
* the number of students that need to deploy in this particular group

The outputs are
* A FileZilla configuration file with credentials for each student + an admin for each group
* a SQL script for each group
* a CSV file for each group, to be provided to the students and admin

Generated groups can be saved and reloaded as a .dat binary file, to make sure the credentials are not lost in case new groups are added and the FileZilla file gets overwritten.

Configuration is done in the main Program.cs file. An Example is provided with the creation of 2 groups.
