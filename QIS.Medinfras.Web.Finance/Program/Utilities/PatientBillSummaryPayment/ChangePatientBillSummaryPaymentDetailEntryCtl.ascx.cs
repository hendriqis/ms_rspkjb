using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ChangePatientBillSummaryPaymentDetailEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnPaymentID.Value = param;

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(String.Format(
                                            "ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
                                            Constant.StandardCode.CARD_TYPE, //0
                                            Constant.StandardCode.CARD_PROVIDER //1
                                        ));

            Methods.SetComboBoxField<StandardCode>(cboCardType, lstSc.Where(p => p.ParentID == Constant.StandardCode.CARD_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCardProvider, lstSc.Where(p => p.ParentID == Constant.StandardCode.CARD_PROVIDER).ToList(), "StandardCodeName", "StandardCodeID");

            cboCardDateMonth.DataSource = Enumerable.Range(1, 12).Select(a => new
            {
                MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
                MonthNumber = a
            });

            cboCardDateMonth.TextField = "MonthName";
            cboCardDateMonth.ValueField = "MonthNumber";
            cboCardDateMonth.EnableCallbackMode = false;
            cboCardDateMonth.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboCardDateMonth.DropDownStyle = DropDownStyle.DropDownList;
            cboCardDateMonth.DataBind();

            cboCardDateYear.DataSource = Enumerable.Range(DateTime.Now.Year, 10);
            cboCardDateYear.EnableCallbackMode = false;
            cboCardDateYear.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboCardDateYear.DropDownStyle = DropDownStyle.DropDownList;
            cboCardDateYear.DataBind();

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("PaymentID = {0}", hdnPaymentID.Value);

            List<vPatientPaymentHdDtCardInfo> lst = BusinessLayer.GetvPatientPaymentHdDtCardInfoList(filterExpression);
            grdView.DataSource = lst;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnPaymentDetailID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PatientPaymentDt entity)
        {
            entity.GCCardType = cboCardType.Value.ToString();
            entity.GCCardProvider = cboCardProvider.Value.ToString();
            entity.CardNumber = string.Format("{0}-XXXX-XXXX-{1}", txtCardNumber1.Text, txtCardNumber4.Text);
            entity.CardValidThru = string.Format("{0:00}/{1:00}", cboCardDateMonth.Value.ToString(), cboCardDateYear.Value.ToString().Substring(2));
            entity.CardHolderName = txtHolderName.Text;
            entity.ReferenceNo = txtReferenceNo.Text;
            entity.BatchNo = txtBatchNo.Text;
            entity.TraceNo = txtTraceNo.Text;
            entity.ApprovalCode = txtApprovalCode.Text;
            entity.TerminalID = txtTerminalID.Text;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                PatientPaymentDt entity = BusinessLayer.GetPatientPaymentDt(Convert.ToInt32(hdnPaymentDetailID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientPaymentDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}