/*
 * FORMAT CETAKAN AMPLOP RADIOLOGI
 * Created By   : EAM
 * Created Date : 2018-06-07
 */
/// <summary>
/// Unit ini digunakan untuk mengenerate code format cetakan amplop radiologi di RS.Medistra
/// Format menggunakan bahasa EPL
/// <example>
/// <code>
///  StikerHasil stk = new StikerHasil();
///  stk.nama = "NAMA PASIEN ADALAH SAYA";
///  stk.tgllahir = DateTime.Today;
///  stk.sex = "P";
///  stk.pengirim = "DOKTER YANG MENGIRIM";
///  stk.tglpemeriksaan = DateTime.Now;
///  stk.norm = "00-50-89-22";
///  stk.asalpasien = "MD";
///  // Maks 3 pemeriksaan
///  stk.Pemeriksaan.Add("CTA CARDIAC MSCT");
///  stk.Pemeriksaan.Add("PEMERIKSAAN LAIN");
///  // Kembalikan hasilnya jadi String
///  return (new AmplopRad()).GetAmplopCode(stk);
/// </code>
/// </example>
/// </summary>

using System;
using System.Collections;
using System.Text;

namespace QIS.Medinfras.Web.Common
{
    public class StikerHasil
    {
        public string nama { get; set; }
        public DateTime tgllahir { get; set; }
        public string sex { get; set; }
        public DateTime tglpemeriksaan { get; set; }
        public string norm { get; set; }
        public string pengirim { get; set; }
        public string asalpasien { get; set; }
        public int ukuran { get; set; }
        public ArrayList Pemeriksaan { get; set; }

        public StikerHasil()
        {
            Pemeriksaan = new ArrayList();
        }
    }

    public class AmplopRad
    {
        private System.Array GetLinesBasedOnSpace(string str, int iMaxChar)
        {
            iMaxChar++;
            ArrayList strret = new ArrayList();
            str = str.Trim();
            if (str.Length > 0)
            {
                do
                {
                    if (str.Length > iMaxChar)
                    {
                        bool isAdaSpasi = false;
                        for (int i = (iMaxChar - 1); i > 0; i--)
                        {
                            if (str[i] == ' ')
                            {
                                strret.Add(str.Substring(0, i));
                                str = str.Substring(i + 1).Trim();
                                isAdaSpasi = true;
                                break;
                            }
                        }
                        if (!isAdaSpasi)
                        {
                            // jika sampai kesini berarti karakternya panjang tanpa spasi, paksa saja potong
                            strret.Add(str.Substring(0, iMaxChar - 1));
                            str = str.Substring(iMaxChar - 1).Trim();
                        }
                    }
                    else
                    {
                        strret.Add(str);
                        str = string.Empty;
                    }
                } while (str.Length > 0);
            }
            return strret.ToArray("".GetType());
        }

        private string CutText(string str, int iMaxLen)
        {
            str = str.Trim();
            if (str.Length <= iMaxLen) return str;
            return str.Substring(0, iMaxLen - 1);
        }

