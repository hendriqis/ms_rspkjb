using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class MappingTreasuryTransactionEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.TREASURY_TRANSACTION_MAPPING;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            
            List<TransactionType> lstTransactionType = BusinessLayer.GetTransactionTypeList("TransactionCode BETWEEN '7281' AND '7288'");
            Methods.SetComboBoxField<TransactionType>(cboTransactionCode, lstTransactionType, "TransactionName", "TransactionCode");

            List<StandardCode> lstTreasuryType = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND TagProperty = '1'", Constant.StandardCode.TREASURY_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboTreasuryType, lstTreasuryType, "StandardCodeName", "StandardCodeID");
            cboTreasuryType.SelectedIndex = 0;

            List<StandardCode> lstTreasuryGroup = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND TagProperty = '1'", Constant.StandardCode.TREASURY_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboTreasuryGroup, lstTreasuryGroup, "StandardCodeName", "StandardCodeID");
            cboTreasuryGroup.SelectedIndex = 0;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                List<vTreasuryTransactionMapping> lst = BusinessLayer.GetvTreasuryTransactionMappingList(String.Format("ID = {0}", Convert.ToInt32(ID)));
                if (lst.Count() > 0)
                {
                    vTreasuryTransactionMapping entity = lst.FirstOrDefault();
                    EntityToControl(entity); 
                }
            }
            else
            {
                IsAdd = true;
            }

            cboTransactionCode.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboTransactionCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboTreasuryType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboTreasuryGroup, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(lblGLAccountTreasury, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGLAccountTreasuryID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGLAccountCodeTreasury, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGLAccountNameTreasury, new ControlEntrySetting(false, false, true));
        }

        private void EntityToControl(vTreasuryTransactionMapping entity)
        {
            cboTransactionCode.Value = entity.TransactionCode;
            cboTreasuryType.Value = entity.GCTreasuryType;
            cboTreasuryGroup.Value = entity.GCTreasuryGroup;
            hdnGLAccountTreasuryID.Value = entity.GLAccountID.ToString();
            txtGLAccountCodeTreasury.Text = entity.GLAccountNo;
            txtGLAccountNameTreasury.Text = entity.GLAccountName;
        }

        private void ControlToEntity(TreasuryTransactionMapping entity)
        {
            entity.TransactionCode = cboTransactionCode.Value.ToString();
            entity.GCTreasuryType = cboTreasuryType.Value.ToString();
            entity.GCTreasuryGroup = cboTreasuryGroup.Value.ToString();
            entity.GLAccountID = Convert.ToInt32(hdnGLAccountTreasuryID.Value);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string oTransactionCode = cboTransactionCode.Value.ToString();
            string oGCTreasuryType = cboTreasuryType.Value.ToString();
            string oGCTreasuryGroup = cboTreasuryGroup.Value.ToString();
            string oGLAccountID = hdnGLAccountTreasuryID.Value;

            string FilterExpression = string.Format("TransactionCode = '{0}' AND GCTreasuryType = '{1}' AND GCTreasuryGroup = '{2}' AND GLAccountID = {3} AND IsDeleted = 0",
                                                        oTransactionCode, oGCTreasuryType, oGCTreasuryGroup, oGLAccountID);
            List<TreasuryTransactionMapping> lst = BusinessLayer.GetTreasuryTransactionMappingList(FilterExpression);
            if (lst.Count > 0)
                errMessage = "Data dengan kombinasi isian ini sudah tersedia";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string oTransactionCode = cboTransactionCode.Value.ToString();
            string oGCTreasuryType = cboTreasuryType.Value.ToString();
            string oGCTreasuryGroup = cboTreasuryGroup.Value.ToString();
            string oGLAccountID = hdnGLAccountTreasuryID.Value;

            string FilterExpression = string.Format("TransactionCode = '{0}' AND GCTreasuryType = '{1}' AND GCTreasuryGroup = '{2}' AND GLAccountID = {3} AND IsDeleted = 0",
                                                        oTransactionCode, oGCTreasuryType, oGCTreasuryGroup, oGLAccountID);
            List<TreasuryTransactionMapping> lst = BusinessLayer.GetTreasuryTransactionMappingList(FilterExpression);
            if (lst.Count > 0)
                errMessage = "Data dengan kombinasi isian ini sudah tersedia";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TreasuryTransactionMappingDao entityDao = new TreasuryTransactionMappingDao(ctx);
            try
            {
                TreasuryTransactionMapping entity = new TreasuryTransactionMapping();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                int ID = entityDao.InsertReturnPrimaryKeyID(entity);
                ctx.CommitTransaction();

                retval = ID.ToString();
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TreasuryTransactionMappingDao entityDao = new TreasuryTransactionMappingDao(ctx);
            try
            {
                TreasuryTransactionMapping entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
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