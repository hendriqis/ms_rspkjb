using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using System.Text;
using System.IO;
using DevExpress.Web.ASPxCallbackPanel;
using System.Net;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ViewPreHDAssessmentCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                SetControlProperties(paramInfo);
            }
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnID.Value = paramInfo[0];
            vPreHDAssessment entity = BusinessLayer.GetvPreHDAssessmentList(string.Format("ID = {0}", hdnID.Value)).FirstOrDefault();
            if (entity != null)
            {
                EntityToControl(entity);
            }
        }

        private void EntityToControl(vPreHDAssessment entity)
        {
            txtAsessmentDate.Text = entity.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAsessmentTime.Text = entity.AssessmentTime;
            txtParamedicName.Text = entity.ParamedicName;
            txtAdditionalRemarks.Text = entity.AdditionalRemarks;
            txtHDNo.Text = entity.HDNo.ToString();
            txtFirstHDDate.Text = entity.FirstHemodialysisDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtHFRNo.Text = entity.HFRNo.ToString("G29");
            txtHDFMDNo.Text = entity.HDFMDNo.ToString("G29");
            txtHemoperfusionNo.Text = entity.HemoperfusionNo.ToString("G29");
            txtMachineNo.Text = entity.MachineNo;
            txtHDType.Text = entity.HDType;
            txtHDMethod.Text = entity.HDMethod;
            txtMachineType.Text = entity.HDMachineType;
            txtReuseNo.Text = entity.ReuseNo.ToString("G29");
            txtVolumePriming.Text = entity.VolumePriming.ToString();
            txtDialysate.Text = entity.Dialysate;
            txtDialysateRemarks.Text = entity.DialysateRemarks;
            txtHDDuration.Text = entity.HDDuration.ToString();
            txtHDFrequency.Text = entity.HDFrequency.ToString();
            txtQB.Text = entity.QB.ToString();
            txtQD.Text = entity.QD.ToString();
            txtUFGoal.Text = entity.UFGoal.ToString();
            txtProgProfilingNa.Text = entity.ProgFillingNa.ToString();
            txtProgProfilingUF.Text = entity.ProgFillingUF.ToString();
            txtAdditionalRemarks.Text = entity.AdditionalRemarks;

            optIsHeparization.Checked = entity.HeparinizationStatus == "1";
            txtHeparizationDosageInitiate.Text = entity.HeparizationDosageInitiate.ToString();
            txtHeparizationDosageCirculation.Text = entity.HeparizationDosageCirculation.ToString();
            txtHeparizationDosageContinues.Text = entity.HeparizationDosageContinues.ToString();
            txtHeparizationDosageIntermitten.Text = entity.HeparizationDosageIntermitten.ToString();

            optIsWithoutHeparization.Checked = entity.HeparinizationStatus == "2";
            txtWithoutHeparizationRemarks.Text = entity.WithoutHeparizationRemarks;

            optIsLMWH.Checked = entity.HeparinizationStatus == "3";
            txtLMWHRemarks.Text = entity.LMWHRemarks;

            chkIsDialysisBleach.Checked = entity.IsDialysisBleach;
            txtDialysisBleach.Text = entity.DialysisBleach.ToString();
            txtDialysisBleachUnit.Text = entity.DialysisBleachUnit;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}