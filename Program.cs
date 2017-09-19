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
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.IO;

namespace MOSWdeploy
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathBase = Path.Combine("Files", "ReadOnly", @"FileZillaBase.xml");
            string pathOut = Path.Combine("Files", @"FileZilla Server.xml");
            string baseDirFTP = @"c:\projetsweb\";
            string userTemplate = Path.Combine("Files", "ReadOnly", @"FZConfUserTemplate.xml");
            string mySQLOutDir = "Files";

            // load existing groups
            StudentGroup gr10 = StudentGroup.LoadFromFile("gr10");
            StudentGroup gr12 = StudentGroup.LoadFromFile("gr12");

            // new groups creation
            // StudentGroup gr10=new StudentGroup("gr10",8);
            // StudentGroup gr12=new StudentGroup("gr12",14);

            // create file backup
            // gr10.SaveToFile();
            // gr12.SaveToFile();

            // FileZilla generation
            FileZillaConf fzConf = new FileZillaConf(pathBase, pathOut, baseDirFTP, userTemplate);
            fzConf.AddGroup(gr10);
            fzConf.AddGroup(gr12);

            // Writing config file
            Console.WriteLine("Saving XML config file");
            fzConf.WriteXML();

            // MySQL generation
            Console.WriteLine("Saving DB config file");
            MySQLConf.GenerateDBFile(gr10, mySQLOutDir);
            MySQLConf.GenerateDBFile(gr12, mySQLOutDir);

            // CSV generation
            Console.WriteLine("Exporting to CSV");
            ExportGroup.ToCSV(gr10);
            ExportGroup.ToCSV(gr12);
        }
    }
}
