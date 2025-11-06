using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Inpatient.Controls;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class BedReservationInformation : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.BED_RESERVATION_INFORMATION;
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
                List<GetServiceUnitUserList> lstServiceUnit = null;
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
                txtReservationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.INPATIENT, "IsUsingRegistration = 1");
                lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                List<ClassCare> lstCC = BusinessLayer.GetClassCareList(string.Format("IsDeleted = 0"));
                lstCC.Insert(0, new ClassCare { ClassID = 0, ClassName = "" });
                Methods.SetComboBoxField<ClassCare>(cboClassCare, lstCC, "ClassName", "ClassID");
                cboClassCare.SelectedIndex = 0;

                string filterSU2 = string.Format("DepartmentID = '{0}' AND IsDeleted = 0", Constant.Facility.INPATIENT);
                List<vHealthcareServiceUnit> lstWard2 = BusinessLayer.GetvHealthcareServiceUnitList(filterSU2);
                lstWard2.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboBedPicksWardSum, lstWard2, "ServiceUnitName", "HealthcareServiceUnitID");
                cboBedPicksWardSum.SelectedIndex = 0;

                string filterCL2 = string.Format("IsDeleted = 0");
                List<ClassCare> lstCC2 = BusinessLayer.GetClassCareList(filterCL2);
                lstCC2.Insert(0, new ClassCare { ClassID = 0, ClassName = "" });
                Methods.SetComboBoxField<ClassCare>(cboClassPicksSum, lstCC2, "ClassName", "ClassID");
                cboClassPicksSum.SelectedIndex = 0;

                trClassSum.Attributes.Add("style", "display:none");

                BindingViewSummary();

                ((GridBedReservationCtl)grdInpatientReservation).InitializeControl();
            }
        }

        protected void cbpViewSum_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindingViewSummary();
                result = "refresh|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindingViewSummary()
        {
            string filterParam = "";
            string cboBedWard2 = "";
            string cboClass2 = "";
            cboBedWard2 = cboBedPicksWardSum.Value.ToString();
            cboClass2 = cboClassPicksSum.Value.ToString();
            if (rblFilterSum.SelectedValue == "filterClassSum")
            {
                if (cboClass2 != "0")
                {
                    filterParam = string.Format("ClassID = {0}", cboClassPicksSum.Value);
                }
                else
                {
                    filterParam = string.Format("");
                }
            }
            else
            {
                if (cboBedWard2 != "0")
                {
                    filterParam = string.Format("HealthcareServiceUnitID = {0}", cboBedPicksWardSum.Value);
                }
                else
                {
                    filterParam = string.Format("");
                }
            }

            List<vBedInformationSummary> lstSum = BusinessLayer.GetvBedInformationSummaryList(filterParam, Constant.GridViewPageSize.GRID_TEMP_MAX_5000, 1, "RoomCode ASC");
            lvwView.DataSource = lstSum;
            lvwView.DataBind();
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("GCReservationStatus != '{0}' AND ReservationDate = '{1}'", Constant.Bed_Reservation_Status.CANCELLED, Helper.GetDatePickerValue(txtReservationDate).ToString(Constant.FormatString.DATE_FORMAT_112));

            //Cek Combo Box Service Unit
            if (cboServiceUnit.Value != null && cboServiceUnit.Value.ToString() != "0")
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            }

            if (cboClassCare.Value != null && cboClassCare.Value.ToString() != "0")
            {
                filterExpression += string.Format(" AND ClassID = {0}", cboClassCare.Value);
            }

            //Cek Quick Search
            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            return filterExpression;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }
    }
}