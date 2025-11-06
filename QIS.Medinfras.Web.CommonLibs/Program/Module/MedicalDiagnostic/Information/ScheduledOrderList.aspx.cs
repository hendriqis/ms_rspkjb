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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ScheduledOrderList : BasePagePatientPageList
    {
        protected string laboratoryTransactionCode = "";
        public override string OnGetMenuCode()
        {
            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                return Constant.MenuCode.Imaging.HISTORY_INFORMATION;
            else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                return Constant.MenuCode.Laboratory.HISTORY_INFORMATION;
            else return Constant.MenuCode.MedicalDiagnostic.HISTORY_INFORMATION;
        }
        
        protected int PageCount = 1;
        protected override void InitializeDataControl()
        {
            laboratoryTransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.OtherMedicalDiagnostic)
            {
                string filterExpressionOrder = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID NOT IN ('{2}','{3}')", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
                List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(filterExpressionOrder);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboMedicSupport, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
                cboMedicSupport.SelectedIndex = 0;
            }
            else
                trServiceUnit.Style.Add("display", "none");

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";
            if (hdnMRN.Value == "")
                filterExpression = "1 = 0";
            else
            {
                int healthcareServiceUnitID = 0;
                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.OtherMedicalDiagnostic)
                    healthcareServiceUnitID = Convert.ToInt32(cboMedicSupport.Value);
                else
                    healthcareServiceUnitID = AppSession.MedicalDiagnostic.HealthcareServiceUnitID;
                filterExpression += string.Format("HealthcareServiceUnitID = {0}", healthcareServiceUnitID);

                if (hdnMRN.Value != "")
                    filterExpression += string.Format(" AND RegistrationID IN (SELECT RegistrationID FROM Registration WHERE MRN = {0})", hdnMRN.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vPatientChargesHd> lstEntity = BusinessLayer.GetvPatientChargesHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TransactionID DESC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
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
    }
}