        public String GetAmplopCode(StikerHasil stk)
        {
            string tgllhr = String.Format("{0:d/M/yyyy}", stk.tgllahir);
            string tglperiksa = String.Format("{0:d/M/yyyy}", stk.tglpemeriksaan);

            StringBuilder sPrint = new StringBuilder();
            sPrint.Append("N").AppendLine();
            sPrint.Append("q784").AppendLine();
            sPrint.Append("Q1215,24").AppendLine();
            sPrint.Append("oH1,500").AppendLine();
            sPrint.AppendLine();

            sPrint.Append("N").AppendLine();

            int iLeft = 40;         // batas kiri
            int iMaxChar = 22;      // maksimal panjang karakter
            int iMaxCharSS = 26;    // maksimal panjang karakter super small
            int iTop = 30;          // batas atas baris pertama
            int iNextL = 80;        // jarak dari batas atas pertama dengan batas atas selanjutnya antar sesama font gede
            int iNextS = 45;        // jarak dari batas atas pertama dengan batas atas selanjutnya antar sesama font kecil
            int iNextSS = 40;       // jarak dari batas atas pertama dengan batas atas selanjutnya antar sesama font super kecil
            int iNextSL = 55;       // jarak dari batas atas pertama dengan batas atas selanjutnya antar font kecil ke font gede :P

            // if length of name is more than 22 chars then split 2 lines
            Array nm = GetLinesBasedOnSpace(stk.nama, iMaxChar);
            int iLen = nm.Length;
            if (iLen == 1)
            {
                // single line
                sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,3,N,\"").Append(nm.GetValue(0).ToString()).Append("\"").AppendLine();
                iTop += iNextL;
            }
            else
            {
                // 2 lines
                sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,2,N,\"").Append(nm.GetValue(0).ToString()).Append("\"").AppendLine();
                iTop += iNextS;
                sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,2,N,\"").Append(nm.GetValue(1).ToString()).Append("\"").AppendLine();
                iTop += iNextSL;
            }

            sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,3,N,\"").Append(tgllhr + " / " + stk.sex).Append("\"").AppendLine();
            iTop += iNextL;

            Array nmdr = GetLinesBasedOnSpace(stk.pengirim, iMaxChar);
            iLen = nmdr.Length;
            if (iLen <= 1)
            {
                // single line
                string nama = ""; if (iLen == 1) nama = nmdr.GetValue(0).ToString();
                sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,3,N,\"").Append(nama).Append("\"").AppendLine();
                iTop += iNextL;
            }
            else
            {
                // 2 lines
                sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,2,N,\"").Append(nmdr.GetValue(0).ToString()).Append("\"").AppendLine();
                iTop += iNextS;
                sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,2,N,\"").Append(nmdr.GetValue(1).ToString()).Append("\"").AppendLine();
                iTop += iNextSL;
            }

            sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,3,N,\"").Append(tglperiksa).Append("\"").AppendLine();
            iTop += iNextL;
            sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,3,N,\"").Append(stk.norm + " / " + stk.asalpasien).Append("\"").AppendLine();
            iTop += iNextL;

            iLen = stk.Pemeriksaan.Count;
            switch (iLen)
            {
                case 1:
                    Array exam = GetLinesBasedOnSpace(stk.Pemeriksaan[0].ToString(), iMaxChar);
                    iLen = exam.Length;
                    if (iLen == 1)
                    {
                        sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,3,N,\"").Append(exam.GetValue(0).ToString()).Append("\"").AppendLine();
                        iTop += iNextL;
                    }
                    else
                    {
                        sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,2,N,\"").Append(exam.GetValue(0).ToString()).Append("\"").AppendLine();
                        iTop += iNextS;
                        sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,2,N,\"").Append(exam.GetValue(1).ToString()).Append("\"").AppendLine();
                        iTop += iNextSL;
                    }
                    break;
                case 2:
                    sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,2,N,\"").Append(CutText(stk.Pemeriksaan[0].ToString(), iMaxChar)).Append("\"").AppendLine();
                    iTop += iNextS;
                    sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,2,N,\"").Append(CutText(stk.Pemeriksaan[1].ToString(), iMaxChar)).Append("\"").AppendLine();
                    iTop += iNextSL;
                    break;
                case 3:
                default:
                    sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,2,N,\"").Append(CutText(stk.Pemeriksaan[0].ToString(), iMaxChar)).Append("\"").AppendLine();
                    iTop += iNextS;
                    sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,2,N,\"").Append(CutText(stk.Pemeriksaan[1].ToString(), iMaxChar)).Append("\"").AppendLine();
                    iTop += iNextS;
                    sPrint.Append("A" + iLeft.ToString() + "," + iTop + ",0,4,2,2,N,\"").Append(CutText(stk.Pemeriksaan[2].ToString(), iMaxChar)).Append("\"").AppendLine();
                    iTop += iNextSL;
                    break;
            }

            sPrint.Append("P1").AppendLine();
            return sPrint.ToString();
        }
    }
}
