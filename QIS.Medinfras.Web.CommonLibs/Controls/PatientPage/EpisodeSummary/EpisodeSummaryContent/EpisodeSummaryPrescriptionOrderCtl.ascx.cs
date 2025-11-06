using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class EpisodeSummaryPrescriptionOrderCtl : BaseViewPopupCtl
    {
        private List<vPrescriptionOrderDt1> lstPrescriptionOrderDt = null;
        public override void InitializeDataControl(string queryString)
        {
            string filterExpression = string.Format("VisitID = {0} AND OrderIsDeleted = 0", queryString);
            lstPrescriptionOrderDt = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);

            filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", queryString, Constant.TransactionStatus.VOID);
            rptPrescriptionOrder.DataSource = BusinessLayer.GetvPrescriptionOrderHdList(filterExpression);
            rptPrescriptionOrder.DataBind();
        }

        protected void rptPrescriptionOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vPrescriptionOrderHd obj = (vPrescriptionOrderHd)e.Item.DataItem;

                Repeater rptPrescriptionOrderDtCustom = (Repeater)e.Item.FindControl("rptPrescriptionOrderDtCustom");
                rptPrescriptionOrderDtCustom.DataSource = GetPrescriptionOrderData(obj.PrescriptionOrderID);
                rptPrescriptionOrderDtCustom.DataBind();
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                if (rptPrescriptionOrder.Items.Count < 1)
                {
                    HtmlGenericControl divRptEmpty = (HtmlGenericControl)e.Item.FindControl("divRptEmpty");
                    divRptEmpty.Style["display"] = "block";
                }
            }        
        }

        protected object GetPrescriptionOrderData(Int32 PrescriptionOrderID)
        {
            return lstPrescriptionOrderDt.Where(p => p.PrescriptionOrderID == PrescriptionOrderID).ToList();
        }
    }
}