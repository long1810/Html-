using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BES
{
    public class InvoiceHtml_SimpleSyntax : HtmlElement
    {
        //Chỉnh độ rộng từng cột
        public int[] TableWidth;
        //chỉnh độ co cho tằng **cell**
        public float[] HorizontalSacleX;
        public string[] ItemHeaderVN;
        public string[] ItemHeaderEN;
        public string[] ItemHeaderSub;
        public string[] ItemCustomer;
        public int[] ItemAlgin;

        public string ItemFooterCustomer;


        public InvoiceHtml_SimpleSyntax()
        {
        }
        public virtual string Init()
        {

            CSS = new HtmlCSS("Arial, sans-serif", 10, ColorText, BorderColor);
            ItemAlgin = new int[] { 1, 0, 1, 2, 2, 2 };
            CSS.SetItemAlign(ItemAlgin);

            TableWidth = new int[] { 8, 28, 10, 9, 15, 22 };
            HorizontalSacleX = new float[] { 1, 1, 1, 1, 1, 0.9f };
            ItemHeaderVN = new string[] { "STT", "Tên hàng hóa, dịch vụ", "ĐVT", "SL", "Đơn giá", "Thành tiền" };
            ItemHeaderEN = new string[] { "(No.)", "(Description)", "(Unit)", "(Quantity)", "(Unit Price)", "(Amount)" };
            ItemHeaderSub = new string[] { "1", "2", "3", "4", "5", "6 = 4 x 5" };

            ItemCustomer = new string[]
              {
                     DataKey.Item.ItemName,
                     DataKey.Item.UnitName,
                     DataKey.Item.Qty,
                     DataKey.Item.Price,
                     DataKey.Item.Amount,
              };
            return string.Empty;
        }


        public virtual string HTMLConveroPDF(string html, out byte[] data)
        {
            string msg = string.Empty;
            Puppeteer oPuppeteer = new Puppeteer();

            msg = oPuppeteer.DoGen_ContentHTML(html, out data, 1);
            return msg;
        }
        public virtual string CreateHMLT(out string PDF)
        {
            PDF = null;
            string msg = Init();
            if (msg.Length > 0) return msg;

            StringBuilder html = new StringBuilder();
            //Phần đầu HTMl
            html.Append(StartHTML);

            //Phần header
            msg += AddHeaderHTML(html);

            //Phần body
            msg += AddBody(html);

            //Phần cuối HTML
            html.Append(EndHTML);

            PDF = html.ToString();
            return msg;
        }

        public virtual string AddHeaderHTML(StringBuilder html)
        {
            string msg = CSS.ToStyle(BackgroundImage, LogoImage, StampImage, SignerImage, out string style);
            if (msg.Length > 0) return msg;

            html.Append("<head>");
            html.Append(Title);
            html.Append(style);
            html.Append("</head>");
            return msg;
        }
        public static string AddBackground(StringBuilder html)
        {
            html.Append("<div class='Background' style='position: fixed; bottom: 0%; left: 0%;  width: 210mm;height: 297mm; text-align: center;'>");

            html.Append("</div>");

            return html.ToString();
        }
        public virtual string AddBody(StringBuilder html)
        {
            string msg = string.Empty;

            AddBackground(html);
            //Phần body
            html.Append("<body class >");

            //Phần Logo, invoiceHeader, invoiceHeader_LastCell, seller
            html.Append("<div class='PDF'>");

            html.Append($"<div style='display: grid; grid-template-columns: 0.5fr 1.3fr 1fr; gap: 10px; border: 1pt solid {BorderColor};padding:10px;background-color: {BackgroundColor}'>");
            html.Append("<div class='Logo'>");
            html.Append("</div>");

            msg = Add_Header(html);
            if (msg.Length > 0) return msg;

            msg = Add_LastCell(html);
            if (msg.Length > 0) return msg;   
            html.Append("</div>");

            // MaCQT
            html.Append($"<div style=' border: 1pt solid {BorderColor};line-height: 1.4; padding:5px; border-top: none;'>");
            Add_MaCQT(html);
            html.Append("</div>");

            //Seller
            html.Append($"<div style=' border: 1pt solid {BorderColor};line-height: 1.4; padding:5px; border-top: none;'>");
            Add_Seller(html);
            html.Append("</div>");

            //Buyer
            html.Append($"<div style=' border: 1pt solid {BorderColor};line-height: 1.4; padding:5px; border-top: none; border-bottom: none;'>");
            Add_Buyer(html);
            html.Append("</div>");

            //Item
            html.Append("<table class='table' style='table-layout: fixed;width: 100%;border-collapse: collapse;' >");
            //Tiêu đề bảng hàng hóa itemHeader

            msg = Add_ItemHeader(html);
            if (msg.Length > 0) return msg;

            html.Append("<tbody>");
            //Hàng hóa item 
            msg = Add_Item(html);
            if (msg.Length > 0) return msg;
            //Tổng hợp bảng hàng hóa ItemFooter
            msg = Add_ItemFooter(html);
            if (msg.Length > 0) return msg;
            html.Append("</tbody>");

            html.Append("</table>");
            //phần chữ ký 
            Add_Signer(html);


            html.Append("</div>");

            AddPageFooter(html, "Hóa đơn", "0101360697");
            AddStamp(html);

            html.Append("</body>");

            return msg;
        }
        public virtual string Add_Header(StringBuilder html)
        {
            html.Append("<div>");
            string txt =
                "1|1|HÓA ĐƠN GIÁ TRỊ GIA TĂNG<br>|(VAT INVOICE)|<br>|C|40|T|false|16.b*14.i*14.n|16|1|0900|||0&" +
                "";
            string result = TextToHtml_Gird(html, txt);
            var invoiceDate = (DateTime)KeyValues[DataKey.InvoiceDate];

            txt = $"Ngày| (day)| {invoiceDate.ToString("dd")}&" +
                  $"tháng| (month)| {invoiceDate.ToString("MM")}&" +
                  $"năm| (year)| {invoiceDate.ToString("yyyy")}&" +
                    "";

            AddItemNIB(html, txt, 10, "n,i,b", "c", 1);
            html.Append("</div>");

            return result;
        }
        public virtual string Add_MaCQT(StringBuilder html)
        {
            string lastcell =
                    $"1|1|Mã của Cơ quan thuế: ||{DataKey.MaCuaCQT}|L|0|M|false|10.n*9.i*10.n|12|1|0100|||0&" +
                    $"";
            string msg = TextToHtml_Gird(html, lastcell);
            if (msg.Length > 0) return msg;
            return msg; ;
        }

        public virtual string Add_LastCell(StringBuilder html)
        {
            AddDivGird(html, new float[] { 1 }, 0, 0, "40px");
            string lastcell =
                       $"1|1|Mẫu số - Ký hiệu|(Serial No.):|{DataKey.InvoiceForm}{DataKey.InvoiceSerial}|L|0|M|false|10.n*9.i*10.b|11|1|0900|||0&" +
                       //$"1|1||EN| |L|0|M|false|10.b*9.i*10.b|10|1|3100|||0&" +
                       $"1|1|Số hóa đơn|(Invoice No.):| {DataKey.InvoiceNo}|L|0|T|false|10.n*9.i*10.b.255,0,0|11|1|0400|||0&" +
                       //$"1|1||EN| {DataKey.InvoiceNo}|L|0|T|false|10.b*9.i*10.b.255,0,0|10|1|3100|||0&" +
                       $"";
            string msg = TextToHtml_Gird(html, lastcell);
            if (msg.Length > 0) return msg;
            html.Append("</div>");
            return msg;
        }

        public virtual string Add_Seller(StringBuilder html)
        {

            AddDivGird(html, new float[] { 1 }, 0);
            string lastcell =
                       $"1|1|Đơn vị bán |(Seller): |{DataKey.UnitName}|L|0|M|false|10.n*9.i*10.n|13|1|3200|||0&" +
                       $"1|1|MST |(Tax Code): |{DataKey.TaxCode}|L|0|M|false|10.n*9.i*10.n|13|1|3200|||0&" +
                       $"1|1|Địa chỉ |(Address): |{DataKey.UnitAddress}|L|0|M|false|10.n*9.i*10.n|13|1|3200|||0&" +
                       $"1|1|Điện thoại |(Phone): |{DataKey.UnitPhone}|L|0|M|false|10.n*9.i*10.n|13|1|3200|||0&" +
                       $"1|1|STK |(Account No.): |{DataKey.BankAccount} {DataKey.BankName}|L|0|M|false|10.n*9.i*10.n|12|1|3200|||0&" +
                       $"";
            string msg = TextToHtml_Gird(html, lastcell);
            if (msg.Length > 0) return msg;
            html.Append("</div>");
            return msg;
        }
        public virtual string Add_Buyer(StringBuilder html)
        {
            AddDivGird(html, new float[] { 2, 8 }, 0, 0);
            string lastcell =
                       $"1|1|Người mua |(Buyer) |:|L|0|M|false|10.n*9.i*10.n|13|1|3200|||0&" +
                       $"1|1|||{DataKey.BuyerName}|L|0|M|false|10.n*9.i*10.n|13|1|3200|||0&" +

                       $"1|1|Đơn vị mua |(Tax Code)||L|0|M|false|10.n*9.i*10.n|13|1|3200|||0&" +
                       $"1|1||:|{DataKey.BuyerUnitName}|L|0|M|false|10.n*9.i*10.n|13|1|3200|||0&" +

                       $"1|1|Địa chỉ |(Address)|:|L|0|M|false|10.n*9.i*10.n|13|1|3200|||0&" +
                       $"1|1|||{DataKey.BuyerAddress}|L|0|M|false|10.n*9.i*10.n|13|1|3200|||0&" +

                       $"1|1|HTTT |(Pay method)|:|L|0|M|false|10.n*9.i*10.n|13|1|3200|||0&" +
                       $"1|1|||{DataKey.PayMethodID}|L|0|M|false|10.n*9.i*10.n|13|1|3200|||0&" +

                       $"1|1|STK |(Account No.)|:|L|0|M|false|10.n*9.i*10.n|12|1|3200|||0&" +
                       $"1|1|||{DataKey.BuyerBankAccount}|L|0|M|false|10.n*9.i*10.n|13|1|3200|||0&" +

                       $"";
            string msg = TextToHtml_Gird(html, lastcell);
            if (msg.Length > 0) return msg;
            html.Append("</div>");
            return msg;
        }
        public virtual string Add_ItemHeader(StringBuilder html)
        {
            string msg = string.Empty;
            if (TableWidth == null && TableWidth.Length < 0)
                return "Độ rộng cột chưa được cấu hình trong bảng hàng hóa";
            if (ItemHeaderVN != null && TableWidth.Length != ItemHeaderVN.Length)
                return "Cột và tiêu đề không hớp nhau có: " + TableWidth.Length + " và" + ItemHeaderVN.Length;
            //tạo mới nếu HorizontalSacleX != TableWidth
            if (HorizontalSacleX.Length != TableWidth.Length)
            {
                HorizontalSacleX = Enumerable.Repeat(1.0f, TableWidth.Length).ToArray();
            }
            html.Append("<thead>");
            //Độ rộng các cột 
            html.Append("<colgroup>");
            for (int i = 0; i < TableWidth.Length; i++)
            {
                html.Append($"<col style='width: {TableWidth[i]}px;'>");
            }
            html.Append("</colgroup>");

            //Phần tiêu đề cột
            html.Append("<thead>");
            //TextVN
            html.Append("<tr>");
            for (int i = 0; i < ItemHeaderVN.Length; i++)
            {
                string horizontal = $"transform: scaleX({HorizontalSacleX[i].ToString()})";
                html.Append($"" +
                    $"<th>" +
                    $"<span class='ItemHeaderVN' style='{horizontal}'>{ItemHeaderVN[i]}</span>" +
                    $"<br>" +
                    $"<span class='ItemHeaderEN' style='{horizontal}'>{ItemHeaderEN[i]}</span>" +
                    $"</th>");
            }
            html.Append("</tr>");
            html.Append("<tr>");
            for (int i = 0; i < ItemHeaderSub.Length; i++)
            {
                html.Append($"<th>{ItemHeaderSub[i]}</th>");
            }
            html.Append("</tr>");
            html.Append("</thead>");


            return msg;
        }

        public virtual string Add_Item(StringBuilder html)
        {
            string msg = string.Empty;

            for (int i = 0; i < KeyValueItems.Count; i++)
            {
                html.Append("<tr>");
                //stt 
                Add_td(html, (i + 1).ToString());
                Add_ItemFromHT(html, KeyValueItems[i], i);

                html.Append("</tr>");
            }

            Add_ItemBlank(html, 4);

            return msg;
        }

        public virtual string Add_ItemFromHT(StringBuilder html, Dictionary<string, object> itemDic, int i)
        {
            string msg = string.Empty;
            // Lặp qua các phần tử trong ItemCustomer
            for (int x = 0; x < ItemCustomer.Length; x++)
            {
                string key = ItemCustomer[x]; // Lấy key từ ItemCustomer
                if (itemDic.ContainsKey(key)) // Kiểm tra xem key có tồn tại trong itemDic
                {
                    string val = ""; // Lấy giá trị từ itemDic và chuyển thành chuỗi
                    var objValue = itemDic[key];
                    switch (objValue)
                    {
                        case string s: val = s; break;
                        case double d: val =  d.ToString("#,###", Cul.NumberFormat); break;
                        case int tempI: val = tempI.ToString("#,###", Cul.NumberFormat); break;
                        default: val = objValue?.ToString() ?? ""; break;
                    }
                    Add_td(html, val); // Gọi hàm Add_td để thêm HTML
                }
            }
            return msg;
        }
        public virtual string Add_ItemBlank(StringBuilder html, int time)
        {
            string msg = string.Empty;
            for (int i = 0; i < time; i++)
            {
                html.Append("<tr>");

                for (int x = 0; x < ItemCustomer.Length+1; x++)
                {
                    Add_td(html, "");
                }

                html.Append("</tr>");
            }

            return msg;
        }

        public virtual string Add_ItemFooter(StringBuilder html)
        {
            string msg = string.Empty;
            //html.Append("<tfoot>");
            string lastcell =
                      $"5|1|Tồng tiền |(Amount):||R|0|M|false|10.n*9.i*10.n|13|1|3433|||0&" +
                      $"1|1|||{ DataKey.ItemFooter.SumItemAmount}|R|0|M|false|10.n*9.i*10.n|13|1|3433|||0&" +
                      "^" +
                       $"5|1|Tồng tiền Thuế|(Tax Amount):||R|0|M|false|10.n*9.i*10.n|13|1|3433|||0&" +
                      $"1|1|||{ DataKey.ItemFooter.SumTaxAmount}|R|0|M|false|10.n*9.i*10.n|13|1|3433|||0&" +
                      "^" +
                       $"5|1|Tồng tiền Thanh toán|(Payment Amount):||R|0|M|false|10.n*9.i*10.n|13|1|3433|||0&" +
                      $"1|1|||{ DataKey.ItemFooter.SumPaymentAmount}|R|0|M|false|10.n*9.i*10.n|13|1|3433|||0&" +

                      $"";
            msg = TextToHtml_Tr(html, lastcell);

            //html.Append("</tfoot>");
            return msg;
        }

        public virtual string Add_Signer(StringBuilder html)
        {

            AddDivGird(html, new float[] { 1, 1 }, 0);
            string signer =
                       $"1|1|Người mua hàng |(Buyer) ||C|0|M|false|12.b*11.i*10.n|13|1|3200|||0&" +
                       $"1|1|Đơn vị bán |(Seller) ||C|0|M|false|12.b*11.i*10.n|13|1|3200|||0&" +
                       $"";

            string msg = TextToHtml_Gird(html, signer);
            if (msg.Length > 0) return msg;
            html.Append("<div> </div>");
            Add_CellSigner(html);


            html.Append("</div>");
            return msg;

        }
        public virtual string Add_CellSigner(StringBuilder html)
        {
            //string folderIMG = Path.Combine(Application.StartupPath, "PDFs").Replace(@"\bin\Debug\PDFs", "") + @"\IMG\";
            //string backgournd64 = ImgBase64.ImageToBase64(folderIMG + background);
            html.Append($"<div class='Signer'>");
            string signer =
                        $"1|1|Đã được ký điện tử bởi|<br>(Signed digitally by)||C|0|M|false|10.b*9.i*10.n|13|1|3200|||0&" +
                        $"1|1|{DataKey.UnitName} | ||C|0|M|false|10.b*11.i*10.n|13|1|3200|||0&" +
                        $"1|1|Ngày: {DataKey.InvoiceDate} | ||C|0|M|false|10.n*11.i*10.n|13|1|3200|||0&" +

                        $"";

            string msg = TextToHtml_Gird(html, signer);
            if (msg.Length > 0) return msg;

            html.Append("</div>");

            return msg;
        }

        public virtual string AddPageFooter(StringBuilder html, string codeHD, string mst)
        {
            html.Append("<div style='position: fixed; bottom: 0; left: 0; width: 100%; text-align: center; font-size: 10px; padding: 5px 0'>");
            html.Append("<div style='padding-bottom: 35px;'>(Cần kiểm tra đối chiếu khi lập, giao, nhận hóa đơn)</div>");
            html.Append($"<div><b>Giải pháp {codeHD} Điện tử</b> được cung cấp bởi <b>Công ty Cổ phần Bkav</b> - MST {mst} - ĐT 1900 545414 - <b>http://ehoadon.vn</b></div>");
            html.Append($"<div style='margin-top: 3px;'>Hóa đơn Điện tử (HĐĐT) được tra cứu trực tuyến tại <b>http://tracuu.ehoadon.vn.</b> Mã tra cứu HĐĐT này: N1FDQC</div>");
            html.Append("</div>");

            return html.ToString();
        }
        public static string AddStamp(StringBuilder html)
        {
            html.Append("<div class='Stamp' style='position: fixed; bottom: 30%; left: 21%; width: 500px;height:375px; text-align: center; font-size: 10px; padding: 5px 0'>");

            html.Append("</div>");

            return html.ToString();
        }
        public virtual string AddItemNIB(StringBuilder html, string setting, float fontSize = 10, string font = "n,i,b", string align = "L", int padding = 0)
        {
            string msg = string.Empty;
            html.Append($"<div style='{CustomCellSettings.ParseToHorizontalAlignment(align)} '>");
            string[] items = setting.Split('&');
            string[] fonts = GetFont(fontSize, font);
            for (int i = 0; i < items.Length; i++)
            {
                string[] item = items[i].Split('|');
                if (items.Length > 2)
                {
                    html.Append($"" +
                        $"<span style='{fonts[0]}'>{item[0]}</span> " +
                        $"<span style='{fonts[1]}'>{item[1]}</span> " +
                        $"<span style='{fonts[2]} padding-right:{padding}pt'>{item[2]}</span> ");
                }
            }
            html.Append("</div>");
            return msg;
        }
        public string[] GetFont(float fontSize = 10, string font = "n,i,b")
        {
            string[] items = font.Split(',');
            //string [] fontstle = 
            string[] Fonts = new string[]
            {
                $"font-size: {fontSize}pt;" + CustomCellSettings.getFontWeight(items[0]),
                $"font-size: {fontSize-1}pt;" +CustomCellSettings.getFontWeight(items[1]),
                $"font-size: {fontSize}pt;" + CustomCellSettings.getFontWeight(items[2]),
            };
            return Fonts;
        }


        public string GetFont(string font)
        {
            switch (font)
            {
                case "n": return "font-weight: 400;";
                case "b": return "font-weight: 700;";
                case "i": return "font-weight: 400;font-style: italic;";
                case "ib": return "font-weight: 700;font-style: italic;";

                default: return "font-weight: 400;";
            }
        }
    }
}
