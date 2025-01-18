
using BES;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CreatePDF_HTML();
            //this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public string CreatePDF_HTML()
        {

            string msg = "";
            InvoiceHtml_SimpleSyntax t1 = new InvoiceHtml_SimpleSyntax();

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("InvoiceGUID", "feac315f-75f1-41ad-bbd3-e8f47f90839e");
            dict.Add("InvoiceDetailID", "1628871836");
            dict.Add("InvoiceDetailGUID", "8f0d9509-beea-4b81-a053-d938ac9021eb");
            dict.Add("CreateDate", "10/25/2024 5:27:29 PM");
            dict.Add("LastUpdate", "10/25/2024 5:27:29 PM");
            dict.Add("InvoiceID", "368172883");
            dict.Add("IsDiscount", "False");
            dict.Add("ItemName", "Thức ăn nhanh đặc biệt");
            dict.Add("UnitName", "Miếng");
            dict.Add("Qty", "10");
            dict.Add("Price", "46000");
            dict.Add("Amount", "4600000");
            dict.Add("TaxRateID", "4");
            dict.Add("TaxRate", "-1");
            dict.Add("TaxName", "Không chịu thuế");
            dict.Add("TaxAmount", "0");
            dict.Add("DiscountRate", "0");
            dict.Add("DiscountAmount", "0");
            dict.Add("SVChargeAmount", "0");
            dict.Add("OtherAmount", "0");
            dict.Add("ItemCode", "");
            dict.Add("ItemTypeID", "0");
            dict.Add("UserDefineDetails", "");
            dict.Add("IsIncrease", "");
            dict.Add("ItemNote", "");
            dict.Add("InvoiceDetailRelationID", "0");

            //t1.KeyValues = new Hashtable<string, object>();

            //t1.KeyValues.Add("@IsTaxDetails@", "False");
            //t1.KeyValues.Add("@UserDefine@", "");
            //t1.KeyValues.Add("@CacheNoteRemoveHTML@", "");
            //t1.KeyValues.Add("@ReceiverAddress@", "");
            //t1.KeyValues.Add("@IsDiscountDetails@", "False");
            //t1.KeyValues.Add("@LastUpdate@", "10/28/2024 11:21:21 AM");
            //t1.KeyValues.Add("@UnitAddress@", "Tòa nhà HH1 Yên Hoà, Phường Yên Hoà, quận Cầu Giấy, Hà Nội");
            //t1.KeyValues.Add("@IsND51@", "False");
            //t1.KeyValues.Add("@AccountMode@", "0");
            //t1.KeyValues.Add("@InvoiceStatusID@", "1");
            //t1.KeyValues.Add("@OganizationName@", "");
            //t1.KeyValues.Add("@OriginalInvoiceGUID@", "00000000-0000-0000-0000-000000000000");
            //t1.KeyValues.Add("@IsBExistsAccount@", "True");
            //t1.KeyValues.Add("@BuyerCode@", "");
            //t1.KeyValues.Add("@FormattedTaxCode@", "0 1 0 1 3 6 0 6 9 7 - 9 9 9");
            //t1.KeyValues.Add("@ReceiverMobile@", "");
            //t1.KeyValues.Add("@ProvinceName@", "");
            //t1.KeyValues.Add("@SendMTC@", "");
            //t1.KeyValues.Add("@TaxDepartmentCode@", "0");
            //t1.KeyValues.Add("@InvTempMode@", "3");
            //t1.KeyValues.Add("@IsND123@", "True");
            //t1.KeyValues.Add("@IsBSigned@", "False");
            //t1.KeyValues.Add("@IsCancel@", "False");
            //t1.KeyValues.Add("@DomainCheckInvoice@", "tracuu.ehoadon.vn");
            //t1.KeyValues.Add("@ContactFullName@", "");
            //t1.KeyValues.Add("@SumItemAmount@", "4600000");
            //t1.KeyValues.Add("@TaxRateHeaderID@", "4");
            //t1.KeyValues.Add("@InvoiceSerial@", "C24TCH");
            //t1.KeyValues.Add("@ListBESFile@", "");
            //t1.KeyValues.Add("@InvoiceForm@", "1");
            //t1.KeyValues.Add("@CurrencyID@", "VND");
            //t1.KeyValues.Add("@CheckNumber@", "32603");
            //t1.KeyValues.Add("@OrganizationPhone@", "");
            //t1.KeyValues.Add("@UserNamePrint@", "");
            //t1.KeyValues.Add("@InvoiceStatusName@", "Mới tạo");
            //t1.KeyValues.Add("@InvoiceTypeID@", "1");
            //t1.KeyValues.Add("@IsNotAllowBuyerPrintConvertion@", "False");
            //t1.KeyValues.Add("@ReceiveTypeID@", "1");
            //t1.KeyValues.Add("@CacheNote@", "AP125");
            //t1.KeyValues.Add("@UnitEmail@", "dongvtd@bkav.com");
            //t1.KeyValues.Add("@IsEmpty@", "False");
            //t1.KeyValues.Add("@InvoiceStatusShow@", "MT");
            //t1.KeyValues.Add("@UIDefine@", "");
            //t1.KeyValues.Add("@UnitPhone@", "0979226960");
            //t1.KeyValues.Add("@UnitPersonRepresent@", "PHẠM VĂN QUANG");
            //t1.KeyValues.Add("@BankName@", "Ngân hàng A;Ngân hàng B");
            //t1.KeyValues.Add("@BuyerAddress@", "64 Lê Trực, Hoàn Kiếm, Hà Nội");
            //t1.KeyValues.Add("@IsBTH@", "False");
            //t1.KeyValues.Add("@Website@", "");
            //t1.KeyValues.Add("@BuyerUnitName@", "Công ty TM Hoàng Q");
            //t1.KeyValues.Add("@TaxCode@", "0101360697-999");
            //t1.KeyValues.Add("@UserID@", "340217");
            //t1.KeyValues.Add("@Showroom@", "");
            //t1.KeyValues.Add("@InvoiceID@", "368172883");
            //t1.KeyValues.Add("@IsReplace@", "False");
            //t1.KeyValues.Add("@Note@", "");
            //t1.KeyValues.Add("@UnitName@", "Cty Bkav");
            //t1.KeyValues.Add("@BuyerBankAccount@", "");
            //t1.KeyValues.Add("@IsNotUpdateSumItemsFromWS@", "False");
            //t1.KeyValues.Add("@SynchStatusID@", "0");
            //t1.KeyValues.Add("@IsDieuChinhGiam@", "False");
            //t1.KeyValues.Add("@BuyerName@", "Bùi Bình D");
            //t1.KeyValues.Add("@SumTaxAmount@", "0");
            //t1.KeyValues.Add("@AccountID@", "339713");
            //t1.KeyValues.Add("@ReceiverEmail@", "");
            //t1.KeyValues.Add("@Fax@", "");
            //t1.KeyValues.Add("@MaCuaCQT@", "CCCCCCCCCCCCCCCCCCCCCCCCCCCCC");
            //t1.KeyValues.Add("@IsSend04SS@", "False");
            //t1.KeyValues.Add("@UserNameCreate@", "Cty Bkav");
            //t1.KeyValues.Add("@DocConfigs@", "");
            //t1.KeyValues.Add("@SignedDate@", "1/1/1900 12:00:00 AM");
            //t1.KeyValues.Add("@SumPaymentAmount@", "4100000");
            //t1.KeyValues.Add("@SumDiscountAmount@", "500000");
            //t1.KeyValues.Add("@ExchangeRate@", "1");
            //t1.KeyValues.Add("@TaxRateHeader@", "-1");
            //t1.KeyValues.Add("@OriginalInvoiceID@", "0");
            //t1.KeyValues.Add("@IsCheckMST@", "False");
            //t1.KeyValues.Add("@IsSigned@", "False");
            //t1.KeyValues.Add("@IsCreateXMLTBao@", "False");
            //t1.KeyValues.Add("@TaxDepartmentName@", "");
            //t1.KeyValues.Add("@BankAccount@", "123;456");
            //t1.KeyValues.Add("@Reason@", "");
            //t1.KeyValues.Add("@OganizationAddress@", "");
            //t1.KeyValues.Add("@InvoiceAction@", "0");
            //t1.KeyValues.Add("@IsCoMa@", "True");
            //t1.KeyValues.Add("@PayMethodName@", "Tiền mặt/Chuyển khoản");
            //t1.KeyValues.Add("@AccountGUID@", "920e6dad-705b-49a1-a0ae-43f64c6393e0");
            //t1.KeyValues.Add("@OriginalInvoiceIdentify@", "");
            //t1.KeyValues.Add("@BillCode@", "AP125");
            //t1.KeyValues.Add("@InvoiceTemplateID@", "1170808");
            //t1.KeyValues.Add("@CCCD@", "");
            //t1.KeyValues.Add("@ContactAddress@", "");
            //t1.KeyValues.Add("@ShowDateModeByTax@", "2");
            //t1.KeyValues.Add("@IsKhongMa@", "False");
            //t1.KeyValues.Add("@SellerTaxCode@", "0101360697-999");
            //t1.KeyValues.Add("@FormattedBuyerTaxCode@", "");
            //t1.KeyValues.Add("@CreateDate@", "10/25/2024 1:19:52 PM");
            //t1.KeyValues.Add("@OriginalInvoiceNo@", "0");
            //t1.KeyValues.Add("@BAccountGUID@", "962c4a76-18d3-40cd-b2db-21dfc4d8afeb");
            //t1.KeyValues.Add("@SumItemDetails@", "0");
            //t1.KeyValues.Add("@InvoiceDate@", "10/25/2024 12:00:00 AM");
            //t1.KeyValues.Add("@CertificateSerial@", "5403026a9b63a7cd351b60f29df7cc8d");
            //t1.KeyValues.Add("@IsMTT@", "False");
            //t1.KeyValues.Add("@SourceID@", "2");
            //t1.KeyValues.Add("@PayMethodID@", "3");
            //t1.KeyValues.Add("@UnitPersonRepresentPosition@", "GIÁM ĐỐC");
            //t1.KeyValues.Add("@SourceName@", "BTF - Bkav Template File (Hóa đơn được tạo bằng cách import dữ liệu từ file Excel có định dạng BTF)");
            //t1.KeyValues.Add("@ContactPhone@", "");
            //t1.KeyValues.Add("@ReceiverName@", "");
            //t1.KeyValues.Add("@InvoiceNo@", "0");
            //t1.KeyValues.Add("@InvoiceGUID@", "feac315f-75f1-41ad-bbd3-e8f47f90839e");
            //t1.KeyValues.Add("@BuyerTaxCode@", "");
            //t1.KeyValues.Add("@ProvinceCode@", "");


            //t1.KeyValueItems.Add(dict);
            //t1.KeyValueItems.Add(dict);
            //t1.htItems.Add("","");



            //t1.BackgroundImage = 












            Random random = new Random();
            int index = random.Next(0, 1000000000); // Tạo số ngẫu nhiên từ 0 đến 100
            // Đường dẫn thư mục và file PDF cần tạo
            string PDFFullPath = @"..\..\PDFFile\Test"
                + (new Random().Next(1000))
                + ".pdf";



            msg = t1.CreateHMLT(out string html);
            addhtml(html);

            msg = t1.HTMLConveroPDF(html, out byte[] data);



            // Ghi mảng byte vào file PDF
            File.WriteAllBytes(PDFFullPath, data);
            Process.Start(PDFFullPath);

            return msg;
        }

        public void addhtml(string html)
        {
            string filePath = @"..\..\..\your_file_name.html";
            try
            {
                // Ghi nội dung vào file HTML
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(html);
                }

                Console.WriteLine("Nội dung đã được ghi vào file HTML thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
            }

        }
    }
}
