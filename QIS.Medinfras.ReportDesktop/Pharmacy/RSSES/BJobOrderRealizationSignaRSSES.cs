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
    public partial class BJobOrderRealizationSignaRSSES : BaseDailyPortraitRpt
    {
        public BJobOrderRealizationSignaRSSES()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            GetPrescriptionOrderDtMedicalPrescription entity = BusinessLayer.GetPrescriptionOrderDtMedicalPrescriptionList(Convert.ToInt32(param[0])).FirstOrDefault();
            Registration entityReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
            PrescriptionOrderHd entityOrderHd = BusinessLayer.GetPrescriptionOrderHdList(string.Format("PrescriptionOrderID = {0}", entity.PrescriptionOrderID)).FirstOrDefault();
            PatientChargesHd entityChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PrescriptionOrderID = {0}", entity.PrescriptionOrderID)).FirstOrDefault();
            PharmacyJournal entityPharmacyJournal = BusinessLayer.GetPharmacyJournalList(string.Format("TransactionID = {0} ORDER BY ID DESC", entityChargesHd.TransactionID)).FirstOrDefault();
            
            if (entityReg.MRN != null && entityReg.MRN != 0)
            {
                List<PatientAllergy> entityAllergyLst = BusinessLayer.GetPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0 AND GCAllergenType = '{1}'", entityReg.MRN, Constant.AllergenType.DRUG));

                if (entityAllergyLst.Count() > 0)
                {
                    cAlergi.Text = entityAllergyLst.FirstOrDefault().Allergen;
                }
                else
                {
                    cAlergi.Text = "Tidak Ada";
                }

                Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityReg.MRN)).FirstOrDefault();
                Address entityAddress = BusinessLayer.GetAddressList(string.Format("AddressID = {0}", entityPatient.HomeAddressID)).FirstOrDefault();
                String PhoneNo = "";

                cPatientName.Text = string.Format("{0} {1} {2}", entity.Salution, entity.PatientName, entity.MedicalNo);
                cDateOfBirth.Text = string.Format("{0} ( {1} thn {2} bln {3} hr )", entity.DateOfBirthInString, entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
                cCorporate.Text = entity.BusinessPartnerName;
                cStreetName.Text = entityAddress.StreetName;
                cPrescriptionNo.Text = entity.TransactionNo;
                cOrderNo.Text = entity.PrescriptionOrderNo;
                if (entityReg.AppointmentID != null)
                {
                    cRegistrationNo.Text = entity.cfRegistrationNo;
                }
                else
                {
                    cRegistrationNo.Text = entity.RegistrationNo;
                }
                cReferenceNo.Text = entity.ReferenceNo;
                cRegisteredPhysician.Text = entity.ParamedicName;
                cServiceUnitName.Text = entity.ServiceUnitName;
                if (entityOrderHd.SendOrderDateTime.ToString() != "" && entityOrderHd.SendOrderDateTime.ToString() != null)
                {
                    cStartTime.Text = entityOrderHd.cfSendOrderDateTime;
                }
                else
                {
                    cStartTime.Text = "";
                }
                lblAntrian.Text = entity.ReferenceNo;
                //if (entityChargesHd.QueueNoLabel != "")
                //{
                //    if (entityChargesHd.QueueNoLabel.Contains(','))
                //    {
                //        string[] queueNo = entityChargesHd.QueueNoLabel.Split(',');
                //        lblAntrian.Text = queueNo[2];
                //        if (entityChargesHd.QueueNoLabel != null)
                //        {
                //            lblBarCode.Text = entityChargesHd.QueueNoLabel;
                //        }
                //        else
                //        {
                //            lblBarCode.Visible = false;
                //        }
                //    }
                //    else
                //    {
                //        lblBarCode.Text = string.Empty;
                //        lblBarCode.Visible = false;
                //    }
                //}
                //else
                //{
                //    lblAntrian.Text = "";
                //    lblBarCode.Visible = false;
                //}

                #region QR Codes Image
                string contents = string.Format(@"{0},{1},{2},{3},{4},{5}",
                    entity.ReferenceNoPrefix, entity.TransactionDateInString, entity.QueueNo, entity.MedicalNo, entity.RegistrationNo, entity.AppointmentNo);

                QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
                qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qRCodeEncoder.QRCodeScale = 4;
                qRCodeEncoder.QRCodeVersion = 7;
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
                        pictMRNQR.Image = System.Drawing.Image.FromStream(ms, true, true);
                    }
                }
                #endregion

                if (entityPatient.MobilePhoneNo1 != "")
                {
                    PhoneNo = entityPatient.MobilePhoneNo1;
                }
                else
                {
                    PhoneNo = "-";
                }
                cPhoneNo.Text = PhoneNo;
                lblPrescriptionOrderNotes.Text = entity.Remarks;
                if (entity.IsHasDrugInteraction == false)
                    lblIsHasDrugInteraction.Text = "X";
                else lblIsHasDrugInteraction.Text = "V";
                if (entity.IsHasDuplication == false)
                    lblIsHasDuplication.Text = "X";
                else lblIsHasDrugInteraction.Text = "V";

                //List<PatientAllergy> entityAllergy = BusinessLayer.GetPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0 AND GCAllergenType = '{1}'", entityReg.MRN, Constant.AllergenType.DRUG));
                //if (entityAllergy.Count > 0)
                //{
                //    cAlergi.Text = entityAllergy.LastOrDefault().Allergen;
                //}

                //List<PharmacyJournal> entityPharmacyJournal = BusinessLayer.GetPharmacyJournalList(string.Format("TransactionID = {0} ORDER BY ID DESC", entityChargesHd.TransactionID));
                //if (entityPharmacyJournal.Count > 0)
                //{
                //    lblCF.Text = entityPharmacyJournal.FirstOrDefault().JournalText;
                //}
                //else
                //{
                //    lblCF.Text = "";
                //}

                if (entityPharmacyJournal != null)
                {
                    lblCF.Text = entityPharmacyJournal.JournalText;
                }
                else
                {
                    lblCF.Text = "";
                }

                List<PrescriptionOrderChangesLog> entityOrderChanges = BusinessLayer.GetPrescriptionOrderChangesLogList(string.Format("PrescriptionOrderID = {0} ORDER BY ID DESC", entity.PrescriptionOrderID));
                if (entityOrderChanges.Count > 0)
                {
                    lblCPO.Text = entityOrderChanges.FirstOrDefault().NoteText;
                }
                else
                {
                    lblCPO.Text = "";
                }
            }
            else
            {
                Guest entityPatient = BusinessLayer.GetGuestList(string.Format("GuestID = {0}", entityReg.GuestID)).FirstOrDefault();
                String PhoneNo = "";

                cPatientName.Text = string.Format("{0} {1}", entity.PatientName, entity.MedicalNo);
                cDateOfBirth.Text = string.Format("{0} ( {1} thn {2} bln {3} hr )", entity.DateOfBirthInString, entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
                cCorporate.Text = entity.BusinessPartnerName;
                cStreetName.Text = entityPatient.StreetName;
                cPrescriptionNo.Text = entity.TransactionNo;
                cOrderNo.Text = entity.PrescriptionOrderNo;
                if (entityReg.AppointmentID != null)
                {
                    cRegistrationNo.Text = entity.cfRegistrationNo;
                }
                else
                {
                    cRegistrationNo.Text = entity.RegistrationNo;
                }
                cReferenceNo.Text = entity.ReferenceNo;
                cRegisteredPhysician.Text = entity.ParamedicName;
                cServiceUnitName.Text = entity.ServiceUnitName;
                lblAntrian.Text = entity.QueueNo;
                if (entityPatient.MobilePhoneNo != "")
                {
                    PhoneNo = entityPatient.MobilePhoneNo;
                }
                else
                {
                    PhoneNo = "-";
                }
                cPhoneNo.Text = PhoneNo;
                lblPrescriptionOrderNotes.Text = entity.Remarks;
                if (entity.IsHasDrugInteraction == false)
                    lblIsHasDrugInteraction.Text = "X";
                else lblIsHasDrugInteraction.Text = "V";
                if (entity.IsHasDuplication == false)
                    lblIsHasDuplication.Text = "X";
                else lblIsHasDrugInteraction.Text = "V";

                #region QR Codes Image
                string contents = string.Format(@"{0},{1},{2},{3},{4},{5}",
                    entity.ReferenceNoPrefix, entity.TransactionDateInString, entity.QueueNo, entity.MedicalNo, entity.RegistrationNo, entity.AppointmentNo);

                QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
                qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qRCodeEncoder.QRCodeScale = 4;
                qRCodeEncoder.QRCodeVersion = 7;
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
                        pictMRNQR.Image = System.Drawing.Image.FromStream(ms, true, true);
                    }
                }
                #endregion

                //if (entityChargesHd.QueueNoLabel != "")
                //{
                //    if (entityChargesHd.QueueNoLabel.Contains(','))
                //    {
                //        string[] queueNo = entityChargesHd.QueueNoLabel.Split(',');
                //        lblAntrian.Text = queueNo[2];
                //        if (entityChargesHd.QueueNoLabel != null)
                //        {
                //            lblBarCode.Text = entityChargesHd.QueueNoLabel;
                //        }
                //        else
                //        {
                //            lblBarCode.Visible = false;
                //        }
                //    }
                //    else
                //    {
                //        lblBarCode.Text = string.Empty;
                //        lblBarCode.Visible = false;
                //    }
                //}
                //else
                //{
                //    lblAntrian.Text = "";
                //    lblBarCode.Visible = false;
                //}
            }
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
                    cItemName.Text = string.Format("{0} ({1} {2})", ItemName, CoenamRule, SignaLabel);
                }
                else
                {
                    cItemName.Text = string.Format("{0}", ItemName);
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

        private void cNumberOfDosage_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;
        }

        private void cDosingUnit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;
        }

        private void cDispenseQty_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;
        }

        private void cTakenQty_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;
        }

        private void lblPrescriptionOrderNotes_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = String.IsNullOrEmpty(lblPrescriptionOrderNotes.Text);
        }

        private void cFlagCaption_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;
        }

        private void cSignaRule_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;
        }
        
    }
}
