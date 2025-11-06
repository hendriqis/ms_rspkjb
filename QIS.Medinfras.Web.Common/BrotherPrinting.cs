using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Medinfras.Data.Service;
using System.Web;
using System.IO;

namespace QIS.Medinfras.Web.Common
{
    public static class BrotherPrinting
    {
        #region Helper
        public enum PrintingGroup
        {
            Cover,
            DrugLabel,
            Registration,
            Diagnostic,
            Imaging,
            Laboratory,
            Order,
            PatientLabel,
            ProductionLabel,
            Tracer,
            Wristband,
            UDD,
            NutritionLabel,
            AssetLabel
        }

        private static List<string> GetMetadata(PrintingGroup printingGroup, string printFormat)
        {
            List<string> metadata = new List<string>();
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");
            string fileName = string.Format(@"{0}\print_format\{1}\brother\{2}.rdl", filePath, printingGroup.ToString().ToLower(), printFormat.Replace('^', '_'));
            IEnumerable<string> lstField = File.ReadAllLines(fileName);
            metadata = lstField.ToList();
            return metadata;
        }

        private static string GetPrintFormat(PrintingGroup printGroup, ref string pathTemplate, string printFormat = "X215^01")
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            //supaya jika printFormat kosong tidak error
            if (String.IsNullOrEmpty(printFormat))
            {
                printFormat = "X225^01";
            }

            string fileName = string.Format(@"{0}\print_format\{1}\brother\{2}.rdl", filePath, printGroup.ToString().ToLower(), printFormat.Replace('^', '_'));
            pathTemplate = string.Format(@"{0}\print_format\{1}\brother\{2}.lbx", filePath, printGroup.ToString().ToLower(), printFormat.Replace('^', '_'));
            IEnumerable<string> lstCommand = File.ReadAllLines(fileName);
            StringBuilder commandText = new StringBuilder();
            foreach (string command in lstCommand)
            {
                commandText.AppendLine(command);
            }
            string result = commandText.ToString();

            return result;
        }

        public static object GetPropertyValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public static void SendCommandToBrotherPrinter(List<string> lstField, string pathTemplate, string printerUrl, string command, int printCount, Dictionary<string, string> labelInfo, bool isUsingDirectPrintingTools = false, PrintingGroup printingGroup = PrintingGroup.PatientLabel, string printFormat = "X215^01")
        {
            if (!isUsingDirectPrintingTools)
            {
                #region Send Command to Printer
                bpac.Document doc = new bpac.Document();
                doc.Open(pathTemplate);
                bool processPrint = doc.SetPrinter(printerUrl, true);
                if (processPrint)
                {
                    #region Replace Command's
                    string labelCommand = command;
                    foreach (string fieldName in lstField)
                    {
                        string[] paramField = fieldName.Split('=');

                        doc.GetObject(paramField[0]).Text = labelInfo[paramField[1]];
                    }
                    #endregion

                    doc.StartPrint("", bpac.PrintOptionConstants.bpoDefault);
                    doc.PrintOut(printCount, bpac.PrintOptionConstants.bpoDefault);
                    doc.EndPrint();
                    doc.Close();
                }
                #endregion
            }
            else
            {
                string labelInfoString = string.Join(",", labelInfo.Select(kv => string.Format("{0}:{1}", kv.Key, kv.Value)));
                string commandText = string.Format("{0}|{1}|{2}|{3}", printingGroup.ToString().ToLower(), printFormat, printCount.ToString(), labelInfoString);
                Helper.MedinfrasDirectPrintingTools(printerUrl, commandText);
            }
        }
        #endregion

        #region RSSY

