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
            byte[] Bytespass = Encoding.UTF8.GetBytes(pass);
            //use encoding .utf8.getbytes when you are converting user input only

            //digest
            var myalg = SHA512.Create();
            byte[] digest = myalg.ComputeHash(Bytespass);//need a byte array

            //use conver to base 64 when converting cryptographic bytes only
            return Convert.ToBase64String(digest);//converting back to string
        }


        //Symetric Encryption
        // -- Fast/efficent not as secure
        public static string SymmetricEncrypt(string input)
        {
            //1.choose an algorithm and intialize it
            // rinjdael , AES, RC2,TripleDes,Des

            Rijndael myAlg = Rijndael.Create();

            //2.Conevrting input into an array of bytes to be able to input it later on in the algorithm 
            byte[] inputAsBytes = Encoding.UTF32.GetBytes(input);

            //3.Genrating the key and the iv
            string password = ConfigurationManager.AppSettings["password"];

            Rfc2898DeriveBytes myKeyGen = new Rfc2898DeriveBytes(password, new byte[] { 78, 52, 254, 122, 75, 68, 97, 87 });

            myAlg.Key = myKeyGen.GetBytes(myAlg.KeySize / 8);
            myAlg.IV = myKeyGen.GetBytes(myAlg.BlockSize / 8);

            //4.Preaping the input to be encrypted as a memorystream
            MemoryStream msInput = new MemoryStream(inputAsBytes);

            //5.declaring the object that will encrypt input
            CryptoStream cs = new CryptoStream(msInput, myAlg.CreateEncryptor(), CryptoStreamMode.Read);

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

        public static string SymmetricDecrypt(String input)
        {
            //1.choose an algorithm and intialize it
            // rinjdael , AES, RC2,TripleDes,Des

            Rijndael myAlg = Rijndael.Create();

            //2.Conevrting input into an array of bytes to be able to input it later on in the algorithm 
            byte[] inputAsBytes = Convert.FromBase64String(input);

            //3.Genrating the key and the iv
            string password = ConfigurationManager.AppSettings["password"];

            Rfc2898DeriveBytes myKeyGen = new Rfc2898DeriveBytes(password, new byte[] { 78, 52, 254, 122, 75, 68, 97, 87 });

            myAlg.Key = myKeyGen.GetBytes(myAlg.KeySize / 8);
            myAlg.IV = myKeyGen.GetBytes(myAlg.BlockSize / 8);

            //4.Preaping the input to be encrypted as a memorystream
            MemoryStream msInput = new MemoryStream(inputAsBytes);

            //5.declaring the object that will encrypt input
            CryptoStream cs = new CryptoStream(msInput, myAlg.CreateDecryptor(), CryptoStreamMode.Read);

            //6.preapare a place where to store the encrypted data
            MemoryStream msOutput = new MemoryStream();

            //7.extracting the encrypted data
            cs.CopyTo(msOutput);

            //8.converting data back to string
            //i.is data out from a cryptographic algorithm
            //ii. do you need the data back as a string?
            return Encoding.UTF32.GetString(msOutput.ToArray());
        }


        public static string DecryptQueryString(string input)
        {

            //"Special char in query string:" /

            input = input.Replace('|', '/');
            input = input.Replace('!', '+');
            input = input.Replace('$', '=');

            string cipher = SymmetricDecrypt(input);

            return cipher;
        }


        //Asymmetrix Encryption->
        public static AsymetricKeys GenerateAsymeticKeys()
        {

            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider(); //DSA

            AsymetricKeys keys = new AsymetricKeys();
            keys.PublicKey = myAlg.ToXmlString(false);
            keys.PrivateKey = myAlg.ToXmlString(true);

            return keys;
        }

        public static byte[] AsymmeticallyEncrypt(byte[] input, string publicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();//same algorithm used in asymetric
            rsa.FromXmlString(publicKey);

            //if you need to convert input from a string you use encoding utf8.getBytes
            byte[] output = rsa.EncryptValue(input);
            return output;

            //convert to string would require converttobase64string
        }

        public static byte[] AsymmeticallyDerypt(byte[] input, string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();//same algorithm used in asymetric
            rsa.FromXmlString(privateKey);

            //if you need to convert input from a string you use encoding utf8.getBytes
            byte[] output = rsa.DecryptValue(input);
            return output;

            //convert to string would require converttobase64string
        }

        //public static byte[] SymetricEncrypt(Stream input,byte[] key, byte []iv)


        public static MemoryStream HybridEncrypt(Stream input, string publickey)
        {
            //1.make sure you have a pair of public and private key for the user uploading the file
            //input.Postion = 0; postion has moved becouse file was checked before hand

            //2.generate a secret key and iv(you can yser generateIV(), Genratekey())
            var alg = Rijndael.Create();
            alg.GenerateIV();
            alg.GenerateKey();

            //3. asymetically encrypt the secret key and the iv using user`s public key
            //call method use already
            //bytep[ key = alg.key
            //byte[] encryptedkey = AymetricallyEncrypt(key,publickey); breakpoint here

            //4. symetrically encrypt the contents of the file being uploaded using the iv and the key generated in no.2
            //symmeticEncrypt()

            //location for the item
            MemoryStream MsOut = new MemoryStream();

            //5. save the encrypted key 
            //MsOut.Write(encryptedKey, 0, encrypted.length); 128 //take note of any sizes for comparsion 
            //6. save the encrypted iv


            //7. save the encrypted file contents
            //MemoryStream msEncryptedFile  = new MemoryStream(the result of the symmetricEncryptMethod)
            //msEncryptedFile.CopyTo(MsOut);

            return MsOut;
        }


        public static MemoryStream HybridDecrypt(Stream input, string privateKey)
        {
            //1.retrieve the private key of the file owner
            //2.retrieve the encrypted secret key and the encrypted iv
            byte[] enckey = new byte[128];//compare them with the above 
            input.Read(enckey, 0, 128);
            //remeber to read the iv as well

            //3.symmerically decrypt the secret and the iv from no 2 with the private key from no2
            //AsymeticDecrypt

            //4.create the algorithm instance used in encryption, load it with the decrypted secret key and iv

            //5.symmetrically decrypt the remaining file content using the parameters in step no 4
            MemoryStream remmainingFilecontent = new MemoryStream();
            input.CopyTo(remmainingFilecontent);

            //decrypt 

            //SymmetricDecrypt
            MemoryStream decryptedFile = new MemoryStream();
            //6.return the decrypted data

            return decryptedFile;
        }

        //In database you have a new column of type text where to store the signature in the audios table 

        public string SignData(byte[] input, string privatekey)
        {
            RSACryptoServiceProvider alg = new RSACryptoServiceProvider(); //same algorithm used for asymetric encyption
            alg.FromXmlString(privatekey);

           byte[] signature = alg.SignData(input, new HashAlgorithmName("SHA512"), RSASignaturePadding.Pss);

            return Convert.ToBase64String(signature);
        }

        public bool VerifyData(byte[] input, string publickey,string signature)
        {
            RSACryptoServiceProvider alg = new RSACryptoServiceProvider(); //same algorithm used for asymetric encyption
            alg.FromXmlString(publickey);

            bool result = alg.VerifyData(input,Convert.FromBase64String(signature), new HashAlgorithmName("SHA512"), RSASignaturePadding.Pss);
            //true data was not changed
            //false data was changed

            return result;
        }
    }

    public class AsymetricKeys
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }

    }
}
