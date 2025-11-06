using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;
using System.IO;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PreHDAssessmentEntry1 : BasePagePatientPageList
    {
        string deptType = string.Empty;
        string menuType = string.Empty;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalDiagnostic.MD035122;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Page.Request.QueryString["id"] != null)
            {
                hdnID.Value = string.IsNullOrEmpty(Page.Request.QueryString["id"]) ? "0" : Page.Request.QueryString["id"];
            }

            vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}'", AppSession.RegisteredPatient.DepartmentID)).FirstOrDefault();
            hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();

            txtAsessmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnTimeNow1.Value = txtAsessmentTime1.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(0, 2);
            hdnTimeNow2.Value = txtAsessmentTime2.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(3, 2);

            hdnIsChanged.Value = "0";
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsActive = 1 AND IsDeleted = 0",
                Constant.StandardCode.JENIS_PERESEPAN_HD,
                Constant.StandardCode.TEKNIK_HD,
                Constant.StandardCode.JENIS_DIALISER,
                Constant.StandardCode.JENIS_DIALISAT,
                Constant.StandardCode.SATUAN_PEMBILASAN_NACL
                );

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboGCHDType, lstSc.Where(p => p.ParentID == Constant.StandardCode.JENIS_PERESEPAN_HD || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCHDMethod, lstSc.Where(p => p.ParentID == Constant.StandardCode.TEKNIK_HD || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCHDMachineType, lstSc.Where(p => p.ParentID == Constant.StandardCode.JENIS_DIALISER || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCDialysate, lstSc.Where(p => p.ParentID == Constant.StandardCode.JENIS_DIALISAT || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCDialysisBleach, lstSc.Where(p => p.ParentID == Constant.StandardCode.SATUAN_PEMBILASAN_NACL || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            cboGCHDMachineType.SelectedIndex = 0;
            cboGCDialysisBleach.SelectedIndex = 0;

            vPreHDAssessment entity = BusinessLayer.GetvPreHDAssessmentList(string.Format("VisitID = {0} AND ID = {1}", AppSession.RegisteredPatient.VisitID, hdnID.Value)).FirstOrDefault();

            if (entity != null)
            {
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "0";
                Patient oPatient = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN);
                if (oPatient != null)
                {
                    if (oPatient.FirstHemodialysisDate.Year != 1900)
                        txtFirstHDDate.Text = oPatient.FirstHemodialysisDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    else
                        txtFirstHDDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }
            }

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            filterExpression = string.Empty;
            if (paramedicID != 0)
                filterExpression = string.Format("ParamedicID = {0}", paramedicID);
            else
                filterExpression = string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician);

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(filterExpression);
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

            hdnParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

            bool isEnabled = true;
            if (entity != null)
            {
                isEnabled = entity.ParamedicID == AppSession.UserLogin.ParamedicID;
            }

            Helper.SetControlEntrySetting(txtAdditionalRemarks, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtAsessmentDate, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtAsessmentTime1, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtAsessmentTime2, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHDNo, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHFRNo, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHDFMDNo, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHemoperfusionNo, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboGCHDType, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboGCHDMachineType, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboGCDialysate, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHDDuration, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHDFrequency, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtQB, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtQD, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtUFGoal, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtProgProfilingNa, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtProgProfilingUF, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHeparizationDosageInitiate, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHeparizationDosageCirculation, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHeparizationDosageContinues, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHeparizationDosageIntermitten, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");


            if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
            {
                if (!string.IsNullOrEmpty(hdnAssessmentParamedicID.Value) && hdnAssessmentParamedicID.Value != "0")
                {
                    if (Convert.ToInt32(hdnAssessmentParamedicID.Value) != AppSession.UserLogin.ParamedicID)
                        cboParamedicID.Value = Convert.ToInt32(hdnAssessmentParamedicID.Value);
                    else
                        cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                }
                else
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                }
                cboParamedicID.Enabled = false;
            }
            else
            {
                cboParamedicID.SelectedIndex = 0;
            }
        }

        private void EntityToControl(vPreHDAssessment entity)
        {
            hdnAssessmentParamedicID.Value = entity.ParamedicID.ToString();

            if (entity.ID == 0)
            {
                hdnID.Value = "";
            }
            else
            {
                hdnID.Value = entity.ID.ToString();
            }

            txtAsessmentDate.Text = entity.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAsessmentTime1.Text = entity.AssessmentTime.Substring(0, 2);
            txtAsessmentTime2.Text = entity.AssessmentTime.Substring(3, 2);

            #region PreHDAssessment
            txtAdditionalRemarks.Text = entity.AdditionalRemarks;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            txtHDNo.Text = entity.HDNo.ToString("G29");
            txtHFRNo.Text = entity.HFRNo.ToString("G29");
            txtHDFMDNo.Text = entity.HDFMDNo.ToString("G29");
            txtHemoperfusionNo.Text = entity.HemoperfusionNo.ToString("G29");
            txtReuseNo.Text = entity.ReuseNo.ToString("G29");
            txtVolumePriming.Text = entity.VolumePriming.ToString();

            Healthcare heathcare = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
            if (heathcare.Initial == "PHS")
            {
                txtFirstHDDate.Text = entity.FirstHemodialysisDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            else
            {
                if (entity.FirstHemodialysisDate.Year != 1900)
                    txtFirstHDDate.Text = entity.FirstHemodialysisDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                else
                    txtFirstHDDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }

            txtMachineNo.Text = entity.MachineNo;
            cboGCHDType.Value = entity.GCHDType;
            cboGCHDMethod.Value = entity.GCHDMethod;
            cboGCHDMachineType.Value = entity.GCHDMachineType;
            cboGCDialysate.Value = entity.GCDialysate;
            txtDialysateRemarks.Text = entity.DialysateRemarks;
            txtHDDuration.Text = entity.HDDuration.ToString();
            txtHDFrequency.Text = entity.HDFrequency.ToString();
            txtQB.Text = entity.QB.ToString();
            txtQD.Text = entity.QD.ToString();
            txtUFGoal.Text = entity.UFGoal.ToString();
            txtProgProfilingNa.Text = entity.ProgFillingNa.ToString();
            txtProgProfilingUF.Text = entity.ProgFillingUF.ToString();
            txtAdditionalRemarks.Text = entity.AdditionalRemarks;

            optIsHeparization.Checked = entity.HeparinizationStatus == "1";
            optIsWithoutHeparization.Checked = entity.HeparinizationStatus == "2";
            optIsLMWH.Checked = entity.HeparinizationStatus == "3";
            txtHeparizationDosageInitiate.Text = entity.HeparizationDosageInitiate.ToString();
            txtHeparizationDosageCirculation.Text = entity.HeparizationDosageCirculation.ToString();
            txtHeparizationDosageContinues.Text = entity.HeparizationDosageContinues.ToString();
            txtHeparizationDosageIntermitten.Text = entity.HeparizationDosageIntermitten.ToString();

            txtWithoutHeparizationRemarks.Text = entity.WithoutHeparizationRemarks;
            txtLMWHRemarks.Text = entity.LMWHRemarks;

            chkIsDialysisBleach.Checked = entity.IsDialysisBleach;
            txtDialysisBleach.Text = entity.DialysisBleach.ToString();
            cboGCDialysisBleach.Value = entity.GCDialysisBleachUnit;
            #endregion

            hdnIsChanged.Value = "0";
        }
        protected override bool OnCustomButtonClick(string type, ref string messages)
        {
            bool result = true;
            if (type == "save")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PreHDAssessmentDao assesmentDao = new PreHDAssessmentDao(ctx);
                PatientDao patientDao = new PatientDao(ctx);
                bool isAllowSave = true;
                try
                {
                    if (!string.IsNullOrEmpty(hdnAssessmentParamedicID.Value) && hdnAssessmentParamedicID.Value != "0")
                    {
                        if (Convert.ToInt32(hdnAssessmentParamedicID.Value) != AppSession.UserLogin.ParamedicID)
                            isAllowSave = false;
                    }

                    if (isAllowSave)
                    {
                        PreHDAssessment obj = BusinessLayer.GetPreHDAssessmentList(string.Format("VisitID = {0} AND ID = '{1}'", AppSession.RegisteredPatient.VisitID, hdnID.Value), ctx).FirstOrDefault();
                        bool isNewRecord = false;
                        if (obj == null)
                        {
                            isNewRecord = true;
                            obj = new PreHDAssessment();
                        }
                        ControlToEntity(obj);

                        if (isNewRecord)
                        {
                            obj.CreatedBy = AppSession.UserLogin.UserID;
                            hdnID.Value = assesmentDao.InsertReturnPrimaryKeyID(obj).ToString();
                        }
                        else
                        {
                            obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                            assesmentDao.Update(obj);
                        }

                        #region Patient First HD Date
                        Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                        if (oPatient != null)
                        {
                            oPatient.IsHemodialyisPatient = true;
                            oPatient.FirstHemodialysisDate = Helper.GetDatePickerValue(txtFirstHDDate.Text);
                            patientDao.Update(oPatient);
                        }
                        #endregion

                        ctx.CommitTransaction();

                        hdnIsSaved.Value = "1";
                        hdnIsChanged.Value = "0";

                        messages = hdnID.Value;
                    }
                    else
                    {
                        messages = "Maaf, Perubahan Pengkajian Pasien hanya bisa dilakukan oleh Perawat yang melakukan pengkajian pertama kali";
                        hdnIsChanged.Value = "0";

                        ctx.RollBackTransaction();
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    messages = ex.Message;
                    hdnIsSaved.Value = "0";
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            return result;
        }

        private void ControlToEntity(PreHDAssessment obj)
        {
            obj.VisitID = AppSession.RegisteredPatient.VisitID;
            obj.AssessmentDate = Helper.GetDatePickerValue(txtAsessmentDate);
            obj.AssessmentTime = string.Format("{0}:{1}", txtAsessmentTime1.Text, txtAsessmentTime2.Text);
            obj.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            obj.FirstHemodialysisDate = Helper.GetDatePickerValue(txtFirstHDDate.Text);
            obj.HDNo = Convert.ToDecimal(txtHDNo.Text);
            obj.HFRNo = Convert.ToDecimal(txtHFRNo.Text);
            obj.HDFMDNo = Convert.ToDecimal(txtHDFMDNo.Text);
            obj.HemoperfusionNo = Convert.ToDecimal(txtHemoperfusionNo.Text);
            obj.MachineNo = txtMachineNo.Text;
            if (cboGCHDType.Value != null)
            {
                obj.GCHDType = cboGCHDType.Value.ToString();
            }
            if (cboGCHDMethod.Value != null)
            {
                obj.GCHDMethod = cboGCHDMethod.Value.ToString();
            }
            if (cboGCHDMachineType.Value != null)
            {
                obj.GCHDMachineType = cboGCHDMachineType.Value.ToString();
            }
            if (!string.IsNullOrEmpty(txtReuseNo.Text) && Methods.IsNumeric(txtReuseNo.Text))
            {
                obj.ReuseNo = Convert.ToDecimal(txtReuseNo.Text);
            }
            if (!string.IsNullOrEmpty(txtVolumePriming.Text) && Methods.IsNumeric(txtVolumePriming.Text))
            {
                obj.VolumePriming = Convert.ToDecimal(txtVolumePriming.Text);
            }

            if (cboGCDialysate.Value != null)
            {
                obj.GCDialysate = cboGCDialysate.Value.ToString();
            }
            obj.DialysateRemarks = txtDialysateRemarks.Text;

            obj.HDDuration = Convert.ToDecimal(txtHDDuration.Text);
            obj.HDFrequency = Convert.ToInt16(txtHDFrequency.Text);
            obj.QB = Convert.ToDecimal(txtQB.Text);
            obj.QD = Convert.ToDecimal(txtQD.Text);
            obj.UFGoal = Convert.ToDecimal(txtUFGoal.Text);
            obj.ProgFillingNa = txtProgProfilingNa.Text;
            obj.ProgFillingUF = Convert.ToDecimal(txtProgProfilingUF.Text);

            obj.AdditionalRemarks = txtAdditionalRemarks.Text;



            if (optIsHeparization.Checked)
            {
                obj.HeparinizationStatus = "1";
                obj.HeparizationDosageInitiate = Convert.ToDecimal(txtHeparizationDosageInitiate.Text);
                obj.HeparizationDosageCirculation = Convert.ToDecimal(txtHeparizationDosageCirculation.Text);
                obj.HeparizationDosageContinues = Convert.ToDecimal(txtHeparizationDosageContinues.Text);
                obj.HeparizationDosageIntermitten = Convert.ToDecimal(txtHeparizationDosageIntermitten.Text);
            }
            if (optIsWithoutHeparization.Checked)
            {
                obj.HeparinizationStatus = "2";
                obj.WithoutHeparizationRemarks = txtWithoutHeparizationRemarks.Text;
                obj.IsDialysisBleach = chkIsDialysisBleach.Checked;
                if (chkIsDialysisBleach.Checked)
                {
                    obj.DialysisBleach = Convert.ToDecimal(txtDialysisBleach.Text);
                    if (cboGCDialysisBleach.Value != null)
                    {
                        obj.GCDialysisBleachUnit = cboGCDialysisBleach.Value.ToString();
                    }
                }
            }
            if (optIsLMWH.Checked)
            {
                obj.HeparinizationStatus = "3";
                obj.LMWHRemarks = txtLMWHRemarks.Text;
            }
        }
    }
}