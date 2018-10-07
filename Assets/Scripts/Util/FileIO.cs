//#define ZIP
#define ENCRYPT
//#define BINARY
using System;
using System.IO;
using System.Collections.Generic;
//using Newtonsoft.Json;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;

#if BINARY
using System.Runtime.Serialization.Formatters.Binary;
#endif
#if ZiP
using ICSharpCode.SharpZipLib.Zip;
#endif

public class FileIO
{

    static string GenerateKey()
    {
        // Must be 64 bits, 8 bytes.
        // Distribute this key to the user who will decrypt this file.
        // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
        //		DESCryptoServiceProvider desCrypto =(DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
        //
        //		// Use the Automatically generated key for Encryption. 
        //		string str=ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        //		Debug.Log(str);

        return "8iheuw6miE";
    }

    static public void Delete(string str)
    {

        File.Delete(Application.persistentDataPath + "/" + str);

    }
    static public bool Exist(string str)
    {

        return File.Exists(Application.persistentDataPath + "/" + str);

    }

#if ENCRYPT
    static void EncryptFile(string strInput,FileStream fsEncrypted,string sKey) {  

		byte[] bytearrayinput=Encoding.UTF8.GetBytes(strInput);

		DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
		DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
		DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
		ICryptoTransform desencrypt = DES.CreateEncryptor();
		CryptoStream cryptostream = new CryptoStream(fsEncrypted, 
			desencrypt, 
			CryptoStreamMode.Write); 

		cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
		cryptostream.Close();

	}  

	static string DecryptFile(FileStream fsread,string sKey) {
		DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
		//A 64 bit key and IV is required for this provider.
		//Set secret key For DES algorithm.
		DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
		//Set initialization vector.
		DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

		//Create a DES decryptor from the DES instance.
		ICryptoTransform desdecrypt = DES.CreateDecryptor();
		//Create crypto stream set to read and do a 
		//DES decryption transform on incoming bytes.
		CryptoStream cryptostreamDecr = new CryptoStream(fsread, 
			desdecrypt,
			CryptoStreamMode.Read);
		return new StreamReader (cryptostreamDecr).ReadToEnd ();

	} 
#endif

    public static T Deserialize<T>(string file) {

		T t = default(T);
		using (FileStream fs = new FileStream(Application.persistentDataPath + "/" + file, FileMode.OpenOrCreate, FileAccess.Read))
		{
#if ZiP
		
			if (fs.Length > 0) {

				ZipInputStream zin = new ZipInputStream(fs);
			
				zin.Password= GenerateKey();

				byte[] buffer = new byte[ zin.GetNextEntry().Size];

				zin.Read(buffer, 0, buffer.Length);

		
				t = JsonReader.Deserialize<T> ( System.Text.Encoding.UTF8.GetString(buffer));

			}
		
//#elif ENCRYPT
//            try {
//			// Must be 64 bits, 8 bytes.
//			// Distribute this key to the user who will decrypt this file.

//			    string str = DecryptFile (fs, GenerateKey ());
//                t = JsonConvert.DeserializeObject<T>(str, new JsonSerializerSettings()

//                {
//                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
//                    TypeNameHandling = TypeNameHandling.All

//                });
//    		} catch {

//    		}
#elif BINARY
            BinaryFormatter bf = new BinaryFormatter();
            t=(T)bf.Deserialize(fs);
#endif

		}
		return t;

	}

	public static void Serialize(string file,object v) {

        using (FileStream fs = new FileStream(Application.persistentDataPath + "/" + file, FileMode.Create, FileAccess.Write))
        {
#if ZiP
			using (ZipOutputStream zout = new ZipOutputStream(fs))
			{
                zout.Password = GenerateKey();
				zout.SetLevel (0);
				ZipEntry entry = new ZipEntry ("0");
				zout.PutNextEntry (entry);
			    byte[] b = System.Text.Encoding.ASCII.GetBytes (JsonConvert.SerializeObject (v));
								//	gzipOut.Write(aData
				zout.Write(b,0,b.Length);
				zout.CloseEntry ();
			}
//#elif ENCRYPT

//			EncryptFile(JsonConvert.SerializeObject(v, new JsonSerializerSettings()
//			{
//                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
//                TypeNameHandling = TypeNameHandling.All
//            }), fs, GenerateKey());
#elif BINARY
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, v);
#endif

        }
	}

	public static FileStream GetFileStream(string file, bool bWrite=false) {
		return new FileStream (Application.persistentDataPath + "/" + file, bWrite?FileMode.Create:FileMode.OpenOrCreate, FileAccess.ReadWrite);
	}

#region TempData

	static Dictionary<string,object> tempDic=null;
	static void  SetTempData() {
		if(tempDic==null ) tempDic=Deserialize<Dictionary<string,object>> ("localtemp.sav");
		if(tempDic == null)
			tempDic = new Dictionary<string, object> ();
	}

	public static bool IsKeyData(string key) {
		SetTempData ();
		return tempDic.ContainsKey (key);
	}
		

	public static T GetKeyData<T>(string key)  where T : struct{
		SetTempData ();
		object obj = null;
		tempDic.TryGetValue(key, out obj);
		if (obj == null)
			return default(T);
		return (T)Convert.ChangeType(obj, typeof(T));
	}

	public static void SetKeyData(string key, object v) {
		SetTempData ();
		tempDic[key]= v;
		FileIO.Serialize ("localtemp.sav", tempDic);

	}

	public static void RemoveKeyData(string key) {
		SetTempData ();
		tempDic.Remove (key);
		FileIO.Serialize ("localtemp.sav", tempDic);
	}
#endregion


}


