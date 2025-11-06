using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Common;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Data.Service.Helper;

namespace QIS.Medinfras.Data.Service
{
    #region Answer
    public partial class Answer
    {
        public int CfMargin
        {
            get
            {
                //if (GCNurseDomainClassType == Constant.StandardCode.NursingDomainClass.DOMAIN)
                if (_IsHasChild == 1)
                    return 0;
                else
                    return 2;
            }
        }
    }
    #endregion
    #region APIMessageLog
    public partial class APIMessageLog
    {
        public String cfMessageDateTimeInFullString
        {
            get
            {
                return _MessageDateTime.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }
        }
        public String cfMessageText
        {
            get
            {
                return Function.JsonHelper.FormatJson(_MessageText);
            }
        }
        public String cfResponse
        {
            get
            {
                return Function.JsonHelper.FormatJson(_Response);
            }
        }
    }
    #endregion
    #region ARInvoiceDtAmortization
    public partial class ARInvoiceDtAmortization
    {
        public String cfAmortizationDateInString
        {
            get { return _AmortizationDate.ToString(Constant.FormatString.DATE_FORMAT); }
        }
    }
    #endregion
    #region ARInvoiceDtAmortizationTemp
    public partial class ARInvoiceDtAmortizationTemp
    {
        public String cfAmortizationDateInString
        {
            get { return _AmortizationDate.ToString(Constant.FormatString.DATE_FORMAT); }
        }
    }
    #endregion
    #region ARInvoiceHd
    public partial class ARInvoiceHd
    {
        public String DueDateInString
        {
            get { return _DueDate.ToString(Constant.FormatString.DATE_FORMAT); }
        }

        public String DocumentDateInString
        {
            get { return _DocumentDate.ToString(Constant.FormatString.DATE_FORMAT); }
        }

        public String ARInvoiceDateInString
        {
            get { return _ARInvoiceDate.ToString(Constant.FormatString.DATE_FORMAT); }
        }

        public Decimal RemainingAmount
        {
            get { return (_TotalClaimedAmount - _TotalPaymentAmount); }
        }

        public String cfTotalClaimedAmountInString
        {
            get { return _TotalClaimedAmount.ToString(Constant.FormatString.NUMERIC_2); }
        }

        public String cfTotalClaimedAmountInStringInd
        {
            get
            {
                return Function.NumberInWords(Convert.ToInt64(_TotalClaimedAmount), true);
            }
        }

        public String cfTotalPaymentAmountInString
        {
            get { return _TotalPaymentAmount.ToString(Constant.FormatString.NUMERIC_2); }
        }

        public String cfRemainingAmountInString
        {
            get
            {
                decimal remainingAmount = (_TotalClaimedAmount - _TotalPaymentAmount);
                return remainingAmount.ToString(Constant.FormatString.NUMERIC_2);
            }
        }
    }
    #endregion
    #region ARReceivingHd
    public partial class ARReceivingHd
    {
        public Decimal cfReceiveAmount
        {
            get
            {
                if (_CashBackAmount < 0)
                {
                    return _TotalReceivingAmount;
                }
                return _TotalReceivingAmount - _CashBackAmount;
            }
        }
    }
    #endregion
    #region BankReconciliationHd
    public partial class BankReconciliationHd
    {
        public String cfBankReconciliationDateInString
        {
            get
            {
                return _BankReconciliationDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
        }
    }
    #endregion
    #region Bed
    public partial class Bed
    {
        public String BedCodeSuffix
        {
            get { return _GCBedStatus.Split('^')[1]; }
        }
    }
    #endregion
    #region BusinessPartners
    public partial class BusinessPartners
    {
        public String cfIsActiveInString
        {
            get
            {
                if (_IsActive)
                {
                    return "ACTIVE";
                }
                else
                {
                    return _NotActiveReason;
                }
            }
        }

