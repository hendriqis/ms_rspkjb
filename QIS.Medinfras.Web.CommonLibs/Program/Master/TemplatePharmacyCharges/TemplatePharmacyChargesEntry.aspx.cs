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
    public partial class TemplatePharmacyChargesEntry : BasePageEntry
    {

        public override string OnGetMenuCode()
        {
            String queryString = Request.QueryString["id"];
            String[] query = queryString.Split('|');
            String temp = query[0];
            switch (temp)
            {
                case "PH": return Constant.MenuCode.Pharmacy.TEMPLATE_PHARMACY_CHARGES;
                default: return Constant.MenuCode.Pharmacy.TEMPLATE_PHARMACY_CHARGES;
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

            List<SettingParameterDt> lstSetvar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM));
            //hdnHSUIDImaging.Value = lstSetvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            //hdnHSUIDLaboratory.Value = lstSetvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;

            if (query.Count() > 1)
            {
                IsAdd = false;
                hdnTemplateID.Value = query[1];
                int ID = Convert.ToInt32(query[1]);
                ChargesTemplateHd entity = BusinessLayer.GetChargesTemplateHd(ID);
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }

            txtTemplateName.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            SetControlProperties();
        }

        protected override void SetControlProperties()
        {
            String queryString2 = Request.QueryString["id"];
            String[] query2 = queryString2.Split('|');
            String temp = query2[0];
            String filterExpression = "";

            filterExpression = "";

            filterExpression = string.Format("ServiceUnitID IN (SELECT ServiceUnitID FROM ServiceUnitMaster WHERE DepartmentID = '{0}')", Constant.Facility.PHARMACY);

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboHealthcareServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            //cboHealthcareServiceUnit.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTemplateCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTemplateName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboHealthcareServiceUnit, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(ChargesTemplateHd entity)
        {
            txtTemplateCode.Text = entity.ChargesTemplateCode;
            txtTemplateName.Text = entity.ChargesTemplateName;
            cboHealthcareServiceUnit.Value = Convert.ToString(entity.HealthcareServiceUnitID);
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(ChargesTemplateHd entity)
        {
            entity.ChargesTemplateCode = txtTemplateCode.Text;
            entity.ChargesTemplateName = txtTemplateName.Text;
            entity.HealthcareServiceUnitID = Convert.ToInt32(cboHealthcareServiceUnit.Value);
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {

            errMessage = string.Empty;

            string FilterExpression = string.Format("ChargesTemplateCode = '{0}'", txtTemplateCode.Text);
            List<ChargesTemplateHd> lst = BusinessLayer.GetChargesTemplateHdList(FilterExpression);
            if (lst.Count > 0)
                errMessage = " Template with Code " + txtTemplateCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ChargesTemplateHdDao entityDao = new ChargesTemplateHdDao(ctx);
            try
            {
                ChargesTemplateHd entity = new ChargesTemplateHd();
                ControlToEntity(entity);
                entity.ChargesTemplateCode = Helper.GenerateChargesTemplateCode(ctx, entity.ChargesTemplateName);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);

                retval = BusinessLayer.GetTextChargesTemplateMaxID(ctx).ToString();

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
            ChargesTemplateHdDao entityDao = new ChargesTemplateHdDao(ctx);
            try
            {
                ChargesTemplateHd entity = entityDao.Get(Convert.ToInt32(hdnTemplateID.Value));
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