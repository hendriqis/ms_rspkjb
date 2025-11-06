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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingVerificationEntryCtl : BaseViewPopupCtl
    {
        List<SettingParameter> lstSettingParameter = null;

        public override void InitializeDataControl(string param)
        {
            hdnRSTransactionID.Value = param;
            TransRevenueSharingHd entity = BusinessLayer.GetTransRevenueSharingHd(Convert.ToInt32(hdnRSTransactionID.Value));
            txtRevenueSharingNo.Text = entity.RevenueSharingNo.ToString();
            txtProcessDate.Text = entity.ProcessedDate.ToString(Constant.FormatString.DATE_FORMAT);

            BindGridView();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView() 
        {
            string filterExpression = string.Format("IsDeleted = 0 AND ParamedicID = {0} AND RSTransactionID = {1} AND RevenueSharingAmount > 0",AppSession.ParamedicID,hdnRSTransactionID.Value);
            List<vTransRevenueSharingDt> lstEntity = BusinessLayer.GetvTransRevenueSharingDtList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            string filterExpression2, filterExpression3 = "";
            
            filterExpression2 = string.Format("IsDeleted = 0 AND ParamedicID = {0} AND RSTransactionID = {1} AND GCRSAdjustmentGroup = '{2}'", AppSession.ParamedicID, hdnRSTransactionID.Value, Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN);

            filterExpression3 = string.Format("IsDeleted = 0 AND ParamedicID = {0} AND RSTransactionID = {1} AND GCRSAdjustmentGroup = '{2}'", AppSession.ParamedicID, hdnRSTransactionID.Value, Constant.RevenueSharingAdjustmentGroup.PENGURANGAN);
            
            List<vTransRevenueSharingAdj> lstEntity2 = BusinessLayer.GetvTransRevenueSharingAdjList(filterExpression2);
            grdView2.DataSource = lstEntity2;
            grdView2.DataBind();

            List<vTransRevenueSharingAdj> lstEntity3 = BusinessLayer.GetvTransRevenueSharingAdjList(filterExpression3);
            grdView3.DataSource = lstEntity3;
            grdView3.DataBind();
        }
    }
}