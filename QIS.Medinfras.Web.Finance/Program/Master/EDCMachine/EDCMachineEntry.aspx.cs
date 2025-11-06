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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class EDCMachineEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.EDC_MACHINE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            setConfig(); 
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                vEDCMachine entity = BusinessLayer.GetvEDCMachineList(String.Format("EDCMachineID = {0}", Convert.ToInt32(ID))).FirstOrDefault();
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
          
            txtEDCMachineCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }
        private void setConfig() {
            List<SettingParameterDt> lstSetparDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}') AND HealthcareID='{1}'", Constant.SettingParameter.SA_EDC_BRIDGING, AppSession.UserLogin.HealthcareID));
            string flagEdcBridging = lstSetparDt.Where(p => p.ParameterCode == Constant.SettingParameter.SA_EDC_BRIDGING).FirstOrDefault().ParameterValue;
            if (flagEdcBridging == "1")
            {
                trEDCArea.Style.Remove("display");
                hdnIsBridgingEdc.Value = "1"; 
            }
            else {
                hdnIsBridgingEdc.Value = "0"; 
            }
           
            List<StandardCode> lstEdc = BusinessLayer.GetStandardCodeList(String.Format("ParentID = '{0}' AND  IsDeleted = 0", Constant.StandardCode.EDC));
            lstEdc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboEdcVendor, lstEdc , "StandardCodeName", "StandardCodeID");
            

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
            SetControlEntrySetting(hdnBankID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtBankCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBankName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(cboEdcVendor, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(vEDCMachine entity)
        {
            txtEDCMachineCode.Text = entity.EDCMachineCode;
            txtEDCMachineName.Text = entity.EDCMachineName;
            cboCardProvider.Value = entity.GCCardProvider;
            chkIsChargeFeeToPatient.Checked = entity.IsChargeFeeToPatient;
            chkIsUsingECR.Checked = entity.IsUsingECR;
            hdnBankID.Value = Convert.ToString(entity.BankID);
            txtBankCode.Text = entity.BankCode;
            txtBankName.Text = entity.BankName;
            cboEdcVendor.Value = entity.GCBridgingType;
        }

        private void ControlToEntity(EDCMachine entity)
        {
            entity.EDCMachineCode = txtEDCMachineCode.Text;
            entity.EDCMachineName = txtEDCMachineName.Text;
            entity.GCCardProvider = cboCardProvider.Value.ToString();
            entity.IsChargeFeeToPatient = chkIsChargeFeeToPatient.Checked;
            entity.IsUsingECR = chkIsUsingECR.Checked;
            if (!String.IsNullOrEmpty(Convert.ToString(hdnBankID.Value)))
            {
                entity.BankID = Convert.ToInt32(hdnBankID.Value);
            }
            else 
            {
                entity.BankID = null;
            }

            if (hdnIsBridgingEdc.Value == "1")
            {

                if (entity.IsUsingECR)
                {
                    if (cboEdcVendor != null)
                    {
                        entity.GCBridgingType = cboEdcVendor.Value.ToString();
                    }
                }
                else
                {
                    entity.GCBridgingType = null;
                }
               
            }
            else {
                entity.GCBridgingType = null;
            }

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