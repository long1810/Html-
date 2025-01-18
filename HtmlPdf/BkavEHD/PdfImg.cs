namespace BES
{
    public class PdfImg
    {
        public static string HTMLConveroPDF(string html, out byte[] data)
        {
            string msg = string.Empty;
            Puppeteer oPuppeteer = new Puppeteer();

            msg = oPuppeteer.DoGen_ContentHTML(html, out data, 1);
            return msg;
        }
    }
}
