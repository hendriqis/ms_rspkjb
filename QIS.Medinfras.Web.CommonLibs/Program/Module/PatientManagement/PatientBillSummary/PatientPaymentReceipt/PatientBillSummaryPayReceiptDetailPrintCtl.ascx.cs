using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public partial class PatientBillSummaryPayReceiptDetailPrintCtl : BaseViewPopupCtl
    {
        private string[] lstSelectedPayment = null;
        public override void InitializeDataControl(string param)
        {
            hdnParam1.Value = param.Split(';')[0];
            hdnParam2.Value = param.Split(';')[1];

            hdnRegistrationID.Value = hdnParam1.Value.Split('|')[0];
            hdnDepartmentID.Value = hdnParam1.Value.Split('|')[1];
            hdnPatientName.Value = hdnParam1.Value.Split('|')[3];
            hdnIsCumulative.Value = hdnParam1.Value.Split('|')[4];

            int piutangInstansiCount = 0;
            int count = Convert.ToString(hdnParam2.Value).Split('|').Count();
            for (int i = 0; i < count; i++)
            {
                if (BusinessLayer.GetPatientPaymentHd(Convert.ToInt32(Convert.ToString(hdnParam2.Value).Split('|')[i])).GCPaymentType == Constant.PaymentType.AR_PAYER)
                {
                    piutangInstansiCount = piutangInstansiCount + 1;
                }
            }

            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI, Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_BAHASA_ASING,
                Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_RAWATJALAN, Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_RAWATJALAN_BAHASA_ASING,
                Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_MCU, Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_MCU_BAHASA_ASING,
                Constant.SettingParameter.FN_IS_ALLOW_BACKDATED_PAYMENT,
                Constant.SettingParameter.FN_PROSES_CETAK_KWITANSI_LANGSUNG_PREVIEW_DAN_HITUNG_JUMLAH_CETAK));

            hdnReportCodeReceipt.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI).FirstOrDefault().ParameterValue;
            hdnReportCodeReceiptEnglish.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_BAHASA_ASING).FirstOrDefault().ParameterValue;

            hdnReportCodeReceiptOutpatient.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_RAWATJALAN).FirstOrDefault().ParameterValue;
            hdnReportCodeReceiptEnglishOutpatient.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_RAWATJALAN_BAHASA_ASING).FirstOrDefault().ParameterValue;

            hdnReportCodeReceiptMCU.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_MCU).FirstOrDefault().ParameterValue;
            hdnReportCodeReceiptEnglishMCU.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_MCU_BAHASA_ASING).FirstOrDefault().ParameterValue;

            hdnIsAllowBackDate.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_IS_ALLOW_BACKDATED_PAYMENT).FirstOrDefault().ParameterValue;

            hdnIsPaymentReceiptPrintPreviewFirst.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_PROSES_CETAK_KWITANSI_LANGSUNG_PREVIEW_DAN_HITUNG_JUMLAH_CETAK).FirstOrDefault().ParameterValue;

            if (piutangInstansiCount > 0)
            {
                txtReceiptName.Text = BusinessLayer.GetvRegistrationList(String.Format("RegistrationID = {0}", hdnRegistrationID.Value)).FirstOrDefault().BusinessPartnerName;
            }
            else
            {
                txtReceiptName.Text = hdnPatientName.Value;
            }

            if (hdnDepartmentID.Value != Constant.Facility.INPATIENT)
            {
                SettingParameterDt setvarDotMatrix = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'",
                    Constant.SettingParameter.IS_OUTPATIENT_RECEIPT_USING_DOT_MATRIX)).FirstOrDefault();

                if (setvarDotMatrix.ParameterValue == "1")
                {
                    hdnIsDotMatrixAndOutpatient.Value = "1";
                }
                else
                {
                    hdnIsDotMatrixAndOutpatient.Value = "0";
                }
            }
            else { 
                //ranap
                SettingParameterDt setvarDotMatrix = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'",
                    Constant.SettingParameter.IS_INPATIENT_RECEIPT_USING_DOT_MATRIX)).FirstOrDefault();

                if (setvarDotMatrix.ParameterValue == "1")
                {
                    hdnIsDotMatrixAndInpatien.Value = "1";
                }
                else
                {
                    hdnIsDotMatrixAndInpatien.Value = "0";
                }
                
            }

            txtPaymentReceiptDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPaymentReceiptTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            Helper.SetControlEntrySetting(txtPaymentReceiptDate, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtPaymentReceiptTime, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtReceiptName, new ControlEntrySetting(true, true, true), "mpTrxPopup");
        }

        protected void cbpPatientPaymentReceiptProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string retval = "";
            string errMessage = "";
            if (OnSaveRecord(ref retval, ref errMessage))
            {
                if (hdnIsDotMatrixAndOutpatient.Value == "1" || hdnIsDotMatrixAndInpatien.Value == "1")
                {
                    int selectedLanguage = rblReceiptOption.SelectedIndex;
                    if (selectedLanguage == 0)
                    {
                        string dotMatrix = PrintKwitansiDotMatrix(ref retval, ref errMessage);
                        result = dotMatrix;
                    }
                    else
                    {
                        string dotMatrix = PrintKwitansiDotMatrixEng(ref retval, ref errMessage);
                        result = dotMatrix;
                    }
                }
                else
                {
                    result = "success";
                }
            }
            else
            {
                result = "fail|" + errMessage;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }

        private string PrintKwitansiDotMatrix(ref string retval, ref string errMessage)
        {
            string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
            AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

            List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
            string result = "";
            Healthcare entityHSU = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string ipAddress = HttpContext.Current.Request.UserHostAddress;

            filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                            ipAddress, Constant.DirectPrintType.PRINT_KWITANSI_RAJAL);

            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            try
            {
                if (entityHSU.Initial == "rsdo-soba")
                {
                    string paymentReceipt = retval;
                    ZebraPrinting.PrintKwitansiIndRSDOSOBA(paymentReceipt, lstPrinter[0].PrinterName);
                }
                else if (entityHSU.Initial == "RSSES") {
                    int paymentReceipt = Convert.ToInt32(retval);
                    if (lstPrinter.Count == 0)
                    {
                        errMessage = "konfigurasi printer ip tidak ditemukan";
                        result = "";
                    }
                    else {
                        ZebraPrinting.PintKwitansiIndoRSSES(paymentReceipt, lstPrinter[0].PrinterName);
                    }
                    
                }
                else
                {
                    string id = hdnParamReport.Value;
                    if (entityHSU.Initial == "RSMD")
                    {
                        id = retval;
                    }
                    result = ZebraPrinting.PrintKwitansi(id, "");
                }
            }
            catch (Exception ex)
            {
                result = "";
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            finally
            {
            }
            return result;
        }

        private string PrintKwitansiDotMatrixEng(ref string retval, ref string errMessage)
        {
            string result = "";
            string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
            AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

            List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
            Healthcare entityHSU = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string ipAddress = HttpContext.Current.Request.UserHostAddress;

            filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                            ipAddress, Constant.DirectPrintType.PRINT_KWITANSI_RAJAL);

            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

            try
            {
                if (entityHSU.Initial == "rsdo-soba")
                {
                    string paymentReceipt = retval;
                    ZebraPrinting.PrintKwitansiEngRSDOSOBA(paymentReceipt, lstPrinter[0].PrinterName);
                }
                else if (entityHSU.Initial == "RSSES")
                {
                    int paymentReceipt = Convert.ToInt32(retval);
                    if (lstPrinter.Count == 0)
                    {
                        errMessage = "konfigurasi printer ip tidak ditemukan";
                        result = "";
                    }
                    else {
                        ZebraPrinting.PintKwitansiIndoRSSES(paymentReceipt, lstPrinter[0].PrinterName);
                    }
                   
                }
                else {
                    string id = retval;
                    if (entityHSU.Initial == "RSMD")
                    {
                        id = retval;
                    }
                    result = ZebraPrinting.PrintKwitansiEng(id, "");
                }
              
            }
            catch (Exception ex)
            {
                result = "";
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            finally
            {
            }
            return result;
        }

        private bool OnSaveRecord(ref string retval, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PaymentReceiptDao entityDao = new PaymentReceiptDao(ctx);
            PatientPaymentHdDao entityPaymentDao = new PatientPaymentHdDao(ctx);

            try
            {
                string transactionCode = "";

                string filterPayment = string.Format("PaymentID IN ({0})", hdnParam2.Value.Replace("|", ","));
                List<PatientPaymentHd> lstPayment = BusinessLayer.GetPatientPaymentHdList(filterPayment, ctx);
                if (lstPayment.Where(a => a.PaymentReceiptID != null && a.PaymentReceiptID != 0).ToList().Count == 0)
                {

                    PaymentReceipt entity = new PaymentReceipt();
                    entity.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                    if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                        transactionCode = Constant.TransactionCode.ER_PAYMENT_RECEIPT;
                    else if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
                        transactionCode = Constant.TransactionCode.MCU_PAYMENT_RECEIPT;
                    else if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                        transactionCode = Constant.TransactionCode.IP_PAYMENT_RECEIPT;
                    else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                        transactionCode = Constant.TransactionCode.OP_PAYMENT_RECEIPT;
                    else if (hdnDepartmentID.Value == Constant.Facility.PHARMACY)
                        transactionCode = Constant.TransactionCode.PH_PAYMENT_RECEIPT;
                    else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                    {
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                            transactionCode = Constant.TransactionCode.LABORATORY_PAYMENT_RECEIPT;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            transactionCode = Constant.TransactionCode.IMAGING_PAYMENT_RECEIPT;
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                            transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PAYMENT_RECEIPT;
                        else
                            transactionCode = Constant.TransactionCode.OTHER_PAYMENT_RECEIPT;
                    }
                    lstSelectedPayment = hdnParam2.Value.Split('|');
                    DateTime paymentReceiptDate = Helper.GetDatePickerValue(txtPaymentReceiptDate.Text);
                    string[] tempPaymentReceiptTime = txtPaymentReceiptTime.Text.Split(':');
                    entity.PaymentReceiptDateTime = new DateTime(paymentReceiptDate.Year, paymentReceiptDate.Month, paymentReceiptDate.Day, Convert.ToInt32(tempPaymentReceiptTime[0]), Convert.ToInt32(tempPaymentReceiptTime[1]), 0);
                    entity.IsMultiPayment = chkIsMultiPayment.Checked;
                    entity.IsPrintWithDiagnose = chkIsWithDiagnose.Checked;
                    entity.LastPrintedDate = DateTime.Now;
                    entity.PaymentReceiptNo = BusinessLayer.GenerateTransactionNo(transactionCode, entity.PaymentReceiptDateTime, ctx);
                    entity.PrintAsName = txtReceiptName.Text;
                    entity.Remarks = txtRemarks.Text;
                    if (hdnIsPaymentReceiptPrintPreviewFirst.Value == "1")
                    {
                        entity.PrintNumber = 1;
                    }
                    else
                    {
                        entity.PrintNumber = 0;
                    }

                    if (hdnIsCumulative.Value == "1")
                    {
                        entity.IsCumulative = true;
                    }
                    else
                    {
                        entity.IsCumulative = false;
                    }

                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entity.ReceiptAmount = Convert.ToDecimal(hdnParam1.Value.Split('|')[2]);

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entity.PaymentReceiptID = entityDao.InsertReturnPrimaryKeyID(entity);

                    retval = entity.PaymentReceiptID.ToString();
                    foreach (string param in lstSelectedPayment)
                    {
                        int paymentID = Convert.ToInt32(param);

                        PatientPaymentHd entityPayment = entityPaymentDao.Get(paymentID);
                        entityPayment.PaymentReceiptID = entity.PaymentReceiptID;
                        entityPayment.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPaymentDao.Update(entityPayment);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Nomor pembayaran sudah diproses kwitansi.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}