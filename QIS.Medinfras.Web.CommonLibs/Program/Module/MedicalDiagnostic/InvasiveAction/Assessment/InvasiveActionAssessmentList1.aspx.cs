using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Data.Service.DataLayer.Base;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InvasiveActionAssessmentList1 : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        protected static bool _isInitialAssessmentExists = false;
        protected List<vVitalSignDt> lstMonitoringVitalSignDt = null;
        public string chartData = "";

        public override string OnGetMenuCode()
        {
            string[] paramSplit = hdnModuleID.Value.Split('|');
            if (paramSplit[0] == Constant.Facility.OUTPATIENT)
            {
                if (paramSplit.Length > 1)
                {
                    if (paramSplit[1] == "TR")
                    {
                        return Constant.MenuCode.Outpatient.OP032410;
                    }
                    else if (paramSplit[1] == "FO")
                    {
                        return Constant.MenuCode.Outpatient.OP020491;
                    }
                    else if (paramSplit[1] == "dp")
                    {
                        return Constant.MenuCode.Outpatient.OP030481;
                    }
                    else
                    {
                        return Constant.MenuCode.Outpatient.OP032410;
                    }
                }
                else
                {
                    return Constant.MenuCode.Outpatient.OP032410;
                }
            }
            else if (paramSplit[0] == Constant.Facility.EMERGENCY)
            {
                if (paramSplit.Length > 1)
                {
                    if (paramSplit[1] == "FO")
                    {
                        return Constant.MenuCode.EmergencyCare.ER010491;
                    }
                    else if (paramSplit[1] == "dp")
                    {
                        return Constant.MenuCode.EmergencyCare.ER020121;
                    }
                    else
                    {
                        return Constant.MenuCode.EmergencyCare.ER022410;
                    }
                }
                else
                {
                    return Constant.MenuCode.EmergencyCare.ER022410;
                }
            }
            else if (paramSplit[0] == Constant.Facility.DIAGNOSTIC)
            {
                if (paramSplit.Length > 1)
                {
                    if (paramSplit[1] == "FO")
                    {
                        return Constant.MenuCode.MedicalDiagnostic.MD020521;
                    }
                    else
                    {
                        return Constant.MenuCode.MedicalDiagnostic.MD035141;
                    }
                }
                else
                {
                    return Constant.MenuCode.MedicalDiagnostic.MD035141;
                }
            }
            else
            {
                return Constant.MenuCode.MedicalDiagnostic.MD035141;
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
            IsAllowAdd = false;
            IsAllowEdit = false;
            IsAllowDelete = false;
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
            hdnInputSurgeryAssessmentFirst.Value = AppSession.EM0046;

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

            List<PatientProcedureCharges> lstEntity = BusinessLayer.GetPatientProcedureChargesList(AppSession.RegisteredPatient.VisitID, cvLinkedID);
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

            string id = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", hdnLinkedID.Value, hdnTransactionNo.Value, "0", hdnItemName.Value, hdnChargesParamedicID.Value, hdnChargesParamedicName.Value);
            switch (hdnSelectedTab.Value)
            {
                case "preSurgeryAssessment":
                    url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/InvasiveAction/ProcedureReporting/AssessmentPreProcedureEntry1.aspx?id=" + id);
                    break;
                case "preSurgeryAnesthesy":
                    url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/InvasiveAction/ProcedureReporting/ProcedurePreAnesthesyEntry1.aspx?id=" + id);
                    break;
                case "anesthesyStatus":
                    url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/InvasiveAction/ProcedureReporting/ProcedureAnesthesyStatusEntry1.aspx?id=" + id);
                    break;
                case "surgeryReport":
                    url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/InvasiveAction/ProcedureReporting/ProcedureSurgeryReportEntry1.aspx?id=" + id);
                    break;
                case "postSurgeryInstruction":
                    url = ResolveUrl("~/Program/PatientPage/SOAP/Surgery/PostSurgeryInstructionEntry1.aspx?id=" + id);
                    break;
                default:
                    url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/InvasiveAction/ProcedureReporting/AssessmentPreProcedureEntry1.aspx?id=" + id);
                    break;
            }

            result = true;

            return result;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            bool result = true;
            string id = string.Format("{0}|{1}|{2}|{3}", hdnLinkedID.Value, hdnTransactionNo.Value, hdnAssessmentID.Value, hdnItemName.Value);
            switch (hdnSelectedTab.Value)
            {
                case "preSurgeryAssessment":
                    url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/InvasiveAction/ProcedureReporting/AssessmentPreProcedureEntry1.aspx?id=" + id);
                    break;
                case "preSurgeryAnesthesy":
                    url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/InvasiveAction/ProcedureReporting/ProcedurePreAnesthesyEntry1.aspx?id=" + id);
                    break;
                case "anesthesyStatus":
                    url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/InvasiveAction/ProcedureReporting/ProcedureAnesthesyStatusEntry1.aspx?id=" + id);
                    break;
                case "surgeryReport":
                    id = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", hdnLinkedID.Value, hdnTransactionNo.Value, hdnSurgeryReportID.Value, hdnItemName.Value, hdnChargesParamedicID.Value, hdnChargesParamedicName.Value);
                    url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/InvasiveAction/ProcedureReporting/ProcedureSurgeryReportEntry1.aspx?id=" + id);
                    break;
                case "postSurgeryInstruction":
                    id = string.Format("{0}|{1}|{2}|{3}", hdnLinkedID.Value, hdnTransactionNo.Value, hdnPostSurgeryInstructionID.Value, "postSurgeryInstruction");
                    url = ResolveUrl("~/Program/PatientPage/SOAP/Surgery/PostSurgeryInstructionEntry1.aspx?id=" + id);
                    break;
                default:
                    url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/InvasiveAction/ProcedureReporting/AssessmentPreProcedureEntry1.aspx?id=" + id);
                    break;
            }

            return result;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected List<vVitalSignDt> GetVitalSignDt(Int32 ID)
        {
            return lstMonitoringVitalSignDt.Where(p => p.ID == ID).ToList();
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
                    if (hdnLinkedID.Value != "" && hdnLinkedID.Value != "0")
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

                                string filterExpressionAssessment = string.Format("PreSurgeryAssessmentID = {0} AND IsDeleted = 0", hdnAssessmentID.Value);
                                VitalSignHd obj1 = BusinessLayer.GetVitalSignHdList(filterExpressionAssessment, ctx).FirstOrDefault();
                                if (obj1 != null)
                                {
                                    obj1.IsDeleted = true;
                                    obj1.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    vitalSignHdDao.Update(obj1);
                                }

                                ReviewOfSystemHd obj2 = BusinessLayer.GetReviewOfSystemHdList(filterExpressionAssessment, ctx).FirstOrDefault();
                                if (obj2 != null)
                                {
                                    obj2.IsDeleted = true;
                                    obj2.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    rosHDDao.Update(obj2);
                                }

                                string filterExpression = string.Format("PatientChargesDtID = {0} AND IsDeleted = 0", hdnLinkedID.Value);
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
                    if (hdnLinkedID.Value != "" && hdnLinkedID.Value != "0")
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

                                string filterExpression = string.Format("PreAnesthesyAssessmentID = {0} AND IsDeleted = 0", hdnAssessmentID.Value);
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
                    if (hdnLinkedID.Value != "" && hdnLinkedID.Value != "0")
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
                        PatientProcedureDao procedureGroupDao = new PatientProcedureDao(ctx);
                        //PatientSurgeryTeamDao teamDao = new PatientSurgeryTeamDao(ctx);

                        try
                        {
                            PatientSurgery entity = assesmentDao.Get(Convert.ToInt32(hdnSurgeryReportID.Value));
                            if (entity != null)
                            {
                                entity.IsDeleted = true;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                assesmentDao.Update(entity);

                                string filterExpression = string.Format("PatientChargesDtID = {0} AND IsDeleted = 0", hdnLinkedID.Value);
                                List<PatientProcedure> lstGroup = BusinessLayer.GetPatientProcedureList(filterExpression, ctx);
                                foreach (PatientProcedure obj1 in lstGroup)
                                {
                                    if (obj1 != null)
                                    {
                                        obj1.IsDeleted = true;
                                        obj1.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        procedureGroupDao.Update(obj1);
                                    }
                                }

                                //List<PatientSurgeryTeam> lstTeam = BusinessLayer.GetPatientSurgeryTeamList(filterExpression, ctx);
                                //foreach (PatientSurgeryTeam obj2 in lstTeam)
                                //{
                                //    if (obj2 != null)
                                //    {
                                //        obj2.IsDeleted = true;
                                //        obj2.LastUpdatedBy = AppSession.UserLogin.UserID;
                                //        teamDao.Update(obj2);
                                //    }
                                //}

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
                case "postSurgeryInstruction":
                    if (hdnLinkedID.Value != "" && hdnLinkedID.Value != "0")
                    {
                        IDbContext ctx = DbFactory.Configure(true);
                        PostSurgeryInstructionDao assesmentDao = new PostSurgeryInstructionDao(ctx);

                        try
                        {
                            PostSurgeryInstruction entity = assesmentDao.Get(Convert.ToInt32(hdnPostSurgeryInstructionID.Value));
                            if (entity != null)
                            {
                                entity.IsDeleted = true;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                assesmentDao.Update(entity);

                                ctx.CommitTransaction();
                                result = true;
                            }
                            else
                            {
                                ctx.RollBackTransaction();
                                errMessage = "Tidak ada instruksi paska bedah terintegrasi yang ditemukan untuk pasien ini.";
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
            panel.JSProperties["cpIsSurgeryPreAssessmentExists"] = hdnIsSurgeryPreAssessmentExists.Value;
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

        protected void cbpViewDt10_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt10(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt10(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Detail List
        private void BindGridViewDt1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND PatientChargesDtID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnLinkedID.Value);

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

        private void BindGridViewDt2(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND PatientChargesDtID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnLinkedID.Value);

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
            string filterExpression = string.Format("VisitID = {0} AND PatientChargesDtID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnLinkedID.Value);

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
            string filterExpression = string.Format("VisitID = {0} AND TransactionDtID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnLinkedID.Value);

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
            filterExpression += string.Format("VisitID = {0} AND PatientChargesDtID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnLinkedID.Value);

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
            string filterExpression = string.Format("VisitID = {0} AND PatientChargesDtID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnLinkedID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvSurgeryAnesthesyStatusRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vSurgeryAnesthesyStatus> lstEntity = BusinessLayer.GetvSurgeryAnesthesyStatusList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AnesthesyStatusID DESC");

            grdViewDt6.DataSource = lstEntity;
            grdViewDt6.DataBind();
        }

        private void BindGridViewDt8(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND PatientChargesDtID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnLinkedID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPreProcedureChecklistRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPreProcedureChecklist> lstEntity = BusinessLayer.GetvPreProcedureChecklistList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ChecklistDate DESC, ChecklistTime DESC");
            grdViewDt8.DataSource = lstEntity;
            grdViewDt8.DataBind();
        }

        private void BindGridViewDt10(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnLinkedID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPostSurgeryInstructionRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPostSurgeryInstruction> lstEntity = BusinessLayer.GetvPostSurgeryInstructionList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "InstructionDate DESC, InstructionTime DESC");

            grdViewDt10.DataSource = lstEntity;
            grdViewDt10.DataBind();
        }
        #endregion

        #region Callback Panel Event
        protected void cbpDeleteIntraAnesthesy_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            string type = "01";
            string recordID = "0";
            try
            {
                string[] paramInfo = param.Split('|');
                type = paramInfo[0];
                recordID = paramInfo[1];
                string retVal = "1|";

                if (type == "01")
                {
                    retVal = DeleteIntraVitals(recordID);
                }
                else if (type == "02")
                {
                    retVal = DeleteIntraMedication(recordID);
                }
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
            panel.JSProperties["cpType"] = type;
            panel.JSProperties["cpRecordID"] = recordID;
        }

        private string DeleteIntraMedication(string recordID)
        {
            string result = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);
            AnesthesyMedicationLogDao medicationDao = new AnesthesyMedicationLogDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                AnesthesyMedicationLog obj = medicationDao.Get(Convert.ToInt32(id));
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    medicationDao.Update(obj);

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

        private string DeleteIntraVitals(string recordID)
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

        private string DeleteIntra(string recordID)
        {
            string result = string.Empty;


            return result;
        }

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

        protected void cbpDeleteSurgicalCheckList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteSurgicalCheckList(param);
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

        private string DeleteSurgicalCheckList(string recordID)
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
        #endregion

        #region Chart Region
        public string GetChartDataSample()
        {
            ChartBase.ChartInfo chartInfo = new ChartBase.ChartInfo { Title = "Grafik Status Fisiologis", XLabel = "Period", YLabel = "Point" };

            //Title, yMin, yMax
            //string thumbChartData = "Temperature^36.5^40;Systolic^95^140;Diastolic^60^120;";
            //thumbChartData += "HDL^0^200;LDL^0^200;Triglycerids^0^400;Cholestrol^0^400";
            string thumbChartData = "";

            List<ChartBase.ChartPoint> list = GetList(ref thumbChartData);
            StringBuilder sbSeries = new StringBuilder();
            StringBuilder sbLegend = new StringBuilder();
            IEnumerable<string> lstSeriesName = (from p in list
                                                 select p.SeriesName).Distinct();

            if (lstSeriesName.Count() > 0)
            {
                sbSeries.Append("[");
                int ctr = 1;
                foreach (string seriesName in lstSeriesName)
                {
                    sbLegend.Append(seriesName);
                    sbSeries.Append(GetSeriesSample(list.Where(p => p.SeriesName == seriesName).ToList()));
                    if (ctr < lstSeriesName.Count())
                    {
                        sbLegend.Append(";");
                        sbSeries.Append(",");
                    }
                    else
                        break;
                    ctr++;
                }
                sbSeries.Append("]");
            }
            return thumbChartData + "|" + chartInfo.Title + "|" + chartInfo.XLabel + "|" + chartInfo.YLabel + "|" + sbLegend.ToString() + "|" + sbSeries.ToString();
        }

        private static string GetSeriesSample(List<ChartBase.ChartPoint> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            int ctr = 1;
            foreach (ChartBase.ChartPoint obj in list)
            {
                sb.Append("['").Append(obj.XPoint).Append("',").Append(obj.YPoint).Append(",").Append(obj.YPoint).Append("]");
                if (ctr < list.Count)
                    sb.Append(",");
                ctr++;
            }
            sb.Append("]");
            return sb.ToString();
        }

        private List<ChartBase.ChartPoint> GetList(ref string thumbChartData)
        {
            List<ChartBase.ChartPoint> list = new List<ChartBase.ChartPoint>();
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("TestOrderID = {0} AND VitalSignValue != '' AND IsDeleted = 0", hdnLinkedID.Value));

            list = (from p in lstVitalSignDt
                    select new ChartBase.ChartPoint { SeriesName = p.VitalSignLabel, XPoint = p.ObservationDate.ToString("yyyy-MM-dd"), YPoint = Convert.ToDouble(p.VitalSignValue) }).ToList();

            IEnumerable<int> lstSeriesID = (from p in lstVitalSignDt
                                            select p.VitalSignID).Distinct();

            StringBuilder sbFilterExpression = new StringBuilder();
            foreach (int seriesID in lstSeriesID)
            {
                if (sbFilterExpression.ToString() != "")
                    sbFilterExpression.Append(",");
                sbFilterExpression.Append(seriesID);
            }
            string filterExpression = string.Format("VitalSignID IN ({0})", sbFilterExpression.ToString());
            List<VitalSignType> lstVitalSignType = BusinessLayer.GetVitalSignTypeList(filterExpression);
            foreach (VitalSignType vitalSignType in lstVitalSignType)
            {
                if (thumbChartData != "")
                    thumbChartData += ";";
                thumbChartData += string.Format("{0}^{1}^{2}", vitalSignType.VitalSignLabel, vitalSignType.MinNormalValue, vitalSignType.MaxNormalValue);
            }

            return list;
        }
        #endregion
    }
}