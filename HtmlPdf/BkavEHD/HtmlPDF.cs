using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BES
{
    public class HtmlPDF : HtmlElement
    {
        public HtmlPDF()
        {
            CSS = new HtmlCSS();
        }
        public virtual string createHeader()
        {
            StringBuilder html = new StringBuilder();

            return html.ToString();
        }

        public string AddLogo(StringBuilder html, string pathLogo, float height, float width, float opacity)
        {
            string folderPath = Path.Combine(Application.StartupPath, "PDFs");
            string folderIMG = folderPath.Replace(@"\bin\Debug\PDFs", "") + @"\IMG\";

            string Logo64 = ImgBase64.ImageToBase64(folderIMG + pathLogo);

            html.Append($"<img src='data:image/jpeg;base64,{Logo64}' style='height:{height}px; width:{width}px; opacity:{opacity};' />");
            return html.ToString();
        }

        public string AddSignerPDF(StringBuilder html, string pathLogo, float height, float width, float opacity)
        {
            string folderPath = Path.Combine(Application.StartupPath, "PDFs");
            string folderIMG = folderPath.Replace(@"\bin\Debug\PDFs", "") + @"\IMG\";

            string Logo64 = ImgBase64.ImageToBase64(folderIMG + pathLogo);

            html.Append($"<div style='text-align: right;'><img src='data:image/jpeg;base64,{Logo64}' style='height:{height}px; width:{width}px; opacity:{opacity};' /></div>");

            return html.ToString();
        }




        public string AddTem(StringBuilder html, string pathLogo)
        {
            string folderIMG = Path.Combine(Application.StartupPath, "PDFs").Replace(@"\bin\Debug\PDFs", "") + @"\IMG\";
            string logoBase64 = ImgBase64.ImageToBase64(folderIMG + pathLogo);

            string logoHtml = $@"
             <div style='
             position: fixed;
             top: 50%;
             left: 50%;
             transform: translate(-50%, -50%);
             display: flex;
             justify-content: center;
             align-items: center;
             width: 100%;
             height: 100%;
             z-index: 9999; /* Đảm bảo logo ở lớp trên cùng */
             pointer-events: none; /* Cho phép nhấp chuột qua logo mà không ảnh hưởng */
             '>
             <img src='data:image/png;base64,{logoBase64}' style='max-width: 50%; max-height: 50%; object-fit: contain;' />
             </div>";
            html.Append(logoHtml);

            return html.ToString();
        }

        public string AddBackGround(StringBuilder html, string pathLogo)
        {
            string folderIMG = Path.Combine(Application.StartupPath, "PDFs").Replace(@"\bin\Debug\PDFs", "") + @"\IMG\";
            string logoBase64 = ImgBase64.ImageToBase64(folderIMG + pathLogo);

            string logoHtml = $@"
        <div style='
            position: fixed;
            top: 100;
            left: 100;
            width: 100%;
            height: 100%;
            z-index: -1; /* Đảm bảo logo ở lớp dưới */
            pointer-events: none; /* Cho phép nhấp chuột qua logo mà không ảnh hưởng */
            '>
            <img src='data:image/png;base64,{logoBase64}' style='width: 100%; height: ; object-fit: cover;' />
        </div>";
            html.Append(logoHtml);

            return html.ToString();
        }

        public static string AddTable(int[] array)
        {
            return $"grid-template-columns: {string.Join(" ", array.Select(item => $"{item}fr"))};";
        }

        public static string AddItemHeadTable(StringBuilder html, params string[] headers)
        {
            html.Append("<thead>");
            html.Append("<tr>");

            foreach (var header in headers)
            {
                html.AppendFormat($"<th>{header}</th>");
            }

            html.Append("</tr>");
            html.Append("</thead>");

            return html.ToString();
        }

        public static string AddItemBodyTable(StringBuilder html, params string[] values)
        {
            html.Append("<tbody >");
            html.Append("<tr>");

            foreach (var value in values)
            {
                html.AppendFormat("<td>{0}</td>", value);
            }

            html.Append("</tr>");
            html.Append("</tbody>");

            return html.ToString();
        }

        public static string AddItem(StringBuilder html, object data)
        {
            foreach (var property in data.GetType().GetProperties())
            {
                var value = property.GetValue(data)?.ToString() ?? "";
                html.Append($"<div >{value}</div>"); // Thêm vào thẻ <div>
            }

            html.Append("</div>");

            return html.ToString();
        }
        public static string AddTaxCode(StringBuilder html, string taxCode, string value)
        {
            html.Append($"<div>{taxCode}: </div>");
            html.Append($"<div>{value}</div>");
            return html.ToString();
        }


        public static string AddInvoiceHeader(StringBuilder html, string title, string subtitle, int day, int month, int year, int caseType)
        {
            html.Append("<div style=\"text-align: center\">");
            html.Append($"<b style=\"font-size: 22px;white-space: nowrap;\">{title}</b>");
            html.Append($"<div style=\"font-size: 17px;\">{subtitle}</div>");
            html.Append("<div style=\"display: flex; justify-content: center; gap: 10px; margin-top: 10px;\">");

            switch (caseType)
            {
                case 1:
                    html.Append($"<div>Ngày (day) {day}</div>");
                    html.Append($"<div>tháng (month) {month}</div>");
                    html.Append($"<div>năm (year) {year}</div>");
                    break;
                case 2:
                    html.Append($"<div>Ngày {day}</div>");
                    html.Append($"<div>tháng {month}</div>");
                    html.Append($"<div>năm {year}</div>");
                    break;
                case 3:
                    html.Append($"<div>{day}/{month}/{year}</div>");
                    break;
                default:
                    throw new ArgumentException("Chưa nhập typeDate");
            }

            html.Append("</div>");
            html.Append("</div>");

            return html.ToString();
        }

        public static string AddSingerItem(StringBuilder html, string background, float height, float width, string signedText, string signedByText, string companyName, string signedDate)
        {
            string folderIMG = Path.Combine(Application.StartupPath, "PDFs").Replace(@"\bin\Debug\PDFs", "") + @"\IMG\";
            string backgournd64 = ImgBase64.ImageToBase64(folderIMG + background);

            html.Append($"<div style=\"text-align: center;background-repeat: ROUND; background-image: url('data:image/png;base64,{backgournd64}') \">");
            html.Append($"    <p>{signedText}</p>");
            html.Append($"    <p>({signedByText})</p>");
            html.Append($"    <p>{companyName}</p>");
            html.Append($"    <p>Ngày ký: {signedDate}</p>");
            html.Append("</div>");

            return html.ToString();
        }

        public static string AddPageFooter(StringBuilder html, string codeHD, string mst)
        {
            html.Append("<div style='position: fixed; bottom: 0; left: 0; width: 100%; text-align: center; font-size: 10px; padding: 5px 0'>");
            html.Append("<div style='padding-bottom: 25px;'>(Cần kiểm tra đối chiếu khi lập, giao, nhận hóa đơn)</div>");
            html.Append($"<div><b>Giải pháp {codeHD} Điện tử</b> được cung cấp bởi <b>Công ty Cổ phần Bkav</b> - MST {mst} - ĐT 1900 545414 - <b>http://ehoadon.vn</b></div>");
            html.Append($"<div style='margin-top: 3px;'>Hóa đơn Điện tử (HĐĐT) được tra cứu trực tuyến tại <b>http://tracuu.ehoadon.vn.</b> Mã tra cứu HĐĐT này: N1FDQC</div>");
            html.Append("</div>");

            return html.ToString();
        }


        // style='background-color: red,  background-color: white;'
        //public string HMTL = "";
        public virtual string CreateHMLT()
        {
            string msg = string.Empty;
            StringBuilder html = new StringBuilder();
            html.Append("<!DOCTYPE html> <html lang='en'> <head> <meta charset='UTF-8'> <meta name='viewport' content='width=device-width, initial-scale=1.0'> <title>Document</title>");
            html.Append(CSS.ToStyle(BackgroundImage));
            AddTem(html, "BIENLAIMAU.png");
            //AddBackGround(html, "Background_HANKYU_HANSHIN.png");
            html.Append("<body style='background-color: red,  background-color: white;'>");
            html.Append("<div class='PDF'>");
            //html.Append("<div style='display: grid; grid-template-columns: 1fr 3fr 1fr; gap: 10px; height: 2%; border-bottom: 1px solid #000;'>");

            ////AddLogo(html, "Logo_SongDaVietDuc.png", 70, 100, 1f);
            ////AddInvoiceHeader(html, "HÓA ĐƠN GIÁ TRỊ GIA TĂNG", "(VAT INVOICE)", 13, 7, 2023, 1);

            ////html.Append("<div>InvoiceHeader</div>");
            //html.Append("</div>");


            //html.Append("<div style='display: grid; grid-template-columns: 1fr 2fr; gap: 10px; height: 2%; border-bottom: 1px solid #000;'>");
            //AddTaxCode(html, "Mã của Cơ quan thuế", "0060538E5C96CA4A46B37CDFB2DEC364FE");
            //html.Append("</div>");

            //html.Append("<div style='display: grid; grid-template-columns: 1fr 2fr; gap: 10px; height: 20%; border-bottom: 1px solid #000;'>");
            //html.Append("<div>Đơn vị bán (Seller): </div>");
            //html.Append("<div>BỆNH VIỆN ĐA KHOA NAM LIÊN CHIỂU</div>");
            //html.Append("</div>");

            html.Append("<div style=' padding-top:70px'>");
            //html.Append("<div>Đơn vị bán (Seller): </div>");
            //html.Append("<div>BỆNH VIỆN ĐA KHOA NAM LIÊN CHIỂU</div>");
            html.Append("</div>");


            html.Append("   <table class='table'>");

            AddItemHeadTable(html, "STT (No.)", "Tên hàng hóa, dịch vụ (Description)", "SL (Quantity)", "Đơn giá (Unit Price)", "Thành tiền (Amount)", "Thuế suất (Tax Rate)", "Tiền thuế (Tax Amount)");
            for (int i = 0; i < 50; i++)
            {
                AddItemBodyTable(html, "1", "2.644.545", "2.644.545", "10%", "264.455", "264.455", "264.455");
            }


            html.Append("   </table>");

            html.Append("<div style='display: grid; grid-template-columns: 1fr 3fr 1fr 1fr 2fr 2fr 2fr 2fr; gap: 10px; border-bottom: 1px solid #000;'>");
            html.Append("<div style='grid-column: span 6; text-align: right;'>Tổng:</div>");
            html.Append("<div style='grid-column: span 2; text-align: right;'>100000000</div>");
            html.Append("<div style='grid-column: span 6; text-align: right;'>Tổng:</div>");
            html.Append("<div style='grid-column: span 2; text-align: right;'>100000000</div>");
            html.Append("<div style='grid-column: span 6; text-align: right;'>Tổng:</div>");
            html.Append("<div style='grid-column: span 2; text-align: right;'>100000000</div>");
            html.Append("</div>");

            html.Append("<div style='display: grid; grid-template-columns: 1fr 1fr; gap: 10px; border-bottom: 1px solid #000; '>");
            html.Append("<div style='text-align: center;'>Người mua hàng (Buyer)</div>");
            html.Append("<div style='text-align: center;'>Người bán hàng (Seller)");
            //AddSignerPDF(html, "SIGNING.png", 150, 300, 1);
            AddSingerItem(html, "SIGNING.png", 150, 300, "Đã được ký điện tử bởi", "Signed digitally by", "CÔNG TY CỔ PHẦN BKAV", "Ngày 17/2/2023");
            html.Append("</div>");
            html.Append("</div>");

            //AddPageFooter(html, "Hóa đơn", "0101360697");


            html.Append("</body>");
            html.Append("</html>");


            return html.ToString();
        }
    }

}
