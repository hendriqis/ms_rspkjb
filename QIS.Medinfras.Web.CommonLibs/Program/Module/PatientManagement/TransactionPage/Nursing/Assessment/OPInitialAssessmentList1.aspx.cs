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
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class OPInitialAssessmentList1 : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        protected static bool _isInitialAssessmentExists = false;

        public override string OnGetMenuCode()
        {
            if (hdnMenuType.Value == "fo")
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_NURSE_ASSESSMENT;
                    default: return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_NURSE_ASSESSMENT;
                }
            }
            else
            {
                switch (hdnDeptType.Value)
                {
                    case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PATIENT_PAGE_NURSE_ASSESSMENT;
                    default: return Constant.MenuCode.Outpatient.PATIENT_PAGE_NURSE_ASSESSMENT;
                }
            }
        }

        public override bool IsEntryUsePopup()
        {
            return false;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
        }

        protected override void InitializeDataControl()
        {
            if (!string.IsNullOrEmpty(Page.Request.QueryString["id"]))
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param.Count() > 1)
                {
                    hdnDeptType.Value = param[0];
                    hdnMenuType.Value = param[1];
                }
                else
                {
                    hdnDeptType.Value = param[0];
                }
            }
            else
            {
                hdnDeptType.Value = Constant.Facility.OUTPATIENT;
                hdnMenuType.Value = "";
            }
            
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvNurseChiefComplaintRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vNurseChiefComplaint> lstEntity = BusinessLayer.GetvNurseChiefComplaintList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ChiefComplaintID DESC");
            _isInitialAssessmentExists = lstEntity.Where(t => t.VisitID == AppSession.RegisteredPatient.VisitID).ToList().Count  > 0;
            grdView.DataSource = lstEntity;
            grdView.DataBind();
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


        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            bool result = true;

            if (_isInitialAssessmentExists) 
            {
                errMessage = "Hanya boleh ada 1 kajian pasien oleh perawat dalam 1 kunjungan.";
                result = false;
            }
            else
            {
                string AssessmentEntry = string.Format("~/libs/Program/Module/PatientManagement/TransactionPage/Nursing/Assessment/OPInitialAssessmentEntry1.aspx?id={0}|{1}|{2}", hdnDeptType.Value, hdnMenuType.Value, hdnID.Value);
                url = ResolveUrl(AssessmentEntry);
                result = true;
            }

            return result;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            bool result = true;

            if (hdnID.Value != "")
            {
                string AssessmentEntry = string.Format("~/libs/Program/Module/PatientManagement/TransactionPage/Nursing/Assessment/OPInitialAssessmentEntry1.aspx?id={0}|{1}|{2}", hdnDeptType.Value, hdnMenuType.Value, hdnID.Value);
                url = ResolveUrl(AssessmentEntry);

                result = true;
            }
            else
            {
                errMessage = "Maaf, Tidak ada Kajian awal yang dapat diedit.";
                result = false;
            }

            return result;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void cbpCompleted_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = CompletedAssessment(param);
                string[] retValInfo = retVal.Split('|');
                if (retValInfo[0] == "1")
                    result += string.Format("success|{0}", string.Empty);
                else
                    result += string.Format("fail|{0}", retValInfo[1]);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = param;
        }

        private string CompletedAssessment(string recordID)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                NurseChiefComplaint obj = BusinessLayer.GetNurseChiefComplaint(id);
                if (obj != null)
                {
                    if (string.IsNullOrEmpty(obj.GCAssessmentStatus) || obj.GCAssessmentStatus == Constant.AssessmentStatus.OPEN )
                    {
                        obj.GCAssessmentStatus = Constant.AssessmentStatus.COMPLETED;
                        obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                        obj.LastUpdatedDate = DateTime.Now;
                        BusinessLayer.UpdateNurseChiefComplaint(obj);
                        result = string.Format("1|{0}", string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}", ex.Message);
            }
            finally
            {
            }
            return result;
        }
        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                try
                {
                    NurseChiefComplaint entity = BusinessLayer.GetNurseChiefComplaint(Convert.ToInt32(hdnID.Value));
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateNurseChiefComplaint(entity);
                    return true;
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    return false;
                }
            }
            return false;
        }
    }
}