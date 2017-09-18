// Copyright (C) 2017 Sylvain TENIER
// 
// This file is part of MOSWdeploy.
// 
// MOSWdeploy is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// MOSWdeploy is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MOSWdeploy.  If not, see <http://www.gnu.org/licenses/>.
// 

using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace MOSWdeploy
{
    class MySQLConf
    {
        public static void GenerateDBFile(StudentGroup group, string dir)
        {
            string pathOut = Path.Combine(dir, "createDBs_" + group.GroupId + ".sql");
            try
            {
                using (StreamWriter sqlFile = new StreamWriter(pathOut))
                {
                    // create admin
                    sqlFile.WriteLine("CREATE USER '" + group.admin.Login + "'@'%' IDENTIFIED BY '" + group.admin.ClearTextPwd + "';");
                    // configure users
                    foreach (var student in group.Students)
                    {
                        // create user
                        sqlFile.WriteLine("CREATE USER '" + student.Login + "'@'%' IDENTIFIED BY '" + student.ClearTextPwd + "';");
                        // create database for user
                        sqlFile.WriteLine("CREATE DATABASE `" + student.DbName + "`;");

                        // add priviledges to user
                        sqlFile.WriteLine(@"GRANT SELECT, UPDATE, TRIGGER, REFERENCES, INSERT
                                        , EVENT, INDEX, DROP, DELETE, EXECUTE, SHOW VIEW
                                        , ALTER, CREATE, CREATE ROUTINE, CREATE TEMPORARY TABLES, CREATE VIEW, ALTER ROUTINE
                                        ON `" + student.DbName + "`.* TO '" + student.Login + "'@'%';");

                        // add priviledges to admin
                        sqlFile.WriteLine(@"GRANT SELECT ON `" + student.DbName + "`.* TO '" + group.admin.Login + "'@'%';");
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine("errror : " + e.Message);
                throw;
            }
            Console.WriteLine("SQL conf generated from group {0} at {1}",group.GroupId,pathOut);
        }

    }
};