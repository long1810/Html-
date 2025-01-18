using BES;
using BSS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BES
{
    public class HtmlElement
    {
        protected DataKey DataKey;
        protected HtmlCSS CSS;

        public Hashtable KeyValues { protected set; get; }
        public List<Dictionary<string, object>> KeyValueItems { protected set; get; }

        public string BackgroundColor = "rgb(158,200,251)";
        public string BorderColor = "rgb(128,125,239)";
        public string ColorText = "rgb(0,0,0)";

        public string BackgroundImage = "";
        public string BackgroundImageLogo = "";
        public string LogoImage = "";
        public string SignerImage = "";
        public string StampImage = "";

        public int widthDef = 718;
        public string widthDefCSS = "width:718px;";

        public float FontSize = 12;
        public string FontFamily = "arial";

        public string Title = "<title>Document</title>";
        public string StartHTML = "<!DOCTYPE html> <html lang='en'> <head> <meta charset='UTF-8'> <meta name='viewport' content='width=device-width, initial-scale=1.0'>";
        public string EndHTML = "</html>";
        public CultureInfo Cul = CultureInfo.GetCultureInfo("vi-VN");
        public int TypeDelimiter = 1;
        public bool IsVND = true;
        public string CurrencyID = VND;
        public HtmlElement()
        {
            DataKey = new DataKey();
            //CSS = new HtmlCSS();
        }

        public string AddKeyValues(object objData)
        {
            string msg = DataKey.AddKeyValues(objData);
            if (msg.Length > 0) return msg;

            KeyValues = DataKey.KeyValues;
            KeyValueItems = DataKey.KeyValueItems;
            UpdateInvoiceData();
            return msg;
        }
        public virtual void UpdateInvoiceData()
        {
            //Chuyển đổi invoiceNo từ 1 thành 00000001
            if (KeyValues.ContainsKey("@InvoiceNo@"))
            {
                int invoiceNo = (int)KeyValues[DataKey.InvoiceNo];
                KeyValues["@InvoiceNo@"] = invoiceNo.ToString("D8");
            }
            //kiểm tra VND không
            if (KeyValues.ContainsKey(DataKey.CurrencyID))
            {
                CurrencyID = (string)KeyValues[DataKey.CurrencyID];
                if (CurrencyID.CompareTo("VND") != 0)
                {
                    TypeDelimiter = 2;
                    IsVND = false;
                    Cul = CultureInfo.GetCultureInfo("en-US");
                }
            }

            //thêm key InWords
            //if (KeyValues.ContainsKey(DataKey. DataKey.ItemFooter.SumPaymentAmount))
            //{
            //    double totalPaymentAmount = (double)KeyValues[DataKey. DataKey.ItemFooter.SumPaymentAmount];
            //    bool isDieuChinhGiam = (bool)KeyValues[DataKey.IsDieuChinhGiam];
            //    string inWord = GetAmountInwords(totalPaymentAmount, isDieuChinhGiam,"Điều chỉnh giảm");
            //    KeyValues.Add(DataKey. DataKey.ItemFooter.InWord, inWord);
            //}
        }
        public virtual void UpdateInWord(string inWord)
        {
            KeyValues.Add( DataKey.ItemFooter.InWord,inWord);
        }
        //public string GetAmountInwords(double amount, bool isDieuChinhGiam, string preTxtDC = "")
        //{
        //    string defaultPreTxtDC = string.Empty;
        //    if (isDieuChinhGiam || amount < 0)
        //        defaultPreTxtDC = preTxtDC;
        //    if (amount < 0) amount = 0 - amount;
        //    if (amount == 0)
        //        defaultPreTxtDC = String.Empty;
        //    return defaultPreTxtDC + MoneyRead.NumberToTextVND(amount, CurrencyText(CurrencyID), CurrencyID) + "./.";
        //}
        static public string CurrencyText(string id)
        {
            switch (id?.ToUpper())
            {
                case VND: return "đồng chẵn";
                case USD: return "đô la Mỹ";
                case CNY: return "nhân dân tệ";
                case EUR: return "euro";
                case JPY: return "Yên Nhật";
                case SGD: return "đô la Singapore";
                case AUD: return "đô la Úc";
                case GBP: return "bảng Anh";
            }

            return id;
        }
        #region Hàm gọi khải tạo sẵn
        public static void AddDivGird(StringBuilder html, float[] table, int gap = 0, int paddingTop = 0, string height = "auto")
        {
            string tableWidth = "";
            foreach (var row in table)
            {
                tableWidth += row.ToString() + "fr ";
            }
            html.Append($"<div style='display: grid; grid-template-columns: {tableWidth};height: {height}; gap: {gap}pt;padding-top:{paddingTop}px'>");
        }
        public static void Add_td(StringBuilder html, string val)
        {
            html.Append("<td>");
            html.Append(val);
            html.Append("</td>");

        }
        #endregion  
        #region Cấu hình theo cú pháp
        //0_0|1_1|2_HÓA ĐƠN GIÁ TRỊ GIA|3_EN|4_VAL|5_C1234|6_0|7_T|8_false|9_12.b*9.i*10.b|10_13|11_1|12_0100|13_255,0,0|14_255,0,0|13_0;
        //0_ColSpan|1_RowSpan|2_TextVN|3_TextEN|4_TextValue|5_sHAlign_Boder|6_MiniumHeight|7_ParseToVerticalAlignment|8_IsDottedLine|9_FontStyle|10_Leading|11_SpacingAfter|12_Padding|13_BgColor|14_BgColor|15_BorderColor|16_DottedCell,offset;
        public virtual string TextToHtml_Tr(StringBuilder html, string txt)
        {
            string msg = string.Empty;
            string[] trAray = txt.Split('^');

            //html.Append("<table style='border-collapse: collapse; width: 100%;'>");
            foreach (string tr in trAray)
            {
                html.Append("<tr>");

                string[] tdAray = tr.Split('&');
                foreach (string td in tdAray)
                {
                    if (td.Length == 0) continue;

                    msg = TextToCustomCellSettings_Tr(td, out CustomCellSettings settings);
                    if (msg.Length > 0) return msg;

                    StringBuilder styleTd = new StringBuilder();
                    //styleTd.Append(settings.ColSpan);
                    //styleTd.Append(settings.RowSpan);
                    styleTd.Append(settings.BorderWidthLeft);
                    styleTd.Append(settings.BorderWidthTop);
                    styleTd.Append(settings.BorderWidthRight);
                    styleTd.Append(settings.BorderWidthBottom);
                    styleTd.Append(settings.HorizontalAlignment);
                    styleTd.Append(settings.MiniumHeight);
                    styleTd.Append(settings.VerticalAlignment);
                    styleTd.Append(settings.PaddingLeft);
                    styleTd.Append(settings.PaddingTop);
                    styleTd.Append(settings.PaddingRight);
                    styleTd.Append(settings.PaddingBottom);
                    styleTd.Append(settings.BgColor);

                    html.Append(
                        $"<td {settings.ColSpan} {settings.RowSpan} style='{styleTd}'>" +
                            $"<span style='{settings.FontStyle[0]}'>{settings.TextVN} </span>" +
                            $"<span style='{settings.FontStyle[1]}'>{settings.TextEN} </span>" +
                            $"<span style='{settings.FontStyle[2]}'>{settings.TextValue} </span>" +
                        "</td>"
                    );
                }
                html.Append("</tr>");
            }


            return msg;
        }

        public virtual string TextToCustomCellSettings_Tr(string txt, out CustomCellSettings settings)
        {
            string[] values = txt.Split('|');
            settings = new CustomCellSettings();
            int length = values.Length;

            // Cấu hình colspan và rowspan
            settings.ColSpan = values[0] != "1" ? $" colspan='{values[0]}' " : "";
            settings.RowSpan = values[1] != "1" ? $" rowspan='{values[1]}' " : "";

            // Thiết lập văn bản hiển thị
            Tuple<string, string, string> texts = FormatCustomItemFooterTexts(values[2], values[3], values[4]);
            settings.TextVN = texts.Item1;
            settings.TextEN = texts.Item2;
            settings.TextValue = texts.Item3;

            // Cấu hình căn chỉnh ngang và viền
            string hAlignBorderMode = values[5];
            if (hAlignBorderMode.Length == 0) return "";

            string sHAlign = hAlignBorderMode.Substring(0, 1).ToLower();
            settings.HorizontalAlignment = CustomCellSettings.ParseToHorizontalAlignment(sHAlign);
            Tuple<float, float, float, float> borderWidth = CustomCellSettings.GetBorderWidths(hAlignBorderMode.Substring(1));
            settings.BorderWidthLeft = borderWidth.Item1 == 0 ? "" : $"border-left: {borderWidth.Item1}px solid;";
            settings.BorderWidthTop = borderWidth.Item2 == 0 ? "" : $"border-top: {borderWidth.Item2}px solid;";
            settings.BorderWidthRight = borderWidth.Item3 == 0 ? "" : $"border-right: {borderWidth.Item3}px solid;";
            settings.BorderWidthBottom = borderWidth.Item4 == 0 ? "" : $"border-bottom: {borderWidth.Item4}px solid;";

            // Thiết lập các thuộc tính bổ sung
            if (length > 6) settings.MiniumHeight = $"min-height: {values[6]}px;";
            if (length > 7) settings.VerticalAlignment = CustomCellSettings.ParseToVerticalAlignment(values[7]);
            if (length > 8) settings.IsDottedLine = values[8].ToBoolean(false);
            if (length > 9)
            {
                CustomCellSettings.getFont(values[9], settings);
            }

            if (length > 10) settings.Leading = $"line-height: {values[10]}px;";
            if (length > 11) settings.SpacingAfter = values[11].ToNumber(6);

            // Cấu hình padding
            if (length > 12)
            {
                Tuple<char, char, char, char> padding = CustomCellSettings.GetPadding(values[12]);
                settings.PaddingLeft = $"padding-left: {padding.Item1}px;";
                settings.PaddingTop = $"padding-top: {padding.Item2}px;";
                settings.PaddingRight = $"padding-right: {padding.Item3}px;";
                settings.PaddingBottom = $"padding-bottom: {padding.Item4}px;";
            }

            // Cấu hình màu nền và màu viền
            if (length > 13 && !string.IsNullOrEmpty(values[13])) settings.BgColor = $"background-color: {values[13]};";

            if (length > 14)
            {
                settings.BorderColor = values[14];
                if (!string.IsNullOrEmpty(settings.BorderColor))
                {
                    settings.BorderWidthLeft = $"border-left: {borderWidth.Item1}px solid {settings.BorderColor};";
                    settings.BorderWidthTop = $"border-top: {borderWidth.Item2}px solid {settings.BorderColor};";
                    settings.BorderWidthRight = $"border-right: {borderWidth.Item3}px solid {settings.BorderColor};";
                    settings.BorderWidthBottom = $"border-bottom: {borderWidth.Item4}px solid {settings.BorderColor};";
                }
            }

            // Cấu hình dòng kẻ chấm (dotted line)
            if (length > 15)
            {
                CustomCellSettings.getDotedCell(values[15], settings);
                settings.Dotted = $"<svg style='position: relative; bottom: 2px; left: 0; width: 100%; height: 2px;'><line x1='0' y1='0' x2='100%' y2='0' stroke='red' stroke-width='1' stroke-dasharray='{settings.DottedWidth},{settings.Offset}' /></svg>";
            }

            return string.Empty;
        }

        public virtual string TextToHtml_Gird(StringBuilder html, string txt)
        {
            string msg = string.Empty;
            string[] txtAray = txt.Split('&');
            foreach (string s in txtAray)
            {
                if (s.Length == 0) continue;
                msg = TextToCustomCellSettings_Gird(s, out CustomCellSettings settings);
                if (msg.Length > 0) return msg;

                StringBuilder styleDiv = new StringBuilder();
                styleDiv.Append(settings.ColSpan);
                styleDiv.Append(settings.RowSpan);
                styleDiv.Append(settings.BorderWidthLeft);
                styleDiv.Append(settings.BorderWidthTop);
                styleDiv.Append(settings.BorderWidthRight);
                styleDiv.Append(settings.BorderWidthBottom);
                styleDiv.Append(settings.HorizontalAlignment);
                styleDiv.Append(settings.MiniumHeight);
                styleDiv.Append(settings.VerticalAlignment);
                //styleDiv.Append(settings.Padding);
                styleDiv.Append(settings.PaddingLeft);
                styleDiv.Append(settings.PaddingTop);
                styleDiv.Append(settings.PaddingLeft);
                styleDiv.Append(settings.PaddingBottom);
                styleDiv.Append(settings.Leading);

                StringBuilder styleTextVN = new StringBuilder();
                styleTextVN.Append(settings.FontStyle[0]);

                html.Append(
                    $"<div style='{styleDiv}'>" +
                        $"<span style='{settings.FontStyle[0]}'>{settings.TextVN} </span>" +
                        $"<span style='{settings.FontStyle[1]}'>{settings.TextEN} </span>" +
                        $"<span style='{settings.FontStyle[2]}' >{settings.TextValue} </span>" +
                    $"</div>");
            }

            return msg;
        }

        public virtual string TextToCustomCellSettings_Gird(string txt, out CustomCellSettings settings)
        {
            string[] values = txt.Split('|');
            settings = new CustomCellSettings();
            int length = values.Length;
            settings.ColSpan = values[0] != "1" ? $"grid-column: span {values[0]};" : "";
            settings.RowSpan = values[1] != "1" ? $"grid-row: span {values[1]};" : "";
            Tuple<string, string, string> texts = FormatCustomItemFooterTexts(values[2], values[3], values[4]);
            settings.TextVN = texts.Item1;
            settings.TextEN = texts.Item2;
            settings.TextValue = texts.Item3;

            string hAlignBorderMode = values[5];
            if (hAlignBorderMode.Length == 0) return "";
            string sHAlign = hAlignBorderMode.Substring(0, 1).ToLower();
            settings.HorizontalAlignment = CustomCellSettings.ParseToHorizontalAlignment(sHAlign);
            Tuple<float, float, float, float> borderWidth = CustomCellSettings.GetBorderWidths(hAlignBorderMode.Substring(1));
            settings.BorderWidthLeft = borderWidth.Item1 == 0 ? "" : $"border-left: {borderWidth.Item1}pt solid;" + BorderColor;
            settings.BorderWidthTop = borderWidth.Item2 == 0 ? "" : $"border-top: {borderWidth.Item2}pt solid;" + BorderColor;
            settings.BorderWidthRight = borderWidth.Item3 == 0 ? "" : $"border-right: {borderWidth.Item3}pt solid;" + BorderColor;
            settings.BorderWidthBottom = borderWidth.Item4 == 0 ? "" : $"border-bottom: {borderWidth.Item4}pt solid;" + BorderColor;

            if (length > 6) settings.MiniumHeight = $"min-height: {values[6]}pt;";
            if (length > 7) settings.VerticalAlignment = CustomCellSettings.ParseToVerticalAlignment(values[7]);
            if (length > 8) settings.IsDottedLine = values[8].ToBoolean(false);
            if (length > 9)
            {
                CustomCellSettings.getFont(values[9], settings);
            }

            if (length > 10) settings.Leading = $"line-height: {values[10]}pt";
            if (length > 11) settings.SpacingAfter = values[11].ToNumber(6);
            if (length > 12)
            {
                settings.Padding = $"padding:{values[12]};";
                if (settings.Padding.Length > 1)
                {
                    Tuple<char, char, char, char> padding = CustomCellSettings.GetPadding(values[12]);
                    settings.PaddingLeft = $"padding-left: {padding.Item1}pt;";
                    settings.PaddingTop = $"padding-top: {padding.Item2}pt;";
                    settings.PaddingRight = $"padding-right: {padding.Item3}pt;";
                    settings.PaddingBottom = $"padding-bottom: {padding.Item4}pt;";
                }
            }
            if (length > 13) settings.BgColor = values[13];
            if (length > 14)
            {
                settings.BorderColor = values[14];
                settings.BorderWidthLeft = $"border-left: {borderWidth.Item1}pt solid;" + settings.BorderColor;
                settings.BorderWidthTop = $"border-top: {borderWidth.Item2}pt solid;" + settings.BorderColor;
                settings.BorderWidthRight = $"border-right: {borderWidth.Item3}pt solid;" + settings.BorderColor;
                settings.BorderWidthBottom = $"border-bottom: {borderWidth.Item4}pt solid;" + settings.BorderColor;
            }
            if (length > 15)
            {
                CustomCellSettings.getDotedCell(values[15], settings);

                settings.Dotted = $"<svg style='position: relative; bottom: 2pt; left: 0; width: 100%; height: 2pt;'><line x1='0' y1='0' x2='100%' y2='0' stroke='red' stroke-width='1' stroke-dasharray='{settings.DottedWidth},{settings.Offset}' /></svg>";
            }

            return string.Empty;
        }

        private Tuple<string, string, string> FormatCustomItemFooterTexts(string textVN, string textEN, string value)
        {
            ReplaceKeys(textVN, out textVN);
            ReplaceKeys(textEN, out textEN);
            ReplaceKeys(value, out value);

            if (textVN == "" || textVN == "VN") textVN = null;
            if (textEN == "" || textEN == "EN") textEN = null;
            if (value == "" || value == "VALUE") value = null;

            return new Tuple<string, string, string>(textVN, textEN, value);
        }

        protected string ReplaceKeys(string sampleInvoice, out string content, bool IsChangeValue = true)
        {
            content = sampleInvoice;
            if (string.IsNullOrEmpty(sampleInvoice)) return string.Empty;

            var matches = Regex.Matches(sampleInvoice, @"@[\w\d,\(\)]+@");
            foreach (Match match in matches)
            {
                string key = match.ToString();

                // Nếu key không tồn tại trong KeyValues hoặc không có trong danh sách trắng
                if (!KeyValues.Contains(key) && !IsMacroInWhiteList(key))
                {
                    // Thay thế key không tồn tại bằng chuỗi trống
                    content = content.Replace(key, string.Empty);
                    continue;
                }

                // Lấy giá trị từ key
                string value = GetValueFromKey(key, IsChangeValue);
                content = content.Replace(key, value);
            }

            return content;
        }
        protected string GetValueFromKey(string key, bool IsChangeValue)
        {

            string value = string.Empty;
            if (!KeyValues.Contains(key))
            {
                if (HasPrefixUD(key)) value = string.Empty;
            }
            else
            {
                var objValue = KeyValues[key];
                switch (objValue)
                {
                    case string s:
                        return s;
                    case double d:
                        return d.ToString("#,###", Cul.NumberFormat);
                    default:
                        return objValue.ToString();
                }

            }

            if (value == null || value == key) return "";

            //string delimited = "\\\\(?![bfnrt\"\'%])";
            //if (!value.Equals("\\") && !value.Equals("\\ %") && Regex.IsMatch(value, delimited)) value = Regex.Replace(value, delimited, @"\\", RegexOptions.None);
            //if (value.Contains("|") && !IsChangeValue) value = key;

            //if (KeyValues.Contains(value)) value = GetValueFromKey(value, IsChangeValue);

            //value = Regex.Replace(value, @"\\n", "\n");
            return value;
        }

        public static bool IsMacroInWhiteList(string key)
        {
            return HasPrefixUD(key);
            //return HasPrefixUD(key) || MissingItemFooterKey(key);
        }
        public static bool HasPrefixUD(string key)
        {
            return key.StartsWith($"@{PrefixUserDefine}") || key.StartsWith($"@{PrefixUserDefineDetails}") || key.StartsWith($"@{PrefixUserDefineSum}")
                || key.StartsWith($"@{PrefixUserDefineNumber}") || key.StartsWith($"@{PrefixUserDefineDetailsNumber}")
                || key.StartsWith($"@{PrefixUserInfo}");
        }
        public const string PrefixUserDefine = "UD_";
        public const string PrefixUserDefineNumber = "UDN_";
        public const string PrefixUserDefineDetails = "UDD_";
        public const string PrefixUserDefineDetailsNumber = "UDDN_";
        public const string PrefixUserDefineSum = "SUM_";
        public const string PrefixUserInfo = "UI_";
        public const string PrefixUserInfoNumber = "UIN_";

        public class CustomCellSettings
        {
            public string ColSpan { get; set; } = "1";
            public string RowSpan { get; set; } = "1";
            public string TextVN { get; set; }
            public string TextEN { get; set; }
            public string TextValue { get; set; }
            public string[] FontStyle { get; set; } = new string[3];
            public string BorderWidthLeft { get; set; } = "";
            public string BorderWidthTop { get; set; } = "";
            public string BorderWidthRight { get; set; } = "";
            public string BorderWidthBottom { get; set; } = "";
            public string HorizontalAlignment { get; set; } = "left";
            public string VerticalAlignment { get; set; } = "baseline";
            public string MiniumHeight { get; set; } = "";
            public bool IsDottedLine { get; set; } = false;
            public string Leading { get; set; } = "line-height: 5pt";
            public float SpacingAfter { get; set; } = 6;
            public string BgColor { get; set; }
            public string BorderColor { get; set; }
            public string DottedWidth { get; set; } = "1";
            public string Offset { get; set; } = "2";
            public string Dotted { get; set; } = "";

            public string Padding { get; set; }

            public string PaddingLeft { get; set; }
            public string PaddingTop { get; set; }
            public string PaddingRight { get; set; }
            public string PaddingBottom { get; set; }

            public static string ParseToHorizontalAlignment(string v)
            {
                string value = v.ToUpper();
                switch (value)
                {
                    case "L": return "text-align: left;";
                    case "C": return "text-align: center;";
                    case "R": return "text-align: right;";
                    //case "JA": return "";
                    case "J": return "text-align: justify;";
                    default: return "text-align: left;";
                }
            }
            public static string ParseToVerticalAlignment(string v)
            {
                string value = v.ToUpper();
                switch (value)
                {
                    case "BS": return "align-items: baseline;";
                    case "T": return "align-items: top;";
                    case "M": return "align-items: middle;";
                    case "BT": return "align-items: bottom;";
                    default: return "align-items: baseline;";
                }
            }
            public static Tuple<char, char, char, char> GetPadding(string padding)
            {
                char wl = padding[0], wt = padding[1], wr = padding[2], wb = padding[2];

                return new Tuple<char, char, char, char>(wl, wt, wr, wb);
            }
            public static Tuple<float, float, float, float> GetBorderWidths(string borderMode)
            {
                float wl = 0, wt = 0, wr = 0, wb = 0, w = 1;
                if (borderMode.Length == 4)
                {
                    char cl = borderMode[0], ct = borderMode[1], cr = borderMode[2], cb = borderMode[3];

                    wl = cl == '1' ? w : cl == '2' ? w : 0;
                    wt = ct == '1' ? w : ct == '2' ? w : 0;
                    wr = cr == '1' ? w : cr == '2' ? w : 0;
                    wb = cb == '1' ? w : cb == '2' ? w : 0;
                }

                return new Tuple<float, float, float, float>(wl, wt, wr, wb);
            }
            public static void getDotedCell(string dotted, CustomCellSettings settings)
            {
                string[] arrTem = dotted.Split(',');
                settings.DottedWidth = arrTem[0];
                if (arrTem.Length > 1)
                    settings.Offset = arrTem[1];
            }
            public static void getFont(string font, CustomCellSettings settings)
            {
                string[] arrTem = font.Split('*');
                for (int i = 0; i < arrTem.Length; i++)
                {
                    string[] tem = arrTem[i].Split('.');
                    string size = tem[0];
                    string f = tem[1];

                    settings.FontStyle[i] = $"font-size: {size}pt;" + getFontWeight(f);
                    if (tem.Length > 2)
                        settings.FontStyle[i] += $"color: rgb({tem[2]});";
                }
            }
            public static string getFontWeight(string fw)
            {
                switch (fw)
                {
                    case "t": return "font-weight: 100;";
                    case "n": return "font-weight: 400;";
                    case "b": return "font-weight: 700;";
                    case "i": return "font-weight: 400;font-style: italic;";
                    case "l": return "font-weight: 700;font-style: italic;";
                    default: return "font-weight: 400;";
                }
            }


        }
        #endregion


        #region mã tiền tệ
        public const string AFN = "AFN";
        public const string AED = "AED";
        public const string ALL = "ALL";
        public const string AMD = "AMD";
        public const string ANG = "ANG";
        public const string AOA = "AOA";
        public const string ARS = "ARS";
        public const string AUD = "AUD";
        public const string AWG = "AWG";
        public const string AZN = "AZN";
        public const string BAM = "BAM";
        public const string BBD = "BBD";
        public const string BDT = "BDT";
        public const string BGN = "BGN";
        public const string BHD = "BHD";
        public const string BIF = "BIF";
        public const string BMD = "BMD";
        public const string BND = "BND";
        public const string BOB = "BOB";
        public const string BRL = "BRL";
        public const string BSD = "BSD";
        public const string BTN = "BTN";
        public const string BWP = "BWP";
        public const string BYN = "BYN";
        public const string BZD = "BZD";
        public const string CAD = "CAD";
        public const string CDF = "CDF";
        public const string CHF = "CHF";
        public const string CLP = "CLP";
        public const string CNY = "CNY";
        public const string COP = "COP";
        public const string CRC = "CRC";
        public const string CUP = "CUP";
        public const string CVE = "CVE";
        public const string CZK = "CZK";
        public const string DJF = "DJF";
        public const string DKK = "DKK";
        public const string DOP = "DOP";
        public const string DZD = "DZD";
        public const string EGP = "EGP";
        public const string ERN = "ERN";
        public const string ETB = "ETB";
        public const string EUR = "EUR";
        public const string FJD = "FJD";
        public const string FKP = "FKP";
        public const string GBP = "GBP";
        public const string GEL = "GEL";
        public const string GGP = "GGP";
        public const string GHS = "GHS";
        public const string GIP = "GIP";
        public const string GMD = "GMD";
        public const string GNF = "GNF";
        public const string GTQ = "GTQ";
        public const string GYD = "GYD";
        public const string HKD = "HKD";
        public const string HNL = "HNL";
        public const string HRK = "HRK";
        public const string HTG = "HTG";
        public const string HUF = "HUF";
        public const string IDR = "IDR";
        public const string ILS = "ILS";
        public const string IMP = "IMP";
        public const string INR = "INR";
        public const string IQD = "IQD";
        public const string IRR = "IRR";
        public const string ISK = "ISK";
        public const string JEP = "JEP";
        public const string JMD = "JMD";
        public const string JOD = "JOD";
        public const string JPY = "JPY";
        public const string KES = "KES";
        public const string KGS = "KGS";
        public const string KHR = "KHR";
        public const string KMF = "KMF";
        public const string KPW = "KPW";
        public const string KRW = "KRW";
        public const string KWD = "KWD";
        public const string KYD = "KYD";
        public const string KZT = "KZT";
        public const string LAK = "LAK";
        public const string LBP = "LBP";
        public const string LKR = "LKR";
        public const string LRD = "LRD";
        public const string LSL = "LSL";
        public const string LYD = "LYD";
        public const string MAD = "MAD";
        public const string MDL = "MDL";
        public const string MGA = "MGA";
        public const string MKD = "MKD";
        public const string MMK = "MMK";
        public const string MNT = "MNT";
        public const string MOP = "MOP";
        public const string MRO = "MRO";
        public const string MUR = "MUR";
        public const string MVR = "MVR";
        public const string MWK = "MWK";
        public const string MXN = "MXN";
        public const string MYR = "MYR";
        public const string MZN = "MZN";
        public const string NAD = "NAD";
        public const string NGN = "NGN";
        public const string NIO = "NIO";
        public const string NOK = "NOK";
        public const string NPR = "NPR";
        public const string NZD = "NZD";
        public const string OMR = "OMR";
        public const string PEN = "PEN";
        public const string PGK = "PGK";
        public const string PHP = "PHP";
        public const string PKR = "PKR";
        public const string PLN = "PLN";
        public const string PYG = "PYG";
        public const string QAR = "QAR";
        public const string RON = "RON";
        public const string RSD = "RSD";
        public const string RUB = "RUB";
        public const string RWF = "RWF";
        public const string SAR = "SAR";
        public const string SBD = "SBD";
        public const string SCR = "SCR";
        public const string SDG = "SDG";
        public const string SEK = "SEK";
        public const string SGD = "SGD";
        public const string SHP = "SHP";
        public const string SLL = "SLL";
        public const string SOS = "SOS";
        public const string SRD = "SRD";
        public const string SSP = "SSP";
        public const string STD = "STD";
        public const string SYP = "SYP";
        public const string SZL = "SZL";
        public const string THB = "THB";
        public const string TJS = "TJS";
        public const string TMT = "TMT";
        public const string TND = "TND";
        public const string TOP = "TOP";
        public const string TRY = "TRY";
        public const string TTD = "TTD";
        public const string TWD = "TWD";
        public const string TZS = "TZS";
        public const string UAH = "UAH";
        public const string UGX = "UGX";
        public const string USD = "USD";
        public const string UYU = "UYU";
        public const string UZS = "UZS";
        public const string VEF = "VEF";
        public const string VND = "VND";
        public const string VUV = "VUV";
        public const string WST = "WST";
        public const string XAF = "XAF";
        public const string XCD = "XCD";
        public const string XDR = "XDR";
        public const string XOF = "XOF";
        public const string XPF = "XPF";
        public const string YER = "YER";
        public const string ZAR = "ZAR";
        public const string ZMW = "ZMW";
        #endregion

    }


}
