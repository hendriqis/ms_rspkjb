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
    public partial class PatientVentilatorList : BasePagePatientPageList
    {
        string deptType = string.Empty;
        string menuType = string.Empty;
        string vaccinationType = string.Empty;
        protected int PageCount1 = 1;
        protected int PageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;

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

            Healthcare oHc = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
            if (oHc != null)
            {
                if (oHc.Initial == "RSSEBK" || oHc.Initial == "RSSEB" || oHc.Initial == "RSSEBS")
                {
                    hdnReportCode.Value = "MR000053";
                }
                else
                {
                    hdnReportCode.Value = "MR000038";
                }
            }
            else
            {
                hdnReportCode.Value = "MR000038";
            }

            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            BindGridViewType(1, true, ref PageCount1);
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

            string filterExpression = string.Format("VisitID IN ({0},{1}) AND GCDeviceType = '{2}' AND IsDeleted = 0", hdnVisitID.Value, cvLinkedID, hdnDeviceType.Value);
         
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientETTLogHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientETTLogHd> lstEntity = BusinessLayer.GetvPatientETTLogHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");

            grdView.DataSource = lstEntity;
            grdView.DataBind();

            hdnIsHasLogRecord.Value = lstEntity.Count > 0 ? "1" : "0";
        }

        private void BindGridViewDt2(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (!string.IsNullOrEmpty(hdnRecordID.Value) && hdnRecordID.Value != "0")
            {
                string filterExpression = string.Format("PatientETTLogID = {0} AND IsDeleted = 0", hdnRecordID.Value);
                
                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPatientNosokomialRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                List<vPatientNosokomial> lstEntity = BusinessLayer.GetvPatientNosokomialList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AssessmentDate DESC, AssessmentTime DESC");
                grdViewDt2.DataSource = lstEntity;
                grdViewDt2.DataBind();

                hdnHasNosokomialRecord.Value = lstEntity.Count > 0 ? "1" : "0";
            }
        }

        private void BindGridViewDt3(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (!string.IsNullOrEmpty(hdnRecordID.Value) && hdnRecordID.Value != "0")
            {
                string filterExpression = string.Format("PatientETTLogID = {0} AND IsDeleted = 0", hdnRecordID.Value);
                 
                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
                lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("PatientETTLogID = {0} AND IsDeleted = 0 ORDER BY DisplayOrder", hdnRecordID.Value));
                grdViewDt3.DataSource = lstEntity;
                grdViewDt3.DataBind();

                hdnHasIntraVitalSignRecord.Value = lstEntity.Count > 0 ? "1" : "0";
            }
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
            panel.JSProperties["cpHasRecord"] = hdnIsHasLogRecord.Value;
        }


        #region Header-1

        private void BindGridViewType(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.MONITORING_DEVICE_TYPE);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetStandardCodeRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "StandardCodeID DESC");
            grdDeviceTypeList.DataSource = lstEntity;
            grdDeviceTypeList.DataBind();
        }

        protected void cbpDeviceTypeList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewType(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewType(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        } 

        #endregion

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
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_VENTILATOR_MONITORING;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_VENTILATOR_MONITORING;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_VENTILATOR_MONITORING;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_VENTILATOR_MONITORING;
                    default: return Constant.MenuCode.Inpatient.FOLLOWUP_VENTILATOR_MONITORING;
                }
                #endregion
            }
            else if (menuType == "dp")
            {
                #region Data Pemeriksaan Pasien
                switch (deptType)
                {
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.DATA_PATIENT_VENTILATOR_MONITORING;
                    default: return Constant.MenuCode.EmergencyCare.DATA_PATIENT_VENTILATOR_MONITORING;
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
                        return Constant.MenuCode.Inpatient.VENTILATOR_MONITORING;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.MONITORING_VENTILATOR;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.VENTILATOR_MONITORING;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.VENTILATOR_MONITORING;
                    case Constant.Module.RADIOTHERAPHY:
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_PATIENT_MEDICAL_DEVICE;
                    default:
                        return Constant.MenuCode.Inpatient.VENTILATOR_MONITORING;
                }
                #endregion
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //vPatientETTLogHd entity = e.Row.DataItem as vPatientETTLogHd;
            }
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
            PatientETTLogHdDao entityDao = new PatientETTLogHdDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(paramInfo[0]);
                PatientETTLogHd obj = BusinessLayer.GetPatientETTLogHd(id);
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

        protected void cbpCancelStop_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string[] paramInfo = param.Split('|');
                string retVal = CancelStop(paramInfo);
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

        private string CancelStop(string[] paramInfo)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PatientETTLogHdDao entityDao = new PatientETTLogHdDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(paramInfo[0]);
                PatientETTLogHd obj = BusinessLayer.GetPatientETTLogHd(id);
                if (obj != null)
                {
                    obj.IsReleased = false;
                    obj.EndDate = null;
                    obj.EndTime = null;
                    obj.ParamedicID2 = null;
                    obj.ETTStopReason = null;
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
            panel.JSProperties["cpHasRecord"] = hdnHasIntraVitalSignRecord.Value;
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
            panel.JSProperties["cpHasRecord"] = hdnHasIntraVitalSignRecord.Value;
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

        protected void cbpDeleteNosokomial_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteNosokomial(param);
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

        private string DeleteNosokomial(string recordID)
        {
            string result = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);
            PatientNosokomialDao recordDao = new PatientNosokomialDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                PatientNosokomial obj = recordDao.Get(Convert.ToInt32(id));
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