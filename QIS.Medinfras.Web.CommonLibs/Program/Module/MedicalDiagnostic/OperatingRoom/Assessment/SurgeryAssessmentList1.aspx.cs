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
    public partial class SurgeryAssessmentList1 : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected static bool _isInitialAssessmentExists = false;

        public override string OnGetMenuCode()
        {
            if (hdnMenuType.Value == "fo")
            {
                return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_ASESMEN_KAMAR_OPERASI;
            }
            else
            {
                switch (hdnDeptType.Value)
                {
                    case Constant.Module.MEDICAL_DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.ASESMEN_KAMAR_OPERASI;
                    default:
                        return Constant.MenuCode.MedicalDiagnostic.ASESMEN_KAMAR_OPERASI;
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
                hdnDeptType.Value = Constant.Module.MEDICAL_DIAGNOSTIC;
                hdnMenuType.Value = "";
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
            int cvLinkedID = AppSession.RegisteredPatient.VisitID;

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                if (entityLinkedRegistration != null)
                {
                    cvLinkedID = entityLinkedRegistration.VisitID;
                }
            }

            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0},{4}) AND HealthcareServiceUnitID = {1} AND GCOrderStatus NOT IN ('{2}') AND GCTransactionStatus NOT IN ('{3}')", AppSession.RegisteredPatient.VisitID, hdnOperatingRoomID.Value, Constant.OrderStatus.CANCELLED, Constant.TransactionStatus.VOID, cvLinkedID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvSurgeryTestOrderHd2RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vSurgeryTestOrderHd2> lstEntity = BusinessLayer.GetvSurgeryTestOrderHd2List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
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
            bool result = true;

            switch (hdnSelectedTab.Value)
            {
                case "preSurgeryAssessment":
                    if (hdnID.Value != "" && hdnID.Value != "0")
                    {
                        IDbContext ctx = DbFactory.Configure(true);
                        PreSurgeryAssessmentDao assesmentDao = new PreSurgeryAssessmentDao(ctx);
                        VitalSignHdDao vitalSignHdDao = new VitalSignHdDao(ctx);
                        ReviewOfSystemHdDao rosHDDao = new ReviewOfSystemHdDao(ctx);
                        PatientBodyDiagramHdDao bodyDiagramHdDao = new PatientBodyDiagramHdDao(ctx);

                        try
                        {
                            PreSurgeryAssessment entity = assesmentDao.Get(Convert.ToInt32(hdnAssessmentID.Value));
                            if (entity != null)
                            {
                                entity.IsDeleted = true;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                assesmentDao.Update(entity);

                                string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnID.Value);
                                VitalSignHd obj1 = BusinessLayer.GetVitalSignHdList(filterExpression, ctx).FirstOrDefault();
                                if (obj1 != null)
                                {
                                    obj1.IsDeleted = true;
                                    obj1.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    vitalSignHdDao.Update(obj1);
                                }

                                ReviewOfSystemHd obj2 = BusinessLayer.GetReviewOfSystemHdList(filterExpression, ctx).FirstOrDefault();
                                if (obj2 != null)
                                {
                                    obj2.IsDeleted = true;
                                    obj2.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    rosHDDao.Update(obj2);
                                }

                                List<PatientBodyDiagramHd> lstBodyDiagram = BusinessLayer.GetPatientBodyDiagramHdList(filterExpression, ctx);
                                foreach (PatientBodyDiagramHd item in lstBodyDiagram)
                                {
                                    item.IsDeleted = true;
                                    item.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    bodyDiagramHdDao.Update(item);
                                }

                                ctx.CommitTransaction();
                                result = true;
                            }
                            else
                            {
                                ctx.RollBackTransaction();
                                errMessage = "Tidak ada kajian asesmen pra bedah yang ditemukan untuk pasien ini.";
                                result = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            errMessage = ex.Message;
                            result = false;
                        }
                        finally
                        {
                            ctx.Close();
                        }
                    }
                    break;
                case "surgeryReport":
                    if (hdnSurgeryReportID.Value != "" && hdnSurgeryReportID.Value != "0")
                    {
                        IDbContext ctx = DbFactory.Configure(true);
                        PatientSurgeryDao assesmentDao = new PatientSurgeryDao(ctx);
                        PatientSurgeryProcedureGroupDao procedureGroupDao = new PatientSurgeryProcedureGroupDao(ctx);
                        PatientSurgeryTeamDao teamDao = new PatientSurgeryTeamDao(ctx);

                        try
                        {
                            PatientSurgery entity = assesmentDao.Get(Convert.ToInt32(hdnSurgeryReportID.Value));
                            if (entity != null)
                            {
                                entity.IsDeleted = true;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                assesmentDao.Update(entity);

                                string filterExpression = string.Format("PatientSurgeryID = {0} AND IsDeleted = 0", hdnSurgeryReportID.Value);
                                List<PatientSurgeryProcedureGroup> lstGroup = BusinessLayer.GetPatientSurgeryProcedureGroupList(filterExpression, ctx);
                                foreach (PatientSurgeryProcedureGroup obj1 in lstGroup)
                                {
                                    if (obj1 != null)
                                    {
                                        obj1.IsDeleted = true;
                                        obj1.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        procedureGroupDao.Update(obj1);
                                    }                                    
                                }

                                List<PatientSurgeryTeam> lstTeam = BusinessLayer.GetPatientSurgeryTeamList(filterExpression, ctx);
                                foreach (PatientSurgeryTeam obj2 in lstTeam)
                                {
                                    if (obj2 != null)
                                    {
                                        obj2.IsDeleted = true;
                                        obj2.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        teamDao.Update(obj2);
                                    } 
                                }

                                ctx.CommitTransaction();
                                result = true;
                            }
                            else
                            {
                                ctx.RollBackTransaction();
                                errMessage = "Tidak ada laporan operasi yang ditemukan untuk pasien ini.";
                                result = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            errMessage = ex.Message;
                            result = false;
                        }
                        finally
                        {
                            ctx.Close();
                        }
                    }
                    break;
                default:
                    break;
            }
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

        protected void cbpViewDt6_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt6(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt6(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt7_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt7(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt7(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt8_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt8(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt8(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #region Detail List
        private void BindGridViewDt1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPreSurgeryAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPreSurgeryAssessment> lstEntity = BusinessLayer.GetvPreSurgeryAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PreSurgicalAssessmentID DESC");

            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind(); 
        }

        private void BindGridViewDt2(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvSurgicalSafetyCheckRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vSurgicalSafetyCheck> lstEntity = BusinessLayer.GetvSurgicalSafetyCheckList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");

            grdViewDt2.DataSource = lstEntity;
            grdViewDt2.DataBind();
        }

        private void BindGridViewDt3(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientMedicalDeviceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientMedicalDevice> lstEntity = BusinessLayer.GetvPatientMedicalDeviceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdViewDt3.DataSource = lstEntity;
            grdViewDt3.DataBind();
        }

        private void BindGridViewDt4(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientSurgeryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientSurgery> lstEntity = BusinessLayer.GetvPatientSurgeryList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PatientSurgeryID DESC");

            grdViewDt4.DataSource = lstEntity;
            grdViewDt4.DataBind();
        }

        private void BindGridViewDt5(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDocumentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDocument> lstEntity = BusinessLayer.GetvPatientDocumentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdViewDt5.DataSource = lstEntity;
            grdViewDt5.DataBind();
        }

        private void BindGridViewDt6(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPerioperativeNursingRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPerioperativeNursing> lstEntity = BusinessLayer.GetvPerioperativeNursingList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");

            grdViewDt6.DataSource = lstEntity;
            grdViewDt6.DataBind();
        }

        private void BindGridViewDt7(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPreAnesthesyAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPreAnesthesyAssessment> lstEntity = BusinessLayer.GetvPreAnesthesyAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PreAnesthesyAssessmentID DESC");

            grdViewDt7.DataSource = lstEntity;
            grdViewDt7.DataBind();
        }
        private void BindGridViewDt8(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPreProcedureChecklistRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPreProcedureChecklist> lstEntity = BusinessLayer.GetvPreProcedureChecklistList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ChecklistDate DESC, ChecklistTime DESC");
            grdViewDt8.DataSource = lstEntity;
            grdViewDt8.DataBind();
        }
        #endregion

        #region Callback Panel Event
        protected void cbpDeleteDocument_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteDocument(param);
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

        private string DeleteDocument(string recordID)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                PatientDocument obj = BusinessLayer.GetPatientDocument(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdatePatientDocument(obj);
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

        protected void cbpDeleteSurgicalCheckList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                SurgicalSafetyCheck obj = BusinessLayer.GetSurgicalSafetyCheck(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateSurgicalSafetyCheck(obj);
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
                SurgicalSafetyCheck obj = BusinessLayer.GetSurgicalSafetyCheck(id);
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
                            obj.SignOutNurseID = null;
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
                    BusinessLayer.UpdateSurgicalSafetyCheck(obj);
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

        protected void cbpDeleteDevice_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteDevice(param);
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

        private string DeleteDevice(string recordID)
        {
            string result = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);
            PatientMedicalDeviceDao deviceDao = new PatientMedicalDeviceDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                PatientMedicalDevice obj = deviceDao.Get(Convert.ToInt32(id));
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    deviceDao.Update(obj);

                    List<PatientMedicalDevice> lstDevice = BusinessLayer.GetPatientMedicalDeviceList(string.Format("MRN = {0} AND IsDeleted = 0", obj.MRN),ctx);

                    if (lstDevice.Count == 0)
                    {
                        Patient oPatient = patientDao.Get(obj.MRN);
                        if (oPatient.IsUsingImplant)
                        {
                            #region Update Patient Status - Using Implant
                            //oPatient.IsUsingImplant = false;
                            oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientDao.Update(oPatient);
                            #endregion
                        }

                    }
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

        protected void cbpDeletePerioperative_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeletePerioperative(param);
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

        private string DeletePerioperative(string recordID)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                PerioperativeNursing obj = BusinessLayer.GetPerioperativeNursing(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdatePerioperativeNursing(obj);
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

        protected void cbpDeletePerioperativeInfo_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string[] paramInfo = param.Split('|');
                string retVal = DeletePerioperativeInfo(paramInfo);
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

        private string DeletePerioperativeInfo(string[] paramInfo)
        {
            string result = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);
            PerioperativeNursingDao perioperativeDao = new PerioperativeNursingDao(ctx);
            VitalSignHdDao vitalSignHdDao = new VitalSignHdDao(ctx);

            try
            {
                //Confirm
                string infoPath = paramInfo[0]; // 1 = Sign In, 2 = Time Out, 3 = Sign Out
                int id = Convert.ToInt32(paramInfo[1]);
                PerioperativeNursing obj = perioperativeDao.Get(id);
                int vitalSignID = 0;

                if (obj != null)
                {
                    switch (infoPath)
                    {
                        case "1": // Pre Operative
                            vitalSignID = Convert.ToInt32(obj.PreOperativeVitalSignID);

                            obj.PreOperativeDate = null;
                            obj.PreOperativeTime = null;
                            obj.PreOperativeSurgeryNurseID = null;
                            obj.PreOperativeWardNurseID = null;
                            obj.PreOperativeUserID = null;
                            obj.PreOperativeLayout = null;
                            obj.PreOperativeValues = null;
                            obj.PreOperativeRemarks = null;
                            obj.PreOperativeLastUpdatedDate = null;
                            obj.PreOperativeVitalSignID = null;
                            break;
                        case "2": // Intra Operative
                            obj.IntraOperativeDate = null;
                            obj.IntraOperativeTime = null;
                            obj.IntraOperativeRecoveryRoomNurseID = null;
                            obj.IntraOperativeCircularNurseID = null;
                            obj.IntraOperativeUserID = null;
                            obj.IntraOperativeLayout = null;
                            obj.IntraOperativeValues = null;
                            obj.IntraOperativeRemarks = null;
                            obj.IntraOperativeLastUpdatedDate = null;
                            break;
                        case "3": // Post Operative
                            vitalSignID = Convert.ToInt32(obj.PostOperativeVitalSignID);

                            obj.PostOperativeDate = null;
                            obj.PostOperativeTime = null;
                            obj.PostOperativeRecoveryRoomNurseID = null;
                            obj.PostOperativeWardNurseID = null;
                            obj.PostOperativeUserID = null;
                            obj.PostOperativeLayout = null;
                            obj.PostOperativeValues = null;
                            obj.PostOperativeRemarks = null;
                            obj.PostOperativeLastUpdatedDate = null;
                            obj.PostOperativeVitalSignID = null;
                            break;
                        default:
                            break;
                    }

                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    perioperativeDao.Update(obj);

                    if (infoPath == "1" || infoPath == "3")
                    {
                        VitalSignHd obj1 = vitalSignHdDao.Get(vitalSignID);
                        if (obj1 != null)
                        {
                            obj1.IsDeleted = true;
                            vitalSignHdDao.Update(obj1);
                        }
                    }

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

        protected void cbpDeleteChecklist_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteChecklist(param);
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

        private string DeleteChecklist(string recordID)
        {
            string result = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);
            PreProcedureChecklistDao recordDao = new PreProcedureChecklistDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                PreProcedureChecklist obj = recordDao.Get(Convert.ToInt32(id));
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    recordDao.Update(obj);

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