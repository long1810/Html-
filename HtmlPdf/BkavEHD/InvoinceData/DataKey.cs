using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace BES
{
    public class DataKey
    {
        public Hashtable KeyValues { private set; get; }

        public List<Dictionary<string, object>> KeyValueItems { private set; get; }

        public DataKey()
        {
            KeyValues = new Hashtable();
            KeyValueItems = new List<Dictionary<string, object>>();
        }

        public string AddKeyValues(object obj)
        {
            if (obj == null) return string.Empty;

            string msg = string.Empty;
            Type type = obj.GetType();
            PropertyInfo[] pis = type.GetProperties();
            object valObj;
            int length = pis.Length;
            for (int i = 0; i < length; i++)
            {
                PropertyInfo pi = pis[i];
                string key = "@" + pi.Name + "@";
                valObj = pi.GetValue(obj);

                if (KeyValues.ContainsKey(key)) continue;

                if (valObj is IEnumerable enumerable && !(valObj is string))
                {
                    foreach (var item in enumerable)
                    {
                        Dictionary<string, object> itemDict = new Dictionary<string, object>();

                        // Nếu item là một đối tượng, lấy các thuộc tính của nó và thêm vào itemDict
                        foreach (var prop in item.GetType().GetProperties())
                        {
                            itemDict.Add(prop.Name, prop.GetValue(item) ?? string.Empty);
                        }
                        KeyValueItems.Add(itemDict);
                    }
                }
                // Xử lý khi valObj là một object phức tạp
                else if (valObj != null && valObj.GetType().IsClass && valObj.GetType() != typeof(string))
                {
                    // Đệ quy để thêm các thuộc tính của object con
                    AddKeyValues(valObj);
                }
                // Xử lý khi valObj là kiểu đơn giản
                else
                {
                    KeyValues.Add(key, valObj ?? string.Empty);
                }
            }
            return msg;

        }
        public class ItemFooter
        {
            public const string SumItemAmount = "@SumItemAmount@";
            public const string TaxRateHeaderID = "@TaxRateHeaderID@";
            public const string SumItemDetails = "@SumItemDetails@";
            public const string SumPaymentAmount = "@SumPaymentAmount@";
            public const string SumDiscountAmount = "@SumDiscountAmount@";
            public const string SumTaxAmount = "@SumTaxAmount@";
            public const string InWord = "@InWord@";

        }
        // Key Item
        public class Item
        {

            public const string InvoiceGUID = "InvoiceGUID";
            public const string InvoiceDetailID = "InvoiceDetailID";
            public const string InvoiceDetailGUID = "InvoiceDetailGUID";
            public const string CreateDate = "CreateDate";
            public const string LastUpdate = "LastUpdate";
            public const string InvoiceID = "InvoiceID";
            public const string IsDiscount = "IsDiscount";
            public const string ItemName = "ItemName";
            public const string UnitName = "UnitName";
            public const string Qty = "Qty";
            public const string Price = "Price";
            public const string Amount = "Amount";
            public const string TaxRateID = "TaxRateID";
            public const string TaxRate = "TaxRate";
            public const string TaxName = "TaxName";
            public const string TaxAmount = "TaxAmount";
            public const string DiscountRate = "DiscountRate";
            public const string DiscountAmount = "DiscountAmount";
            public const string SVChargeAmount = "SVChargeAmount";
            public const string OtherAmount = "OtherAmount";
            public const string ItemCode = "ItemCode";
            public const string ItemTypeID = "ItemTypeID";
            public const string UserDefineDetails = "UserDefineDetails";
            public const string IsIncrease = "IsIncrease";
            public const string ItemNote = "ItemNote";
            public const string InvoiceDetailRelationID = "InvoiceDetailRelationID";
        }
        //Phần key 
        public const string IsTaxDetails = "@IsTaxDetails@";
        public const string UserDefine = "@UserDefine@";
        public const string CacheNoteRemoveHTML = "@CacheNoteRemoveHTML@";
        public const string ReceiverAddress = "@ReceiverAddress@";
        public const string IsDiscountDetails = "@IsDiscountDetails@";
        public const string LastUpdate = "@LastUpdate@";
        public const string UnitAddress = "@UnitAddress@";
        public const string IsND51 = "@IsND51@";
        public const string AccountMode = "@AccountMode@";
        public const string InvoiceStatusID = "@InvoiceStatusID@";
        public const string OganizationName = "@OganizationName@";
        public const string OriginalInvoiceGUID = "@OriginalInvoiceGUID@";
        public const string IsBExistsAccount = "@IsBExistsAccount@";
        public const string BuyerCode = "@BuyerCode@";
        public const string FormattedTaxCode = "@FormattedTaxCode@";
        public const string ReceiverMobile = "@ReceiverMobile@";
        public const string ProvinceName = "@ProvinceName@";
        public const string SendMTC = "@SendMTC@";
        public const string TaxDepartmentCode = "@TaxDepartmentCode@";
        public const string InvTempMode = "@InvTempMode@";
        public const string IsND123 = "@IsND123@";
        public const string IsBSigned = "@IsBSigned@";
        public const string IsCancel = "@IsCancel@";
        public const string DomainCheckInvoice = "@DomainCheckInvoice@";
        public const string ContactFullName = "@ContactFullName@";

        public const string InvoiceSerial = "@InvoiceSerial@";
        public const string ListBESFile = "@ListBESFile@";
        public const string InvoiceForm = "@InvoiceForm@";
        public const string CurrencyID = "@CurrencyID@";
        public const string CheckNumber = "@CheckNumber@";
        public const string OrganizationPhone = "@OrganizationPhone@";
        public const string UserNamePrint = "@UserNamePrint@";
        public const string InvoiceStatusName = "@InvoiceStatusName@";
        public const string InvoiceTypeID = "@InvoiceTypeID@";
        public const string IsNotAllowBuyerPrintConvertion = "@IsNotAllowBuyerPrintConvertion@";
        public const string ReceiveTypeID = "@ReceiveTypeID@";
        public const string CacheNote = "@CacheNote@";
        public const string UnitEmail = "@UnitEmail@";
        public const string IsEmpty = "@IsEmpty@";
        public const string InvoiceStatusShow = "@InvoiceStatusShow@";
        public const string UIDefine = "@UIDefine@";
        public const string UnitPhone = "@UnitPhone@";
        public const string UnitPersonRepresent = "@UnitPersonRepresent@";
        public const string BankName = "@BankName@";
        public const string BuyerAddress = "@BuyerAddress@";
        public const string IsBTH = "@IsBTH@";
        public const string Website = "@Website@";
        public const string BuyerUnitName = "@BuyerUnitName@";
        public const string TaxCode = "@TaxCode@";
        public const string UserID = "@UserID@";
        public const string Showroom = "@Showroom@";
        public const string InvoiceID = "@InvoiceID@";
        public const string IsReplace = "@IsReplace@";
        public const string Note = "@Note@";
        public const string UnitName = "@UnitName@";
        public const string BuyerBankAccount = "@BuyerBankAccount@";
        public const string IsNotUpdateSumItemsFromWS = "@IsNotUpdateSumItemsFromWS@";
        public const string SynchStatusID = "@SynchStatusID@";
        public const string IsDieuChinhGiam = "@IsDieuChinhGiam@";
        public const string BuyerName = "@BuyerName@";
        public const string AccountID = "@AccountID@";
        public const string ReceiverEmail = "@ReceiverEmail@";
        public const string Fax = "@Fax@";
        public const string MaCuaCQT = "@MaCuaCQT@";
        public const string IsSend04SS = "@IsSend04SS@";
        public const string UserNameCreate = "@UserNameCreate@";
        public const string DocConfigs = "@DocConfigs@";
        public const string SignedDate = "@SignedDate@";

        public const string ExchangeRate = "@ExchangeRate@";
        public const string TaxRateHeader = "@TaxRateHeader@";
        public const string OriginalInvoiceID = "@OriginalInvoiceID@";
        public const string IsCheckMST = "@IsCheckMST@";
        public const string IsSigned = "@IsSigned@";
        public const string IsCreateXMLTBao = "@IsCreateXMLTBao@";
        public const string TaxDepartmentName = "@TaxDepartmentName@";
        public const string BankAccount = "@BankAccount@";
        public const string Reason = "@Reason@";
        public const string OganizationAddress = "@OganizationAddress@";
        public const string InvoiceAction = "@InvoiceAction@";
        public const string IsCoMa = "@IsCoMa@";
        public const string PayMethodName = "@PayMethodName@";
        public const string AccountGUID = "@AccountGUID@";
        public const string OriginalInvoiceIdentify = "@OriginalInvoiceIdentify@";
        public const string BillCode = "@BillCode@";
        public const string InvoiceTemplateID = "@InvoiceTemplateID@";
        public const string CCCD = "@CCCD@";
        public const string ContactAddress = "@ContactAddress@";
        public const string ShowDateModeByTax = "@ShowDateModeByTax@";
        public const string IsKhongMa = "@IsKhongMa@";
        public const string SellerTaxCode = "@SellerTaxCode@";
        public const string FormattedBuyerTaxCode = "@FormattedBuyerTaxCode@";
        public const string CreateDate = "@CreateDate@";
        public const string OriginalInvoiceNo = "@OriginalInvoiceNo@";
        public const string BAccountGUID = "@BAccountGUID@";
        public const string InvoiceDate = "@InvoiceDate@";
        public const string CertificateSerial = "@CertificateSerial@";
        public const string IsMTT = "@IsMTT@";
        public const string SourceID = "@SourceID@";
        public const string PayMethodID = "@PayMethodID@";
        public const string UnitPersonRepresentPosition = "@UnitPersonRepresentPosition@";
        public const string SourceName = "@SourceName@";
        public const string ContactPhone = "@ContactPhone@";
        public const string ReceiverName = "@ReceiverName@";
        public const string InvoiceNo = "@InvoiceNo@";
        public const string InvoiceGUID = "@InvoiceGUID@";
        public const string BuyerTaxCode = "@BuyerTaxCode@";
        public const string ProvinceCode = "@ProvinceCode@";
        
    }
    
}
