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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class SOAPTemplateEntry : BasePageEntry
    {

        public override string OnGetMenuCode()
        {
            String queryString = Request.QueryString["id"];
            String[] query = queryString.Split('|');
            String temp = query[0];
            switch (temp)
            {
                default: return Constant.MenuCode.MedicalDiagnostic.PROCEDURE_GROUP_DIAGNOSTIC;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            string qry = Request.QueryString["id"];
            String[] query = qry.Split('|');
            if (query.Count() > 1)
            {
                IsAdd = false;
                hdnProcedureGroupID.Value = query[1];
                int ID = Convert.ToInt32(query[1]);
                ProcedureGroup entity = BusinessLayer.GetProcedureGroup(ID);
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }

            if (query[0] == "DIAGNOSTIC")
            {
                hdnGCItemType.Value = Constant.ItemType.PENUNJANG_MEDIS;
            }
            else
            {
                hdnGCItemType.Value = Constant.ItemType.PELAYANAN;
            }

            txtProcedureGroupName.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtProcedureGroupCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtProcedureGroupName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(ProcedureGroup entity)
        {
            txtProcedureGroupCode.Text = entity.ProcedureGroupCode;
            txtProcedureGroupName.Text = entity.ProcedureGroupName;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(ProcedureGroup entity)
        {
            entity.ProcedureGroupCode = txtProcedureGroupCode.Text;
            entity.ProcedureGroupName = txtProcedureGroupName.Text;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {

            errMessage = string.Empty;

            string FilterExpression = string.Format("ProcedureGroupCode = '{0}'", txtProcedureGroupCode.Text);
            List<ProcedureGroup> lst = BusinessLayer.GetProcedureGroupList(FilterExpression);
            if (lst.Count > 0)
                errMessage = " Template with Code " + txtProcedureGroupCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ProcedureGroupDao entityDao = new ProcedureGroupDao(ctx);
            try
            {
                ProcedureGroup entity = new ProcedureGroup();
                ControlToEntity(entity);
                entity.ProcedureGroupCode = Helper.GenerateProcedureGroupCode(ctx, entity.ProcedureGroupName);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);

                retval = BusinessLayer.GetTextProcedureGroupMaxID(ctx).ToString();

                ctx.CommitTransaction();
                result = true;
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
            ProcedureGroupDao entityDao = new ProcedureGroupDao(ctx);
            try
            {
                ProcedureGroup entity = entityDao.Get(Convert.ToInt32(hdnProcedureGroupID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                ctx.CommitTransaction();
                result = true;
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