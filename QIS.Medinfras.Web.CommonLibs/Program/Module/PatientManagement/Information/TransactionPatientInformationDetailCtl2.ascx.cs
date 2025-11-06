using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransactionPatientInformationDetailCtl2 : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] parameter = param.Split('|');
            hdnRegistrationID.Value = parameter[0];
            hdnParamCode.Value = parameter[1];

            string filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            vRegistration entityRegistration = BusinessLayer.GetvRegistrationList(filterExpression).FirstOrDefault();
            txtRegistraionNo.Text = entityRegistration.RegistrationNo;
            txtPatientName.Text = entityRegistration.PatientName;

            if (hdnParamCode.Value == "pof")
            {
                trheaderFPrescription.Attributes.Remove("style");
                trdetailFPrescription.Attributes.Remove("style");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "poo")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Remove("style");
                trdetailOPrescription.Attributes.Remove("style");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            if (hdnParamCode.Value == "porf")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Remove("style");
                trdetailFPrescriptionR.Attributes.Remove("style");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "poro")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Remove("style");
                trdetailOPrescriptionR.Attributes.Remove("style");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "pob")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Remove("style");
                trdetailPObat.Attributes.Remove("style");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "opob")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Remove("style");
                trdetailOPObat.Attributes.Remove("style");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "pal")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Remove("style");
                trdetailPAlkes.Attributes.Remove("style");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "opal")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Remove("style");
                trdetailOPAlkes.Attributes.Remove("style");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "pbu")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Remove("style");
                trdetailPBarangUmum.Attributes.Remove("style");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "opbu")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Remove("style");
                trdetailOPBarangUmum.Attributes.Remove("style");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "lbf")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Remove("style");
                trdetailFLaboratory.Attributes.Remove("style");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");
                
                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");
                
                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }

            else if (hdnParamCode.Value == "lbo")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Remove("style");
                trdetailOLaboratory.Attributes.Remove("style");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "imf")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Remove("style");
                trdetailFImaging.Attributes.Remove("style");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "imo")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Remove("style");
                trdetailOImaging.Attributes.Remove("style");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "odf")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Remove("style");
                trdetailFOtherDiagnostic.Attributes.Remove("style");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "odo")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Remove("style");
                trdetailOOtherDiagnostic.Attributes.Remove("style");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "srf")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Remove("style");
                trdetailFService.Attributes.Remove("style");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "sro")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Remove("style");
                trdetailOService.Attributes.Remove("style");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "altf")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Remove("style");
                trdetailFAllTransaction.Attributes.Remove("style");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "alto")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Remove("style");
                trdetailOAllTransaction.Attributes.Remove("style");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "bilf")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Remove("style");
                trdetailFBilling.Attributes.Remove("style");

                trheaderOBilling.Attributes.Add("style", "display:none");
                trdetailOBilling.Attributes.Add("style", "display:none");
            }
            else if (hdnParamCode.Value == "bilo")
            {
                trheaderFPrescription.Attributes.Add("style", "display:none");
                trdetailFPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderFPrescriptionR.Attributes.Add("style", "display:none");
                trdetailFPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderPObat.Attributes.Add("style", "display:none");
                trdetailPObat.Attributes.Add("style", "display:none");

                trheaderOPObat.Attributes.Add("style", "display:none");
                trdetailOPObat.Attributes.Add("style", "display:none");

                trheaderPAlkes.Attributes.Add("style", "display:none");
                trdetailPAlkes.Attributes.Add("style", "display:none");

                trheaderOPAlkes.Attributes.Add("style", "display:none");
                trdetailOPAlkes.Attributes.Add("style", "display:none");

                trheaderPBarangUmum.Attributes.Add("style", "display:none");
                trdetailPBarangUmum.Attributes.Add("style", "display:none");

                trheaderOPBarangUmum.Attributes.Add("style", "display:none");
                trdetailOPBarangUmum.Attributes.Add("style", "display:none");

                trheaderFLaboratory.Attributes.Add("style", "display:none");
                trdetailFLaboratory.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderFImaging.Attributes.Add("style", "display:none");
                trdetailFImaging.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderFOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailFOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderFService.Attributes.Add("style", "display:none");
                trdetailFService.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");

                trheaderFAllTransaction.Attributes.Add("style", "display:none");
                trdetailFAllTransaction.Attributes.Add("style", "display:none");

                trheaderOAllTransaction.Attributes.Add("style", "display:none");
                trdetailOAllTransaction.Attributes.Add("style", "display:none");

                trheaderFBilling.Attributes.Add("style", "display:none");
                trdetailFBilling.Attributes.Add("style", "display:none");

                trheaderOBilling.Attributes.Remove("style");
                trdetailOBilling.Attributes.Remove("style");
            }

            bindGrdPrescriptionOrderF(filterExpression);
            bindGrdPrescriptionOrderO(filterExpression);
            bindGrdPrescriptionROrderF(filterExpression);
            bindGrdPrescriptionROrderO(filterExpression);

            bindGrdLaboratoryOrderF(filterExpression);
            bindGrdLaboratoryOrderO(filterExpression);
            bindGrdImagingOrderF(filterExpression);
            bindGrdImagingOrderO(filterExpression);
            bindGrdOtherDiagnosticF(filterExpression);
            bindGrdOtherDiagnosticO(filterExpression);
            bindGrdServiceF(filterExpression);
            bindGrdServiceO(filterExpression);

            bindGrdAllTransactionO(filterExpression);
            bindGrdAllTransactionF(filterExpression);

            bindGrdPostingObat(filterExpression);
            bindGrdOutstandingPostingObat(filterExpression);
            bindGrdPostingAlkes(filterExpression);
            bindGrdOutstandingPostingAlkes(filterExpression);
            bindGrdPostingBarangUmum(filterExpression);
            bindGrdOutstandingPostingBarangUmum(filterExpression);

            bindGrdBillingF(filterExpression);
            bindGrdBillingO(filterExpression);
        }

        private void bindGrdPrescriptionOrderF(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}') AND GCTransactionStatus != '{2}'", Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vPrescriptionOrderHd> lstPrescription = BusinessLayer.GetvPrescriptionOrderHdList(filterExpression);
            grdViewPrescriptionF.DataSource = lstPrescription;
            grdViewPrescriptionF.DataBind();
        }

        private void bindGrdPrescriptionOrderO(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}','{1}','{2}')", Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vPrescriptionOrderHd> lstPrescription = BusinessLayer.GetvPrescriptionOrderHdList(filterExpression);
            grdViewPrescriptionO.DataSource = lstPrescription;
            grdViewPrescriptionO.DataBind();
        }

        private void bindGrdPrescriptionROrderF(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}') AND GCTransactionStatus != '{2}'", Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vPrescriptionReturnOrderHd> lstPrescriptionR = BusinessLayer.GetvPrescriptionReturnOrderHdList(filterExpression);
            grdViewPrescriptionRF.DataSource = lstPrescriptionR;
            grdViewPrescriptionRF.DataBind();
        }

        private void bindGrdPrescriptionROrderO(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}','{1}','{2}')", Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vPrescriptionReturnOrderHd> lstPrescriptionR = BusinessLayer.GetvPrescriptionReturnOrderHdList(filterExpression);
            grdViewPrescriptionRO.DataSource = lstPrescriptionR;
            grdViewPrescriptionRO.DataBind();
        }

        private void bindGrdLaboratoryOrderF(String filterExpression)
        {
            filterExpression += string.Format(" AND TransactionCode = '{0}' AND GCTransactionStatus IN ('{1}','{2}') AND GCTransactionStatus != '{3}' ORDER BY TestOrderDate,TestOrderTime", Constant.TransactionCode.LABORATORY_TEST_ORDER, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vTestOrderHd> lstTestOrderHd = BusinessLayer.GetvTestOrderHdList(filterExpression);
            grdViewLaboratoryF.DataSource = lstTestOrderHd;
            grdViewLaboratoryF.DataBind();
        }

        private void bindGrdLaboratoryOrderO(String filterExpression)
        {
            filterExpression += string.Format(" AND TransactionCode = '{0}' AND GCTransactionStatus NOT IN ('{1}','{2}','{3}') ORDER BY TestOrderDate,TestOrderTime", Constant.TransactionCode.LABORATORY_TEST_ORDER, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vTestOrderHd> lstTestOrderHd = BusinessLayer.GetvTestOrderHdList(filterExpression);
            grdViewLaboratoryO.DataSource = lstTestOrderHd;
            grdViewLaboratoryO.DataBind();
        }

        private void bindGrdImagingOrderF(String filterExpression)
        {
            filterExpression += string.Format(" AND TransactionCode = '{0}' AND GCTransactionStatus IN ('{1}','{2}') AND GCTransactionStatus != '{3}' ORDER BY TestOrderDate,TestOrderTime", Constant.TransactionCode.IMAGING_TEST_ORDER, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vTestOrderHd> lstTestOrderHd = BusinessLayer.GetvTestOrderHdList(filterExpression);
            grdViewImagingF.DataSource = lstTestOrderHd;
            grdViewImagingF.DataBind();
        }

        private void bindGrdImagingOrderO(String filterExpression)
        {
            filterExpression += string.Format(" AND TransactionCode = '{0}' AND GCTransactionStatus NOT IN ('{1}','{2}','{3}') ORDER BY TestOrderDate,TestOrderTime", Constant.TransactionCode.IMAGING_TEST_ORDER, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vTestOrderHd> lstTestOrderHd = BusinessLayer.GetvTestOrderHdList(filterExpression);
            grdViewImagingO.DataSource = lstTestOrderHd;
            grdViewImagingO.DataBind();
        }

        private void bindGrdOtherDiagnosticF(String filterExpression)
        {
            filterExpression += string.Format(" AND TransactionCode = '{0}' AND GCTransactionStatus IN ('{1}','{2}') AND GCTransactionStatus != '{3}' ORDER BY TestOrderDate,TestOrderTime", Constant.TransactionCode.OTHER_TEST_ORDER, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vTestOrderHd> lstTestOrderHd = BusinessLayer.GetvTestOrderHdList(filterExpression);
            grdViewOtherDiagnosticF.DataSource = lstTestOrderHd;
            grdViewOtherDiagnosticF.DataBind();
        }

        private void bindGrdOtherDiagnosticO(String filterExpression)
        {
            filterExpression += string.Format(" AND TransactionCode = '{0}' AND GCTransactionStatus NOT IN ('{1}','{2}','{3}') ORDER BY TestOrderDate,TestOrderTime", Constant.TransactionCode.OTHER_TEST_ORDER, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vTestOrderHd> lstTestOrderHd = BusinessLayer.GetvTestOrderHdList(filterExpression);
            grdViewOtherDiagnosticO.DataSource = lstTestOrderHd;
            grdViewOtherDiagnosticO.DataBind();
        }

        private void bindGrdServiceF(String filterExpression)
        {
            filterExpression += string.Format(" AND TransactionCode IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND GCTransactionStatus IN ('{6}','{7}') AND GCTransactionStatus != '{8}' ORDER BY ServiceOrderDate,ServiceOrderTime",
                                                Constant.TransactionCode.OP_EMERGENCY_ORDER, Constant.TransactionCode.ER_OUTPATIENT_ORDER, Constant.TransactionCode.IP_EMERGENCY_ORDER, Constant.TransactionCode.IP_OUTPATIENT_ORDER,
                                                Constant.TransactionCode.MCU_EMERGENCY_ORDER, Constant.TransactionCode.MCU_OUTPATIENT_ORDER, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vServiceOrderHd> lstServiceOrderHd = BusinessLayer.GetvServiceOrderHdList(filterExpression);
            grdViewServiceF.DataSource = lstServiceOrderHd;
            grdViewServiceF.DataBind();
        }

        private void bindGrdServiceO(String filterExpression)
        {
            filterExpression += string.Format(" AND TransactionCode IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND GCTransactionStatus NOT IN ('{6}','{7}','{8}') ORDER BY ServiceOrderDate,ServiceOrderTime",
                                                Constant.TransactionCode.OP_EMERGENCY_ORDER, Constant.TransactionCode.ER_OUTPATIENT_ORDER, Constant.TransactionCode.IP_EMERGENCY_ORDER, Constant.TransactionCode.IP_OUTPATIENT_ORDER,
                                                Constant.TransactionCode.MCU_EMERGENCY_ORDER, Constant.TransactionCode.MCU_OUTPATIENT_ORDER, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vServiceOrderHd> lstServiceOrderHd = BusinessLayer.GetvServiceOrderHdList(filterExpression);
            grdViewServiceO.DataSource = lstServiceOrderHd;
            grdViewServiceO.DataBind();
        }

        private void bindGrdAllTransactionF(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus = '{0}' AND GCTransactionStatus != '{1}' ORDER BY TransactionDate,TransactionTime", Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vPatientChargesHd> lstPatientChargesHd = BusinessLayer.GetvPatientChargesHdList(filterExpression);
            grdViewAllTransactionF.DataSource = lstPatientChargesHd;
            grdViewAllTransactionF.DataBind();
        }

        private void bindGrdAllTransactionO(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}','{1}') ORDER BY TransactionDate,TransactionTime", Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vPatientChargesHd> lstPatientChargesHd = BusinessLayer.GetvPatientChargesHdList(filterExpression);
            grdViewAllTransactionO.DataSource = lstPatientChargesHd;
            grdViewAllTransactionO.DataBind();
        }

        private void bindGrdPostingObat(String filterExpression)
        {
            filterExpression += string.Format(" AND IsDeleted = 0 AND IsApproved = 1 AND GCTransactionStatus != '{0}' AND GCItemType = '{1}'", Constant.TransactionStatus.VOID, Constant.ItemType.OBAT_OBATAN);
            List<vPatientChargesDt4> lstCharges4 = BusinessLayer.GetvPatientChargesDt4List(filterExpression);
            grdViewObatP.DataSource = lstCharges4;
            grdViewObatP.DataBind();
        }

        private void bindGrdOutstandingPostingObat(String filterExpression)
        {
            filterExpression += string.Format(" AND IsDeleted = 0 AND IsApproved = 0 AND GCTransactionStatus != '{0}' AND GCItemType = '{1}'", Constant.TransactionStatus.VOID, Constant.ItemType.OBAT_OBATAN);
            List<vPatientChargesDt4> lstCharges4 = BusinessLayer.GetvPatientChargesDt4List(filterExpression);
            grdViewObatOP.DataSource = lstCharges4;
            grdViewObatOP.DataBind();
        }

        private void bindGrdPostingAlkes(String filterExpression)
        {
            filterExpression += string.Format(" AND IsDeleted = 0 AND IsApproved = 1 AND GCTransactionStatus != '{0}' AND GCItemType = '{1}'", Constant.TransactionStatus.VOID, Constant.ItemType.BARANG_MEDIS);
            List<vPatientChargesDt4> lstCharges4 = BusinessLayer.GetvPatientChargesDt4List(filterExpression);
            grdViewAlkesP.DataSource = lstCharges4;
            grdViewAlkesP.DataBind();
        }

        private void bindGrdOutstandingPostingAlkes(String filterExpression)
        {
            filterExpression += string.Format(" AND IsDeleted = 0 AND IsApproved = 0 AND GCTransactionStatus != '{0}' AND GCItemType = '{1}'", Constant.TransactionStatus.VOID, Constant.ItemType.BARANG_MEDIS);
            List<vPatientChargesDt4> lstCharges4 = BusinessLayer.GetvPatientChargesDt4List(filterExpression);
            grdViewAlkesOP.DataSource = lstCharges4;
            grdViewAlkesOP.DataBind();
        }

        private void bindGrdPostingBarangUmum(String filterExpression)
        {
            filterExpression += string.Format(" AND IsDeleted = 0 AND IsApproved = 1 AND GCTransactionStatus != '{0}' AND GCItemType = '{1}'", Constant.TransactionStatus.VOID, Constant.ItemType.BARANG_UMUM);
            List<vPatientChargesDt4> lstCharges4 = BusinessLayer.GetvPatientChargesDt4List(filterExpression);
            grdViewBarangUmumP.DataSource = lstCharges4;
            grdViewBarangUmumP.DataBind();
        }

        private void bindGrdOutstandingPostingBarangUmum(String filterExpression)
        {
            filterExpression += string.Format(" AND IsDeleted = 0 AND IsApproved = 0 AND GCTransactionStatus != '{0}' AND GCItemType = '{1}'", Constant.TransactionStatus.VOID, Constant.ItemType.BARANG_UMUM);
            List<vPatientChargesDt4> lstCharges4 = BusinessLayer.GetvPatientChargesDt4List(filterExpression);
            grdViewBarangUmumOP.DataSource = lstCharges4;
            grdViewBarangUmumOP.DataBind();
        }

        private void bindGrdBillingF(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}') AND GCTransactionStatus != '{2}' ORDER BY BillingDate,BillingTime", Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vPatientBill> lstPatientBillHd = BusinessLayer.GetvPatientBillList(filterExpression);
            grdViewBillingF.DataSource = lstPatientBillHd;
            grdViewBillingF.DataBind();
        }

        private void bindGrdBillingO(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}','{1}','{2}') ORDER BY BillingDate,BillingTime", Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            List<vPatientBill> lstPatientBillHd = BusinessLayer.GetvPatientBillList(filterExpression);
            grdViewBillingO.DataSource = lstPatientBillHd;
            grdViewBillingO.DataBind();
        }
    }
}