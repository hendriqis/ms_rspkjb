using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Web;
using QIS.Medinfras.Data.Service;
using System.Globalization;
using System.IO;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class Helper
    {
        public static String NumberInWords(Int64 amount, Boolean isMoney = false)
        {
            StringBuilder strbuild;
            if (isMoney)
                strbuild = new StringBuilder("RUPIAH");
            else
                strbuild = new StringBuilder();

            String[] arrBil = { "", "SATU ", "DUA ", "TIGA ", "EMPAT ", "LIMA ", "ENAM ", "TUJUH ", "DELAPAN ", "SEMBILAN ", "SE" };
            String[] arrSatKecil = { "", "PULUH ", "RATUS " };
            String[] arrSatBesar = { "", "RIBU ", "JUTA ", "MILYAR " };
            int ctrKecil = 0;
            int ctrBesar = 0;
            if (amount == 0)
            {
                if (isMoney)
                    return "NOL RUPIAH";
                else
                    return "NOL";
            }
            else
            {
                while (amount > 0)
                {
                    long a = amount % 10;
                    amount /= 10;

                    if (a > 0)
                        strbuild.Insert(0, arrSatKecil[ctrKecil]);

                    if (a == 1 && ctrKecil > 0)
                        strbuild.Insert(0, arrBil[10]);
                    else if (ctrKecil == 0 && amount % 10 == 1 && a > 0)
                    {
                        strbuild.Insert(0, "BELAS ");
                        if (a == 1)
                            a = 10;
                        strbuild.Insert(0, arrBil[a]);
                        amount /= 10;
                        ctrKecil++;
                    }
                    else
                        strbuild.Insert(0, arrBil[a]);

                    ctrKecil++;
                    if (ctrKecil % 3 == 0)
                    {
                        ctrBesar++;
                        ctrKecil = 0;
                        if (amount > 0 && amount % 1000 > 0)
                        {
                            strbuild.Insert(0, arrSatBesar[ctrBesar]);
                        }
                    }

                }
                return strbuild.ToString();
            }
        }

        public static XDocument LoadXMLFile(string xmlFileName)
        {
            string physicalPath = string.Format(@"{0}\App_Data\{1}", AppConfigManager.QISLibsPhysicalDirectory, xmlFileName);
            if (File.Exists(physicalPath))
            {
                XDocument xdoc = XDocument.Load(physicalPath);
                return xdoc;
            }
            return null;
        }

        public static string[] LoadTextFile(string textFileName)
        {
            string physicalPath = string.Format(@"{0}\App_Data\{1}", AppConfigManager.QISLibsPhysicalDirectory, textFileName);
            if (File.Exists(physicalPath))
                return System.IO.File.ReadAllLines(physicalPath, Encoding.GetEncoding("windows-1250"));
            else
            {
                string[] str = { };
                return str;
            }
        }

        public static String GetTextFormatText(string code)
        {
            XDocument xdoc = LoadXMLFile("config.xml");
            var config = (from pg in xdoc.Descendants("page")
                          select new
                          {
                              Lang = pg.Attribute("lang").Value
                          }).FirstOrDefault();

            string[] tempWords = Helper.LoadTextFile(string.Format("text_format/{0}.txt", config.Lang));
            foreach (string word in tempWords)
            {
                string[] param = word.Split(';');
                if (param[0] == code)
                    return param[1];
            }
            return "";
        }

        #region Language
        public static string GetWordsLabel(List<Words> words, string code)
        {
            if (words == null)
                return code;
            Words word = words.FirstOrDefault(w => w.Code == code);
            return word == null ? code : word.Text;
        }

        public static List<Words> LoadWords()
        {
            XDocument xdoc = LoadXMLFile("config.xml");
            var config = (from pg in xdoc.Descendants("page")
                          select new
                          {
                              Lang = pg.Attribute("lang").Value
                          }).FirstOrDefault();

            List<Words> words = new List<Words>();
            string[] tempWords = Helper.LoadTextFile(string.Format("lang/{0}.txt", config.Lang));
            foreach (string word in tempWords)
            {
                string[] param = word.Split(';');
                words.Add(new Words { Code = param[0], Text = param[1] });
            }
            return words;
        }
        #endregion

        public static String GetPatientAge(List<Words> words, DateTime DoB)
        {
            int ageInYear = Function.GetPatientAgeInYear(DoB, DateTime.Now);
            int ageInMonth = Function.GetPatientAgeInMonth(DoB, DateTime.Now);
            int ageInDay = Function.GetPatientAgeInDay(DoB, DateTime.Now);

            return string.Format("{0} {3}  {1} {4}  {2} {5}", ageInYear, ageInMonth, ageInDay, GetWordsLabel(words, "yr"), GetWordsLabel(words, "mo"), GetWordsLabel(words, "day"));
        }

        public static DateTime YYYYMMDDToDate(String text)
        {
            if (text != "")
            {
                var culture = System.Globalization.CultureInfo.CurrentCulture;
                return DateTime.ParseExact(text, "yyyyMMdd", culture);
            }
            return new DateTime(1900, 1, 1);
        }
    }
}
