using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class TransactionOrderDetailListCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] parameter = param.Split('|');
            hdnOrderID.Value = parameter[0].ToString();
            hdnOrderTypeID.Value = parameter[1].ToString();
            string filterExpression = string.Format("OrderID = {0} AND OrderType = '{1}'", hdnOrderID.Value, hdnOrderTypeID.Value);
            vPatientOrderAll entity = BusinessLayer.GetvPatientOrderAllList(filterExpression).FirstOrDefault();
            txtTestOrderHdNo.Text = entity.OrderNo;
            txtServiceUnitName.Text = entity.ServiceUnitName;

            BindGridView();

        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnOrderID.Value != "")
                filterExpression = string.Format("OrderID = {0} AND OrderType = '{1}' AND IsDeleted = 0 ORDER BY ID DESC", hdnOrderID.Value ,hdnOrderTypeID.Value);
            List<vPatientOrderAllDt> lstEntity = BusinessLayer.GetvPatientOrderAllDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
            
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            result += "success";

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}