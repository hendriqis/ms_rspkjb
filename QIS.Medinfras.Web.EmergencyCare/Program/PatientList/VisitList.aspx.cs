using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EmergencyCare.Program
{
    public partial class VisitList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case "ct": return Constant.MenuCode.EmergencyCare.CHANGE_PATIENT_TRANSACTION_STATUS;
                case "ptp": return Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_PAGE;
                case "pe": return Constant.MenuCode.EmergencyCare.PATIENT_PAGE;
                default: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY;
            }
        }
        private GetUserMenuAccess menu;
        public override bool IsShowRightPanel()
        {
            return false;
        }
        protected String GetMenuCaption()
        {
            return menu.MenuCaption;
        }
        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string id = Page.Request.QueryString["id"];

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                hdnIsUsedReopenBilling.Value = AppSession.IsUsedReopenBilling ? "1" : "0";
                if (id == "bs")
                {
                    trFilterReopenBilling.Visible = AppSession.IsUsedReopenBilling;
                }
                else
                {
                    trFilterReopenBilling.Visible = false;
                }

                SettingControlProperties();
                grdRegisteredPatient.InitializeControl();

                Helper.SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                txtBarcodeEntry.Focus();
            }
        }

        private void SettingControlProperties()
        {
            if (AppSession.LastContentVisitListER != null)
            {
                LastContentVisitListER lc = AppSession.LastContentVisitListER;
                txtRegistrationDate.Text = lc.RegistrationDate;
                txtBarcodeEntry.Text = lc.MedicalNo;
                hdnPhysicianID.Value = lc.ParamedicID.ToString();
                hdnQuickText.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                hdnFilterExpressionQuickSearch.Value = lc.QuickFilterExpression;
            }
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string id = Page.Request.QueryString["id"];
            string filterExpression = string.Empty;

            if (id == "pe")
            {
                filterExpression += string.Format("DepartmentID = '{0}' AND GCVisitStatus NOT IN ('{1}','{2}')",
                        Constant.Facility.EMERGENCY,
                        Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED);
            }
            else
            {
                if (hdnIsUsedReopenBilling.Value == "1")
                {
                    if (chkIsIncludeReopenBilling.Checked)
                    {
                        filterExpression += string.Format("DepartmentID = '{0}' AND GCVisitStatus IN ('{1}') AND IsBillingReopen = 1",
                                Constant.Facility.EMERGENCY,
                                Constant.VisitStatus.CLOSED);
                    }
                    else
                    {
                        if (id == "ptp")
                        {
                            filterExpression += string.Format("DepartmentID = '{0}' AND GCVisitStatus NOT IN ('{1}','{2}','{3}','{4}')",
                                    Constant.Facility.EMERGENCY,
                                    Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED);
                        }
                        else
                        {
                            filterExpression += string.Format("DepartmentID = '{0}' AND GCVisitStatus NOT IN ('{1}','{2}','{3}')",
                                    Constant.Facility.EMERGENCY,
                                    Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
                        }
                    }
                }
                else
                {
                    if (id == "ptp")
                    {
                        filterExpression += string.Format("DepartmentID = '{0}' AND GCVisitStatus NOT IN ('{1}','{2}','{3}','{4}')",
                                Constant.Facility.EMERGENCY,
                                Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED);
                    }
                    else
                    {
                        filterExpression += string.Format("DepartmentID = '{0}' AND GCVisitStatus NOT IN ('{1}','{2}','{3}')",
                                Constant.Facility.EMERGENCY,
                                Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
                    }
                }
            }

            if (txtPhysicianCode.Text != "")
                filterExpression += string.Format(" AND ParamedicID = {0}", hdnPhysicianID.Value);

            if (!chkIsPreviousEpisodePatient.Checked)
            {
                filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            if (hdnSearchBarcodeNoRM.Value != "" && hdnSearchBarcodeNoRM.Value != null && hdnSearchBarcodeNoRM.Value != "00-00-00-00")
            {
                filterExpression += string.Format(" AND MedicalNo = '{0}'", hdnSearchBarcodeNoRM.Value);
            }

            return filterExpression;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo)).FirstOrDefault();
            Response.Redirect(GetResponseRedirectUrl(entity));
        }

        private string GetResponseRedirectUrl(vConsultVisit4 entity)
        {
            string url = "";
            string id = Page.Request.QueryString["id"];
            string test = Page.ResolveUrl("~/");
            if (id == "bs" || id == "ptp" || id == "pe")
            {
                RegisteredPatient pt = new RegisteredPatient();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
                pt.GCGender = entity.GCGender;
                pt.GCSex = entity.GCSex;
                pt.DateOfBirth = entity.DateOfBirth;
                pt.RegistrationID = entity.RegistrationID;
                pt.RegistrationNo = entity.RegistrationNo;
                pt.RegistrationDate = entity.RegistrationDate;
                pt.RegistrationTime = entity.RegistrationTime;
                pt.VisitID = entity.VisitID;
                pt.VisitDate = entity.VisitDate;
                pt.VisitTime = entity.VisitTime;
                pt.StartServiceDate = entity.StartServiceDate;
                pt.StartServiceTime = entity.StartServiceTime;
                pt.DischargeDate = entity.DischargeDate;
                pt.DischargeTime = entity.DischargeTime;
                pt.GCCustomerType = entity.GCCustomerType;
                pt.BusinessPartnerID = entity.BusinessPartnerID;
                pt.ParamedicID = entity.ParamedicID;
                pt.ParamedicCode = entity.ParamedicCode;
                pt.ParamedicName = entity.ParamedicName;
                pt.SpecialtyID = entity.SpecialtyID;
                pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                pt.DepartmentID = entity.DepartmentID;
                pt.ServiceUnitName = entity.ServiceUnitName;
                pt.RoomCode = entity.RoomCode;
                pt.BedCode = entity.BedCode;
                pt.DepartmentID = entity.DepartmentID;
                pt.ChargeClassID = entity.ChargeClassID;
                pt.ClassID = entity.ClassID;
                pt.GCRegistrationStatus = entity.GCVisitStatus;
                pt.IsLockDown = entity.IsLockDown;
                pt.IsBillingReopen = entity.IsBillingReopen;
                pt.LinkedRegistrationID = entity.LinkedRegistrationID;
                pt.LinkedToRegistrationID = entity.LinkedToRegistrationID;
                AppSession.RegisteredPatient = pt;
                AppSession.HealthcareServiceUnitID = entity.HealthcareServiceUnitID.ToString();

                LastContentVisitListER lc = new LastContentVisitListER()
                {
                    RegistrationDate = txtRegistrationDate.Text,
                    ParamedicID = hdnPhysicianID.Value == string.Empty ? 0 : Convert.ToInt32(hdnPhysicianID.Value),
                    QuickText = txtSearchView.Text,
                    QuickFilterExpression = hdnFilterExpressionQuickSearch.Value,
                    MedicalNo = txtBarcodeEntry.Text
                };
                AppSession.LastContentVisitListER = lc;
                
                string paramValue = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_TRANSAKSI_HANYA_DAPAT_DIUBAH_UNIT_ASAL).ParameterValue;
                AppSession.IsAdminCanCancelAllTransaction = paramValue == "0" ? true : false;

                if (id == "bs" || id == "ptp")
                {
                    string parentCode = "";
                    if (id == "bs")
                    {
                        parentCode = Constant.MenuCode.EmergencyCare.BILL_SUMMARY;
                        string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
                        List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.EMERGENCY, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                        int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

                        filterExpression = string.Format("ParentID = {0}", parentID);
                        lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.EMERGENCY, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                        GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                        url = Page.ResolveUrl(menu.MenuUrl);
                    }
                    else
                    {
                        parentCode = Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_PAGE;
                        string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
                        List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.EMERGENCY, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                        int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

                        filterExpression = string.Format("ParentID = {0}", parentID);
                        lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.EMERGENCY, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                        GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                        url = Page.ResolveUrl(menu.MenuUrl);
                    }
                }
                else
                    url = "~/Libs/Program/Module/PatientManagement/PatientAssessment/PatientStatus.aspx";
            }
            else
                url = string.Format("~/Libs/Program/Module/PatientManagement/ChangePatientTransactionStatus/ChangePatientTransactionStatusEntry.aspx?id={0}", entity.VisitID);
            return url;
        }


        protected void cbpBarcodeEntryProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression = GetFilterExpression();
            filterExpression += string.Format(" AND MedicalNo = '{0}'", txtBarcodeEntry.Text);
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(filterExpression).FirstOrDefault();

            string url = "";
            if (entity != null)
                url = GetResponseRedirectUrl(entity);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpUrl"] = url;
        }
    }
}