        public String cfBusinessPartnerNameCode
        {
            get
            {
                if (_BusinessPartnerCode != "")
                {
                    return _BusinessPartnerName + " (" + _BusinessPartnerCode + ")";
                }
                else
                {
                    return _BusinessPartnerName;
                }
            }
        }
    }
    #endregion
    #region BridgingStatus
    public partial class BridgingStatus
    {
        public String cfMessageText
        {
            get
            {
                return Function.JsonHelper.FormatJson(_MessageText);
            }
        }
    }
    #endregion
    #region ChargesStatusLog
    public partial class ChargesStatusLog
    {
        public String LogDateInString
        {
            get { return _LogDate.ToString(Constant.FormatString.DATE_FORMAT); }
        }
    }
    #endregion
    #region ConsignmentOrderDt
    public partial class ConsignmentOrderDt
    {
        public Decimal CustomSubTotal
        {
            get
            {
                Decimal totalAfterDisc1 = (Quantity * UnitPrice) - ((Quantity * UnitPrice) *
               _DiscountPercentage1 / 100);
                Decimal totalAfterDisc2 = totalAfterDisc1 - (_DiscountPercentage2 / 100 * totalAfterDisc1);
                return totalAfterDisc2;
            }
        }
    }
    #endregion
    #region CustomerContract
    public partial class CustomerContract
    {
        public String StartDateInString
        {
            get { return _StartDate.ToString("dd-MMM-yyyy"); }
        }
        public String EndDateInString
        {
            get { return _EndDate.ToString("dd-MMM-yyyy"); }
        }
    }
    #endregion
    #region EKlaimParameter
    public partial class EKlaimParameter
    {
        public String cfCreatedDateInString
        {
            get
            {
                if (_CreatedDate != null && _CreatedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT) != "01-Jan-1900 00:00:00")
                {
                    return _CreatedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT);
                }
                else
                {
                    return "";
                }
            }
        }

