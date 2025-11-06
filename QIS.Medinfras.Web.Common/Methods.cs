using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using System.Net;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using QIS.Medinfras.Data.Service;


namespace QIS.Medinfras.Web.Common
{
    public class Methods
    {
        public static void SetComboBoxField<T>(DropDownList ctl, List<T> list, string textField, string valueField)
        {
            ctl.DataTextField = textField;
            ctl.DataValueField = valueField;
            ctl.DataSource = list;
            ctl.DataBind();
        }


        public static void SetComboBoxField<T>(ASPxComboBox ctl, List<T> list, string textField, string valueField)
        {
            SetComboBoxField(ctl, list, textField, valueField, DropDownStyle.DropDownList);
        }

        public static void SetComboBoxField<T>(ASPxComboBox ctl, List<T> list, string textField, string valueField, DropDownStyle dropDownStyle)
        {
            ctl.TextField = textField;
            ctl.ValueField = valueField;
            ctl.DataSource = list;
            ctl.CallbackPageSize = 50;
            ctl.EnableCallbackMode = false;
            ctl.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            ctl.DropDownStyle = dropDownStyle;
            ctl.DataBind();
        }

        public static void SetRadioButtonListField<T>(RadioButtonList ctl, List<T> list, string textField, string valueField)
        {
            ctl.SelectedValue = null;
            ctl.DataTextField = textField;
            ctl.DataValueField = valueField;
            ctl.DataSource = list;
            ctl.DataBind();
        }

