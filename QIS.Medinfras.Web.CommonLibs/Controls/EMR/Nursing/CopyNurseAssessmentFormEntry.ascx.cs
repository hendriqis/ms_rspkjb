using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CopyNurseAssessmentFormEntry : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                if (param.Contains("X401^113") || param.Contains("X401^004") || param.Contains("X401^003") || param.Contains("X401^013") || param.Contains("X401^017"))
                {
                    paramInfo = param.Split('@');
                }
                IsAdd = true;
                SetControlProperties(paramInfo);
            }
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnFormType.Value = paramInfo[0];
            hdnVisitID.Value = paramInfo[1];
            hdnIDCopyCtl.Value = paramInfo[2];
            hdnFormLayoutCopyCtl.Value = paramInfo[3];
            hdnFormValuesCopyCtl.Value = paramInfo[4];
            divFormContent.InnerHtml = hdnFormLayoutCopyCtl.Value;

            if (paramInfo.Length >= 9)
            {
                txtMedicalNo.Text = paramInfo[5];
                txtPatientName.Text = paramInfo[6];
                txtDateOfBirth.Text = paramInfo[7];
                txtRegistrationNo.Text = paramInfo[8];
            }

            PatientAssessment oEntity = BusinessLayer.GetPatientAssessment(Convert.ToInt32(hdnIDCopyCtl.Value));

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND ParamedicID = {2}",
                Constant.ParamedicType.Nurse, AppSession.RegisteredPatient.HealthcareServiceUnitID, paramedicID));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician));
            }
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

            string isNutritionMenu = "0";
            if (paramInfo.Count() > 9)
            {
                isNutritionMenu = paramInfo[9];
            }
            if (isNutritionMenu == "1")
            {
                trToddlerNutritionProblem.Attributes.Remove("style");
            }
            else
            {
                trToddlerNutritionProblem.Attributes.Add("style", "display:none");
            }

            string filterSC = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1",
                                                    Constant.StandardCode.TODDLER_NUTRITION_PROBLEM //0
                                                );
            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(filterSC);
            lstSC.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboToddlerNutritionProblemCopyCtl, lstSC.Where(sc => sc.ParentID == Constant.StandardCode.TODDLER_NUTRITION_PROBLEM || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            if (oEntity.GCToddlerNutritionProblem != null)
            {
                cboToddlerNutritionProblemCopyCtl.Value = oEntity.GCToddlerNutritionProblem;
            }
            else
            {
                cboToddlerNutritionProblemCopyCtl.Value = "";
            }

            if (IsAdd)
            {
                if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                    cboParamedicID.ClientEnabled = false;
                }
                else
                {
                    cboParamedicID.SelectedIndex = 0;
                }
                chkIsInitialAssessment.Checked = false;
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
        }

        private void ControlToEntity(PatientAssessment entity)
        {
            entity.AssessmentDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.AssessmentTime = txtObservationTime.Text;
            entity.AssessmentFormLayout = hdnFormLayoutCopyCtl.Value;
            entity.AssessmentFormValue = hdnFormValuesCopyCtl.Value;
            if (cboToddlerNutritionProblemCopyCtl.Value != null)
            {
                entity.GCToddlerNutritionProblem = cboToddlerNutritionProblemCopyCtl.Value.ToString();
            }
            else
            {
                entity.GCToddlerNutritionProblem = null;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientAssessmentDao entityDao = new PatientAssessmentDao(ctx);
            try
            {
                PatientAssessment entity = new PatientAssessment();
                ControlToEntity(entity);
                entity.GCAssessmentType = hdnFormType.Value;
                entity.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entity.IsInitialAssessment = chkIsInitialAssessment.Checked;
                entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.AssessmentID = entityDao.InsertReturnPrimaryKeyID(entity);

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
            PatientAssessmentDao entityDao = new PatientAssessmentDao(ctx);
            RegistrationDao regDao = new RegistrationDao(ctx);
            try
            {
                PatientAssessment entity = entityDao.Get(Convert.ToInt32(hdnIDCopyCtl.Value));
                ControlToEntity(entity);
                entity.IsInitialAssessment = chkIsInitialAssessment.Checked;
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}