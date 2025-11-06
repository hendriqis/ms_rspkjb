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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ItemDistributionVerificationDetailCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnDistributionID.Value = param;
            vItemDistributionHd entity = BusinessLayer.GetvItemDistributionHdList(string.Format("DistributionID = {0}", param))[0];
            EntityToControl(entity);
            BindGridView(1, true, ref PageCount);
        }

        private void EntityToControl(vItemDistributionHd entity)
        {
            txtItemDistributionNo.Text = entity.DistributionNo;
            txtFromLocationName.Text = string.Format("{0} ({1})", entity.FromLocationName, entity.FromLocationCode);
            txtToLocationName.Text = string.Format("{0} ({1})", entity.ToLocationName, entity.ToLocationCode);
            txtNotes.Text = entity.DeliveryRemarks;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (grdView.Rows.Count < 1)
                BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnDistributionID.Value != "")
                filterExpression = string.Format("DistributionID = {0} AND IsDeleted = 0", hdnDistributionID.Value);
            
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemDistributionDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 15);
            }
            List<vItemDistributionDt> lstEntity = BusinessLayer.GetvItemDistributionDtList(filterExpression, 15, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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