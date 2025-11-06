using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MSTFormEntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] parameter = param.Split('|');
            if (parameter[0] == "add")
            {
                IsAdd = true;
                hdnVisitID.Value = parameter[1];
                hdnID.Value = "";
                if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                    cboParamedicID.ClientEnabled = false;
                }
                else
                {
                    cboParamedicID.SelectedIndex = 0;
                }
                SetControlProperties();
            }
            else if (parameter[0] == "edit")
            {
                IsAdd = false;
                hdnID.Value = parameter[1];
                SetControlProperties();
                MSTAssessment entity = BusinessLayer.GetMSTAssessment(Convert.ToInt32(hdnID.Value));
                hdnVisitID.Value = entity.VisitID.ToString();
                cboParamedicID.ClientEnabled = false;
                cboParamedicID.Value = entity.ParamedicID.ToString();
                EntityToControl(entity);
            }
        }

        private void SetControlProperties()
        {
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.EM0113));
            hdnEM0113.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0113).ParameterValue;

            txtAssessmentDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAssessmentTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0",
                Constant.StandardCode.MST_WEIGHT_CHANGED, 
                Constant.StandardCode.MST_WEIGHT_CHANGED_GROUP, 
                Constant.StandardCode.MST_DIAGNOSIS
                );
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboGCWeightChangedStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.MST_WEIGHT_CHANGED ).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCWeightChangedGroup, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.MST_WEIGHT_CHANGED_GROUP ).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCMSTDiagnosis, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.MST_DIAGNOSIS ).ToList(), "StandardCodeName", "StandardCodeID");

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND ParamedicID = {2}",
                Constant.ParamedicType.Nurse, AppSession.RegisteredPatient.HealthcareServiceUnitID, paramedicID));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician));
            }

            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
         }

        private void EntityToControl(MSTAssessment entityMST)
        {
            txtAssessmentDate.Text = entityMST.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAssessmentTime.Text = entityMST.AssessmentTime;
            cboGCWeightChangedStatus.Value = entityMST.GCWeightChangedStatus;
            cboGCWeightChangedGroup.Value = entityMST.GCWeightChangedGroup;
            txtOtherMSTDiagnosis.Text = entityMST.OtherMSTDiagnosis;
            cboGCMSTDiagnosis.Value = entityMST.GCMSTDiagnosis;
            txtTotalMST.Text = entityMST.MSTScore.ToString();
            if (entityMST.IsHasSpecificDiagnosis == true)
            {
                rblIsHasSpecificDiagnosis.SelectedValue = "1";
            }
            else
            {
                rblIsHasSpecificDiagnosis.SelectedValue = "0";
            }
            if (entityMST.IsHasWeightLoss == true)
            {
                rblIsHasWeightLoss.SelectedValue = "1";
            }
            else
            {
                rblIsHasWeightLoss.SelectedValue = "0";
            }
            if (entityMST.IsReducedFoodIntake == true)
            {
                rblIsReducedFoodIntake.SelectedValue = "1";
                txtFoodIntakeScore.Text = "1";
            }
            else
            {
                rblIsReducedFoodIntake.SelectedValue = "0";
                txtFoodIntakeScore.Text = "0";
            }
            if (entityMST.IsReadedByNutritionist == true)
            {
                rblIsReadedByNutritionist.SelectedValue = "1";
            }
            else
            {
                rblIsReadedByNutritionist.SelectedValue = "0";
            }
        }

        private void ControlToEntity(MSTAssessment entityMST)
        {
            if (cboGCWeightChangedStatus.Value != null)
            {
                if (!string.IsNullOrEmpty(cboGCWeightChangedStatus.Value.ToString()))
                {
                    entityMST.GCWeightChangedStatus = cboGCWeightChangedStatus.Value.ToString();
                }
            }
            if (cboGCWeightChangedGroup.Value != null)
            {
                if (!string.IsNullOrEmpty(cboGCWeightChangedGroup.Value.ToString()))
                {
                    entityMST.GCWeightChangedGroup = cboGCWeightChangedGroup.Value.ToString();
                }
            }
            if (cboGCMSTDiagnosis.Value != null)
            {
                if (!string.IsNullOrEmpty(cboGCMSTDiagnosis.Value.ToString()))
                {
                    entityMST.GCMSTDiagnosis = cboGCMSTDiagnosis.Value.ToString();
                }
            }
            if (!string.IsNullOrEmpty(txtOtherMSTDiagnosis.Text))
            {
                entityMST.OtherMSTDiagnosis = txtOtherMSTDiagnosis.Text;
            }
            entityMST.IsHasSpecificDiagnosis = rblIsHasSpecificDiagnosis.SelectedValue == "1" ? true : false;
            entityMST.IsHasWeightLoss = rblIsHasWeightLoss.SelectedValue == "1" ? true : false;
            entityMST.IsReducedFoodIntake = rblIsReducedFoodIntake.SelectedValue == "1" ? true : false;
            entityMST.IsReadedByNutritionist = rblIsReadedByNutritionist.SelectedValue == "1" ? true : false;
            entityMST.MSTScore = Convert.ToInt16(Request.Form[txtTotalMST.UniqueID]);
            entityMST.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entityMST.AssessmentDate = Helper.GetDatePickerValue(txtAssessmentDate);
            entityMST.AssessmentTime = txtAssessmentTime.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            MSTAssessmentDao entityDao = new MSTAssessmentDao(ctx);
            VitalSignHdDao vitalSignHdDao = new VitalSignHdDao(ctx);
            VitalSignDtDao vitalSignDtDao = new VitalSignDtDao(ctx);
            try
            {
                int assessmentID = 0;

                MSTAssessment entityMST = new MSTAssessment();
                ControlToEntity(entityMST);

                entityMST.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                entityMST.CreatedBy = AppSession.UserLogin.UserID;
                assessmentID = entityDao.InsertReturnPrimaryKeyID(entityMST);

                if (!string.IsNullOrEmpty(hdnEM0113.Value) && hdnEM0113.Value != "0")
                {
                    VitalSignHd vitalSignHd = new VitalSignHd();

                    vitalSignHd.ObservationDate = Helper.GetDatePickerValue(txtAssessmentDate);
                    vitalSignHd.ObservationTime = txtAssessmentTime.Text;
                    vitalSignHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    vitalSignHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    vitalSignHd.Remarks = string.Format("Digenerate otomatis sebagai hasil pengkajian MST {0}", string.Empty);
                    vitalSignHd.IsInitialAssessment = false;
                    vitalSignHd.MSTAssessmentID = assessmentID;
                    vitalSignHd.IsLinkedToAssessment = true;
                    vitalSignHd.CreatedBy = AppSession.UserLogin.UserID;

                    int headerID = vitalSignHdDao.InsertReturnPrimaryKeyID(vitalSignHd);

                    VitalSignDt vitalSignDt = new VitalSignDt();
                    vitalSignDt.ID = headerID;
                    vitalSignDt.VitalSignID = Convert.ToInt32(hdnEM0113.Value);
                    vitalSignDt.VitalSignValue = Request.Form[txtTotalMST.UniqueID];
                    vitalSignDt.IsAutoGenerated = true;

                    vitalSignDtDao.Insert(vitalSignDt);
                }

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
            MSTAssessmentDao entityDao = new MSTAssessmentDao(ctx);
            VitalSignHdDao vitalSignHdDao = new VitalSignHdDao(ctx);
            VitalSignDtDao vitalSignDtDao = new VitalSignDtDao(ctx);
            try
            {
                MSTAssessment entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                if (!string.IsNullOrEmpty(hdnEM0113.Value) && hdnEM0113.Value != "0")
                {
                    VitalSignHd vitalSignHd = BusinessLayer.GetVitalSignHdList(string.Format("MSTAssessmentID = {0}", entity.ID), ctx).FirstOrDefault();
                    bool isNewRecordVitalSign = false;
                    int headerID = 0;

                    if (vitalSignHd == null)
                    {
                        isNewRecordVitalSign = true;
                        vitalSignHd = new VitalSignHd();
                    }
                    else
                    {
                        headerID = vitalSignHd.ID;
                    }

                    vitalSignHd.ObservationDate = Helper.GetDatePickerValue(txtAssessmentDate);
                    vitalSignHd.ObservationTime = txtAssessmentTime.Text;
                    vitalSignHd.VisitID = AppSession.RegisteredPatient.VisitID;
                    vitalSignHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    vitalSignHd.Remarks = string.Format("Digenerate otomatis sebagai hasil pengkajian MST {0}", string.Empty);
                    vitalSignHd.IsInitialAssessment = false;
                    vitalSignHd.IsLinkedToAssessment = true;
                    vitalSignHd.MSTAssessmentID = entity.ID;

                    if (isNewRecordVitalSign)
                    {
                        vitalSignHd.CreatedBy = AppSession.UserLogin.UserID;
                        headerID = vitalSignHdDao.InsertReturnPrimaryKeyID(vitalSignHd);

                        VitalSignDt vitalSignDt = new VitalSignDt();
                        vitalSignDt.ID = headerID;
                        vitalSignDt.VitalSignID = Convert.ToInt32(hdnEM0113.Value);
                        vitalSignDt.VitalSignValue = Request.Form[txtTotalMST.UniqueID];
                        vitalSignDt.IsAutoGenerated = true;

                        vitalSignDtDao.Insert(vitalSignDt);
                    }
                    else
                    {
                        vitalSignHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        vitalSignHdDao.Update(vitalSignHd);

                        VitalSignDt vitalSignDt = BusinessLayer.GetVitalSignDtList(string.Format("ID = {0} AND VitalSignID = {1}", headerID, hdnEM0113.Value)).FirstOrDefault();
                        if (vitalSignDt != null)
                        {
                            vitalSignDt.VitalSignID = Convert.ToInt32(hdnEM0113.Value);
                            vitalSignDt.VitalSignValue = Request.Form[txtTotalMST.UniqueID];
                            vitalSignDt.IsAutoGenerated = true;
                            vitalSignDtDao.Update(vitalSignDt);
                        }
                        else
                        {
                            vitalSignDt = new VitalSignDt();
                            vitalSignDt.ID = headerID;
                            vitalSignDt.VitalSignID = Convert.ToInt32(hdnEM0113.Value);
                            vitalSignDt.VitalSignValue = Request.Form[txtTotalMST.UniqueID];
                            vitalSignDt.IsAutoGenerated = true;
                            vitalSignDtDao.Insert(vitalSignDt);
                        }
                    }
                }

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