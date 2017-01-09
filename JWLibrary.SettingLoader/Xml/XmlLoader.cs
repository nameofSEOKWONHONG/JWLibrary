using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace JWLibrary.SettingLoader.Xml
{
    // T는 [Serializable]을 가져야 함    
    public class XmlLoader<T>
        where T : class
    {
        public static T LoadFromXml(string filename)
        {
            T settings = null;
            if (false == File.Exists(filename)) return null;
            FileStream fs = null;
            XmlSerializer xs = null;

            try
            {
                xs = new XmlSerializer(typeof(T));
            }
            catch (System.Exception e)
            {
                Debug.Assert(false);
                e.ToString();
            }

            try
            {
                fs = new FileStream(filename, FileMode.Open);
                settings = (T)xs.Deserialize(fs);
            }
            catch (System.Exception e)
            {
                Debug.Assert(false);
                e.ToString();
            }

            if (null != fs) fs.Close();
            return settings;
        }

        public static bool Save2Xml(string filename, T settings)
        {
            if (null == filename || string.Empty == Path.GetFileName(filename))
            {
                return false;
            }
            TextWriter tw = null;
            XmlSerializer xs = null;

            try
            {
                xs = new XmlSerializer(typeof(T));
            }
            catch (System.Exception e)
            {
                Debug.Assert(false);
                e.ToString();
            }

            try
            {
                tw = new StreamWriter(filename);
                xs.Serialize(tw, settings);
            }
            catch (System.Exception e)
            {
                Debug.Assert(false);
                e.ToString();
                return false;
            }
            finally
            {
                if (null != tw) tw.Close();
            }
            return true;
        }
    }
}
