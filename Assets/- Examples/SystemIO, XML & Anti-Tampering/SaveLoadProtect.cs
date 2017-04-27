using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Xml;
using System.Security.Cryptography;

public class SaveLoadProtect : MonoBehaviour {


	public string filename = "//- Examples//SystemIO, XML & Anti-Tampering//myXML.xml";
	string filepath;

	public bool fileProtection = false;
	public string filenameProtection = "//- Examples//SystemIO, XML & Anti-Tampering//myXmlMd5hash.txt";

	public class MyXMLObject{
		public string nameOfElement;
		public int intElement;
		public float floatElement;
		public Child childElement;
		public ArrayChild[] arrayElement;
		public ObjectPosition objectPos;
	}
	public class Child{
		public string yetAnotherElementName;
	}
	public class ArrayChild{
		public string elementName;
	}
	public class ObjectPosition{
		public float xPos;
		public float yPos;
		public float zPos;
	}

	MyXMLObject myXMLObject;

	void Start(){

		filepath = Application.dataPath+"//"+filename;
		filenameProtection = Application.dataPath+"//"+filenameProtection;

		//If the file doesn't exists, create it
		if( ! System.IO.File.Exists(filepath)){
			
			//Initialization of the Object & its data, which will be later saved to an XML file
			myXMLObject = new MyXMLObject{
				nameOfElement = "A name for the xml value of this element",
				intElement = 5,
				floatElement = 5.05f,
				childElement = new Child{
					yetAnotherElementName = "test"
				},
				arrayElement = new ArrayChild[]{
					new ArrayChild{elementName="abc"},
					new ArrayChild{elementName="def"},
					new ArrayChild{elementName="ghi"},
					new ArrayChild{elementName="jkl"},
					new ArrayChild{elementName="mno"}
				},
				objectPos = new ObjectPosition{
					xPos = transform.position.x,
					yPos = transform.position.y,
					zPos = transform.position.z
				}
			};

			//Save the object to XML file
			SaveXML();

		}else{
			//Load the existing file
			LoadXML();
		}

		//Move the object based on the loaded myXMLObject
		transform.position = new Vector3(myXMLObject.objectPos.xPos, myXMLObject.objectPos.yPos, myXMLObject.objectPos.zPos);
	}

	//The XML Serializer using the Object Class as reference
	System.Xml.Serialization.XmlSerializer myXMLObjectSerializer = new System.Xml.Serialization.XmlSerializer(typeof(MyXMLObject));

	void SaveXML(){
		//Start StreamWriter to write to the specified filepath
		using(StreamWriter xmlWriter = new StreamWriter(filepath)){
			//Serialize the XML Object data
			myXMLObjectSerializer.Serialize(xmlWriter, myXMLObject);
		}

		//Start the Anti Tampering Protection hashing
		//Disable if you want to be able to modify the file outside from Unity
		if(fileProtection) AntiTamperingProtection();
	}

	void LoadXML(){

		//Test the Anti Tampering Protection
		//Disable if you want to be able to modify the file outside from Unity
		if(fileProtection) TestAntiTampering();

		//Start StreamReader to load the specified file to an object
		using(StreamReader xmlLoader = new StreamReader(filepath)){
			//Deserialize the XML File and apply to our XML Object
			myXMLObject = (MyXMLObject) myXMLObjectSerializer.Deserialize(xmlLoader);
		}
	}

	//Some lessons about Cryptography!
	//MD5 is a "hashing" algorigthm that allows us to create a Unique Identifier for our file
	//By comparing the file vs the expected md5 hash, we can know if the file has been tampered
	//Not tested for actual real life security purposes. This could be exploitable and needs to be thoroughly tested
	//Always take security seriously!
	void AntiTamperingProtection(){
		using(StreamWriter protectionFile = new StreamWriter(filenameProtection)){
			protectionFile.WriteLine(GetSaltedMD5HashFromFile(filepath));
		}
	}

	void TestAntiTampering(){
		string protectionFileMD5;
		string currentFileMD5;

		using(StreamReader protectionFile = new StreamReader(filenameProtection)){
			protectionFileMD5 = protectionFile.ReadLine();
		}

		currentFileMD5 = GetSaltedMD5HashFromFile(filepath);

		//If both strings are not equal to the same value, the data have been tampered
		if(!string.Equals(protectionFileMD5, currentFileMD5))
		{
			Debug.LogWarning("CRITICAL WARNING! Data have been tampered!");
		}else{
			Debug.Log("Data is safe. It has not been tampered.");
		}
	}



	//This string is what we call "Salt". It is used to modify the value of an MD5 Hash
	//This creates "Randomness" in the MD5 hash, which makes hackers life a lot more difficult :)
	string salt = "HJKHKJSDHF7SHJFKSDHYNSFSDFN";

	//Returns the Salted and Hashed MD5 string that will be saved and compared to original files.
	string GetSaltedMD5HashFromFile(string path)
	{
		//Take the salt, and transform it to bytes
		byte[] byteSalt = System.Text.Encoding.UTF8.GetBytes(salt);
		//Start a HMACMD5. It uses the salt to modify the original MD5 hash
		using(HMACMD5 hmacMD5 = new HMACMD5(byteSalt))
		{
			//Opens the original file, then convert it to a hmac md5 string, protected by the salt
			using(FileStream stream = System.IO.File.OpenRead(path)){
				return System.BitConverter.ToString(hmacMD5.ComputeHash(stream)).Replace("-",string.Empty);
			}
		}
	}

	//For your information, but this method is 100% non-secure!
	string GetMD5HashFromFile(string path)
	{
		//Create a MD5 Hashing object
		using (MD5 md5 = MD5.Create())
		{
			//First, let's open the non-tampered file
			using (FileStream stream = System.IO.File.OpenRead(path))
			{
				//Return the value of the MD5 hash, in a convenient structure
				return System.BitConverter.ToString(md5.ComputeHash(stream)).Replace("-",string.Empty);
			}
		}
	}
}
