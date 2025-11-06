using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Common;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class MRNMergeHistory : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.VIEW_MRN_MERGE_LOG;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }

        private string refreshGridInterval = "";

        protected override void InitializeDataControl()
        {
            if (!Page.IsPostBack)
            {
                txtFromLogDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtToLogDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                MPTrx2 masterTrx = ((MPTrx2)Master);
                MPMain masterMain = (MPMain)masterTrx.Master;
                menu = (masterMain).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
                BindGridView(1, true, ref PageCount);
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvMrnmergehistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += String.Format(" AND {0} ", hdnFilterExpressionQuickSearch.Value);
            }

            List<vMrnmergehistory> lstEntity = BusinessLayer.GetvMrnmergehistoryList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }


        private string GetFilterExpression()
        {
            string fromDate = Helper.GetDatePickerValue(txtFromLogDate).ToString("yyyyMMdd");
            string toDate = Helper.GetDatePickerValue(txtToLogDate).ToString("yyyyMMdd");
            String filterExpression = String.Format("CONVERT(date, CreatedDate) BETWEEN '{0}' AND '{1}'", fromDate, toDate);

            return filterExpression;
        }

        protected void cbpSendToRIS_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            try
            {
                int fromMRN = Convert.ToInt32(hdnFromMRN.Value);
                int toMRN = Convert.ToInt32(hdnToMRN.Value);
                string[] resultInfo = "0|Unknown Protocol".Split('|');

                if (AppSession.RIS_HL7_MESSAGE_FORMAT == Constant.RIS_HL7MessageFormat.MEDAVIS)
                {
                    var result2 = SendMedavisHL7OrderToRIS(fromMRN, toMRN);
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
                else if (AppSession.RIS_HL7_MESSAGE_FORMAT == Constant.RIS_HL7MessageFormat.ZED)
                {
                    var result2 = SendZEDHL7OrderToRIS(fromMRN, toMRN);
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
            panel.JSProperties["cpTransactionID"] = hdnToMRN.Value;
        }

        private string SendMedavisHL7OrderToRIS(int fromMRN, int toMRN)
        {
            string result = string.Empty;

            string msgDateTime = string.Format("{0}{1}00", DateTime.Now.Date.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss").Replace(":", ""));
            string messageControlID = string.Format("{0}{1}", DateTime.Now.Date.ToString("yyyyMMdd"), DateTime.Now.ToString("HH:mm:ss.fff").Replace(":", "").Replace(".", ""));

            #region Patient Information
            List<vPatient> lstPatient = BusinessLayer.GetvPatientList(string.Format("MRN IN ({0},{1})", fromMRN.ToString(), toMRN.ToString()));

            vPatient oPatient1 = lstPatient.Where(lst => lst.MRN == fromMRN).FirstOrDefault();
            vPatient oPatient2 = lstPatient.Where(lst => lst.MRN == toMRN).FirstOrDefault();

            if (oPatient2 != null)
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
                msh.Field(9, "ADT^A34");
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
                string patientName = string.Format("{2}{0}^{1}^^^{3}^^^", oPatient2.LastName, oPatient2.FirstName, oPatient2.MiddleName, oPatient2.Salutation);
                string dateofBirth = string.Format("{0}000000", oPatient2.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                string gender = oPatient2.GCGender.Split('^')[1]; ;
                string patientAddress = oPatient2.HomeAddress == null ? string.Empty : string.Format("{0}^^{1}^", oPatient2.StreetName.Replace("\n", " ").Replace("\t", " ").Replace(Environment.NewLine, " ").TrimEnd(), oPatient2.City.TrimEnd());
                string phoneNo = oPatient2.PhoneNo1 == null ? string.Empty : oPatient2.PhoneNo1.Trim();
                string fromMedicalNo = oPatient1.MedicalNo;
                string toMedicalNo = oPatient2.MedicalNo;

                HL7Segment pid = new HL7Segment();
                pid.Field(0, "PID");
                pid.Field(1, "1");
                pid.Field(2, toMedicalNo);
                pid.Field(3, toMedicalNo);
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

                #region MRG
                HL7Segment mrg = new HL7Segment();
                mrg.Field(0, "MRG");
                mrg.Field(1, fromMedicalNo);

                hl7Message.Add(mrg);
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
                    result = string.Format("{0}|{1}", "1", string.Format("{0}", toMedicalNo));
                }
                else
                {
                    result = string.Format("{0}|{1} ({2})", "0", "An error occured when sending HL7 Message.", resultInfo[1]);
                }
            }
            #endregion
            return result;
        }

        private string SendZEDHL7OrderToRIS(int fromMRN, int toMRN)
        {
            string result = string.Empty;

            string msgDateTime = string.Format("{0}{1}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.ToString("HHmmss").Replace(":", ""));
            string messageControlID = string.Format("{0}{1}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112_2), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_FULL_2).Replace(":", "").Replace(".", ""));

            #region Patient Information
            List<vPatient> lstPatient = BusinessLayer.GetvPatientList(string.Format("MRN IN ({0},{1})", fromMRN.ToString(), toMRN.ToString()));

            vPatient oPatient1 = lstPatient.Where(lst => lst.MRN == fromMRN).FirstOrDefault();
            vPatient oPatient2 = lstPatient.Where(lst => lst.MRN == toMRN).FirstOrDefault();

            if (oPatient2 != null)
            {
                HL7MessageText hl7Message = new HL7MessageText();

                #region MSH
                HL7Segment msh = new HL7Segment();
                msh.Field(0, "MSH");
                msh.Field(1, ""); //will be ignored
                msh.Field(2, @"^~\&");
                msh.Field(3, "MEDINFRAS-API_RIS");
                msh.Field(4, AppSession.UserLogin.HealthcareID); //HealthcareID
                msh.Field(5, CommonConstant.HL7_ZED_MSG.IDENTIFICATION_1);
                msh.Field(6, CommonConstant.HL7_ZED_MSG.IDENTIFICATION_2);
                msh.Field(7, msgDateTime);
                msh.Field(8, string.Empty);
                msh.Field(9, "ADT^A18");
                msh.Field(10, messageControlID);
                msh.Field(11, "P");
                msh.Field(12, "2.3.1");

                hl7Message.Add(msh);
                #endregion

                #region PID
                string patientName = string.Format("{2}{0}^{1}^^^{3}^^^", oPatient2.LastName, oPatient2.FirstName, oPatient2.MiddleName, oPatient2.Salutation);
                string dateofBirth = string.Format("{0}000000", oPatient2.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                string gender = oPatient2.GCGender.Split('^')[1]; ;
                string patientAddress = oPatient2.HomeAddress == null ? string.Empty : string.Format("{0}^^{1}^", oPatient2.StreetName.Replace("\n", " ").Replace("\t", " ").Replace(Environment.NewLine, " ").TrimEnd(), oPatient2.City.TrimEnd());
                string phoneNo = oPatient2.PhoneNo1 == null ? string.Empty : oPatient2.PhoneNo1.Trim();
                string fromMedicalNo = oPatient1.MedicalNo;
                string toMedicalNo = oPatient2.MedicalNo;

                HL7Segment pid = new HL7Segment();
                pid.Field(0, "PID");
                pid.Field(1, string.Empty);
                pid.Field(2, string.Empty);
                pid.Field(3, toMedicalNo);
                pid.Field(4, string.Empty);
                pid.Field(5, string.Empty);
                pid.Field(6, string.Empty);
                pid.Field(7, string.Empty);
                pid.Field(8, string.Empty);
                pid.Field(9, string.Empty);
                pid.Field(10, string.Empty);
                pid.Field(11, string.Empty);
                pid.Field(12, string.Empty);
                pid.Field(13, string.Empty);

                hl7Message.Add(pid);
                #endregion

                #region MRG
                HL7Segment mrg = new HL7Segment();
                mrg.Field(0, "MRG");
                mrg.Field(1, fromMedicalNo);

                hl7Message.Add(mrg);
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
                    result = string.Format("{0}|{1}", "1", string.Format("{0}", toMedicalNo));
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