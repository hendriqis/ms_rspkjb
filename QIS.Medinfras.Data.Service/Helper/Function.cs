using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service.Helper;
using Newtonsoft.Json;

namespace QIS.Medinfras.Data.Service
{
    public class Function
    {
        #region Insert Log
        public enum LogType
        {
            Insert = 0, Update = 1, Delete = 2
        }


        public static void InsertLog(object obj, LogType logType)
        {
            Thread thread1 = new Thread(new ThreadStart(() => InsertLogToTextFile(obj, logType)));
            thread1.Start();
        }

        static void InsertLogToTextFile(object obj, LogType logType)
        {
            if (obj is DbDataModel)
            {
                Type type = obj.GetType();

                string physicalPath = "D:\\tablelog\\conf.txt";
                string[] lstConf = System.IO.File.ReadAllLines(physicalPath, Encoding.GetEncoding("windows-1250"));
                string _tableName = GetTableName(type);
                foreach (string conf in lstConf)
                {
                    string[] temp = conf.Split(';');
                    if (temp[0] == _tableName)
                    {
                        physicalPath = string.Format("D:\\tablelog\\{0}\\", _tableName);
                        if (!Directory.Exists(physicalPath))
                            Directory.CreateDirectory(physicalPath);

                        string myFile = string.Format("{0}\\{1}.txt", physicalPath, DateTime.Now.ToString("yyyyMMdd"));

                        string[] listColumn = temp[1].Split('|');
                        string message = "";
                        PropertyInfo[] propInfs = type.GetProperties();
                        foreach (PropertyInfo prop in propInfs)
                        {
                            object[] custAttr = prop.GetCustomAttributes(false);
                            foreach (Attribute attrib in custAttr)
                            {
                                ColumnAttribute schema = attrib as ColumnAttribute;
                                if (schema != null)
                                {
                                    if (listColumn.Contains(schema.Name))
                                    {
                                        object fieldValue = prop.GetValue(obj, null);
                                        if (!schema.IsNullable)
                                            fieldValue = CheckIsNull(fieldValue, prop.PropertyType);

                                        if (message != "")
                                            message += "|";
                                        if (fieldValue != null)
                                            message += fieldValue;
                                        else
                                            message += "NULL";
                                    }
                                }
                            }
                        }
                        message += "|" + ((int)logType).ToString();
                        message += Environment.NewLine;
                        if (!File.Exists(myFile))
                            File.WriteAllText(myFile, message);
                        else
                            File.AppendAllText(myFile, message);
                        break;
                    }
                }
            }
        }

        public object CheckIsNull(object obj)
        {
            if (obj == null) return null;
            Type type = obj.GetType();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                foreach (Attribute attrib in prop.GetCustomAttributes(true))
                {
                    ColumnAttribute schema = attrib as ColumnAttribute;
                    if (schema != null && !schema.IsNullable)
                    {
                        prop.SetValue(obj, CheckIsNull(prop.GetValue(obj, null), prop.PropertyType), null);
                    }
                }
            }
            return obj;
        }


        private static object CheckIsNull(object obj, Type type)
        {
            //if (type.FullName.Contains("DateTime"))
            //{
            //    if (obj is DBNull || obj == null)
            //        return Convert.ToDateTime("1900-01-01");
            //    if (Convert.ToDateTime(obj).Year < 1900)
            //        return Convert.ToDateTime("1900-01-01");
            //}
            //else if (obj is DBNull || obj == null)
            //{
            //    if (type.FullName.Contains("String")) return string.Empty;
            //    if (type.FullName.Contains("Int16")) return 0;
            //    if (type.FullName.Contains("Int32")) return 0;
            //    if (type.FullName.Contains("Int64")) return 0;
            //    if (type.FullName.Contains("Boolean")) return false;
            //    if (type.FullName.Contains("Double")) return Double.NaN;
            //    if (type.FullName.Contains("Decimal")) return Decimal.Zero;
            //    if (type.FullName.Contains("DateTime")) return Convert.ToDateTime("1900-01-01");
            //    if (type.FullName.Contains("Byte")) return Byte.MinValue;
            //    if (type.FullName.Contains("Byte[]")) return new Byte[] { };
            //}

            if (type.FullName.Contains("DateTime"))
            {
                if (obj is DBNull || obj == null)
                    return Convert.ToDateTime("1900-01-01");
                if (Convert.ToDateTime(obj).Year < 1900)
                    return Convert.ToDateTime("1900-01-01");
            }
            else if (obj is DBNull || obj == null)
            {
                if (type.FullName.Contains("String")) return string.Empty;
                //if (type.FullName.Contains("Int64")) return 0;
                if (type.FullName.Contains("Boolean")) return false;
                return null;
            }
            return obj;
        }

