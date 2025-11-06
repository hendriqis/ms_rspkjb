using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;
using System.Data;
using System.Text;
using System.IO;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ViewBrachytherapyProgramLogCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            SetControlProperties(paramInfo);
            BrachytherapyProgramLog entity = BusinessLayer.GetBrachytherapyProgramLog(Convert.ToInt32(hdnProgramLogID.Value));
            EntityToControl(entity);
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            string fileName = string.Format(@"{0}\medicalForm\Radiotherapy\{1}.html", filePath, "brachytherapyLog01");
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            hdnFormLayout.Value = innerHtml.ToString();
            divFormContent.InnerHtml = innerHtml.ToString();
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnProgramLogID.Value = paramInfo[0];
            hdnProgramID.Value = paramInfo[1];
            hdnPopupVisitID.Value = paramInfo[2];
            hdnTotalFraction.Value = paramInfo[3];

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
Constant.StandardCode.BRACHYTHERAPY_TYPE, Constant.StandardCode.APPLICATOR_TYPE));

            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BRACHYTHERAPY_TYPE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCBrachyTherapyType, lstCode1, "StandardCodeName", "StandardCodeID");
            List<StandardCode> lstCode2 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.APPLICATOR_TYPE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCApplicatorType, lstCode2, "StandardCodeName", "StandardCodeID");
        }

        private void EntityToControl(BrachytherapyProgramLog entity)
        {
           txtLogDate.Text = entity.LogDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
           txtLogTime.Text = entity.LogTime;
           cboGCBrachyTherapyType.Value = entity.GCBrachytherapyType;
           cboGCApplicatorType.Value = entity.GCApplicatorType;
           txtFractionNo.Text = entity.FractionNo.ToString();
           txtTotalChannel.Text = entity.TotalChannel.ToString();
           if (!string.IsNullOrEmpty(entity.ChannelPositionFormValue))
           {
               divFormContent.InnerHtml = entity.ChannelPositionFormLayout;
               hdnFormLayout.Value = entity.ChannelPositionFormLayout;
               hdnFormValues.Value = entity.ChannelPositionFormValue;
               trChannelLayout.Style.Add("display", "table-row");
               hdnIsUsingForm.Value = "1";
           }
           else
           {
               trChannelLayout.Style.Add("display", "none");
               hdnIsUsingForm.Value = "0";
               PopulateFormContent();
           }

           txtTotalDosage.Text = entity.TotalDosage.ToString();
           txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(BrachytherapyProgramLog entity)
        {
            entity.LogDate = Helper.GetDatePickerValue(txtLogDate);
            entity.LogTime = txtLogTime.Text;
            entity.ProgramID = Convert.ToInt32(hdnProgramID.Value);
            entity.VisitID = Convert.ToInt32(hdnPopupVisitID.Value);
            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            entity.GCBrachytherapyType = cboGCBrachyTherapyType.Value.ToString();
            entity.GCApplicatorType = cboGCApplicatorType.Value.ToString();
            entity.FractionNo = Convert.ToInt32(txtFractionNo.Text);
            entity.TotalChannel = Convert.ToInt32(txtTotalChannel.Text);
            if (hdnIsUsingForm.Value == "1")
            {
                entity.ChannelPositionFormLayout = hdnFormLayout.Value;
                entity.ChannelPositionFormValue = hdnFormValues.Value;
            }
            entity.TotalDosage = Convert.ToDecimal(txtTotalDosage.Text);
            entity.Remarks = txtRemarks.Text;
        }
    }
}