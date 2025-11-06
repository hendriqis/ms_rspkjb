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
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class BrachytherapyProgramList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected List<vBrachytherapyProgramLog> lstLog = null;
        protected List<vVitalSignDt> lstVitalSignDt = null;

        public override string OnGetMenuCode()
        {
            string menuCode = string.Empty;

            #region Pasien Dalam Perawatan
            switch (hdnDepartmentID.Value)
            {
                case Constant.Module.EMR:
                    menuCode = Constant.MenuCode.EMR.PATIENT_PAGE_RT_RADIOTHERAPHY_PROGRAM_BRACHYTHERAPY;
                    hdnIsEMR.Value = "1";
                    break;
                case Constant.Module.RADIOTHERAPHY:
                    menuCode = Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_RADIOTHERAPHY_PROGRAM_BRACHYTHERAPY;
                    hdnIsEMR.Value = "0";
                    break;
                default:
                    menuCode = Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_RADIOTHERAPHY_PROGRAM_BRACHYTHERAPY;
                    hdnIsEMR.Value = "0";
                    break;
            }
            #endregion

            return menuCode;
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
            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param.Count() > 1)
                {
                    deptType = param[0];
                    menuType = param[1];
                }
                else
                {
                    hdnDepartmentID.Value = Page.Request.QueryString["id"];
                    deptType = param[0];
                }
            }
            else
            {
                hdnDepartmentID.Value = string.Empty;
            }

            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnPageVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnPageMRN.Value = AppSession.RegisteredPatient.MRN.ToString();
            hdnPageMedicalNo.Value = AppSession.RegisteredPatient.MedicalNo;
            hdnPagePatientName.Value = AppSession.RegisteredPatient.PatientName;

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("MRN = {0} AND GCTherapyType = '{1}' AND  IsDeleted = 0", AppSession.RegisteredPatient.MRN, Constant.RadiotherapyType.BRACHYTHERAPY);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRadiotheraphyProgramRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vRadiotheraphyProgram> lstEntity = BusinessLayer.GetvRadiotheraphyProgramList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            string param = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", "0", AppSession.RegisteredPatient.VisitID, AppSession.RegisteredPatient.MRN, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.PatientName, hdnParamedicID.Value);
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Radiotheraphy/RadiotheraphyProgramEntry.ascx");
            queryString = param;
            popupWidth = 800;
            popupHeight = 500;
            popupHeaderText = "Program Radioterapi";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnProgramID.Value != "")
            {
                string param = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", hdnProgramID.Value, AppSession.RegisteredPatient.VisitID, AppSession.RegisteredPatient.MRN, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.PatientName, hdnParamedicID.Value);
                url = ResolveUrl("~/libs/Controls/EMR/_PopupEntry/Radiotherapy/RTProgramEntryCtl.ascx");
                queryString = param;
                popupWidth = 800;
                popupHeight = 500;
                popupHeaderText = "Program Radioterapi";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;

            if (hdnProgramID.Value != "")
            {
                if (IsValidToDelete(hdnProgramID.Value, ref errMessage))
                {
                    RadiotherapyProgram entity = BusinessLayer.GetRadiotherapyProgram(Convert.ToInt32(hdnProgramID.Value));
                    entity.IsDeleted = true;
                    entity.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateRadiotherapyProgram(entity);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool IsValidToDelete(string ID, ref string errMessage)
        {
            StringBuilder message = new StringBuilder();

            List<vRadiotheraphyProgramLog> lstMeasurement = BusinessLayer.GetvRadiotheraphyProgramLogList(string.Format("ProgramID = {0} AND IsDeleted = 0", ID));
            if (lstMeasurement.Count > 0)
            {
                message.AppendLine("Data Program Penyinaran tidak bisa dihapus karena masih ada informasi log penyinaran yang terdapat dalam program ini");
            }

            errMessage = message.ToString();

            return string.IsNullOrEmpty(errMessage);
        }

        #region Detail List
        private void BindGridViewDt1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ProgramID = {0} AND IsDeleted = 0", hdnProgramID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvBrachytherapyProgramLogRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vBrachytherapyProgramLog> lstEntity = BusinessLayer.GetvBrachytherapyProgramLogList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "FractionNo");

            lstLog = lstEntity;

            grdViewDt1.DataSource = lstEntity;
            grdViewDt1.DataBind();
        }

        protected void cbpViewDt1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt1(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt1(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpDeleteLog_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteLog(param);
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

        private string DeleteLog(string recordID)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                BrachytherapyProgramLog obj = BusinessLayer.GetBrachytherapyProgramLog(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateBrachytherapyProgramLog(obj);
                    result = string.Format("1|{0}", string.Empty);
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

        protected void cbpViewDt2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt2(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt2(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewDt2(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ProgramID = {0} AND IsDeleted = 0", hdnProgramID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvBrachytherapySafetyCheckRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vBrachytherapySafetyCheck> lstEntity = BusinessLayer.GetvBrachytherapySafetyCheckList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "FractionNo");

            grdViewDt2.DataSource = lstEntity;
            grdViewDt2.DataBind();
        }

        protected void cbpDeleteCheckListInfo_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string[] paramInfo = param.Split('|');
                string retVal = DeleteCheckListInfo(paramInfo);
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

        private string DeleteCheckListInfo(string[] paramInfo)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                string infoPath = paramInfo[0]; // 1 = Sign In, 2 = Time Out, 3 = Sign Out
                int id = Convert.ToInt32(paramInfo[1]);
                BrachytherapySafetyCheck obj = BusinessLayer.GetBrachytherapySafetyCheck(id);
                if (obj != null)
                {
                    switch (infoPath)
                    {
                        case "1": // Sign In
                            obj.SignInDate = null;
                            obj.SignInTime = null;
                            obj.SignInParamedicID = null;
                            obj.SignInUserID = null;
                            obj.SignInLayout = null;
                            obj.SignInValues = null;
                            break;
                        case "2": // Time Out
                            obj.TimeOutDate = null;
                            obj.TimeOutTime = null;
                            obj.TimeOutParamedicID = null;
                            obj.TimeOutUserID = null;
                            obj.TimeOutLayout = null;
                            obj.TimeOutValues = null;
                            break;
                        case "3": // Sign Out
                            obj.SignOutDate = null;
                            obj.SignOutTime = null;
                            obj.SignOutPhysicianID = null;
                            obj.SignOutAnesthetistID = null;
                            obj.SignOutUserID = null;
                            obj.SignOutLayout = null;
                            obj.SignOutValues = null;
                            break;
                        default:
                            break;
                    }

                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateBrachytherapySafetyCheck(obj);
                    result = string.Format("1|{0}", string.Empty);
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

        protected void cbpDeleteSafetyCheckList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteCheckList(param);
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

        private string DeleteCheckList(string recordID)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                BrachytherapySafetyCheck obj = BusinessLayer.GetBrachytherapySafetyCheck(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateBrachytherapySafetyCheck(obj);
                    result = string.Format("1|{0}", string.Empty);
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

        private void BindGridViewDt3(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ProgramID = {0} AND IsDeleted = 0", hdnProgramID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvBrachytherapyProgramLogRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vBrachytherapyProgramLog> lstEntity = BusinessLayer.GetvBrachytherapyProgramLogList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "FractionNo");

            grdViewDt3.DataSource = lstEntity;
            grdViewDt3.DataBind();
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

        private void BindGridViewDt3_1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (!string.IsNullOrEmpty(hdnIntraProgramLogID.Value) && hdnIntraProgramLogID.Value != "0")
            {
                string filterExpression = string.Format("RadiotherapyProgramLogID = {0} AND IsDeleted = 0", hdnIntraProgramLogID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
                lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("RadiotherapyProgramLogID = {0} AND IsDeleted = 0 ORDER BY DisplayOrder", hdnIntraProgramLogID.Value));
                grdViewDt3_1.DataSource = lstEntity;
                grdViewDt3_1.DataBind();

                hdnHasIntraVitalSignRecord.Value = lstEntity.Count > 0 ? "1" : "0";
            }
        }

        protected void cbpViewDt3_1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt3_1(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt3_1(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpHasRecord"] = hdnHasIntraVitalSignRecord.Value;
        }

        protected void grdViewDt3_1_RowDataBound(object sender, GridViewRowEventArgs e)
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

        private void BindGridViewDt4(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ProgramID = {0} AND IsDeleted = 0", hdnProgramID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvBrachytherapyProcedureReportRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vBrachytherapyProcedureReport> lstEntity = BusinessLayer.GetvBrachytherapyProcedureReportList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "BrachytherapyProcedureReportID DESC");

            grdViewDt4.DataSource = lstEntity;
            grdViewDt4.DataBind();
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

        protected void cbpDeleteReport_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteReport(param);
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

        private string DeleteReport(string recordID)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                BrachytherapyProcedureReport obj = BusinessLayer.GetBrachytherapyProcedureReport(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateBrachytherapyProcedureReport(obj);
                    result = string.Format("1|{0}", string.Empty);
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

        #region Delete Intra
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
        #endregion

        #region Verification Process
        protected void cbpVerification_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = LogVerification(param);
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

        private string LogVerification(string param)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(param);
                BrachytherapyProgramLog obj = BusinessLayer.GetBrachytherapyProgramLog(id);
                if (obj != null)
                {
                    obj.IsVerified = true;
                    obj.VerifiedDateTime = DateTime.Now;
                    obj.VerifiedParamedicID = AppSession.UserLogin.ParamedicID;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateBrachytherapyProgramLog(obj);

                    result = string.Format("1|{0}", string.Empty);
                }
                else
                {
                    result = string.Format("0|{0}", "Invalid Record ID");
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

        protected void cbpCancelVerification_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = CancelVerification(param);
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

        private string CancelVerification(string param)
        {
            string result = string.Empty;
            try
            {
                //Cancel
                int id = Convert.ToInt32(param);
                BrachytherapyProgramLog obj = BusinessLayer.GetBrachytherapyProgramLog(id);
                if (obj != null)
                {

                    obj.IsVerified = false;
                    obj.VerifiedDateTime = null;
                    obj.VerifiedParamedicID = null;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateBrachytherapyProgramLog(obj);

                    result = string.Format("1|{0}", string.Empty);
                }
                else
                {
                    result = string.Format("0|{0}", "Invalid Record ID");
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
        #endregion
        #endregion
    }
}