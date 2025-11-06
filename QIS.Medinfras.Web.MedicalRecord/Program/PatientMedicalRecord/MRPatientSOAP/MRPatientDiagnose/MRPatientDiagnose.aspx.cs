using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class MRPatientDiagnose : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PATIENT_SOAP;
        }

        protected override void InitializeDataControl()
        {
            ctlToolbar.SetSelectedMenu(1);

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.RM_IS_RM_DIAGNOSE_TEXT_EDITABLE, //1
                                                        Constant.SettingParameter.RM_IS_FOLLOWUP_DEFAULT_CHECKED //2
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsRMDiagnoseTextEditable.Value = lstSetVarDt.FirstOrDefault(a => a.ParameterCode == Constant.SettingParameter.RM_IS_RM_DIAGNOSE_TEXT_EDITABLE).ParameterValue;
            hdnIsFollowUpDefaultChecked.Value = lstSetVarDt.FirstOrDefault(a => a.ParameterCode == Constant.SettingParameter.RM_IS_FOLLOWUP_DEFAULT_CHECKED).ParameterValue;

            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
            int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", AppSession.RegisteredPatient.HealthcareServiceUnitID));
            if (count > 0)
                hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
            else
                hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

            hdnDefaultParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
            ParamedicMaster entity = BusinessLayer.GetParamedicMaster(AppSession.RegisteredPatient.ParamedicID);
            hdnDefaultParamedicCode.Value = entity.ParamedicCode;
            hdnDefaultParamedicName.Value = entity.FullName;

            if (entity.SpecialtyID != null && entity.SpecialtyID != "0" && entity.SpecialtyID != "")
            {
                Specialty entitySpecialty = BusinessLayer.GetSpecialty(entity.SpecialtyID);
                hdnDefaultSpecialtyName.Value = entitySpecialty.SpecialtyName;
                txtSpecialtyName.Text = entitySpecialty.SpecialtyName;
            }

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNutritionDiagnosis = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDiagnosisRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_FIVE);
            }

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression, Constant.GridViewPageSize.GRID_FIVE, pageIndex, "ID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                int recordID = Convert.ToInt32(hdnID.Value);
                PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(recordID);
                if (entity != null)
                {
                    if (entity.ClaimDiagnosisID != null && entity.ClaimDiagnosisID != "")
                    {
                        errMessage = string.Format("Data diagnosa pasien ini tidak dapat dihapus karena sudah dilengkapi oleh Casemix.");
                        return false;
                    }
                    else
                    {
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePatientDiagnosis(entity);

                        return true;
                    }
                }
                else
                {
                    errMessage = string.Format("Invalid Patient Diagnosis Record Information");
                    return false;
                }
            }
            else
            {
                errMessage = string.Format("Invalid Patient Diagnosis Record Information");
                return false;
            }
        }

        #region Entry
        protected override void SetControlProperties()
        {
            txtDifferentialDate.Text = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDifferentialTime.Text = AppSession.RegisteredPatient.VisitTime;

            txtFinalDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtFinalTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            hdnDefaultDateToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnDefaultTimeToday.Value = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}') AND IsDeleted = 0 AND IsActive = 1",
                                                            Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS,
                                                            Constant.StandardCode.DIAGNOSIS_TYPE,
                                                            Constant.StandardCode.VISIT_CASE_TYPE
                                                        );
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Add(new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            Methods.SetComboBoxField<StandardCode>(cboStatus, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDiagnoseType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboVisitCaseType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.VISIT_CASE_TYPE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            ConsultVisit entityCV = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            if (entityCV.SpecialtyID != null && entityCV.SpecialtyID != "0" && entityCV.SpecialtyID != "")
            {
                if (entityCV.GCCaseType != null)
                {
                    hdnDefaultVisitCaseType.Value = entityCV.GCCaseType;
                    cboVisitCaseType.Value = entityCV.GCCaseType;
                }
                else
                {
                    Specialty entitySpecialty = BusinessLayer.GetSpecialty(entityCV.SpecialtyID);
                    if (entitySpecialty.GCCaseType != null)
                    {
                        hdnDefaultVisitCaseType.Value = entitySpecialty.GCCaseType;
                        cboVisitCaseType.Value = entitySpecialty.GCCaseType;
                    }
                    else
                    {
                        hdnDefaultVisitCaseType.Value = "";
                        cboVisitCaseType.Value = "";
                    }
                }
            }
        }

        protected string GetDefaultDiagnosisType()
        {
            return Constant.DiagnoseType.MAIN_DIAGNOSIS;
        }

        protected string GetDefaultDifferentialDiagnosisStatus()
        {
            return Constant.DifferentialDiagnosisStatus.CONFIRMED;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboVisitCaseType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboDiagnoseType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(txtFinalDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFinalTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDiagnoseCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDiagnoseName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtDiagnosisText, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtMorphologyCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMorphologyName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(chkIsFollowUp, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsChronic, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboStatus, new ControlEntrySetting(true, true, true));
        }

        private void ControlToEntity(PatientDiagnosis entity)
        {
            entity.GCDiagnoseType = cboDiagnoseType.Value.ToString();
            entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);

            entity.FinalDate = Helper.GetDatePickerValue(txtFinalDate);
            entity.FinalTime = txtFinalTime.Text;
            entity.FinalDiagnosisID = txtDiagnoseCode.Text;
            entity.FinalDiagnosisText = txtDiagnosisText.Text;
            entity.FinalDiagnosisBy = AppSession.UserLogin.UserID;

            entity.MorphologyID = txtMorphologyCode.Text;
            entity.IsChronicDisease = chkIsChronic.Checked;
            entity.IsFollowUpCase = chkIsFollowUp.Checked;
            entity.GCFinalStatus = cboStatus.Value.ToString();
        }

        private bool IsValidToSave(ref string errMessage, bool IsAddMode)
        {
            string filterExpression = string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS);

            if (hdnEntryID.Value != null && hdnEntryID.Value != "")
            {
                filterExpression += string.Format(" AND ID != {0}", hdnEntryID.Value);
            }
            
            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression);

            if (lstEntity.Count > 0)
            {
                //Validate one episode should only have one main diagnose
                if (cboDiagnoseType.Value.ToString() == Constant.DiagnoseType.MAIN_DIAGNOSIS)
                {
                    errMessage = "Dalam satu episode keperawatan/kunjungan pasien hanya boleh ada 1 diagnosa utama.";
                    return false;
                }
            }
            return true;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDiagnosisDao entityPDiagDao = new PatientDiagnosisDao(ctx);
            ConsultVisitDao entityVisitDao = new ConsultVisitDao(ctx);

            try
            {
                if (IsValidToSave(ref errMessage, true))
                {
                    PatientDiagnosis entity = new PatientDiagnosis();
                    ControlToEntity(entity);
                    entity.GCDifferentialStatus = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
                    entity.VisitID = AppSession.RegisteredPatient.VisitID;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entityPDiagDao.Insert(entity);

                    if (cboVisitCaseType.Value != null && cboVisitCaseType.Value != "")
                    {
                        ConsultVisit entityCV = entityVisitDao.Get(AppSession.RegisteredPatient.VisitID);
                        entityCV.GCCaseType = cboVisitCaseType.Value.ToString();
                        entityCV.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityVisitDao.Update(entityCV);

                        hdnDefaultVisitCaseType.Value = entityCV.GCCaseType;
                    }

                    result = true;
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
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
            PatientDiagnosisDao entityPDiagDao = new PatientDiagnosisDao(ctx);
            ConsultVisitDao entityVisitDao = new ConsultVisitDao(ctx);

            try
            {
                if (IsValidToSave(ref errMessage, true))
                {
                    if (hdnEntryID.Value != null && hdnEntryID.Value != "")
                    {
                        PatientDiagnosis entity = entityPDiagDao.Get(Convert.ToInt32(hdnEntryID.Value));
                        ControlToEntity(entity);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPDiagDao.Update(entity);
                    }

                    if (cboVisitCaseType.Value != null && cboVisitCaseType.Value != "")
                    {
                        ConsultVisit entityCV = entityVisitDao.Get(AppSession.RegisteredPatient.VisitID);
                        entityCV.GCCaseType = cboVisitCaseType.Value.ToString();
                        entityCV.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityVisitDao.Update(entityCV);

                        hdnDefaultVisitCaseType.Value = entityCV.GCCaseType;
                    }

                    result = true;
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
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
        #endregion
    }
}