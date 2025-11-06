using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program.PatientPage
{
    public partial class NurseHandsOverEntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] param2 = param.Split('|');

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            if (string.IsNullOrEmpty(param2[0]) || param2[0] == "0")
            {
                IsAdd = true;
                SetControlProperties();
            }
            else
            {
                hdnFromHealthcareServiceUnitID.Value = param2[1];
                hdnToHealthcareServiceUnitID.Value = param2[2];

                IsAdd = false;
                SetControlProperties();

                List<Registration> dataRegID = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0} OR LinkedToRegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID));
                string lstReg = "";
                if (dataRegID != null)
                {
                    foreach (Registration reg in dataRegID)
                    {
                        if (lstReg != "")
                        {
                            lstReg += ",";
                        }
                        lstReg += reg.RegistrationID;
                    }
                }

                string filterExpression = string.Format("RegistrationID IN ({0}) AND ID = {1} AND IsDeleted = 0", lstReg, param2[0]);

                vPatientNurseTransfer obj = BusinessLayer.GetvPatientNurseTransferList(filterExpression).FirstOrDefault();
                if (obj != null)
                {
                    hdnID.Value = obj.ID.ToString();
                    EntityToControl(obj);
                }
                else
                {
                    hdnID.Value = "0";
                }
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTransferDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTransferTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            //SetControlEntrySetting(cboParamedic, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboParamedic2, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboTransferType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSituationText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBackgroundText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAssessmentText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRecommendationText, new ControlEntrySetting(true, true, true));
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            errMessage = string.Empty;
 

            IDbContext ctx = DbFactory.Configure(true);

            PatientNurseTransferDao transferDao = new PatientNurseTransferDao(ctx);
            NursingJournalDao journalDao = new NursingJournalDao(ctx);

            try
            {
                NursingJournal journal = new NursingJournal();
                journal.VisitID = Convert.ToInt32(hdnVisitID.Value);
                journal.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                journal.JournalDate = Helper.GetDatePickerValue(txtTransferDate);
                journal.JournalTime = txtTransferTime.Text;
                journal.ParamedicID = Convert.ToInt32(cboParamedic.Value.ToString());
                journal.Situation = txtSituationText.Text;
                journal.Background = txtBackgroundText.Text;
                journal.Assessment = txtAssessmentText.Text;
                journal.Recommendation = txtRecommendationText.Text;
                journal.NursingJournalText = string.Format(@"S:{0}{1}{2}B:{3}{4}{5}A:{6}{7}{8}R:{9}{10}{11}",
    Environment.NewLine, txtSituationText.Text, Environment.NewLine,
    Environment.NewLine, txtBackgroundText.Text, Environment.NewLine,
    Environment.NewLine, txtAssessmentText.Text, Environment.NewLine,
    Environment.NewLine, txtRecommendationText.Text, Environment.NewLine);
                journal.IsPatientHandover = true;
                journal.Remarks = journal.NursingJournalText;
                journal.CreatedBy = AppSession.UserLogin.UserID;

                int journalID = journalDao.InsertReturnPrimaryKeyID(journal);

                PatientNurseTransfer entity = new PatientNurseTransfer();
                ControlToEntity(entity);
                entity.GCPatientNurseTransferStatus = Constant.NursePatientTransferStatus.OPEN;
                entity.NursingJournalID = journalID;
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                transferDao.Insert(entity);

                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
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
            errMessage = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);

            PatientNurseTransferDao transferDao = new PatientNurseTransferDao(ctx);
            NursingJournalDao journalDao = new NursingJournalDao(ctx);

            try
            {
                PatientNurseTransfer entity = transferDao.Get(Convert.ToInt32(hdnID.Value));
                if (!string.IsNullOrEmpty(AppSession.HealthcareServiceUnitID))
                {
                    if (Convert.ToInt32(hdnFromHealthcareServiceUnitID.Value) == Convert.ToInt32(AppSession.HealthcareServiceUnitID))
                    {
                        entity.FromHealthcareServiceUnitID = Convert.ToInt32(hdnFromHealthcareServiceUnitID.Value);
                        entity.ToHealthcareServiceUnitID = Convert.ToInt32(cboToHealthcareServiceUnit.Value);
                    }
                    else
                    {
                        entity.FromHealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
                        entity.ToHealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
                    }
                }
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                transferDao.Update(entity);

                NursingJournal journal = journalDao.Get(Convert.ToInt32(entity.NursingJournalID));
                journal.Situation = txtSituationText.Text;
                journal.Background = txtBackgroundText.Text;
                journal.Assessment = txtAssessmentText.Text;
                journal.Recommendation = txtRecommendationText.Text;
                journal.NursingJournalText = string.Format(@"S:{0}{1}{2}B:{3}{4}{5}A:{6}{7}{8}R:{9}{10}{11}",
    Environment.NewLine, txtSituationText.Text, Environment.NewLine,
    Environment.NewLine, txtBackgroundText.Text, Environment.NewLine,
    Environment.NewLine, txtAssessmentText.Text, Environment.NewLine,
    Environment.NewLine, txtRecommendationText.Text, Environment.NewLine);
                journal.IsPatientHandover = true;
                journal.Remarks = journal.NursingJournalText;
                journal.LastUpdatedBy = AppSession.UserLogin.UserID;
                journalDao.Update(journal);

                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        private void ControlToEntity(PatientNurseTransfer entity)
        {
            entity.TransferDate = Helper.GetDatePickerValue(txtTransferDate);
            entity.TransferTime = txtTransferTime.Text;
            entity.GCPatientNurseTransferType = cboTransferType.Value.ToString();
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entity.FromNurseID = Convert.ToInt32(cboParamedic.Value);
            entity.ToHealthcareServiceUnitID = Convert.ToInt32(cboToHealthcareServiceUnit.Value);
            entity.ToNurseID = Convert.ToInt32(cboParamedic2.Value);
            entity.ToVisitID = Convert.ToInt32(hdnVisitID.Value);
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.NURSE_HANDS_OVER_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboTransferType, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.NURSE_HANDS_OVER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboTransferType.SelectedIndex = 0;

            #region Healthcareserviceunit
            List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsDeleted = 0", Constant.Facility.EMERGENCY, Constant.Facility.INPATIENT, Constant.Facility.DIAGNOSTIC, Constant.Facility.OUTPATIENT, Constant.Facility.IMAGING, Constant.Facility.LABORATORY));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboFromHealthcareServiceUnit, lstHealthcareServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboToHealthcareServiceUnit, lstHealthcareServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");

            if (!string.IsNullOrEmpty(AppSession.HealthcareServiceUnitID))
            {
                cboFromHealthcareServiceUnit.Value = AppSession.HealthcareServiceUnitID;
                cboToHealthcareServiceUnit.Value = AppSession.HealthcareServiceUnitID;
            }
            else
            {
                cboFromHealthcareServiceUnit.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                cboToHealthcareServiceUnit.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            }
            cboFromHealthcareServiceUnit.ClientEnabled = false;

            #endregion

            #region Physician Combobox
            if (IsAdd)
            {
                string filterexpression = string.Format("GCParamedicMasterType NOT IN ('{0}','{1}') AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID IN ({2},{3})) AND IsDeleted = 0 AND IsAvailable = 1", Constant.ParamedicType.Physician, Constant.ParamedicType.Pharmacist, cboFromHealthcareServiceUnit.Value, cboToHealthcareServiceUnit.Value);
                List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(filterexpression);
                Methods.SetComboBoxField<vParamedicMaster>(cboParamedic, lstParamedic, "ParamedicName", "ParamedicID");

                string filterexpression2 = string.Format("GCParamedicMasterType NOT IN ('{0}','{1}') AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {2}) AND IsDeleted = 0 AND IsAvailable = 1", Constant.ParamedicType.Physician, Constant.ParamedicType.Pharmacist, cboToHealthcareServiceUnit.Value);
                List<vParamedicMaster> lstParamedic2 = BusinessLayer.GetvParamedicMasterList(filterexpression2);
                Methods.SetComboBoxField<vParamedicMaster>(cboParamedic2, lstParamedic2, "ParamedicName", "ParamedicID");

                cboParamedic.Enabled = false;
                cboParamedic2.Value = string.Empty;
                txtSituationText.Text = string.Empty;
                txtBackgroundText.Text = string.Empty;
                txtAssessmentText.Text = string.Empty;
                txtRecommendationText.Text = string.Empty;
            }
            else
            {
                string filterexpression = string.Format("GCParamedicMasterType NOT IN ('{0}','{1}') AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID IN ({2},{3})) AND IsDeleted = 0 AND IsAvailable = 1", Constant.ParamedicType.Physician, Constant.ParamedicType.Pharmacist, hdnFromHealthcareServiceUnitID.Value, hdnToHealthcareServiceUnitID.Value);
                List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(filterexpression);
                Methods.SetComboBoxField<vParamedicMaster>(cboParamedic, lstParamedic, "ParamedicName", "ParamedicID");

                string filterexpression2 = string.Format("GCParamedicMasterType NOT IN ('{0}','{1}') AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {2}) AND IsDeleted = 0 AND IsAvailable = 1", Constant.ParamedicType.Physician, Constant.ParamedicType.Pharmacist, hdnToHealthcareServiceUnitID.Value);
                List<vParamedicMaster> lstParamedic2 = BusinessLayer.GetvParamedicMasterList(filterexpression2);
                Methods.SetComboBoxField<vParamedicMaster>(cboParamedic2, lstParamedic2, "ParamedicName", "ParamedicID");
            }

            cboParamedic.Value = AppSession.UserLogin.ParamedicID.ToString();

            if (!string.IsNullOrEmpty(AppSession.UserLogin.GCParamedicMasterType))
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboParamedic.ClientEnabled = false;
                cboParamedic.Value = userLoginParamedic.ToString();
            }
            #endregion
        }

        private void EntityToControl(vPatientNurseTransfer obj)
        {
            cboParamedic.Value = obj.FromNurseID.ToString();
            cboParamedic2.Value = obj.ToNurseID.ToString(); ;
            cboTransferType.Value = obj.GCPatientNurseTransferType.ToString();
            cboToHealthcareServiceUnit.Value = obj.ToHealthcareServiceUnitID.ToString();
            txtSituationText.Text = obj.Situation;
            txtBackgroundText.Text = obj.Background;
            txtAssessmentText.Text = obj.Assessment;
            txtRecommendationText.Text = obj.Recommendation;
        }

        protected void cbpTransferType_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }

        protected void cboToHealthcareServiceUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            PopulateToHealthcareServiceUnit();
        }

        protected void cboParamedic2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            PopulateToHealthcareServiceUnitParamedic();
        }

        private void PopulateToHealthcareServiceUnit()
        {
            if (cboTransferType.Value.ToString() == Constant.NurseHandOverType.SHIFT)
            {
                cboToHealthcareServiceUnit.Value = cboFromHealthcareServiceUnit.Value;
                List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsDeleted = 0", Constant.Facility.EMERGENCY, Constant.Facility.INPATIENT, Constant.Facility.DIAGNOSTIC, Constant.Facility.OUTPATIENT, Constant.Facility.IMAGING));
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboToHealthcareServiceUnit, lstHealthcareServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboToHealthcareServiceUnit.DataBind();
                cboToHealthcareServiceUnit.Value = cboFromHealthcareServiceUnit.Value;
                cboToHealthcareServiceUnit.Enabled = false;
                PopulateToHealthcareServiceUnitParamedic();
            }
            else if (cboTransferType.Value.ToString() == Constant.NurseHandOverType.PINDAH_BANGSAL)
            {
                List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsDeleted = 0", Constant.Facility.INPATIENT, Constant.Facility.EMERGENCY, Constant.Facility.DIAGNOSTIC, Constant.Facility.OUTPATIENT, Constant.Facility.IMAGING));
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboToHealthcareServiceUnit, lstHealthcareServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboToHealthcareServiceUnit.DataBind();
                cboToHealthcareServiceUnit.SelectedIndex = 0;
                cboToHealthcareServiceUnit.Enabled = true;
                PopulateToHealthcareServiceUnitParamedic();
            }
            else
            {
                List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.Facility.DIAGNOSTIC, Constant.Facility.IMAGING));
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboToHealthcareServiceUnit, lstHealthcareServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboToHealthcareServiceUnit.DataBind();
                cboToHealthcareServiceUnit.SelectedIndex = 0;
                cboToHealthcareServiceUnit.Enabled = true;
                PopulateToHealthcareServiceUnitParamedic();
            }
        }

        private void PopulateToHealthcareServiceUnitParamedic()
        {
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType NOT IN ('{0}','{1}') AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {2}) AND IsDeleted = 0 AND IsAvailable = 1", Constant.ParamedicType.Physician, Constant.ParamedicType.Pharmacist, cboToHealthcareServiceUnit.Value));
            List<vParamedicMaster> lstParamedic2 = lstParamedic.Where(lst => lst.ParamedicID != AppSession.UserLogin.ParamedicID).ToList();
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedic2, lstParamedic2, "ParamedicName", "ParamedicID");
            cboParamedic2.DataBind();
            cboParamedic2.ClientEnabled = true;
        }
    }
}