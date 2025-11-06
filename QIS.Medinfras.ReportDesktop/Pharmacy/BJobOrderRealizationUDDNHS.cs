using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class BJobOrderRealizationUDDNHS : BaseCustomPharmacyA5Rpt
    {
       public BJobOrderRealizationUDDNHS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            GetPrescriptionOrderDtCustom entity = BusinessLayer.GetPrescriptionOrderDtCustomList(Convert.ToInt32(param[0])).FirstOrDefault();
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(String.Format("ParamedicID = {0}", entity.ParamedicID)).FirstOrDefault();
            vVitalSignDt entityHeight = BusinessLayer.GetvVitalSignDtList(String.Format("VisitID = {0} AND IsDeleted = 0 AND VitalSignID = 9", entity.VisitID)).LastOrDefault();
            vVitalSignDt entityWeight = BusinessLayer.GetvVitalSignDtList(String.Format("VisitID = {0} AND IsDeleted = 0 AND VitalSignID = 8", entity.VisitID)).LastOrDefault();

            #region Header
            cMedicalNo.Text = entity.MedicalNo;
            cPatientName.Text = entity.PatientName;
            cDOB.Text = string.Format("{0} ( {1} yr {2} mth {3} day )", entity.DateOfBirthInString, entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
            cCorporate.Text = entity.BusinessPartnerName;
            if (entityHeight == null && entityWeight == null)
                cHeightWeight.Text = "-";
            else if (entityHeight != null && entityWeight == null)
                cHeightWeight.Text = entityHeight.VitalSignValue + " / -";
            else if (entityHeight == null && entityWeight != null)
                cHeightWeight.Text = "- / " + entityWeight.VitalSignValue;
            else
                cHeightWeight.Text = entityHeight.VitalSignValue + " / " + entityWeight.VitalSignValue;

            cPrescriptionNo.Text = entity.TransactionNo;
            cOrderNo.Text = entity.PrescriptionOrderNo;
            cRegistrationNo.Text = entity.RegistrationNo;
            cRegisteredPhysician.Text = entity.ParamedicName;
            if (entityPM.LicenseNo == null)
                cPhysicianLicense.Text = "";
            else
                cPhysicianLicense.Text = entityPM.LicenseNo;
            #endregion

            #region Header : Per Page
            //cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
            //    entityCV.PatientName, entityCV.Gender, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            //cHeaderRegistration.Text = entityCV.RegistrationNo;
            //cHeaderMedicalNo.Text = entityCV.MedicalNo;
            #endregion

            #region Footer
            lblPrescriptionOrderNotes.Text = entity.Remarks;
            //lblOrderNotes.Text = entity.Remarks;
            //cTTDTakenBy.Text = entityCV.PatientName;
            //cTTDEtiketBy.Text = entityPresHD.ParamedicName;
            //cTTDCompoundBy.Text = entityPresHD.ParamedicName;
            //cTTDVerificationBy.Text = entityPresHD.ParamedicName;
            #endregion

            #region QR Codes Image
            string contents = string.Format(@"{0}", entity.RegistrationNo);

            QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
            qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qRCodeEncoder.QRCodeScale = 2;
            qRCodeEncoder.QRCodeVersion = 2;
            qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            MemoryStream memoryStream = new MemoryStream();
            //System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            //imgBarCode.Height = 400;
            //imgBarCode.Width = 400;

            using (Bitmap bitMap = qRCodeEncoder.Encode(contents, System.Text.Encoding.UTF8))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    //byte[] byteImage = ms.ToArray();
                    //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    lblQRCode.Image = System.Drawing.Image.FromStream(ms, true, true);
                }
            }
            #endregion

            //#region subReport
            //subReport.CanGrow = true;
            ////bJobOrderRealizationDt1.InitializeReport(entity);
            //#endregion

            base.InitializeReport(param);
        }

        private void cFlagCaption_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;
        }

        private void cItemName_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String ItemName = Convert.ToString(GetCurrentColumnValue("ItemName"));
                String CompoundQtyInString = Convert.ToString(GetCurrentColumnValue("CompoundQtyInString"));
                String CompoundUnit = Convert.ToString(GetCurrentColumnValue("CompoundUnit"));
                String ChargeQty = Convert.ToDecimal(GetCurrentColumnValue("ChargeQty")).ToString("G29");
                String ResultQty = Convert.ToDecimal(GetCurrentColumnValue("ResultQty")).ToString("G29");
                String CoenamRule = Convert.ToString(GetCurrentColumnValue("CoenamRule"));
                String SignaLabel = Convert.ToString(GetCurrentColumnValue("SignaLabel"));

                if (CompoundQtyInString == "")
                {
                    if (CoenamRule != "" && SignaLabel != "")
                    {
                        cItemName.Text = string.Format("{0} {1} {2} ({3} {4})", ItemName, ChargeQty, ResultQty, CoenamRule, SignaLabel);
                    }
                    else
                    {
                        cItemName.Text = string.Format("{0} {1} {2}", ItemName, ChargeQty, ResultQty);
                    }
                }
                else
                {
                    if (CoenamRule != "" && SignaLabel != "")
                    {
                        cItemName.Text = string.Format("{0} ({1} {2}) {3} {4} ({5} {6})",
                            ItemName, ChargeQty, CompoundQtyInString, CompoundUnit, ResultQty, CoenamRule, SignaLabel);
                    }
                    else
                    {
                        cItemName.Text = string.Format("{0} ({1} {2}) {3} {4}",
                            ItemName, ChargeQty, CompoundQtyInString, CompoundUnit, ResultQty);
                    }
                }
            }

        //private void cNumberOfDosage_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;
        //}

        private void cDosingUnit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;
        }

        private void cDispenseQty_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;

            if (Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == true && Convert.ToBoolean(GetCurrentColumnValue("IsCompound")) == true)
            {
                cDispenseQty.Text = GetCurrentColumnValue("DispenseQty").ToString();
            }
            else
            {
                if (Convert.ToBoolean(GetCurrentColumnValue("IsUsingUDD")) == true)
                {
                    cDispenseQty.Text = GetCurrentColumnValue("IsUsedUDDQty").ToString();
                }
                else
                {
                    cDispenseQty.Text = GetCurrentColumnValue("ChargedQuantity").ToString();
                }
            }
        }

        private void cTakenQty_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;

            if (Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == true && Convert.ToBoolean(GetCurrentColumnValue("IsCompound")) == true)
            {
                cTakenQty.Text = GetCurrentColumnValue("TakenQty").ToString();
            }
            else
            {
                cTakenQty.Text = GetCurrentColumnValue("ChargedQuantity").ToString();
            }
        }

        private void lblPrescriptionOrderNotesCaption_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = String.IsNullOrEmpty(lblPrescriptionOrderNotes.Text);
        }

        private void lblPrescriptionOrderNotes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = String.IsNullOrEmpty(lblPrescriptionOrderNotes.Text);
        }

        //private void cItemUnit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;

        //    if (Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == true && Convert.ToBoolean(GetCurrentColumnValue("IsCompound")) == true)
        //    {
        //        cItemUnit.Text = GetCurrentColumnValue("DosingUnit").ToString();
        //    }
        //    else
        //    {
        //        cItemUnit.Text = GetCurrentColumnValue("BaseUnit").ToString();
        //    }
        //}

        //private void xrTableCell66_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    e.Cancel = String.IsNullOrEmpty(lblOrderNotes.Text);
        //}

        //private void xrTable12_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    e.Cancel = String.IsNullOrEmpty(lblOrderNotes.Text);
        //}  
    }
}
