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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NurseNotesEntry : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.NURSE_NOTES;
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

                String filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}'", AppSession.UserLogin.HealthcareID, Constant.Facility.INPATIENT);
                List<vHealthcareServiceUnit> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                lstHServiceUnit.Insert(0, new vHealthcareServiceUnit { ServiceUnitName = "", HealthcareServiceUnitID = 0 });
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                txtActualVisitDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                Helper.SetControlEntrySetting(txtActualVisitDate, new ControlEntrySetting(true, true, true), "mpPatientList");

                txtStartTime.Text = "00:00";
                txtEndTime.Text = "23:59";

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                BindGridView(1, true, ref PageCount);
            }
        }

        protected string GetFilterExpression()
        {
            string filterExpression = string.Format("ActualVisitDate = '{0}' AND GCRegistrationStatus NOT IN ('{1}')", Helper.GetDatePickerValue(txtActualVisitDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            filterExpression += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.INPATIENT);

            String HealthcareServiceUnitID = Convert.ToString(cboServiceUnit.Value);
            if (HealthcareServiceUnitID != "0")
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }
            filterExpression += string.Format(" AND StartServiceTime BETWEEN '{0}' AND '{1}' AND StartServiceDate IS NOT NULL", txtStartTime.Text, txtEndTime.Text);

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

        List<vPatientVisitNote> lstNotes = null;
        List<vVitalSignDt> lstTTV = null;
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            List<SettingParameter> lstSettingParameterDt = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.VITAL_SIGN_EWS));

            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRegistrationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }
            List<vRegistration> lstEntity = BusinessLayer.GetvRegistrationList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "StartServiceTime DESC");
            if (lstEntity.Count > 0)
            {
                StringBuilder sbVisitID = new StringBuilder();
                foreach (vRegistration entity in lstEntity)
                {
                    if (sbVisitID.ToString() != "")
                        sbVisitID.Append(",");
                    sbVisitID.Append(entity.VisitID.ToString());
                }
                lstNotes = BusinessLayer.GetvPatientVisitNoteList(string.Format("VisitID IN ({0}) AND GCPatientNoteType = '{1}' AND IsDeleted = 0", sbVisitID.ToString(), Constant.PatientVisitNotes.NURSE_NOTES));
                lstTTV = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) AND VitalSignID = (select VitalSignID from VitalSignType where VitalSignID = '{1}') AND IsDeleted = 0", sbVisitID.ToString(), lstSettingParameterDt.FirstOrDefault().ParameterValue));
            }

            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vRegistration entity = e.Row.DataItem as vRegistration;
                vPatientVisitNote note = lstNotes.FirstOrDefault(p => p.VisitID == entity.VisitID);
                vVitalSignDt ttv = lstTTV.FirstOrDefault(p => p.VisitID == entity.VisitID);
                if (note != null)
                {
                    HtmlInputText txtNotes = e.Row.FindControl("txtNotes") as HtmlInputText;
                    txtNotes.Value = note.NoteText;
                }
                if (ttv != null)
                {
                    HtmlInputText txtttv = e.Row.FindControl("txtEWS") as HtmlInputText;
                    txtttv.Value = ttv.VitalSignValue;
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
                String notes = param[1];

                PatientVisitNote entity = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", visitID, Constant.PatientVisitNotes.NURSE_NOTES)).FirstOrDefault();
                bool IsAdd = false;
                if (entity == null)
                {
                    IsAdd = true;
                    entity = new PatientVisitNote();
                    entity.VisitID = visitID;
                    entity.GCPatientNoteType = Constant.PatientVisitNotes.NURSE_NOTES;
                    entity.NoteDate = Convert.ToDateTime(DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT2));
                    entity.NoteTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                }
                entity.NoteText = notes;
                if (IsAdd)
                {
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.InsertPatientVisitNote(entity);
                }
                else
                {
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePatientVisitNote(entity);
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