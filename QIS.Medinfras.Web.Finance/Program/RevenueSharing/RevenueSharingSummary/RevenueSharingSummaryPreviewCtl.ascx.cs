using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingSummaryPreviewCtl : BaseViewPopupCtl
    {
        List<SettingParameter> lstSettingParameter = null;

        public override void InitializeDataControl(string param)
        {
            hdnRSSummaryID.Value = param;
            TransRevenueSharingSummaryHd entity = BusinessLayer.GetTransRevenueSharingSummaryHd(Convert.ToInt32(hdnRSSummaryID.Value));
            txtRSSummaryNo.Text = entity.RSSummaryNo.ToString();
            txtRSSummaryDate.Text = entity.RSSummaryDate.ToString(Constant.FormatString.DATE_FORMAT);

            BindGridView();
        }

        protected void cbpViewPopUpCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView() 
        {
            string filterExpression = string.Format("IsDeleted = 0 AND RSSummaryID = {0}", hdnRSSummaryID.Value);
            List<vTransRevenueSharingSummaryDt> lstEntity = BusinessLayer.GetvTransRevenueSharingSummaryDtList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            string filterExpression2, filterExpression3 = "";
            
            filterExpression2 = string.Format("IsDeleted = 0 AND RSSummaryID = {0} AND GCRSAdjustmentGroup = '{1}'", hdnRSSummaryID.Value, Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN);

            filterExpression3 = string.Format("IsDeleted = 0 AND RSSummaryID = {0} AND GCRSAdjustmentGroup = '{1}'", hdnRSSummaryID.Value, Constant.RevenueSharingAdjustmentGroup.PENGURANGAN);
            
            List<vTransRevenueSharingSummaryAdj> lstEntity2 = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterExpression2);
            grdView2.DataSource = lstEntity2;
            grdView2.DataBind();

            List<vTransRevenueSharingSummaryAdj> lstEntity3 = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterExpression3);
            grdView3.DataSource = lstEntity3;
            grdView3.DataBind();
        }
    }
}