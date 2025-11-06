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
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ItemServiceItemCostEntryCtl : BaseEntryPopupCtl
    {
        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            hdnItemCostID.Value = param;

            vItemCost entity = BusinessLayer.GetvItemCostList(string.Format("ItemCostID = {0}", hdnItemCostID.Value))[0];
            txtItemName.Text = entity.ItemName1;
            txtHealthcareName.Text = entity.HealthcareName;

            EntityToControl(entity);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtMaterialCurrent, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBurdenCurrent, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtLaborCurrent, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSubContractCurrent, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtOverheadCurrent, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(vItemCost entity)
        {
            txtBurdenPrev.Text = entity.PreviousBurden.ToString();
            txtBurdenCurrent.Text = entity.CurrentBurden.ToString();
            txtBurdenTotal.Text = entity.TotalBurden.ToString();

            txtMaterialPrev.Text = entity.PreviousMaterial.ToString();
            txtMaterialCurrent.Text = entity.CurrentMaterial.ToString();
            txtMaterialTotal.Text = entity.TotalMaterial.ToString();

            txtLaborPrev.Text = entity.PreviousLabor.ToString();
            txtLaborCurrent.Text = entity.CurrentLabor.ToString();
            txtLaborTotal.Text = entity.TotalLabor.ToString();

            txtSubContractPrev.Text = entity.PreviousSubContract.ToString();
            txtSubContractCurrent.Text = entity.CurrentSubContract.ToString();
            txtSubContractTotal.Text = entity.TotalSubContract.ToString();

            txtOverheadPrev.Text = entity.PreviousOverhead.ToString();
            txtOverheadCurrent.Text = entity.CurrentOverhead.ToString();
            txtOverheadTotal.Text = entity.TotalOverhead.ToString();
        }

        private void ControlToEntity(ItemCost entity)
        {
            entity.CurrentBurden = Convert.ToDecimal(txtBurdenCurrent.Text);
            entity.CurrentMaterial = Convert.ToDecimal(txtMaterialCurrent.Text);
            entity.CurrentLabor = Convert.ToDecimal(txtLaborCurrent.Text);
            entity.CurrentSubContract = Convert.ToDecimal(txtSubContractCurrent.Text);
            entity.CurrentOverhead = Convert.ToDecimal(txtOverheadCurrent.Text);
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                ItemCost entity = BusinessLayer.GetItemCost(Convert.ToInt32(hdnItemCostID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemCost(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}