using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace QIS.Medinfras.Web.Common
{
    /// <summary>
    /// Kelas ini digunakan untuk mempermudah Anda menuliskan perintah-perintah format pencetakan,
    /// khususnya untuk printer Epson.
    /// Perintah-perintah pada kelas ini mengacu pada printer LX-800.
    /// </summary>
    public class DotMatrixPrinter
    {
        /// <summary>
        /// 
        /// </summary>
        private ArrayList CommandList;
        private StringBuilder sBuffer;

        /// <summary>
        /// Constructor
        /// </summary>
        public DotMatrixPrinter()
        {
            CommandList = new ArrayList();
            sBuffer = new StringBuilder();
        }

        /// <summary>
        /// Inisialisasi Printer
        /// </summary>           
        public void InitPrinter()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)64);
        }


        /// <summary>
        /// Mengatur panjang halaman menjadi n baris.
        /// </summary>
        /// <param name="Value">jumlah baris dalam satu halaman. (n=1..127)</param>
        /// <example>
        /// Berikut ini contoh penggunaan method ini:
        /// <code>
        /// EpsonPrnCode prn = new EpsonPrnCode();
        /// 
        /// prn.InitPrinter();              //Inisialisasi Printer
        /// prn.SetPageLengthInLines(33);   //Panjang halaman 33 baris
        /// </code>
        /// </example>
        public void SetPageLengthInLines(int Value)
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)67);
            sBuffer.Append((char)Value);
        }

        /// <summary>
        /// Mengatur panjang halaman menjadi n inchi.
        /// </summary>
        /// <param name="Value">panjang satu halaman dalam inchi. (n=1..22)</param>
        /// <example>
        /// Berikut ini contoh penggunaan method ini:
        /// <code>
        /// EpsonPrnCode prn = new EpsonPrnCode();
        /// 
        /// prn.InitPrinter();              //Inisialisasi Printer
        /// prn.SetPageLengthInInches(4);   //Panjang halaman 8 inchi
        /// </code>
        /// </example>
        public void SetPageLengthInInches(int Value)
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)67);
            sBuffer.Append((char)48);
            sBuffer.Append((char)Value);
        }

        /// <summary>
        /// Mengatur margin kanan
        /// </summary>
        /// <param name="Value">jumlah kolom</param>
        public void SetRightMargin(int Value)
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)81);
            sBuffer.Append((char)Value);
        }

        /// <summary>
        /// Reset Margin Kanan
        /// </summary>
        public void ResetRightMargin()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)81);
            sBuffer.Append((char)255);
        }

        /// <summary>
        /// Mengatur margin kiri
        /// </summary>
        /// <param name="Value">jumlah kolom</param>
        public void SetLeftMargin(int Value)
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)108);
            sBuffer.Append((char)Value);
        }

        /// <summary>
        /// Reset Margin Kiri
        /// </summary>
        public void ResetLeftMargin()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)108);
            sBuffer.Append((char)1);
        }

        /// <summary>
        /// Set Margin Bawah
        /// </summary>
        /// <param name="Value">Nilai Margin</param>
        public void SetBottomMargin(int Value)
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)78);
            sBuffer.Append((char)Value);
        }

        /// <summary>
        /// Reset Margin Bawah
        /// </summary>
        public void ResetBottomMargin()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)79);
        }

        /// <summary>
        /// Reset semua margin ke posisi default
        /// </summary>
        public void ResetMargin()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)108);
            sBuffer.Append((char)1);
            sBuffer.Append((char)27);
            sBuffer.Append((char)81);
            sBuffer.Append((char)255);
            sBuffer.Append((char)27);
            sBuffer.Append((char)79);
        }

        /// <summary>
        /// Memindahkan posisi printer ke halaman baru (gulung kertas).
        /// </summary>
        public void FormFeed()
        {
            sBuffer.Append((char)12);
        }

        /// <summary>
        /// Mengatur font menjadi Times New Roman NLQ
        /// </summary>
        public void SelectRomanFont()
        {
            sBuffer.Append((char)27);
            sBuffer.Append('k');
            sBuffer.Append((char)0);
        }

        /// <summary>
        /// Mengatur font menjadi Sans Serif NLQ
        /// </summary>
        public void SelectSansSerifFont()
        {
            sBuffer.Append((char)27);
            sBuffer.Append('k');
            sBuffer.Append((char)1);
        }

        /// <summary>
        /// Mengatur font menjadi Courier
        /// </summary>
        public void SelectCourierFont()
        {
            sBuffer.Append((char)27);
            sBuffer.Append('k');
            sBuffer.Append((char)2);
        }
        /// <summary>
        /// Mengatur font menjadi Prestige
        /// </summary>
        public void SelectPrestigeFont()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)107);
            sBuffer.Append((char)3);
        }
        /// <summary>
        /// Mengatur font menjadi Script
        /// </summary>
        public void SelectScriptFont()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)107);
            sBuffer.Append((char)4);
        }

        /// <summary>
        /// Mengatur font menjadi Draft
        /// </summary>
        public void SelectDraftMode()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)120);
            sBuffer.Append((char)0);
        }

        /// <summary>
        /// Beralih ke mode NLQ
        /// </summary>
        public void SelectNLQMode()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)107);
            sBuffer.Append((char)1);
        }

        /// <summary>
        /// Mengaktifkan mode pencetakan condensed
        /// </summary>
        public void SelectCondensedMode()
        {
            sBuffer.Append((char)15);
        }

        /// <summary>
        /// Membatalkan mode pencetakan Condensed
        /// </summary>
        public void CancelCondensedMode()
        {
            sBuffer.Append((char)18);
        }

        /// <summary>
        /// Mengaktifkan mode pencetakan lebar huruf menjadi double
        /// </summary>
        public void SelectDoubleWidthMode()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)87);
            sBuffer.Append((char)49);
        }

        /// <summary>
        /// Membatalkan mode pencetakan lebar huruf
        /// </summary>
        public void CancelDoubleWidthMode()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)87);
            sBuffer.Append((char)48);
        }

        /// <summary>
        /// Mengaktifkan mode pencetakan garis bawah
        /// </summary>
        public void SelectUnderlineMode()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)45);
            sBuffer.Append((char)49);
        }

        /// <summary>
        /// Membatalkan mode pencetakan garis bawah
        /// </summary>
        public void CancelUnderlineMode()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)45);
            sBuffer.Append((char)48);
        }

        /// <summary>
        /// Mengaktifkan mode pencetakan Emphasized
        /// </summary>
        public void SelectEmphasizedMode()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)69);
        }

        /// <summary>
        /// Membatalkan mode pencetakan Emphasized
        /// </summary>
        public void CancelEmphasizedMode()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)70);
        }

        /// <summary>
        /// Mengaktifkan pencetakan Double Strike
        /// </summary>
        public void SelectDoubleStrikeMode()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)71);
        }

        /// <summary>
        /// Membatalkan mode pencetakan Double Strike
        /// </summary>
        public void CancelDoubleStrikeMode()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)72);
        }

        /// <summary>
        /// Mengaktifkan mode pencetakan Superscript (hufuf ke atas)
        /// </summary>
        public void SelectSuperscriptMode()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)83);
            sBuffer.Append((char)48);
        }

        /// <summary>
        /// Mengaktifkan mode pencetakan Subscript (huruf di bawah)
        /// </summary>
        public void SelectSubscriptMode()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)83);
            sBuffer.Append((char)49);
        }

        /// <summary>
        /// Membatalkan mode pencetakan huruf di atas / di bawah
        /// </summary>
        public void CancelSuperSubScriptMode()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)84);
        }

        /// <summary>
        /// Mengaktifkan mode pencetakan miring
        /// </summary>
        public void SelectItalicMode()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)52);
        }

        /// <summary>
        /// Membatalkan mode pencetakan miring
        /// </summary>
        public void CancelItalicMode()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)53);
        }

        /// <summary>
        /// Mengatifkan untuk dapat mencetak karakter ASCII (128-159,255)
        /// </summary>
        public void EnablePrintExtraChar()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)54);
        }

        /// <summary>
        /// Membatalkan mode pencetakan karakter ASCII (128-159,255)
        /// </summary>
        public void DisablePrintExtraChar()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)55);
        }

        /// <summary>
        /// Set font 8cpi
        /// </summary>
        public void SetFont8cpi()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)83);
        }

        /// <summary>
        /// Set font 10cpi
        /// </summary>
        public void SetFont10cpi()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)80);
        }

        /// <summary>
        /// Set font 12cpi
        /// </summary>
        public void SetFont12cpi()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)77);
        }

        /// <summary>
        /// Set font 15cpi
        /// </summary>
        public void SetFont15cpi()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)103);
        }

        /// <summary>
        /// SetFontByPitchAndPoint
        /// </summary>
        public void SetFontByPitchAndPoint(int m, int l, int h)
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)88);
            sBuffer.Append((char)m);
            sBuffer.Append((char)l);
            sBuffer.Append((char)h);
        }

        /// <summary>
        /// Set type face
        /// </summary>
        public void SetTypeFace(int value)
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)107);
            sBuffer.Append((char)value);
        }

        /// <summary>
        /// Set tab horizontally
        /// </summary>
        public void SetTabHorizontally()
        {
            sBuffer.Append((char)9);
        }

        /// <summary>
        /// Mengosongkan buffer string
        /// </summary>
        public void Empty()
        {
            sBuffer.Length = 0;
        }

        /// <summary>
        /// Menambahkan kalimat ke dalam buffer
        /// </summary>
        /// <param name="s"></param>
        public void Append(string s)
        {
            sBuffer.Append(s);
        }

        /// <summary>
        /// Mengkonversi buffer menjadi string
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            return sBuffer.ToString();
        }

        public void AppendCenterAlign(string s, int maxLength)
        {
            sBuffer.Append(s.PadLeft(maxLength, ' '));
        }

        public void SetDoubleSizeFont()
        {
            sBuffer.Append((char)27);
            sBuffer.Append((char)119);
            sBuffer.Append((char)49);        
        }

        public void PrintHealthcareInfo()
        {
            SetFont15cpi();
            SelectUnderlineMode();
            sBuffer.Append(AppSession.UserLogin.HealthcareName);
            sBuffer.Append("Address");
            CancelUnderlineMode();
        }

        public void SetTittleFont(string z) {
            sBuffer.Append((char)27);
            sBuffer.Append((char)87);
            sBuffer.Append((char)49);
            sBuffer.Append(z);
            sBuffer.Append((char)27);
            sBuffer.Append((char)87);
            sBuffer.Append((char)48);
            sBuffer.Append((char)15);
        }

        public void PrintReportTitle1(string text)
        {
            ///TODO : (HI) Place your format code here
            sBuffer.Append(text);
        }        
    }
}
