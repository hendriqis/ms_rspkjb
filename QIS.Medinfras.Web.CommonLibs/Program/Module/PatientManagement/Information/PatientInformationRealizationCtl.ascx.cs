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
    public partial class PatientInformationRealizationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            //hdnHSUID.Value = param;e
            string[] parameter = param.Split('|');
            hdnHSUID.Value = parameter[0];
            hdnDeptID.Value = parameter[1];

            //DATA DISPLAY - HIDE
            if (hdnHSUID.Value == "1")
            {
                trheaderOPrescription.Attributes.Remove("style");
                trdetailOPrescription.Attributes.Remove("style");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");
            }
            else if (hdnHSUID.Value == "2")
            {
                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Remove("style");
                trdetailOPrescriptionR.Attributes.Remove("style");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");
            }
            else if (hdnHSUID.Value == "3")
            {
                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Remove("style");
                trdetailOLaboratory.Attributes.Remove("style");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");
            }
            else if (hdnHSUID.Value == "4")
            {
                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Remove("style");
                trdetailOImaging.Attributes.Remove("style");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");
            }
            else if (hdnHSUID.Value == "5")
            {
                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Remove("style");
                trdetailOOtherDiagnostic.Attributes.Remove("style");

                trheaderOService.Attributes.Add("style", "display:none");
                trdetailOService.Attributes.Add("style", "display:none");
            }
            else if (hdnHSUID.Value == "6")
            {
                trheaderOPrescription.Attributes.Add("style", "display:none");
                trdetailOPrescription.Attributes.Add("style", "display:none");

                trheaderOPrescriptionR.Attributes.Add("style", "display:none");
                trdetailOPrescriptionR.Attributes.Add("style", "display:none");

                trheaderOLaboratory.Attributes.Add("style", "display:none");
                trdetailOLaboratory.Attributes.Add("style", "display:none");

                trheaderOImaging.Attributes.Add("style", "display:none");
                trdetailOImaging.Attributes.Add("style", "display:none");

                trheaderOOtherDiagnostic.Attributes.Add("style", "display:none");
                trdetailOOtherDiagnostic.Attributes.Add("style", "display:none");

                trheaderOService.Attributes.Remove("style");
                trdetailOService.Attributes.Remove("style");
            }

            //CALL METHOD
            bindGrdPrescriptionOrderO();
            bindGrdPrescriptionROrderO();
            bindGrdLaboratoryOrderO();
            bindGrdImagingOrderO();
            bindGrdOtherDiagnosticO();
            bindGrdServiceO();
        }

        ////METHOD
        private void bindGrdPrescriptionOrderO()
        {
            List<GetRegistrationRealizationInfoRekap> lstEntity = BusinessLayer.GetRegistrationRealizationInfoRekapList(hdnDeptID.Value, Convert.ToInt32(hdnHSUID.Value));
            grdViewPrescriptionO.DataSource = lstEntity;
            grdViewPrescriptionO.DataBind();
        }
        private void bindGrdPrescriptionROrderO()
        {
            List<GetRegistrationRealizationInfoRekap> lstEntity = BusinessLayer.GetRegistrationRealizationInfoRekapList(hdnDeptID.Value, Convert.ToInt32(hdnHSUID.Value));
            grdViewPrescriptionRO.DataSource = lstEntity;
            grdViewPrescriptionRO.DataBind();
        }
        private void bindGrdLaboratoryOrderO()
        {
            List<GetRegistrationRealizationInfoRekap> lstEntity = BusinessLayer.GetRegistrationRealizationInfoRekapList(hdnDeptID.Value, Convert.ToInt32(hdnHSUID.Value));
            grdViewLaboratoryO.DataSource = lstEntity;
            grdViewLaboratoryO.DataBind();
        }
        private void bindGrdImagingOrderO()
        {
            List<GetRegistrationRealizationInfoRekap> lstEntity = BusinessLayer.GetRegistrationRealizationInfoRekapList(hdnDeptID.Value, Convert.ToInt32(hdnHSUID.Value));
            grdViewImagingO.DataSource = lstEntity;
            grdViewImagingO.DataBind();
        }
        private void bindGrdOtherDiagnosticO()
        {
            List<GetRegistrationRealizationInfoRekap> lstEntity = BusinessLayer.GetRegistrationRealizationInfoRekapList(hdnDeptID.Value, Convert.ToInt32(hdnHSUID.Value));
            grdViewOtherDiagnosticO.DataSource = lstEntity;
            grdViewOtherDiagnosticO.DataBind();
        }
        private void bindGrdServiceO()
        {
            List<GetRegistrationRealizationInfoRekap> lstEntity = BusinessLayer.GetRegistrationRealizationInfoRekapList(hdnDeptID.Value, Convert.ToInt32(hdnHSUID.Value));
            grdViewServiceO.DataSource = lstEntity;
            grdViewServiceO.DataBind();
        }
    }
}