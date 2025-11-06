using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class VisitList : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case "ptp": return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_PAGE;
                case "vt": return Constant.MenuCode.Inpatient.TRANSACTION_VERIFICATION;
                case "ct": return Constant.MenuCode.Inpatient.CHANGE_PATIENT_TRANSACTION_STATUS;
                default: return Constant.MenuCode.Inpatient.BILL_SUMMARY;
            }
        }
        private GetUserMenuAccess menu;
        public override bool IsShowRightPanel()
        {
            return false;
        }
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
        protected int PageCount = 1;
        string id = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.INPATIENT, "IsUsingRegistration = 1");
                id = Page.Request.QueryString["id"].Split('|')[0];
                if (id == "bs")
                {
                    StringBuilder sbLstHealthcareServiceUnitID = new StringBuilder();
                    foreach (GetServiceUnitUserList serviceUnit in lstServiceUnit)
                    {
                        if (sbLstHealthcareServiceUnitID.ToString() != "")
                            sbLstHealthcareServiceUnitID.Append(",");
                        sbLstHealthcareServiceUnitID.Append(serviceUnit.HealthcareServiceUnitID.ToString());
                    }
                    hdnLstHealthcareServiceUnitID.Value = sbLstHealthcareServiceUnitID.ToString();
                    lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                }
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                if (id == "bs")
                    cboServiceUnit.SelectedIndex = 1;
                else
                    cboServiceUnit.SelectedIndex = 0;

                List<StandardCode> lstCheckIn = new List<StandardCode>();
                List<StandardCode> lstRegistrationStatus = new List<StandardCode>();
                FillComboBoxForFilter(lstCheckIn, lstRegistrationStatus);
                Methods.SetComboBoxField<StandardCode>(cboCheckinStatus, lstCheckIn, "StandardCodeName", "StandardCodeID");
                cboCheckinStatus.SelectedIndex = 0;

                hdnIsUsedReopenBilling.Value = AppSession.IsUsedReopenBilling ? "1" : "0";
                if (id == "bs")
                {
                    tdFilterReopenBilling.Visible = AppSession.IsUsedReopenBilling;
                }
                else
                {
                    tdFilterReopenBilling.Visible = false;
                }

                SettingControlProperties();
                BindGridView(1, true, ref PageCount);
                Helper.SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, true), "mpPatientList");
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
                txtBarcodeEntry.Focus();
            }
        }

        private void SettingControlProperties()
        {
            if (AppSession.LastContentVisitListIP != null)
            {
                LastContentVisitListIP lc = AppSession.LastContentVisitListIP;
                hdnPhysicianID.Value = lc.ParamedicID.ToString();
                if (id == "bs")
                {
                    cboServiceUnit.Value = lc.HealthcareServiceUnitID.ToString();
                }
                else
                {
                    if (lc.HealthcareServiceUnitID == 0)
                        cboServiceUnit.SelectedIndex = 0;
                    else
                        cboServiceUnit.Value = lc.HealthcareServiceUnitID.ToString();
                }

                if (lc.ParamedicID != 0)
                {
                    ParamedicMaster pm = BusinessLayer.GetParamedicMaster(lc.ParamedicID);
                    if (pm != null)
                    {
                        txtPhysicianCode.Text = pm.ParamedicCode;
                        txtPhysicianName.Text = pm.FullName;
                    }
                }
                else
                {
                    txtPhysicianCode.Text = string.Empty;
                    txtPhysicianName.Text = string.Empty;
                }

                txtBarcodeEntry.Text = lc.MedicalNo;
                hdnQuickText.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                hdnFilterExpressionQuickSearch.Value = lc.QuickFilterExpression;

                if (id == "ptp")
                {
                    if (lc.Status != "1")
                    {
                        cboCheckinStatus.Value = "1";
                    }
                    else
                    {
                        cboCheckinStatus.Value = lc.Status;
                    }
                }
                else
                {
                    cboCheckinStatus.Value = lc.Status;
                }
            }
        }

        private void FillComboBoxForFilter(List<StandardCode> lstCheckIn, List<StandardCode> lstRegistrationStatus)
        {
            lstCheckIn.Add(new StandardCode() { StandardCodeID = "1", StandardCodeName = "Masih Dirawat" });
            string id = Page.Request.QueryString["id"];
            //if (id != "bs")
            if (id == "bs")
            {
                lstCheckIn.Add(new StandardCode() { StandardCodeID = "2", StandardCodeName = "Rencana Pulang" });
                lstCheckIn.Add(new StandardCode() { StandardCodeID = "3", StandardCodeName = "Sudah Pulang" });
                lstCheckIn.Add(new StandardCode() { StandardCodeID = "4", StandardCodeName = "Belum Dikonfirmasi" });
            }
            else if (id == "ptp")
            {
                lstCheckIn.Add(new StandardCode() { StandardCodeID = "2", StandardCodeName = "Rencana Pulang" });
            }
            lstRegistrationStatus.Add(new StandardCode() { StandardCodeID = "1", StandardCodeName = "Open" });
            lstRegistrationStatus.Add(new StandardCode() { StandardCodeID = "2", StandardCodeName = "Close" });
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

        private string GetFilterExpression()
        {
            string filterExpression = "";
            /////string id = Page.Request.QueryString["id"];

            if (cboCheckinStatus.Value.ToString() == "1")
            {
                filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')", 
                    Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.OPEN);
            }
            else if (cboCheckinStatus.Value.ToString() == "2")
            {
                filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}') AND IsPlanDischarge = 1", 
                    Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.OPEN);
            }
            else if (cboCheckinStatus.Value.ToString() == "3")
            {
                if (hdnIsUsedReopenBilling.Value == "1")
                {
                    if (chkIsIncludeReopenBilling.Checked)
                    {
                        filterExpression += string.Format("GCVisitStatus IN ('{0}','{1}') AND IsBillingReopen = 1", Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED);
                    }
                    else
                    {
                        filterExpression += string.Format("GCVisitStatus = '{0}'", Constant.VisitStatus.DISCHARGED);
                    }
                }
                else
                {
                    filterExpression += string.Format("GCVisitStatus = '{0}'", Constant.VisitStatus.DISCHARGED);
                }
            }
            else
            {
                filterExpression += string.Format("GCVisitStatus = '{0}'", Constant.VisitStatus.OPEN);
            }

            if (cboServiceUnit.Value.ToString() != "0")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            else
                filterExpression += string.Format(" AND HealthcareServiceUnitID IN ({0})", hdnLstHealthcareServiceUnitID.Value); 

            if (txtPhysicianCode.Text != "")
            {
                filterExpression += string.Format(" AND (ParamedicID = {0} OR {0} IN (SELECT pt.ParamedicID FROM ParamedicTeam pt WITH(NOLOCK) WHERE pt.RegistrationID = vConsultVisit9.RegistrationID AND pt.IsDeleted = 0))", hdnPhysicianID.Value);
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            if (hdnSearchBarcodeNoRMRegistration.Value != "" && hdnSearchBarcodeNoRMRegistration.Value != null && hdnSearchBarcodeNoRMRegistration.Value != "00-00-00-00")
            {
                if (hdnSearchBarcodeNoRMRegistration.Value.Length > 11)
                {
                    filterExpression += string.Format(" AND RegistrationNo = '{0}'", hdnSearchBarcodeNoRMRegistration.Value);
                }
                else
                {
                    filterExpression += string.Format(" AND MedicalNo = '{0}'", hdnSearchBarcodeNoRMRegistration.Value);
                }
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisit9RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST_2);
            }

            List<vConsultVisit9> lstEntity = BusinessLayer.GetvConsultVisit9List(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST_2, pageIndex, "BedCode");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vConsultVisit9 entity = e.Item.DataItem as vConsultVisit9;
                Healthcare entityH = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);

                HtmlGenericControl divBusinessPartnerName = e.Item.FindControl("divBusinessPartnerName") as HtmlGenericControl;

                if (entity.CoverageTypeID != null && entity.CoverageTypeID != 0)
                {
                    if (entity.CoverageTypeCode == "R001" && entityH.Initial == "NHS")
                    {
                        divBusinessPartnerName.InnerHtml = string.Format("{0} ({1})", entity.BusinessPartnerName, entity.CoverageTypeName);
                    }
                    else
                    {
                        divBusinessPartnerName.InnerHtml = string.Format("{0}", entity.BusinessPartnerName);
                    }
                }
                else
                {
                    divBusinessPartnerName.InnerHtml = string.Format("{0}", entity.BusinessPartnerName);
                }
            }
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnTransactionNo.Value != "")
            {
                vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", hdnTransactionNo.Value))[0];
                Response.Redirect(GetResponseRedirectUrl(entity));
            }
        }

        private string GetResponseRedirectUrl(vConsultVisit4 entity)
        {
            string url = "";
            string id = Page.Request.QueryString["id"];

            LastContentVisitListIP lc = new LastContentVisitListIP()
            {
                HealthcareServiceUnitID = entity.HealthcareServiceUnitID,
                ParamedicID = !string.IsNullOrEmpty(hdnPhysicianID.Value) ? Convert.ToInt32(hdnPhysicianID.Value) : 0,
                QuickText = txtSearchView.Text,
                QuickFilterExpression = hdnFilterExpressionQuickSearch.Value,
                Status = cboCheckinStatus.Value.ToString(),
                MedicalNo = string.Empty //txtBarcodeEntry.Text
            };
            AppSession.LastContentVisitListIP = lc;

            string paramValue = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_TRANSAKSI_HANYA_DAPAT_DIUBAH_UNIT_ASAL).ParameterValue;
            AppSession.IsAdminCanCancelAllTransaction = paramValue == "0" ? true : false;

            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.PatientName = entity.PatientName;
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

            AppSession.HealthcareServiceUnitID = cboServiceUnit.Value.ToString();

            if (id == "bs" || id == "ptp")
            {
                string parentCode = "";

                if (id == "bs")
                {
                    parentCode = Constant.MenuCode.Inpatient.BILL_SUMMARY;
                    string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
                    List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.INPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                    int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

                    filterExpression = string.Format("ParentID = {0}", parentID);
                    lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.INPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                    GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                    url = Page.ResolveUrl(menu.MenuUrl);
                }
                else
                {
                    parentCode = Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_PAGE;
                    string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
                    List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.INPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                    int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

                    filterExpression = string.Format("ParentID = {0}", parentID);
                    lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.INPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                    GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                    url = Page.ResolveUrl(menu.MenuUrl);
                }
            }
            else if (id == "ct")
            {
                url = string.Format("~/Libs/Program/Module/PatientManagement/ChangePatientTransactionStatus/ChangePatientTransactionStatusEntry.aspx?id={0}|{1}", cboServiceUnit.Value, entity.VisitID);
            }
            else
            {
                url = string.Format("~/Program/TransactionVerification/TransactionVerificationDetail.aspx?id={0}", entity.VisitID);
            }
            return url;
        }

        protected void cbpBarcodeEntryProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression = GetFilterExpression();
            string param = txtBarcodeEntry.Text;

            filterExpression = Methods.GenerateFilterExpFromPatientListBarcode(filterExpression, param);
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(filterExpression).FirstOrDefault();

            string url = "";
            if (entity != null)
            {
                url = GetResponseRedirectUrl(entity);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpUrl"] = url;
        }
    }
}