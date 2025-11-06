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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientDataList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                //case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.REGISTRATION;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.MEDICAL_RECORD;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.MEDICAL_RECORD;
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.MEDICAL_RECORD;
                case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.MEDICAL_RECORD;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.MEDICAL_RECORD;
                default: return Constant.MenuCode.EMR.PATIENT_DATA;
            }
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnDepartmentID.Value = Page.Request.QueryString["id"];
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvPatientRowIndex(filterExpression, keyValue, "PatientName ASC") + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Name", "Address", "Medical No", "Old Medical No" };
            fieldListValue = new string[] { "PatientName","StreetName County District City State ZipCode", "MedicalNo", "OldMedicalNo" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += "IsDeleted = 0";
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatient> lstEntity = BusinessLayer.GetvPatientList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PatientName ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
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

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnID.Value.ToString() != "")
            {
                Patient entity = BusinessLayer.GetPatient(Convert.ToInt32(hdnID.Value));
                PatientDetail pt = new PatientDetail();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
                AppSession.PatientDetail = pt;

                string queryID = "";
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: queryID = "INPATIENT"; break;
                    case Constant.Facility.EMERGENCY: queryID = "EMERGENCY"; break;
                    case Constant.Facility.OUTPATIENT: queryID = "OUTPATIENT"; break;
                    default:
                        queryID = "EMR";break;
                }
                Response.Redirect("~/Libs/Program/Module/MedicalRecord/PatientEMRView.aspx");
            }
        }
    }
}