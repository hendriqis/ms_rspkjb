using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MDTestResultDetailVerifiedCtl : BaseContentPopupCtl
    {
        protected string GCTemplateGroup = "";
        public override void InitializeControl(string param)
        {
            GCTemplateGroup = Constant.TemplateGroup.IMAGING;

            hdnItemID.Value = param;
            string[] par = param.Split('|');
            hdnItemID.Value = par[0];
            hdnID.Value = par[1];
            hdnRISVendor.Value = AppSession.RIS_HL7_MESSAGE_FORMAT;

            vImagingResultDt entityDT = BusinessLayer.GetvImagingResultDtList(string.Format("ID = {0} AND ItemID = {1}", hdnID.Value, hdnItemID.Value)).FirstOrDefault();
            EntityToControl(entityDT);

            List<SettingParameterDt> lstSetpar = BusinessLayer.GetSettingParameterDtList(
                        string.Format("HealthcareID='{0}' AND ParameterCode IN ('{1}', '{2}')",
                        AppSession.UserLogin.HealthcareID,
                        Constant.SettingParameter.IS_RIS_WEB_VIEW_URL,
                        Constant.SettingParameter.IS_RIS_BRIDGING
                        ));
            hdnIsRisBRIDGING.Value = lstSetpar.Where(p => p.ParameterCode == Constant.SettingParameter.IS_RIS_BRIDGING).FirstOrDefault().ParameterValue;

            if (hdnIsRisBRIDGING.Value == "1")
            {
                SettingParameterDt setvar = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_RIS_WEB_VIEW_URL);
                if (setvar != null && !string.IsNullOrEmpty(setvar.ParameterValue))
                {
                    hdnViewerUrl.Value = setvar.ParameterValue;
                    if (AppSession.RIS_HL7_MESSAGE_FORMAT == Constant.RIS_HL7MessageFormat.FUJI_FILM)
                    {
                        hdnViewerUrl.Value += "&winpass=true&filter=accessionnumber=";
                    }
                }
                else
                {
                    hdnViewerUrl.Value = AppSession.RIS_WEB_VIEW_URL;
                    if (AppSession.RIS_HL7_MESSAGE_FORMAT == Constant.RIS_HL7MessageFormat.FUJI_FILM)
                    {
                        hdnViewerUrl.Value += "&winpass=true&filter=accessionnumber=";
                    }
                }
            }
            else
            {
                vImagingResultHd imgHd = BusinessLayer.GetvImagingResultHdList(string.Format("ID='{0}'", entityDT.ID)).FirstOrDefault();
                string patientImagingPath = string.Format("Patient/{0}/Document/Imaging/", imgHd.MedicalNo);
                hdnViewerUrl.Value = string.Format("{0}{1}{2}", AppConfigManager.QISVirtualDirectory, patientImagingPath, entityDT.FileName);
            }

        }

        private void EntityToControl(vImagingResultDt entity)
        {
            txtItemInfo.Text = string.Format("{0} - {1}", entity.ItemCode,entity.ItemName);
            txtBorderLine.Text = entity.BorderLine;
            txtPhotoNumber.Text = entity.PhotoNumber;
            contentIndonesia.InnerHtml = entity.TestResult1;
            contentEnglish.InnerHtml = entity.TestResult2;
            txtFileName.Text = entity.FileName;
        }

    }
}