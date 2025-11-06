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
    public partial class CSSDServiceDetailListCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnRequestID.Value = param;
            vMDServiceRequestHd entity = BusinessLayer.GetvMDServiceRequestHdList(string.Format("RequestID = {0}", param)).FirstOrDefault();
            EntityToControl(entity);
            BindGridView(1, true, ref PageCount);
        }

        private void EntityToControl(vMDServiceRequestHd entity)
        {
            txtRequestNo.Text = entity.RequestNo;
            txtFromLocationName.Text = string.Format("{0} ({1})", entity.FromLocationName, entity.FromLocationCode);
            txtToLocationName.Text = string.Format("{0} ({1})", entity.ToLocationName, entity.ToLocationCode);
            txtPackageName.Text = entity.PackageName;
            txtNotes.Text = entity.Remarks;

            if (entity.SterilitationExpiredDate != null && entity.SterilitationExpiredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != "01-01-1900")
            {
                txtSterilitationExpired.Text = entity.SterilitationExpiredDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
            if (entity.SterilitationDate != null && entity.SterilitationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != "01-01-1900")
            {
                txtSterilitationDate.Text = entity.SterilitationDate.ToString(Constant.FormatString.DATE_FORMAT);
                txtSterilitationTime.Text = entity.SterilitationDate.ToString(Constant.FormatString.TIME_FORMAT);
            }
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
            if (hdnRequestID.Value != "")
                filterExpression = string.Format("RequestID = {0} AND IsDeleted = 0", hdnRequestID.Value);
            
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvMDServiceRequestDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }
            List<vMDServiceRequestDt> lstEntity = BusinessLayer.GetvMDServiceRequestDtList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "IsConsumption, ItemName1 ASC");
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