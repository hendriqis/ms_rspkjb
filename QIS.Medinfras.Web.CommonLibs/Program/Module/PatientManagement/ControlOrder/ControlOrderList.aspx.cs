using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ControlOrderList : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.CONTROL_ORDER_AIO;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.CONTROL_ORDER_AIO;
                case Constant.Facility.IMAGING: return Constant.MenuCode.Imaging.CONTROL_ORDER_AIO;
                case Constant.Facility.LABORATORY: return Constant.MenuCode.Laboratory.CONTROL_ORDER_AIO;
                case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.CONTROL_ORDER_AIO;
                default: return Constant.MenuCode.Outpatient.CONTROL_ORDER_AIO;
            }
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
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}')",
                                                                                    Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL
                                                                                ));

                string filterSetvar = string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM);
                List<SettingParameterDt> lstSetvar = BusinessLayer.GetSettingParameterDtList(filterSetvar);
                hdnImagingHealthcareServiceUnitID.Value = lstSetvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
                hdnLaboratoryHealthcareServiceUnitID.Value = lstSetvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;

                refreshGridInterval = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                BindGridView();

                Helper.SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");                
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("VisitDate = '{0}' AND GCItemDetailStatus = '{1}' AND GCRegistrationStatus NOT IN ('{2}','{3}','{4}','{5}')",
                                                        Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                                                        Constant.TransactionStatus.PROCESSED,
                                                        Constant.VisitStatus.OPEN,
                                                        Constant.VisitStatus.CANCELLED,
                                                        Constant.VisitStatus.DISCHARGED,
                                                        Constant.VisitStatus.CLOSED
                                                    );
            if (Page.Request.QueryString["id"] == Constant.Facility.OUTPATIENT)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.OUTPATIENT);
            }
            else if (Page.Request.QueryString["id"] == Constant.Facility.INPATIENT)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.INPATIENT);
            }
            else if (Page.Request.QueryString["id"] == Constant.Facility.IMAGING)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID = '{1}'", Constant.Facility.DIAGNOSTIC, hdnImagingHealthcareServiceUnitID.Value);
            }
            else if (Page.Request.QueryString["id"] == Constant.Facility.LABORATORY)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID = '{1}'", Constant.Facility.DIAGNOSTIC, hdnLaboratoryHealthcareServiceUnitID.Value);
            }
            else if (Page.Request.QueryString["id"] == Constant.Facility.DIAGNOSTIC)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ('{1}','{2}')", Constant.Facility.DIAGNOSTIC, hdnImagingHealthcareServiceUnitID.Value, hdnLaboratoryHealthcareServiceUnitID.Value);
            }
            else if (Page.Request.QueryString["id"] == Constant.Facility.PHARMACY)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.PHARMACY);
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }
            filterExpression += " ORDER BY RegistrationID, ID";

            List<vConsultVisitAIOItem> lstEntity = BusinessLayer.GetvConsultVisitAIOItemList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}