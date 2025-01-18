using System;
using System.IO;
using System.Text;

namespace BES
{


    public class HtmlCSS
    {

        public string Body = "html, body { height: 100%;margin: 0;display: flex; align-items: center;   justify-content: center; background-color: #ffffff;-webkit-print-color-adjust: exact; print-color-adjust: exact; }";
        //public string PDF = ".PDF {padding-bottom: 200px;width: 210mm !important;height: 297mm !important;padding: 20mm;border: 1px solid #000;box-sizing: border-box;background-image: url('Background_LyLe.png');background-size: calc(100% - 20mm) calc(100% - 20mm);background-position: center;background-repeat: no-repeat;background-color: white;box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);} ";
        public string h1 = "h1 {text-align: center;color: #00459A;}";
 

        public string header = ".header, .footer {display: flex;justify-content: space-between;margin-bottom: 10px;}";
        public string info = ".info {margin-top: 10px;font-size: 14px;}";
        public string table = ".table {width: 100%;border-collapse: collapse;}  .table th,.table td {border: 1px solid #807def; padding: 5px; text-align: center;}";
        public string footer = ".footer-sign {margin-top: 40px;display: flex;justify-content: space-between;}";
        public string footersign = ".footer-sign div {text-align: center;}";
        public string itemHeader = "th { white-space: nowrap;text-align: center;}  ";
        public string itemHeaderVN = ".ItemHeaderVN { transform: scaleX(1);display: inline-block;}";
        public string itemHeaderEN = ".ItemHeaderEN { transform: scaleX(1);display: inline-block;}";
        public string itemAlign = "";

        public string Custom = "";
        public string style = $@"
            .PDF {{
            width: 210mm !important;
            height: 297mm !important;
            //padding: 10mm;
            //border: 1px solid #000;
            box-sizing: border-box;
            /* Ảnh nền */
            background-size: calc(100% ) calc(100%); /* Trừ padding để khớp */
            background-position: center;
            background-repeat: no-repeat;
            background-color: white; /* Nền trắng cho PDF */
            box-shadow: 0 0 15px rgba(0, 0, 0, 0.1); /* Hiệu ứng đổ bóng nhẹ */;
            ";
        public string H1 = "h1 {text-align: center;color: #00459A;}";
        public string Header = ".header, .footer {display: flex;justify-content: space-between;margin-bottom: 10px;}";
        public string Info = ".info {margin-top: 10px;font-size: 14px;}";
        public string Footer = ".footer-sign {margin-top: 40px;display: flex;justify-content: space-between;}";
        public string Footersign = ".footer-sign div {text-align: center;}";
        public string ItemHeader = "th { white-space: nowrap;text-align: center;}  ";
        public string ItemHeaderVN = ".ItemHeaderVN { transform: scaleX(1);display: inline-block;}";
        public string ItemHeaderEN = ".ItemHeaderEN { transform: scaleX(1);display: inline-block;}";
        public string stylePDF = $@"
            .PDF {{
            width: 210mm !important;
            height: 297mm !important;
            padding: 10mm;
            z-index: 2;
            box-sizing: border-box;
            box-shadow: 0 0 15px rgba(0, 0, 0, 0.1); /* Hiệu ứng đổ bóng nhẹ */;
             }}
            ";


        public string Background = "";
        public string Logo = "";

        public HtmlCSS()
        {
        }

        public HtmlCSS(string fontFamily, float fontSize, string colorText = "", string colorBoreder = "")
        {
            Body = "html, body {" +
                "height: 100%;margin: 0;" +
                "display: flex; " +
                "align-items: center;   " +
                "justify-content: center; " +
                "background-color: #ffffff;" +
                "-webkit-print-color-adjust: exact; print-color-adjust: exact;" +
                $"font-family: {fontFamily};font-size: {fontSize}pt;" +
                $"color: {colorText};" +
                $"box-sizing: border-box;" +
                " }";
            itemHeaderEN = ".ItemHeaderEN { transform: scaleX(1);display: inline-block;" +
                "font-style: italic;" +
                $"font-size: {fontSize - 1}pt;" +
                $"font-weight: 100;" +
                "}";
            table = ".table {width: 100%;border-collapse: collapse;} " +
            " .table th,.table td" +
            $"{{border: 1pt solid {colorBoreder};" +
            $"height:{fontSize}pt;" +
            "padding: 5px; text-align: center;}";
    }

        public virtual string ToStyle(string backgroundImg, string logo, string stamp,string signer, out string style)
        {
            style = null;
            StringBuilder css = new StringBuilder();
            css.Append("<style>");
            css.Append(Body);

            string msg = GenerateStyleBackgroundImg(css, backgroundImg, "Background", " width: 210mm !important;height: 297mm!important;");
            if (msg.Length > 0) return msg;
            msg = GenerateStyleBackgroundImg(css, logo, "Logo");
            if (msg.Length > 0) return msg;
            msg = GenerateStyleBackgroundImg(css, stamp, "Stamp", "opacity: 0.7;");
            if (msg.Length > 0) return msg;
            msg = GenerateStyleBackgroundImg(css, signer, "Signer");
            if (msg.Length > 0) return msg;

            css.Append(stylePDF);
            css.Append(h1);
            css.Append(header);
            css.Append(info);
            css.Append(table);
            css.Append(footer);
            css.Append(footersign);
            css.Append(itemHeader);
            css.Append(itemHeaderEN);
            css.Append(itemHeaderVN);
            css.Append(Custom);
            css.Append(itemAlign);

            css.Append("</style>");
            style = css.ToString();

            return msg;

        }



        public void SetItemAlign(int[] align)
        {
            itemAlign = "table{width: 100%;border-collapse: collapse;}" +
                "td { padding: 10px; text-align: left;vertical-align: middle;}";
            for (int i = 0; i < align.Length; i++)
            {
                itemAlign += $"td:nth-child({i + 1})" +
                    $"{{text-align: {getAlign(align[i])};}}";
            }
        }
        public string getAlign(int align)
        {
            switch (align)
            {
                case 0: return "left";
                case 1: return "center";
                case 2: return "right";
                default: return "left";
            }
        }
        public virtual string GenerateStyleBackgroundImg(StringBuilder css, string backgroundImg, string className,string stylePlus="")
        {

            if (string.IsNullOrWhiteSpace(backgroundImg))
            {
                style = "";
                return string.Empty;
            }
            if (File.Exists(backgroundImg))
            {

                string msg = ImageToBase64(backgroundImg, out string imgBase64);

                style = $".{className}{{background-image: url('data:image/png;base64,{imgBase64}');background-repeat: no-repeat; background-size: contain; background-position: center;{stylePlus}}}";
                css.Append(style);
                return string.Empty;
            }
            style = $".{className}{{ background-image: url('{backgroundImg}');background-repeat: no-repeat; background-size: contain; background-position: center;{stylePlus}}}";
            css.Append(style);

            return string.Empty;
        }

        public static string ImageToBase64(string imgPath, out string imgBase64)
        {
            imgBase64 = string.Empty;
            try
            {
                byte[] imageBytes = File.ReadAllBytes(imgPath);
                imgBase64 = Convert.ToBase64String(imageBytes);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
    }

    

}
