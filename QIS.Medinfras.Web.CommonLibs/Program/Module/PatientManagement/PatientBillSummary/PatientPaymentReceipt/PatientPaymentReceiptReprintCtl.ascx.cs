using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientPaymentReceiptReprintCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnRegistrationID.Value = hdnParam.Value.Split('|')[0];
            hdnPaymentReceiptID.Value = hdnParam.Value.Split('|')[1];
            hdnDepartmentID.Value = hdnParam.Value.Split('|')[2];
            List<StandardCode> lstReprintReason = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.RECEIPT_REPRINT_REASON));
            Methods.SetComboBoxField<StandardCode>(cboReprintReason, lstReprintReason, "StandardCodeName", "StandardCodeID");
            cboReprintReason.SelectedIndex = 0;

            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}')",
                Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI, Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_BAHASA_ASING,
                Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_RAWATJALAN, Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_RAWATJALAN_BAHASA_ASING,
                Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_MCU, Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_MCU_BAHASA_ASING));

            hdnReportCodeReceiptReprint.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI).FirstOrDefault().ParameterValue;
            hdnReportCodeReceiptEnglishReprint.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_BAHASA_ASING).FirstOrDefault().ParameterValue;

            hdnReportCodeReceiptOutpatientReprint.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_RAWATJALAN).FirstOrDefault().ParameterValue;
            hdnReportCodeReceiptEnglishOutpatientReprint.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_RAWATJALAN_BAHASA_ASING).FirstOrDefault().ParameterValue;

            hdnReportCodeReceiptMCU.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_MCU).FirstOrDefault().ParameterValue;
            hdnReportCodeReceiptEnglishMCU.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORTCODE_CETAK_KWITANSI_MCU_BAHASA_ASING).FirstOrDefault().ParameterValue;

            if (hdnDepartmentID.Value != Constant.Facility.INPATIENT)
            {
                SettingParameterDt setvarDotMatrix = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'",
                    Constant.SettingParameter.IS_OUTPATIENT_RECEIPT_USING_DOT_MATRIX)).FirstOrDefault();

                if (setvarDotMatrix.ParameterValue == "1")
                {
                    hdnIsDotMatrixAndOutpatientReprint.Value = "1";
                }
                else
                {
                    hdnIsDotMatrixAndOutpatientReprint.Value = "0";
                }
            }

            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                SettingParameterDt setvarDotMatrix = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'",
                    Constant.SettingParameter.IS_INPATIENT_RECEIPT_USING_DOT_MATRIX)).FirstOrDefault();

                if (setvarDotMatrix.ParameterValue == "1")
                {
                    hdnIsDotMatrixAndOutpatientReprint.Value = "1";
                }
                else
                {
                    hdnIsDotMatrixAndOutpatientReprint.Value = "0";
                }
            }
        }

        protected void cbpPatientPaymentReceiptReprint_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnReprintRecord(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpPatientPaymentReceiptReprintDotMatrix_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnReprintRecord(ref errMessage))
            {
                int selectedLanguage = rblReceiptOption.SelectedIndex;
                if (selectedLanguage == 0)
                {
                    result = PrintKwitansiDotMatrix(ref errMessage);
                }
                else 
                {
                    result = PrintKwitansiDotMatrixEng(ref errMessage);                
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private string PrintKwitansiDotMatrix(ref string errMessage)
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
                    string paymentReceipt = hdnPaymentReceiptID.Value;
                    ZebraPrinting.PrintKwitansiIndRSDOSOBA(paymentReceipt, lstPrinter[0].PrinterName);
                }
                else if (entityHSU.Initial == "RSSES")
                    {
                        if (lstPrinter.Count == 0){
                            errMessage = "konfigurasi printer ip tidak ditemukan";
                            result = "";

                        }else {
                            int paymentReceipt = Convert.ToInt32(hdnPaymentReceiptID.Value);
                            ZebraPrinting.PintKwitansiIndoRSSES(paymentReceipt, lstPrinter[0].PrinterName);
                        }
                    }
                else
                {
                    string id = hdnParamReport.Value;
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

        private string PrintKwitansiDotMatrixEng(ref string errMessage)
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
                    string paymentReceipt = hdnPaymentReceiptID.Value;
                    ZebraPrinting.PrintKwitansiEngRSDOSOBA(paymentReceipt, lstPrinter[0].PrinterName);
                }
                else if (entityHSU.Initial == "RSCK")
                    {
                        int paymentReceipt = Convert.ToInt32(hdnPaymentReceiptID.Value);
                        ZebraPrinting.PintKwitansiIndoRSSES(paymentReceipt, lstPrinter[0].PrinterName);
                    }
                else
                {
                    string id = hdnParamReport.Value;
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

        private bool OnReprintRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PaymentReceiptDao entityDao = new PaymentReceiptDao(ctx);

            try
            {
                PaymentReceipt entity = BusinessLayer.GetPaymentReceiptList(string.Format("PaymentReceiptID = {0} AND IsDeleted = 0", hdnPaymentReceiptID.Value))[0];
                entity.GCReprintReason = cboReprintReason.Value.ToString();
                if (txtReason.Text != "" && txtReason.Text != null)
                    entity.ReprintReason = txtReason.Text;
                entity.PrintNumber += 1;
                entity.LastPrintedDate = DateTime.Now;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityDao.Update(entity);

                ctx.CommitTransaction();
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