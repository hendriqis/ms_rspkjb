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
using System.Text;
using System.IO;
using DevExpress.Web.ASPxCallbackPanel;
using System.Net;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientAssessmentFormEntry : BasePagePatientPageEntryCtl
    {

        protected string GetUserParamedicName()
        {
            return hdnUserParamedicName.Value;
        }

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                string isNutritionMenu = paramInfo[9];
                IsAdd = paramInfo[0] == "1";
                SetControlProperties(paramInfo);
                if (AppSession.UserLogin.ParamedicID != null && AppSession.UserLogin.ParamedicID != 0)
                    hdnUserParamedicName.Value = AppSession.UserLogin.UserFullName;
                else
                    hdnUserParamedicName.Value = string.Empty;

                if (IsAdd)
                {
                    PopulateFormContent();
                }

                if (isNutritionMenu == "1")
                {
                    trIsNeedVerify.Attributes.Remove("style");
                    trToddlerNutritionProblem.Attributes.Remove("style");
                }
                else
                {
                    trIsNeedVerify.Attributes.Add("style", "display:none");
                    trToddlerNutritionProblem.Attributes.Add("style", "display:none");
                }
            }
        }


        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            string fileName = string.Format(@"{0}\medicalForm\General\{1}.html", filePath, hdnFormType.Value.Replace('^', '_'));
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            hdnFormLayout.Value = innerHtml.ToString();
            divFormContent.InnerHtml = innerHtml.ToString();
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnFormType.Value = paramInfo[1];
            hdnID.Value = paramInfo[2];
            hdnFormGroup.Value = paramInfo[4];
            if (paramInfo.Length >= 9)
            {
                txtMedicalNo.Text = paramInfo[5];
                txtPatientName.Text = paramInfo[6];
                txtDateOfBirth.Text = paramInfo[7];
                txtRegistrationNo.Text = paramInfo[8];
            }

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                        "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND ParamedicID = {2}",
                                                        Constant.ParamedicType.Nurse, AppSession.RegisteredPatient.HealthcareServiceUnitID, paramedicID));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician));
            }
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

            string filterSC = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1",
                                                    Constant.StandardCode.TODDLER_NUTRITION_PROBLEM //0
                                                );
            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(filterSC);
            lstSC.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboToddlerNutritionProblem, lstSC.Where(sc => sc.ParentID == Constant.StandardCode.TODDLER_NUTRITION_PROBLEM || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

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
            else
            {
                cboParamedicID.ClientEnabled = false;

                PatientAssessment obj = BusinessLayer.GetPatientAssessment(Convert.ToInt32(hdnID.Value));
                if (obj != null)
                {
                    txtObservationDate.Text = obj.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtObservationTime.Text = obj.AssessmentTime;
                    cboParamedicID.Value = obj.ParamedicID.ToString();
                    hdnFormLayout.Value = obj.AssessmentFormLayout;
                    hdnFormValues.Value = obj.AssessmentFormValue;
                    divFormContent.InnerHtml = hdnFormLayout.Value;
                    chkIsInitialAssessment.Checked = obj.IsInitialAssessment;
                    chkIsNeedVerify.Checked = obj.IsNeedVerified;
                    cboToddlerNutritionProblem.Value = obj.GCToddlerNutritionProblem;
                }
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
            entity.AssessmentFormLayout = hdnFormLayout.Value;
            entity.AssessmentFormValue = hdnFormValues.Value;
            entity.IsNeedVerified = chkIsNeedVerify.Checked;
            if (cboToddlerNutritionProblem.Value != null)
            {
                entity.GCToddlerNutritionProblem = cboToddlerNutritionProblem.Value.ToString();
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
                entity.GCAssessmentGroup = hdnFormGroup.Value;
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
                PatientAssessment entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
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