using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NursingPatientProblemList : BasePagePatientPageListEntry
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType)
                {
                    case Constant.Module.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_NURSING_ASSESSMENT_PROBLEM;
                    case Constant.Module.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_NURSING_ASSESSMENT_PROBLEM;
                    case Constant.Module.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_NURSING_ASSESSMENT_PROBLEM;
                    case Constant.Module.MEDICAL_DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_NURSING_ASSESSMENT_PROBLEM;
                    default:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_NURSING_ASSESSMENT_PROBLEM;
                }
                #endregion
            }
            else if (menuType == "dp")
            {
                #region Data Pemeriksaan Pasien
                switch (deptType)
                {
                    case Constant.Module.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_NURSING_ASSESSMENT_PROBLEM;
                    case Constant.Module.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.DATA_PATIENT_NURSING_ASSESSMENT_PROBLEM;
                    default:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_NURSING_ASSESSMENT_PROBLEM;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case Constant.Module.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.NURSING_ASSESSMENT_PROBLEM;
                    case Constant.Module.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.NURSING_ASSESSMENT_PROBLEM;
                    case Constant.Module.INPATIENT:
                        return Constant.MenuCode.Inpatient.NURSING_ASSESSMENT_PROBLEM;
                    case Constant.Module.MEDICAL_DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.NURSING_ASSESSMENT_PROBLEM;
                    case Constant.Module.LABORATORY:
                        return Constant.MenuCode.Laboratory.NURSING_ASSESSMENT_PROBLEM;
                    case Constant.Module.IMAGING:
                        return Constant.MenuCode.Imaging.NURSING_ASSESSMENT_PROBLEM;
                    case Constant.Module.RADIOTHERAPHY:
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_NURSING_ASSESSMENT_PROBLEM;
                    default:
                        return Constant.MenuCode.Inpatient.NURSING_ASSESSMENT_PROBLEM;
                }
                #endregion
            }
        }

        #region List
        protected override void InitializeDataControl()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param.Count() > 1)
            {
                deptType = param[0];
                menuType = param[1];
            }
            else
            {
                deptType = param[0];
            }

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                ConsultVisit obj = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", AppSession.RegisteredPatient.LinkedRegistrationID)).FirstOrDefault();
                if (obj != null)
                {
                    hdnLinkedVisitID.Value = obj.VisitID.ToString();
                }
                else
                {
                    hdnLinkedVisitID.Value = "0";
                }
            }
            else
            {
                hdnLinkedVisitID.Value = "0";
            }

            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0},{1}) AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnLinkedVisitID.Value);

            List<vNursingPatientProblem> lstEntity = BusinessLayer.GetvNursingPatientProblemList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');

                BindGridView();
                result = "refresh|1";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                NursingPatientProblem entity = BusinessLayer.GetNursingPatientProblem(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdateNursingPatientProblem(entity);
                return true;
            }
            return false;
        }
        #endregion

        #region Entry
        protected override void SetControlProperties()
        {
            txtDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                    "GCParamedicMasterType NOT IN ('{0}') AND ParamedicID = {1}",
                                                    Constant.ParamedicType.Physician, paramedicID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedic, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedic.SelectedIndex = 0;
            cboParamedic.ClientEnabled = false;

            if (lstParamedic.Count() > 0)
            {
                hdnDefaultParamedicID.Value = lstParamedic.FirstOrDefault().ParamedicID.ToString();
                hdnDefaultParamedicName.Value = lstParamedic.FirstOrDefault().ParamedicName;
            }

            //int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            //List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0})",
            //    AppSession.RegisteredPatient.HealthcareServiceUnitID));
            //if (lstParamedic.Count == 0)
            //{
            //    lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician));
            //}
            //Methods.SetComboBoxField<vParamedicMaster>(cboParamedic, lstParamedic, "ParamedicName", "ParamedicID");
            //hdnDefaultParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            //if (AppSession.UserLogin.GCParamedicMasterType != Constant.ParamedicType.Physician)
            //{
            //    int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
            //    cboParamedic.ClientEnabled = false;
            //    hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
            //    cboParamedic.Value = hdnDefaultParamedicID.Value;
            //}
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtProblemCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtProblemName, new ControlEntrySetting(false, false, false));
        }

        private void ControlToEntity(NursingPatientProblem entity)
        {
            entity.ProblemDate = Helper.GetDatePickerValue(txtDate);
            entity.ProblemTime = txtTime.Text;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.ParamedicID = Convert.ToInt32(cboParamedic.Value);
            entity.ProblemID = Convert.ToInt32(hdnProblemID.Value);
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                NursingPatientProblem entity = new NursingPatientProblem();
                ControlToEntity(entity);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertNursingPatientProblem(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                NursingPatientProblem entity = BusinessLayer.GetNursingPatientProblem(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNursingPatientProblem(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion
    }
}