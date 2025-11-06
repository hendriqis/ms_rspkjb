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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ProcedurePanelEntry : BasePageEntry
    {

        public override string OnGetMenuCode()
        {
            String queryString = Request.QueryString["id"];
            switch (queryString)
            {
                //case "SERVICES": return Constant.MenuCode.Finance.TEMPLATE_PANEL_SERVICES;
                //case "LABORATORY": return Constant.MenuCode.Laboratory.TEMPLATE_PANEL_LABORATORY;
                //case "IMAGING": return Constant.MenuCode.Imaging.TEMPLATE_PANEL_IMAGING;
                //case "DIAGNOSTIC": return Constant.MenuCode.MedicalDiagnostic.TEMPLATE_PANEL_DIAGNOSTIC;
                default: return Constant.MenuCode.MedicalDiagnostic.MD_PROCEDURE_PANEL;
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
                hdnProcedureID.Value = query[1];
                int ID = Convert.ToInt32(query[1]);
                //TestTemplateHd entity = BusinessLayer.GetTestTemplateHd(ID);
                ProcedurePanelHd entity = BusinessLayer.GetProcedurePanelHd(ID);
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }

            if (query[0] == "SERVICES")
            {
                hdnGCItemType.Value = Constant.ItemType.PELAYANAN;
            }
            else if (query[0] == "LABORATORY")
            {
                hdnGCItemType.Value = Constant.ItemType.LABORATORIUM;
            }
            else if (query[0] == "IMAGING")
            {
                hdnGCItemType.Value = Constant.ItemType.RADIOLOGI;
            }
            else if (query[0] == "DIAGNOSTIC")
            {
                hdnGCItemType.Value = Constant.ItemType.PENUNJANG_MEDIS;
            }
            else
            {
                hdnGCItemType.Value = Constant.ItemType.PELAYANAN;
            }

            txtProcedureName.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtProcedureCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtProcedureName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(ProcedurePanelHd entity)
        {
            txtProcedureCode.Text = entity.ProcedurePanelCode;
            txtProcedureName.Text = entity.ProcedurePanelName;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(ProcedurePanelHd entity)
        {
            entity.ProcedurePanelCode = txtProcedureCode.Text;
            entity.ProcedurePanelName = txtProcedureName.Text;
            //entity.GCItemType = hdnGCItemType.Value;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {

            errMessage = string.Empty;

            string FilterExpression = string.Format("ProcedurePanelCode = '{0}'", txtProcedureCode.Text);
            List<ProcedurePanelHd> lst = BusinessLayer.GetProcedurePanelHdList(FilterExpression);
            if (lst.Count > 0)
                errMessage = " Procedure with Code " + txtProcedureCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ProcedurePanelHdDao entityDao = new ProcedurePanelHdDao(ctx);
            try
            {
                ProcedurePanelHd entity = new ProcedurePanelHd();
                ControlToEntity(entity);
                entity.ProcedurePanelCode = Helper.GenerateProcedurePanelCode(ctx, entity.ProcedurePanelName);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);

                retval = BusinessLayer.GetTextProcedurePanelMaxID(ctx).ToString();

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
            ProcedurePanelHdDao entityDao = new ProcedurePanelHdDao(ctx);
            try
            {
                ProcedurePanelHd entity = entityDao.Get(Convert.ToInt32(hdnProcedureID.Value));
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