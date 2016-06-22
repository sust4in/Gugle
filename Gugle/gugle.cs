using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Text;
using System.IO;
using HtmlAgilityPack;

namespace Gugle
{

    public class gugle
    {
        public static HttpResponseMessage response;
        HtmlDocument htmlsnippet = new HtmlDocument();
        public static string search;
        public static string uri;


        static void Main(string[] args)
        {
            List<string> list = new List<string>();
            search = args[0];

            //searching for links
            StringBuilder builder = new StringBuilder();

            byte[] ResultsBuffer = new byte[8192];
            string SearchResults = "http://google.com/search?q=" + search.Trim();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SearchResults);
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();

            Stream resStream = resp.GetResponseStream();
            string tempString = null;

            int count = 0;
            do
            {
                count = resStream.Read(ResultsBuffer, 0, ResultsBuffer.Length);
                if (count != 0)
                {
                    tempString = Encoding.ASCII.GetString(ResultsBuffer, 0, count);
                    builder.Append(tempString);
                }
            }

            while (count > 0);
            string builder2 = builder.ToString();

            HtmlDocument html = new HtmlDocument();
            html.OptionOutputAsXml = true;
            html.LoadHtml(builder2);
            HtmlNode doc = html.DocumentNode;
            int counter = 0;

            foreach (HtmlNode link in doc.SelectNodes("//a[@href]"))
            {
                string hrefValue = link.GetAttributeValue("href", string.Empty);

                //i<3sof
                
                if (!hrefValue.ToString().ToUpper().Contains("GOOGLE") && hrefValue.ToString().Contains("/url?q=") && hrefValue.ToString().ToUpper().Contains("HTTP://"))
                {
                    int index = hrefValue.IndexOf("&");
                    if (index > 0)
                    {

                        hrefValue = hrefValue.Substring(0, index);
                        list.Add(hrefValue.Replace("/url?q=", ""));
                        counter++;
                        Console.WriteLine("# Url count [" + counter+"]"+" keyword => "+search);

                    }
                }
            }
            Console.Write("Now, release the hounds !");
            Thread.Sleep(2000);
                         
            foreach (var item in list)
            {
                try
                {
                    request = (HttpWebRequest)WebRequest.Create(item);
                    resp = (HttpWebResponse)request.GetResponse();
                    HttpStatusHandler(item, resp);

                }
                catch (HttpRequestException err)
                {
                    HttpStatusHandler(item, resp);
                }

            }

            Console.ReadLine();


        }

        static void HttpStatusHandler(string uri, HttpWebResponse response)
        {
            try
            {
                // Handle unsuccessfull status
                if (response.StatusCode == HttpStatusCode.Redirect)
                {
                    Console.WriteLine(response.ResponseUri + ">[" + response.StatusCode + "]" + "ERROR :" + response.StatusDescription);
                }
                if (response.StatusCode == HttpStatusCode.RedirectKeepVerb)
                {
                    Console.WriteLine(response.ResponseUri + ">[" + response.StatusCode + "]" + "ERROR :" + response.StatusDescription);
                }
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine(response.ResponseUri + ">[" + response.StatusCode + "]" + "ERROR :" + response.StatusDescription);
                }
                if (response.StatusCode == HttpStatusCode.TemporaryRedirect)
                {
                    Console.WriteLine(response.ResponseUri + ">[" + response.StatusCode + "]" + "ERROR :" + response.StatusDescription);
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    Console.WriteLine(response.ResponseUri + ">[" + response.StatusCode + "]" + "ERROR :" + response.StatusDescription);
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine(response.ResponseUri + ">[" + response.StatusCode + "]" + "ERROR :" + response.StatusDescription);
                }
                if (response.StatusCode == HttpStatusCode.RequestTimeout)
                {
                    Console.WriteLine(response.ResponseUri + ">[" + response.StatusCode + "]" + "ERROR :" + response.StatusDescription);
                }
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    Console.WriteLine(response.ResponseUri + ">[" + response.StatusCode + "]" + "ERROR :" + response.StatusDescription);
                }
                if (response.StatusCode == HttpStatusCode.GatewayTimeout)
                {
                    Console.WriteLine(response.ResponseUri+">["+response.StatusCode+"]"+"ERROR :" + response.StatusDescription);
                }
                else
                {
                    Console.WriteLine(response.ResponseUri + ">[" + response.StatusCode + "]" + ">" + response.StatusDescription);

                }
            }
            catch (NullReferenceException err)
            {
                Console.WriteLine(uri + "=>" + "Site Doesnt Exist !");
            }


        }

    }


}