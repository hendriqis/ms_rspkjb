using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Text;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class MIMSClassEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.MIMS_CLASS;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String MIMSClassID = Request.QueryString["id"];
                hdnID.Value = MIMSClassID;
                MIMSClass entity = BusinessLayer.GetMIMSClass(Convert.ToInt32(MIMSClassID));
                MIMSClass entityParent = null;
                if(entity.ParentID != null && entity.ParentID > 0)
                    entityParent = BusinessLayer.GetMIMSClass((int)entity.ParentID);
                EntityToControl(entity, entityParent);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtMIMSClassCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtMIMSClassCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMIMSClassName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtParentCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsHeader, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(hdnParentID, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(MIMSClass entity, MIMSClass entityParent)
        {
            txtMIMSClassCode.Text = entity.MIMSClassCode;
            txtMIMSClassName.Text = entity.MIMSClassName;
            hdnParentID.Value = entity.ParentID.ToString();
            chkIsHeader.Checked = entity.IsHeader;
            if (entityParent != null)
            {
                txtParentCode.Text = entityParent.MIMSClassCode;
                txtParentName.Text = entityParent.MIMSClassName;
            }
        }

        private void ControlToEntity(MIMSClass entity)
        {
            entity.MIMSClassCode = txtMIMSClassCode.Text;
            entity.MIMSClassName = txtMIMSClassName.Text;
            if (hdnParentID.Value == "" || hdnParentID.Value == "0")
                entity.ParentID = null;
            else
                entity.ParentID = Convert.ToInt32(hdnParentID.Value);
            entity.IsHeader = chkIsHeader.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("MIMSClassCode = '{0}'", txtMIMSClassCode.Text);
            List<MIMSClass> lst = BusinessLayer.GetMIMSClassList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " MIMS Class with Code " + txtMIMSClassCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 menuID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("MIMSClassCode = '{0}' AND MIMSClassID != {1}", txtMIMSClassCode.Text, menuID);
            List<MIMSClass> lst = BusinessLayer.GetMIMSClassList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " MIMS Class with Code " + txtMIMSClassCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            MIMSClassDao entityDao = new MIMSClassDao(ctx);
            bool result = false;
            try
            {
                MIMSClass entity = new MIMSClass();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetMIMSClassMaxID(ctx).ToString();
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                MIMSClass entity = BusinessLayer.GetMIMSClass(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                BusinessLayer.UpdateMIMSClass(entity);
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