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
    public partial class TemplatePanelEntry : BasePageEntry
    {

        public override string OnGetMenuCode()
        {
            String queryString = Request.QueryString["id"];
            String[] query = queryString.Split('|');
            String temp = query[0];
            switch (temp)
            {
                case "LABORATORY": return Constant.MenuCode.Laboratory.TEMPLATE_PANEL_LABORATORY;
                case "IMAGING": return Constant.MenuCode.Imaging.TEMPLATE_PANEL_IMAGING;
                case "DIAGNOSTIC": return Constant.MenuCode.MedicalDiagnostic.TEMPLATE_PANEL_DIAGNOSTIC;
                //case "SERVICES": return Constant.MenuCode.Finance.TEMPLATE_PANEL_SERVICES;
                //case "MCU": return Constant.MenuCode.MedicalCheckup.TEMPLATE_PANEL_MCU;
                default: return Constant.MenuCode.Laboratory.TEMPLATE_PANEL_LABORATORY;
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
                hdnTemplateID.Value = query[1];
                int ID = Convert.ToInt32(query[1]);
                string filterData = string.Format("TestTemplateID = {0}", ID);
                vTestTemplateHd entity = BusinessLayer.GetvTestTemplateHdList(filterData).FirstOrDefault();
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

            if (temp == "LABORATORY")
            {
                filterExpression = string.Format("StandardCodeID IN ('{0}')", Constant.ItemType.LABORATORIUM);
            }
            else if (temp == "IMAGING")
            {
                filterExpression = string.Format("StandardCodeID IN ('{0}')", Constant.ItemType.RADIOLOGI);
            }
            else if (temp == "DIAGNOSTIC")
            {
                filterExpression = string.Format("StandardCodeID IN ('{0}')", Constant.ItemType.PENUNJANG_MEDIS);
            }
            //else if (temp == "MCU")
            //{
            //    filterExpression = string.Format("StandardCodeID = '{0}'", Constant.ItemType.MEDICAL_CHECKUP);
            //}
            //else
            //{
            //    filterExpression = string.Format("ParentID IN ('{0}')", Constant.StandardCode.ITEM_TYPE);
            //}

            List<StandardCode> lstScItemType = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboItemType, lstScItemType, "StandardCodeName", "StandardCodeID");
            cboItemType.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTemplateCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTemplateName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboItemType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(vTestTemplateHd entity)
        {
            txtTemplateCode.Text = entity.TestTemplateCode;
            txtTemplateName.Text = entity.TestTemplateName;
            cboItemType.Value = entity.GCItemType;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(TestTemplateHd entity)
        {
            entity.TestTemplateCode = txtTemplateCode.Text;
            entity.TestTemplateName = txtTemplateName.Text;
            entity.GCItemType = cboItemType.Value.ToString();
            if (hdnHealthcareServiceUnitID.Value != null && hdnHealthcareServiceUnitID.Value != "" && hdnHealthcareServiceUnitID.Value != "0")
            {
                entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            }
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {

            errMessage = string.Empty;

            string FilterExpression = string.Format("TestTemplateCode = '{0}' AND IsDeleted = 0", txtTemplateCode.Text);
            List<TestTemplateHd> lst = BusinessLayer.GetTestTemplateHdList(FilterExpression);
            if (lst.Count > 0)
                errMessage = " Template with Code " + txtTemplateCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestTemplateHdDao entityDao = new TestTemplateHdDao(ctx);
            try
            {
                TestTemplateHd entity = new TestTemplateHd();
                ControlToEntity(entity);
                entity.TestTemplateCode = Helper.GenerateTestTemplateCode(ctx, entity.TestTemplateName);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();

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
            TestTemplateHdDao entityDao = new TestTemplateHdDao(ctx);
            try
            {
                TestTemplateHd entity = entityDao.Get(Convert.ToInt32(hdnTemplateID.Value));
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