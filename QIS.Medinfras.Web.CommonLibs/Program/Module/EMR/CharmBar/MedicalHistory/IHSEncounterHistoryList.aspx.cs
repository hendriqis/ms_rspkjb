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
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class IHSEncounterHistoryList : BasePage
    {

        protected int PageCount = 1;
        string menuType = string.Empty;
        protected static bool _isInitialAssessmentExists = false;
        protected List<EncounterInfo> lstEncounterInfo = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnOperatingRoomIDCBCtl.Value = AppSession.MD0006;
            hdnRegistrationIDCBCtl.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitIDCBCtl.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnMRNCBCtl.Value = AppSession.RegisteredPatient.MRN.ToString();
            hdnPatientDocumentUrl.Value = string.Format(@"{0}/{1}/", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISPatientDocumentsPath.Replace("#MRN", AppSession.RegisteredPatient.MedicalNo));

            BindGridView(1, false, ref PageCount);
        }

        protected class EncounterInfo
        {
            public string EncounterID { get; set; }
            public string EncounterVisitNo { get; set; }
            public string EncounterLocation { get; set; }
            public string EncounterRegisteredPhysician { get; set; }
            public string EncounterStartDate { get; set; }
            public string EncounterStartTime { get; set; }
            public List<EncounterDiagnosis> EncounterDiagnosisList { get; set; }
        }

        protected class EncounterDiagnosis
        {
            public string EncounterID { get; set; }
            public string Rank { get; set; }
            public string DiagnosisType { get; set; }
            public string DiagnosisCode { get; set; }
            public string DiagnosisName { get; set; }
        }


        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            IHSService oService = new IHSService();
            string apiResult = string.Empty;
            string[] apiResultInfo = apiResult.Split('|');

            QIS.Medinfras.Web.Common.IHSModel.GetEncounterListResponse respInfo = oService.GetIHSNumberEncounterList("P01199742658");
            if (respInfo != null)
            {
                if (respInfo.total > 0)
                {
                    List<IHSModel.EncounterHistory> lstHistory = respInfo.entry;
                    List<EncounterInfo> lstEncounter = new List<EncounterInfo>();

                    foreach (IHSModel.EncounterHistory encounter in lstHistory)
                    {
                        EncounterInfo visitHistory = new EncounterInfo();
                        visitHistory.EncounterID = encounter.resource.id;
                        if (encounter.resource.identifier != null)
                        {
                            visitHistory.EncounterVisitNo = encounter.resource.identifier[0].value;
                        }
                        visitHistory.EncounterLocation = encounter.resource.location[0].location.display;
                        if (encounter.resource.participant != null)
                        {
                            if (encounter.resource.participant[0].individual != null)
                            {
                                visitHistory.EncounterRegisteredPhysician = encounter.resource.participant[0].individual.display;
                            }
                        }
                        visitHistory.EncounterStartDate = encounter.resource.period.start.Substring(0, 10);
                        visitHistory.EncounterStartTime = encounter.resource.period.start.Substring(12, 5);

                        if (encounter.resource.diagnosis != null)
                        {
                            List<EncounterDiagnosis> lstDiagnosis = new List<EncounterDiagnosis>();
                            if (encounter.resource.diagnosis.Count > 0)
                            {
                                foreach (IHSModel.Diagnosis dx in encounter.resource.diagnosis)
                                {
                                    EncounterDiagnosis diagnosis = new EncounterDiagnosis();
                                    diagnosis.EncounterID = encounter.resource.id;
                                    diagnosis.Rank = dx.rank.ToString();
                                    diagnosis.DiagnosisCode = dx.use.coding[0].code;
                                    diagnosis.DiagnosisName = dx.use.coding[0].display;
                                    lstDiagnosis.Add(diagnosis);
                                }
                            }
                            visitHistory.EncounterDiagnosisList = lstDiagnosis;
                        }
                        lstEncounter.Add(visitHistory);
                    }

                    lstEncounterInfo = lstEncounter;
                    grdView.DataSource = lstEncounter;
                    grdView.DataBind();
                }
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
                    if (string.IsNullOrEmpty(obj.GCAssessmentStatus) || obj.GCAssessmentStatus == Constant.AssessmentStatus.OPEN)
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
            EncounterInfo encounterInfo = lstEncounterInfo.Where(lst => lst.EncounterID == hdnID.Value).FirstOrDefault();

            grdViewDt.DataSource = encounterInfo.EncounterDiagnosisList;
            grdViewDt.DataBind();
        }

        private void BindGridViewDt2(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnID.Value);

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
            filterExpression += string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnID.Value);

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
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnID.Value);

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
            filterExpression += string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnID.Value);

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
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnID.Value);

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
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnID.Value);

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
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvSurgeryAnesthesyStatusRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vSurgeryAnesthesyStatus> lstEntity = BusinessLayer.GetvSurgeryAnesthesyStatusList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AnesthesyStatusID DESC");

            grdViewDt8.DataSource = lstEntity;
            grdViewDt8.DataBind();
        }
        #endregion
    }
}