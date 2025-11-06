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

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class ChartOfAccountEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.CHART_OF_ACCOUNT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                hdnID.Value = Request.QueryString["id"];
                vChartOfAccount entity = BusinessLayer.GetvChartOfAccountList(string.Format("GLAccountID = {0}", hdnID.Value))[0];
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtGLAccountNo.Focus();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(String.Format(
                                                        "ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
                                                        Constant.StandardCode.GLACCOUNT_TYPE, //0
                                                        Constant.StandardCode.BUSINESS_OBJECT_TYPE //1
                                                    ));

            lst.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCGLAccountType, lst.Where(a => a.StandardCodeID == "" || a.ParentID == Constant.StandardCode.GLACCOUNT_TYPE).ToList() , "StandardCodeName", "StandardCodeID");
            cboGCGLAccountType.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboGCBusinessPartnerType, lst.Where(a => a.ParentID == Constant.StandardCode.BUSINESS_OBJECT_TYPE && (a.StandardCodeID == Constant.BusinessObjectType.CUSTOMER || a.StandardCodeID == Constant.BusinessObjectType.SUPPLIER)).ToList(), "StandardCodeName", "StandardCodeID");
            cboGCBusinessPartnerType.SelectedIndex = 0;

            List<Variable> lstPosition = new List<Variable>();
            lstPosition.Add(new Variable { Code = "D", Value = GetLabel("Debet") });
            lstPosition.Add(new Variable { Code = "K", Value = GetLabel("Kredit") });
            Methods.SetRadioButtonListField<Variable>(rblPosition, lstPosition, "Value", "Code");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtGLAccountNo, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtGLAccountName, new ControlEntrySetting(true, true, true));
            
            SetControlEntrySetting(hdnParentAccountID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtParentAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentAccountName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(cboGCGLAccountType, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(hdnSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(rblPosition, new ControlEntrySetting(true, true, true, "D"));
            SetControlEntrySetting(txtAccountLevel, new ControlEntrySetting(true, true, true, "0"));

            SetControlEntrySetting(chkIsActive, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHeader, new ControlEntrySetting(true, true, false));            
            SetControlEntrySetting(chkIsUsedAsTreasury, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUsingDocumentControl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUsingRevenueCostCenter, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUsingCustomerGroup, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUsingParamedicMaster, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUsingBusinessPartner, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCBusinessPartnerType, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(vChartOfAccount entity)
        {
            txtGLAccountNo.Text = entity.GLAccountNo;
            txtGLAccountName.Text = entity.GLAccountName;

            hdnParentAccountID.Value = entity.ParentGLAccount.ToString();
            txtParentAccountNo.Text = entity.ParentGLAccountNo;
            txtParentAccountName.Text = entity.ParentGLAccountName;
            cboGCGLAccountType.Value = entity.GCGLAccountType;

            hdnSubLedgerID.Value = entity.SubLedgerID.ToString();
            txtSubLedgerCode.Text = entity.SubLedgerCode;
            txtSubLedgerName.Text = entity.SubLedgerName;

            rblPosition.SelectedValue = entity.Position;
            txtAccountLevel.Text = entity.AccountLevel.ToString();
            chkIsActive.Checked = entity.IsActive;
            chkIsHeader.Checked = entity.IsHeader;
            chkIsUsedAsTreasury.Checked = entity.IsUsedAsTreasury;
            chkIsUsingDocumentControl.Checked = entity.IsUsingDocumentControl;
            chkIsUsingRevenueCostCenter.Checked = entity.IsUsingRevenueCostCenter;
            chkIsUsingCustomerGroup.Checked = entity.IsUsingCustomerGroup;
            chkIsUsingParamedicMaster.Checked = entity.IsUsingParamedicMaster;

            chkIsUsingBusinessPartner.Checked = entity.IsUsingBusinessPartner;
            if (entity.IsUsingBusinessPartner)
            {
                cboGCBusinessPartnerType.Value = entity.GCBusinessPartnerType;
                tdGCBusinessPartnerType.Attributes.Remove("style");
            }
            else
            {
                cboGCBusinessPartnerType.Value = Constant.BusinessObjectType.CUSTOMER;
                tdGCBusinessPartnerType.Attributes.Add("style", "display:none");
            }
        }

        private void ControlToEntity(ChartOfAccount entity)
        {
            entity.GLAccountNo = txtGLAccountNo.Text;
            entity.GLAccountName = txtGLAccountName.Text;

            if (hdnParentAccountID.Value != "" && hdnParentAccountID.Value != "0")
                entity.ParentGLAccount = Convert.ToInt32(hdnParentAccountID.Value);
            else
                entity.ParentGLAccount = null;
            if (cboGCGLAccountType.Value != null && cboGCGLAccountType.Value.ToString() != "")
                entity.GCGLAccountType = cboGCGLAccountType.Value.ToString();
            else
                entity.GCGLAccountType = null;
            if (hdnSubLedgerID.Value != "" && hdnSubLedgerID.Value != "0")
                entity.SubLedgerID = Convert.ToInt32(hdnSubLedgerID.Value);
            else
                entity.SubLedgerID = null;
            entity.Position = rblPosition.SelectedValue;
            entity.AccountLevel = Convert.ToInt16(txtAccountLevel.Text);
            entity.IsActive = chkIsActive.Checked;
            entity.IsHeader = chkIsHeader.Checked;
            entity.IsUsedAsTreasury = chkIsUsedAsTreasury.Checked;
            entity.IsUsingDocumentControl = chkIsUsingDocumentControl.Checked;
            entity.IsUsingRevenueCostCenter = chkIsUsingRevenueCostCenter.Checked;
            entity.IsUsingCustomerGroup = chkIsUsingCustomerGroup.Checked;
            entity.IsUsingParamedicMaster = chkIsUsingParamedicMaster.Checked;

            entity.IsUsingBusinessPartner = chkIsUsingBusinessPartner.Checked;
            if (entity.IsUsingBusinessPartner)
            {
                entity.GCBusinessPartnerType = cboGCBusinessPartnerType.Value.ToString();
            }
            else
            {
                entity.GCBusinessPartnerType = null;
            }
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            if (chkIsUsingBusinessPartner.Checked && cboGCBusinessPartnerType.Value == null)
            {
                errMessage = " Please select Business Partner Type first !";
            }
            else
            {
                string FilterExpression = string.Format("GLAccountNo = '{0}' AND IsDeleted = 0", txtGLAccountNo.Text);
                List<ChartOfAccount> lst = BusinessLayer.GetChartOfAccountList(FilterExpression);
                if (lst.Count > 0)
                {
                    errMessage = " COA with number " + txtGLAccountNo.Text + " is already exist!";
                }
            }

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            if (chkIsUsingBusinessPartner.Checked && cboGCBusinessPartnerType.Value == null)
            {
                errMessage = " Please select Business Partner Type first !";
            }
            else
            {
                string FilterExpression = string.Format("GLAccountNo = '{0}' AND GLAccountID != {1} AND IsDeleted = 0", txtGLAccountNo.Text, hdnID.Value);
                List<ChartOfAccount> lst = BusinessLayer.GetChartOfAccountList(FilterExpression);
                if (lst.Count > 0)
                {
                    errMessage = " COA with number " + txtGLAccountNo.Text + " is already exist!";
                }
            }

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ChartOfAccountDao entityDao = new ChartOfAccountDao(ctx);
            try
            {
                ChartOfAccount entity = new ChartOfAccount();
                ControlToEntity(entity);
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();
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
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ChartOfAccountDao entityDao = new ChartOfAccountDao(ctx);
            try
            {
                ChartOfAccount entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
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
    }
}