        /// <summary>
        /// Converts a string to an enumeration value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        #region Web API Utility Function
        public static void SetRequestHeader(HttpWebRequest Request)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string consID = AppSession.RIS_Consumer_ID;
            string pass = AppSession.RIS_Consumer_Pwd;
            string data = unixTimestamp.ToString() + consID;

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}", data), pass));
        }

        public static void SetRequestHeader(HttpWebRequest Request, string consID, string pass)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string data = unixTimestamp.ToString() + consID;

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}", data), pass));
        }

        public static void SetRequestHeaderWithHealthcareID(HttpWebRequest Request, string healthcareID, string consID, string pass)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string data = unixTimestamp.ToString() + consID;

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}", data), pass));
            Request.Headers.Add("X-healthcareID", healthcareID);
        }

        public static void SetRequestHeaderForMedixSoftView(HttpWebRequest Request, string referenceNo)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string consID = referenceNo;
            string pass = AppSession.RIS_Consumer_Pwd;
            string data = unixTimestamp.ToString() + consID;

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}", data), pass));
        }

        public static void SetRequestHeaderForNalagenetics(HttpWebRequest Request, string key)
        {
            Request.Headers.Add("x-api-key", key);
        }

        public static string GenerateSignature(string data, string secretKey)
        {
            // Initialize the keyed hash object using the secret key as the key
            HMACSHA256 hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));

            // Computes the signature by hashing the salt with the secret key as the key
            var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));

            // Base 64 Encode
            var encodedSignature = Convert.ToBase64String(signature);

            // URLEncode
            // encodedSignature = System.Web.HttpUtility.UrlEncode(encodedSignature);
            return encodedSignature;
        }
        #endregion

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public static string GenerateFilterExpFromPatientListBarcode(string filterExp, string param)
        {
            string whereClause = string.Empty;
            if (param.Length > 11)
            {
                //Registration Number
                if (!param.Contains("/"))
                {
                    //a little bit hard-code due to bio-connect maximum length for registration number
                    string tempRegNo = param;
                    string prefix = tempRegNo.Substring(0, 3);
                    string periodNo = tempRegNo.Substring(3, 8);
                    string counter = tempRegNo.Substring(11, 5);
                    string registrationNo = string.Format(@"{0}/{1}/{2}", prefix, periodNo, counter);
                    whereClause = string.Format(" RegistrationNo = '{0}'", registrationNo);
                }
                else
                {
                    whereClause = string.Format(" RegistrationNo = '{0}'", param);
                }
            }
            else
            {
                //Medical Number
                whereClause = string.Format(" MedicalNo = '{0}'", param);
            }

            if (string.IsNullOrEmpty(filterExp))
                filterExp = whereClause;
            else
                filterExp += string.Format(" AND {0}", whereClause);


            return filterExp;
        }

        public static string GenerateFilterExpFromUDDQRCode(string filterExp, string param)
        {
            //string[] paramInfo = param.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string[] paramInfo = param.Split('|');
            string whereClause = string.Empty;
            if (paramInfo.Length == 4)
            {
                string patientMRN = paramInfo[0];
                whereClause = string.Format(" MedicalNo = '{0}'", paramInfo[0]);
            }
            else
            {
                //Medical Number
                whereClause = string.Format(" MedicalNo = '{0}'", param);
            }

            if (string.IsNullOrEmpty(filterExp))
                filterExp = whereClause;
            else
                filterExp += string.Format(" AND {0}", whereClause);


            return filterExp;
        }

        public static decimal CalculateEWSScore(decimal rr, decimal spo2, decimal temp, decimal systolicBP, decimal hr, string avpu )
        {
            decimal result = -99; //Invalid Values

            #region Respiratory Rate
            decimal rrScore = 0;
            if (rr < 7 || rr > 35)
            {
                rrScore = 3;
            }
            else if (rr >= 31 && rr <= 35)
            {
                rrScore = 2;
            }
            else if (rr >= 21 && rr <= 30)
            {
                rrScore = 1;
            }
            else if (rr >= 9 && rr <= 20)
            {
                rrScore = 0;
            }
            #endregion

            #region SP02
            decimal spo2Score = 0;
            if (spo2 < 85)
            {
                spo2Score = 3;
            }
            else if (spo2 >= 85 && spo2 <= 89)
            {
                spo2Score = 2;
            }
            else if (spo2 >= 90 && spo2 <= 92)
            {
                spo2Score = 1;
            }
            else if (spo2 > 92)
            {
                spo2Score = 0;
            }
            #endregion

            #region Temperature
            decimal tempScore = 0;
            if (temp < 34)
            {
                tempScore = 3;
            }
            else if ((temp >= 34 && temp <= Convert.ToDecimal(34.9)) || (temp > Convert.ToDecimal(38.9)))
            {
                tempScore = 2;
            }
            else if ((temp >= 35 && temp <= Convert.ToDecimal(35.9)) || (temp >= 38 && temp <= Convert.ToDecimal(38.9)))
            {
                tempScore = 1;
            }
            else if (temp >= 36 && temp <= Convert.ToDecimal(37.9))
            {
                tempScore = 0;
            }
            #endregion

            #region Systolic BP
            decimal NBPsScore = 0;
            if (systolicBP < 70)
            {
                NBPsScore = 3;
            }
            else if ((systolicBP >= 70 && systolicBP <= 79) || (systolicBP > 199))
            {
                NBPsScore = 2;
            }
            else if (systolicBP >= 80 && systolicBP <= 99)
            {
                NBPsScore = 1;
            }
            else if (systolicBP >= 100 && systolicBP <= 199)
            {
                NBPsScore = 0;
            }
            #endregion

            #region Heart Rate
            decimal hrScore = 0;
            if (hr < 30 || hr > 129)
            {
                hrScore = 3;
            }
            else if ((hr >= 30 && hr <= 39) || (hr >= 110 && hr <= 129))
            {
                hrScore = 2;
            }
            else if ((hr >= 40 && hr <= 49) || (hr >= 100 && hr <= 109))
            {
                hrScore = 1;
            }
            else if (hr >= 50 && hr <= 99)
            {
                hrScore = 0;
            }
            #endregion

            #region AVPU
            decimal avpuScore = 0;
            switch (avpu)
            {
                case Constant.AVPU.ALERT:
                    avpuScore = 0;
                    break;
                case Constant.AVPU.VERBAL:
                    avpuScore = 1;
                    break;
                case Constant.AVPU.PAIN:
                    avpuScore = 2;
                    break;
                case Constant.AVPU.UNRESPONSIVE:
                    avpuScore = 3;
                    break;
                default:
                    break;
            }
            #endregion


            return result = rrScore + spo2Score + tempScore + NBPsScore + hrScore + avpuScore;
        }

        public static decimal CalculatePEWSScore(string behavior, string cardiovascular, string respiration)
        {
            decimal result = -99; //Invalid Values

            #region Behavior
            decimal score1 = 0;

            switch (behavior)
            {
                case Constant.PEWS_Behavior.X370_01:
                    score1 = 0;
                    break;
                case Constant.PEWS_Behavior.X370_02:
                    score1 = 1;
                    break;
                case Constant.PEWS_Behavior.X370_03:
                    score1 = 2;
                    break;
                case Constant.PEWS_Behavior.X370_04:
                    score1 = 3;
                    break;
                default:
                    break;
            }
            #endregion

            #region Cardiosvascular
            decimal score2 = 0;

            switch (cardiovascular)
            {
                case Constant.PEWS_Cardiovascular.X371_01:
                    score1 = 0;
                    break;
                case Constant.PEWS_Cardiovascular.X371_02:
                    score1 = 1;
                    break;
                case Constant.PEWS_Cardiovascular.X371_03:
                    score1 = 2;
                    break;
                case Constant.PEWS_Cardiovascular.X371_04:
                    score1 = 3;
                    break;
                default:
                    break;
            }
            #endregion

            #region Respiration
            decimal score3 = 0;
            switch (respiration)
            {
                case Constant.PEWS_Respiration.X372_01:
                    score3 = 0;
                    break;
                case Constant.PEWS_Respiration.X372_02:
                    score3 = 1;
                    break;
                case Constant.PEWS_Respiration.X372_03:
                    score3 = 2;
                    break;
                case Constant.PEWS_Respiration.X372_04:
                    score3 = 3;
                    break;
                default:
                    break;
            }
            #endregion

            return result = score1 + score2 + score3;
        }

        public static string GetMedicationSequenceTime(int frequency)
        {
            string result = "-|-|-|-|-|-";

            switch (frequency)
            {
                case 1:
                    result = "08:00|-|-|-|-|-";
                    break;
                case 2:
                    result = "08:00|20:00|-|-|-|-";
                    break;
                case 3:
                    result = "08:00|16:00|23:59|-|-|-";
                    break;
                case 4:
                    result = "06:00|12:00|18:00|23:59|-|-";
                    break;
                case 5:
                    result = "06:00|09:00|12:00|15:00|18:00|-";
                    break;
                case 6:
                    result = "02:00|06:00|10:00|14:00|18:00|22:00";
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// Used to search text something like SQL LIKE Search
        /// </summary>
        /// <param name="toSearch">Text value to be search</param>
        /// <param name="toFind">Text Value to be find</param>
        /// <param name="minTextLength">Minimum Length of text to be search</param>
        /// <returns></returns>
        public static bool SearchLike (string toSearch, string toFind, int minTextLength = 0)
        {
            List<string> listToSearch = toSearch.Replace(' ',',').Split(',').ToList();
            List<string> listToFind = toFind.Replace(' ', ',').Split(',').ToList();
            bool isFound = false;
            foreach (string text in listToFind)
            {
                if (!string.IsNullOrEmpty(text) && text.Length >= minTextLength)
                {
                    List<string> lstResult = (from val in listToSearch
                                              where val.Contains(text)
                                              select val).ToList(); // like '%toFind%'

                    isFound = (lstResult.Count > 0);
                }

                if (isFound) return isFound;
            }
            return isFound;
        }

        public static DateTime SetCurrentDate()
        {
            DateTime ServerTimeNow = DateTime.Now;
            //if (!string.IsNullOrEmpty(AppSession.TimeVariance))
            //{
            //    DateTime newServerTimeNow = ServerTimeNow.AddMinutes(Convert.ToInt32(AppSession.TimeVariance));
            //    return newServerTimeNow;

            //}
            //else
            //{
                return ServerTimeNow;
            //}
        }

        public static string SetCurrentTime()
        {
            DateTime ServerTimeNow = DateTime.Now;
            string TimeNow = ServerTimeNow.ToString(Constant.FormatString.TIME_FORMAT);
            //if (!string.IsNullOrEmpty(AppSession.TimeVariance) && AppSession.TimeVariance != "0")
            //{
            //    DateTime newServerTimeNow = ServerTimeNow.AddMinutes(Convert.ToInt32(AppSession.TimeVariance));
            //    TimeNow = newServerTimeNow.ToString(Constant.FormatString.TIME_FORMAT);
            //}
            return TimeNow;

        }

        #region Desktop Service Message
        public static string GenerateDesktopService(DesktopServiceMessageType messageType, string healthcareID, Data.Service.RegisteredPatient registeredPatient, string txnData)
        {
            string result = string.Empty;

            switch (messageType)
            {
                case DesktopServiceMessageType.MD101:
                    result = FormatMD1DesktopServiceMessage("MD101", healthcareID, registeredPatient, txnData);
                    break;
                case DesktopServiceMessageType.MD201:
                    result = FormatMD2DesktopServiceMessage("MD201", healthcareID, registeredPatient, txnData);
                    break;
                case DesktopServiceMessageType.MD202:
                    result = FormatMD2DesktopServiceMessage("MD202", healthcareID, registeredPatient, txnData);
                    break;
                default:
                    break;
            }

            return result;
        }

        private static string FormatMD1DesktopServiceMessage(string messageTypeCode, string healthcareID, Data.Service.RegisteredPatient registeredPatient, string txnData)
        {
            string header = string.Format("{0};{1}", messageTypeCode, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
            string pvi = string.Format("{0};{1}", healthcareID, registeredPatient.RegistrationNo);
            string pid = string.Format("{0};{1}", healthcareID, registeredPatient.RegistrationNo);
            string txn = string.Format("{0}", txnData);

            return string.Format("{0}|{1}|{2}|{3}", header, pvi, pid, txn);
        }

        private static string FormatMD2DesktopServiceMessage(string messageTypeCode, string healthcareID, Data.Service.RegisteredPatient registeredPatient, string txnData)
        {
            string header = string.Format("{0};{1}", messageTypeCode, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
            string pvi = string.Format("{0};{1}", healthcareID, registeredPatient.RegistrationNo);
            string pid = string.Format("{0};{1}", healthcareID, registeredPatient.RegistrationNo);
            string txn = string.Format("{0}", txnData);

            return string.Format("{0}|{1}|{2}|{3}", header, pvi, pid, txn);
        }

        #endregion

        #region Load Quick Search Intellisense Hints
        public static List<CustomControl.QISIntellisenseHint> LoadRegistrationWorklistQuickFilterHints(string type)
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data/searchHints");

            string fileName = string.Format(@"{0}\QuickSearchHints.cfg", filePath);
            IEnumerable<string> lstHints = File.ReadAllLines(fileName);
            StringBuilder commandText = new StringBuilder();
            List<CustomControl.QISIntellisenseHint> lst = new List<CustomControl.QISIntellisenseHint>();
            foreach (string hint in lstHints)
            {
                string[] hintInfo = hint.Split(';');
                lst.Add(new CustomControl.QISIntellisenseHint() { FieldName = hintInfo[0], Text = hintInfo[1], Description = hintInfo[2] });
            }
            return lst;
        }

        public static List<CustomControl.QISIntellisenseHint> LoadPharmacyWorklistQuickFilterHints(string type)
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data/searchHints");

            string fileName = string.Format(@"{0}\QuickSearchPHOrderHints.cfg", filePath);
            IEnumerable<string> lstHints = File.ReadAllLines(fileName);
            StringBuilder commandText = new StringBuilder();
            List<CustomControl.QISIntellisenseHint> lst = new List<CustomControl.QISIntellisenseHint>();
            foreach (string hint in lstHints)
            {
                string[] hintInfo = hint.Split(';');
                lst.Add(new CustomControl.QISIntellisenseHint() { FieldName = hintInfo[0], Text = hintInfo[1], Description = hintInfo[2] });
            }
            return lst;
        }

        public static List<CustomControl.QISIntellisenseHint> LoadDiagnosticWorklistQuickFilterHints(string type)
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data/searchHints");

            string fileName = string.Format(@"{0}\QuickSearchMDOrderHints.cfg", filePath);
            IEnumerable<string> lstHints = File.ReadAllLines(fileName);
            StringBuilder commandText = new StringBuilder();
            List<CustomControl.QISIntellisenseHint> lst = new List<CustomControl.QISIntellisenseHint>();
            foreach (string hint in lstHints)
            {
                string[] hintInfo = hint.Split(';');
                lst.Add(new CustomControl.QISIntellisenseHint() { FieldName = hintInfo[0], Text = hintInfo[1], Description = hintInfo[2] });
            }
            return lst;
        }

        public static List<CustomControl.QISIntellisenseHint> LoadLaboratoryWorklistQuickFilterHints(string type)
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data/searchHints");

            string fileName = string.Format(@"{0}\QuickSearchLBOrderHints.cfg", filePath);
            IEnumerable<string> lstHints = File.ReadAllLines(fileName);
            StringBuilder commandText = new StringBuilder();
            List<CustomControl.QISIntellisenseHint> lst = new List<CustomControl.QISIntellisenseHint>();
            foreach (string hint in lstHints)
            {
                string[] hintInfo = hint.Split(';');
                lst.Add(new CustomControl.QISIntellisenseHint() { FieldName = hintInfo[0], Text = hintInfo[1], Description = hintInfo[2] });
            }
            return lst;
        }
        #endregion

        /// <summary>
        /// Load Layout Form Elektronik (HTML) berdasarkan lokasi dan nama file
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static StringBuilder LoadHTMLFormContent(string fileLocation, string fileName)
        {
            string file = string.Format(@"{0}\{1}", fileLocation, fileName);
            StringBuilder innerHtml = new StringBuilder();
            if (File.Exists(file))
            {
                IEnumerable<string> lstText = File.ReadAllLines(file);
                foreach (string text in lstText)
                {
                    innerHtml.AppendLine(text);
                }
            }
            return innerHtml;
        }

        /// <summary>
        /// Validasi format jam dalam format : HH:MM (24-Hour Format)
        /// Regex : @"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$"
        /// </summary>
        /// <param name="toSearch">Text value to be search</param>
        /// <returns></returns>
        public static bool ValidateTimeFormat(string timeText)
        {
            string format = @"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$"; //24-Hour Format
            var match = Regex.Match(timeText, format, RegexOptions.IgnoreCase);
            return match.Success;
        }

        public static bool IsNumeric(string value)
        {
            double test;
            return double.TryParse(value, out test);
        }

        public static void LogIntegrationNotesError(string noteDate, string noteTime, string paramedicCode, string noteText)
        {
            string fileName = string.Format("{0}_{1}_{2}_{3}", noteDate, noteTime.Replace(':', '_'), paramedicCode, DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT_5.Replace(':','_')));
            string myFile = VirtualPathUtility.ToAbsolute(string.Format("~/Libs/App_Data/log/{0}.cppt", fileName));
            string physicalPath = HttpContext.Current.Request.MapPath(myFile);

            if (!File.Exists(physicalPath))
                File.WriteAllText(physicalPath, noteText);
            else
                File.AppendAllText(physicalPath, noteText);
        }

        #region Bridging Helper Methods
        private static VisitInfo ConvertVisitToDTO(ConsultVisit entityVisit)
        {
            VisitInfo visitInfo = new VisitInfo();
            visitInfo.VisitID = entityVisit.VisitID;
            visitInfo.VisitDate = entityVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            visitInfo.VisitTime = entityVisit.VisitTime;
            visitInfo.RoomID = entityVisit.RoomID;
            visitInfo.PhysicianID = Convert.ToInt32(entityVisit.ParamedicID);
            return visitInfo;
        }
        private static PatientData ConvertPatientToDTO(Patient oPatient)
        {
            PatientData oData = new PatientData();
            oData.PatientID = oPatient.MRN;
            oData.MedicalNo = oPatient.MedicalNo;
            oData.FirstName = oPatient.FirstName;
            oData.MiddleName = oPatient.MiddleName;
            oData.LastName = oPatient.LastName;
            oData.PrefferedName = oPatient.PreferredName;
            oData.Gender = oPatient.GCGender.Substring(5);
            oData.Religion = string.Empty;
            oData.MaritalStatus = string.Empty;
            oData.Nationality = string.Empty;

            oData.DateOfBirth = oPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112);
            oData.CityOfBirth = oPatient.CityOfBirth;
            oData.MobileNo1 = oPatient.MobilePhoneNo1;
            oData.MobileNo2 = oPatient.MobilePhoneNo2;
            oData.EmailAddress = oPatient.EmailAddress;
            return oData;
        } 
        #endregion
    }
}
