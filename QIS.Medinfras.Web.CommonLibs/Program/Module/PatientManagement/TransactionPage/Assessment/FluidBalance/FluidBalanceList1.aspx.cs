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
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class FluidBalanceList1 : BasePagePatientPageList
    {
        protected int PageCount = 1;
        protected int PageCount2 = 1;
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected static bool _isInitialAssessmentExists = false;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType)
                {
                    case Constant.Module.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_MONITORING_INTAKE_OUTPUT;
                    case Constant.Module.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_MONITORING_INTAKE_OUTPUT;
                    case Constant.Module.MEDICAL_DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_MONITORING_INTAKE_OUTPUT;
                    case Constant.Module.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_MONITORING_INTAKE_OUTPUT;
                    default:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_MONITORING_INTAKE_OUTPUT;
                }
                #endregion
            } 
            else if (menuType == "dp")
            {
                #region Data Pemeriksaan Pasien
                switch (deptType)
                {
                    case Constant.Module.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.DATA_PATIENT_MONITORING_INTAKE_OUTPUT;
                    default:
                        return Constant.MenuCode.EmergencyCare.DATA_PATIENT_MONITORING_INTAKE_OUTPUT;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case Constant.Module.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.MONITORING_INTAKE_OUTPUT;
                    case Constant.Module.INPATIENT:
                        return Constant.MenuCode.Inpatient.MONITORING_INTAKE_OUTPUT;
                    case Constant.Module.MEDICAL_DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.MONITORING_INTAKE_OUTPUT;
                    case Constant.Module.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.MONITORING_INTAKE_OUTPUT;
                    case Constant.Module.RADIOTHERAPHY:
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_MONITORING_INTAKE_OUTPUT;
                    default:
                        return Constant.MenuCode.Inpatient.MONITORING_INTAKE_OUTPUT;
                }
                #endregion
            }
        }
  
        public override bool IsEntryUsePopup()
        {
            return true;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = false;
            IsAllowEdit = false;
            IsAllowDelete = false;
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
            BindGridView6(1, true, ref PageCount2);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFluidBalanceSummaryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vFluidBalanceSummary> lstEntity = BusinessLayer.GetvFluidBalanceSummaryList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "LogDate DESC");
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

        protected void cbpView6_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView6(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView6(1, true, ref pageCount);
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

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;

            return result;
        }

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        protected void cbpViewDt5_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt5(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt5(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Detail List
        private void BindGridViewDt1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND IsInitializeIntake = 1 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnLogDate.Value, Constant.FluidBalanceGroup.Intake);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFluidBalanceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFluidBalance> lstEntity = BusinessLayer.GetvFluidBalanceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime DESC, ID DESC");

            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind(); 
        }

        private void BindGridViewDt4(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND FluidName = '{3}' AND IsInitializeIntake = 0 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnLogDate.Value, Constant.FluidBalanceGroup.Intake, hdnFluidName.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFluidBalanceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFluidBalance> lstEntity = BusinessLayer.GetvFluidBalanceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime DESC, ID DESC");

            grdViewDt4.DataSource = lstEntity;
            grdViewDt4.DataBind();
        }

        private void BindGridViewDt2(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnLogDate.Value, Constant.FluidBalanceGroup.Output);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFluidBalanceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFluidBalance> lstEntity = BusinessLayer.GetvFluidBalanceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime DESC, ID DESC");

            grdViewDt2.DataSource = lstEntity;
            grdViewDt2.DataBind(); 
        }

        private void BindGridViewDt3(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnLogDate.Value, Constant.FluidBalanceGroup.Output_Tidak_Diukur);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFluidBalanceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFluidBalance> lstEntity = BusinessLayer.GetvFluidBalanceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime ASC, ID ASC");

            grdViewDt3.DataSource = lstEntity;
            grdViewDt3.DataBind();
        }

        private void BindGridViewDt5(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnLogDate.Value, Constant.FluidBalanceGroup.Intake_Tidak_Diukur);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFluidBalanceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFluidBalance> lstEntity = BusinessLayer.GetvFluidBalanceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime DESC, ID DESC");

            grdViewDt5.DataSource = lstEntity;
            grdViewDt5.DataBind();
        }

        private void BindGridView6(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvIVTheraphyNoteRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vIVTheraphyNote> lstEntity = BusinessLayer.GetvIVTheraphyNoteList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "IVTherapyNoteDate DESC");
            grdView6.DataSource = lstEntity;
            grdView6.DataBind();
        }
        #endregion

        #region Callback Panel Event
        protected void cbpDeleteFluidBalance_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string[] paramInfo = param.Split('|');
                string retVal = DeleteFluidBalance(paramInfo);
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

        private string DeleteFluidBalance(string[] paramInfo)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            FluidBalanceDao entityDao = new FluidBalanceDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(paramInfo[1]);
                FluidBalance obj = BusinessLayer.GetFluidBalance(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(obj);

                    if (obj.IsInitializeIntake)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<FluidBalance> lstDetail = BusinessLayer.GetFluidBalanceList(string.Format(
                                                            "VisitID = {0} AND LogDate = '{1}' AND GCFluidGroup = '{2}' AND FluidName = '{3}' AND IsInitializeIntake = 0 AND IsDeleted = 0",
                                                            obj.VisitID, obj.LogDate.ToString(Constant.FormatString.DATE_FORMAT), obj.GCFluidGroup, obj.FluidName), ctx);
                        foreach (FluidBalance dt in lstDetail)
                        {
                            dt.IsDeleted = true;
                            dt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            dt.LastUpdatedDate = DateTime.Now;
                            entityDao.Update(dt);
                        }
                    }

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

        protected void cbpDeleteIVNote_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteIVNote(param);
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

        private string DeleteIVNote(string param)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(param);
                IVTheraphyNote obj = BusinessLayer.GetIVTheraphyNote(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateIVTheraphyNote(obj);
                    result = string.Format("1|{0}|{1}", string.Empty, param);
                }
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}|{1}", ex.Message, param);
            }
            finally
            {
            }
            return result;
        }
        #endregion
    }
}