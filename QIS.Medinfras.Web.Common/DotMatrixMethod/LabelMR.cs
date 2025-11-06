/*
 * FORMAT LABEL MEDICAL RECORD
 * Created By   : EAM
 * Created Date : 2018-06-07
 */
/// <summary>
/// Unit ini digunakan untuk mengenerate code format label status medical record di RS.Medistra
/// Format menggunakan bahasa EPL
/// <example>
/// <code>
///  LabelMR code = new LabelMR();
///  return code.GetLabelMRCode("99-99-89-22", "NAMA PASIEN ADALAH SAYA");
/// </code>
/// </example>
/// </summary>
/// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common
{
    class LabelMR
    {

        public string GetLabelMRCode(string norm, string nama)
        {
            StringBuilder txtprn = new StringBuilder();

            #region ...Setting kertas
            txtprn.Append("\n");
            txtprn.Append("Q80,24\n");
            txtprn.Append("q831\n");
            txtprn.Append("rN\n");
            txtprn.Append("S4\n");
            txtprn.Append("D7\n");
            txtprn.Append("ZT\n");
            txtprn.Append("OD\n");
            txtprn.Append("JB\n");
            txtprn.Append("R16,0\n");
            #endregion

            #region ...Cetak baris 1
            int nPanjang = nama.Length / 24;
            nPanjang = (nama.Length % 24) > 0 ? nPanjang + 1 : nPanjang;
            nama = nama.PadRight(nPanjang * 24);
            for (int i = 0; i < nPanjang; i++)
            {
                string s = nama.Substring(i * 24, 24);
                s = ConvertStr(s);
                txtprn.Append("N\n");
                txtprn.Append("A5,3,0,4,2,3,N,\"" + s + "\"\n");
                txtprn.Append("P1\n");
            }
            #endregion

            #region ...Cetak baris 2
            string sNoMed = norm;
            txtprn.Append("N\n");
            txtprn.Append("B468,10,0,1,2,6,50,N,\"" + sNoMed + "\"\n");    //barcode
            sNoMed = sNoMed.Replace("-", "");
            for (int i = 0; i < 4; i++)
            {
                int n = 440 - (i * 110);
                txtprn.Append("LO" + n.ToString() + ",0,5,60\n"); // garis
                txtprn.Append("A" + (n - 5).ToString() + ",15,1,5,1,1,N,\"" + sNoMed[(i * 2)].ToString() + "\"\n");   //no-med
                txtprn.Append("A" + (n - 55).ToString() + ",15,1,5,1,1,N,\"" + sNoMed[(i * 2) + 1].ToString() + "\"\n");   //no-med
            }
            txtprn.Append("P1\n");
            #endregion

            #region ...Baris terakhir
            txtprn.Append("JF\n");
            txtprn.Append("N\n");
            //txtprn.Append("B10,5,0,1,2,6,50,N,""00-30-72-24""" + vbLf)    'barcode
            //txtprn.Append("A425,0,0,4,3,3,N,\"" + nomed + "\"\n");
            txtprn.Append("A3,0,0,4,3,3,N,\"" + norm + "\"\n");

            txtprn.Append("P1\n");
            #endregion

            return txtprn.ToString();
        }

        private string ConvertStr(string s)
        {
            string s1 = string.Empty;
            string s2 = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                string c = s[i].ToString();
                s2 = string.Empty;
                switch (c)
                {
                    case "\"":
                        s2 = "\\\"";
                        break;
                    case "\'":
                        s2 = "\\\'";
                        break;
                    case "\\":
                        s2 = "\\\\";
                        break;
                    default:
                        s2 = c;
                        break;
                }
                s1 += s2;
            }
            return s1;
        }
    }
}