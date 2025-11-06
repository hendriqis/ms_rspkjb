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
    public partial class SignaEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.SIGNA;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                hdnID.Value = Request.QueryString["id"];
                SetControlProperties();
                Signa entity = BusinessLayer.GetSigna(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtSignaLabel.Focus();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.StandardCode.DRUG_FORM, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.DOSING_FREQUENCY));
            Methods.SetComboBoxField<StandardCode>(cboGCDrugForm, lst.Where(p => p.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCDoseUnit, lst.Where(p => p.ParentID == Constant.StandardCode.ITEM_UNIT).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCDosingFrequency, lst.Where(p => p.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");

            cboGCDrugForm.SelectedIndex = 0;
            cboGCDoseUnit.SelectedIndex = 0;
            cboGCDosingFrequency.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtSignaLabel, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSignaName1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSignaName2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCDrugForm, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDose, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDoseInString, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCDoseUnit, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCDosingFrequency, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFrequency, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(Signa entity)
        {
            txtSignaLabel.Text = entity.SignaLabel;
            txtSignaName1.Text = entity.SignaName1;
            txtSignaName2.Text = entity.SignaName2;
            cboGCDrugForm.Value = entity.GCDrugForm;
            txtDose.Text = entity.Dose.ToString();
            txtDoseInString.Text = entity.DoseInString;
            cboGCDoseUnit.Value = entity.GCDoseUnit;
            cboGCDosingFrequency.Value = entity.GCDosingFrequency;
            txtFrequency.Text = entity.Frequency.ToString();
        }

        private void ControlToEntity(Signa entity)
        {
            entity.SignaLabel = txtSignaLabel.Text;
            entity.SignaName1 = txtSignaName1.Text;
            entity.SignaName2 = txtSignaName2.Text;
            entity.GCDrugForm = cboGCDrugForm.Value.ToString();
            entity.Dose = Convert.ToDecimal(txtDose.Text);
            entity.DoseInString = txtDoseInString.Text;
            entity.GCDoseUnit = cboGCDoseUnit.Value.ToString();
            entity.GCDosingFrequency = cboGCDosingFrequency.Value.ToString();
            entity.Frequency = Convert.ToInt16(txtFrequency.Text);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("SignaLabel = '{0}'", txtSignaLabel.Text);
            List<Signa> lst = BusinessLayer.GetSignaList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Signa with Label " + txtSignaLabel.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("SignaLabel = '{0}' AND SignaID != {1}", txtSignaLabel.Text, hdnID.Value);
            List<Signa> lst = BusinessLayer.GetSignaList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Signa with Label " + txtSignaLabel.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            SignaDao entityDao = new SignaDao(ctx);
            bool result = false;
            try
            {
                Signa entity = new Signa();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetSignaMaxID(ctx).ToString();
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
                Signa entity = BusinessLayer.GetSigna(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateSigna(entity);
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