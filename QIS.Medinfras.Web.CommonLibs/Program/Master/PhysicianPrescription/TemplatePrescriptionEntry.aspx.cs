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
    public partial class TemplatePrescriptionEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.MASTER_PHYSICIAN_PRESCRIPTION_TEMPLATE;
        }

        protected String GetPageTitle()
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
                PrescriptionTemplateHd entity = BusinessLayer.GetPrescriptionTemplateHd(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtTemplateCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;           
        }


        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTemplateCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTemplateName, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(PrescriptionTemplateHd entity)
        {
            txtTemplateCode.Text = entity.PrescriptionTemplateCode;
            txtTemplateName.Text = entity.PrescriptionTemplateName;
        }

        private void ControlToEntity(PrescriptionTemplateHd entity)
        {
            entity.PrescriptionTemplateCode = txtTemplateCode.Text;
            entity.PrescriptionTemplateName = txtTemplateName.Text;
            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);           
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("PrescriptionTemplateCode = '{0}' AND ParamedicID = {1} AND IsDeleted = 0 ", txtTemplateCode.Text, AppSession.UserLogin.ParamedicID);
            List<PrescriptionTemplateHd> lst = BusinessLayer.GetPrescriptionTemplateHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Prescription Template With Code " + txtTemplateCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("PrescriptionTemplateCode = '{0}' AND PrescriptionTemplateID != {1} AND ParamedicID = {2} AND IsDeleted = 0 ", txtTemplateCode.Text, hdnID.Value, AppSession.UserLogin.UserID);
            List<PrescriptionTemplateHd> lst = BusinessLayer.GetPrescriptionTemplateHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Prescription Template With Code " + txtTemplateCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionTemplateHdDao entityDao = new PrescriptionTemplateHdDao(ctx);
            bool result = false;
            try
            {
                PrescriptionTemplateHd entity = new PrescriptionTemplateHd();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();

                result = true;
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
            try
            {
                PrescriptionTemplateHd entity = BusinessLayer.GetPrescriptionTemplateHd(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePrescriptionTemplateHd(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}