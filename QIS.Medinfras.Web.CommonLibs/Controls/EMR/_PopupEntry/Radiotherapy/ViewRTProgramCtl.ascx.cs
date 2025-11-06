using System;
using System.Collections.Generic;
using System.Data;
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
using System.Globalization;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ViewRTProgramCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnID.Value = paramInfo[0];
            hdnPopupVisitID.Value = paramInfo[1];
            hdnPopupMRN.Value = paramInfo[2];
            hdnPopupMedicalNo.Value = paramInfo[3];
            hdnPopupPatientName.Value = paramInfo[4];
            hdnPopupParamedicID.Value = paramInfo[5];

            
            SetControlProperties(); 
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            string fileName = string.Format(@"{0}\medicalForm\Radiotherapy\{1}.html", filePath, hdnFormType.Value.Replace('^', '_'));
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            hdnFormLayout.Value = innerHtml.ToString();
            divFormContent.InnerHtml = innerHtml.ToString();
        }

        private void SetControlProperties()
        {
            txtMedicalNo.Text = hdnPopupMedicalNo.Value;
            txtPatientName.Text = hdnPopupPatientName.Value;

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}') AND IsActive = 1 AND IsDeleted = 0",
        Constant.StandardCode.BEAM_TECHNIQUE,
        Constant.StandardCode.RADIOTHERAPY_TYPE,
        Constant.StandardCode.APPLICATOR_TYPE,
        Constant.StandardCode.RADIOTHERAPY_PLAN,
        Constant.StandardCode.RADIOTHERAPY_VERIFICATION,
        Constant.StandardCode.RADIOTHERAPY_PURPOSE,
        Constant.StandardCode.BRACHYTHERAPY_TYPE));

            List<StandardCode> lstCode3 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BEAM_TECHNIQUE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCBeamTechnique, lstCode3, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode4 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BEAM_TECHNIQUE && lst.StandardCodeID != "X572^999").ToList();
            dlBeamTechnique.DataSource = lstCode4;
            dlBeamTechnique.DataBind();

            List<StandardCode> lstCode5 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.RADIOTHERAPY_TYPE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCTherapyType, lstCode5, "StandardCodeName", "StandardCodeID");
            cboGCTherapyType.SelectedIndex = 0;

            List<StandardCode> lstCode6 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BRACHYTHERAPY_TYPE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCBrachyTherapyType, lstCode6, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode7 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.APPLICATOR_TYPE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCApplicatorType, lstCode7, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode8 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.RADIOTHERAPY_PLAN).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCTherapyPlan, lstCode8, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode9 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.RADIOTHERAPY_VERIFICATION).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCVerificationType, lstCode9, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode10 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.RADIOTHERAPY_PURPOSE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCTherapyPurpose, lstCode10, "StandardCodeName", "StandardCodeID");

            VisitInfoForRadiotherapyProgram oInfo = BusinessLayer.GetVisitInfoForRadiotherapyProgram(Convert.ToInt32(hdnPopupVisitID.Value)).FirstOrDefault();
            if (oInfo != null)
            {
                txtDiagnosisInfo.Text = oInfo.PatientDiagnosis.Replace("+", Environment.NewLine);
                txtStagingInfo.Text = string.Format("{0} : T{1} N{2} M{3}", oInfo.CancerStaging, oInfo.TValue, oInfo.NValue, oInfo.MValue);
            }

            string filter = string.Format("ProgramID = '{0}'", hdnID.Value);
            vRadiotheraphyProgram entity = BusinessLayer.GetvRadiotheraphyProgramList(filter).FirstOrDefault();
            EntityToControl(entity);
        }

        private void EntityToControl(vRadiotheraphyProgram oRecord)
        {
            txtProgramDate.Text = oRecord.ProgramDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtProgramTime.Text = oRecord.ProgramTime;
            if (!string.IsNullOrEmpty(oRecord.GCTherapyType))
            {
                cboGCTherapyType.Value = oRecord.GCTherapyType;
            }
            if (oRecord.GCTherapyType == Constant.RadiotherapyType.EXTERNAL)
            {
                trExternal1.Style.Add("display", "table-row");
                trBeamTechnique.Style.Add("display", "table-row");
                trVerification.Style.Add("display", "table-row");

                trBrachytherapy1.Style.Add("display", "none");
                trBrachytherapy2.Style.Add("display", "none");
            }
            else
            {
                trBrachytherapy1.Style.Add("display", "table-row");
                trBrachytherapy1.Style.Add("display", "table-row");

                trExternal1.Style.Add("display", "none");
                trBeamTechnique.Style.Add("display", "none");
                trVerification.Style.Add("display", "none");
            }
            if (!string.IsNullOrEmpty(oRecord.GCTherapyPurpose))
            {
                cboGCTherapyPurpose.Value = oRecord.GCTherapyPurpose;
            }
            if (!string.IsNullOrEmpty(oRecord.GCTherapyPlan))
            {
                cboGCTherapyPlan.Value = oRecord.GCTherapyPlan;
            }
            cboGCBeamTechnique.Value = oRecord.GCBeamTechnique;
            if (oRecord.GCBeamTechnique == "X572^999")
            {
                trCombination.Style.Add("display", "table-row");
            }
            else
            {
                trCombination.Style.Add("display", "none");
            }
            hdnCombinationTechniqueCode.Value = oRecord.CombinationBeamTechCode;
            hdnCombinationTechnique.Value = oRecord.CombinationBeamTech;

            if (!string.IsNullOrEmpty(oRecord.GCBrachytherapyType))
            {
                cboGCBrachyTherapyType.Value = oRecord.GCBrachytherapyType;
            }

            if (!string.IsNullOrEmpty(oRecord.GCApplicatorType))
            {
                cboGCApplicatorType.Value = oRecord.GCApplicatorType;
            }

            txtNumberOfDosage1.Text = oRecord.NumberOfDosage1.ToString();
            txtNumberOfFields1.Text = oRecord.NumberOfFields1.ToString();
            txtTotalDosage1.Text = oRecord.TotalDosage1.ToString();

            if (oRecord.IsHasDosage2)
            {
                chkIsHasSequence2.Checked = oRecord.IsHasDosage2;
                txtNumberOfDosage2.Text = oRecord.NumberOfDosage2.ToString();
                txtNumberOfFields2.Text = oRecord.NumberOfFields2.ToString();
                txtTotalDosage2.Text = oRecord.TotalDosage2.ToString();               
            }

            if (oRecord.IsHasDosage3)
            {
                chkIsHasSequence3.Checked = oRecord.IsHasDosage3;
                txtNumberOfDosage3.Text = oRecord.NumberOfDosage3.ToString();
                txtNumberOfFields3.Text = oRecord.NumberOfFields3.ToString();
                txtTotalDosage3.Text = oRecord.TotalDosage3.ToString();
            }

            if (oRecord.GCVerificationType != null)
            {
                cboGCVerificationType.Value = oRecord.GCVerificationType;
            }

            txtRemarks.Text = oRecord.Remarks;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}