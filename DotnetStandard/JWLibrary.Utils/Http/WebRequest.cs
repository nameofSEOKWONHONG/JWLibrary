using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace JWLibrary.Utils {

    public static class WebRequest {

        public static string GetToken(
            string fullUrl,
            string userName, string password,
            string userNameKeyword, string passwordKeyword) {
            HttpWebResponse response = null;

            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(fullUrl);
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            //request.AllowAutoRedirect = false;
            //request.Proxy = new WebProxy("203.236.20.219", 8086);
            //request.Credentials = new NetworkCredential("admin", "media");
            //request.ContentType = "application/x-www-form-urlencoded";
            using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
                string json = "{\"" + userNameKeyword + "\":\"" + userName + "\"," +
                              "\"" + passwordKeyword + "\":\"" + password + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try {
                response = (HttpWebResponse)request.GetResponse();
                using (Stream stream = response.GetResponseStream()) {
                    StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(response.CharacterSet));
                    String responseString = reader.ReadToEnd();
                    return responseString;
                }
            } catch {
                throw;
            } finally {
                if (response != null)
                    response.Close();
            }
        }

        public static string GetResponse(string fullUrl, string method, string token) {
            HttpWebResponse response = null;

            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(fullUrl);
            request.Method = method;
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("Token", token);

            try {
                response = (HttpWebResponse)request.GetResponse();
                using (Stream stream = response.GetResponseStream()) {
                    StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(response.CharacterSet));
                    String responseString = reader.ReadToEnd();
                    return responseString;
                }
            } catch {
                throw;
            } finally {
                if (response != null)
                    response.Close();
            }
        }

        public static bool GetResponse(string url, string method, string cookie, string encodingName, ref string resultData) {
            bool isRet = false;

            try {
                Uri uri = new Uri(url);
                HttpWebRequest wReq = (HttpWebRequest)System.Net.WebRequest.Create(uri);
                wReq.Method = method;
                wReq.ServicePoint.Expect100Continue = false;
                wReq.CookieContainer = new CookieContainer();
                wReq.CookieContainer.SetCookies(uri, cookie);

                using (HttpWebResponse wRes = (HttpWebResponse)wReq.GetResponse()) {
                    Stream resPostStream = wRes.GetResponseStream();
                    StreamReader readerPost = new StreamReader(resPostStream, Encoding.GetEncoding(encodingName), true);

                    resultData = readerPost.ReadToEnd();
                }

                isRet = true;
            } catch {
                throw;
            }

            return isRet;
        }

        public static XmlDocument GetXMLDocumentFromXMLTemplate(string inURL, string data) {
            HttpWebRequest myHttpWebRequest = null;     //Declare an HTTP-specific implementation of the WebRequest class.
            HttpWebResponse myHttpWebResponse = null;   //Declare an HTTP-specific implementation of the WebResponse class
            XmlDocument myXMLDocument = null;           //Declare XMLResponse document
            XmlTextReader myXMLReader = null;           //Declare XMLReader

            try {
                //admin, media
                //Create Request
                myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(inURL);
                myHttpWebRequest.Method = "GET";
                myHttpWebRequest.ContentType = "text/xml; encoding='utf-8'";
                myHttpWebRequest.AllowAutoRedirect = false;

                //Get Response
                myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                //Now load the XML Document
                myXMLDocument = new XmlDocument();

                //Load response stream into XMLReader
                myXMLReader = new XmlTextReader(myHttpWebResponse.GetResponseStream());
                myXMLDocument.Load(myXMLReader);
            } catch (Exception myException) {
                throw new Exception("Error Occurred in AuditAdapter.getXMLDocumentFromXMLTemplate()", myException);
            } finally {
                myHttpWebRequest = null;
                myHttpWebResponse = null;
                myXMLReader = null;
            }
            return myXMLDocument;
        }

        public static string GetInfo(string url) {
            HttpWebResponse response = null;
            string result = null;

            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(url);
            request.Method = "GET";
            request.AllowAutoRedirect = false;
            //request.Proxy = new WebProxy("203.236.20.219", 8086);
            request.Credentials = new NetworkCredential("admin", "media");
            request.ContentType = "application/x-www-form-urlencoded";

            try {
                response = (HttpWebResponse)request.GetResponse();
                using (Stream stream = response.GetResponseStream()) {
                    StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(response.CharacterSet));
                    String responseString = reader.ReadToEnd();
                }
            } catch {
                throw;
            } finally {
                if (response != null)
                    response.Close();
            }

            return result;
        }

        public static string GetToken(string url, string userid, string password) {
            HttpWebResponse response = null;
            string result = null;

            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(url);
            request.Method = "GET";
            request.AllowAutoRedirect = false;
            //request.Proxy = new WebProxy("203.236.20.219", 8086);
            //request.Credentials = new NetworkCredential("admin", "media");
            request.ContentType = "application/x-www-form-urlencoded";

            try {
                response = (HttpWebResponse)request.GetResponse();
                using (Stream stream = response.GetResponseStream()) {
                    StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(response.CharacterSet));
                    String responseString = reader.ReadToEnd();
                }
            } catch {
                throw;
            } finally {
                if (response != null)
                    response.Close();
            }

            return result;
        }
    }
}