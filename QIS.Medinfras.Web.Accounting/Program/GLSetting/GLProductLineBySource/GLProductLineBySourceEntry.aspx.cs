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
    public partial class GLProductLineBySourceEntry : BasePageEntry
    {
        String page;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_PRODUCT_LINE_ACCOUNT_SOURCEID;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String param = Request.QueryString["id"];
                hdnID.Value = param;
                string[] oID = hdnID.Value.Split('|');
                String oProductLineID = oID[0];
                String oHealthCareServiceUnitID = oID[1];
                String oSourceHealthCareServiceUnitID = oID[2];
                String oClassID = oID[3];

                string filterData = string.Format("ProductLineID = {0} AND HealthCareServiceUnitID = {1} AND SourceHealthCareServiceUnitID = {2} AND ClassID = {3}", oProductLineID, oHealthCareServiceUnitID, oSourceHealthCareServiceUnitID, oClassID);
                vGLProductLineBySourceID entity = BusinessLayer.GetvGLProductLineBySourceIDList(filterData).FirstOrDefault();
                EntityToControl(entity);

                txtProductLineCode.Focus();
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
            SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnSourceHealthcareServiceUnitID, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtSourceServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtSourceServiceUnitName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnClassID, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtClassCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtClassName, new ControlEntrySetting(false, false, false));

            #region Pengaturan Perkiraan untuk Persediaan
            SetControlEntrySetting(hdnGLAccount1ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount1Code, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGLAccount1Name, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnGLAccount2ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount2Code, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGLAccount2Name, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnGLAccount3ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount3Code, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGLAccount3Name, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnGLAccount4ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount4Code, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGLAccount4Name, new ControlEntrySetting(false, false, false));

            #endregion
        }

        private void EntityToControl(vGLProductLineBySourceID entity)
        {
            hdnProductLineID.Value = entity.ProductLineID.ToString();
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;

            hdnHealthcareServiceUnitID.Value = entity.HealthCareServiceUnitID.ToString();
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;

            hdnSourceHealthcareServiceUnitID.Value = entity.SourceHealthCareServiceUnitID.ToString();
            txtSourceServiceUnitCode.Text = entity.SourceServiceUnitCode;
            txtSourceServiceUnitName.Text = entity.SourceServiceUnitName;

            hdnClassID.Value = entity.ClassID.ToString();
            txtClassCode.Text = entity.ClassCode;
            txtClassName.Text = entity.ClassName;

            #region Pengaturan Perkiraan untuk Aktiva Tetap
            hdnGLAccount1ID.Value = entity.COGSGLAccountID.ToString();
            txtGLAccount1Code.Text = entity.COGSGLAccountCode;
            txtGLAccount1Name.Text = entity.COGSGLAccountName;

            hdnGLAccount2ID.Value = entity.ConsumptionGLAccountID.ToString();
            txtGLAccount2Code.Text = entity.ConsumptionGLAccountCode;
            txtGLAccount2Name.Text = entity.ConsumptionGLAccountName;

            hdnGLAccount3ID.Value = entity.AdjustmentINGLAccountID.ToString();
            txtGLAccount3Code.Text = entity.AdjustmentINGLAccountCode;
            txtGLAccount3Name.Text = entity.AdjustmentINGLAccountName;

            hdnGLAccount4ID.Value = entity.AdjustmentOUTGLAccountID.ToString();
            txtGLAccount4Code.Text = entity.AdjustmentOUTGLAccountCode;
            txtGLAccount4Name.Text = entity.AdjustmentOUTGLAccountName;
            #endregion
        }

        private void ControlToEntity(GLProductLineBySourceID entity)
        {
            entity.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            entity.HealthCareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entity.SourceID = Convert.ToInt32(hdnSourceHealthcareServiceUnitID.Value);
            entity.ClassID = Convert.ToInt32(hdnClassID.Value);

            #region Pengaturan Perkiraan untuk Aktiva Tetap
            entity.COGSGLAccountID = Convert.ToInt32(hdnGLAccount1ID.Value);
            entity.ConsumptionGLAccountID = Convert.ToInt32(hdnGLAccount2ID.Value);
            entity.AdjustmentINGLAccountID = Convert.ToInt32(hdnGLAccount3ID.Value);
            entity.AdjustmentOUTGLAccountID = Convert.ToInt32(hdnGLAccount4ID.Value);
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLProductLineBySourceIDDao entityDao = new GLProductLineBySourceIDDao(ctx);
            try
            {
                GLProductLineBySourceID entity = new GLProductLineBySourceID();
                ControlToEntity(entity);
                entity.ID = BusinessLayer.GLProductLineBySourceMaxID(ctx);
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
            GLProductLineBySourceIDDao entityDao = new GLProductLineBySourceIDDao(ctx);
            try
            {
                string[] oID = hdnID.Value.Split('|');
                Int32 oProductLineID = Convert.ToInt32(oID[0]);
                Int32 oHealthCareServiceUnitID = Convert.ToInt32(oID[1]);
                Int32 oSourceHealthCareServiceUnitID = Convert.ToInt32(oID[2]);
                Int32 oClassID = Convert.ToInt32(oID[3]);

                GLProductLineBySourceID entityDelete = entityDao.Get(oProductLineID, oHealthCareServiceUnitID, oSourceHealthCareServiceUnitID, oClassID);
                entityDao.Delete(entityDelete.ProductLineID, entityDelete.HealthCareServiceUnitID, entityDelete.SourceID, entityDelete.ClassID);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                GLProductLineBySourceID entityInsert = new GLProductLineBySourceID();
                ControlToEntity(entityInsert);
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