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
    public partial class IntraHDAssessmentList1 : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected static bool _isInitialAssessmentExists = false;
        protected List<vVitalSignDt> lstVitalSignDt = null;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType.ToUpper())
                {
                    case Constant.Module.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_PAGE_PEMANTAUAN_INTRA_HD;
                    case Constant.Module.MEDICAL_DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_PAGE_PEMANTAUAN_INTRA_HD;
                    default:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_PAGE_PEMANTAUAN_INTRA_HD;
                }
                #endregion
            }
            else if (menuType == "dp")
            {
                #region Data Pemeriksaan Pasien
                switch (deptType.ToUpper())
                {
                    case Constant.Module.MEDICAL_DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.MD035123;
                    default:
                        return Constant.MenuCode.MedicalDiagnostic.MD035123;
                }
                #endregion
            }
            else
            {
                switch (deptType.ToUpper())
                {
                    default:
                        return Constant.MenuCode.MedicalDiagnostic.MD035123;
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
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
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
            hdnOperatingRoomID.Value = AppSession.MD0006;
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();
            hdnPatientDocumentUrl.Value = string.Format(@"{0}/{1}/", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISPatientDocumentsPath.Replace("#MRN", AppSession.RegisteredPatient.MedicalNo));

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPreHDAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPreHDAssessment> lstEntity = BusinessLayer.GetvPreHDAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
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

            string id = string.Format("{0}|{1}|{2}", hdnID.Value, hdnOrderNo.Value, "0");
            switch (hdnSelectedTab.Value)
            {
                case "preSurgeryAssessment":
                    url = ResolveUrl("~/Program/PatientPage/SOAP/Surgery/AssessmentPreOpEntry1.aspx?id=" + id);
                    break;
                case "surgeryReport":
                    url = ResolveUrl("~/Program/PatientPage/SOAP/Surgery/SurgeryReportEntry1.aspx?id=" + id);
                    break;
                default:
                    url = ResolveUrl("~/Program/PatientPage/SOAP/Surgery/AssessmentPreOpEntry1.aspx?id=" + id);
                    break;
            }

            result = true;

            return result;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            bool result = true;
            string id = string.Format("{0}|{1}|{2}", hdnID.Value, hdnOrderNo.Value,"");
            switch (hdnSelectedTab.Value)
            {
                case "preSurgeryAssessment":
                    url = ResolveUrl("~/Program/PatientPage/SOAP/Surgery/AssessmentPreOpEntry1.aspx?id=" + id);
                    break;
                case "surgeryReport":
                    id = string.Format("{0}|{1}|{2}", hdnID.Value,hdnOrderNo.Value, hdnSurgeryReportID.Value);
                    url = ResolveUrl("~/Program/PatientPage/SOAP/Surgery/SurgeryReportEntry1.aspx?id=" + id);
                    break;
                default:
                    url = ResolveUrl("~/Program/PatientPage/SOAP/Surgery/AssessmentPreOpEntry1.aspx?id=" + id);
                    break;
            }
            
            return result;
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
            bool result = true;
            return result;
        }

        protected void cbpViewDt3_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt3(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt3(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt4_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt4(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt4(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Detail List
        private void BindGridViewDt3(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND PreHDAssessmentID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnAssessmentID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ObservationDate DESC, ObservationTime DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID));
            grdViewDt3.DataSource = lstEntity;
            grdViewDt3.DataBind();
        }

        private void BindGridViewDt4(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND PreHDAssessmentID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnAssessmentID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvHSUFluidBalanceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vHSUFluidBalance> lstEntity = BusinessLayer.GetvHSUFluidBalanceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime DESC");

            grdViewDt4.DataSource = lstEntity;
            grdViewDt4.DataBind();
        }

        protected void grdViewDt3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Row.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Row.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = GetVitalSignDt(obj.ID);
                rptVitalSignDt.DataBind();
            }
        }

        protected List<vVitalSignDt> GetVitalSignDt(Int32 ID)
        {
            return lstVitalSignDt.Where(p => p.ID == ID).ToList();
        }
        #endregion

        #region Callback Panel Event
        protected void cbpDeleteIntra_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteIntra(param);
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

        private string DeleteIntra(string recordID)
        {
            string result = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);
            VitalSignHdDao vitalSignHdDao = new VitalSignHdDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                VitalSignHd obj = vitalSignHdDao.Get(Convert.ToInt32(id));
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    vitalSignHdDao.Update(obj);

                    ctx.CommitTransaction();

                    result = string.Format("1|{0}", string.Empty);
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = string.Format("0|{0}", ex.Message);
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        protected void cbpDeleteIntraFluidBalance_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteIntraFluidBalance(param);
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

        private string DeleteIntraFluidBalance(string recordID)
        {
            string result = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);
            HSUFluidBalanceDao oDao = new HSUFluidBalanceDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                HSUFluidBalance obj = oDao.Get(Convert.ToInt32(id));
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    oDao.Update(obj);

                    ctx.CommitTransaction();

                    result = string.Format("1|{0}", string.Empty);
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = string.Format("0|{0}", ex.Message);
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