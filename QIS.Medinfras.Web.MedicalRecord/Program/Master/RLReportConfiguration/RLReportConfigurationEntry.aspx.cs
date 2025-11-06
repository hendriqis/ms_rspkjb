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

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class RLReportConfigurationEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.RL_REPORT_CONFIGURATION;
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
                RLReport entity = BusinessLayer.GetRLReport(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtRLReportCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtRLReportCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRLReportName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtColumnCaption, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(RLReport entity)
        {
            txtRLReportCode.Text = entity.RLReportCode;
            txtRLReportName.Text = entity.RLReportName;
            txtColumnCaption.Text = entity.ColumnCaption;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(RLReport entity)
        {
            entity.RLReportCode = txtRLReportCode.Text;
            entity.RLReportName = txtRLReportName.Text;
            entity.ColumnCaption = txtColumnCaption.Text;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("RLReportCode = '{0}'", txtRLReportCode.Text);
            List<RLReport> lst = BusinessLayer.GetRLReportList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Medical Record Form with Code " + txtRLReportCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            RLReportDao entityDao = new RLReportDao(ctx);
            bool result = false;
            try
            {
                RLReport entity = new RLReport();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetRLReportMaxID(ctx).ToString();
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
                RLReport entity = BusinessLayer.GetRLReport(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateRLReport(entity);
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