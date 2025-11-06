using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Inventory.Program;
using System.Drawing;
using System.Globalization;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class StockDetailInfoDtCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        private StockDetailInfo DetailPage
        {
            get { return (StockDetailInfo)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            String[] lstParam = param.Split('|');
            hdnItemID.Value = lstParam[0];

            ItemMaster im = BusinessLayer.GetItemMaster(Convert.ToInt32(hdnItemID.Value));
            txtItemName.Text = string.Format("{0} - {1}", im.ItemCode, im.ItemName1);

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0",Constant.StandardCode.SHIFT));
            List<StandardCode> lstShift = lstSc.Where(p => p.ParentID == Constant.StandardCode.SHIFT).ToList();
            hdnListShift.Value = string.Join("|", lstShift.Select(p => string.Format("{0},{1}", p.StandardCodeID, p.TagProperty)).ToList());

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = DetailPage.OnGetFilterExpression();
            filterExpression += string.Format(" AND ItemID = {0}", hdnItemID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemMovementInformationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vItemMovementInformation> lstDistributionDt = BusinessLayer.GetvItemMovementInformationList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "MovementID");
            grdPopupView.DataSource = lstDistributionDt;
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

        protected void grdPopupView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemMovementInformation oData = e.Row.DataItem as vItemMovementInformation;
                string _shift = GetShiftValue(oData.CreatedDate);
                //e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='LightBlue'");
                switch (_shift)
                {
                    case Constant.Shift.PAGI:
                        //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='WhiteSmoke'");
                        e.Row.BackColor = Color.WhiteSmoke;
                        break;
                    case Constant.Shift.SIANG:
                        //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='LightYellow'");
                        e.Row.BackColor = Color.LightYellow;
                        break;
                    case Constant.Shift.MALAM:
                        //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='LightGray'");
                        e.Row.BackColor = Color.LightGray;
                        break;
                    default:
                        e.Row.BackColor = Color.WhiteSmoke;
                        break;
                }
            } 
        }

        private string GetShiftValue(DateTime transDate)
        {
            string result = string.Empty;
            string _time = transDate.ToString(Constant.FormatString.TIME_FORMAT);
            string[] arrListShift = hdnListShift.Value.Split('|');
            
            string[] morning = arrListShift[0].Split(',');
            string morningStart = morning[1].Split(';')[0];
            string morningEnd = morning[1].Split(';')[1];

            string[] evening = arrListShift[1].Split(',');
            string eveningStart = evening[1].Split(';')[0];
            string eveningEnd = evening[1].Split(';')[1];

            string[] night = arrListShift[2].Split(',');
            string nightStart = night[1].Split(';')[0];
            string nightEnd = night[1].Split(';')[1];

            if (string.Compare(morningStart, _time) <= 0 && string.Compare(morningEnd, _time) >= 0)
                result = morning[0];
            else if (string.Compare(eveningStart, _time) <= 0 && string.Compare(eveningEnd, _time) >= 0)
                result = evening[0];
            else
                result = night[0];

            return result;
        }
    }
}