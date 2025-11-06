using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class FractionGroupEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.FRACTION_GROUP;
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
                int ID = Convert.ToInt32(Request.QueryString["id"]);
                hdnID.Value = ID.ToString();
                FractionGroup entity = BusinessLayer.GetFractionGroup(ID);
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtFractionGroupCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtFractionGroupCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFractionGroupName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDisplayOrder, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(FractionGroup entity)
        {
            txtFractionGroupCode.Text = entity.FractionGroupCode;
            txtFractionGroupName.Text = entity.FractionGroupName;
            txtDisplayOrder.Text = entity.DisplayOrder.ToString();
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(FractionGroup entity)
        {
            entity.FractionGroupCode = txtFractionGroupCode.Text;
            entity.FractionGroupName = txtFractionGroupName.Text;
            entity.DisplayOrder = Convert.ToInt16(txtDisplayOrder.Text);
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("FractionGroupCode = '{0}'", txtFractionGroupCode.Text);
            List<FractionGroup> lst = BusinessLayer.GetFractionGroupList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Fraction Group with Code " + txtFractionGroupCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            FractionGroupDao entityDao = new FractionGroupDao(ctx);
            bool result = false;
            try
            {
                FractionGroup entity = new FractionGroup();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetFractionGroupMaxID(ctx).ToString();
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
                FractionGroup entity = BusinessLayer.GetFractionGroup(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateFractionGroup(entity);
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