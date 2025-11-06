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
    public partial class CashFlowAccountEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.CASH_FLOW_ACCOUNT;
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
                hdnGLCashFlowAccountID.Value = Request.QueryString["id"];
                vGLCashFlowAccountHd entity = BusinessLayer.GetvGLCashFlowAccountHdList(string.Format("GLCashFlowAccountID = {0}", hdnGLCashFlowAccountID.Value)).FirstOrDefault();
                
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }

        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstContentGroup = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.CONTENT_CASH_FLOW_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboGCContentGroup, lstContentGroup, "StandardCodeName", "StandardCodeID");
            cboGCContentGroup.SelectedIndex = 0;

            List<Variable> lstDisplay = new List<Variable>();
            lstDisplay.Add(new Variable { Code = "+", Value = GetLabel("+") });
            lstDisplay.Add(new Variable { Code = "-", Value = GetLabel("-") });
            Methods.SetComboBoxField<Variable>(cboContentOperator, lstDisplay, "Value", "Code");
            cboContentOperator.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtContentCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtContentName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtContentName2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCContentGroup, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboContentOperator, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(vGLCashFlowAccountHd entity)
        {
            txtContentCode.Text = entity.ContentCode;
            txtContentName.Text = entity.ContentName;
            txtContentName2.Text = entity.ContentName2;
            cboGCContentGroup.Value = entity.GCContentGroup;
            cboContentOperator.Value = entity.ContentOperator;
        }

        private void ControlToEntity(GLCashFlowAccountHd entity)
        {
            entity.ContentCode = txtContentCode.Text;
            entity.ContentName = txtContentName.Text;
            entity.ContentName2 = txtContentName2.Text;
            entity.GCContentGroup = cboGCContentGroup.Value.ToString();
            entity.ContentOperator = cboContentOperator.Value.ToString();
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string filter2 = string.Format("ContentCode = '{0}' AND IsDeleted = 0 AND ", txtContentCode.Text);
            List<GLCashFlowAccountHd> lst2 = BusinessLayer.GetGLCashFlowAccountHdList(filter2);

            if (lst2.Count > 0)
            {
                errMessage = "GLCashFlowAccountHd with ContentCode <b>" + txtContentCode.Text + "</b> is already exist!";
                Exception ex = new Exception(errMessage);
                Helper.InsertErrorLog(ex);
            }

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string filter2 = string.Format("ContentCode = '{0}' AND IsDeleted = 0 AND GLCashFlowAccountID != {1}", txtContentCode.Text, hdnGLCashFlowAccountID.Value);
            List<GLCashFlowAccountHd> lst2 = BusinessLayer.GetGLCashFlowAccountHdList(filter2);

            if (lst2.Count > 0)
            {
                errMessage = "GLCashFlowAccountHd with ContentCode <b>" + txtContentCode.Text + "</b> is already exist!";
                Exception ex = new Exception(errMessage);
                Helper.InsertErrorLog(ex);
            }

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            GLCashFlowAccountHdDao entityDao = new GLCashFlowAccountHdDao(ctx);
            bool result = true;
            try
            {
                GLCashFlowAccountHd entity = new GLCashFlowAccountHd();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetGLCashFlowAccountHdMaxID(ctx).ToString();
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
            IDbContext ctx = DbFactory.Configure(true);
            GLCashFlowAccountHdDao entityDao = new GLCashFlowAccountHdDao(ctx);
            bool result = true;
            try
            {
                GLCashFlowAccountHd entity = BusinessLayer.GetGLCashFlowAccountHd(Convert.ToInt32(hdnGLCashFlowAccountID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
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