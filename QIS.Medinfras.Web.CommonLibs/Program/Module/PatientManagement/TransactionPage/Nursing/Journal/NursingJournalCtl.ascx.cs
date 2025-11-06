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
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NursingJournalCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnUserID.Value = AppSession.UserLogin.UserID.ToString();

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", 
                Constant.SettingParameter.EM_Display_Billing_Notification_NurseJournal));
            string displaychkbill = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.EM_Display_Billing_Notification_NurseJournal).FirstOrDefault().ParameterValue;
            if (displaychkbill == "1")
            {
                tdchkIsBillingInformation.Attributes.Remove("style");
            }
            else
            {
                tdchkIsBillingInformation.Attributes.Add("style","display:none");
           }


                if (param != "")
                {
                    IsAdd = false;
                    hdnID.Value = param;
                    OnControlEntrySettingLocal();
                    ReInitControl();
                    vNursingJournal entity = BusinessLayer.GetvNursingJournalList(string.Format("ID = {0}", Convert.ToInt32(hdnID.Value))).FirstOrDefault();
                    EntityToControl(entity);
                    divSaveAsNewTemplate.Style.Add("display", "none");
                }
                else
                {
                    OnControlEntrySettingLocal();
                    ReInitControl();
                    hdnID.Value = "";
                    hdnIsAddNew.Value = "1";
                    IsAdd = true;
                }
        }

        private void SetControlProperties()
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                    "GCParamedicMasterType IN ('{0}','{1}','{2}') AND ParamedicID = {3}",
                                                    Constant.ParamedicType.Nurse, Constant.ParamedicType.Bidan, Constant.ParamedicType.Nutritionist, paramedicID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0",
        Constant.StandardCode.JENIS_CATATAN_PERAWAT));
            lstCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboNursingJournalType, lstCode, "StandardCodeName", "StandardCodeID");

            cboNursingJournalType.SelectedIndex = 0;
        }

        private void OnControlEntrySettingLocal()
        {
            SetControlEntrySetting(txtNoteDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtNoteTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlProperties();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Nurse
                    || AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Bidan
                    || AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Nutritionist)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(false, false, true, userLoginParamedic.ToString()));
            }
            else
            {
                SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true));
            }
        }

        private void EntityToControl(vNursingJournal entity)
        {
            cboNursingJournalType.Value = entity.GCNursingJournalType;
            txtNoteDate.Text = entity.JournalDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNoteTime.Text = entity.JournalTime;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            txtNoteText.Text = entity.Remarks;
            chkIsNeedConfirmation.Checked = entity.IsNeedVerification;
            chkIsBillingInformation.Checked = entity.IsBillingInformation;
            hdnTransactionID.Value = entity.ChargeTransactionID.ToString();
            txtTransactionNo.Text = entity.ChargeTransactionNo;
        }

        private void ControlToEntity(NursingJournal entity)
        {
            if (cboNursingJournalType.Value == null)
            {
                entity.GCNursingJournalType = "X540^001";
            }
            else
            {
                if (!string.IsNullOrEmpty(cboNursingJournalType.Value.ToString()))
                {
                    entity.GCNursingJournalType = cboNursingJournalType.Value.ToString();
                }
            }
            entity.JournalDate = Helper.GetDatePickerValue(txtNoteDate);
            entity.JournalTime = txtNoteTime.Text;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            if (string.IsNullOrEmpty(AppSession.HealthcareServiceUnitID) || AppSession.HealthcareServiceUnitID == "0")
            {
                entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            }
            else
            {
                entity.HealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
            }
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entity.Remarks = txtNoteText.Text;
            entity.IsNeedVerification = chkIsNeedConfirmation.Checked;
            entity.IsBillingInformation = chkIsBillingInformation.Checked;
            if (!string.IsNullOrEmpty(hdnTransactionID.Value))
            {
                entity.ChargeTransactionID = Convert.ToInt32(hdnTransactionID.Value); 
            }
            entity.IsBillingInformation = chkIsBillingInformation.Checked;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            bool isSaveAsNewTemplate = chkSaveAsNewTemplate.Checked;
            try
            {
                IDbContext ctx = DbFactory.Configure(true);
                NursingJournal entity = new NursingJournal();
                ParamedicTextDao paramedicTextDao = new ParamedicTextDao(ctx);
                ParamedicText paramedicText = null;

                #region Save as New Template
                if (isSaveAsNewTemplate && hdnIsAddNew.Value == "1")
                {
                    paramedicText = new ParamedicText();
                    paramedicText.GCTextTemplateGroup = Constant.ParamedicTemplateTextType.NURSING_JOURNAL_TEMPLATE;
                    paramedicText.UserID = AppSession.UserLogin.UserID;
                    paramedicText.TemplateText = txtNoteText.Text;
                    paramedicText.CreatedBy = AppSession.UserLogin.UserID;

                    int rowCount = BusinessLayer.GetvParamedicTextRowCount(string.Empty, ctx);
                    string templateCode = "NJ" + (rowCount + 1).ToString().PadLeft(3, '0');
                    paramedicText.TemplateCode = templateCode;
                    int templateID = paramedicTextDao.InsertReturnPrimaryKeyID(paramedicText);
                }
                #endregion

                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertNursingJournal(entity);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                NursingJournal entityUpdate = BusinessLayer.GetNursingJournal(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingJournal(entityUpdate);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

       
    
    }
}