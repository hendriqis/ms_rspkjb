using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BillPaymentSummaryTransactionNHSNew : DevExpress.XtraReports.UI.XtraReport
    {        

        public BillPaymentSummaryTransactionNHSNew()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtALLNHS> lst, String registrationID)
        {
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
            if (oHealthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
                cHealthcareName.Text = oHealthcare.HealthcareName;
                cHealthcareAddress.Text = oHealthcare.StreetName;
                cHealthcareCityZipCodes.Text = string.Format("{0} {1}", oHealthcare.City, oHealthcare.ZipCode);
                cHealthcarePhone.Text = oHealthcare.PhoneNo1;
                cHealthcareFax.Text = oHealthcare.FaxNo1;

                lblReportTitle.Text = "INVOICE";
                lblReportSubTitle.Text = "Transaction Summary";
            }

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(registrationID).FirstOrDefault();
            Registration entityRegLinked = BusinessLayer.GetRegistrationList(string.Format("LinkedRegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();
            Healthcare entityHealthcare = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);

            decimal vatPercent = Convert.ToDecimal(BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue);

            #region Header : Patient Detail
            cPatientName.Text = entityReg.PatientName;
            cDOB.Text = entityReg.DateOfBirthInString;
            cRegistrationNo.Text = entityReg.RegistrationNo;
            cRegistrationDate.Text = entityReg.RegistrationDateInString;
            cRegisteredPhysician.Text = entityReg.ParamedicName;

            if (entityReg.ReferrerParamedicID != null && entityReg.ReferrerParamedicID != 0)
            {
                cReferrerPhysician.Text = entityReg.ReferrerParamedicName;
            }
            else
            {
                if (entityReg.ReferrerID != null && entityReg.ReferrerID != 0)
                {
                    cReferrerPhysician.Text = entityReg.ReferrerName;
                }
                else
                {
                    cReferrerPhysician.Text = "";
                }
            }

            string filterPayment = string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}' AND GCPaymentType NOT IN ('{2}','{3}','{4}') ORDER BY PaymentID DESC", entityReg.RegistrationID, Constant.TransactionStatus.VOID, Constant.PaymentType.DOWN_PAYMENT, Constant.PaymentType.DEPOSIT_IN, Constant.PaymentType.DEPOSIT_OUT);
            //List<vPatientPaymentHd> lstPayment = BusinessLayer.GetvPatientPaymentHdList(filterPayment);
            vPatientPaymentHd entityPayment = BusinessLayer.GetvPatientPaymentHdList(filterPayment).FirstOrDefault();
            //if (entityReg.DepartmentID != Constant.Facility.INPATIENT)
            //{
            //    cInvoiceDate.Text = entityReg.RegistrationDateInString;
            //}
            //else
            //{
            //    if (entityReg.DischargeDate != null && entityReg.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
            //    {
            //        cInvoiceDate.Text = entityReg.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT);
            //    }
            //    else
            //    {
            //        cInvoiceDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
            //    }
            //}

            //if (lstPayment.Count > 0)
            if (entityPayment != null)
            {
                cInvoiceDate.Text = entityPayment.CreatedDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
            else
            {
                cInvoiceDate.Text = "";
            }

            String registrationNo = entityReg.RegistrationNo.Replace('/', '.');
            cInvoiceNo.Text = string.Format("INV.{0}.",registrationNo);

            if (entityReg.OldMedicalNo != "" && entityReg.OldMedicalNo != null)
            {
                cMedicalNo.Text = string.Format("{0} ({1})", entityReg.MedicalNo, entityReg.OldMedicalNo);
            }
            else
            {
                cMedicalNo.Text = string.Format("{0}", entityReg.MedicalNo);
            }

            //cServiceUnit.Text = string.Format("{0} | {1}", entityReg.ServiceUnitName, entityReg.ClassName);
            cServiceUnit.Text = entityReg.ServiceUnitName;
            cRoomBed.Text = string.Format("{0} | {1}", entityReg.RoomName, entityReg.BedCode);

            if (entityReg.CoverageTypeID != null && entityReg.CoverageTypeID != 0)
            {
                if (entityReg.CoverageTypeCode == "R001" && entityHealthcare.Initial == "NHS")
                {
                    cCorporate.Text = string.Format("{0} ({1})", entityReg.BusinessPartnerName, entityReg.CoverageTypeName);
                }
                else
                {
                    cCorporate.Text = string.Format("{0}", entityReg.BusinessPartnerName);
                }
            }
            else
            {
                cCorporate.Text = string.Format("{0}", entityReg.BusinessPartnerName);
            }

            string dischargeDateInfo = entityReg.cfDischargeDateInString;
            cDischargeDate.Text = dischargeDateInfo;

            if (entityRegLinked != null)
            {
                cDischargeDate.Text = string.Format("Transfer Rawat Inap ({0})", entityRegLinked.RegistrationNo);
            }
            #endregion

            #region Discount
            subDiscount.CanGrow = true;
            billPaymentDetailDiscountAll.InitializeReport(entityReg.RegistrationID);

            List<vPatientBillDiscount> lstBillDisc = BusinessLayer.GetvPatientBillDiscountList(string.Format(
                "(RegistrationID = {0} AND GCTransactionStatus != '{1}' AND IsDeleted = 0)", entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            decimal billDisc = lstBillDisc.Sum(a => (a.PatientDiscountAmount + a.PayerDiscountAmount));

            #endregion

            #region Hitung : JKP & BPK

            //decimal point1 = Convert.ToDecimal(lst.Where(z => z.GroupInvoice == "GROUP01"
            //                && (z.GCItemType != Constant.ItemType.OBAT_OBATAN 
            //                && z.GCItemType != Constant.ItemType.BARANG_MEDIS
            //                && z.GCItemType != Constant.ItemType.BARANG_UMUM
            //                && z.GCItemType != Constant.ItemType.BAHAN_MAKANAN)).Sum(a => a.LineAmount)) - billDisc;
            //decimal point2 = Convert.ToDecimal(point1 / ((100 + vatPercent) / 100));

            decimal point2 = Convert.ToDecimal(lst.Where(z => z.GroupInvoice == "GROUP01"
                            && (z.GCItemType != Constant.ItemType.OBAT_OBATAN
                            && z.GCItemType != Constant.ItemType.BARANG_MEDIS
                            && z.GCItemType != Constant.ItemType.BARANG_UMUM
                            && z.GCItemType != Constant.ItemType.BAHAN_MAKANAN)).Sum(a => a.LineAmount)) - billDisc;
            decimal point1 = Convert.ToDecimal(point2 + (point2 * (vatPercent/100)));
            decimal point3 = point1 - point2;

            cJKP1.Text = point1.ToString(Constant.FormatString.NUMERIC_2);
            cJKP1Total.Text = point1.ToString(Constant.FormatString.NUMERIC_2);

            cJKP2.Text = point2.ToString(Constant.FormatString.NUMERIC_2);
            cJKP2Total.Text = point2.ToString(Constant.FormatString.NUMERIC_2);

            cJKP3.Text = point3.ToString(Constant.FormatString.NUMERIC_2);
            cJKP3Total.Text = point3.ToString(Constant.FormatString.NUMERIC_2);

            //decimal point4 = Convert.ToDecimal(lst.Where(z => z.GroupInvoice == "GROUP01"
            //                && (z.GCItemType == Constant.ItemType.OBAT_OBATAN
            //                || z.GCItemType == Constant.ItemType.BARANG_MEDIS
            //                || z.GCItemType == Constant.ItemType.BARANG_UMUM
            //                || z.GCItemType == Constant.ItemType.BAHAN_MAKANAN)).Sum(a => a.LineAmount));
            //decimal point5 = Convert.ToDecimal(point4 / ((100 + vatPercent) / 100));

            decimal point5 = Convert.ToDecimal(lst.Where(z => z.GroupInvoice == "GROUP01"
                            && (z.GCItemType == Constant.ItemType.OBAT_OBATAN
                            || z.GCItemType == Constant.ItemType.BARANG_MEDIS
                            || z.GCItemType == Constant.ItemType.BARANG_UMUM
                            || z.GCItemType == Constant.ItemType.BAHAN_MAKANAN)).Sum(a => a.LineAmount));
            decimal point4 = Convert.ToDecimal(point5 + (point5 * (vatPercent / 100)));
            decimal point6 = point4 - point5;

            cBKP1.Text = point4.ToString(Constant.FormatString.NUMERIC_2);
            cBKP1Total.Text = point4.ToString(Constant.FormatString.NUMERIC_2);

            cBKP2.Text = point5.ToString(Constant.FormatString.NUMERIC_2);
            cBKP2Total.Text = point5.ToString(Constant.FormatString.NUMERIC_2);

            cBKP3.Text = point6.ToString(Constant.FormatString.NUMERIC_2);
            cBKP3Total.Text = point6.ToString(Constant.FormatString.NUMERIC_2);

            decimal point7 = Convert.ToDecimal(lst.Where(z => z.GroupInvoice == "GROUP02" && (z.GCItemType == Constant.ItemType.OBAT_OBATAN || z.GCItemType == Constant.ItemType.BARANG_MEDIS)).Sum(a => a.LineAmount));
            decimal point8 = Convert.ToDecimal(point7 / ((100 + vatPercent) / 100));
            decimal point9 = point7 - point8;

            cJKP1RJ.Text = point7.ToString(Constant.FormatString.NUMERIC_2);
            cJKP1TotalRJ.Text = point7.ToString(Constant.FormatString.NUMERIC_2);

            cJKP2RJ.Text = point8.ToString(Constant.FormatString.NUMERIC_2);
            cJKP2TotalRJ.Text = point8.ToString(Constant.FormatString.NUMERIC_2);

            cJKP3RJ.Text = point9.ToString(Constant.FormatString.NUMERIC_2);
            cJKP3TotalRJ.Text = point9.ToString(Constant.FormatString.NUMERIC_2);
            #endregion

            this.DataSource = lst;
        }

        private void lblServiceUnit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String IsChargeTransfered = Convert.ToString(GetCurrentColumnValue("IsChargeTransfered"));
            if (IsChargeTransfered == "0")
            {
                lblServiceUnit.Text = Convert.ToString(GetCurrentColumnValue("DepartmentID")) + " | " + Convert.ToString(GetCurrentColumnValue("ServiceUnitName"));
            }
            else
            {
                lblServiceUnit.Text = Convert.ToString(GetCurrentColumnValue("FromDepartmentID")) + " | " 
                                    + Convert.ToString(GetCurrentColumnValue("FromServiceUnitName")) + " | "
                                    + Convert.ToString(GetCurrentColumnValue("FromRegistrationNo"));
            }
        }

        private void cSubServiceUnit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String IsChargeTransfered = Convert.ToString(GetCurrentColumnValue("IsChargeTransfered"));
            if (IsChargeTransfered == "0")
            {
                cSubServiceUnit.Text = "Sub Total " + Convert.ToString(GetCurrentColumnValue("DepartmentID"));
            }
            else
            {
                cSubServiceUnit.Text = "Sub Total " + Convert.ToString(GetCurrentColumnValue("FromDepartmentID"));
            }
        }

        private void cItemGroupName_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String ItemCreatedDateInString = Convert.ToString(GetCurrentColumnValue("ItemCreatedDateInString"));
            if (ItemCreatedDateInString == "01-Jan-1900")
            {
                xrTableDetail.Visible = false;
            }
            else
            {
                xrTableDetail.Visible = true;
            }

            String ItemGroupID = Convert.ToString(GetCurrentColumnValue("ItemGroupID"));
            if (ItemGroupID == "0")
            {
                cItemGroupName.Text = Convert.ToString(GetCurrentColumnValue("ItemType"));
            }
            else
            {
                cItemGroupName.Text = string.Format("{0} | {1}", Convert.ToString(GetCurrentColumnValue("ItemType")), Convert.ToString(GetCurrentColumnValue("ItemGroupName1")));
            }

        }

        protected string ResolveUrl(string url)
        {
            return url.Replace("~", AppConfigManager.QISAppVirtualDirectory);
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String GroupInvoiceInString = Convert.ToString(GetCurrentColumnValue("GroupInvoice"));
            if (GroupInvoiceInString == "GROUP01")
            {
                cCatatanGroup1.Visible = true;
                cCatatanGroup2.Visible = false;

                subDiscount.Visible = true;
                lblDiskonHeader.Visible = true;

                xrRawatInap.Visible = true;
                xrRawatInapDetail.Visible = true;
                xrRawatJalan.Visible = false;
                xrRawatJalanDetail.Visible = false;

            }
            else
            {
                cCatatanGroup1.Visible = false;
                cCatatanGroup2.Visible = true;

                subDiscount.Visible = false;
                lblDiskonHeader.Visible = false;

                xrRawatInap.Visible = false;
                xrRawatInapDetail.Visible = false;
                xrRawatJalan.Visible = true;
                xrRawatJalanDetail.Visible = true;
            }
        }
    }
}
