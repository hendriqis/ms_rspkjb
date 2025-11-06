using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Data;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for EDCService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class DesktopToolService : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Object CallServiceDesktopTool(int VisitID)
        {
            string IPAddress = GetLocalIPAddress();
 
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                IPAddress, Constant.DirectPrintType.DESKTOP_TOOL);
            PrinterLocation printer = BusinessLayer.GetPrinterLocationList(filterExp).FirstOrDefault();

            if (printer != null)
            {
                string[] printerInfo = printer.PrinterName.Split(':');
                string IPAddressEDC = printer.IPAddress; ///string.Format("{0}:{1}", printer.IPAddress, printerInfo[1]);
                string PortEDC = printerInfo[1];
                string errMessage = string.Empty;

                //int RequestID = InsertNewRequest(RegisterId, ref errMessage);
                vConsultVisit10 oVisit = BusinessLayer.GetvConsultVisit10List(string.Format("VisitID='{0}'", VisitID)).FirstOrDefault();
                //// MD401;01|001;OPR/202208170001;|12131;009-009-00;Daniel aditya andaru putra;1989-08-19| 2022-08-17 18:09;12121;daniel aditya; dr.sp.spog
                string ServiceCommand = string.Format("MD401;01|{0};{1};{2};{3}|;;;", oVisit.MRN,oVisit.MedicalNo,oVisit.PatientName, oVisit.DateOfBirth); // Header + VisitInfo + PatienInfo + TransactionDateFormat + ";" + 0 + ";" + TransactionAmount + PaymentCardInfo;
                    if (!String.IsNullOrEmpty(IPAddressEDC) && !String.IsNullOrEmpty(PortEDC))
                    {
                        TcpClient client = new TcpClient();
                        client.Connect(IPAddressEDC.ToString(), Convert.ToInt16(PortEDC.ToString()));

                        // Retrieve the network stream. 
                        NetworkStream stream = client.GetStream();
                        // Create a BinaryWriter for writing to the stream. 
                        using (BinaryWriter w = new BinaryWriter(stream, Encoding.UTF8))
                        { // Create a BinaryReader for reading from the stream. 
                            using (BinaryReader r = new BinaryReader(stream))
                            { // Start a dialogue. 
                                w.Write(string.Format("{0}", ServiceCommand).ToCharArray());
                            }
                        }


                        var result = new
                        {
                            Status = "1",
                            Remarks = "Silahkan dilanjutkan",
                             
                        };
                        var response = JsonConvert.SerializeObject(result);
                        return response;
                    }
                    else
                    {
                        var result = new
                        {

                            Status = "0",
                            Remarks = "Pasien Tidak ditemukan",
                            RequestID = ""
                        };
                        var response = JsonConvert.SerializeObject(result);
                        return response;
                    }
               
               
            }
            else {
                var result = new
                {

                    Status = "0",
                    Remarks = "Konfigurasi IP CAM tidak ditemukan",
                    RequestID = ""
                };
                var response = JsonConvert.SerializeObject(result);
                return response;
            }
            
        }
 

        #region method
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
        #endregion
    }
}
