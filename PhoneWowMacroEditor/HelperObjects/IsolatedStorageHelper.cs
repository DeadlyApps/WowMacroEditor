using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;

//Add usings and include System.Servicemodel.Web dll
using System.Runtime.Serialization.Json;
using System.IO.IsolatedStorage;
using System.Text;

namespace WowMacroEditorPhone
{
    public class IsolatedStorageHelper
    {
        public static T GetObject<T>(string key)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))  
                {  
                    string serializedObject = IsolatedStorageSettings.ApplicationSettings[key].ToString();  
                    return Deserialize<T>(serializedObject);  
                }  

            return default(T);
        }

        public static void SaveObject<T>(string key, T objectToSave)
        {
                string serializedObject = Serialize(objectToSave);  
                IsolatedStorageSettings.ApplicationSettings[key] = serializedObject;  

        }

        public static void DeleteObject(string key)
        {
            IsolatedStorageSettings.ApplicationSettings.Remove(key); 
        }

        private static string Serialize(object objectToSerialize)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(objectToSerialize.GetType());
                serializer.WriteObject(ms, objectToSerialize);
                ms.Position = 0;

                using (StreamReader reader = new StreamReader(ms))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static T Deserialize<T>(string jsonString)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }
    }
}
