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
    public partial class ResepAsli_UKRIDA : BaseCustomPharmacyA6Rpt

    {
        public ResepAsli_UKRIDA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            GetPrescriptionOrderDtCustom3 entity = BusinessLayer.GetPrescriptionOrderDtCustom3List(Convert.ToInt32(param[0])).FirstOrDefault();

            if (entity != null)
            {
                cPrescriber.Text = string.Format("{0}", entity.PrescriptionParamedicName);
                cRegistrationNo.Text = string.Format("{0} | {1} {2}", entity.RegistrationNo, entity.ActualVisitDateInString, entity.ActualVisitTime);
                cPatient.Text = string.Format("{0} | {1} | {2}", entity.PatientName, entity.MedicalNo, entity.Gender);
                cDOB.Text = string.Format("{0} ( {1} yr {2} mnth {3} day )", entity.DateOfBirthInString, entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
                lblTTD.Text = entity.City + ", " + DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
                cPenjamin.Text = entity.BusinessPartnerName;
                ttdName.Text = entity.PrescriptionParamedicName;
                if (entity.GCPrescriptionParamedicMasterType == Constant.ParamedicType.Physician)
                {
                    if (entity.PrescriptionLicenseNo == null || entity.PrescriptionLicenseNo == "")
                        cLicenseNo.Text = "-";
                    else
                        cLicenseNo.Text = entity.PrescriptionLicenseNo;
                }
                else if (entity.GCPrescriptionParamedicMasterType == Constant.ParamedicType.Bidan)
                {
                    if (entity.PrescriptionLicenseNo == null || entity.PrescriptionLicenseNo == "")
                    {
                        cSIPNo.Text = "SIP No";
                        cLicenseNo.Text = "-";
                    }
                    else
                    {
                        cSIPNo.Text = "SIP No";
                        cLicenseNo.Text = entity.PrescriptionLicenseNo;
                    }
                }
                else
                {
                    xrTableRow20.Visible = false;
                }

                lblBarcode.Visible = false;
                PrescriptionOrderHd oPhd = BusinessLayer.GetPrescriptionOrderHdList(string.Format("PrescriptionOrderID='{0}'", entity.PrescriptionOrderID)).FirstOrDefault();
                if (oPhd != null)
                {
                    string[] odata = oPhd.ReferenceNo.Split('|');
                    if (odata.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(odata[1]))
                        {
                            lblBarcode.Visible = true;
                            lblBarcode.Text = odata[1];
                        }

                    }
                }

                cServisUnitName.Text = string.Format("{0} | {1} {2}", entity.ServiceUnitName, entity.cfSendDateInString, entity.cfSentTimeInString);
                cPrescription.Text = entity.JenisResep;
                cPrescriptionNo.Text = entity.TransactionNo;
                string Picture = string.Format("{0}{1}{2}.png", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISParamedicSignaturePath, entity.PrescriptionParamedicCode);

                ttdParamedic.ImageUrl = Picture;
            }

            base.InitializeReport(param);
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean IsCompound = Convert.ToBoolean(GetCurrentColumnValue("IsCompound"));
            if (!IsCompound) e.Cancel = true;
        }

        //private void lblSignaLabel_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    string consumeMethod = "";
        //    switch (GetCurrentColumnValue("GCDosingFrequency").ToString())
        //    {
        //        case Constant.DosingFrequency.DAY:
        //            consumeMethod = string.Format("{0} x sehari {1} {2} {3}", Convert.ToDecimal(GetCurrentColumnValue("Frequency")).ToString("G29"), Convert.ToDecimal(GetCurrentColumnValue("NumberOfDosage")).ToString("G29"), GetCurrentColumnValue("DosingUnit"), GetCurrentColumnValue("CoenamRule"));
        //            break;
        //        case Constant.DosingFrequency.WEEK:
        //            consumeMethod = string.Format("{0} x seminggu {1} {2} {3}", Convert.ToDecimal(GetCurrentColumnValue("Frequency")).ToString("G29"), Convert.ToDecimal(GetCurrentColumnValue("NumberOfDosage")).ToString("G29"), GetCurrentColumnValue("DosingUnit"), GetCurrentColumnValue("CoenamRule"));
        //            break;
        //        case Constant.DosingFrequency.HOUR:
        //            consumeMethod = string.Format("setiap {0} jam {1} {2} {3}", Convert.ToDecimal(GetCurrentColumnValue("Frequency")).ToString("G29"), Convert.ToDecimal(GetCurrentColumnValue("NumberOfDosage")).ToString("G29"), GetCurrentColumnValue("DosingUnit"), GetCurrentColumnValue("CoenamRule"));
        //            break;
        //        default:
        //            break;
        //    }
        //    lblSignaLabel.Text = consumeMethod;
        //}

        private void lblMFLA_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String DosingUnit = Convert.ToString(GetCurrentColumnValue("DosingUnit"));
            String DispenseQty = Convert.ToString(GetCurrentColumnValue("DispenseQty"));

            Boolean IsCompound = Convert.ToBoolean(GetCurrentColumnValue("IsCompound"));
            if (IsCompound)
            {
                lblMFLA.Text = string.Format("mf la {0} dtd No. {1}", DosingUnit, DispenseQty);
            }
            else
            {
                lblMFLA.Text = "";
            }
        }

        //private void lbldet_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    Decimal TakenQty = Convert.ToDecimal(GetCurrentColumnValue("TakenQty"));

        //    if (TakenQty > 0)
        //    {
        //        lbldet.Text = string.Format("det {0}", TakenQty.ToString("G29"));
        //    }
        //    else
        //    {
        //        lbldet.Text = "nedet";
        //    }
        //}

    }
}
