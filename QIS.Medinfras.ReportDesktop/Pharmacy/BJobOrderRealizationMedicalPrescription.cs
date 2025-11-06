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
    public partial class BJobOrderRealizationMedicalPrescription : BaseCustomPharmacyA5Rpt
    {
        public BJobOrderRealizationMedicalPrescription()
        {
            InitializeComponent();
        }

            public override void InitializeReport(string[] param)
            {
                GetPrescriptionOrderDtMedicalPrescription entity = BusinessLayer.GetPrescriptionOrderDtMedicalPrescriptionList(Convert.ToInt32(param[0])).FirstOrDefault();

                #region Header
                cMedicalNo.Text = entity.MedicalNo;
                cPatientName.Text = entity.PatientName;
                cDOB.Text = string.Format("{0} ( {1} yr {2} mth {3} day )", entity.DateOfBirthInString, entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
                cCorporate.Text = entity.BusinessPartnerName;
                cPrescriptionNo.Text = entity.TransactionNo;
                cOrderNo.Text = entity.PrescriptionOrderNo;
                cRegistrationNo.Text = entity.RegistrationNo;
                cRegisteredPhysician.Text = entity.ParamedicName;
                #endregion

                #region Header : Per Page
                //cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                //    entityCV.PatientName, entityCV.Gender, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
                //cHeaderRegistration.Text = entityCV.RegistrationNo;
                //cHeaderMedicalNo.Text = entityCV.MedicalNo;
                #endregion

                #region Footer
                lblPrescriptionOrderNotes.Text = entity.Remarks;
//                lblOrderNotes.Text = entity.Remarks;
                //cTTDTakenBy.Text = entityCV.PatientName;
                //cTTDEtiketBy.Text = entityPresHD.ParamedicName;
                //cTTDCompoundBy.Text = entityPresHD.ParamedicName;
                //cTTDVerificationBy.Text = entityPresHD.ParamedicName;
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

            private void lblPrescriptionOrderNotesCaption_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
            {
                e.Cancel = String.IsNullOrEmpty(lblPrescriptionOrderNotes.Text);
            }

            private void lblPrescriptionOrderNotes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
            {
                e.Cancel = String.IsNullOrEmpty(lblPrescriptionOrderNotes.Text);
            }

            //private void xrTableCell64_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
            //{
            //    e.Cancel = String.IsNullOrEmpty(lblOrderNotes.Text);
            //}

            //private void lblOrderNotes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
            //{
            //    e.Cancel = String.IsNullOrEmpty(lblOrderNotes.Text);
            //}
    }
}
