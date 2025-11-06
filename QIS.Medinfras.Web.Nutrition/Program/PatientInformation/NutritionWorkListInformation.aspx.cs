using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class NutritionWorkListInformation : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.NUTRITION_WORKLIST_INFORMATION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        //protected String GetItemDetailStatus()
        //{
        //    return Constant.TransactionStatus.PROCESSED;
        //}

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.MEAL_TIME));
            Methods.SetComboBoxField<StandardCode>(cboMealTime, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboMealTime.SelectedIndex = 0;
            //hdnMealTime.Value = Convert.ToString(cboMealTime.Value);

            List<ServiceUnitMaster> lstServiceUnit = BusinessLayer.GetServiceUnitMasterList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<ServiceUnitMaster>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "ServiceUnitCode");
            cboServiceUnit.SelectedIndex = 0;
            //hdnServiceUnit.Value = Convert.ToString(cboServiceUnit.Value);

            txtDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            BindGridView();
        }
        
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            filterExpression = string.Format("ScheduleDate = '{0}' AND GCMealTime = '{1}' AND ServiceUnitCode = '{2}'", Helper.GetDatePickerValue(txtDate.Text).ToString("yyyyMMdd"), cboMealTime.Value, cboServiceUnit.Value);
            return filterExpression;
        }

        private void BindGridView()
        {
            List<vNutritionWorkListInformation> lstEntity = BusinessLayer.GetvNutritionWorkListInformationList(GetFilterExpression());
            if (lstEntity.Count != 0)
            {
                hdnMealTime.Value = Convert.ToString(cboMealTime.Value);
                hdnServiceUnit.Value = Convert.ToString(cboServiceUnit.Value);
            }
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}