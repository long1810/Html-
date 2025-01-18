using System.Text;

namespace BES
{
    public class HoaDonGTGT_2 : InvoiceHtml_Base
    {
        public HoaDonGTGT_2()
        {
            CSS = new HtmlCSS();
        }

        public string br_HD2 = "#807def";
        public string bg_HD2 = "#9ec8fb";
        public string br2_HD2 = "#d4e1f1";

        public override string CreateHMLT(out string PDF)
        {
            string msg = string.Empty;
            StringBuilder html = new StringBuilder();
            html.Append("<!DOCTYPE html> <html lang='en'> <head> <meta charset='UTF-8'> <meta name='viewport' content='width=device-width, initial-scale=1.0'> <title>Document</title>");
            html.Append(CSS.ToStyle(BackgroundImage));
            AddBackgroundImg(html, "BIENLAIMAU.png");
            html.Append(" </head>");
            html.Append("<body > ");
            html.Append("<div class='PDF'>");
            html.Append($"<div style='display: grid; grid-template-columns: 0.5fr 1.3fr 1fr; gap: 10px; border: 1px solid {br_HD2};padding:10px;background-color: {bg_HD2};'>");

            //AddLogo(html, "Logo_SongDaVietDuc.png", 70, 100, 1f);
            html.Append("<div></div>");
            AddInvoiceHeader(html, "HÓA ĐƠN GIÁ TRỊ GIA TĂNG", "(VAT INVOICE)", "", "", "", 1);
            AddInvoiceHeader2(html, "2C24TAA", "0000000");
            html.Append("</div>");

            //Seller
            html.Append($"<div style=' border: 1px solid {br_HD2};line-height: 1.4; padding:5px; border-top: none;'>");
            AddInfo(html, "Đơn vị bán (Seller)", "Công ty Cổ phần ABC");
            AddInfo(html, "MST (Tax Code)", "0123456789");
            AddInfo(html, "Địa chỉ (Address)", "Tầng 2, tòa nhà HH1, Cầu Giấy, TP. Hà Nội");
            AddInfo(html, "Điện thoại (Phone)", "024 1234 5678");
            AddInfo(html, "STK (Account No.)", "ABC");
            html.Append("</div>");

            //Buyer
            html.Append($"<div style=' border: 1px solid {br_HD2};line-height: 1.4; padding:5px; border-top: none; border-bottom: none;'>");
            AddInfo2(html, "Người mua (Buyer)", "", "CCCD (Citizen ID No.)", "");
            AddInfo(html, "Đơn vị (Ca. name)", "");
            AddInfo(html, "MST (Tax Code)", "");
            AddInfo(html, "Địa chỉ (Address)", "");
            AddInfo(html, "HTTT (Pay method)", "");
            AddInfo(html, "STK (Account No.)", "");
            html.Append("</div>");

            //Table
            html.Append("<table class='table'>");
            AddItemHeadTable(html, bg_HD2, "STT (No.)", "Tên hàng hóa, dịch vụ (Description)", "ĐVT (Unit)", "SL (Quantity)", "Đơn giá (Unit Price)", "Thành tiền (Amount)");
            AddItemBodyTable(html, "1", "2", "3", "4", "5", "6 = 4 x 5");
            for (int i = 0; i < 15; i++)
            {
                AddItemBodyTable(html, "1", "2.644.545", "2.644.545", "10%", "264.455", "264.455");
            }
            AddItemBodyTable2(html, br2_HD2, "Cộng tiền hàng (Sub total)", "", 5, 1);
            AddItemBodyTable4(html, br2_HD2, "Thuế suất GTGT (Tax rate)", "10%", "Cộng tiền thuế GTGT (VAT amount)", "", 2, 3, 1);
            AddItemBodyTable2(html, br2_HD2, "Tổng cộng tiền thanh toán (Total payment)", "", 5, 1);
            AddItemBodyTable3(html, br2_HD2, "Số tiền viết bằng chữ (Amount in words)", "", 6);
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
            PDF = html.ToString();

            return msg;
        }



    }

}