        private static string GetTableName(MemberInfo type)
        {
            TableAttribute tableInfo = (TableAttribute)Attribute.GetCustomAttribute(type, typeof(TableAttribute));
            if (tableInfo == null || tableInfo.Name.Equals(""))
            {
                return type.Name;
            }
            else
            {
                return tableInfo.Name;
            }
        }
        #endregion

        /// <summary>
        /// parameter : string time ( yyyyMMdd )
        /// </summary>
        /// <param name="timeIn_yyyyMMdd"></param>
        /// <returns></returns>
        public static DateTime StringToDateTime(string time)
        {
            DateTime theTime = DateTime.ParseExact(time,
                                        "yyyyMMdd",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None);
            return theTime;
        }

        public static String ToRoman(long number)
        {
            if (-3999999 >= number || number >= 3999999)
            {
                throw new ArgumentOutOfRangeException("number");
            }

            if (number == 0)
            {
                return "NUL";
            }

            StringBuilder sb = new StringBuilder(1000);

            //if (number < 0)
            //{
            //    sb.Append('-');
            //    number *= -1;
            //}

            //string[,] table = new string[,] { 
            //    { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" }, // 1, 2, 3, 4, 5, 6, 7, 8, 9
            //    { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" }, // 10, 20, 30, 40, 50, 60, 70, 80, 90
            //    { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" }, // 100, 200, 300, 400, 500, 600, 700, 800, 900
            //    { "", "M", "MM", "MMM", "M(V)", "(V)", "(V)M", "(V)MM", "(V)MMM", "M(X)" }, // 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000
            //    { "", "(X)", "(X)(X)", "(X)(X)(X)", "(X)(L)", "(L)", "(L)(X)", "(L)(X)(X)", "(L)(X)(X)(X)", "(X)(C)" }, // 10rb, 20rb, 30rb, 40rb, 50rb, 60rb, 70rb, 80rb, 90rb
            //    { "", "(C)", "(C)(C)", "(C)(C)(C)", "(C)(D)", "(D)", "(D)(C)", "(D)(C)(C)", "(D)(C)(C)(C)", "(C)(M)" }, // 100rb, 200rb, 300rb, 400rb, 500rb, 600rb, 700rb, 800rb, 900rb
            //    { "", "(M)", "(M)(M)", "(M)(M)(M)", "_undefined_", "_undefined_", "_undefined_", "_undefined_", "_undefined_", "_undefined_" } // 1jt, 2jt, 3jt
            //};
            // Notes :
            // 4rb >>> (I)(V) atau M(V)
            // 9rb >>> (I)(X) atau M(X)

            //for (long i = 1000000, j = 3; i > 0; i /= 10, j--)
            //{
            //    long digit = number / i;
            //    sb.Append(table[j, digit]);
            //    number -= digit * i;
            //}

            var result = string.Empty;
            var map = new Dictionary<string, int>
            {
                {"(M)", 1000000},
                {"(C)(M)", 900000},
                {"(D)", 500000},
                {"(C)(D)", 400000},
                {"(C)", 100000},
                {"(X)(C)", 90000},
                {"(L)", 50000},
                {"(X)(L)", 40000},
                {"(X)", 10000},
                {"(I)(X)", 9000},
                {"(V)", 5000},
                {"(I)(V)", 4000},
                {"M", 1000},
                {"CM", 900},
                {"D", 500},
                {"CD", 400},
                {"C", 100},
                {"XC", 90},
                {"L", 50},
                {"XL", 40},
                {"X", 10},
                {"IX", 9},
                {"V", 5},
                {"IV", 4},
                {"I", 1}
            };
            foreach (var pair in map)
            {
                result += string.Join(string.Empty, Enumerable.Repeat(pair.Key, Convert.ToInt32((number / pair.Value))));
                number %= pair.Value;
            }

            sb.Append(result);

            return sb.ToString();
        }

        public static String GenerateAddress(String _StreetName, String _County, String _District, String _City, String _State)
        {
            StringBuilder result = new StringBuilder();
            if (_StreetName != "")
                result.Append(_StreetName).Append(" ");
            if (_County != "")
                result.Append(_County).Append(" ");
            if (_District != "")
                result.Append(_District).Append(" ");
            if (_City != "")
                result.Append(_City).Append(" ");
            if (_State != "")
                result.Append(_State).Append(" ");
            return result.ToString();
        }

