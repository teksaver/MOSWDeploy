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
    // FileZilla configuration 
    public class FileZillaConf
    {
        // List of added groups
        private List<string> idGroupList;
        // Path for the final conf to be written
        private string pathOut;
        // FileZilla conf XML doc 
        private XDocument xmlOut;
        // FTP directory on server
        private string baseDirFTP;
        // XML FileZilla user template
        private string userTemplate;

        public FileZillaConf(string xIn, string xOut, string baseDirFTP, string userTemplate)
        {
            this.idGroupList = new List<string>();
            this.pathOut = xOut;
            this.baseDirFTP = baseDirFTP;
            this.userTemplate = userTemplate;
            try
            {
                Console.WriteLine("Loading XML config file from " + xIn);
                xmlOut = XDocument.Load(xIn);
            }
            catch (System.Exception)
            {
                Console.WriteLine("Base file not found. Should be in Files/ReadOnly folder");
                System.Environment.Exit(1);
            }
            // If starting from base file, add the users element
            if (xmlOut.Root.Element("Users") == null)
            {
                xmlOut.Root.Add(new XElement("Users"));
            }
        }

        public void AddGroup(StudentGroup group)
        {
            this.idGroupList.Add(group.GroupId);
            // Add admin XML User element 
            XElement admin = createXMLUser(group.admin);
            // Admin only needs access to base dir, does not have own dir
            XElement permadmin = admin.Elements("Permissions").Single()
                .Element("Permission");
            permadmin.SetAttributeValue("Dir", baseDirFTP);
            // Remove global access to subdirs
            permadmin.Elements("Option").Where(
                            x => x.Attribute("Name").Value == "DirSubdirs")
                            .Single()
                            .Value = "0";
            // Buffer for each Student
            XElement userNode;
            // Student loop
            foreach (var student in group.Students)
            {
                // Add User XML Element
                userNode = createXMLUser(student);
                xmlOut.Root.Element("Users").Add(userNode);
                // Set permissions to user and admin
                XElement permUser = new XElement(userNode.Elements("Permissions").Single().Element("Permission"));
                permUser.Elements("Option").Where(
                    x => x.Attribute("Name").Value == "IsHome")
                            .Single()
                            .Value = "0";
                permadmin.Parent.Add(permUser);
            }
            // Add admin to XML
            xmlOut.Root.Element("Users").Add(admin);

        }

        public void WriteXML()
        {
            try
            {
                xmlOut.Save(pathOut);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Cannot write FileZilla conf XML file " + e.Message);
                System.Environment.Exit(1);
            }
        }

        private XElement createXMLUser(Credentials c)
        {
            // load base user template
            XElement u = XElement.Load(this.userTemplate);
            // update login
            u.SetAttributeValue("Name", c.Login);
            // update password
            u.Elements("Option").Where(
                x => x.Attribute("Name").Value == "Pass")
                .Single()
                .Value = c.HashedPwd;
            // update path
            u.Elements("Permissions").Single()
                .Element("Permission")
                .SetAttributeValue("Dir", baseDirFTP + c.Login);
            return u;
        }


    }
};