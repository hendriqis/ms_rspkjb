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
    public partial class PhysicianInstructionList : BasePageTrx
    {
        string menuType = string.Empty;
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_PHYSICIAN_INSTRUCTION;
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.FOLLOWUP_PATIENT_PAGE_PHYSICIAN_INSTRUCTION;
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_PAGE_PHYSICIAN_INSTRUCTION;
                    default: return Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_PAGE_PHYSICIAN_INSTRUCTION;
                }
            }
            else
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PATIENT_PAGE_PHYSICIAN_INSTRUCTION;
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_PHYSICIAN_INSTRUCTION;
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_PAGE_PHYSICIAN_INSTRUCTION;
                    default: return Constant.MenuCode.Inpatient.PATIENT_PAGE_PHYSICIAN_INSTRUCTION;
                }
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        #region List
        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param.Length > 1)
                {
                    hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
                    menuType = param[1];
                }
                else
                {
                    hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
                }
            }

            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
            }

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.LinkedRegistrationID)).FirstOrDefault();
            int cvLinkedID = 0;
            if (entityLinkedRegistration != null)
            {
                cvLinkedID = entityLinkedRegistration.VisitID;
            }

            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0},{1}) AND IsCompleted = 0 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, cvLinkedID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientInstructionRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientInstruction> lstEntity = BusinessLayer.GetvPatientInstructionList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "InstructionDate, InstructionTime, PatientInstructionID DESC");
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
                result = CompletePhysicianInstruction(param[0], lstRecordID); 
            }

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string CompletePhysicianInstruction(string type,string lstRecordID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PatientInstructionDao entityDao = new PatientInstructionDao(ctx);
            string filterExpression = string.Format("PatientInstructionID IN ({0})", lstRecordID);

            try
            {
                if (AppSession.UserLogin.ParamedicID != null)
                {
                    //Confirm
                    List<PatientInstruction> oList = BusinessLayer.GetPatientInstructionList(filterExpression, ctx);
                    foreach (PatientInstruction instruction in oList)
                    {
                        if (!instruction.IsCompleted && AppSession.UserLogin.ParamedicID != null)
                        {
                            instruction.IsCompleted = true;
                            instruction.ExecutedDateTime = DateTime.Now;
                            instruction.ExecutedBy = AppSession.UserLogin.ParamedicID;
                            entityDao.Update(instruction);
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

        private void ControlToEntity(PatientVisitNote entity)
        {
        }
        #endregion
    }
}