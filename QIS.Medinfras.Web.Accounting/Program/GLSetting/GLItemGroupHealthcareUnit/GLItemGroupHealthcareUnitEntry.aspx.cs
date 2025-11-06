using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLItemGroupHealthcareUnitEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_ITEM_GROUP_HEALTHCARE_UNIT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<StandardCode> lstHealthcareUnit = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.HEALTHCARE_UNIT));
            Methods.SetComboBoxField<StandardCode>(cboHealthcareUnit, lstHealthcareUnit, "StandardCodeName", "StandardCodeID");
            cboHealthcareUnit.SelectedIndex = 0;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String param = Request.QueryString["id"];
                hdnID.Value = param;
                string[] oID = hdnID.Value.Split('|');
                Int32 oItemGroupID = Convert.ToInt32(oID[0]);
                String oGCHealthcareUnit = oID[1];

                string filterData = string.Format("ItemGroupID = {0} AND GCHealthcareUnit = '{1}'", oItemGroupID, oGCHealthcareUnit);
                vGLItemGroupHealthcareUnit entity = BusinessLayer.GetvGLItemGroupHealthcareUnitList(filterData).FirstOrDefault();
                EntityToControl(entity);

                txtItemGroupCode.Focus();
            }
            else
            {
                IsAdd = true;
            }

        }

        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnID, new ControlEntrySetting(true, true, false, ""));

            SetControlEntrySetting(hdnItemGroupID, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtItemGroupCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtItemGroupName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(cboHealthcareUnit, new ControlEntrySetting(true, false, true));
            
            #region Pengaturan Perkiraan untuk Persediaan
            SetControlEntrySetting(hdnConsumptionGLAccountID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtConsumptionGLAccountNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtConsumptionGLAccountName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(hdnAdjustmentINGLAccountID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAdjustmentINGLAccountNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAdjustmentINGLAccountName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(hdnAdjustmentOUTGLAccountID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAdjustmentOUTGLAccountNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAdjustmentOUTGLAccountName, new ControlEntrySetting(false, false, true));
            #endregion
        }

        private void EntityToControl(vGLItemGroupHealthcareUnit entity)
        {
            hdnItemGroupID.Value = entity.ItemGroupID.ToString();
            txtItemGroupCode.Text = entity.ItemGroupCode;
            txtItemGroupName.Text = entity.ItemGroupName1;

            cboHealthcareUnit.Value = entity.GCHealthcareUnit;

            #region Pengaturan Perkiraan untuk Aktiva Tetap
            hdnConsumptionGLAccountID.Value = entity.ConsumptionGLAccountID.ToString();
            txtConsumptionGLAccountNo.Text = entity.ConsumptionGLAccountNo;
            txtConsumptionGLAccountName.Text = entity.ConsumptionGLAccountName;

            hdnAdjustmentINGLAccountID.Value = entity.AdjustmentINGLAccountID.ToString();
            txtAdjustmentINGLAccountNo.Text = entity.AdjustmentINGLAccountNo;
            txtAdjustmentINGLAccountName.Text = entity.AdjustmentINGLAccountName;

            hdnAdjustmentOUTGLAccountID.Value = entity.AdjustmentOUTGLAccountID.ToString();
            txtAdjustmentOUTGLAccountNo.Text = entity.AdjustmentOUTGLAccountNo;
            txtAdjustmentOUTGLAccountName.Text = entity.AdjustmentOUTGLAccountName;
            #endregion
        }

        private void ControlToEntity(GLItemGroupHealthcareUnit entity)
        {
            entity.ItemGroupID = Convert.ToInt32(hdnItemGroupID.Value);
            entity.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
            
            #region Pengaturan Perkiraan untuk Aktiva Tetap
            entity.ConsumptionGLAccountID = Convert.ToInt32(hdnConsumptionGLAccountID.Value);
            entity.AdjustmentINGLAccountID = Convert.ToInt32(hdnAdjustmentINGLAccountID.Value);
            entity.AdjustmentOUTGLAccountID = Convert.ToInt32(hdnAdjustmentOUTGLAccountID.Value);
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLItemGroupHealthcareUnitDao entityDao = new GLItemGroupHealthcareUnitDao(ctx);
            try
            {   
                GLItemGroupHealthcareUnit entity = new GLItemGroupHealthcareUnit();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLItemGroupHealthcareUnitDao entityDao = new GLItemGroupHealthcareUnitDao(ctx);
            try
            {
                string[] oID = hdnID.Value.Split('|');
                Int32 oItemGroupID = Convert.ToInt32(oID[0]);
                String oGCHealthcareUnit = oID[1];

                GLItemGroupHealthcareUnit entityDelete = entityDao.Get(oItemGroupID, oGCHealthcareUnit);
                entityDao.Delete(entityDelete.ItemGroupID, entityDelete.GCHealthcareUnit);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                GLItemGroupHealthcareUnit entityInsert = new GLItemGroupHealthcareUnit();
                ControlToEntity(entityInsert);
                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                entityInsert.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityInsert.LastUpdatedDate = DateTime.Now;
                entityDao.Insert(entityInsert);

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