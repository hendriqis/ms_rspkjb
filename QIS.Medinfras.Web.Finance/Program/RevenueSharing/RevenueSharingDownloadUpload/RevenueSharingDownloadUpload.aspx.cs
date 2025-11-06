using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingDownloadUpload : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_DOWNLOAD_UPLOAD_PROCESS;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnFileName.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_FILE_NAME_DOCUMENT_REVENUE_SHARING_DOWNLOAD_UPLOAD).ParameterValue;

            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Variable> lstPayerType = new List<Variable>();
            lstPayerType.Add(new Variable { Code = "1", Value = "PRIBADI" });
            lstPayerType.Add(new Variable { Code = "2", Value = "NON PRIBADI" });
            Methods.SetComboBoxField<Variable>(cboPayerType, lstPayerType, "Value", "Code");
            cboPayerType.Value = "1";

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                                    "IsDeleted = 0 AND IsActive = 1 AND ParentID IN ('{0}','{1}','{2}')",
                                                                    Constant.StandardCode.REVENUE_REDUCTION,
                                                                    Constant.StandardCode.REVENUE_PAYMENT_METHOD,
                                                                    Constant.StandardCode.REVENUE_PERIODE_TYPE
                                                                ));

            Methods.SetComboBoxField<StandardCode>(cboReduction, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.REVENUE_REDUCTION).ToList(), "StandardCodeName", "StandardCodeID");
            cboReduction.Value = Constant.RevenueReduction.REDUCTION_0;

            Methods.SetComboBoxField<StandardCode>(cboPaymentMethod, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.REVENUE_PAYMENT_METHOD).ToList(), "StandardCodeName", "StandardCodeID");
            cboPaymentMethod.Value = Constant.RevenuePaymentMethod.TUNAI;

        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
            TransDetailForProcessRevenueSharingPersonalTempDao transPersonalDao = new TransDetailForProcessRevenueSharingPersonalTempDao(ctx);
            TransDetailForProcessRevenueSharingNonPersonalTempDao transNonPersonalDao = new TransDetailForProcessRevenueSharingNonPersonalTempDao(ctx);

            try
            {
                if (type == "download")
                {
                    #region Download Document

                    string reportCode = "";
                    if (cboPayerType.Value.ToString() == "1")
                    {
                        reportCode = string.Format("ReportCode = '{0}'", "FN-00138");
                    }
                    else
                    {
                        reportCode = string.Format("ReportCode = '{0}'", "FN-00139");
                    }
                    ReportMaster rm = BusinessLayer.GetReportMasterList(reportCode).FirstOrDefault();

                    StringBuilder sbResult = new StringBuilder();

                    List<dynamic> lstDynamic = null;
                    List<Variable> lstVariable = new List<Variable>();

                    string oFromDate = Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112);
                    string oToDate = Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112);

                    lstVariable.Add(new Variable { Code = "FromDate", Value = oFromDate });
                    lstVariable.Add(new Variable { Code = "ToDate", Value = oToDate });

                    lstDynamic = BusinessLayer.GetDataReport(rm.ObjectTypeName, lstVariable);

                    if (lstDynamic.Count() != 0)
                    {
                        dynamic fields = lstDynamic[0];

                        foreach (var prop in fields.GetType().GetProperties())
                        {
                            sbResult.Append(prop.Name);
                            sbResult.Append(",");
                        }

                        sbResult.Append("\r\n");

                        for (int i = 0; i < lstDynamic.Count; ++i)
                        {
                            dynamic entity = lstDynamic[i];

                            foreach (var prop in entity.GetType().GetProperties())
                            {
                                sbResult.Append(prop.GetValue(entity, null).ToString().Replace(',', '_'));
                                sbResult.Append(",");
                            }

                            sbResult.Append("\r\n");
                        }

                        retval = sbResult.ToString();

                    }
                    else
                    {
                        retval = "Download Failed";
                    }

                    ctx.RollBackTransaction();

                    #endregion
                }
                else if (type == "upload")
                {
                    #region Upload Document

                    string fileUpload = hdnRevenueSharingUploadedFile.Value;
                    if (fileUpload != "")
                    {
                        string[] parts = Regex.Split(fileUpload, ",").Skip(1).ToArray();
                        fileUpload = String.Join(",", parts);
                    }

                    string path = AppConfigManager.QISPhysicalDirectory;
                    path += string.Format("{0}\\", AppConfigManager.QISFinanceUploadDocument.Replace('/', '\\'));

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    else
                    {
                        Directory.Delete(path, true);
                        Directory.CreateDirectory(path);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        if (cboPayerType.Value.ToString() == "1")
                        {
                            List<TransDetailForProcessRevenueSharingPersonalTemp> entityList = BusinessLayer.GetTransDetailForProcessRevenueSharingPersonalTempList("1=1", ctx);
                            if (entityList.Count() > 0)
                            {
                                transPersonalDao.Delete();
                            }
                        }
                        else
                        {
                            List<TransDetailForProcessRevenueSharingNonPersonalTemp> entityList = BusinessLayer.GetTransDetailForProcessRevenueSharingNonPersonalTempList("1=1", ctx);
                            if (entityList.Count() > 0)
                            {
                                transNonPersonalDao.Delete();
                            }
                        }
                    }

                    FileStream fs = new FileStream(string.Format("{0}{1}", path, hdnFileName.Value), FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);
                    byte[] data = Convert.FromBase64String(fileUpload);
                    bw.Write(data);
                    bw.Close();

                    string[] lstTemp = File.ReadAllLines(string.Format("{0}{1}", path, hdnFileName.Value));

                    int rowCount = 0;
                    int oParamUpload = 1;

                    foreach (string temp in lstTemp)
                    {
                        if (rowCount != 0)
                        {
                            if (temp.Contains(','))
                            {
                                List<String> fieldTemp = temp.Split(',').ToList();

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                if (cboPayerType.Value.ToString() == "1")
                                {
                                    oParamUpload = 1;

                                    #region PERSONAL

                                    TransDetailForProcessRevenueSharingPersonalTemp oData1 = new TransDetailForProcessRevenueSharingPersonalTemp();
                                    oData1.PaymentNo = fieldTemp[0];
                                    oData1.NO_CM = fieldTemp[1];
                                    oData1.Register = fieldTemp[2];
                                    oData1.Register_Asal = fieldTemp[3];
                                    oData1.Register_Tujuan = fieldTemp[4];
                                    oData1.TGL_TRANS = fieldTemp[5];
                                    oData1.JAM_TRANS = fieldTemp[6];
                                    oData1.nm_pasien = fieldTemp[7];
                                    oData1.kd_tindak = fieldTemp[8];
                                    oData1.MED_ItemCode = fieldTemp[9];
                                    oData1.MED_ItemName = fieldTemp[10];
                                    oData1.KODE = fieldTemp[11];
                                    oData1.MED_ParamedicCode = fieldTemp[12];
                                    oData1.MED_ParamedicName = fieldTemp[13];
                                    oData1.kd_hnr = fieldTemp[14];
                                    oData1.RS = Convert.ToDecimal(fieldTemp[15]);
                                    oData1.DR = Convert.ToDecimal(fieldTemp[16]);
                                    oData1.TGL_BAYAR = fieldTemp[17];
                                    oData1.keterangan = fieldTemp[18];
                                    oData1.status = fieldTemp[19];
                                    oData1.status_COB = fieldTemp[20];
                                    oData1.Status_Registrasi_Lunas = fieldTemp[21];
                                    oData1.ID = Convert.ToInt32(fieldTemp[22]);
                                    oData1.TransactionID = Convert.ToInt32(fieldTemp[23]);
                                    oData1.RevenueSharingNo = fieldTemp[24];
                                    oData1.ChargesAmount = Convert.ToDecimal(fieldTemp[25]);
                                    oData1.AdminAmount = Convert.ToDecimal(fieldTemp[26]);
                                    oData1.DiscountAmount = Convert.ToDecimal(fieldTemp[27]);
                                    oData1.SourceAmount = Convert.ToDecimal(fieldTemp[28]);
                                    oData1.TransferAmount = Convert.ToDecimal(fieldTemp[29]);
                                    oData1.DownPaymentAmount = Convert.ToDecimal(fieldTemp[30]);
                                    oData1.PaymentAmount = Convert.ToDecimal(fieldTemp[31]);
                                    oData1.ARInProcessAmount = Convert.ToDecimal(fieldTemp[32]);
                                    oData1.ARInvoiceAmount = Convert.ToDecimal(fieldTemp[33]);
                                    oData1.ARReceivingAmount = Convert.ToDecimal(fieldTemp[34]);
                                    oData1.UangMukaPasien = Convert.ToDecimal(fieldTemp[35]);
                                    oData1.PelunasanPasien = Convert.ToDecimal(fieldTemp[36]);
                                    oData1.PiutangPasien = Convert.ToDecimal(fieldTemp[37]);
                                    oData1.PiutangInstansi = Convert.ToDecimal(fieldTemp[38]);
                                    oData1.TotalChargesAmount = Convert.ToDecimal(fieldTemp[39]);
                                    oData1.TotalPaymentAmount = Convert.ToDecimal(fieldTemp[40]);
                                    transPersonalDao.Insert(oData1);

                                    if (oData1.ID != null && oData1.ID != 0 && oData1.kd_hnr != null && oData1.kd_hnr != "")
                                    {
                                        List<RevenueSharingHd> lstRevSharing = BusinessLayer.GetRevenueSharingHdList(string.Format("RevenueSharingCode = '{0}' AND IsDeleted = 0", oData1.kd_hnr), ctx);

                                        if (lstRevSharing.Count() > 0)
                                        {
                                            PatientChargesDt chargesDt = chargesDtDao.Get(Convert.ToInt32(oData1.ID));
                                            chargesDt.RevenueSharingID = lstRevSharing.FirstOrDefault().RevenueSharingID;
                                            chargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            chargesDtDao.Update(chargesDt);
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    oParamUpload = 2;

                                    #region NON PERSONAL

                                    TransDetailForProcessRevenueSharingNonPersonalTemp oData2 = new TransDetailForProcessRevenueSharingNonPersonalTemp();
                                    oData2.ARInvoiceNo = fieldTemp[0];
                                    oData2.NO_CM = fieldTemp[1];
                                    oData2.Register = fieldTemp[2];
                                    oData2.Register_Asal = fieldTemp[3];
                                    oData2.Register_Tujuan = fieldTemp[4];
                                    oData2.TGL_TRANS = fieldTemp[5];
                                    oData2.JAM_TRANS = fieldTemp[6];
                                    oData2.nm_pasien = fieldTemp[7];
                                    oData2.kd_tindak = fieldTemp[8];
                                    oData2.MED_ItemCode = fieldTemp[9];
                                    oData2.MED_ItemName = fieldTemp[10];
                                    oData2.KODE = fieldTemp[11];
                                    oData2.MED_ParamedicCode = fieldTemp[12];
                                    oData2.MED_ParamedicName = fieldTemp[13];
                                    oData2.kd_hnr = fieldTemp[14];
                                    oData2.RS = Convert.ToDecimal(fieldTemp[15]);
                                    oData2.DR = Convert.ToDecimal(fieldTemp[16]);
                                    oData2.TGL_BAYAR = fieldTemp[17];
                                    oData2.keterangan = fieldTemp[18];
                                    oData2.status = fieldTemp[19];
                                    oData2.status_COB = fieldTemp[20];
                                    oData2.Status_Registrasi_Lunas = fieldTemp[21];
                                    oData2.ID = Convert.ToInt32(fieldTemp[22]);
                                    oData2.TransactionID = Convert.ToInt32(fieldTemp[23]);
                                    oData2.RevenueSharingNo = fieldTemp[24];
                                    oData2.ChargesAmount = Convert.ToDecimal(fieldTemp[25]);
                                    oData2.AdminAmount = Convert.ToDecimal(fieldTemp[26]);
                                    oData2.DiscountAmount = Convert.ToDecimal(fieldTemp[27]);
                                    oData2.SourceAmount = Convert.ToDecimal(fieldTemp[28]);
                                    oData2.TransferAmount = Convert.ToDecimal(fieldTemp[29]);
                                    oData2.DownPaymentAmount = Convert.ToDecimal(fieldTemp[30]);
                                    oData2.PaymentAmount = Convert.ToDecimal(fieldTemp[31]);
                                    oData2.ARInProcessAmount = Convert.ToDecimal(fieldTemp[32]);
                                    oData2.ARInvoiceAmount = Convert.ToDecimal(fieldTemp[33]);
                                    oData2.ARReceivingAmount = Convert.ToDecimal(fieldTemp[34]);
                                    oData2.UangMukaPasien = Convert.ToDecimal(fieldTemp[35]);
                                    oData2.PelunasanPasien = Convert.ToDecimal(fieldTemp[36]);
                                    oData2.PiutangPasien = Convert.ToDecimal(fieldTemp[37]);
                                    oData2.PiutangInstansi = Convert.ToDecimal(fieldTemp[38]);
                                    oData2.TotalChargesAmount = Convert.ToDecimal(fieldTemp[39]);
                                    oData2.TotalPaymentAmount = Convert.ToDecimal(fieldTemp[40]);
                                    transNonPersonalDao.Insert(oData2);

                                    if (oData2.ID != null && oData2.ID != 0 && oData2.kd_hnr != null && oData2.kd_hnr != "")
                                    {
                                        List<RevenueSharingHd> lstRevSharing = BusinessLayer.GetRevenueSharingHdList(string.Format("RevenueSharingCode = '{0}' AND IsDeleted = 0", oData2.kd_hnr), ctx);

                                        if (lstRevSharing.Count() > 0)
                                        {
                                            PatientChargesDt chargesDt = chargesDtDao.Get(Convert.ToInt32(oData2.ID));
                                            chargesDt.RevenueSharingID = lstRevSharing.FirstOrDefault().RevenueSharingID;
                                            chargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            chargesDtDao.Update(chargesDt);
                                        }
                                    }
                                    #endregion
                                }

                            }
                        }
                        rowCount += 1;
                    }

                    ctx.CommitTransaction();

                    retval = BusinessLayer.GenerateParamedicRevenueSharingFromUpload(oParamUpload, cboReduction.Value.ToString(), cboPaymentMethod.Value.ToString(), AppSession.UserLogin.UserID);
                    #endregion
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

        protected class DataGenerateRevenueSharing
        {
            private int _ParamedicID;
            private string _DepartmentID;
            private string _GCReduction;
            private string _GCPaymentMethod;
            private string _GCPeriodeType;
            private string _PeriodeDateStart;
            private string _ListTransactionDtID;
            private int _CreatedBy;

            public int ParamedicID
            {
                get { return _ParamedicID; }
                set { _ParamedicID = value; }
            }
            public string DepartmentID
            {
                get { return _DepartmentID; }
                set { _DepartmentID = value; }
            }
            public string GCReduction
            {
                get { return _GCReduction; }
                set { _GCReduction = value; }
            }
            public string GCPaymentMethod
            {
                get { return _GCPaymentMethod; }
                set { _GCPaymentMethod = value; }
            }
            public string GCPeriodeType
            {
                get { return _GCPeriodeType; }
                set { _GCPeriodeType = value; }
            }
            public string PeriodeDateStart
            {
                get { return _PeriodeDateStart; }
                set { _PeriodeDateStart = value; }
            }
            public string ListTransactionDtID
            {
                get { return _ListTransactionDtID; }
                set { _ListTransactionDtID = value; }
            }
            public int CreatedBy
            {
                get { return _CreatedBy; }
                set { _CreatedBy = value; }
            }


        }

    }
}