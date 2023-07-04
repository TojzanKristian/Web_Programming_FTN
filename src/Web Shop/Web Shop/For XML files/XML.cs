using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Web_Shop
{
    public class XML
    {
        #region Upis u XML fajl
        public void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    string path = PutanjaDoProjekta();
                    xmlDocument.Save(path + "bin\\Debug\\" + fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion

        #region Čitanje iz XML fajla
        public T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default; }

            T objectOut = default;

            try
            {
                string attributeXml = string.Empty;

                XmlDocument xmlDocument = new XmlDocument();
                string path = PutanjaDoProjekta();
                xmlDocument.Load(path + "bin\\Debug\\" + fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return objectOut;
        }
        #endregion

        #region Nalaženje putanje do projekta
        private string PutanjaDoProjekta()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return baseDirectory;
        }
        #endregion
    }
}