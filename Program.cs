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
            string baseDirFTP= @"c:\projetsweb\";
            string userTemplate = Path.Combine("Files", "ReadOnly", @"FZConfUserTemplate.xml");

            // groups creation
            StudentGroup gr10=new StudentGroup("gr10",15);
            StudentGroup gr12=new StudentGroup("gr12",15);

            // create file backup
            gr10.SaveToFile();
            gr12.SaveToFile();

            // FileZilla generation
            FileZillaConf fzConf=new FileZillaConf(pathBase,pathOut,baseDirFTP,userTemplate);
            fzConf.AddGroup(gr10);
            fzConf.AddGroup(gr12);



            // Writing config file
            Console.WriteLine("Saving XML config file");
            fzConf.WriteXML();
        }
    }
}
