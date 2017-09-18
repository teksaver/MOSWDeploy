using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public string ClassId { get; set; }
        // admin for the group
        public Credentials admin { get; set; }
        // list of groups for the class
        public List<Credentials> Students { get; set; }

        // Constructor for new groups
        public StudentGroup(string groupName, int howManyToCreate)
        {
            Students = new List<Credentials>();
            for (int i = 0; i < howManyToCreate; i++)
            {
                Students.Add(new Credentials(groupName + (i + 1)));
            }
            admin=new Credentials(groupName + "_admin");
        }

        public void Display()
        {
            Console.WriteLine("Credentials for class " + ClassId);
            foreach (var student in Students)
            {
                student.Display();
            }

        }
    }
}
