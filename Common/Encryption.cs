using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Configuration;
using System.IO;

namespace Common
{
    public class Encryption
    {
        //Hasshing - one way only 
        public static string HashPassword(string pass)
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


        //Symetric Encryption
        public static string SymmetricEncrypt(String input)
        {
            //1.choose an algorithm and intialize it
            // rinjdael , AES, RC2,TripleDes,Des

            Rijndael myAlg = Rijndael.Create();

            //2.Conevrting input into an array of bytes to be able to input it later on in the algorithm 
            byte[] inputAsBytes = Encoding.UTF32.GetBytes(input);

            //3.Genrating the key and the iv
            string password = ConfigurationManager.AppSettings["password"];

            Rfc2898DeriveBytes myKeyGen = new Rfc2898DeriveBytes(password,new byte[] { 78, 52, 254, 122, 75, 68, 97, 87 });

            myAlg.Key = myKeyGen.GetBytes(myAlg.KeySize / 8);
            myAlg.IV = myKeyGen.GetBytes(myAlg.BlockSize / 8);

            //4.Preaping the input to be encrypted as a memorystream
            MemoryStream msInput = new MemoryStream(inputAsBytes);

            //5.declaring the object that will encrypt input
            CryptoStream cs = new CryptoStream(msInput,myAlg.CreateEncryptor(),CryptoStreamMode.Read);

            //6.preapare a place where to store the encrypted data
            MemoryStream msOutput = new MemoryStream();

            //7.extracting the encrypted data
            cs.CopyTo(msOutput);

            //8.converting data back to string
            //i.is data out from a cryptographic algorithm
            //ii. do you need the data back as a string?
            return Convert.ToBase64String(msOutput.ToArray());
        }

        public static string EncryptQueryString(string input)
        {
            string cipher = SymmetricEncrypt(input);

            //"Special char in query string:" /

            cipher = cipher.Replace('/', '|');
            cipher = cipher.Replace('+', '!');
            cipher = cipher.Replace('=', '$');

            return cipher;
        }

        //Asymmetrix Encryption

    }
}
