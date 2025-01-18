//using System.Text;

//namespace BESl
//{
//    public class InvoiceHtml_V1 : InvoiceHtml_SimpleSyntax
//    {
//        public override string AddBody(StringBuilder html)
//        {
//            string msg = string.Empty;
//            //Ph?n body
//            html.Append("<body style='background-color: rgb(255 255 255);'>");

//            AddBackground(html);

//            //Ph?n Logo, invoiceHeader, invoiceHeader_LastCell, seller
//            html.Append("<div style='position: fixed; bottom: 20pt; left: 20pt; width: 195mm;height: 297mm; text-align: center;'>");

//            html.Append($"<div style='display: grid; grid-template-columns: 0.5fr 1.3fr 1fr; gap: 10px; border: 1pt solid {BorderColor};padding:10px;background-color: {BackgroundColor}'>");
//            html.Append("<div class='Logo'>");
//            html.Append("</div>");

//            msg = Add_Header(html);
//            if (msg.Length > 0) return msg;

//            msg = Add_LastCell(html);
//            if (msg.Length > 0) return msg;
//            html.Append("</div>");

//            // MaCQT
//            html.Append($"<div style=' border: 1pt solid {BorderColor};line-height: 1.4; padding:5px; border-top: none;'>");
//            Add_MaCQT(html);
//            html.Append("</div>");

//            //Seller
//            html.Append($"<div style=' border: 1pt solid {BorderColor};line-height: 1.4; padding:5px; border-top: none;'>");
//            Add_Seller(html);
//            html.Append("</div>");

//            //Buyer
//            html.Append($"<div style=' border: 1pt solid {BorderColor};line-height: 1.4; padding:5px; border-top: none; border-bottom: none;'>");
//            Add_Buyer(html);
//            html.Append("</div>");

//            html.Append("</div>");
//            html.Append("<div class='PDF'>");
          
//            //html.Append("<div style='margin-top:100px'>");
//            //html.Append("</div>");
//            //Item
//            html.Append("<table class='table' style='table-layout: fixed;width: 100%;border-collapse: collapse;margin-top:300px' >");
//            //Tiêu ?? b?ng hàng hóa itemHeader

//            msg = Add_ItemHeader(html);
//            if (msg.Length > 0) return msg;

//            html.Append("<tbody>");
//            //Hàng hóa item 
//            msg = Add_Item(html);
//            if (msg.Length > 0) return msg;
//            //T?ng h?p b?ng hàng hóa ItemFooter
//            msg = Add_ItemFooter(html);
//            if (msg.Length > 0) return msg;
//            html.Append("</tbody>");

//            html.Append("</table>");
//            //ph?n ch? ký 
//            Add_Signer(html);


//            html.Append("</div>");

//            AddPageFooter(html, "Hóa ??n", "0101360697");
//            AddStamp(html);

//            html.Append("</body>");

//            return msg;
//        }


//    }


//}
