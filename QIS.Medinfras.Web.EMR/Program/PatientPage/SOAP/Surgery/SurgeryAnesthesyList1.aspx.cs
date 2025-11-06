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
    public partial class SurgeryAnesthesyList1 : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        protected static bool _isInitialAssessmentExists = false;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PENGKAJIAN_ANESTESI;
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
            hdnOperatingRoomID.Value = AppSession.MD0006;
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnPatientDocumentUrl.Value = string.Format(@"{0}/{1}/", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISPatientDocumentsPath.Replace("#MRN", AppSession.RegisteredPatient.MedicalNo));

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            int cvLinkedID = 0;
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.LinkedRegistrationID)).FirstOrDefault();
                if (entityLinkedRegistration != null)
                {
                    cvLinkedID = entityLinkedRegistration.VisitID;
                }
            }

            filterExpression += string.Format("VisitID IN ({0},{4}) AND HealthcareServiceUnitID = {1} AND GCOrderStatus NOT IN ('{2}') AND GCTransactionStatus NOT IN ('{3}')", AppSession.RegisteredPatient.VisitID, hdnOperatingRoomID.Value, Constant.OrderStatus.CANCELLED, Constant.TransactionStatus.VOID, cvLinkedID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvSurgeryTestOrderHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vSurgeryTestOrderHd1> lstEntity = BusinessLayer.GetvSurgeryTestOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
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

            string id = string.Format("{0}|{1}|{2}|{3}", hdnID.Value, hdnOrderNo.Value, "0", "anesthesy");
            switch (hdnSelectedTab.Value)
            {
                case "preSurgeryAssessment":
                    url = ResolveUrl("~/Program/PatientPage/SOAP/Surgery/AssessmentPreOpEntry1.aspx?id=" + id);
                    break;
                case "preSurgeryAnesthesy":
                    url = ResolveUrl("~/Program/PatientPage/SOAP/Surgery/AssessmentPreAnesthesyEntry1.aspx?id=" + id);
                    break;
                case "anesthesyStatus":
                    url = ResolveUrl("~/Program/PatientPage/SOAP/Surgery/AnesthesyStatusEntry1.aspx?id=" + id);
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
            string id = string.Format("{0}|{1}|{2}|{3}", hdnID.Value, hdnOrderNo.Value, "", "anesthesy");
            switch (hdnSelectedTab.Value)
            {
                case "preSurgeryAssessment":
                    url = ResolveUrl("~/Program/PatientPage/SOAP/Surgery/AssessmentPreOpEntry1.aspx?id=" + id);
                    break;
                case "preSurgeryAnesthesy":
                    url = ResolveUrl("~/Program/PatientPage/SOAP/Surgery/AssessmentPreAnesthesyEntry1.aspx?id=" + id);
                    break;
                case "anesthesyStatus":
                    url = ResolveUrl("~/Program/PatientPage/SOAP/Surgery/AnesthesyStatusEntry1.aspx?id=" + id);
                    break;
                case "surgeryReport":
                    id = string.Format("{0}|{1}|{2}|{3}", hdnID.Value, hdnOrderNo.Value, hdnSurgeryReportID.Value, "anesthesy");
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
                result = string.Format("1|{0}", string.Empty);
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
                case "preSurgeryAnesthesy":
                    if (hdnID.Value != "" && hdnID.Value != "0")
                    {
                        IDbContext ctx = DbFactory.Configure(true);
                        PreAnesthesyAssessmentDao assesmentDao = new PreAnesthesyAssessmentDao(ctx);
                        VitalSignHdDao vitalSignHdDao = new VitalSignHdDao(ctx);

                        try
                        {
                            PreAnesthesyAssessment entity = assesmentDao.Get(Convert.ToInt32(hdnAssessmentID.Value));
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

                                ctx.CommitTransaction();
                                result = true;
                            }
                            else
                            {
                                ctx.RollBackTransaction();
                                errMessage = "Tidak ada kajian asesmen pra anestesi yang ditemukan untuk pasien ini.";
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
                case "anesthesyStatus":
                    if (hdnID.Value != "" && hdnID.Value != "0")
                    {
                        IDbContext ctx = DbFactory.Configure(true);
                        SurgeryAnesthesyStatusDao assesmentDao = new SurgeryAnesthesyStatusDao(ctx);
                        VitalSignHdDao vitalSignHdDao = new VitalSignHdDao(ctx);

                        try
                        {
                            SurgeryAnesthesyStatus entity = assesmentDao.Get(Convert.ToInt32(hdnAssessmentID.Value));
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

                                ctx.CommitTransaction();
                                result = true;
                            }
                            else
                            {
                                ctx.RollBackTransaction();
                                errMessage = "Tidak ada kajian status anestesi yang ditemukan untuk pasien ini.";
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

            hdnIsSurgeryPreAssessmentExists.Value = lstEntity.Count > 0 ? "1" : "0";
        }

        private void BindGridViewDt3(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPreAnesthesyAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPreAnesthesyAssessment> lstEntity = BusinessLayer.GetvPreAnesthesyAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PreAnesthesyAssessmentID DESC");

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
                int rowCount = BusinessLayer.GetvSurgeryAnesthesyStatusRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vSurgeryAnesthesyStatus> lstEntity = BusinessLayer.GetvSurgeryAnesthesyStatusList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AnesthesyStatusID DESC");

            grdViewDt6.DataSource = lstEntity;
            grdViewDt6.DataBind();
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
        #endregion
    }
}