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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class IPInitialAssessmentList1 : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        protected static bool _isInitialAssessmentExists = false;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.SOAP_TEMPLATE_INPATIENT_INITIAL_ASSESSMENT_1;
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
            if (Page.Request.QueryString["id"] != null)
            {
                hdnModuleID.Value = Page.Request.QueryString["id"];
            }
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvChiefComplaintRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vChiefComplaint> lstEntity = BusinessLayer.GetvChiefComplaintList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            _isInitialAssessmentExists = lstEntity.Count > 0;
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
                errMessage = "Hanya boleh ada 1 kajian awal pasien rawat inap dalam 1 episode perawatan.";
                result = false;
            }
            else
            {
                url = ResolveUrl("~/Program/PatientPage/SOAP/InitialAssessment/IPInitialAssessmentEntry1.aspx?id=" + hdnID.Value);
                result = true;
            }

            return result;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            bool result = true;

            url = ResolveUrl("~/Program/PatientPage/SOAP/InitialAssessment/IPInitialAssessmentEntry1.aspx?id=" + hdnID.Value);
            result = true;
            
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
                    #region Chief Complaint
                    ChiefComplaint entity = BusinessLayer.GetChiefComplaint(Convert.ToInt32(hdnID.Value));
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateChiefComplaint(entity); 
                    #endregion

                    #region Patient Visit Note
                    PatientVisitNote pvn = BusinessLayer.GetPatientVisitNoteList(string.Format("ChiefComplaintID = {0}", Convert.ToInt32(hdnID.Value))).FirstOrDefault();
                    if (pvn != null)
                    {
                        pvn.IsDeleted = true;
                        pvn.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePatientVisitNote(pvn);
                    } 
                    #endregion


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