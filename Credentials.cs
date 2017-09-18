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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace MOSWdeploy
{
    /*
       * Credentials provide access to FileZilla and MariaDB 
       * */
    [Serializable]
    public class Credentials
    {
        private static RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
        public string Login { get; set; }
        // password provided to group
        public string ClearTextPwd { get; set; }
        // password stored inside FTP config file
        public String HashedPwd { get; set; }
        public String DbName { get; set; }

        // Full-control constructor
        public Credentials(string id, string pwd, string dbName)
        {
            Login = id;
            ClearTextPwd = pwd;
            HashedPwd = ComputeMd5(pwd);
            DbName = dbName;
        }

        // Constructor with presets from public name
        public Credentials(string publicName)
        {
            Login = publicName + GenerateRandomString(4);
            ClearTextPwd = GenerateRandomString(8);
            HashedPwd = ComputeMd5(ClearTextPwd);
            DbName = "bdd" + publicName;
        }

        private String GenerateRandomString(int targetLength)
        {
            char[] chars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();
            byte[] data = new byte[targetLength];

            crypto.GetNonZeroBytes(data);
            StringBuilder randomString = new StringBuilder(targetLength);
            foreach (byte b in data)
            {
                randomString.Append(chars[b % (chars.Length)]);
            }
            return randomString.ToString();
        }

        public void Display()
        {
            Console.WriteLine("Login {0} / mdp {1} / Hash {2}", Login, ClearTextPwd, HashedPwd);
        }

        internal string ComputeMd5(string chaine)
        {
            StringBuilder sb = new StringBuilder();
            using (MD5 md5Hash = MD5.Create())
            {
                // Hash generation
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(chaine);
                byte[] hash = md5Hash.ComputeHash(inputBytes);
                // Hash string generation
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
            }
            return sb.ToString();
        }

    }
}