        public String cfLastUpdatedDateInString
        {
            get
            {
                if (_LastUpdatedDate != null && _LastUpdatedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT) != "01-Jan-1900 00:00:00")
                {
                    return _LastUpdatedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT);
                }
                else
                {
                    return "";
                }
            }
        }
    }
    #endregion
    #region EmbalaceDt
    public partial class EmbalaceDt
    {
        public String cfTariffInString
        {
            get { return _Tariff.ToString(Constant.FormatString.NUMERIC_2); }
        }
    }
    #endregion
    #region FADepreciation
    public partial class FADepreciation
    {
        public String DepreciationDateInString
        {
            get { return _DepreciationDate.ToString(Constant.FormatString.DATE_FORMAT); }
        }
        public String DepreciationYear
        {
            get { return _PeriodNo.Substring(0, 4); }
        }
        public String DepreciationPeriodNo
        {
            get { return _PeriodNo.Substring(4, 2); }
        }
        public Decimal cfNilaiBukuAkhir
        {
            get { return (_AssetValue - _DepreciationAmount); }
        }
    }
    #endregion
    #region Guest
    public partial class Guest
    {
        public int AgeInYear
        {
            get
            {
                return Function.GetPatientAgeInYear(_DateOfBirth, DateTime.Now);
            }
        }
        public int AgeInMonth
        {
            get
            {
                return Function.GetPatientAgeInMonth(_DateOfBirth, DateTime.Now);
            }
        }
        public int AgeInDay
        {
            get
            {
                return Function.GetPatientAgeInDay(_DateOfBirth, DateTime.Now);
            }
        }
    }
    #endregion
    #region Holiday
    public partial class Holiday
    {
        public string DateInString
        {
            get
            {
                if (_IsAnnualHoliday)
                    return new DateTime(DateTime.Now.Year, _HolidayMonth, _HolidayDate).ToString(Helper.Constant.FormatString.DATE_FORMAT_5);
                else
                    return new DateTime((int)_HolidayYear, _HolidayMonth, _HolidayDate).ToString(Helper.Constant.FormatString.DATE_FORMAT);
            }
        }
    }
    #endregion
    #region ImagingResultHd
    public partial class ImagingResultHd
    {
        public Boolean cfIsVerified
        {
            get
            {
                return _GCTransactionStatus == Constant.StandardCode.TransactionStatus.PROCESSED;
            }
        }
    }
    #endregion
    #region INACBGrouper
    public partial class INACBGrouper
    {
        public String cfCreatedDateInString
        {
            get
            {
                if (_CreatedDate != null && _CreatedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT) != "01-Jan-1900 00:00:00")
                {
                    return _CreatedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT);
                }
                else
                {
                    return "";
                }
            }
        }

        public String cfLastUpdatedDateInString
        {
            get
            {
                if (_LastUpdatedDate != null && _LastUpdatedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT) != "01-Jan-1900 00:00:00")
                {
                    return _LastUpdatedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT);
                }
                else
                {
                    return "";
                }
            }
        }
    }
    #endregion
    #region ItemMaster
    public partial class ItemMaster
    {
        public string cfItemName
        {
            get
            {
                if (_OldItemCode != null && _OldItemCode != "")
                {
                    return string.Format("{0} - ({1}) ({2})*", _ItemName1, _ItemCode, _OldItemCode);
                }
                else
                {
                    return string.Format("{0} - ({1})", _ItemName1, _ItemCode);
                }
            }
        }
    }
    #endregion
    #region ItemBalanceDtExpired
    public partial class ItemBalanceDtExpired
    {
        public String ExpiredDateInString
        {
            get { return _ExpiredDate.ToString(Constant.FormatString.DATE_FORMAT); }
        }
        public string ExpiredDateInDatePickerFormat
        {
            get
            {
                return _ExpiredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
        }
    }
    #endregion
    #region ItemDistributionDtExpired
    public partial class ItemDistributionDtExpired
    {
        public string ExpiredDateInString
        {
            get
            {
                return _ExpiredDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
        }
        public string ExpiredDateInDatePickerFormat
        {
            get
            {
                return _ExpiredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
        }
    }
    #endregion
    #region ItemRequestHd
    public partial class ItemRequestHd
    {
        public string TransactionDateInString
        {
            get
            {
                return _TransactionDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
        }
    }
    #endregion
    #region LaboratoryResultHd
    public partial class LaboratoryResultHd
    {
        public Boolean cfIsVerified
        {
            get
            {
                return _GCTransactionStatus == Constant.StandardCode.TransactionStatus.PROCESSED;
            }
        }
    }
    #endregion
    #region MarginMarkupDt
    public partial class MarginMarkupDt
    {
        public String cfStartingValueInString
        {
            get
            {
                return _StartingValue.ToString(Constant.FormatString.NUMERIC_2);
            }
        }

        public String cfEndingValueInString
        {
            get
            {
                return _EndingValue.ToString(Constant.FormatString.NUMERIC_2);
            }
        }

        public String cfMarkupAmountInString
        {
            get
            {
                return _MarkupAmount.ToString(Constant.FormatString.NUMERIC_2);
            }
        }

        public String cfMarkupAmount2InString
        {
            get
            {
                return _MarkupAmount2.ToString(Constant.FormatString.NUMERIC_2);
            }
        }
    }
    #endregion
    #region MigrationConfigurationDt
    public partial class MigrationConfigurationDt
    {
        public String InputType
        {
            get
            {
                switch (_Type)
                {
                    case "1": return "Text Box";
                    case "2": return "Combo Box";
                    case "3": return "Check Box";
                    case "4": return "Date Edit";
                    default: return "Search Dialog";
                }
            }
        }
    }
    #endregion
    #region NursingItemGroupSubGroup
    public partial class NursingItemGroupSubGroup
    {
        public int CfMargin
        {
            get
            {
                if (IsHeader)
                    return 0;
                else
                    return 2;
            }
        }
    }
    #endregion
    #region OperationalTime
    public partial class OperationalTime
    {
        public string Time
        {
            get
            {
                string time = "";
                if (_StartTime1 != null && _StartTime1 != "")
                    time += ", " + _StartTime1 + " - " + _EndTime1;
                if (_StartTime2 != null && _StartTime2 != "")
                    time += ", " + _StartTime2 + " - " + _EndTime2;
                if (_StartTime3 != null && _StartTime3 != "")
                    time += ", " + _StartTime3 + " - " + _EndTime3;
                if (_StartTime4 != null && _StartTime4 != "")
                    time += ", " + _StartTime4 + " - " + _EndTime4;
                if (_StartTime5 != null && _StartTime5 != "")
                    time += ", " + _StartTime5 + " - " + _EndTime5;

                //Hapus , yang pertama
                if (time.Length > 2)
                    time = time.Remove(0, 2);
                return time;
            }
        }
    }
    #endregion
    #region ParamedicTaxBalance
    public partial class ParamedicTaxBalance
    {
        public Decimal cfTotalHonorSetelahPajak
        {
            get
            {
                return _TotalRevenueSharingAmount - _TaxAmount;
            }
        }

        public Decimal cfTotalHonorSetelahPajakRSPBT
        {
            get
            {
                return _TotalRevenueSharingAmount - _TaxAmount - _ExtraTaxAmount;
            }
        }

        public String cfIsHasExtraPPH
        {
            get
            {
                if (_ExtraTaxAmount != 0)
                {
                    return "+";
                }
                else
                {
                    return "";
                }
            }
        }

        public string cfMonth
        {
            get
            {
                string periodMonth = _PeriodNo.Substring(5, 2).ToString();
                return periodMonth;
            }
        }

        public String cfYear
        {
            get
            {
                string periodYear = _PeriodNo.Substring(1, 4).ToString();
                return periodYear;
            }
        }
    }
    #endregion
    #region Patient
    public partial class Patient
    {
        public string cfDOBInDatePicker
        {
            get
            {
                string dataDOB = _DateOfBirth.ToString("dd-MM-yyyy");
                if (dataDOB != "01-01-1900")
                {
                    return dataDOB;
                }
                else
                {
                    return "";
                }
            }
        }

        public string cfGenderInPL
        {
            get
            {
                if (_GCGender == "0003^M")
                {
                    return "L";
                }
                else
                {
                    return "P";
                }
            }
        }
    }
    #endregion
    #region PatientAllergy
    public partial class PatientAllergy
    {
        public string cfKnownDateInMonthText
        {
            get
            {
                if (_KnownDate != null && _KnownDate != "")
                {
                    String result = "";
                    if (_KnownDate.Substring(4, 2) == "01")
                    {
                        result = "Januari";
                    }
                    else if (_KnownDate.Substring(4, 2) == "02")
                    {
                        result = "Februari";
                    }
                    else if (_KnownDate.Substring(4, 2) == "03")
                    {
                        result = "Maret";
                    }
                    else if (_KnownDate.Substring(4, 2) == "04")
                    {
                        result = "April";
                    }
                    else if (_KnownDate.Substring(4, 2) == "05")
                    {
                        result = "Mei";
                    }
                    else if (_KnownDate.Substring(4, 2) == "06")
                    {
                        result = "Juni";
                    }
                    else if (_KnownDate.Substring(4, 2) == "07")
                    {
                        result = "Juli";
                    }
                    else if (_KnownDate.Substring(4, 2) == "08")
                    {
                        result = "Agustus";
                    }
                    else if (_KnownDate.Substring(4, 2) == "09")
                    {
                        result = "September";
                    }
                    else if (_KnownDate.Substring(4, 2) == "10")
                    {
                        result = "Oktober";
                    }
                    else if (_KnownDate.Substring(4, 2) == "11")
                    {
                        result = "November";
                    }
                    else if (_KnownDate.Substring(4, 2) == "12")
                    {
                        result = "Desember";
                    }
                    return result;
                }
                return "";
            }
        }
    }
    #endregion
    #region PatientBill
    public partial class PatientBill
    {
        public string BillingDateInString
        {
            get
            {
                return _BillingDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
        }
        public Decimal PatientRemainingAmount
        {
            get
            {
                return _TotalPatientAmount - _TotalPatientPaymentAmount - _PatientDiscountAmount;
            }
        }
        public Decimal PayerRemainingAmount
        {
            get
            {
                return _TotalPayerAmount - _TotalPayerPaymentAmount - _PayerDiscountAmount;
            }
        }
        public Decimal PatientBillAmount
        {
            get
            {
                return _TotalPatientAmount - _PatientDiscountAmount;
            }
        }
        public Decimal PayerBillAmount
        {
            get
            {
                return _TotalPayerAmount - _PayerDiscountAmount;
            }
        }
        public Decimal TotalBillAmount
        {
            get
            {
                return PatientBillAmount + PayerBillAmount;
            }
        }
        public Decimal RemainingAmount
        {
            get
            {
                return _TotalAmount - (_TotalPatientPaymentAmount + _TotalPayerPaymentAmount) - (_PatientDiscountAmount + _PayerDiscountAmount);
            }
        }
    }
    #endregion
    #region PatientChargesHd
    public partial class PatientChargesHd
    {
        public string TransactionDateInString
        {
            get
            {
                return _TransactionDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
        }
        public string TransactionDateInDatePickerFormat
        {
            get
            {
                return _TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
        }
        public string cfTransactionDateInString
        {
            get
            {
                return _TransactionDate.ToString(Constant.FormatString.DATE_FORMAT_2);
            }
        }
    }
    #endregion
    #region PatientPaymentDtVirtual
    public partial class PatientPaymentDtVirtual
    {

        public string cfPaymentStatus
        {
            get
            {
                DateTime DatetimeNow = DateTime.Now;
                string Status = string.Empty;
                if (DatetimeNow >= _ExpiredDateTime && _PaymentID == 0)
                {
                    Status = "EXPIRED";
                }
                else if (_PaymentID == 0 && DatetimeNow >= _ExpiredDateTime)
                {
                    Status = "PENDING";
                }
                else if (_PaymentID > 0)
                {
                    Status = "SUCCESS";
                }
                return Status;
            }
        }

    }
    #endregion
    #region PatientReferral
    public partial class PatientReferral
    {
        public string ReferralDateinString
        {
            get
            {
                return _ReferralDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
        }
    }
    #endregion
    #region PrescriptionOrderHd
    public partial class PrescriptionOrderHd
    {
        public String cfSendOrderDateTime
        {
            get
            {
                if (_SendOrderDateTime != null && Convert.ToDateTime(_SendOrderDateTime).ToString(Constant.FormatString.DATE_TIME_FORMAT) != "01-Jan-1900 00:00:00")
                {
                    return Convert.ToDateTime(_SendOrderDateTime).ToString(Constant.FormatString.DATE_TIME_FORMAT);
                }
                else
                {
                    return "";
                }
            }
        }
    }
    #endregion
    #region PurchaseInvoiceHd
    public partial class PurchaseInvoiceHd
    {
        public Decimal CustomTotalHutang
        {
            get
            {
                //masih belum dihitung PPH
                Decimal total1 = (_TotalTransactionAmount - (_TotalDownPaymentAmount + TotalCreditNoteAmount + _FinalDiscountAmount));
                Decimal total2 = total1 * ((100 + _VATPercentage) / 100);
                Decimal total = total2 - _PPHAmount - _StampAmount - _ChargesAmount;
                return total;
            }
        }
        public Decimal CustomSisaHutang
        {
            get
            {
                Decimal sisa = CustomTotalHutang - _PaymentAmount;
                return sisa;
            }
        }
        public int CustomUmur
        {
            get
            {
                return Function.GetPatientAgeInDay(_DueDate, DateTime.Today);
            }
        }
    }
    #endregion
    #region PurchaseOrderDt
    public partial class PurchaseOrderDt
    {
        public Decimal CustomSubTotal
        {
            get
            {
                Decimal totalAfterDisc1 = (_Quantity * _UnitPrice) - ((_Quantity * _UnitPrice) * _DiscountPercentage1 / 100);
                Decimal totalAfterDisc2 = totalAfterDisc1 - (_DiscountPercentage2 / 100 * totalAfterDisc1);
                return totalAfterDisc2;
            }
        }

        public Decimal CustomSubTotal2
        {
            get
            {
                Decimal totalAfterDisc1 = 0;
                Decimal totalAfterDisc2 = 0;

                if (_IsDiscountInPercentage1)
                {
                    totalAfterDisc1 = (_Quantity * _UnitPrice) - ((_Quantity * _UnitPrice) * _DiscountPercentage1 / 100);
                }
                else
                {
                    totalAfterDisc1 = (_Quantity * _UnitPrice) - _DiscountAmount1;
                }

                if (_IsDiscountInPercentage2)
                {
                    totalAfterDisc2 = totalAfterDisc1 - (_DiscountPercentage2 / 100 * totalAfterDisc1);
                }
                else
                {
                    totalAfterDisc2 = totalAfterDisc1 - _DiscountAmount2;
                }

                return totalAfterDisc2;
            }
        }
    }
    #endregion
    #region PurchaseReceiveDt
    public partial class PurchaseReceiveDt
    {
        public Decimal CustomSubTotal
        {
            get
            {
                Decimal totalAfterDisc1 = (_Quantity * _UnitPrice) - ((_Quantity * _UnitPrice) * _DiscountPercentage1 / 100);
                Decimal totalAfterDisc2 = totalAfterDisc1 - (_DiscountPercentage2 / 100 * totalAfterDisc1);
                return totalAfterDisc2;
            }
        }

        public Decimal CustomSubTotal2
        {
            get
            {
                Decimal totalAfterDisc1 = 0;
                Decimal totalAfterDisc2 = 0;

                if (_IsDiscountInPercentage1)
                {
                    totalAfterDisc1 = (_Quantity * _UnitPrice) - ((_Quantity * _UnitPrice) * _DiscountPercentage1 / 100);
                }
                else
                {
                    totalAfterDisc1 = (_Quantity * _UnitPrice) - _DiscountAmount1;
                }

                if (_IsDiscountInPercentage2)
                {
                    totalAfterDisc2 = totalAfterDisc1 - (_DiscountPercentage2 / 100 * totalAfterDisc1);
                }
                else
                {
                    totalAfterDisc2 = totalAfterDisc1 - _DiscountAmount2;
                }

                return totalAfterDisc2;
            }
        }
    }
    #endregion
    #region PurchaseReceiveDtExpired
    public partial class PurchaseReceiveDtExpired
    {
        public string QuantityInString
        {
            get
            {
                return _Quantity.ToString(Constant.FormatString.NUMERIC_2);
            }
        }
        public string ExpiredDateInString
        {
            get
            {
                return _ExpiredDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
        }
        public string ExpiredDateInDatePickerFormat
        {
            get
            {
                return _ExpiredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
        }
    }
    #endregion
    #region PurchaseRequestHd
    public partial class PurchaseRequestHd
    {
        public string TransactionDateInString
        {
            get
            {
                return _TransactionDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
        }
    }
    #endregion
    #region Procedures
    public partial class Procedures
    {
        public string cfBpjsCode
        {
            get
            {
                string[] BpjsReference = _BPJSReferenceInfo.Split('|', ';');

                if (BpjsReference.Length > 0)
                {
                    return BpjsReference[0];
                }
                else
                {
                    return "";
                }
            }
        }

        public string cfBpjsName
        {
            get
            {
                string[] BpjsReference = _BPJSReferenceInfo.Split('|', ';');

                if (BpjsReference.Length >= 2)
                {
                    return BpjsReference[1];
                }
                else
                {
                    return "";
                }
            }
        }
    }
    #endregion
    #region Question
    public partial class Question
    {
        public int CfMargin
        {
            get
            {
                //if (GCNurseDomainClassType == Constant.StandardCode.NursingDomainClass.DOMAIN)
                if (_IsHeader == 1)
                    return 0;
                else
                    return 2;
            }
        }
    }
    #endregion
    #region RevenueSharingDt
    public partial class RevenueSharingDt
    {
        public string DisplayAmount
        {
            get
            {
                if (_Amount == 0)
                    return "-";
                if (_IsPercentage)
                    return String.Format("{0:n0}%", _Amount);
                return String.Format("{0:n}", _Amount);
            }
        }
    }
    #endregion
    #region RevenueSharingHd
    public partial class RevenueSharingHd
    {
        public string DisplaySharingAmount
        {
            get
            {
                if (_SharingAmount == 0)
                    return "-";
                if (_IsSharingInPercentage)
                    return String.Format("{0:n0}%", _SharingAmount);
                return String.Format("{0:n}", _SharingAmount);
            }
        }

        public string DisplaySharingAmountWithoutRounding
        {
            get
            {
                if (_SharingAmount == 0)
                    return "-";
                if (_IsSharingInPercentage)
                    return String.Format("{0:n}%", _SharingAmount);
                return String.Format("{0:n}", _SharingAmount);
            }
        }

        public string DisplayCreditCard
        {
            get
            {
                if (_CreditCardFeeAmount == 0)
                    return "-";
                if (_IsCreditCardFeeInPercentage)
                    return String.Format("{0:n0}%", _CreditCardFeeAmount);
                return String.Format("{0:n}", _CreditCardFeeAmount);
            }
        }
    }
    #endregion
    #region RevenueSharingOperationalTime
    public partial class RevenueSharingOperationalTime
    {
        public string cfDayNameInEnglish
        {
            get
            {
                switch (_DayNumber)
                {
                    case 1: return "Monday";
                    case 2: return "Tuesday";
                    case 3: return "Wednesday";
                    case 4: return "Thursday";
                    case 5: return "Friday";
                    case 6: return "Saturday";
                    case 7: return "Sunday";
                    default: return "";
                }
            }
        }

        public string cfDayNameInIndonesian
        {
            get
            {
                switch (_DayNumber)
                {
                    case 1: return "Senin";
                    case 2: return "Selasa";
                    case 3: return "Rabu";
                    case 4: return "Kamis";
                    case 5: return "Jumat";
                    case 6: return "Sabtu";
                    case 7: return "Minggu";
                    default: return "";
                }
            }
        }
    }
    #endregion
    #region StandardCode
    public partial class StandardCode
    {
        public String cfStandardCodeID
        {
            get
            {
                if (!_IsHeader)
                    return _StandardCodeID.Split('^')[1];
                return _StandardCodeID;
            }
        }
        public String cfIsMapping
        {
            get
            {
                if (_IsMapping)
                    return "V";
                return "X";
            }
        }
    }
    #endregion
    #region SatuSehatIntegrationLog
    public partial class SatuSehatIntegrationLog
    {
        public String SendDateTimeInString
        {
            get
            {
                return string.Format("{0} {1}", _SendDateTime.ToString(Constant.FormatString.DATE_FORMAT), _SendDateTime.ToString(Constant.FormatString.TIME_FORMAT_2));
            }
        }

        public String ResponseDateTimeInString
        {
            get
            {
                return string.Format("{0} {1}", _ResponseDateTime.ToString(Constant.FormatString.DATE_FORMAT), _ResponseDateTime.ToString(Constant.FormatString.TIME_FORMAT_2));
            }
        }

        public String cfMessageText
        {
            get
            {
                return Function.JsonHelper.FormatJson(_MessageText);
            }
        }
        public String cfResponseText
        {
            get
            {
                return Function.JsonHelper.FormatJson(_ResponseRemarks);
            }
        }
    }
    #endregion
    #region StockTakingDtExpired
    public partial class StockTakingDtExpired
    {
        public String ExpiredDateInString
        {
            get { return _ExpiredDate.ToString(Constant.FormatString.DATE_FORMAT); }
        }
        public String ExpiredDateInDatePickerFormat
        {
            get { return _ExpiredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT); }
        }
    }
    #endregion
    #region SupplierItem
    public partial class SupplierItem
    {
        public String cfSupplierItem
        {
            get
            {
                if (_SupplierItemName != "" && _SupplierItemCode != "")
                    return string.Format("{0} ({1})", _SupplierItemName, _SupplierItemCode);
                if (_SupplierItemName != "")
                    return _SupplierItemName;
                return _SupplierItemCode;
            }
        }
    }
    #endregion
    #region TariffBookDtTemp
    public partial class TariffBookDtTemp
    {
        public String cfProposedTariff
        {
            get
            {
                return _ProposedTariff.ToString(Constant.FormatString.NUMERIC_2);
            }
        }

        public String cfProposedTariffComp1
        {
            get
            {
                return _ProposedTariffComp1.ToString(Constant.FormatString.NUMERIC_2);
            }
        }

        public String cfProposedTariffComp2
        {
            get
            {
                return _ProposedTariffComp2.ToString(Constant.FormatString.NUMERIC_2);
            }
        }

        public String cfProposedTariffComp3
        {
            get
            {
                return _ProposedTariffComp3.ToString(Constant.FormatString.NUMERIC_2);
            }
        }

        public String cfCreatedDateInString
        {
            get
            {
                return _CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }
        }
    }
    #endregion
    #region TaxRevenueDt
    public partial class TaxRevenueDt
    {
        public String cfStartingValueInString
        {
            get
            {
                return _StartingValue.ToString(Constant.FormatString.NUMERIC_2);
            }
        }

        public String cfEndingValueInString
        {
            get
            {
                return _EndingValue.ToString(Constant.FormatString.NUMERIC_2);
            }
        }

        public String cfTaxAmountInString
        {
            get
            {
                return _TaxAmount.ToString(Constant.FormatString.NUMERIC_2);
            }
        }
    }
    #endregion
    #region TransRevenueSharingAdjustmentHd
    public partial class TransRevenueSharingAdjustmentHd
    {
        public string cfRSAdjustmentDateInStringDateFormat
        {
            get
            {
                return _RSAdjustmentDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
        }

        public string cfTotalAdjustmentAmountInString
        {
            get
            {
                return _TotalAdjustmentAmount.ToString(Constant.FormatString.NUMERIC_2);
            }
        }
    }
    #endregion
    #region TransRevenueSharingHd
    public partial class TransRevenueSharingHd
    {
        public String ProcessedDateInString
        {
            get
            {
                return _ProcessedDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
        }
    }
    #endregion

    #region NursingProblem
    public partial class NursingProblem : DbDataModel
    {
        public String cfProblemName
        {
            get
            {
                return string.Format("{0} ({1})", _ProblemName, _ProblemCode);
            }
        }
    }
    #endregion
}
