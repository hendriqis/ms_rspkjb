using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Pharmacy.Program.Prescription.PrescriptionEntry
{
    public partial class UpdateItterStatusList : BaseViewPopupCtl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnPrescriptionOrderID.Value = paramInfo[0];
            string transactionNo = paramInfo[1];
            txtPresciptionNo.Text = transactionNo;
            txtPrescriptionDate.Text = paramInfo[2];
            txtPrescriptionTime.Text = paramInfo[3];
            hdnPrescriptionTypeCtl.Value = paramInfo[4];

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.USE_LAST_PURCHASE_EXPIRED_DATE));
            hdnIsUsedlastPurchaseExpiredDate.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.USE_LAST_PURCHASE_EXPIRED_DATE).FirstOrDefault().ParameterValue;

            if (lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN).FirstOrDefault().ParameterValue == Constant.PrinterType.DOT_MATRIX_FORMAT_1)
            {
                trPrescriptionType.Style.Remove("display");
            }
            else
            {
                trPrescriptionType.Style.Add("display", "none");
            }

            String filterExpression = string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PRESCRIPTION_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(t => t.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboPrescriptionType.SelectedIndex = 0;

            BindGridView();
        }

        private void BindGridView()
        {
            String filterExpression = String.Format("PrescriptionOrderID = {0} AND ParentID IS NULL AND OrderIsDeleted = 0 AND GCPrescriptionOrderStatus NOT IN ('{1}','{2}') AND TakenQty > 0", hdnPrescriptionOrderID.Value, Constant.OrderStatus.OPEN, Constant.OrderStatus.CANCELLED);
            List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwViewPrint_RowDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPrescriptionOrderDt1 entity = e.Item.DataItem as vPrescriptionOrderDt1;
                TextBox txtExpiredDate = e.Item.FindControl("txtExpiredDate") as TextBox;
                String lastPurchaseExpiredDate = entity.LastPurchaseExpiredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                HtmlInputText textItter = (HtmlInputText)e.Item.FindControl("txtItterNo");
                HtmlInputHidden hdnRefillInstruction = (HtmlInputHidden)e.Item.FindControl("hdnRefillInstruction");

                if (hdnIsUsedlastPurchaseExpiredDate.Value == "1")
                {
                    if (lastPurchaseExpiredDate != "01-01-1900")
                    {
                        txtExpiredDate.Text = Convert.ToDateTime(entity.LastPurchaseExpiredDate).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    }
                    else
                    {
                        txtExpiredDate.Text = "";
                    }
                }
                else
                {
                    txtExpiredDate.Text = "";
                }

                if (!String.IsNullOrEmpty(entity.GCRefillInstruction))
                {
                    hdnRefillInstruction.Value = entity.GCRefillInstruction;
                    if (entity.GCRefillInstruction == Constant.RefillInstruction.DET || entity.GCRefillInstruction == Constant.RefillInstruction.DET_ORIG)
                    {
                        textItter.Style.Remove("display");
                    }
                    textItter.Value = entity.ItterNumber.ToString();
                }
            }
        }

        protected void cbpProcessItter_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string lstPrescriptionOrderDetailID = hdnSelectedID.Value;
            string lstExpiredDate = hdnSelectedDate.Value;
            string lstGcItter = hdnSelectedGcItter.Value;
            string lstItterNo = hdnSelectedItterNo.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            if (!String.IsNullOrEmpty(lstExpiredDate) || !String.IsNullOrEmpty(lstGcItter))
            {
                //Update Drug Expired Date
                string[] detailIDLst = lstPrescriptionOrderDetailID.Split(',');
                string[] expiredDateLst = lstExpiredDate.Split(',');
                string[] gcItter = lstGcItter.Split(',');
                string[] itterNo = lstItterNo.Split(',');

                int index = 0;
                foreach (string recordID in detailIDLst)
                {
                    if (!string.IsNullOrEmpty(expiredDateLst[index]))
                    {
                        DateTime expiredDate;
                        string format = Constant.FormatString.DATE_PICKER_FORMAT;
                        expiredDate = DateTime.ParseExact(expiredDateLst[index], format, CultureInfo.InvariantCulture);
                        PrescriptionOrderDt orderDt = BusinessLayer.GetPrescriptionOrderDt(Convert.ToInt32(recordID));
                        PrescriptionOrderHd orderHd = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                        if (orderDt != null)
                        {
                            orderDt.ExpiredDate = expiredDate;
                            orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePrescriptionOrderDt(orderDt);
                        }
                        if (orderHd != null)
                        {
                            orderHd.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                            orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePrescriptionOrderHd(orderHd);
                        }
                    }

                    if (!string.IsNullOrEmpty(gcItter[index]))
                    {
                        PatientChargesDt chargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("PrescriptionOrderDetailID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", Convert.ToInt32(recordID), Constant.TransactionStatus.VOID)).FirstOrDefault();
                        if (chargesDt != null)
                        {
                            PatientChargesDtInfo info = BusinessLayer.GetPatientChargesDtInfo(chargesDt.ID);
                            info.GCRefillInstruction = gcItter[index];
                            if (!String.IsNullOrEmpty(itterNo[index]))
                            {
                                info.ItterNumber = Convert.ToInt32(itterNo[index]);
                            }
                            info.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientChargesDtInfo(info);
                        }
                    }
                    index += 1;
                }
            }

            string result = "";
            panel.JSProperties["cpZebraPrinting"] = result;
        }
    }
}