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
    public partial class RevenueSharingEntryCtl : BaseViewPopupCtl
    {
        List<SettingParameter> lstSettingParameter = null;

        public override void InitializeDataControl(string param)
        {
            hdnTransactionID.Value = param;

            vPatientChargesDt entity = BusinessLayer.GetvPatientChargesDtList(string.Format("ID = {0}", hdnTransactionID.Value))[0];
            txtTransactionNo.Text = entity.TransactionNo;
            txtPatientName.Text = entity.PatientName;

            lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')",
            Constant.SettingParameter.TARIFF_COMPONENT1_TEXT, Constant.SettingParameter.TARIFF_COMPONENT2_TEXT, Constant.SettingParameter.TARIFF_COMPONENT3_TEXT));

            BindGridView();
        }

        List<StandardCode> lstFormulaType = null;

        protected int GetFormulaTypeCount()
        {
            if (lstFormulaType != null)
                return lstFormulaType.Count;
            else return 0;
        }

        private void BindGridView()
        {
            int TransactionDtID = Convert.ToInt32(hdnTransactionID.Value);

            lstFormulaType = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID != '{1}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REVENUE_SHARING_COMPONENT, Constant.RevenueSharingComponent.PARAMEDIC));
            rptRevenueSharingDtHeader.DataSource = lstFormulaType;
            rptRevenueSharingDtHeader.DataBind();

            List<GetPatientChargesDtRevenueSharing> lstEntity = BusinessLayer.GetPatientChargesDtRevenueSharingList(TransactionDtID, AppSession.ParamedicID);

            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                GetPatientChargesDtRevenueSharing entity = e.Item.DataItem as GetPatientChargesDtRevenueSharing;

                List<decimal> lstRevenueSharing = entity.RevenueSharingData.Split('|').Select(x => Convert.ToDecimal(x)).ToList();
                lstRevenueSharing.RemoveAt(0);

                Repeater rptRevenueSharing = (Repeater)e.Item.FindControl("rptRevenueSharing");
                rptRevenueSharing.DataSource = lstRevenueSharing;
                rptRevenueSharing.DataBind();

                //if (entity.SharingAmount == 0) e.Item.Visible = false;
                //else 
                //{
                //    List<decimal> lstRevenueSharing = entity.RevenueSharingData.Split('|').Select(x => Convert.ToDecimal(x)).ToList();
                //    lstRevenueSharing.RemoveAt(0);

                //    Repeater rptRevenueSharing = (Repeater)e.Item.FindControl("rptRevenueSharing");
                //    rptRevenueSharing.DataSource = lstRevenueSharing;
                //    rptRevenueSharing.DataBind();
                //}
            }

        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected string GetTariffComponent1Text()
        {
            return lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT1_TEXT).ParameterValue;
        }

        protected string GetTariffComponent2Text()
        {
            return lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT2_TEXT).ParameterValue;
        }

        protected string GetTariffComponent3Text()
        {
            return lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT3_TEXT).ParameterValue;
        }
    }
}