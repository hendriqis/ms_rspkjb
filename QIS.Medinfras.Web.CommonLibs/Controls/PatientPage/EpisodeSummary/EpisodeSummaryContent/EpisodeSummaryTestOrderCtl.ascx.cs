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
    public partial class EpisodeSummaryTestOrderCtl : BaseViewPopupCtl
    {
        private List<vTestOrderDt> ListvTestOrderDt = null;
        public override void InitializeDataControl(string queryString)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", queryString);
            ListvTestOrderDt = BusinessLayer.GetvTestOrderDtList(filterExpression);

            filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", queryString, Constant.TransactionStatus.VOID);
            rptTestOrder.DataSource = BusinessLayer.GetvTestOrderHdList(filterExpression);
            rptTestOrder.DataBind();
        }

        protected void rptTestOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vTestOrderHd obj = (vTestOrderHd)e.Item.DataItem;

                Repeater rptTestOrderDt = (Repeater)e.Item.FindControl("rptTestOrderDt");
                rptTestOrderDt.DataSource = GetTestOrderData(obj.TestOrderID);
                rptTestOrderDt.DataBind();
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                if (rptTestOrder.Items.Count < 1)
                {
                    HtmlGenericControl divRptEmpty = (HtmlGenericControl)e.Item.FindControl("divRptEmpty");
                    divRptEmpty.Style["display"] = "block";
                }
            }        
        }

        protected object GetTestOrderData(Int32 TestOrderID)
        {
            return ListvTestOrderDt.Where(p => p.TestOrderID == TestOrderID).ToList();
        }
    }
}