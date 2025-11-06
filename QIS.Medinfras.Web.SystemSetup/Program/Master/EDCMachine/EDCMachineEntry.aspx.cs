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
    public partial class EDCMachineEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.EDC_MACHINE;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                EDCMachine entity = BusinessLayer.GetEDCMachine(Convert.ToInt32(ID));
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtEDCMachineCode.Focus();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(String.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.CARD_PROVIDER));
            Methods.SetComboBoxField<StandardCode>(cboCardProvider, lst, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtEDCMachineCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtEDCMachineName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCardProvider, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(EDCMachine entity)
        {
            txtEDCMachineCode.Text = entity.EDCMachineCode;
            txtEDCMachineName.Text = entity.EDCMachineName;
            cboCardProvider.Value = entity.GCCardProvider;
        }

        private void ControlToEntity(EDCMachine entity)
        {
            entity.EDCMachineCode = txtEDCMachineCode.Text;
            entity.EDCMachineName = txtEDCMachineName.Text;
            entity.GCCardProvider = cboCardProvider.Value.ToString();
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("EDCMachineCode = '{0}'", txtEDCMachineCode.Text);
            List<EDCMachine> lst = BusinessLayer.GetEDCMachineList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " EDC Machine With Code " + txtEDCMachineCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("EDCMachineCode = '{0}' AND EDCMachineID != {1}", txtEDCMachineCode.Text, hdnID.Value);
            List<EDCMachine> lst = BusinessLayer.GetEDCMachineList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " EDC Machine With Code " + txtEDCMachineCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            EDCMachineDao entityDao = new EDCMachineDao(ctx);
            bool result = false;
            try
            {
                EDCMachine entity = new EDCMachine();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetEDCMachineMaxID(ctx).ToString();
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
                EDCMachine entity = BusinessLayer.GetEDCMachine(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateEDCMachine(entity);
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