using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class RecipeHistoryDetailCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            hdnPrescriptionOrderID.Value = param;
            vPrescriptionOrderHd entityHd = BusinessLayer.GetvPrescriptionOrderHdList(String.Format("PrescriptionOrderID = {0}",param))[0];

            txtMedicalNo.Text = entityHd.MedicalNo;
            txtPatientName.Text = entityHd.PatientName;
            txtRegistrationNo.Text = entityHd.RegistrationNo;
            txtPrescriptionDate.Text = entityHd.PrescriptionDateInString;
            txtPhysicianName.Text = entityHd.ParamedicName;
            txtCreatedBy.Text = entityHd.CreatedByName;
            txtRemarksCtl.Text = entityHd.ChargesRemark;
            BindGridView(1, true, ref PageCount);
        }

        protected string GetFilterExpression() 
        {
            string filterExpression = string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND ChargesIsDeleted = 0 AND OrderIsDeleted = 0", hdnPrescriptionOrderID.Value);

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPrescriptionOrderDt1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vPrescriptionOrderDt1> lstPrescriptionOrderDt = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex);
            grdPopupView.DataSource = lstPrescriptionOrderDt;
            grdPopupView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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