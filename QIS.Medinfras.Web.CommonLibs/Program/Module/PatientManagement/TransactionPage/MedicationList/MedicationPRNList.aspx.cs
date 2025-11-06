using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MedicationPRNList : BasePageTrx
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        string deptType = string.Empty;
        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_MEDICATION_LIST_PRN;
                    default:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_MEDICATION_LIST_PRN;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.MEDICATION_LIST_PRN;
                    case Constant.Facility.PHARMACY:
                        return Constant.MenuCode.Pharmacy.UDD_MEDICATION_LIST_PRN;
                    default:
                        return Constant.MenuCode.Inpatient.MEDICATION_LIST_PRN;
                }
                #endregion
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

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            BindGridView(1, true, ref PageCount);
            BindGridViewDt(1, true, ref PageCount);
        }

        #region Header
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";//hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            String TransactionStatus = String.Format("'{0}','{1}'", Constant.TestOrderStatus.OPEN, Constant.TestOrderStatus.CANCELLED);
            filterExpression += string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID);

            List<PRNMedicationItem> lstEntity = BusinessLayer.GetPRNMedicationItemList(AppSession.RegisteredPatient.VisitID.ToString(), "1");
            lstEntity = lstEntity.OrderBy(lst => lst.DrugName).ToList();
            grdView.DataSource = lstEntity;
            grdView.DataBind();
            pageCount = lstEntity.Count() > 0 ? pageCount : 0;
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


        protected void cbpProposed_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (e.Parameter != null && e.Parameter != "")
            {
                int PrescriptionOrderID = Convert.ToInt32(e.Parameter);
            }
        }
        #endregion

        #region Detail
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";

            if (hdnPrescriptionOrderDtID.Value != "" && hdnPrescriptionOrderDtID.Value != "0")
            {
                filterExpression = string.Format("PrescriptionOrderDetailID = {0}", hdnPrescriptionOrderDtID.Value);
            }
            else if (hdnPastMedicationID.Value != "" && hdnPastMedicationID.Value != "0")
            {
                filterExpression = string.Format("PastMedicationID = {0}", hdnPastMedicationID.Value);
            }

            filterExpression += string.Format(" AND VisitID = {0}", AppSession.RegisteredPatient.VisitID);
            filterExpression += string.Format(" AND GCMedicationStatus = '{0}' AND IsDeleted = 0", Constant.MedicationStatus.TELAH_DIBERIKAN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvMedicationScheduleRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            List<vMedicationSchedule> lstTransaction = BusinessLayer.GetvMedicationScheduleList(filterExpression, 10, pageIndex, "MedicationDate");
            grdViewDt.DataSource = lstTransaction;
            grdViewDt.DataBind();
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
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            if (type == "Propose")
            {
                try
                {
                    PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHdList(String.Format("PrescriptionOrderID = {0}", hdnPrescriptionOrderID.Value))[0];
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    BusinessLayer.UpdatePrescriptionOrderHd(entity);

                    //try
                    //{
                    //    HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(entity.DispensaryServiceUnitID));
                    //    string ipAddress = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                    //    if (!String.IsNullOrEmpty(ipAddress))
                    //    {
                    //        SendNotification(entity,ipAddress,"6000");
                    //    }
                    //}
                    //catch (Exception)
                    //{
                    //}
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

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PRNMedicationItem entity = e.Row.DataItem as PRNMedicationItem;

                if (!entity.IsInternalMedication)
                {
                    HtmlTableRow tr = (HtmlTableRow)e.Row.FindControl("trItemName");
                    tr.Attributes.Add("class", "externalMedicationColor");
                }
            }
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int recordID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "delete")
            {
                recordID = Convert.ToInt32(hdnID.Value);

                string filterExp = string.Format("ID = {0}", recordID);
                MedicationSchedule oSchedule = BusinessLayer.GetMedicationSchedule(recordID);

                try
                {
                    if (oSchedule != null)
                    {
                        string[] paramDelete = param[1].Split(';');
                        int ID = Convert.ToInt32(paramDelete[0]);
                        string gcDeleteReason = paramDelete[1];
                        string reason = paramDelete[2];

                        oSchedule.IsDeleted = true;
                        oSchedule.GCDeleteReason = gcDeleteReason;
                        oSchedule.DeleteReason = reason;
                        oSchedule.DeleteBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdateMedicationSchedule(oSchedule);

                        result += "delete|success";
                    }
                    else
                    {
                        result += string.Format("delete|fail|{0}", errMessage);
                    }
                }
                catch (Exception ex)
                {
                    result += string.Format("delete|fail|{0}", ex.Message);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRecordID"] = recordID.ToString();
        }
    }
}