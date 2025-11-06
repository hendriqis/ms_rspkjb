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
    public partial class PatientMedicalFormEntry : BaseEntryPopupCtl
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
                IsAdd = paramInfo[0] == "0";
                hdnID.Value = paramInfo[0];
                SetControlProperties(paramInfo);
                if (AppSession.UserLogin.ParamedicID != null && AppSession.UserLogin.ParamedicID != 0)
                    hdnUserParamedicName.Value = AppSession.UserLogin.UserFullName;
                else
                    hdnUserParamedicName.Value = string.Empty;
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

            divFormContent.InnerHtml = innerHtml.ToString();
            hdnDivHTML.Value = innerHtml.ToString();
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnFormGroup.Value = paramInfo[1];
            hdnFormType.Value = paramInfo[2];
            hdnReferenceID.Value = paramInfo[3];
            hdnPopupVisitID.Value = paramInfo[4];
            hdnPopupRegistrationID.Value = paramInfo[10];

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND ParamedicID = {2}",
                Constant.ParamedicType.Nurse, AppSession.RegisteredPatient.HealthcareServiceUnitID, paramedicID));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician));
            }

            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

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

                PopulateFormContent();
            }
            else
            {
                cboParamedicID.ClientEnabled = false;

                txtObservationDate.Text = paramInfo[5];
                txtObservationTime.Text = paramInfo[6];
                cboParamedicID.Value = paramInfo[7];
                divFormContent.InnerHtml = paramInfo[8];
                hdnDivHTML.Value = paramInfo[8];
                hdnFormValues.Value = paramInfo[9];
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
            entity.GCAssessmentGroup = hdnFormGroup.Value;
            entity.GCAssessmentType = hdnFormType.Value;
            entity.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entity.AssessmentFormLayout = hdnDivHTML.Value;
            entity.AssessmentFormValue = hdnFormValues.Value;
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entity.TestOrderID = Convert.ToInt32(hdnReferenceID.Value);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientAssessmentDao entityDao = new PatientAssessmentDao(ctx);
            try
            {
                PatientAssessment entity = new PatientAssessment();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                int assessmentID = entityDao.InsertReturnPrimaryKeyID(entity);

                retVal = string.Format("formEntry|{0}", assessmentID.ToString());

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                retVal = string.Format("formEntry|{0}", "0");
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientAssessmentDao entityDao = new PatientAssessmentDao(ctx);
            try
            {
                PatientAssessment entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                retVal = string.Format("formEntry|{0}", entity.AssessmentID.ToString());

                ctx.CommitTransaction();

            }
            catch (Exception ex)
            {
                result = false;
                retVal = string.Format("formEntry|{0}", "0");
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