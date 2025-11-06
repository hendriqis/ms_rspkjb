using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class VaccinationHistoryList1 : BasePagePatientPageList
    {
        string deptType = string.Empty;
        string menuType = string.Empty;
        string vaccinationType = string.Empty;
        protected int PageCount = 1;

        protected override void InitializeDataControl()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param.Count() > 1)
            {
                deptType = param[0];
                menuType = param[1];
                //formGroup = string.Format("X397^{0}", param[2]);
            }
            else
            {
                deptType = param[0];
                //formGroup = string.Format("X397^001");
            }


            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnRegistrationNo.Value = AppSession.RegisteredPatient.RegistrationNo;
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            //hdnGCAssessmentGroup.Value = formGroup;

            BindGridView(1, false, ref PageCount);
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType)
                {
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_VACCINATION;
                    case Constant.Facility.INPATIENT: 
                        return Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_PAGE_VACCINATION;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_PATIENT_PAGE_VACCINATION;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_PAGE_VACCINATION;
                    default: return Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_PAGE_VACCINATION;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                string[] param = Page.Request.QueryString["id"].Split('|');
                switch (param[0])
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_VACCINATION;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.PATIENT_PAGE_VACCINATION;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_VACCINATION;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_VACCINATION;
                    case Constant.Module.RADIOTHERAPHY:
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_VACCINATION_HISTORY;
                    default:
                        return Constant.MenuCode.Outpatient.PATIENT_PAGE_VACCINATION;
                }
                #endregion
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        #region Header
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("IsDeleted = 0 ORDER BY DisplayOrder", string.Empty);

            List<VaccinationType> lstEntity = BusinessLayer.GetVaccinationTypeList(filterExpression);
            grdVaccinTypeList.DataSource = lstEntity;
            grdVaccinTypeList.DataBind();
        }

        protected void cbpVaccinTypeList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        #region Detail
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += string.Format("MRN = {0} AND VaccinationTypeID = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, hdnVaccinationTypeID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVaccinationHistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vVaccinationHistory> lstEntity = BusinessLayer.GetvVaccinationHistoryList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpDelete_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string[] paramInfo = param.Split('|');
                string retVal = DeleteRecord(paramInfo);
                string[] retValInfo = retVal.Split('|');
                if (retValInfo[0] == "1")
                    result += string.Format("success|{0}", string.Empty);
                else
                    result += string.Format("fail|{0}", retValInfo[1]);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = param;
        }

        private string DeleteRecord(string[] paramInfo)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            VaccinationHistoryDao entityDao = new VaccinationHistoryDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(paramInfo[0]);
                VaccinationHistory obj = BusinessLayer.GetVaccinationHistory(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(obj);

                    ctx.CommitTransaction();

                    result = string.Format("1|{0}|{1}", string.Empty, paramInfo[0]);
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = string.Format("0|{0}|{1}", ex.Message, paramInfo[0]);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
    }
}