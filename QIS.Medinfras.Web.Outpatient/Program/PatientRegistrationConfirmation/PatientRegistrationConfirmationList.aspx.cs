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
using DevExpress.Web.ASPxEditors;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Outpatient.Program
{
    public partial class PatientRegistrationConfirmationList : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Outpatient.PATIENT_REGISTRATION_CONFIRMATION;
        }

        #region Error Message
        protected string GetErrMessageSelectRegistrationFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_REGISTRATION_FIRST_VALIDATION);
        }
        #endregion

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

        protected int PageCount = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.OUTPATIENT, "");
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                BindGridView();

                Helper.SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");
                Helper.SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, true), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("VisitDate = '{0}' AND HealthcareServiceUnitID = {1} AND GCVisitStatus = '{2}'", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), cboServiceUnit.Value, Constant.VisitStatus.OPEN);
            if (txtPhysicianCode.Text != "")
                filterExpression += string.Format(" AND ParamedicID = {0}", hdnPhysicianID.Value);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpRegistrationConfirmation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
            string result = "";
            try
            {
                List<Registration> lstRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID IN (SELECT RegistrationID FROM ConsultVisit WHERE VisitID IN ({0}))", hdnSelectedVisit.Value), ctx);
                List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("VisitID IN ({0})", hdnSelectedVisit.Value), ctx);
                foreach (ConsultVisit consultVisit in lstConsultVisit)
                {
                    Registration registration = lstRegistration.FirstOrDefault(p => p.RegistrationID == consultVisit.RegistrationID);

                    consultVisit.GCVisitStatus = Constant.VisitStatus.CHECKED_IN;
                    consultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                    Helper.InsertAutoBillItem(ctx, consultVisit, Constant.Facility.OUTPATIENT, (int)consultVisit.ChargeClassID, registration.GCCustomerType, registration.IsPrintingPatientCard, 0);

                    if (registration.GCRegistrationStatus == Constant.VisitStatus.OPEN)
                    {
                        registration.GCRegistrationStatus = Constant.VisitStatus.CHECKED_IN;
                        registration.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityRegistrationDao.Update(registration);
                    }

                    entityConsultVisitDao.Update(consultVisit);
                }

                result = "success"; 
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = string.Format("fail|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}