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

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class ControlOrderList : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalCheckup.CONTROL_ORDER;
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
                Healthcare oHc = BusinessLayer.GetHealthcareList(string.Format("HealthcareID='{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                if (oHc != null) {
                    hdnHealthcareInitial.Value = oHc.Initial;
                }

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')",
                                                                                    Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID,
                                                                                    Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID,
                                                                                    Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL
                                                                                ));
                hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;

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
                                                                        Constant.VisitStatus.DISCHARGED,
                                                                        Constant.VisitStatus.CANCELLED,
                                                                        Constant.VisitStatus.CLOSED
                                                                    );
            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }
            filterExpression += " ORDER BY RegistrationID, IsMainPackage DESC, ID";

            List<vConsultVisitMCUItem> lstEntity = BusinessLayer.GetvConsultVisitMCUItemList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}