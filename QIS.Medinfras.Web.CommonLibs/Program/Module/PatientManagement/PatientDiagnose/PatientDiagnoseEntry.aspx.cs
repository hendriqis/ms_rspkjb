using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Outpatient.Program
{
    public partial class PatientDiagnoseEntry : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.DIAGNOSE_ENTRY;
                default: return Constant.MenuCode.Outpatient.DIAGNOSE_ENTRY;
            }
        }
        protected int PageCount = 1;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                hdnDepartmentID.Value = Page.Request.QueryString["id"];
                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, hdnDepartmentID.Value, "IsUsingRegistration = 1");
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                    trServiceUnit.Style.Add("display", "none");

                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                Helper.SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");
                Helper.SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, true), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                BindGridView(1, true, ref PageCount);
            }
        }

        protected string GetFilterExpression()
        {
            string filterExpression = string.Format("VisitDate = '{0}' AND GCVisitStatus NOT IN ('{1}')", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            
            if (txtPhysicianCode.Text != "")
                filterExpression += string.Format(" AND ParamedicID = {0}", hdnPhysicianID.Value);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            return filterExpression;
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

        List<PatientDiagnosis> lstPatientDiagnosis = null;
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex);
            if (lstEntity.Count > 0)
            {
                StringBuilder sbVisitID = new StringBuilder();
                foreach (vConsultVisit entity in lstEntity)
                {
                    if (sbVisitID.ToString() != "")
                        sbVisitID.Append(",");
                    sbVisitID.Append(entity.VisitID.ToString());
                }
                lstPatientDiagnosis = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID IN ({0}) AND GCDiagnoseType = '{1}' AND IsDeleted = 0", sbVisitID.ToString(), Constant.DiagnoseType.MAIN_DIAGNOSIS));
            }
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vConsultVisit entity = e.Row.DataItem as vConsultVisit;
                PatientDiagnosis diagnosis = lstPatientDiagnosis.FirstOrDefault(p => p.VisitID == entity.VisitID);
                if (diagnosis != null)
                {
                    HtmlInputText txtDiagnose = e.Row.FindControl("txtDiagnose") as HtmlInputText;
                    txtDiagnose.Value = diagnosis.DiagnosisText;
                }
            }
        }

        protected void cbpSaveDiagnose_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            try
            {
                string[] param = e.Parameter.Split('|');
                int visitID = Convert.ToInt32(param[0]);
                String diagnoseText = param[1];
                int paramedicID = Convert.ToInt32(param[2]);

                PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", visitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();
                bool IsAdd = false;
                if (entity == null)
                {
                    entity = new PatientDiagnosis();
                    IsAdd = true;
                    entity.VisitID = visitID;
                    entity.ParamedicID = paramedicID;
                    entity.DifferentialDate = DateTime.Now;
                    entity.DifferentialTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    entity.GCDiagnoseType = Constant.DiagnoseType.MAIN_DIAGNOSIS;
                    entity.GCDifferentialStatus = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
                }
                entity.DiagnosisText = diagnoseText;
                if (IsAdd)
                {
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.InsertPatientDiagnosis(entity);
                }
                else
                {
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePatientDiagnosis(entity);
                }
                result = "success";
            }
            catch (Exception ex)
            {
                result = string.Format("fail|{0}", ex.Message);
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}