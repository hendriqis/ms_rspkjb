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
    public partial class ATCClassEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.ATC_CLASSIFICATION;
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
                String ATCClassID = Request.QueryString["id"];
                hdnID.Value = ATCClassID;
                ATCClassification entity = BusinessLayer.GetATCClassification(Convert.ToInt32(ATCClassID));
                ATCClassification entityParent = null;
                if(entity.ParentID != null && entity.ParentID > 0)
                    entityParent = BusinessLayer.GetATCClassification((int)entity.ParentID);
                EntityToControl(entity, entityParent);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtATCClassCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtATCClassCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtATCClassName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtParentCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsHeader, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(hdnParentID, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(ATCClassification entity, ATCClassification entityParent)
        {
            txtATCClassCode.Text = entity.ATCClassCode;
            txtATCClassName.Text = entity.ATCClassName;
            hdnParentID.Value = entity.ParentID.ToString();
            chkIsHeader.Checked = entity.IsHeader;
            if (entityParent != null)
            {
                txtParentCode.Text = entityParent.ATCClassCode;
                txtParentName.Text = entityParent.ATCClassName;
            }
        }

        private void ControlToEntity(ATCClassification entity)
        {
            entity.ATCClassCode = txtATCClassCode.Text;
            entity.ATCClassName = txtATCClassName.Text;
            if (hdnParentID.Value == "" || hdnParentID.Value == "0")
                entity.ParentID = null;
            else
                entity.ParentID = Convert.ToInt32(hdnParentID.Value);
            entity.IsHeader = chkIsHeader.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ATCClassCode = '{0}'", txtATCClassCode.Text);
            List<ATCClassification> lst = BusinessLayer.GetATCClassificationList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " ATC Class with Code " + txtATCClassCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 menuID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("ATCClassCode = '{0}' AND ATCClassID != {1}", txtATCClassCode.Text, menuID);
            List<ATCClassification> lst = BusinessLayer.GetATCClassificationList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " ATC Class with Code " + txtATCClassCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ATCClassificationDao entityDao = new ATCClassificationDao(ctx);
            bool result = false;
            try
            {
                ATCClassification entity = new ATCClassification();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetATCClassificationMaxID(ctx).ToString();
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
                ATCClassification entity = BusinessLayer.GetATCClassification(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                BusinessLayer.UpdateATCClassification(entity);
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