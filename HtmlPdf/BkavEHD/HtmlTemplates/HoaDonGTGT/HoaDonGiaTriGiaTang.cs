using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BES
{
    public class HoaDonGiaTriGiaTang : InvoiceHtml_Base
    {
        public HoaDonGiaTriGiaTang()
        {
            CSS = new HtmlCSS();
        }

        protected override string AddLogo(StringBuilder html, string logoImg, float height, float width, float opacity)
        {
            string Logo64 = ImgBase64.ImageToBase64(logoImg);

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

        protected override string AddBackgroundImg(StringBuilder html, string pathLogo)
        {
            string folderIMG = Path.Combine(Application.StartupPath, "PDFs").Replace(@"\bin\Debug\PDFs", "") + @"\IMG\";
            string logoBase64 = ImgBase64.ImageToBase64(folderIMG + pathLogo);

            string logoHtml = $@"
             <div style='
             position: fixed;
             top: 45%;
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

        public static string AddTable(int[] array)
        {
            return $"grid-template-columns: {string.Join(" ", array.Select(item => $"{item}fr"))};";
        }

        public static string AddItemHeadTable(StringBuilder html, string backgroudColor, params string[] headers)
        {
            html.Append($"<thead  style='background-color: {backgroudColor}'>");
            html.Append("<tr>");

            foreach (var header in headers)
            {
                // Tách phần text chính và phần nháy ()
                var parts = header.Split('(');
                var mainText = parts[0].Trim(); // Phần chính
                var subText = parts.Length > 1 ? $"({parts[1]}" : ""; // Phần trong dấu nháy ()

                html.Append("<th style='text-align: center;'>");
                html.Append($"<div>{mainText}</div>"); // Phần chữ chính
                if (!string.IsNullOrEmpty(subText))
                {
                    html.Append($"<div style='font-style: italic; font-weight: normal; font-size: 0.9em;'>{subText}</div>");
                }
                html.Append("</th>");
            }

            html.Append("</tr>");
            html.Append("</thead>");

            return html.ToString();
        }

        public static string AddItemBodyTable(StringBuilder html, params string[] values)
        {
            html.Append("<tbody>");
            html.Append($"<tr>");

            foreach (var value in values)
            {
                html.AppendFormat("<td>{0}</td>", value);
            }

            html.Append("</tr>");
            html.Append("</tbody>");

            return html.ToString();
        }

        public static string AddItemBodyTable2(StringBuilder html, String br, String Subtoal, String value, int colspan, int colspan2)
        {
            html.Append($"<tbody  style='background-color:{br}'>");
            html.Append("<tr>");

            html.Append($"<td colspan={colspan} style=' text-align: right;'>{Subtoal}:</td>");
            html.Append($"<td colspan={colspan2} style='text-align: right;'>{value}</td>");

            html.Append("</tr>");
            html.Append("</tbody>");

            return html.ToString();
        }

        public static string AddItemBodyTable4(StringBuilder html, String br, String Tax, String taxValue, String vatAmount, String vatValue, int colspan, int colspan2, int colspan3)
        {
            html.Append($"<tbody  style='background-color:{br}'>");
            html.Append("<tr>");
            html.Append($"<td colspan={colspan} style=' text-align: left;'>{Tax}: {taxValue}</td>");
            html.Append($"<td colspan={colspan2} style='text-align: right;'>{vatAmount}</td>");
            html.Append($"<td colspan={colspan3} style='text-align: right;'>{vatValue}</td>");
            html.Append("</tr>");
            html.Append("</tbody>");

            return html.ToString();
        }
        public static string AddItemBodyTable3(StringBuilder html, String br, String Subtoal, String value, int colspan)
        {
            html.Append($"<tbody  style='background-color:{br}'>");
            html.Append("<tr>");
            html.Append($"<td colspan={colspan} style='text-align: left;'>{Subtoal}: {value}</td>");
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


        public virtual string AddInvoiceHeader(StringBuilder html, string title, string subtitle, string day, string month, string year, int caseType)
        {
            html.Append("<div style=\"text-align: center\">");
            html.Append($"<b style=\"font-size: 22px;white-space: nowrap;\">{title}</b>");
            html.Append($"<div style=\"font-size: 17px;\">{subtitle}</div>");
            html.Append("<div style=\"display: flex; justify-content: center; gap: 20px; margin-top: 10px;\">");

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
        public static string AddInvoiceHeader2(StringBuilder html, string serial, string invoice)

        {
            html.Append("<div style=\"text-align: left;font-size: 12px;line-height: 1.4;\" > ");
            html.Append($"<div>Mẫu số - Ký hiệu (Serial No.):<b> {serial}</b> </div>");
            html.Append($"<div >Số (Invoice No.): <b>{invoice}</b></div>");
            html.Append("</div>");
            return html.ToString();
        }

        public static string AddSingerItem(StringBuilder html, string background, float height, float width, string signedText, string signedByText, string companyName, string signedDate)
        {
            string folderIMG = Path.Combine(Application.StartupPath, "PDFs").Replace(@"\bin\Debug\PDFs", "") + @"\IMG\";
            string backgournd64 = ImgBase64.ImageToBase64(folderIMG + background);

            html.Append($"<div style=\"text-align: center;background-repeat: ROUND;padding:5px; background-image: url('data:image/png;base64,{backgournd64}') \">");
            html.Append($"    <p>{signedText}</p>");
            html.Append($"    <p>({signedByText})</p>");
            html.Append($"    <p>{companyName}</p>");
            html.Append($"    <p>Ngày ký: {signedDate}</p>");
            html.Append("</div>");

            return string.Empty;
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

        public static void AddInfo(StringBuilder html, string label, string value)
        {
            html.Append("<div style='display: grid; grid-template-columns: 1fr 4.5fr;'>");
            html.AppendFormat("<div>{0}: </div>", label);
            html.AppendFormat("<b  style=''>{0}</b>", value);
            html.Append("</div>");
        }

        public static void AddInfo2(StringBuilder html, string label, string value, string label2, string value2)
        {
            html.Append("<div style='display: flex'>");

            html.AppendFormat($"<div style='padding-right: 21px;' >{label}: </div>");
            html.AppendFormat($"<b >{value}</b>");
            html.AppendFormat($"<div style='padding: 0px 15px'>{label2}: </div>");
            html.AppendFormat($"<b>{value2}</b>");
            html.Append("</div>");
        }

        public string borderColor = "#807def";
        public string backGroudColor = "white";
        public virtual string CreateHMLT()
        {
            string msg = string.Empty;
            StringBuilder html = new StringBuilder();
            html.Append("<!DOCTYPE html> <html lang='en'> <head> <meta charset='UTF-8'> <meta name='viewport' content='width=device-width, initial-scale=1.0'> <title>Document</title>");
            html.Append(CSS.ToStyle(BackgroundImage));
            AddBackgroundImg(html, "BIENLAIMAU.png");
            html.Append(" </head>");
            html.Append("<body > ");
            html.Append("<div class='PDF'>");
            html.Append($"<div style='display: grid; grid-template-columns: 0.5fr 1.3fr 1fr; gap: 10px; border: 1px solid {borderColor};padding:10px'>");

            //AddLogo(html, "Logo_SongDaVietDuc.png", 70, 100, 1f);
            html.Append("<div></div>");
            AddInvoiceHeader(html, "HÓA ĐƠN GIÁ TRỊ GIA TĂNG", "(VAT INVOICE)", "", "", "", 1);
            AddInvoiceHeader2(html, "2C24TAA", "0000000");
            html.Append("</div>");

            // TaxCODE
            html.Append($"<div style=' border: 1px solid {borderColor};line-height: 1.4; padding:5px; border-top: none;'>");
            AddTaxCode(html, "Mã của Cơ quan thuế", "");
            html.Append("</div>");

            //Seller
            html.Append($"<div style=' border: 1px solid {borderColor};line-height: 1.4; padding:5px; border-top: none;'>");
            AddInfo(html, "Đơn vị bán (Seller)", "Công ty Cổ phần ABC");
            AddInfo(html, "MST (Tax Code)", "0123456789");
            AddInfo(html, "Địa chỉ (Address)", "Tầng 2, tòa nhà HH1, Cầu Giấy, TP. Hà Nội");
            AddInfo(html, "Điện thoại (Phone)", "024 1234 5678");
            AddInfo(html, "STK (Account No.)", "ABC");
            html.Append("</div>");

            //Buyer
            html.Append($"<div style=' border: 1px solid {borderColor};line-height: 1.4; padding:5px; border-top: none; border-bottom: none;'>");
            AddInfo2(html, "Người mua (Buyer)", "", "CCCD (Citizen ID No.)", "");
            AddInfo(html, "Đơn vị (Ca. name)", "");
            AddInfo(html, "MST (Tax Code)", "");
            AddInfo(html, "Địa chỉ (Address)", "");
            AddInfo(html, "HTTT (Pay method)", "");
            AddInfo(html, "STK (Account No.)", "");
            html.Append("</div>");

            //Table
            html.Append("<table class='table'>");
            AddItemHeadTable(html, "white", "STT (No.)", "Tên hàng hóa, dịch vụ (Description)", "ĐVT (Unit)", "SL (Quantity)", "Đơn giá (Unit Price)", "Thành tiền (Amount)");
            AddItemBodyTable(html, "1", "2", "3", "4", "5", "6 = 4 x 5");
            for (int i = 0; i < 15; i++)
            {
                AddItemBodyTable(html, "1", "2.644.545", "2.644.545", "10%", "264.455", "264.455");
            }
            AddItemBodyTable2(html, backGroudColor, "Cộng tiền hàng (Sub total)", "", 5, 1);
            AddItemBodyTable4(html, backGroudColor, "Thuế suất GTGT (Tax rate)", "10%", "Cộng tiền thuế GTGT (VAT amount)", "", 2, 3, 1);
            AddItemBodyTable2(html, backGroudColor, "Tổng cộng tiền thanh toán (Total payment)", "", 5, 1);
            AddItemBodyTable3(html, backGroudColor, "Số tiền viết bằng chữ (Amount in words)", "", 6);
            html.Append("   </table>");


            html.Append("<div style='display: grid; grid-template-columns: 1fr 1fr; gap: 10px;padding-top:10px'>");
            html.Append("<div style='text-align: center; font-weight: bold'>Người mua hàng <span style=\"font-weight: normal;\">(Buyer)</span></div>");
            html.Append("<div style='text-align: center; font-weight: bold'>Người bán hàng <span style=\"font-weight: normal;\">(Seller)</span>");
            AddSingerItem(html, "SIGNING.png", 150, 300, "Đã được ký điện tử bởi", "Signed digitally by", "CÔNG TY CỔ PHẦN BKAV", "Ngày 17/2/2023");
            html.Append("</div>");
            html.Append("</div>");

            AddPageFooter(html, "Hóa đơn", "0101360697");


            html.Append("</body>");
            html.Append("</html>");


            return html.ToString();
        }
    }

}
