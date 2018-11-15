using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Common
{
    public class Encryption
    {
        //Hasshing - one way only 
        public string HashPassword(string pass)
        {
            //converting to bytes
            byte[] Bytespass =Encoding.UTF8.GetBytes(pass);
            //use encoding .utf8.getbytes when you are converting user input only

            //digest
            var myalg = SHA512.Create();
            byte[] digest = myalg.ComputeHash(Bytespass);//need a byte array

            //use conver to base 64 when converting cryptographic bytes only
            return Convert.ToBase64String(digest);//converting back to string
        }

    }
}