        public static String GeneratePhysicianPictureFileName(string pictureFileName)
        {
            Random random = new Random();
            int randomNum = random.Next(1000000, 100000000);
            string filePath = string.Format("{0}{1}{2}", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, pictureFileName);
            FileInfo file = new FileInfo(filePath);
            string imageUrl = string.Format("{0}{1}{2}", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISParamedicImagePath, pictureFileName);
            if (file.Exists)
                imageUrl = string.Format("{0}{1}{2}", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISParamedicImagePath, pictureFileName);
            else
                imageUrl = string.Format("{0}{1}{2}", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISParamedicImagePath, "physician.png");

            return string.Format("{0}?{1}", imageUrl, randomNum);
        }
        public static String GeneratePatientPictureFileName(string pictureFileName, string medicalNo, string gender = Constant.StandardCode.Gender.MALE, int ageInYear = 0, bool isByAge = false)
        {
            Random random = new Random();
            int randomNum = random.Next(1000000, 100000000);
            string imageUrl = string.Format(@"{0}/Patient/{1}", AppConfigManager.QISVirtualDirectory, "patient.png");
            if (String.IsNullOrEmpty(pictureFileName))
            {
                imageUrl = string.Format(@"{0}/Patient/{1}", AppConfigManager.QISVirtualDirectory, "patient.png");
            }
            else
            {
                string filePath = string.Format("{0}{1}{2}", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISPatientImagePath, pictureFileName).Replace(Constant.SpecialField.MRN, medicalNo).Replace("/", @"\");
                FileInfo file = new FileInfo(filePath);
                if (file.Exists)
                {
                    imageUrl = string.Format("{0}{1}{2}", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISPatientImagePath, pictureFileName).Replace(Constant.SpecialField.MRN, medicalNo);
                }
                else
                {
                    if (isByAge)
                    {
                        if (ageInYear >= 0 && ageInYear <= 5)
                            imageUrl = string.Format(@"{0}Patient\{1}", AppConfigManager.QISVirtualDirectory, gender == Constant.StandardCode.Gender.MALE ? "patient_male_baby.png" : "patient_female_baby.png");
                        else if (ageInYear > 5 && ageInYear <= 13)
                            imageUrl = string.Format(@"{0}Patient\{1}", AppConfigManager.QISVirtualDirectory, gender == Constant.StandardCode.Gender.MALE ? "patient_male_child.png" : "patient_female_child.png");
                        else
                            imageUrl = string.Format(@"{0}Patient\{1}", AppConfigManager.QISVirtualDirectory, gender == Constant.StandardCode.Gender.MALE ? "patient_male.png" : "patient_female.png");
                    }
                    else
                    {
                        switch (gender)
                        {
                            case Constant.StandardCode.Gender.MALE:
                                imageUrl = string.Format(@"{0}Patient\{1}", AppConfigManager.QISVirtualDirectory, "patient_male.png");
                                break;
                            case Constant.StandardCode.Gender.FEMALE:
                                imageUrl = string.Format(@"{0}Patient\{1}", AppConfigManager.QISVirtualDirectory, "patient_female.png");
                                break;
                            default:
                                imageUrl = string.Format(@"{0}Patient\{1}", AppConfigManager.QISVirtualDirectory, "patient.png");
                                break;
                        }
                        imageUrl = string.Format(@"{0}Patient\{1}", AppConfigManager.QISVirtualDirectory, gender == Constant.StandardCode.Gender.MALE ? "patient_male.png" : "patient_female.png");
                    }
                }
            }

            return string.Format("{0}?{1}", imageUrl, randomNum);
        }

        public static String GenerateGuestPictureFileName(string pictureFileName, string medicalNo, string gender = Constant.StandardCode.Gender.MALE, int ageInYear = 0, bool isByAge = false)
        {
            Random random = new Random();
            int randomNum = random.Next(1000000, 100000000);
            string imageUrl = string.Format(@"{0}/Guest/{1}", AppConfigManager.QISVirtualDirectory, "patient.png");
            if (String.IsNullOrEmpty(pictureFileName))
            {
                imageUrl = string.Format(@"{0}/Guest/{1}", AppConfigManager.QISVirtualDirectory, "patient.png");
            }
            else
            {
                string filePath = string.Format("{0}{1}{2}", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISGuestImagePath, pictureFileName).Replace(Constant.SpecialField.GUESTNO, medicalNo).Replace("/", @"\");
                FileInfo file = new FileInfo(filePath);
                if (file.Exists)
                {
                    imageUrl = string.Format("{0}{1}{2}", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISGuestImagePath, pictureFileName).Replace(Constant.SpecialField.GUESTNO, medicalNo);
                }
                else
                {
                    if (isByAge)
                    {
                        if (ageInYear >= 0 && ageInYear <= 5)
                            imageUrl = string.Format(@"{0}Guest\{1}", AppConfigManager.QISVirtualDirectory, gender == Constant.StandardCode.Gender.MALE ? "patient_male_baby.png" : "patient_female_baby.png");
                        else if (ageInYear > 5 && ageInYear <= 13)
                            imageUrl = string.Format(@"{0}Guest\{1}", AppConfigManager.QISVirtualDirectory, gender == Constant.StandardCode.Gender.MALE ? "patient_male_child.png" : "patient_female_child.png");
                        else
                            imageUrl = string.Format(@"{0}Guest\{1}", AppConfigManager.QISVirtualDirectory, gender == Constant.StandardCode.Gender.MALE ? "patient_male.png" : "patient_female.png");
                    }
                    else
                    {
                        switch (gender)
                        {
                            case Constant.StandardCode.Gender.MALE:
                                imageUrl = string.Format(@"{0}Guest\{1}", AppConfigManager.QISVirtualDirectory, "patient_male.png");
                                break;
                            case Constant.StandardCode.Gender.FEMALE:
                                imageUrl = string.Format(@"{0}Guest\{1}", AppConfigManager.QISVirtualDirectory, "patient_female.png");
                                break;
                            default:
                                imageUrl = string.Format(@"{0}Guest\{1}", AppConfigManager.QISVirtualDirectory, "patient.png");
                                break;
                        }
                        imageUrl = string.Format(@"{0}Guest\{1}", AppConfigManager.QISVirtualDirectory, gender == Constant.StandardCode.Gender.MALE ? "patient_male.png" : "patient_female.png");
                    }
                }
            }

            return string.Format("{0}?{1}", imageUrl, randomNum);
        }

        public static String NumberInWords(Int64 amount, Boolean isMoney = false)
        {
            StringBuilder strbuild = new StringBuilder();
            Int64 amountNew = amount;
            bool flag = false;

            if (amount < 0)
            {
                amountNew = amount * -1;
                flag = true;
            }

            if (isMoney)
            {
                strbuild.Insert(0, "RUPIAH");
            }

            String[] arrBil = { "", "SATU ", "DUA ", "TIGA ", "EMPAT ", "LIMA ", "ENAM ", "TUJUH ", "DELAPAN ", "SEMBILAN ", "SE" };
            String[] arrSatKecil = { "", "PULUH ", "RATUS " };
            String[] arrSatBesar = { "", "RIBU ", "JUTA ", "MILYAR ", "TRILIUN ", "KUADRILIUN " };
            int ctrKecil = 0;
            int ctrBesar = 0;
            if (amountNew == 0)
            {
                if (isMoney)
                    return "NOL RUPIAH";
                else
                    return "NOL";
            }
            else
            {
                while (amountNew > 0)
                {
                    long a = amountNew % 10;
                    amountNew /= 10;

                    if (a > 0)
                        strbuild.Insert(0, arrSatKecil[ctrKecil]);

                    if (a == 1 && ctrKecil > 0)
                        strbuild.Insert(0, arrBil[10]);
                    else if (ctrKecil == 0 && amountNew % 10 == 1 && a > 0)
                    {
                        strbuild.Insert(0, "BELAS ");
                        if (a == 1)
                            a = 10;
                        strbuild.Insert(0, arrBil[a]);
                        amountNew /= 10;
                        ctrKecil++;
                    }
                    else
                        strbuild.Insert(0, arrBil[a]);

                    ctrKecil++;
                    if (ctrKecil % 3 == 0)
                    {
                        ctrBesar++;
                        ctrKecil = 0;
                        if (amountNew > 0 && amountNew % 1000 > 0)
                        {
                            strbuild.Insert(0, arrSatBesar[ctrBesar]);
                        }
                    }

                }
                if (flag)
                {
                    strbuild.Insert(0, "MINUS ");
                }
                return strbuild.ToString();
            }
        }

        public static String NumberInWordsInEnglish(Int64 amount, Boolean isMoney = false)
        {
            if (amount == 0)
                return "ZERO";

            if (amount < 0)
                return "MINUS " + NumberInWordsInEnglish(Math.Abs(amount));

            string words = "";

            if ((amount / 1000000000) > 0)
            {
                words += NumberInWordsInEnglish(amount / 1000000000) + " BILLION ";
                amount %= 1000000;
            }

            if ((amount / 1000000) > 0)
            {
                words += NumberInWordsInEnglish(amount / 1000000) + " MILLION ";
                amount %= 1000000;
            }

            if ((amount / 1000) > 0)
            {
                words += NumberInWordsInEnglish(amount / 1000) + " THOUSAND ";
                amount %= 1000;
            }

            if ((amount / 100) > 0)
            {
                words += NumberInWordsInEnglish(amount / 100) + " HUNDRED ";
                amount %= 100;
            }

            if (amount > 0)
            {
                if (words != "")
                    words += "AND ";

                var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", 
                        "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
                var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

                if (amount < 20)
                    words += unitsMap[amount];
                else
                {
                    words += tensMap[amount / 10];
                    if ((amount % 10) > 0)
                        words += "-" + unitsMap[amount % 10];
                }
            }

            if (isMoney)
            {
                words += " RUPIAH";
            }

            return words;
        }

        #region New Convert Number In Indonesian Words

        public static String NumberWithPointInWordsInIndonesian(Double amount, Boolean isMoney = false)
        {
            StringBuilder strbuild = new StringBuilder();

            string[] amountInSplit = amount.ToString().Split('.');


            String[] arrBil = { "", "SATU ", "DUA ", "TIGA ", "EMPAT ", "LIMA ", "ENAM ", "TUJUH ", "DELAPAN ", "SEMBILAN ", "SE" };
            String[] arrSatKecil = { "", "PULUH ", "RATUS " };
            String[] arrSatBesar = { "", "RIBU ", "JUTA ", "MILYAR ", "TRILIUN ", "KUADRILIUN " };
            int ctrKecil = 0;
            int ctrBesar = 0;

            Int64 amountNew = Convert.ToInt64(amountInSplit[0]); // angka di depan koma desimal

            bool flagMinus = false;

            if (amountNew < 0)
            {
                amountNew = amountNew * -1;
                flagMinus = true;
            }

            if (amountNew == 0)
            {
                if (isMoney)
                    return "NOL RUPIAH";
                else
                    return "NOL";
            }
            else
            {
                while (amountNew > 0)
                {
                    long a = amountNew % 10;
                    amountNew /= 10;

                    if (a > 0)
                    {
                        strbuild.Insert(0, arrSatKecil[ctrKecil]);
                    }

                    if (a == 1 && ctrKecil > 0)
                    {
                        strbuild.Insert(0, arrBil[10]);
                    }
                    else if (ctrKecil == 0 && amountNew % 10 == 1 && a > 0)
                    {
                        strbuild.Insert(0, "BELAS ");
                        if (a == 1)
                            a = 10;
                        strbuild.Insert(0, arrBil[a]);
                        amountNew /= 10;
                        ctrKecil++;
                    }
                    else
                    {
                        strbuild.Insert(0, arrBil[a]);
                    }

                    ctrKecil++;
                    if (ctrKecil % 3 == 0)
                    {
                        ctrBesar++;
                        ctrKecil = 0;
                        if (amountNew > 0 && amountNew % 1000 > 0)
                        {
                            strbuild.Insert(0, arrSatBesar[ctrBesar]);
                        }
                    }
                }

                if (flagMinus)
                {
                    strbuild.Insert(0, "MINUS ");
                }

                #region Point
                Int64 amountPointNew = 0;
                string amountDecimal1 = "0";
                string amountDecimal2 = "0";

                if (amountInSplit.Count() > 1)
                {
                    amountPointNew = Convert.ToInt64(amountInSplit[1]); // angka di belakang koma desimal

                    if (amountInSplit[1].Length > 1)
                    {
                        amountDecimal1 = amountInSplit[1].Substring(0, 1);
                        amountDecimal2 = amountInSplit[1].Substring(1, 1);
                    }
                    else
                    {
                        amountDecimal1 = amountInSplit[1].Substring(0, 1);
                    }

                    if (amountPointNew > 0)
                    {
                        int point1 = Convert.ToInt32(amountDecimal1);
                        int point2 = Convert.ToInt32(amountDecimal2);

                        strbuild.Append(" KOMA ");

                        if (point1 == 0)
                        {
                            strbuild.Append("NOL ");
                        }
                        else
                        {
                            strbuild.Append(arrBil[point1]);
                        }

                        if (point2 == 0)
                        {
                            //strbuild.Append("NOL");
                        }
                        else
                        {
                            strbuild.Append("PULUH ");
                            strbuild.Append(arrBil[point2]);
                        }
                    }
                }
                #endregion

                strbuild.Append(" RUPIAH");

                return strbuild.ToString();
            }
        }

        #endregion

        #region New Convert Number In English Words (from https://www.codeproject.com/Lounge.aspx?msg=4667570#xx4667570xx)

        private enum Digit
        {
            ZERO = 0, ONE = 1, TWO = 2, THREE = 3, FOUR = 4, FIVE = 5, SIX = 6, SEVEN = 7, EIGHT = 8, NINE = 9
        }

        private enum Teen
        {
            TEN = 10, ELEVEN = 11, TWELVE = 12, THIRTEEN = 13, FOURTEEN = 14, FIFTEEN = 15, SIXTEEN = 16, SEVENTEEN = 17, EIGHTEEN = 18, NINETEEN = 19
        }

        private enum Ten
        {
            TWENTY = 2, THIRTY = 3, FORTY = 4, FIFTY = 5, SIXTY = 6, SEVENTY = 7, EIGHTY = 8, NINETY = 9
        }

        private enum PowerOfTen
        {
            HUNDRED = 0, THOUSAND = 1, MILLION = 2, BILLION = 3, TRILLION = 4, QUADRILLION = 5, QUINTILLION = 6
        }

        /// <summary>
        /// How many powers of ten there are; faster to work this out ahead of time,
        /// and I didn't want to hard-code it into the algorithm...
        /// </summary>
        private static int PowersOfTen = Enum.GetValues(typeof(PowerOfTen)).Length;

        /// <summary>
        /// Converts a number to English words
        /// </summary>
        /// <param name="N">The number</param>
        /// <returns>The number, in English</returns>
        public static string NumberWithPointInWordsInEnglish(Double N, Boolean isMoney = false)
        {
            string Prefix = N < 0 ? "MINUS " : "";
            string Significand = Digit.ZERO.ToString();
            string Mantissa = "";
            if ((N = Math.Abs(N)) > 0)
            {
                // Do the Mantissa
                if (N != Math.Floor(N))
                {
                    Mantissa = " POINT";

                    Int32 point = Convert.ToInt32(N.ToString().Substring(N.ToString().IndexOf('.') + 1));
                    if (point < 10)
                    {
                        foreach (char C in N.ToString().Substring(N.ToString().IndexOf('.') + 1))
                        {
                            Mantissa += " " + ((Digit)(int.Parse(C.ToString())));
                        }
                    }
                    else
                    {
                        Int16 i = 0;
                        foreach (char C in N.ToString().Substring(N.ToString().IndexOf('.') + 1))
                        {
                            if (i == 0)
                            {
                                Mantissa += " " + ((Ten)(int.Parse(C.ToString())));
                            }
                            else if (i == 1)
                            {
                                Mantissa += " " + ((Digit)(int.Parse(C.ToString())));
                            }
                            i++;
                        }
                    }
                }

                // Figure out the bit of the Significand less than 100
                long n = Convert.ToInt64(N = Math.Floor(N)) % 100;
                Significand = n == 0 ? ""
                  : n < 10 ? ((Digit)n).ToString()
                  : n < 20 ? ((Teen)n).ToString()
                  : ((Digit)(n % 10)) != 0 ? ((Ten)(n / 10) + "-" + (Digit)(n % 10)).ToString()
                  : ((Ten)(n / 10)).ToString();

                // Do the other powers of 10, if there are any
                if ((N = Math.Floor(N / 100D)) > 0)
                {
                    string EW = "";
                    for (int i = 0; (N > 0) && (i < PowersOfTen); i++)
                    {
                        double p = Math.Pow(10, (i << 1) + 1);
                        n = Convert.ToInt64(N % p);
                        if (n > 0)
                            EW = NumberWithPointInWordsInEnglish(n) + " " + (PowerOfTen)i + (EW.Length == 0 ? "" : ", " + EW);
                        N = Math.Floor(N / p);
                    }
                    if (EW.Length > 0)
                        Significand = EW + (Significand.Length == 0 ? "" : " AND " + Significand);
                }
            }

            if (isMoney)
            {
                return Prefix + (Significand + Mantissa).Trim() + " RUPIAH";
            }
            else
            {
                return Prefix + (Significand + Mantissa).Trim();
            }
        }

        #endregion

        public static string DifferentDateTimeInTime(string value1, string value2)
        {
            DateTime date1 = DateTime.ParseExact(value1, "dd-MM-yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime date2 = DateTime.ParseExact(value2, "dd-MM-yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);
            TimeSpan duration = date2 - date1;
            double totalMin = duration.TotalMinutes;

            int day = Convert.ToInt32(Math.Floor(totalMin / 60)) / 24;
            int hour = Convert.ToInt32(Math.Floor((totalMin - (day * 24 * 60)) / 60));
            int min = Convert.ToInt32(totalMin - (day * 24 * 60) - (hour * 60));

            string dayInString = day.ToString();
            string hourInString = hour.ToString();
            string minInString = min.ToString();

            if (dayInString.Length == 1) dayInString = string.Format("0{0}", dayInString);
            if (hourInString.Length == 1) hourInString = string.Format("0{0}", hourInString);
            if (minInString.Length == 1) minInString = string.Format("0{0}", minInString);

            if (dayInString == "00")
            {
                if (hourInString == "00" && minInString != "00")
                {
                    return string.Format("{0} Menit", minInString);
                }
                else if (hourInString != "00" && minInString == "00")
                {
                    return string.Format("{0} Jam", hourInString);
                }
                else if (hourInString == "00")
                {
                    return string.Format("{0} Menit", minInString);
                }
                else
                {
                    return string.Format("{0} Jam {1} Menit", hourInString, minInString);
                }
            }
            else
            {
                if (hourInString == "00" && minInString != "00")
                {
                    return string.Format("{0} Hari {1} Menit", dayInString, minInString);
                }
                else if (hourInString != "00" && minInString == "00")
                {
                    return string.Format("{0} Hari {1} Jam", dayInString, hourInString);
                }
                else
                {
                    return string.Format("{0} Hari {1} Jam {2} Menit", dayInString, hourInString, minInString);
                }
            }

            //            return string.Format("{0} Hari {1} Jam {2} Menit", dayInString, hourInString, minInString);
        }

        public static string DifferentDateTimeInTimeSplit(string value1, string value2)
        {
            DateTime date1 = DateTime.ParseExact(value1, "dd-MM-yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime date2 = DateTime.ParseExact(value2, "dd-MM-yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);
            TimeSpan duration = date2 - date1;
            double totalMin = duration.TotalMinutes;

            int day = Convert.ToInt32(Math.Floor(totalMin / 60)) / 24;
            int hour = Convert.ToInt32(Math.Floor((totalMin - (day * 24 * 60)) / 60));
            int min = Convert.ToInt32(totalMin - (day * 24 * 60) - (hour * 60));

            string dayInString = day.ToString();
            string hourInString = hour.ToString();
            string minInString = min.ToString();

            if (dayInString.Length == 1) dayInString = string.Format("0{0}", dayInString);
            if (hourInString.Length == 1) hourInString = string.Format("0{0}", hourInString);
            if (minInString.Length == 1) minInString = string.Format("0{0}", minInString);

            if (dayInString == "00")
            {
                if (hourInString == "00" && minInString != "00")
                {
                    return string.Format("{0}", minInString);
                }
                else if (hourInString != "00" && minInString == "00")
                {
                    return string.Format("{0}", hourInString);
                }
                else if (hourInString == "00")
                {
                    return string.Format("{0}", minInString);
                }
                else
                {
                    return string.Format("{0}|{1}", hourInString, minInString);
                }
            }
            else
            {
                if (hourInString == "00" && minInString != "00")
                {
                    return string.Format("{0}|{1}", dayInString, minInString);
                }
                else if (hourInString != "00" && minInString == "00")
                {
                    return string.Format("{0}|{1}", dayInString, hourInString);
                }
                else
                {
                    return string.Format("{0}|{1}|{2}", dayInString, hourInString, minInString);
                }
            }

            //            return string.Format("{0} Hari {1} Jam {2} Menit", dayInString, hourInString, minInString);
        }

        #region Calculate Patient Age Based on DateOfBirth
        public static int GetPatientAgeInDay(DateTime dateOfBirth, DateTime nowDate)
        {
            int day = GetPatientAge(dateOfBirth, nowDate, 1);
            return day;
        }
        public static int GetPatientAgeInMonth(DateTime dateOfBirth, DateTime nowDate)
        {
            int month = GetPatientAge(dateOfBirth, nowDate, 2);
            return month;
        }
        public static int GetPatientAgeInYear(DateTime dateOfBirth, DateTime nowDate)
        {
            int year = GetPatientAge(dateOfBirth, nowDate, 3);
            return year;
        }
        public static int GetPatientAge(DateTime dateOfBirth, DateTime nowDate, int type)
        {
            // From : Pak EAM

            int typo = 0;
            int year = 0;
            int month = 0;
            int day = 0;

            int yearFrom = dateOfBirth.Year;
            int monthFrom = dateOfBirth.Month;
            int dayFrom = dateOfBirth.Day;

            int yearTo = nowDate.Year;
            int monthTo = nowDate.Month;
            int dayTo = nowDate.Day;

            year = yearTo - yearFrom;
            month = monthTo - monthFrom;
            day = dayTo - dayFrom;

            if (day < 0)
            {
                if (monthFrom == 1 || monthFrom == 3 || monthFrom == 5 || monthFrom == 7 || monthFrom == 8 || monthFrom == 10 || monthFrom == 12)
                {
                    day = 31 - Math.Abs(day);
                }
                else if (monthFrom == 4 || monthFrom == 6 || monthFrom == 9 || monthFrom == 11)
                {
                    day = 30 - Math.Abs(day);
                }
                else if (monthFrom == 2)
                {
                    if (yearFrom % 4 == 0)
                    {
                        day = 29 - Math.Abs(day);
                    }
                    else
                    {
                        day = 28 - Math.Abs(day);
                    }
                }

                month = month - 1;
            }

            if (month < 0)
            {
                month = 12 - Math.Abs(month);
                year = year - 1;
            }

            switch (type)
            {
                case 1: typo = day; break;
                case 2: typo = month; break;
                case 3: typo = year; break;
            }
            return typo;


            // ORI

            //int day = nowDate.Day - dateOfBirth.Day;
            //int month = nowDate.Month - dateOfBirth.Month;
            //int year = nowDate.Year - dateOfBirth.Year;
            //int typo = 0;

            //if (day < 0)
            //{
            //    day = day + System.DateTime.FromOADate(nowDate.Day - System.DateTime.Now.Day).Day;
            //    month = month - 1;
            //}
            //if (month < 0)
            //{
            //    month = month + 12;
            //    year = year - 1;
            //}
            //switch (type)
            //{
            //    case 1: typo = day; break;
            //    case 2: typo = month; break;
            //    case 3: typo = year; break;
            //}
            //return typo;
        }
        #endregion


        #region Calculate Due Date Age
        public static int GetDueDateAgeInDay(DateTime dueDate, DateTime nowDate)
        {
            int day = GetDueDateAge(dueDate, nowDate, 1);
            return day;
        }
        public static int GetDueDateAgeInMonth(DateTime dueDate, DateTime nowDate)
        {
            int month = GetDueDateAge(dueDate, nowDate, 2);
            return month;
        }
        public static int GetDueDateAgeInYear(DateTime dueDate, DateTime nowDate)
        {
            int year = GetDueDateAge(dueDate, nowDate, 3);
            return year;
        }
        public static int GetDueDateAge(DateTime dueDate, DateTime nowDate, int type)
        {
            int day = 0;
            int month = 0;
            int year = 0;
            int typo = 0;

            if (dueDate < nowDate)
            {
                day = nowDate.Day - dueDate.Day;
                month = nowDate.Month - dueDate.Month;
                year = nowDate.Year - dueDate.Year;

                if (day < 0)
                {
                    day = day + System.DateTime.FromOADate(nowDate.Day - System.DateTime.Now.Day).Day;
                    month = month - 1;
                }
                if (month < 0)
                {
                    month = month + 12;
                    year = year - 1;
                }
            }
            switch (type)
            {
                case 1: typo = day; break;
                case 2: typo = month; break;
                case 3: typo = year; break;
            }
            return typo;
        }
        #endregion

        #region JSon

        public class JsonHelper
        {
            private const string INDENT_STRING = "    ";
            public static string FormatJson(string str)
            {
                var indent = 0;
                var quoted = false;
                var sb = new StringBuilder();
                for (var i = 0; i < str.Length; i++)
                {
                    var ch = str[i];
                    switch (ch)
                    {
                        case '{':
                        case '[':
                            sb.Append(ch);
                            if (!quoted)
                            {
                                sb.AppendLine();
                                Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
                            }
                            break;
                        case '}':
                        case ']':
                            if (!quoted)
                            {
                                sb.AppendLine();
                                Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
                            }
                            sb.Append(ch);
                            break;
                        case '"':
                            sb.Append(ch);
                            bool escaped = false;
                            var index = i;
                            while (index > 0 && str[--index] == '\\')
                                escaped = !escaped;
                            if (!escaped)
                                quoted = !quoted;
                            break;
                        case ',':
                            sb.Append(ch);
                            if (!quoted)
                            {
                                sb.AppendLine();
                                Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                            }
                            break;
                        case ':':
                            sb.Append(ch);
                            if (!quoted)
                                sb.Append(" ");
                            break;
                        default:
                            sb.Append(ch);
                            break;
                    }
                }
                return sb.ToString();
            }
        }

        #endregion
    }

    #region JSon Extensions

    static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var i in ie)
            {
                action(i);
            }
        }
    }
    #endregion
}
