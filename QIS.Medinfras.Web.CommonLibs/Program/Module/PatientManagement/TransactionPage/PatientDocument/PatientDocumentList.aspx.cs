using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.IO;
using System.Text;
using System.Net.Sockets;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientDocumentList : BasePagePatientPageList
    {
        string deptType = string.Empty;
        string menuType = string.Empty;

        protected int PageCount = 1;
        protected List<vPatientDocument> lstPatientDocument = null;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                switch (deptType)
                {
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_PAGE_E_DOCUMENT;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_E_DOCUMENT;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_PATIENT_PAGE_E_DOCUMENT;
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_PAGE_E_DOCUMENT;
                    default:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_E_DOCUMENT;
                }
            }
            else if (menuType == "dp")
            {
                switch (deptType)
                {
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_PAGE_E_DOCUMENT;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.DATA_PATIENT_PATIENT_PAGE_E_DOCUMENT;
                    case Constant.Facility.MEDICAL_CHECKUP:
                        return Constant.MenuCode.MedicalCheckup.DATA_PATIENT_PATIENT_PAGE_E_DOCUMENT;
                    default:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_PAGE_E_DOCUMENT;
                }
            }
            else if (menuType == "nt")
            {
                #region Gizi
                return Constant.MenuCode.Nutrition.DATA_PATIENT_PATIENT_PAGE_E_DOCUMENT;
                #endregion
            }
            else if (menuType == "cp")
            {
                switch (deptType)
                {
                    case Constant.Facility.PHARMACY:
                        return Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL_E_DOCUMENT;
                    default:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_PAGE_E_DOCUMENT;
                }
            }
            else
            {
                switch (deptType)
                {
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_E_DOCUMENT;
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_PAGE_E_DOCUMENT;
                    case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PATIENT_PAGE_E_DOCUMENT;
                    case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_E_DOCUMENT;
                    case Constant.Facility.IMAGING: return Constant.MenuCode.Imaging.PATIENT_PAGE_E_DOCUMENT;
                    case Constant.Facility.LABORATORY: return Constant.MenuCode.Laboratory.PATIENT_PAGE_E_DOCUMENT;
                    case Constant.Module.RADIOTHERAPHY:
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_E_DOCUMENT;

                    default: return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_E_DOCUMENT;
                }
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param.Count() > 1)
            {
                deptType = param[0];
                menuType = param[1];
            }
            else
            {
                deptType = param[0];
            }

            hdnPatientDocumentUrl.Value = string.Format(@"{0}/{1}/", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISPatientDocumentsPath.Replace("#MRN", AppSession.RegisteredPatient.MedicalNo));
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDocumentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_ITEM);
            }

            List<vPatientDocument> lstEntity = BusinessLayer.GetvPatientDocumentList(filterExpression, Constant.GridViewPageSize.GRID_ITEM, pageIndex, "DocumentDate");
            //lstPatientDocument = BusinessLayer.GetvPatientDocumentList(string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY ID DESC", AppSession.RegisteredPatient.VisitID));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientDocument obj = (vPatientDocument)e.Row.DataItem;
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/PatientDocument/NewPatientDocumentCtl.ascx");
            queryString = "";
            popupWidth = 700;
            popupHeight = 500;
            popupHeaderText = "Patient e-Document";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/PatientDocument/NewPatientDocumentCtl.ascx");
                queryString = hdnID.Value;
                popupWidth = 700;
                popupHeight = 500;
                popupHeaderText = "Patient e-Document";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                PatientDocument entity = BusinessLayer.GetPatientDocument(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                if (entity.CreatedBy > 0 || entity.GCDocumentType != Constant.DocumentType.DIAGNOSTIC_IMAGING)
                {
                    Patient oPatient = BusinessLayer.GetPatient(entity.MRN);
                    if (oPatient != null) {
                        string path = AppConfigManager.QISPhysicalDirectory;
                        path += string.Format("{0}\\{1}", AppConfigManager.QISPatientDocumentsPath.Replace('/', '\\'), entity.FileName);
                        path = path.Replace("#MRN", oPatient.MedicalNo);
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                }
                BusinessLayer.UpdatePatientDocument(entity);
                BindGridView(1, true, ref PageCount);
                return true;
            }
            return false;
        }

        protected void cbpOpenDocument_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            try
            {
                string path = AppConfigManager.QISPhysicalDirectory;
                path += string.Format("{0}\\{1}", AppConfigManager.QISPatientDocumentsPath.Replace('/', '\\'),param[1]);
                path = path.Replace("#MRN", AppSession.RegisteredPatient.MedicalNo);
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    //file.Open(FileMode.Open);
                }

                result += string.Format("success|{0}", errMessage);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpScanDocument_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            result = SendPatientInformationToScanner();

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string SendPatientInformationToScanner()
        {
            string result = string.Empty;

            try
            {
                StringBuilder sbMessage = new StringBuilder();
                //string localIPAddress = HttpContext.Current.Request.UserHostAddress;
                string localIPAddress = Methods.GetLocalIPAddress();
                string port = string.IsNullOrEmpty(hdnPort.Value) ? "6000" : hdnPort.Value;
                string message = string.Format("MD102|{0};{1};{2};{3};{4};{5}", AppSession.UserLogin.HealthcareID, AppSession.RegisteredPatient.MRN,AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.PatientName, "", "", "0");
                TcpClient client = new TcpClient();
                client.Connect(localIPAddress, Convert.ToInt16(port));
                NetworkStream stream = client.GetStream();
                using (BinaryWriter w = new BinaryWriter(stream))
                {
                    using (BinaryReader r = new BinaryReader(stream))
                    {
                        w.Write(string.Format(@"{0}", message).ToCharArray());
                    }

                    //#region Receive ACK Response From Server
                    //using (BinaryReader r = new BinaryReader(stream, Encoding.GetEncoding(1252)))
                    //{
                    //    // Reads NetworkStream into a byte buffer.
                    //    int length = (int)client.ReceiveBufferSize;
                    //    byte[] buffer = new byte[client.ReceiveBufferSize];

                    //    stream.Read(buffer, 0, length);

                    //    string data = Encoding.UTF8.GetString(buffer);

                    //    // Find start of MLLP frame, a VT character ...
                    //    int start = data.IndexOf((char)0x0B);
                    //    if (start >= 0)
                    //    {
                    //        //Look for the end of the frame, a FS Character
                    //        int end = data.IndexOf((char)0x1C);
                    //        if (end > start)
                    //        {
                    //            string temp = data.Substring(start + 1, end - start);
                    //            result = ResponseToACKMessage(temp);
                    //        }
                    //    }
                    //}
                    //#endregion
                }

                result = string.Format("process|1|{0}|{1}|{2}", "Dokumen pasien berhasil diproses", string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}|{1}|{2}", ex.Message, string.Empty, string.Empty);
            }

            return result;
        }
    }
}