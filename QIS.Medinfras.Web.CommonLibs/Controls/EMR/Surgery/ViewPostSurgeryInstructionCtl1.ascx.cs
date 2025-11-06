using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class ViewPostSurgeryInstructionCtl1 : BaseViewPopupCtl
    {
        protected int gridMedicationPageCount = 1;
        protected int gridLaboratoryPageCount = 1;
        protected int gridImagingPageCount = 1;

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnVisitID.Value = paramInfo[0];
            hdnTestOrderID.Value = paramInfo[1];
            hdnRecordID.Value = paramInfo[2];

            #region Patient Information
            vConsultVisit4 registeredPatient = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
            lblPatientName.InnerHtml = registeredPatient.cfPatientNameSalutation;
            lblGender.InnerHtml = registeredPatient.Gender;
            lblDateOfBirth.InnerHtml = string.Format("{0} ({1})", registeredPatient.cfDateOfBirth, Helper.GetPatientAge(words, registeredPatient.DateOfBirth));

            lblRegistrationDateTime.InnerHtml = string.Format("{0} / {1}", registeredPatient.cfVisitDate, registeredPatient.VisitTime);
            lblRegistrationNo.InnerHtml = registeredPatient.RegistrationNo;
            lblPhysician.InnerHtml = registeredPatient.ParamedicName;

            lblMedicalNo.InnerHtml = registeredPatient.MedicalNo;

            lblPayerInformation.InnerHtml = registeredPatient.BusinessPartnerName;
            lblPatientLocation.InnerHtml = registeredPatient.cfPatientLocation;
            imgPatientImage.Src = registeredPatient.PatientImageUrl;
            #endregion

            vPostSurgeryInstruction obj1 = BusinessLayer.GetvPostSurgeryInstructionList(string.Format("VisitID = {0} AND PostSurgeryInstructionID = {1} AND IsDeleted = 0", hdnVisitID.Value, hdnRecordID.Value)).FirstOrDefault();
            if (obj1 != null)
            {
                if (obj1.PostSurgeryInstructionID != 0)
                {
                    txtInstructionDate.Text = obj1.InstructionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtInstructionTime.Text = obj1.InstructionTime;
                    divFormContent1.InnerHtml = obj1.MonitoringInstructionFormLayout;
                    hdnMonitoringLayout.Value = obj1.MonitoringInstructionFormLayout;
                    hdnMonitoringValue.Value = obj1.MonitoringInstructionFormValue;
                    divFormContent2.InnerHtml = obj1.NutritionInstructionFormLayout;
                    hdnNutritionLayout.Value = obj1.NutritionInstructionFormLayout;
                    hdnNutritionFormValue.Value = obj1.NutritionInstructionFormValue;
                    divFormContent3.InnerHtml = obj1.OtherInstructionFormLayout;
                    hdnOtherInstructionLayout.Value = obj1.OtherInstructionFormLayout;
                    hdnOtherInstructionValue.Value = obj1.OtherInstructionFormValue;
                    txtRemarks.Text = obj1.Remarks;

                    BindGridViewMedication(1, true, ref gridMedicationPageCount);
                    BindGridViewLaboratory(1, true, ref gridLaboratoryPageCount);
                    BindGridViewImaging(1, true, ref gridImagingPageCount);

                    lblPhysicianName2.InnerText = obj1.ParamedicName;
                }
            }
        }

        protected void cbpMedicationView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewMedication(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewMedication(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }


        #region Medication
        #region Prescription Hd
        private void BindGridViewMedication(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string TransactionStatus = String.Format("'{0}','{1}'", Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            string filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus NOT IN ({1}) AND PostSurgeryInstructionID = {2}", AppSession.RegisteredPatient.VisitID, TransactionStatus, hdnRecordID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPrescriptionOrderHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPrescriptionOrderHd> lstEntity = BusinessLayer.GetvPrescriptionOrderHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionOrderID DESC");
            grdMedicationView.DataSource = lstEntity;
            grdMedicationView.DataBind();
        }


        protected void cbpProposed_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (e.Parameter != null && e.Parameter != "")
            {
                int PrescriptionOrderID = Convert.ToInt32(e.Parameter);
            }
        }
        #endregion

        #region Prescription Dt
        private void BindGridViewMedicationDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnPrescriptionOrderID.Value != "")
            {
                filterExpression = string.Format("PrescriptionOrderID = {0} AND ParentID IS NULL", hdnPrescriptionOrderID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPrescriptionOrderDt1RowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            //string code = ddlViewType.SelectedValue;

            //if (code == "1")
            //    filterExpression += string.Format(" AND GCPrescriptionOrderStatus IN ('{0}','{1}','{2}') AND TakenQty > 0", Constant.OrderStatus.IN_PROGRESS, Constant.OrderStatus.COMPLETED, Constant.OrderStatus.CLOSED);
            //else if (code == "2")
            //    filterExpression += string.Format(" AND GCPrescriptionOrderStatus IN ('{0}','{1}','{2}') AND TakenQty = 0", Constant.OrderStatus.IN_PROGRESS, Constant.OrderStatus.COMPLETED, Constant.OrderStatus.CLOSED);
            //else if (code == "3")
            //    filterExpression += string.Format(" AND GCPrescriptionOrderStatus IN ('{0}')", Constant.OrderStatus.CANCELLED);

            List<vPrescriptionOrderDt10> lstEntity = BusinessLayer.GetvPrescriptionOrderDt10List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionOrderDetailID");
            grdMedicationViewDt.DataSource = lstEntity;
            grdMedicationViewDt.DataBind();
        }
        protected void cbpMedicationViewDtCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewMedicationDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewMedicationDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
        protected void grdMedicationViewDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPrescriptionOrderDt10 entity = e.Row.DataItem as vPrescriptionOrderDt10;

                HtmlImage imgHAM = e.Row.FindControl("imgHAM") as HtmlImage;
                if (imgHAM != null)
                {
                    imgHAM.Visible = entity.IsHAM;
                }
            }
        }

        #region Laboratory
        private void BindGridViewLaboratory(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TransactionCode  = {1} AND GCTransactionStatus != '{2}' AND PostSurgeryInstructionID = '{3}'", AppSession.RegisteredPatient.VisitID, Constant.TransactionCode.LABORATORY_TEST_ORDER, Constant.TransactionStatus.VOID, hdnRecordID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd1> lstEntity = BusinessLayer.GetvTestOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            StringBuilder sbText = new StringBuilder();
            foreach (vTestOrderHd1 item in lstEntity)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbText.AppendLine(string.Format("- {0}", detail.ItemName1));
                }
            }
            hdnLaboratorySummary.Value = sbText.ToString();

            grdLaboratoryView.DataSource = lstEntity;
            grdLaboratoryView.DataBind();
        }

        protected void cbpLaboratoryView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewLaboratory(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewLaboratory(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = hdnLaboratorySummary.Value;
        }

        protected void grdLaboratoryView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd1 obj = (vTestOrderHd1)e.Row.DataItem;
                Repeater rptLaboratoryDt = (Repeater)e.Row.FindControl("rptLaboratoryDt");
                rptLaboratoryDt.DataSource = obj.cfTestOrderDetailList;
                rptLaboratoryDt.DataBind();
            }
        }

        private object GetTestOrderDt(int testOrderID)
        {
            List<vTestOrderDt> lstOrderDt = BusinessLayer.GetvTestOrderDtList(string.Format("TestOrderID = {0} ORDER BY ItemName1", testOrderID));
            return lstOrderDt;
        }
        #endregion

        #region Imaging
        private void BindGridViewImaging(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TransactionCode = {1} AND GCTransactionStatus != '{2}' AND PostSurgeryInstructionID = {3}", AppSession.RegisteredPatient.VisitID, Constant.TransactionCode.IMAGING_TEST_ORDER, Constant.TransactionStatus.VOID, hdnRecordID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd1> lstEntity = BusinessLayer.GetvTestOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            StringBuilder sbText = new StringBuilder();
            foreach (vTestOrderHd1 item in lstEntity)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbText.AppendLine(string.Format("- {0}", detail.ItemName1));
                }
            }
            hdnImagingSummary.Value = sbText.ToString();

            grdImagingView.DataSource = lstEntity;
            grdImagingView.DataBind();
        }

        protected void cbpImagingView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewImaging(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewImaging(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = hdnImagingSummary.Value;
        }

        protected void grdImagingView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd1 obj = (vTestOrderHd1)e.Row.DataItem;
                Repeater rptImagingDt = (Repeater)e.Row.FindControl("rptImagingDt");
                rptImagingDt.DataSource = obj.cfTestOrderDetailList;
                rptImagingDt.DataBind();
            }
        }
        #endregion
        #endregion
    }
}