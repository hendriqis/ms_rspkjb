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
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class VerifyPatientAssessmentForm : BasePageTrx
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            if (menuType == "nt")
            {
                switch (deptType)
                    {
                        case Constant.Facility.EMERGENCY:
                            return Constant.MenuCode.EmergencyCare.NUTRITION_SCREENING_VERIFY;
                        case Constant.Facility.OUTPATIENT:
                            return Constant.MenuCode.Outpatient.NUTRITION_SCREENING_VERIFY;
                        case Constant.Facility.INPATIENT:
                            return Constant.MenuCode.Inpatient.NUTRITION_SCREENING_VERIFY;
                        case Constant.Facility.DIAGNOSTIC:
                            return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_VERIFY_NUTRITION_SCREENING;
                        default:
                            return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_VERIFY_NUTRITION_SCREENING;
                    }
            }
            else if (menuType == "gz")
            {
                #region Menu Gizi
                return Constant.MenuCode.Nutrition.NUTRITION_DIAGNOSTIC_VERIFY_FORM_PENGKAJIAN_GIZI;
                #endregion
            }
            else if (menuType == "fogz")
            {
                #region Follow-up Pasien Pulang Gizi
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_NUTRITION_SCREENING;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_NUTRITION_SCREENING;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_NUTRITION_SCREENING;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_NUTRITION_SCREENING;
                    default:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_NUTRITION_SCREENING;
                }
                #endregion
            }
            else if (menuType == "dpgz")
            {
                #region Data Pemeriksaan Pasien (Verify Skrining Gizi)
                return Constant.MenuCode.EmergencyCare.DATA_PATIENT_NUTRITION_SCREENING_VERIFY;
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                string[] param = Page.Request.QueryString["id"].Split('|');
                switch (param[0])
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.NUTRITION_SCREENING_VERIFY;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.NUTRITION_SCREENING_VERIFY;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.NUTRITION_SCREENING_VERIFY;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_VERIFY_NUTRITION_SCREENING;
                    case Constant.Module.RADIOTHERAPHY:
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_VERIFY_NUTRITION_SCREENING;
                    default:
                        return Constant.MenuCode.EMR.MEDICAL_ASSESSMENT_FORM;
                }
                #endregion
            }  
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
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

            if (menuType == "gz" || menuType == "fogz" || menuType == "dpgz" || menuType == "nt")
            {
                hdnNutritionMenu.Value = "1";
            }
            else
            {
                hdnNutritionMenu.Value = "0";
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
            }

            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();

            if (entity != null)
            {
                hdnPageMedicalNo.Value = entity.MedicalNo;
                hdnPagePatientDOB.Value = entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
                hdnPagePatientName.Value = entity.PatientName;
                hdnPageRegistrationNo.Value = entity.RegistrationNo;
            }

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsNeedVerified = 1 AND IsVerified = 0 AND IsDeleted = 0 AND ParamedicID != {1}", AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientAssessment> lstEntity = BusinessLayer.GetvPatientAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AssessmentDate ASC, AssessmentTime ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpCustomProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string lstRecordID = hdnSelectedID.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            if (param[0] == "complete")
            {
                result = ProcessCheckedItem(param[0], lstRecordID); 
            }

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string ProcessCheckedItem(string type,string lstRecordID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PatientAssessmentDao entityDao = new PatientAssessmentDao(ctx);
            string filterExpression = string.Format("AssessmentID IN ({0})", lstRecordID);

            try
            {
                if (AppSession.UserLogin.ParamedicID != null)
                {
                    //Confirm
                    List<PatientAssessment> oList = BusinessLayer.GetPatientAssessmentList(filterExpression, ctx);
                    foreach (PatientAssessment item in oList)
                    {
                        if (!item.IsVerified && AppSession.UserLogin.ParamedicID != null)
                        {
                            item.IsVerified = true;
                            item.VerifiedDateTime = DateTime.Now;
                            item.VerifiedBy = AppSession.UserLogin.ParamedicID;
                            entityDao.Update(item);
                        }
                    }
                    ctx.CommitTransaction();
                    result = string.Format("process|1|{0}", string.Empty);
                }
                else
                {
                    result = string.Format("process|0|{0}", "Invalid Paramedic / Nurse ID");
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
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

        #endregion
        #region Entry
        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
        }

        private void ControlToEntity(PatientAssessment entity)
        {
        }
        #endregion
    }
}