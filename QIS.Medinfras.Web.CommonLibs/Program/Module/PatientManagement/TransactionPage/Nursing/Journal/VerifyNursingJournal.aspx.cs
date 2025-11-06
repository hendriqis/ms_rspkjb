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
    public partial class VerifyNursingJournal : BasePageTrx
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
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_PAGE_VERIFY_NURSING_JOURNAL;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_VERIFY_NURSING_JOURNAL;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_PATIENT_PAGE_VERIFY_NURSING_JOURNAL;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_VERIFY_NURSING_JOURNAL;
                    default:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_PAGE_VERIFY_NURSING_JOURNAL;
                }
                #endregion
            }
            else if (menuType == "dp")
            {
                #region Data Pemeriksaan Pasien
                switch (deptType)
                {
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_VERIFY_NURSING_JOURNAL;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.DATA_PATIENT_PATIENT_VERIFY_NURSING_JOURNAL;
                    default:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_VERIFY_NURSING_JOURNAL;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_VERIFY_NURSING_JOURNAL;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.PATIENT_PAGE_VERIFY_NURSING_JOURNAL;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_VERIFY_NURSING_JOURNAL;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.PATIENT_VERIFY_NURSING_JOURNAL;
                    case Constant.Facility.LABORATORY:
                        return Constant.MenuCode.Laboratory.PATIENT_VERIFY_NURSING_JOURNAL;
                    case Constant.Module.RADIOTHERAPHY:
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_NURSING_NOTES_CONFIRMATION;
                    default:
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_VERIFY_NURSING_JOURNAL;
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
            filterExpression += string.Format("VisitID = {0} AND IsNeedVerification = 1 AND IsVerified = 0 AND IsDeleted = 0 AND ParamedicID != {1}", AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvNursingJournalRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vNursingJournal> lstEntity = BusinessLayer.GetvNursingJournalList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "JournalDate ASC, JournalTime ASC");
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
            NursingJournalDao entityDao = new NursingJournalDao(ctx);
            string filterExpression = string.Format("ID IN ({0})", lstRecordID);

            try
            {
                if (AppSession.UserLogin.ParamedicID != null)
                {
                    //Confirm
                    List<NursingJournal> oList = BusinessLayer.GetNursingJournalList(filterExpression, ctx);
                    foreach (NursingJournal item in oList)
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

        private void ControlToEntity(PatientVisitNote entity)
        {
        }
        #endregion
    }
}