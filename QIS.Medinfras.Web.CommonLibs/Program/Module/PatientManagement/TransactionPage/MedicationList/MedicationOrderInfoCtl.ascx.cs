using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using Newtonsoft.Json;
using System.Text;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MedicationOrderInfoCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            this.PopupTitle = "Medication Order";
            string[] paramInfo = param.Split('|');
            hdnItemID.Value = paramInfo[0];            
            hdnItemName.Value = paramInfo[1];

            txtItemName.Text = hdnItemName.Value;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnItemID.Value != "")
            {
                filterExpression = string.Format("VisitID = {0} AND ItemID = {1} AND OrderIsDeleted = 0",AppSession.RegisteredPatient.VisitID, hdnItemID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPrescriptionOrderDt5RowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, 6);
                }
            }
            List<vPrescriptionOrderDt5> lstEntity = BusinessLayer.GetvPrescriptionOrderDt5List(filterExpression, 6, pageIndex, "PrescriptionOrderDetailID DESC");
            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPrescriptionOrderDt5 entity = e.Item.DataItem as vPrescriptionOrderDt5;
                if (entity.IsUsingUDD)
                {
                    HtmlImage imgStatusImageUri = (HtmlImage)e.Item.FindControl("imgStatusImageUri");
                    if (Convert.ToDecimal(entity.cfRemainingQuantity) > 0)
                        imgStatusImageUri.Src = ResolveUrl(string.Format("~/Libs/Images/Status/udd_wip.png"));
                    else
                        imgStatusImageUri.Src = ResolveUrl(string.Format("~/Libs/Images/Status/udd_finish.png"));
                }
            }
        }

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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