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
    public partial class PartografList1 : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected List<vVitalSignDt> lstVitalSignDt = null;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_PARTOGRAF;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PARTOGRAF;
                    default:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PARTOGRAF;
                }
                #endregion
            }
            else if (menuType == "dp")
            {
                #region Data Pemeriksaan Pasien
                switch (deptType)
                {
                    case Constant.Module.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.DATA_PATIENT_PARTOGRAF;
                    default:
                        return Constant.MenuCode.EmergencyCare.DATA_PATIENT_PARTOGRAF;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PARTOGRAF;
                    case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.PARTOGRAF;
                    default: return Constant.MenuCode.MedicalDiagnostic.PARTOGRAF;
                }
                #endregion
            }
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
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvAntenatalRecordRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vAntenatalRecord> lstEntity = BusinessLayer.GetvAntenatalRecordList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/MedicalForm/AntenatalRecordFormEntry.ascx");
            queryString = "antenatalForm01";
            popupWidth = 800;
            popupHeight = 500;
            popupHeaderText = "Antenatal Record";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/MedicalForm/AntenatalRecordFormEntry.ascx");
                queryString = "antenatalForm01" + "|" + hdnID.Value;
                popupWidth = 800;
                popupHeight = 500;
                popupHeaderText = "Antenatal Record";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;

            if (hdnID.Value != "")
            {
                if (IsValidToDelete(hdnID.Value, ref errMessage))
                {
                    Antenatal entity = BusinessLayer.GetAntenatal(Convert.ToInt32(hdnID.Value));
                    entity.IsDeleted = true;
                    entity.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateAntenatal(entity);
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

            List<vFetalMeasurement> lstMeasurement = BusinessLayer.GetvFetalMeasurementList(string.Format("AntenatalRecordID = {0} AND IsDeleted = 0", ID));
            if (lstMeasurement.Count > 0)
            {
                message.AppendLine("Data antenatal tidak bisa dihapus karena masih ada informasi janin yang terdapat dalam data kehamilan ini");
            }
            else
            {
                Antenatal oAntenatal = BusinessLayer.GetAntenatal(Convert.ToInt32(ID));
                if (oAntenatal != null)
                {
                    if (oAntenatal.ObstetricHistoryID != 0)
                    {
                        message.AppendLine("Data antenatal tidak bisa dihapus karena sudah ada link dengan Riwayat Kehamilan Pasien");
                    }
                }
            }



            errMessage = message.ToString();

            return string.IsNullOrEmpty(errMessage);
        }

        #region Detail List
        private void BindGridViewDt1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("AntenatalRecordID = {0} AND IsDeleted = 0", hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFetusDataRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFetusData> lstEntity = BusinessLayer.GetvFetusDataList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "FetusNo");

            grdViewDt1.DataSource = lstEntity;
            grdViewDt1.DataBind();
        }

        private void BindGridViewDt1_1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("FetusID = {0} AND IsDeleted = 0", hdnFetusID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPartografRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPartograf> lstEntity = BusinessLayer.GetvPartografList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            grdViewDt1_1.DataSource = lstEntity;
            grdViewDt1_1.DataBind();
        }

        private void BindGridViewDt2(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND AntenatalRecordID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvLaborStageRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vLaborStage> lstEntity = BusinessLayer.GetvLaborStageList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");

            grdViewDt2.DataSource = lstEntity;
            grdViewDt2.DataBind();
        }

        private void BindGridViewDt3(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND AntenatalRecordID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID));
            grdViewDt3.DataSource = lstEntity;
            grdViewDt3.DataBind();
        }

        private void BindGridViewDt4(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND AntenatalRecordID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvIntraMedicationLogRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vIntraMedicationLog> lstEntity = BusinessLayer.GetvIntraMedicationLogList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            grdViewDt4.DataSource = lstEntity;
            grdViewDt4.DataBind();
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

        protected void cbpDeleteFetus_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;


            List<FetalMeasurement> lstMeasurement = BusinessLayer.GetFetalMeasurementList(string.Format("FetusID = {0}", param));
            if (lstMeasurement.Count > 0)
            {
                errMessage = "Data tidak bisa dihapus karena sudah ada informasi pengukuran janin (Fetal Measurement)";
                result += string.Format("fail|{0}", errMessage);
            }
            else
            {
                try
                {
                    string retVal = DeleteFetus(param);
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
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = param;
        }

        private string DeleteFetus(string recordID)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                FetusData obj = BusinessLayer.GetFetusData(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateFetusData(obj);
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

        protected void cbpViewDt1_1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt1_1(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt1_1(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpDeleteMeasurement_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteMeasurement(param);
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

        private string DeleteMeasurement(string recordID)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                Partograf obj = BusinessLayer.GetPartograf(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdatePartograf(obj);
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

        protected void cbpDeleteVitalSign_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteVitalSign(param);
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

        private string DeleteVitalSign(string recordID)
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

        protected void cbpDeleteMedication_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteMedication(param);
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

        private string DeleteMedication(string recordID)
        {
            string result = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);
            IntraMedicationLogDao intraMedicationLogDao = new IntraMedicationLogDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                IntraMedicationLog obj = intraMedicationLogDao.Get(Convert.ToInt32(id));
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    intraMedicationLogDao.Update(obj);

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

        protected void cbpDeleteLaborStage_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteLaborStage(param);
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

        private string DeleteLaborStage(string recordID)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                LaborStage obj = BusinessLayer.GetLaborStage(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateLaborStage(obj);
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

        protected void cbpDeleteLaborStageInfo_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteLaborStageInfo(param);
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

        private string DeleteLaborStageInfo(string param)
        {
            string result = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);
            LaborStageDao laborStageDao = new LaborStageDao(ctx);

            try
            {
                //Confirm
                string[] paramInfo = param.Split('|');
                string infoPath = paramInfo[0]; // 1 = Sign In, 2 = Time Out, 3 = Sign Out
                int id = Convert.ToInt32(paramInfo[1]);

                LaborStage entity = laborStageDao.Get(Convert.ToInt32(id));
                if (entity != null)
                {
                    switch (infoPath)
                    {
                        case "1":
                            entity.Stage1Date = null;
                            entity.Stage1Time = null;
                            entity.Stage1ParamedicID = null;
                            entity.Stage1Layout = null;
                            entity.Stage1Values = null;
                            entity.Stage1UserID = null;
                            entity.Stage1LastUpdatedDate = null;
                            break;
                        case "2":
                            entity.Stage2Date = null;
                            entity.Stage2Time = null;
                            entity.Stage2ParamedicID = null;
                            entity.Stage2Layout = null;
                            entity.Stage2Values = null;
                            entity.Stage2UserID = null;
                            entity.Stage2LastUpdatedDate = null;
                            break;
                        case "3":
                            entity.Stage3Date = null;
                            entity.Stage3Time = null;
                            entity.Stage3ParamedicID = null;
                            entity.Stage3Layout = null;
                            entity.Stage3Values = null;
                            entity.Stage3UserID = null;
                            entity.Stage3LastUpdatedDate = null;
                            break;
                        case "4":
                            entity.Stage4Date = null;
                            entity.Stage4Time = null;
                            entity.Stage4ParamedicID = null;
                            entity.Stage4Layout = null;
                            entity.Stage4Values = null;
                            entity.Stage4UserID = null;
                            entity.Stage4LastUpdatedDate = null;
                            break;
                        default:
                            break;
                    }

                    laborStageDao.Update(entity);

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