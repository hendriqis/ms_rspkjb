using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AntenatalRecordList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        string deptType = string.Empty;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_ANTENATAL_RECORD;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_ANTENATAL_RECORD;
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_ANTENATAL_RECORD;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_ANTENATAL_RECORD;
                    default:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_ANTENATAL_RECORD;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.ANTENATAL_RECORD;
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.ANTENATAL_RECORD;
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.ANTENATAL_RECORD;
                    case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.ANTENATAL_RECORD;
                    case Constant.Module.RADIOTHERAPHY: return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_ANTENATAL_RECORD;
                    default: return Constant.MenuCode.EMR.ANTENATAL_RECORD;
                }
                #endregion
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
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
            filterExpression += string.Format("FetusID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnFetusID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFetalMeasurementRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFetalMeasurement> lstEntity = BusinessLayer.GetvFetalMeasurementList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            grdViewDt1_1.DataSource = lstEntity;
            grdViewDt1_1.DataBind();
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

        protected void cbpDeleteFetus_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;


            List<FetalMeasurement> lstMeasurement = BusinessLayer.GetFetalMeasurementList(string.Format("FetusID = {0} AND IsDeleted = 0", param));
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
                FetalMeasurement obj = BusinessLayer.GetFetalMeasurement(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateFetalMeasurement(obj);
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