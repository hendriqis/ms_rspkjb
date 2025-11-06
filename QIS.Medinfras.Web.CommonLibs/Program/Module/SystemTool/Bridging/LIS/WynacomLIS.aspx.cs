using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class WynacomLIS : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            switch (hdnModuleID.Value)
            {
                case Constant.Module.SYSTEM_SETUP:
                    return Constant.MenuCode.SystemSetup.LIS_Wynacom;
                default:
                    return Constant.MenuCode.SystemSetup.LIS_Wynacom;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetRefreshGridInterval()
        {
            return AppSession.RefreshGridInterval;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnModuleID.Value = Page.Request.QueryString["id"];
            }

            txtFromDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(CurrPage, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string fromDate = Helper.GetDatePickerValue(txtFromDate).ToString("yyyyMMdd");
            string toDate = Helper.GetDatePickerValue(txtFromDate).ToString("yyyyMMdd");
            String filterExpression = String.Format("TransactionCode = '{0}'  AND CONVERT(date, TransactionDate) BETWEEN '{1}' AND '{2}'",Constant.TransactionCode.LABORATORY_CHARGES,  fromDate, toDate);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += String.Format(" AND {0} ", hdnFilterExpressionQuickSearch.Value);
            }

            List<vPatientChargesHd> lstEntity = BusinessLayer.GetvPatientChargesHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TransactionID");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
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

        protected void cbpSendToRIS_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            try
            {
                int mrn = Convert.ToInt32(hdnMRN.Value);
                string[] resultInfo = "0|Unknown Protocol".Split('|');

                if (AppSession.RIS_HL7_MESSAGE_FORMAT == Constant.RIS_HL7MessageFormat.MEDAVIS)
                {
                    var result2 = SendMedavisHL7OrderToRIS(mrn);
                    resultInfo = ((string)result2).Split('|');
                    if (resultInfo[0] == "1")
                    {
                        result += string.Format("success|{0}", string.Empty);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", resultInfo[1]);
                    }
                }
                else
                {
                    //TODO : Implement Code for another HL7 Order Mechanism (i.e NovaRAD)
                    //var result2 = SendHL7OrderToRIS(Convert.ToInt32(hdnTestOrderID.Value), Convert.ToInt32(hdnTransactionHdID.Value));
                    //resultInfo = ((string)result2).Split('|');

                    //resultInfo = "0|Unknown Protocol".Split('|');
                    result += string.Format("fail|{0}", "Unknown Protocol");
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = hdnMRN.Value;
        }

        private string SendMedavisHL7OrderToRIS(int mrn)
        {
            string result = string.Empty;

            string msgDateTime = string.Format("{0}{1}00", DateTime.Now.Date.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss").Replace(":", ""));
            string messageControlID = string.Format("{0}{1}", DateTime.Now.Date.ToString("yyyyMMdd"), DateTime.Now.ToString("HH:mm:ss.fff").Replace(":", "").Replace(".", ""));

            #region Patient Information
            vPatient oPatient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", mrn.ToString())).FirstOrDefault();
            if (oPatient != null)
            {
                HL7MessageText hl7Message = new HL7MessageText();

                #region MSH
                HL7Segment msh = new HL7Segment();
                msh.Field(0, "MSH");
                msh.Field(1, ""); //will be ignored
                msh.Field(2, @"^~\&");
                msh.Field(3, "MEDINFRAS-API_RIS");
                msh.Field(4, AppSession.UserLogin.HealthcareID); //HealthcareID
                msh.Field(5, CommonConstant.HL7_MEDAVIS_MSG.IDENTIFICATION_1);
                msh.Field(6, CommonConstant.HL7_MEDAVIS_MSG.IDENTIFICATION_2);
                msh.Field(7, msgDateTime);
                msh.Field(8, string.Empty);
                msh.Field(9, "ADT^A08");
                msh.Field(10, messageControlID);
                msh.Field(11, "P");
                msh.Field(12, "2.3.1");
                msh.Field(13, string.Empty);
                msh.Field(14, string.Empty);
                msh.Field(15, "ER");
                msh.Field(16, "ER");
                msh.Field(17, string.Empty);
                msh.Field(18, "8859/1");

                hl7Message.Add(msh);
                #endregion

                #region PID
                string patientName = string.Format("{2} {0}^{1}^^^{3}^^^", oPatient.LastName, oPatient.FirstName, oPatient.MiddleName, oPatient.Salutation);
                string dateofBirth = string.Format("{0}000000", oPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                string gender = oPatient.GCGender.Split('^')[1]; ;
                string patientAddress = oPatient.HomeAddress == null ? string.Empty : string.Format("{0}^^{1}^", oPatient.StreetName.Replace("\n", " ").Replace("\t", " ").Replace(Environment.NewLine, " ").TrimEnd(), oPatient.City.TrimEnd());
                string phoneNo = oPatient.PhoneNo1 == null ? string.Empty : oPatient.PhoneNo1.Trim();
                string medicalNo = oPatient.MedicalNo;

                HL7Segment pid = new HL7Segment();
                pid.Field(0, "PID");
                pid.Field(1, "1");
                pid.Field(2, medicalNo);
                pid.Field(3, medicalNo);
                pid.Field(4, string.Empty);
                pid.Field(5, patientName.Trim());
                pid.Field(6, string.Empty);
                pid.Field(7, dateofBirth);
                pid.Field(8, gender);
                pid.Field(9, string.Empty);
                pid.Field(10, string.Empty);
                pid.Field(11, patientAddress);
                pid.Field(12, string.Empty);
                pid.Field(13, phoneNo);

                hl7Message.Add(pid);
                #endregion

                string msgText = (char)0x0B + hl7Message.Serialize() + (char)0x1C + (char)0x0D;
                string ipaddress, port = string.Empty;
                SettingParameterDt oParam = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode ='{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_RIS_HL7_BROKER)).FirstOrDefault();
                string[] paramInfo = oParam.ParameterValue.Split(':');
                ipaddress = paramInfo[0];
                port = !string.IsNullOrEmpty(paramInfo[1]) ? paramInfo[1] : "6000";

                result = CommonMethods.SendMessageToListener(ipaddress, port, msgText);
                string[] resultInfo = result.Split('|');
                bool isSuccess = resultInfo[0] == "1";

                if (isSuccess)
                {
                    result = string.Format("{0}|{1}", "1", string.Format("{0}", medicalNo));
                }
                else
                {
                    result = string.Format("{0}|{1} ({2})", "0", "An error occured when sending HL7 Message.", resultInfo[1]);
                }
            }
            #endregion
            return result;
        }
    }
}