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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class INACBGsGrouperEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.BPJS_MASTER_INACBGs_GROUPER;
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
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;

                INACBGrouper entity = BusinessLayer.GetINACBGrouperList(string.Format("GrouperID = {0}", ID)).FirstOrDefault();

                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }

            txtGrouperCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtGrouperCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGrouperName, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(INACBGrouper entity)
        {
            txtGrouperCode.Text = entity.GrouperCode;
            txtGrouperName.Text = entity.GrouperName;
        }

        private void ControlToEntity(INACBGrouper entity)
        {
            entity.GrouperCode = txtGrouperCode.Text;
            entity.GrouperName = txtGrouperName.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("GrouperCode = '{0}'", txtGrouperCode.Text);
            List<INACBGrouper> lst = BusinessLayer.GetINACBGrouperList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " INACBGs Grouper With Code " + txtGrouperCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("GrouperCode = '{0}' AND GrouperID != {1}", txtGrouperCode.Text, hdnID.Value);
            List<INACBGrouper> lst = BusinessLayer.GetINACBGrouperList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " INACBGs Grouper With Code " + txtGrouperCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            INACBGrouperDao entityDao = new INACBGrouperDao(ctx);

            try
            {
                INACBGrouper entity = new INACBGrouper();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();

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
            INACBGrouperDao entityDao = new INACBGrouperDao(ctx);

            try
            {
                int BankID = Convert.ToInt32(hdnID.Value);
                INACBGrouper entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
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