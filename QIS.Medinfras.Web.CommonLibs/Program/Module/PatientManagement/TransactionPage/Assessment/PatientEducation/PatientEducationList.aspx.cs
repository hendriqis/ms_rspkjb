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
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientEducationList : BasePagePatientPageList
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected int PageCount = 1;
        protected List<vPatientEducationDt> lstPatientEducationDt = null;

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType.ToUpper())
                {
                    case Constant.Module.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_PATIENT_PAGE_PATIENT_EDUCATION;
                    case Constant.Module.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_PATIENT_EDUCATION;
                    case Constant.Module.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_PAGE_PATIENT_EDUCATION;
                    case Constant.Module.MEDICAL_DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_PAGE_PATIENT_EDUCATION;
                    default:
                        return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_PATIENT_EDUCATION;
                }
                #endregion
            }
            else if (menuType == "nt")
            {
                #region Gizi
                return Constant.MenuCode.Nutrition.PATIENT_EDUCATION_NUTRITION;
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType.ToUpper())
                {
                    case Constant.Module.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_PATIENT_EDUCATION;
                    case Constant.Module.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.PATIENT_PAGE_PATIENT_EDUCATION;
                    case Constant.Module.INPATIENT:
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_PATIENT_EDUCATION;
                    case Constant.Module.MEDICAL_DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_PATIENT_EDUCATION;
                    case Constant.Module.PHARMACY:
                        return Constant.MenuCode.Pharmacy.PATIENT_PAGE_PATIENT_EDUCATION;
                    case Constant.Module.LABORATORY:
                        return Constant.MenuCode.Laboratory.PATIENT_PAGE_PATIENT_EDUCATION;
                    case Constant.Module.EMR:
                        return Constant.MenuCode.EMR.PATIENT_PAGE_PATIENT_EDUCATION;
                    default:
                        return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_PATIENT_EDUCATION;
                }
                #endregion
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

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

            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            BindGridFormView(1, true, ref PageCount);
            //BindGridView(1, true, ref PageCount);

        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            int cvLinkedID = 0;
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                if (entityLinkedRegistration != null)
                {
                    cvLinkedID = entityLinkedRegistration.VisitID;
                }
            }

            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0},{1}) AND GCEducationFormGroup = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, cvLinkedID, hdnEducationFormGroup.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientEducationFormRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientEducationForm> lstEntity = BusinessLayer.GetvPatientEducationFormList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        #region Subject List
        private void BindGridFormView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0 ORDER BY StandardCodeID", Constant.StandardCode.PATIENT_EDUCATION_TYPE);

            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression);
            grdFormList.DataSource = lstEntity;
            grdFormList.DataBind();
        }

        protected void cbpFormList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridFormView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridFormView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        } 

        #endregion


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

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientEducationForm entity = e.Row.DataItem as vPatientEducationForm;
                HtmlGenericControl divSignature3 = e.Row.FindControl("divSignature3") as HtmlGenericControl;
                HtmlGenericControl divSignature4 = e.Row.FindControl("divSignature4") as HtmlGenericControl;

                if (entity.SignatureName3 == "" || entity.SignatureName3 == null)
                {
                    divSignature3.Style.Add("display", "none");
                }
                if (entity.SignatureName4 == "" || entity.SignatureName4 == null)
                {
                    divSignature4.Style.Add("display", "none");
                }
            }
        }

        protected List<vPatientEducationDt> GetPatientEducationDt(Int32 ID)
        {
            return lstPatientEducationDt.Where(p => p.ID == ID).ToList();
        }


        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/PatientEducation/PatientEducationListCtl1.ascx");
            queryString = string.Format("{0}|{1}", hdnEducationFormGroup.Value, hdnID.Value);
            popupWidth = 750;
            popupHeight = 500;
            popupHeaderText = "Edukasi Pasien";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/PatientEducation/PatientEducationListCtl1.ascx");
                queryString = hdnID.Value;
                popupWidth = 750;
                popupHeight = 500;
                popupHeaderText = "Edukasi Pasien";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            return false;
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
            PatientEducationFormDao entityDao = new PatientEducationFormDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(paramInfo[0]);
                PatientEducationForm obj = entityDao.Get(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(obj);

                    result = string.Format("1|{0}|{1}", string.Empty, paramInfo[0]);
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}|{1}", ex.Message, paramInfo[0]);
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