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
using System.Text;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class VisitList : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.NUTRITION_PATIENT_PAGE;
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
                
                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID != '{0}' AND IsActive = 1 AND IsHasRegistration=1", Constant.Facility.PHARMACY));
                Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");
                cboPatientFrom.Value = Constant.Facility.INPATIENT;                
                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                List<StandardCode> lstCheckIn = new List<StandardCode>();
                List<StandardCode> lstRegistrationStatus = new List<StandardCode>();
                FillComboBoxForFilter(lstCheckIn, lstRegistrationStatus);
                Methods.SetComboBoxField<StandardCode>(cboCheckinStatus, lstCheckIn, "StandardCodeName", "StandardCodeID");
                cboCheckinStatus.SelectedIndex = 0;
                cboRegistrationStatus.SelectedIndex = 0;
                SettingControlProperties();
                Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false), "mpServiceUnit");

                BindGridView(1, true, ref PageCount);
                
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                txtBarcodeEntry.Focus();
            }
        }

        private void SettingControlProperties()
        {
        }

        private void FillComboBoxForFilter(List<StandardCode> lstCheckIn, List<StandardCode> lstRegistrationStatus)
        {
            lstCheckIn.Add(new StandardCode() { StandardCodeID = "1", StandardCodeName = "Masih Dirawat" });
            lstCheckIn.Add(new StandardCode() { StandardCodeID = "2", StandardCodeName = "Rencana Pulang" });
            lstCheckIn.Add(new StandardCode() { StandardCodeID = "3", StandardCodeName = "Sudah Pulang" });
            lstCheckIn.Add(new StandardCode() { StandardCodeID = "4", StandardCodeName = "Belum Dikonfirmasi" });

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
            string id = Page.Request.QueryString["id"];
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
                filterExpression += string.Format("GCVisitStatus = '{0}'", Constant.VisitStatus.DISCHARGED);
            }
            else
            {
                filterExpression += string.Format("GCVisitStatus = '{0}'", Constant.VisitStatus.OPEN);
            }

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            else if (cboPatientFrom.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFrom.Value);

            if (cboPatientFrom.Value != null && cboPatientFrom.Value.ToString() != Constant.Facility.INPATIENT)
            {
                filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisit19RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            List<vConsultVisit19> lstEntity = BusinessLayer.GetvConsultVisit19List(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex,"BedCode");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
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
                QuickText = txtSearchView.Text,
                QuickFilterExpression = hdnFilterExpressionQuickSearch.Value,
                Status = cboCheckinStatus.Value.ToString(),
                MedicalNo = string.Empty //txtBarcodeEntry.Text
            };
            AppSession.LastContentVisitListIP = lc;

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

            string parentCode = "";

            parentCode = Constant.MenuCode.Nutrition.NUTRITION_PATIENT_PAGE;
            string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
            List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.NUTRITION, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

            filterExpression = string.Format("ParentID = {0}", parentID);
            lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.NUTRITION, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
            url = Page.ResolveUrl(menu.MenuUrl);

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