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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class PatientLogList : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.VIEW_PATIENT_CHANGED_LOG;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
        }

        protected override void InitializeDataControl()
        {
            txtFromLogDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToLogDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(CurrPage, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string fromDate = Helper.GetDatePickerValue(txtFromLogDate).ToString("yyyyMMdd");
            string toDate = Helper.GetDatePickerValue(txtToLogDate).ToString("yyyyMMdd");
            String filterExpression = String.Format("CONVERT(date, LogDate) BETWEEN '{0}' AND '{1}'", fromDate, toDate);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAuditLogHistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += String.Format(" AND {0} ", hdnFilterExpressionQuickSearch.Value);
            }

            List<vPatientAuditLogHistory> lstEntity = BusinessLayer.GetvPatientAuditLogHistoryList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
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
                else if (AppSession.RIS_HL7_MESSAGE_FORMAT == Constant.RIS_HL7MessageFormat.ZED)
                {
                    var result2 = SendZedHL7UpdatePatientToRIS(mrn);
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
                else if (AppSession.RIS_HL7_MESSAGE_FORMAT == Constant.RIS_HL7MessageFormat.INFINITT)
                {
                    var result2 = SendInfinittHL7OrderToRIS(mrn);
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
                else if (AppSession.RIS_HL7_MESSAGE_FORMAT == Constant.RIS_HL7MessageFormat.MEDSYNAPTIC)
                {
                    var result2 = SendMedsynapticHL7UpdatePatientToRIS(mrn);
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

        private string SendInfinittHL7OrderToRIS(int mrn)
        {
            string result = string.Empty;

            string msgDateTime = string.Format("{0}{1}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.ToString("HHmmss").Replace(":", ""));
            string messageControlID = string.Format("{0}{1}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112_2), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_FULL_2).Replace(":", "").Replace(".", ""));

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
                msh.Field(5, CommonConstant.HL7_INFINITT_MSG.IDENTIFICATION_1);
                msh.Field(6, CommonConstant.HL7_INFINITT_MSG.IDENTIFICATION_2);
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

                #region EVN
                HL7Segment evn = new HL7Segment();
                evn.Field(0, "EVN");
                evn.Field(1, "A08");
                evn.Field(2, msgDateTime);
                msh.Field(3, string.Empty);
                msh.Field(4, string.Empty);
                msh.Field(5, string.Empty);

                hl7Message.Add(evn);
                #endregion

                #region PID
                string patientName = string.Format("{2} {0} {3}^{1}^^^^^^", oPatient.LastName, oPatient.FirstName, oPatient.MiddleName, oPatient.Salutation);
                string dateofBirth = string.Format("{0}000000", oPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                string gender = oPatient.GCGender.Split('^')[1]; ;
                string patientAddress = oPatient.HomeAddress == null ? string.Empty : string.Format("{0}^^{1}^", oPatient.StreetName.Replace("\n", " ").Replace("\t", " ").Replace(Environment.NewLine, " ").TrimEnd(), oPatient.City.TrimEnd());
                string phoneNo = oPatient.PhoneNo1 == null ? string.Empty : oPatient.PhoneNo1.Trim();
                string mobilephoneNo = oPatient.MobilePhoneNo1 == null ? string.Empty : oPatient.MobilePhoneNo1.Trim();
                string medicalNo = oPatient.MedicalNo;

                HL7Segment pid = new HL7Segment();
                pid.Field(0, "PID");
                pid.Field(1, "1");
                pid.Field(2, medicalNo);
                pid.Field(3, medicalNo);
                pid.Field(4, string.Empty);
                pid.Field(5, oPatient.PatientName);
                pid.Field(6, string.Empty);
                pid.Field(7, dateofBirth);
                pid.Field(8, gender);
                pid.Field(9, string.Empty);
                pid.Field(10, string.Empty);
                pid.Field(11, patientAddress);
                pid.Field(12, string.Empty);
                pid.Field(13, mobilephoneNo);
                pid.Field(14, phoneNo);

                hl7Message.Add(pid);
                #endregion

                #region PV1
                HL7Segment pv1 = new HL7Segment();
                pv1.Field(0, "PV1");
                pv1.Field(1, "1");
                pv1.Field(2, "O");
                for (int i = 3; i <= 43; i++)
                {
                    pv1.Field(2, "|");
                }
                pv1.Field(44, msgDateTime);
                hl7Message.Add(pv1);
                #endregion

                #region ZDS
                HL7Segment zds = new HL7Segment();
                zds.Field(0, "ZDS");
                zds.Field(1, string.Empty);
                zds.Field(2, string.Empty);
                zds.Field(3, string.Empty);
                zds.Field(4, string.Empty);

                hl7Message.Add(zds);
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

                HL7Message log = new HL7Message();
                log.MessageDateTime = DateTime.Now;
                log.Sender = "MEDINFRAS";
                log.DeviceNo = string.Empty;
                log.TransactionID = mrn;
                log.PatientName = patientName;
                log.MessageType = "ADT^A08";
                log.MessageCode = "ADT^A08";
                log.MessageText = msgText;
                log.MessageStatus = isSuccess ? "OK" : "ERR";
                log.ErrorMessage = resultInfo[1];
                BusinessLayer.InsertHL7Message(log);

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

        private string SendZedHL7UpdatePatientToRIS(int mrn)
        {
            string result = string.Empty;

            string msgDateTime = string.Format("{0}{1}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.ToString("HHmmss").Replace(":", ""));
            string messageControlID = string.Format("{0}{1}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112_2), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_FULL_2).Replace(":", "").Replace(".", ""));

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
                msh.Field(5, CommonConstant.HL7_ZED_MSG.IDENTIFICATION_1);
                msh.Field(6, CommonConstant.HL7_ZED_MSG.IDENTIFICATION_2);
                msh.Field(7, msgDateTime);
                msh.Field(8, string.Empty);
                msh.Field(9, "ADT^A08");
                msh.Field(10, messageControlID);
                msh.Field(11, "P");
                msh.Field(12, "2.3.1");

                hl7Message.Add(msh);
                #endregion

                #region PID
                //string patientName = string.Format("{2} {0} {3}^{1}^^^^^^", oPatient.LastName, oPatient.FirstName, oPatient.MiddleName, oPatient.Salutation);
                string patientName = string.Empty;
                string newFirstName = string.Empty;
                string newLastName = string.Empty;
                if (string.IsNullOrEmpty(oPatient.FirstName))
                {
                    string[] nameSplit = new string[0];

                    if (oPatient.LastName.Contains(" "))
                    {
                        nameSplit = oPatient.LastName.Split(' ');
                        if (nameSplit.Count() > 1)
                        {
                            newFirstName = nameSplit[0];
                            for (int i = 1; i < nameSplit.Count(); i++)
                            {
                                if (string.IsNullOrEmpty(newLastName))
                                {
                                    newLastName = nameSplit[i];
                                }
                                else
                                {
                                    newLastName += string.Format(" {0}", nameSplit[i]);
                                }
                            }
                            //patientName = string.Format("{0} {1} {2} {3}^^^^^^", newLastName, newFirstName, oVisit.MiddleName, oVisit.Salutation);
                            patientName = string.Format("{0} {3}^{1}^{2}", newLastName, newFirstName, oPatient.MiddleName, oPatient.Salutation);
                        }
                        else
                        {
                            patientName = string.Format("{0} {3}^{1}^{2}", newLastName, newFirstName, oPatient.MiddleName, oPatient.Salutation);
                        }
                    }
                    else
                    {
                        patientName = string.Format("{0} {3}^{1}^{2}", oPatient.LastName, oPatient.FirstName, oPatient.MiddleName, oPatient.Salutation);
                    }
                }
                else
                {
                    patientName = string.Format("{0} {3}^{1}^{2}", oPatient.LastName, oPatient.FirstName, oPatient.MiddleName, oPatient.Salutation);
                }
                if (patientName.Contains(','))
                {
                    patientName = patientName.Replace(",", " ");
                }

                //string dateofBirth = string.Format("{0}000000", oPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                //string gender = oPatient.GCGender.Split('^')[1];
                string gender = string.Empty;
                if (oPatient.GCGender == Constant.Gender.MALE)
                {
                    gender = "M";
                }
                else if (oPatient.GCGender == Constant.Gender.FEMALE)
                {
                    gender = "F";
                }
                else if (oPatient.GCGender == Constant.Gender.UNSPECIFIED)
                {
                    gender = "O";
                }
                
                string patientAddress = oPatient.HomeAddress == null ? string.Empty : string.Format("{0}^^{1}^", oPatient.StreetName.Replace("\n", " ").Replace("\t", " ").Replace(Environment.NewLine, " ").TrimEnd(), oPatient.City.TrimEnd());
                string phoneNo = oPatient.PhoneNo1 == null ? string.Empty : oPatient.PhoneNo1.Trim();
                string mobilephoneNo = oPatient.MobilePhoneNo1 == null ? string.Empty : string.Format("^^CP^{0}^62^^{1}", !string.IsNullOrEmpty(oPatient.EmailAddress) ? oPatient.EmailAddress : string.Empty, oPatient.MobilePhoneNo1.Trim());
                string medicalNo = oPatient.MedicalNo;

                HL7Segment pid = new HL7Segment();
                pid.Field(0, "PID");
                pid.Field(1, medicalNo);
                pid.Field(2, string.Empty);
                pid.Field(3, medicalNo);
                pid.Field(4, string.Empty);
                pid.Field(5, patientName);
                pid.Field(6, string.Empty);
                pid.Field(7, oPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112));
                pid.Field(8, gender);
                pid.Field(9, string.Empty);
                pid.Field(10, string.Empty);
                pid.Field(11, string.Empty);
                pid.Field(12, string.Empty);
                pid.Field(13, mobilephoneNo);

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

                HL7Message log = new HL7Message();
                log.MessageDateTime = DateTime.Now;
                log.Sender = "MEDINFRAS";
                log.DeviceNo = string.Empty;
                log.TransactionID = mrn;
                log.PatientName = patientName;
                log.MessageType = "ADT^A08";
                log.MessageCode = "ADT^A08";
                log.MessageText = msgText;
                log.MessageStatus = isSuccess ? "OK" : "ERR";
                log.ErrorMessage = resultInfo[1];
                BusinessLayer.InsertHL7Message(log);

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

        private string SendMedsynapticHL7UpdatePatientToRIS(int mrn)
        {
            string result = string.Empty;

            string msgDateTime = string.Format("{0}{1}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.ToString("HHmmss").Replace(":", ""));
            string messageControlID = string.Format("{0}{1}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112_2), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_FULL_2).Replace(":", "").Replace(".", ""));

            #region Patient Information
            vPatient oPatient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", mrn.ToString())).FirstOrDefault();
            vConsultVisit10 oVisit = BusinessLayer.GetvConsultVisit10List(string.Format("MRN = {0}", mrn.ToString())).FirstOrDefault();
            vMrnmergehistory oMerge = BusinessLayer.GetvMrnmergehistoryList(string.Format("FromMRN = {0}", mrn.ToString())).FirstOrDefault();

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
                msh.Field(5, CommonConstant.HL7_MEDSYNAPTIC_MSG.IDENTIFICATION_1);
                msh.Field(6, CommonConstant.HL7_MEDSYNAPTIC_MSG.IDENTIFICATION_2);
                msh.Field(7, msgDateTime);
                msh.Field(8, string.Empty);
                msh.Field(9, "ADT^A08");
                msh.Field(10, messageControlID);
                msh.Field(11, "P");
                msh.Field(12, "2.3.1");

                hl7Message.Add(msh);
                #endregion

                #region PID
                //string patientName = string.Format("{2} {0} {3}^{1}^^^^^^", oPatient.LastName, oPatient.FirstName, oPatient.MiddleName, oPatient.Salutation);
                string patientName = string.Empty;
                string newFirstName = string.Empty;
                string newLastName = string.Empty;
                if (string.IsNullOrEmpty(oPatient.FirstName))
                {
                    string[] nameSplit = new string[0];

                    if (oPatient.LastName.Contains(" "))
                    {
                        nameSplit = oPatient.LastName.Split(' ');
                        if (nameSplit.Count() > 1)
                        {
                            newFirstName = nameSplit[0];
                            for (int i = 1; i < nameSplit.Count(); i++)
                            {
                                if (string.IsNullOrEmpty(newLastName))
                                {
                                    newLastName = nameSplit[i];
                                }
                                else
                                {
                                    newLastName += string.Format(" {0}", nameSplit[i]);
                                }
                            }
                            //patientName = string.Format("{0} {1} {2} {3}^^^^^^", newLastName, newFirstName, oVisit.MiddleName, oVisit.Salutation);
                            patientName = string.Format("{0} {3}^{1}^{2}", newLastName, newFirstName, oPatient.MiddleName, oPatient.Salutation);
                        }
                        else
                        {
                            patientName = string.Format("{0} {3}^{1}^{2}", newLastName, newFirstName, oPatient.MiddleName, oPatient.Salutation);
                        }
                    }
                    else
                    {
                        patientName = string.Format("{0} {3}^{1}^{2}", oPatient.LastName, oPatient.FirstName, oPatient.MiddleName, oPatient.Salutation);
                    }
                }
                else
                {
                    patientName = string.Format("{0} {3}^{1}^{2}", oPatient.LastName, oPatient.FirstName, oPatient.MiddleName, oPatient.Salutation);
                }
                if (patientName.Contains(','))
                {
                    patientName = patientName.Replace(",", " ");
                }

                //string dateofBirth = string.Format("{0}000000", oPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                //string gender = oPatient.GCGender.Split('^')[1];
                string gender = string.Empty;
                if (oPatient.GCGender == Constant.Gender.MALE)
                {
                    gender = "M";
                }
                else if (oPatient.GCGender == Constant.Gender.FEMALE)
                {
                    gender = "F";
                }
                else if (oPatient.GCGender == Constant.Gender.UNSPECIFIED)
                {
                    gender = "O";
                }

                string patientAddress = oPatient.HomeAddress == null ? string.Empty : string.Format("{0}^^{1}^", oPatient.StreetName.Replace("\n", " ").Replace("\t", " ").Replace(Environment.NewLine, " ").TrimEnd(), oPatient.City.TrimEnd());
                string phoneNo = oPatient.PhoneNo1 == null ? string.Empty : oPatient.PhoneNo1.Trim();
                string mobilephoneNo = oPatient.MobilePhoneNo1 == null ? string.Empty : string.Format("^^CP^{0}^62^^{1}", !string.IsNullOrEmpty(oPatient.EmailAddress) ? oPatient.EmailAddress : string.Empty, oPatient.MobilePhoneNo1.Trim());
                string medicalNo = oPatient.MedicalNo;
                string registrationNo = oVisit.RegistrationNo;

                HL7Segment pid = new HL7Segment();
                pid.Field(0, "PID");
                pid.Field(1, medicalNo);
                pid.Field(2, string.Empty);
                pid.Field(3, medicalNo);
                pid.Field(4, string.Empty);
                pid.Field(5, patientName);
                pid.Field(6, string.Empty);
                pid.Field(7, oPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112));
                pid.Field(8, gender);
                pid.Field(9, string.Empty);
                pid.Field(10, string.Empty);
                pid.Field(11, string.Empty);
                pid.Field(12, string.Empty);
                pid.Field(13, mobilephoneNo);
                pid.Field(14, string.Empty);
                pid.Field(15, string.Empty);
                pid.Field(16, string.Empty);
                pid.Field(17, string.Empty);
                pid.Field(18, registrationNo);

                hl7Message.Add(pid);
                #endregion

                #region MRG
                if (oMerge != null)
                {
                    string fromMedicalNo = oMerge.FromMedicalNo;
                    HL7Segment mrg = new HL7Segment();
                    mrg.Field(0, "MRG");
                    mrg.Field(1, fromMedicalNo);

                    hl7Message.Add(mrg);
                }
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

                HL7Message log = new HL7Message();
                log.MessageDateTime = DateTime.Now;
                log.Sender = "MEDINFRAS";
                log.DeviceNo = string.Empty;
                log.TransactionID = mrn;
                log.PatientName = patientName;
                log.MessageType = "ADT^A08";
                log.MessageCode = "ADT^A08";
                log.MessageText = msgText;
                log.MessageStatus = isSuccess ? "OK" : "ERR";
                log.ErrorMessage = resultInfo[1];
                BusinessLayer.InsertHL7Message(log);

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