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
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PrescriptionOrderList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PRESCRIPTION_ORDER;
        }

        protected override void InitializeDataControl()
        {
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND GCTransactionStatus <> '{1}'", AppSession.RegisteredPatient.VisitID,Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPrescriptionOrderHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPrescriptionOrderHd> lstEntity = BusinessLayer.GetvPrescriptionOrderHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionOrderID DESC");
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

        protected override bool OnBeforeEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnID.Value));
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
                {
                    errMessage = "Prescription have already been proceed by Pharmacy";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }

            return result;
        }

        protected override bool OnBeforeDeleteRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnID.Value));
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
                {
                    errMessage = "Prescription have already been proceed by Pharmacy";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }

            return result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Program/PatientPage/Planning/PrescriptionOrder/PrescriptionOrderEntryCtl.ascx");
            queryString = "";
            popupWidth = 800;
            popupHeight = 400;
            popupHeaderText = "PRESCRIPTION ORDER - FREE TEXT MODE";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Program/PatientPage/Planning/PrescriptionOrder/PrescriptionOrderEntryCtl.ascx");
                queryString = hdnID.Value;
                popupWidth = 800;
                popupHeight = 400;
                popupHeaderText = "PRESCRIPTION ORDER - FREE TEXT MODE";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                try
                {
                    PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHdList(String.Format("PrescriptionOrderID = {0}", hdnID.Value))[0];
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePrescriptionOrderHd(entity);

                    if (AppSession.SA0137 == "1")
                    {
                        if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                        {
                            BridgingToMedinfrasV1(3, null, entity);
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    return false;
                }
            }
            return false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            if (type == "Propose")
            {
                try
                {
                    PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHdList(String.Format("PrescriptionOrderID = {0}", hdnID.Value))[0];
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                    entity.SendOrderDateTime = DateTime.Now;
                    entity.SendOrderBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePrescriptionOrderHd(entity);

                    if (AppSession.SA0137 == "1")
                    {
                        if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                        {
                            BridgingToMedinfrasV1(1, null, entity);
                        }
                    }
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                }
                return result;
            }
            return false;
        }

        private void BridgingToMedinfrasV1(int ProcessType, TestOrderHd entity, PrescriptionOrderHd entityPrescription)
        {
            APIMessageLog apiLog = new APIMessageLog();
            apiLog.MessageDateTime = DateTime.Now;
            apiLog.Sender = Constant.BridgingVendor.HIS;
            apiLog.Recipient = Constant.BridgingVendor.MEDINFRAS_API;

            MedinfrasV1Service oService = new MedinfrasV1Service();
            string serviceResult = oService.OnSendOrderMedicalDiagnosticServices(ProcessType, entity, entityPrescription, null);
            string[] serviceResultInfo = serviceResult.Split('|');
            if (serviceResultInfo[0] == "1")
            {
                apiLog.IsSuccess = true;
                apiLog.MessageText = serviceResultInfo[1];
                apiLog.Response = serviceResultInfo[2];
            }
            else
            {
                apiLog.IsSuccess = false;
                apiLog.MessageText = serviceResultInfo[1];
                apiLog.Response = serviceResultInfo[2];
                apiLog.ErrorMessage = serviceResultInfo[2];
            }
            BusinessLayer.InsertAPIMessageLog(apiLog);
        }
    }
}