using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using DevExpress.Web.ASPxEditors;
using QISEncryption;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class DetailPiutangInformation : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.INFORMASI_DETAIL_PIUTANG;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnFileName.Value = "Detail Piutang";
            
            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            //BindGridView();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            DateTime DateFrom = Helper.GetDatePickerValue(txtPeriodFrom.Text);
            DateTime DateTo = Helper.GetDatePickerValue(txtPeriodTo.Text);            

            string datePeriod = string.Format("{0}|{1}", DateFrom.ToString(Constant.FormatString.DATE_FORMAT_112), DateTo.ToString(Constant.FormatString.DATE_FORMAT_112));
            List<GetARInvoiceDetailTransactionRSSBBInformation> lstEntity = BusinessLayer.GetARInvoiceDetailTransactionRSSBBInformation(datePeriod, txtARInvoiceNo.Text);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;

            DateTime DateFrom = Helper.GetDatePickerValue(txtPeriodFrom.Text);
            DateTime DateTo = Helper.GetDatePickerValue(txtPeriodTo.Text);
            string datePeriod = string.Format("{0}|{1}", DateFrom.ToString(Constant.FormatString.DATE_FORMAT_112), DateTo.ToString(Constant.FormatString.DATE_FORMAT_112));

            SqlConnection sqlCon = new SqlConnection(getCNSetting());
            sqlCon.Open();
            try
            {
                if (type == "download")
                {
                    #region Download Document
                    StringBuilder sbResult = new StringBuilder();

                    string query = string.Format("EXEC GetARInvoiceDetailTransactionRSSBBInformation '{0}','{1}'", datePeriod, txtARInvoiceNo.Text);
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    object[] output = new object[reader.FieldCount];

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string name = reader.GetName(i);
                        if (name == "ARInvoiceNo")
                        {
                            output[i] = string.Format("NO TAGIHAN");                        
                        }
                        else if (name == "ARInvoiceDate")
                        {
                            output[i] = string.Format("TGL TAGIHAN");                            
                        }
                        else if (name == "BusinessPartnerName")
                        {
                            output[i] = string.Format("PENJAMIN KE 1");
                        }
                        else if (name == "BusinessPartnerName2")
                        {
                            output[i] = string.Format("PENJAMIN KE 2");
                        }
                        else if (name == "PaymentNo")
                        {
                            output[i] = string.Format("NO PEMIUTANGAN");
                        }
                        else if (name == "PaymentDate")
                        {
                            output[i] = string.Format("TGL PEMIUTANGAN");
                        }
                        else if (name == "RegistrationNo")
                        {
                            output[i] = string.Format("NO REGISTER");
                        }
                        else if (name == "RegistrationDate")
                        {
                            output[i] = string.Format("TGL MASUK");
                        }
                        else if (name == "DischargeDate")
                        {
                            output[i] = string.Format("TGL PULANG");
                        }
                        else if (name == "MedicalNo")
                        {
                            output[i] = string.Format("NO RM");
                        }
                        else if (name == "PatientName")
                        {
                            output[i] = string.Format("NAMA PASIEN");
                        }
                        else if (name == "DateOfBirth")
                        {
                            output[i] = string.Format("TGL LAHIR");
                        }
                        else if (name == "NoPeserta")
                        {
                            output[i] = string.Format("NO PESERTA");
                        }
                        else if (name == "NoSEP")
                        {
                            output[i] = string.Format("NO SEP");
                        }
                        else if (name == "ReferralNo")
                        {
                            output[i] = string.Format("NO RUJUKAN");
                        }
                        else if (name == "VisitServiceUnitName")
                        {
                            output[i] = string.Format("UNIT PELAYANAN ASAL");
                        }
                        else if (name == "RoomName")
                        {
                            output[i] = string.Format("KAMAR");
                        }
                        else if (name == "BedCode")
                        {
                            output[i] = string.Format("BED");
                        }
                        else if (name == "VisitClassName")
                        {
                            output[i] = string.Format("KELAS");
                        }
                        else if (name == "VisitParamedicName")
                        {
                            output[i] = string.Format("DPJP UTAMA");
                        }
                        else if (name == "Diagnose")
                        {
                            output[i] = string.Format("DIAGNOSE");
                        }
                        else if (name == "TransactionDate")
                        {
                            output[i] = string.Format("TGL TRANSAKSI");
                        }
                        else if (name == "DepartmentID")
                        {
                            output[i] = string.Format("DEPARTEMEN");
                        }
                        else if (name == "ChargesServiceUnitName")
                        {
                            output[i] = string.Format("UNIT PELAYANAN TRANSAKSI");
                        }
                        else if (name == "ItemGroupName1")
                        {
                            output[i] = string.Format("GROUP TRANSAKSI");
                        }
                        else if (name == "ItemCode")
                        {
                            output[i] = string.Format("KODE TRANSAKSI");
                        }
                        else if (name == "ItemName1")
                        {
                            output[i] = string.Format("NAMA TRANSAKSI");
                        }
                        else if (name == "ChargedQuantity")
                        {
                            output[i] = string.Format("JUMLAH");
                        }
                        else if (name == "ItemUnit")
                        {
                            output[i] = string.Format("SATUAN");
                        }
                        else if (name == "BillingGroupName")
                        {
                            output[i] = string.Format("KELOMPOK RINCIAN TRANSAKSI");
                        }
                        else if (name == "ChargesParamedicName")
                        {
                            output[i] = string.Format("NAMA DOKTER");
                        }
                        else if (name == "UnitAmount")
                        {
                            output[i] = string.Format("HARGA SATUAN YAKES");
                        }
                        else if (name == "PatientAmount")
                        {
                            output[i] = string.Format("TANGGUNGAN PASIEN");
                        }
                        else if (name == "PayerAmount")
                        {
                            output[i] = string.Format("TANGGUNGAN PENJAMIN 1");
                        }
                        else if (name == "PayerAmount2")
                        {
                            output[i] = string.Format("TANGGUNGAN PENJAMIN 2");
                        }
                        else if (name == "TotalCorporateAmount1")
                        {
                            output[i] = string.Format("TANGGUNGAN YAKES");
                        }
                        else if (name == "TotalCorporateAmount2")
                        {
                            output[i] = string.Format("TANGGUNGAN BPJS");
                        }                        
                    }

                    sbResult.Append(string.Join(",", output));
                    sbResult.Append("\r\n");

                    while (reader.Read())
                    {
                        reader.GetValues(output);

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            output[i] = output[i].ToString().Replace(',', '_');
                        }

                        sbResult.Append(string.Join(",", output));
                        sbResult.Append("\r\n");
                    }

                    reader.Close();                    
                    retval = sbResult.ToString();
                    #endregion
                }
                else if (type == "downloadv2")
                {
                    #region Download Document V2
                    StringBuilder sbResult = new StringBuilder();

                    string query = string.Format("EXEC GetARInvoiceDetailTransactionRSSBBInformationV2 '{0}','{1}'", datePeriod, txtARInvoiceNo.Text);
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    object[] output = new object[reader.FieldCount];

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string name = reader.GetName(i);
                        if (name == "ARInvoiceNo")
                        {
                            output[i] = string.Format("NO TAGIHAN");
                        }
                        else if (name == "ARInvoiceDate")
                        {
                            output[i] = string.Format("TGL TAGIHAN");
                        }
                        //else if (name == "BusinessPartnerName")
                        //{
                        //    output[i] = string.Format("PENJAMIN KE 1");
                        //}
                        //else if (name == "BusinessPartnerName2")
                        //{
                        //    output[i] = string.Format("PENJAMIN KE 2");
                        //}
                        //else if (name == "PaymentNo")
                        //{
                        //    output[i] = string.Format("NO PEMIUTANGAN");
                        //}
                        else if (name == "PaymentDate")
                        {
                            output[i] = string.Format("TGL PEMIUTANGAN");
                        }
                        else if (name == "RegistrationNo")
                        {
                            output[i] = string.Format("NO REGISTER");
                        }
                        else if (name == "RegistrationDate")
                        {
                            output[i] = string.Format("TGL MASUK");
                        }
                        else if (name == "DischargeDate")
                        {
                            output[i] = string.Format("TGL PULANG");
                        }
                        else if (name == "MedicalNo")
                        {
                            output[i] = string.Format("NO RM");
                        }
                        else if (name == "CorporateAccountNo")
                        {
                            output[i] = string.Format("NIKES");
                        }
                        else if (name == "PatientName")
                        {
                            output[i] = string.Format("NAMA PASIEN");
                        }
                        //else if (name == "DateOfBirth")
                        //{
                        //    output[i] = string.Format("TGL LAHIR");
                        //}
                        //else if (name == "NoPeserta")
                        //{
                        //    output[i] = string.Format("NO PESERTA");
                        //}
                        //else if (name == "NoSEP")
                        //{
                        //    output[i] = string.Format("NO SEP");
                        //}
                        else if (name == "ReferralNo")
                        {
                            output[i] = string.Format("NO RUJUKAN");
                        }
                        //else if (name == "VisitServiceUnitName")
                        //{
                        //    output[i] = string.Format("UNIT PELAYANAN ASAL");
                        //}
                        //else if (name == "RoomName")
                        //{
                        //    output[i] = string.Format("KAMAR");
                        //}
                        //else if (name == "BedCode")
                        //{
                        //    output[i] = string.Format("BED");
                        //}
                        else if (name == "VisitClassName")
                        {
                            output[i] = string.Format("KELAS");
                        }
                        else if (name == "VisitParamedicName")
                        {
                            output[i] = string.Format("DPJP UTAMA");
                        }
                        else if (name == "Diagnose")
                        {
                            output[i] = string.Format("DIAGNOSE");
                        }
                        else if (name == "TransactionDate")
                        {
                            output[i] = string.Format("TGL TRANSAKSI");
                        }
                        else if (name == "DepartmentID")
                        {
                            output[i] = string.Format("DEPARTEMEN");
                        }
                        else if (name == "ChargesServiceUnitName")
                        {
                            output[i] = string.Format("UNIT PELAYANAN TRANSAKSI");
                        }
                        else if (name == "ItemGroupName1")
                        {
                            output[i] = string.Format("GROUP TRANSAKSI");
                        }
                        else if (name == "ItemCode")
                        {
                            output[i] = string.Format("KODE TRANSAKSI");
                        }
                        else if (name == "ItemName1")
                        {
                            output[i] = string.Format("NAMA TRANSAKSI");
                        }
                        else if (name == "ChargedQuantity")
                        {
                            output[i] = string.Format("JUMLAH");
                        }
                        else if (name == "ItemUnit")
                        {
                            output[i] = string.Format("SATUAN");
                        }
                        //else if (name == "BillingGroupName")
                        //{
                        //    output[i] = string.Format("KELOMPOK RINCIAN TRANSAKSI");
                        //}
                        else if (name == "ChargesParamedicName")
                        {
                            output[i] = string.Format("NAMA DOKTER");
                        }
                        else if (name == "UnitAmount")
                        {
                            output[i] = string.Format("HARGA SATUAN YAKES");
                        }
                        else if (name == "PatientAmount")
                        {
                            output[i] = string.Format("TANGGUNGAN PASIEN");
                        }
                        //else if (name == "PayerAmount")
                        //{
                        //    output[i] = string.Format("TANGGUNGAN PENJAMIN 1");
                        //}
                        //else if (name == "PayerAmount2")
                        //{
                        //    output[i] = string.Format("TANGGUNGAN PENJAMIN 2");
                        //}
                        else if (name == "TotalCorporateAmount1")
                        {
                            output[i] = string.Format("TANGGUNGAN YAKES");
                        }
                        else if (name == "TotalCorporateAmount2")
                        {
                            output[i] = string.Format("TANGGUNGAN BPJS");
                        }
                    }

                    sbResult.Append(string.Join(",", output));
                    sbResult.Append("\r\n");

                    while (reader.Read())
                    {
                        reader.GetValues(output);

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            output[i] = output[i].ToString().Replace(',', '_');
                        }

                        sbResult.Append(string.Join(",", output));
                        sbResult.Append("\r\n");
                    }

                    reader.Close();
                    retval = sbResult.ToString();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            finally
            {
                sqlCon.Close();
            }
            return result;
        }

        public String getCNSetting()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["cnsetting"];
            string cnstring = settings.ConnectionString;
            string paramCNS = Encryption.DecryptString(cnstring);

            string[] paramCN = paramCNS.Split(';');
            string paramDec = paramCN[3];
            paramCN[3] = paramDec;

            string finalcnstring = "";
            for (int i = 0; i < paramCN.Length; i++)
            {
                if (String.IsNullOrEmpty(finalcnstring))
                {
                    finalcnstring = paramCN[i];
                }
                else
                {
                    finalcnstring += string.Format(";{0}", paramCN[i]);
                }
            }
            return finalcnstring;
        }
    }
}