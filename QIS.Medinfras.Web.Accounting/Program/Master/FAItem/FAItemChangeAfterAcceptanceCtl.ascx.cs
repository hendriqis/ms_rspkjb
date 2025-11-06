using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FAItemChangeAfterAcceptanceCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            IsAdd = false;
            hdnFixedAssetIDCtl.Value = param;

            FAItem entity = BusinessLayer.GetFAItem(Convert.ToInt32(hdnFixedAssetIDCtl.Value));
            EntityToControl(entity);
        }
        
        private void EntityToControl(FAItem entity)
        {
            txtFixedAssetCodeCtl.Text = entity.FixedAssetCode;
            txtFixedAssetNameCtl.Text = entity.FixedAssetName;
            txtSerialNumberCtl.Text = entity.SerialNumber;
            txtBudgetPlanNoCtl.Text = entity.BudgetPlanNo;
            txtRemarksCtl.Text = entity.Remarks;
            txtPurchaseRequestNumberCtl.Text = entity.NoPPB;

            string filterFAAcceptance = string.Format("FixedAssetID = {0} AND IsDeleted = 0 AND GCTransactionStatus != '{1}'", entity.FixedAssetID, Constant.TransactionStatus.VOID);
            List<vFAAcceptanceDt> faAcceptanceLst = BusinessLayer.GetvFAAcceptanceDtList(filterFAAcceptance);
            if (faAcceptanceLst.Count() > 0)
            {
                vFAAcceptanceDt faAcceptance = faAcceptanceLst.FirstOrDefault();
                txtFAAcceptanceNoCtl.Text = faAcceptance.FAAcceptanceNo;
            }

        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAItemDao entityDao = new FAItemDao(ctx);

            try
            {
                FAItem entity = entityDao.Get(Convert.ToInt32(hdnFixedAssetIDCtl.Value));
                entity.FixedAssetName = txtFixedAssetNameCtl.Text;
                entity.SerialNumber = txtSerialNumberCtl.Text;
                entity.BudgetPlanNo = txtBudgetPlanNoCtl.Text;
                entity.Remarks = txtRemarksCtl.Text;
                entity.NoPPB = txtPurchaseRequestNumberCtl.Text;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                retval = entity.FixedAssetID.ToString();
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}