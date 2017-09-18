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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MOSWdeploy
{
    //un groupe de travail est un sous-ensemble d'étudiants d'un groupe de modules
    //aussi appelé binome si le groupe est constitué de 2 étudiants
    [Serializable]
    public class StudentGroup
    {
        // identifier of the class the working group belongs to
        public string GroupId { get; set; }
        // admin for the group
        public Credentials admin { get; set; }
        // list of groups for the class
        public List<Credentials> Students { get; set; }

        // Constructor for new groups
        public StudentGroup(string groupName, int howManyToCreate)
        {
            this.GroupId=groupName;
            Students = new List<Credentials>();
            for (int i = 0; i < howManyToCreate; i++)
            {
                Students.Add(new Credentials(String.Format("{0}{1:D2}",groupName,i + 1)));
            }
            admin = new Credentials(groupName + "_admin");
        }

        public void Display()
        {
            Console.WriteLine("Credentials for class " + GroupId);
            foreach (var student in Students)
            {
                student.Display();
            }
        }

        public void SaveToFile()
        {
            string outFile = Path.Combine("Files", this.GroupId + ".dat");
            try
            {
                using (var stream = new FileStream(outFile, FileMode.CreateNew))
                {
                    var srz = new BinaryFormatter();
                    srz.Serialize(stream, this);
                    Console.WriteLine("Group saved to " + outFile);
                }

            }
            catch (System.Exception e)
            {
                Console.WriteLine("File {0} already exists for group. Remove it if you really want to overwrite, else use LoadFromFile. " + e.Message,outFile);
                System.Environment.Exit(1);
            }
        }

        public static StudentGroup LoadFromFile(string groupId)
        {
            string datFile = Path.Combine("Files", groupId + ".dat");
            StudentGroup group = null;
            try
            {//si le fichier existe on charge le contenu
                using (var stream = new FileStream(datFile, FileMode.Open))
                {
                    var srz = new BinaryFormatter();
                    group = (StudentGroup)srz.Deserialize(stream);
                }
                Console.WriteLine("Group (id={0}) loaded from file {1}", group.GroupId, datFile);

            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Fichier {0} not found, add to Files folder or create new group", datFile);
                System.Environment.Exit(1);
            }

            return group;
        }
    }
}