        #region Asset
        public static void PrintLabelAsset_RSSY(string printFormat, int id = 0, string printerUrl = "", string lstExpiredDate = "")
        {
            try
            {
                string printerName = printerUrl;
                List<vFAItem> lstEntity = BusinessLayer.GetvFAItemList(String.Format("FixedAssetID = {0} AND IsDeleted = 0", id.ToString()));

                if (lstEntity.Count > 0)
                {
                    string command = string.Empty;
                    string pathTemplate = string.Empty;
                    command = GetPrintFormat(PrintingGroup.AssetLabel, ref pathTemplate, printFormat);

                    List<string> lstField = GetMetadata(PrintingGroup.AssetLabel, printFormat);
                    if (!string.IsNullOrEmpty(command))
                    {
                        foreach (vFAItem item in lstEntity)
                        {
                            #region Store Registration Information to Dictionary
                            var labelInfo = new Dictionary<string, string>();
                            labelInfo.Add("HealthcareName", AppSession.UserLogin.HealthcareName);
                            foreach (string fieldName in lstField)
                            {
                                string[] paramField = fieldName.Split('=');

                                if (paramField[1] != "HealthcareName")
                                    labelInfo.Add(paramField[1], GetPropertyValue(item, paramField[1]).ToString());
                            }
                            #endregion

                            SendCommandToBrotherPrinter(lstField, pathTemplate, printerUrl, command, 1, labelInfo);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
            }
        }
        #endregion

        #region Farmasi
        public static void PrintDrugLabel_RSSY(PrescriptionOrderHd oHeader, string printFormat, string lstPrescriptionOrderDtID = "", string printerUrl1 = "", string printerUrl2 = "", Boolean isPrintByType = false, string isUsedDispenseQty = "", bool isPrintCompoundDetailLabel = false, int labelCount = 0, string lstExpiredDate = "", string lstPrintNo = "")
        {
            try
            {
                string printerName1 = printerUrl1;
                string printerName2 = printerUrl2;

                if (!string.IsNullOrEmpty(printerName1))
                {
                    List<vPrintPrescriptionEtiket> lstEntity = BusinessLayer.GetvPrintPrescriptionEtiketList(String.Format(
                                                                                "VisitID = {0} AND PrescriptionOrderID = {1} AND PrescriptionOrderDetailID IN ({2})",
                                                                                oHeader.VisitID.ToString(),
                                                                                oHeader.PrescriptionOrderID,
                                                                                lstPrescriptionOrderDtID
                                                                            ));
                    if (lstEntity != null)
                    {
                        string command = string.Empty;
                        string pathTemplate = string.Empty;
                        command = GetPrintFormat(PrintingGroup.DrugLabel, ref pathTemplate, printFormat);

                        List<string> lstField = GetMetadata(PrintingGroup.DrugLabel, printFormat);
                        if (!string.IsNullOrEmpty(command))
                        {
                            foreach (vPrintPrescriptionEtiket item in lstEntity)
                            {
                                #region Store Registration Information to Dictionary

                                var labelInfo = new Dictionary<string, string>();

                                vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                                labelInfo.Add("HealthcareName", h.HealthcareName);
                                labelInfo.Add("HealthcarePhoneFax", string.Format("Telp.{0} | Fax.{1}", h.PhoneNo1, h.FaxNo1));

                                foreach (string fieldName in lstField)
                                {
                                    string[] paramField = fieldName.Split('=');

                                    if (paramField[1] != "HealthcareName" && paramField[1] != "HealthcarePhoneFax")
                                    {
                                        labelInfo.Add(paramField[1], GetPropertyValue(item, paramField[1]).ToString());
                                    }

                                }

                                #endregion

                                SendCommandToBrotherPrinter(lstField, pathTemplate, printerName1, command, labelCount, labelInfo);
                            }
                        }
                        else
                        {
                            throw new Exception("Cannot find Patient Visit Information!");
                        }
                    }
                    else
                    {
                        throw new Exception("Please setup the printer address in System Parameter!");
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
            }

        }

        public static void PrintDrugLabelUDD_RSSY(string printFormat, string lstPrescriptionOrderDtID = "", string printerUrl1 = "", string printerUrl2 = "", Boolean isPrintByType = false, string isUsedDispenseQty = "", bool isPrintCompoundDetailLabel = false, string lstExpiredDate = "", string lstPrintNo = "")
        {
            try
            {
                string printerName1 = printerUrl1;
                string printerName2 = printerUrl2;

                if (!string.IsNullOrEmpty(printerName1))
                {
                    List<vPrintPrescriptionEtiket> lstEntity = BusinessLayer.GetvPrintPrescriptionEtiketList(String.Format(
                                                                                "PrescriptionOrderDetailID IN ({0})",
                                                                                lstPrescriptionOrderDtID
                                                                            ));
                    if (lstEntity != null)
                    {
                        string command = string.Empty;
                        string pathTemplate = string.Empty;
                        command = GetPrintFormat(PrintingGroup.UDD, ref pathTemplate, printFormat);

                        List<string> lstField = GetMetadata(PrintingGroup.UDD, printFormat);
                        if (!string.IsNullOrEmpty(command))
                        {
                            string[] lstPrescriptionOrderDtIDParam = lstPrescriptionOrderDtID.Split(',');
                            string[] lstPrintNoParam = lstPrintNo.Split(',');

                            for (int i = 0; i < lstPrescriptionOrderDtIDParam.Count(); i++)
                            {
                                vPrintPrescriptionEtiket item = lstEntity.FirstOrDefault(a => Convert.ToInt32(a.PrescriptionOrderDetailID) == Convert.ToInt32(lstPrescriptionOrderDtIDParam[i]));
                                int labelCount = Convert.ToInt32(lstPrintNoParam[i]);
                                if (item != null)
                                {
                                    #region Store Registration Information to Dictionary

                                    var labelInfo = new Dictionary<string, string>();

                                    vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                                    labelInfo.Add("HealthcareName", h.HealthcareName);
                                    labelInfo.Add("HealthcarePhoneFax", string.Format("Telp.{0} | Fax.{1}", h.PhoneNo1, h.FaxNo1));

                                    foreach (string fieldName in lstField)
                                    {
                                        string[] paramField = fieldName.Split('=');

                                        if (paramField[1] != "HealthcareName" && paramField[1] != "HealthcarePhoneFax")
                                        {
                                            labelInfo.Add(paramField[1], GetPropertyValue(item, paramField[1]).ToString());
                                        }

                                    }

                                    #endregion

                                    SendCommandToBrotherPrinter(lstField, pathTemplate, printerName1, command, labelCount, labelInfo);
                                }
                                else
                                {
                                    throw new Exception("No Data for Printing");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Cannot find Patient Visit Information!");
                        }
                    }
                    else
                    {
                        throw new Exception("Please setup the printer address in System Parameter!");
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
            }

        }
        #endregion

        #region Gizi
        public static void PrintLabelDistribusiGizi_RSSY(String lstNutritionOrderDtID = "", string printFormat = "", string printerUrl = "", int labelCount = 0, string lstExpiredDate = "")
        {
            try
            {
                vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];

                List<vNutritionOrderDtCustom2> lstEntity = BusinessLayer.GetvNutritionOrderDtCustom2List(String.Format("NutritionOrderDtID IN ({0}) AND StapleFood IS NOT NULL ORDER BY BedCode", lstNutritionOrderDtID));

                if (lstEntity.Count > 0)
                {
                    string command = string.Empty;
                    string pathTemplate = string.Empty;
                    command = GetPrintFormat(PrintingGroup.NutritionLabel, ref pathTemplate, printFormat);

                    List<string> lstField = GetMetadata(PrintingGroup.NutritionLabel, printFormat);
                    if (!string.IsNullOrEmpty(command))
                    {
                        foreach (vNutritionOrderDtCustom2 item in lstEntity)
                        {
                            #region Store Registration Information to Dictionary
                            var labelInfo = new Dictionary<string, string>();

                            labelInfo.Add("HealthcareName", entityHealthcare.HealthcareName);
                            labelInfo.Add("HealthcarePhoneFax", string.Format("Telp.{0} | Fax.{1}", entityHealthcare.PhoneNo1, entityHealthcare.FaxNo1));

                            foreach (string fieldName in lstField)
                            {
                                string[] paramField = fieldName.Split('=');

                                if (paramField[1] != "HealthcareName" && paramField[1] != "HealthcarePhoneFax")
                                {
                                    labelInfo.Add(paramField[1], GetPropertyValue(item, paramField[1]).ToString());
                                }
                            }
                            #endregion

                            SendCommandToBrotherPrinter(lstField, pathTemplate, printerUrl, command, 1, labelInfo);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
            }
        }
        #endregion

        #region Radiologi
        public static void PrintLabelCoverHasilRadiologi_RSSY(vPatientChargesHd5 oData, string printerUrl, string printFormat, int printCount = 1)
        {
            try
            {
                //Get Printing Metadata
                List<string> lstField = GetMetadata(PrintingGroup.Imaging, printFormat);

                //Get and Load Printer Command
                string command = string.Empty;
                string pathTemplate = string.Empty;
                command = GetPrintFormat(PrintingGroup.Imaging, ref pathTemplate, printFormat);

                if (!string.IsNullOrEmpty(command))
                {
                    #region Store Registration Information to Dictionary
                    var labelInfo = new Dictionary<string, string>();
                    labelInfo.Add("HealthcareName", AppSession.UserLogin.HealthcareName);
                    foreach (string fieldName in lstField)
                    {
                        string[] paramField = fieldName.Split('=');

                        if (paramField[1] != "HealthcareName")
                            labelInfo.Add(paramField[1], GetPropertyValue(oData, paramField[1]).ToString());
                    }
                    #endregion

                    SendCommandToBrotherPrinter(lstField, pathTemplate, printerUrl, command, printCount, labelInfo);
                }
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
            }
        }
        #endregion

        #region Rekam Medis
        public static void PrintLabelRM_RSSY(vLabelPatientRegistrationInfo oData, string printerUrl, string printFormat, int printCount = 1)
        {
            try
            {
                //Get Printing Metadata
                List<string> lstField = GetMetadata(PrintingGroup.PatientLabel, printFormat);

                //Get and Load Printer Command
                string command = string.Empty;
                string pathTemplate = string.Empty;
                command = GetPrintFormat(PrintingGroup.PatientLabel, ref pathTemplate, printFormat);

                if (!string.IsNullOrEmpty(command))
                {
                    #region Store Registration Information to Dictionary
                    var labelInfo = new Dictionary<string, string>();
                    labelInfo.Add("HealthcareName", AppSession.UserLogin.HealthcareName);
                    foreach (string fieldName in lstField)
                    {
                        string[] paramField = fieldName.Split('=');

                        if (paramField[1] != "HealthcareName")
                            labelInfo.Add(paramField[1], GetPropertyValue(oData, paramField[1]).ToString());
                    }
                    #endregion

                    SendCommandToBrotherPrinter(lstField, pathTemplate, printerUrl, command, printCount, labelInfo);
                }
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
            }
        }
        #endregion

        #endregion

        #region RSSK

        #region Rekam Medis
        public static void PrintLabelRM_RSSK(vLabelPatientRegistrationInfo oData, string printerUrl, string printFormat, int printCount = 1, bool isUsingPrintingTools = false)
        {
            try
            {
                //Get Printing Metadata
                List<string> lstField = GetMetadata(PrintingGroup.PatientLabel, printFormat);

                //Get and Load Printer Command
                string command = string.Empty;
                string pathTemplate = string.Empty;
                command = GetPrintFormat(PrintingGroup.PatientLabel, ref pathTemplate, printFormat);

                if (!string.IsNullOrEmpty(command))
                {
                    #region Store Registration Information to Dictionary
                    var labelInfo = new Dictionary<string, string>();
                    labelInfo.Add("HealthcareName", AppSession.UserLogin.HealthcareName);
                    foreach (string fieldName in lstField)
                    {
                        string[] paramField = fieldName.Split('=');

                        if (paramField[1] != "HealthcareName")
                            labelInfo.Add(paramField[1], GetPropertyValue(oData, paramField[1]).ToString());
                    }
                    #endregion

                    SendCommandToBrotherPrinter(lstField, pathTemplate, printerUrl, command, printCount, labelInfo, isUsingPrintingTools, PrintingGroup.PatientLabel, printFormat);
                }
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
            }
        }
        #endregion

        #endregion
    }
}
