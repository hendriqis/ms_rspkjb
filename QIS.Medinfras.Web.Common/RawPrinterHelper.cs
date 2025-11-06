using System;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace QIS.Medinfras.Web.Common
{
    public class RawPrinterHelper
    {
        private string printerName;

        private IntPtr hPrinter = new IntPtr(0);
        private DOCINFOA di = new DOCINFOA();
        private Boolean printerOpen = false;

        #region "Interop UnManage"

        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        #endregion

        #region "Public Properties"

        public Boolean PrinterIsOpen
        {
            get { return printerOpen; }
        }

        public string DocumentDatatype
        {
            get { return di.pDataType; }
        }

        public string DocumentName
        {
            get { return di.pDocName; }
        }

        public string DocumentOutputFile
        {
            get { return di.pOutputFile; }
        }

        public IntPtr PrinterHandler
        {
            get { return hPrinter; }
        }

        #endregion

        #region "Raw Printer"

        public Boolean OpenPrint(string szPrinterName)
        {
            if (printerOpen == false)
            {
                di.pDocName = ".NET RAW Document";
                di.pDataType = "RAW";
                if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
                {
                    printerName = szPrinterName;
                    //Start a document
                    if (StartDocPrinter(hPrinter, 1, di))
                    {
                        if (StartPagePrinter(hPrinter))
                            printerOpen = true;
                    }
                }
            }
            return printerOpen;
        }

        public void ClosePrint()
        {
            if (printerOpen)
            {
                EndPagePrinter(hPrinter);
                EndDocPrinter(hPrinter);
                ClosePrinter(hPrinter);
                printerOpen = false;
            }
        }

        public Boolean SendStringToPrinter(string sData)
        {
            if (printerOpen)
            {
                IntPtr pBytes;
                Int32 dwCount;
                Int32 dwWritten = 0;

                dwCount = sData.Length;
                pBytes = Marshal.StringToCoTaskMemAnsi(sData);
                WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                Marshal.FreeCoTaskMem(pBytes);
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}