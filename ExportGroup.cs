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

// This file deals with output generation : CSV first, and PDF when a .NET Core library is available

using System;
using System.IO;

namespace MOSWdeploy
{
    public class ExportGroup
    {
        public static void ToCSV(StudentGroup group)
        {
            string csvFile = Path.Combine("Files", group.GroupId + ".csv");
            using (var sw = new StreamWriter(csvFile))
            {
                var nb = 1;
                foreach (var student in group.Students)
                {
                    // Header
                    sw.Write("group name,login,password, dbname");
                    sw.Write(Environment.NewLine);
                    // Group credentials
                    sw.Write("Grp. " + nb + ","
                        + student.Login + ","
                        + student.ClearTextPwd + ","
                        + student.DbName);
                    sw.Write(Environment.NewLine);
                    nb += 1;
                }
                sw.Write("Admin grp" + group.GroupId + ","
                       + group.admin.Login + ","
                       + group.admin.ClearTextPwd + ","
                       + "");
                sw.Write(Environment.NewLine);
            }
        }


    }
};