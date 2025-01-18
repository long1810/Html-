using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace BES
{
    public class Puppeteer
    {
        public class Input_GenHTML
        {
            public string headerTemplate { set; get; } = "";
            public string footerTemplate { set; get; } = "";
            public int margintop { set; get; } = 0;
            public int marginright { set; get; } = 0;
            public int marginbottom { set; get; } = 0;
            public int marginleft { set; get; } = 0;
            public string contenthtml { set; get; } = "";
            public string format { set; get; } = "A4";
            public bool landscape { set; get; } = false;
            public double scale { set; get; } = 1;
        }

        public static string HtmlToPdf(string html, out byte[] data)
        {
            string msg = string.Empty;
            Puppeteer oPuppeteer = new Puppeteer();

            msg = oPuppeteer.DoGen_ContentHTML(html, out data, 1);
            return msg;
        }
        public static string HtmlToPdf(string html,string head,string footer, out byte[] data)
        {
            string msg = string.Empty;
            Puppeteer oPuppeteer = new Puppeteer();

            msg = oPuppeteer.DoGen_ContentHTML(html, out data, 1, head,footer);
            return msg;
        }
        //public virtual string DoGen_ContentHTML(string contentHTML, out byte[] contentPDF, double scale = 1)
        //{
        //    contentPDF = null;

        //    Input_GenHTML input = new Input_GenHTML();


        //    input.contenthtml = contentHTML;

        //    string msg = ApiCall("http://localhost:3002/generatePdf_content", "application/json", "", "", input, "POST", out contentPDF);
        //    return msg;
        //}
        public virtual string DoGen_ContentHTML(string contentHTML, out byte[] contentPDF, double scale = 1,string header = "", string footer ="")
        {
            contentPDF = null;

            Input_GenHTML input = new Input_GenHTML();

            input.headerTemplate = header;
            input.contenthtml = contentHTML;
            input.footerTemplate = footer;
            string msg = ApiCall("http://localhost:3002/generatePdf_content", "application/json", "", "", input, "POST", out contentPDF);
            return msg;
        }
        public string ApiCall(string url, string contentType, string headerKey, string headerValue, object body, string method, out byte[] responseArray)
        {
            responseArray = null;

            try
            {
                var dataWsRequest = JsonConvert.SerializeObject(body, Formatting.Indented);
                var myWebClient = new WebClient();
                myWebClient.Headers.Add("Content-Type", contentType);
                if (headerKey != "") myWebClient.Headers.Add(headerKey, headerValue);
                var byteArray = Encoding.UTF8.GetBytes(dataWsRequest);
                responseArray = myWebClient.UploadData(url, method, byteArray);
            }
            catch (WebException we)
            {
                if (we.Response != null)
                {
                    using (WebResponse response = we.Response)
                    {
                        Stream dataRs = response.GetResponseStream();
                        using (StreamReader reader = new StreamReader(dataRs))
                        {
                            HttpStatusCode statusCode = ((HttpWebResponse)response).StatusCode;
                            return statusCode + ":" + reader.ReadToEnd();
                        }
                    }
                }
                else return $"WebException Call API: {url}, method {method}, header {headerValue}, headerKey{headerKey}, body {body}, error {we.ToString()}";
            }
            catch (Exception ex)
            {
                return $"Exception Call API: {url}, method {method}, header {headerValue}, headerKey{headerKey}, body {body}, error {ex.ToString()}";
            }
            return "";
        }

        public class Input_GenImage
        {
            public string type { set; get; } = "jpeg"; //jpeg, png or webp
            public int quality { set; get; } = 100;
            public bool fullPage { set; get; }  //When true, takes a screenshot of the full scrollable page. Defaults to false.
            public int clipx { set; get; } = 0;
            public int clipy { set; get; } = 0;
            public double clipwidth { set; get; }
            public double clipheight { set; get; }
            public string contenthtml { set; get; }
            public string selector { set; get; } = "#Signature";
            public string omitBackground { set; get; } //Hides default white background and allows capturing screenshots with transparency. Defaults to false.
            public string encoding { set; get; }//base64, binary
            public string captureBeyondViewport { set; get; } //When true, captures screenshot beyond the viewport. Whe false, falls back to old behaviour, and cuts the screenshot by the viewport size. Defaults to true

        }

        public string DoGenImage(string contentHTML, double clipwidth, double clipheight, out byte[] contentImage)
        {
            contentImage = null;
            Input_GenImage input = new Input_GenImage();
            input.clipwidth = clipwidth;
            input.clipheight = clipheight + 500;
            input.contenthtml = contentHTML;
            string msg = ApiCall("http://localhost:3002/generateImage", "application/json", "", "", input, "POST", out contentImage);
            return msg;
        }
    }
}
