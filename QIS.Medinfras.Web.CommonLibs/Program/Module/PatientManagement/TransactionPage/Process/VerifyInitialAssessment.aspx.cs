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
    public partial class VerifyInitialAssessment : BasePageTrx
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                switch (deptType)
                {
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_VERIFY_NURSE_INITIAL_ASSESSMENT;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_VERIFY_NURSE_INITIAL_ASSESSMENT;
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_VERIFY_NURSE_INITIAL_ASSESSMENT;
                    default:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_VERIFY_NURSE_INITIAL_ASSESSMENT;
                }
            }
            else
            {
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_VERIFY_NURSE_INITIAL_ASSESSMENT;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_VERIFY_NURSE_INITIAL_ASSESSMENT;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_VERIFY_NURSE_INITIAL_ASSESSMENT;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.PATIENT_PAGE_VERIFY_NURSE_INITIAL_ASSESSMENT;
                    default:
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_VERIFY_NURSE_INITIAL_ASSESSMENT;
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

            hdnDeptID.Value = deptType;

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
            }

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND GCAssessmentStatus = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.AssessmentStatus.COMPLETED);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvNurseChiefComplaintRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vNurseChiefComplaint> lstEntity = BusinessLayer.GetvNurseChiefComplaintList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ChiefComplaintDate ASC, ChiefComplaintTime ASC");
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
            NurseChiefComplaintDao entityDao = new NurseChiefComplaintDao(ctx);
            ParamedicMasterDao paramedicDao = new ParamedicMasterDao(ctx);

            try
            {
                if (AppSession.UserLogin.ParamedicID != null)
                {
                    ParamedicMaster pm = paramedicDao.Get(Convert.ToInt32(AppSession.UserLogin.ParamedicID));
                    if (pm.IsPrimaryNurse)
                    {
                        //Verification
                        string filterExpression = string.Format("ID IN ({0})", lstRecordID);
                        List<NurseChiefComplaint> oList = BusinessLayer.GetNurseChiefComplaintList(filterExpression, ctx);
                        foreach (NurseChiefComplaint item in oList)
                        {
                            if (!item.IsVerifiedByPrimaryNurse && AppSession.UserLogin.ParamedicID != null)
                            {
                                item.GCAssessmentStatus = Constant.AssessmentStatus.VERIFIKASI_PP;
                                item.IsVerifiedByPrimaryNurse = true;
                                item.PrimaryNurseVerifiedDateTime = DateTime.Now;
                                item.PrimaryNurseID = AppSession.UserLogin.ParamedicID;
                                item.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDao.Update(item);
                            }
                        }
                        ctx.CommitTransaction();
                        result = string.Format("process|1|{0}", string.Empty);
                    }
                    else
                    {
                        result = string.Format("process|0|{0}", "Not Primary Nurse");
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = string.Format("process|0|{0}", "Invalid NurseID");
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