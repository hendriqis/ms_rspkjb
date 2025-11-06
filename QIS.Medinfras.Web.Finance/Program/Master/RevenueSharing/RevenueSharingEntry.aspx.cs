using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_FORMULA;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string OnGetFormulaTypeBaseTarifCode() 
        {
            return Constant.RevenueSharingFormulaType.BASE_TARIF;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                SetControlProperties(); 
                RevenueSharingHd entity = BusinessLayer.GetRevenueSharingHd(Convert.ToInt32(ID));
                List<RevenueSharingDt> lstEntityDt = BusinessLayer.GetRevenueSharingDtList(string.Format("RevenueSharingID = {0}", hdnID.Value));
                EntityToControl(entity, lstEntityDt);
            }
            else
            {
                SetControlProperties();

                IsAdd = true;
                chkComp1.Checked = true;
                chkComp2.Checked = true;
                chkComp3.Checked = true;
                chkIsSharingInPercentage.Checked = true;
                txtSharingAmount.Text = "100";
            }

            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')",
            Constant.SettingParameter.TARIFF_COMPONENT1_TEXT, Constant.SettingParameter.TARIFF_COMPONENT2_TEXT, Constant.SettingParameter.TARIFF_COMPONENT3_TEXT));

            hdnComp1Text.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT1_TEXT).ParameterValue;
            hdnComp2Text.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT2_TEXT).ParameterValue;
            hdnComp3Text.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT3_TEXT).ParameterValue;

            chkComp1.Text = GetTariffComponent1Text();
            chkComp2.Text = GetTariffComponent2Text();
            chkComp3.Text = GetTariffComponent3Text();

            txtRevenueSharingCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected string GetTariffComponent1Text()
        {
            return hdnComp1Text.Value;
        }

        protected string GetTariffComponent2Text()
        {
            return hdnComp2Text.Value;
        }

        protected string GetTariffComponent3Text()
        {
            return hdnComp3Text.Value;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REVENUE_SHARING_FORMULA_TYPE));
            Methods.SetRadioButtonListField<StandardCode>(rblFormulaType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.REVENUE_SHARING_FORMULA_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            rblFormulaType.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtRevenueSharingCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRevenueSharingName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSharingAmount, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsSharingInPercentage, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsControlCreditCard, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsCreditCardFeeInPercentage, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCreditCardFeeAmount, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(rblFormulaType, new ControlEntrySetting(true, true, false, Constant.RevenueSharingFormulaType.TARIF));
            SetControlEntrySetting(chkIsControlGuaranteePayment, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(RevenueSharingHd entity, List<RevenueSharingDt> lstEntityDt)
        {
            txtRevenueSharingCode.Text = entity.RevenueSharingCode;
            txtRevenueSharingName.Text = entity.RevenueSharingName;
            rblFormulaType.SelectedValue = entity.GCSharingFormulaType;
            txtSharingAmount.Text = entity.SharingAmount.ToString();
            chkIsSharingInPercentage.Checked = entity.IsSharingInPercentage;
            chkIsControlCreditCard.Checked = entity.IsControlCreditCard;
            chkIsCreditCardFeeInPercentage.Checked = entity.IsCreditCardFeeInPercentage;
            txtCreditCardFeeAmount.Text = entity.CreditCardFeeAmount.ToString();
            chkComp1.Checked = entity.IsTariffComp1;
            chkComp2.Checked = entity.IsTariffComp2;
            chkComp3.Checked = entity.IsTariffComp3;
            chkIsControlGuaranteePayment.Checked = entity.IsControlGuaranteePayment;

            #region RevenueSharingDt
            foreach (RepeaterItem item in rptFormulaType.Items)
            {
                HtmlInputHidden hdnGCSharingComponent = (HtmlInputHidden)item.FindControl("hdnGCSharingComponent");
                RevenueSharingDt entityDt = lstEntityDt.FirstOrDefault(p => p.GCSharingComponent == hdnGCSharingComponent.Value);
                if (entityDt != null)
                {
                    TextBox txtAmount = (TextBox)item.FindControl("txtAmount");
                    txtAmount.Text = entityDt.Amount.ToString();
                    CheckBox chkIsInPercentage = (CheckBox)item.FindControl("chkIsInPercentage");
                    chkIsInPercentage.Checked = entityDt.IsPercentage;
                }
            }
            #endregion
        }

        private void ControlToEntity(RevenueSharingHd entity, List<RevenueSharingDt> lstEntityDt)
        {
            entity.RevenueSharingCode = txtRevenueSharingCode.Text;
            entity.RevenueSharingName = txtRevenueSharingName.Text;
            entity.GCSharingFormulaType = rblFormulaType.SelectedValue;
            entity.SharingAmount = Convert.ToDecimal(txtSharingAmount.Text);
            entity.IsSharingInPercentage = chkIsSharingInPercentage.Checked;
            entity.IsControlCreditCard = chkIsControlCreditCard.Checked;
            entity.IsCreditCardFeeInPercentage = chkIsCreditCardFeeInPercentage.Checked;
            entity.CreditCardFeeAmount = Convert.ToDecimal(txtCreditCardFeeAmount.Text);
            entity.IsTariffComp1 = chkComp1.Checked;
            entity.IsTariffComp2 = chkComp2.Checked;
            entity.IsTariffComp3 = chkComp3.Checked;
            entity.IsControlGuaranteePayment = chkIsControlGuaranteePayment.Checked;
            
            #region RevenueSharingDt
            foreach (RepeaterItem item in rptFormulaType.Items)
            {
                HtmlInputHidden hdnGCSharingComponent = (HtmlInputHidden)item.FindControl("hdnGCSharingComponent");

                TextBox txt = (TextBox)item.FindControl("txtAmount");
                CheckBox chk = (CheckBox)item.FindControl("chkIsInPercentage");
                RevenueSharingDt entityDt = new RevenueSharingDt();
                entityDt.GCSharingComponent = hdnGCSharingComponent.Value;
                entityDt.IsPercentage = chk.Checked;
                entityDt.Amount = Convert.ToDecimal(txt.Text);
                lstEntityDt.Add(entityDt);
            }
            #endregion
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("RevenueSharingCode = '{0}'", txtRevenueSharingCode.Text);
            List<RevenueSharingHd> lst = BusinessLayer.GetRevenueSharingHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Manufacturer with Code " + txtRevenueSharingCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("RevenueSharingCode = '{0}' AND RevenueSharingID != {1}", txtRevenueSharingCode.Text, hdnID.Value);
            List<RevenueSharingHd> lst = BusinessLayer.GetRevenueSharingHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Revenue Sharing with Code " + txtRevenueSharingCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            RevenueSharingHdDao entityDao = new RevenueSharingHdDao(ctx);
            RevenueSharingDtDao entityDtDao = new RevenueSharingDtDao(ctx);
            bool result = false;
            try
            {
                RevenueSharingHd entity = new RevenueSharingHd();
                List<RevenueSharingDt> lstEntityDt = new List<RevenueSharingDt>();
                ControlToEntity(entity, lstEntityDt);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                entity.RevenueSharingID = BusinessLayer.GetRevenueSharingHdMaxID(ctx);
                foreach(RevenueSharingDt entityDt in lstEntityDt)
                {
                    entityDt.RevenueSharingID = entity.RevenueSharingID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                }
                retval = BusinessLayer.GetRevenueSharingHdMaxID(ctx).ToString();
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
            IDbContext ctx = DbFactory.Configure(true);
            RevenueSharingHdDao entityDao = new RevenueSharingHdDao(ctx);
            RevenueSharingDtDao entityDtDao = new RevenueSharingDtDao(ctx);
            bool result = false;
            try
            {
                RevenueSharingHd entity = BusinessLayer.GetRevenueSharingHd(Convert.ToInt32(hdnID.Value));
                List<RevenueSharingDt> lstEntityDt = BusinessLayer.GetRevenueSharingDtList(string.Format("RevenueSharingID = {0}", hdnID.Value), ctx);
                List<RevenueSharingDt> lstNewEntityDt = new List<RevenueSharingDt>();
                ControlToEntity(entity, lstNewEntityDt);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                foreach (RevenueSharingDt entityDt in lstNewEntityDt)
                {
                    RevenueSharingDt obj = lstEntityDt.FirstOrDefault(p => p.GCSharingComponent == entityDt.GCSharingComponent);
                    entityDt.RevenueSharingID = entity.RevenueSharingID;
                    if (obj == null)
                    {
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);
                    }
                    else
                    {
                        entityDt.CreatedBy = obj.CreatedBy;
                        entityDt.CreatedDate = obj.CreatedDate;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                    }
                }
                //retval = BusinessLayer.GetRevenueSharingHdMaxID(ctx).ToString();
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            List<StandardCode> lstFormulaType = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID != '{1}' AND IsDeleted = 0", Constant.StandardCode.REVENUE_SHARING_COMPONENT, Constant.RevenueSharingComponent.PARAMEDIC));
            rptFormulaType.DataSource = lstFormulaType;
            rptFormulaType.DataBind();

            rptFormulaTypePreview.DataSource = lstFormulaType;
            rptFormulaTypePreview.DataBind();
        }
    }
}