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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class VisitTypeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.VISIT_TYPE;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                int ID = Convert.ToInt32(Request.QueryString["id"]);
                hdnID.Value = ID.ToString();
                VisitType entity = BusinessLayer.GetVisitType(ID);
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtVisitTypeCode.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtVisitTypeCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtVisitTypeName, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(VisitType entity)
        {
            txtVisitTypeCode.Text = entity.VisitTypeCode;
            txtVisitTypeName.Text = entity.VisitTypeName;
            chkIsSickVisit.Checked = entity.IsSickVisit;
        }

        private void ControlToEntity(VisitType entity)
        {
            entity.VisitTypeCode = txtVisitTypeCode.Text;
            entity.VisitTypeName = txtVisitTypeName.Text;
            entity.IsSickVisit = chkIsSickVisit.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("VisitTypeCode = '{0}'", txtVisitTypeCode.Text);
            List<VisitType> lst = BusinessLayer.GetVisitTypeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Visit Type with Code " + txtVisitTypeCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }
        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 VisitTypeID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("VisitTypeCode = '{0}' AND VisitTypeID != {1}", txtVisitTypeCode.Text, VisitTypeID);
            List<VisitType> lst = BusinessLayer.GetVisitTypeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " VisitType with Code " + txtVisitTypeCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            VisitTypeDao entityDao = new VisitTypeDao(ctx);
            bool result = false;
            try
            {
                VisitType entity = new VisitType();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetVisitTypeMaxID(ctx).ToString();
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
                VisitType entity = BusinessLayer.GetVisitType(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVisitType(entity);
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