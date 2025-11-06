using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web;
using System.IO;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program.PatientPage
{
    public partial class ViewConsentFormCtl1 : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnEducationFormGroup.Value = paramInfo[0];
            if (paramInfo[1] != "" && paramInfo[1] != "0")
            {
                hdnID.Value = paramInfo[1];
                vConsentForm entity = BusinessLayer.GetvConsentFormList(string.Format("ConsentFormID = {0}",Convert.ToInt32(hdnID.Value))).FirstOrDefault();
                EntityToControl(entity);
            }
        }

        private void EntityToControl(vConsentForm entity)
        {
            txtObservationDate.Text = entity.ConsentFormDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = entity.ConsentFormTime;
            txtParamedicName.Text = entity.ParamedicName;
            rblIsPatientFamily.SelectedValue = entity.IsPatientFamily ? "1" : "0";
            trFamilyInfo.Visible = entity.IsPatientFamily;
            if (entity.IsPatientFamily)
                trFamilyInfo.Style.Add("display", "table-row");
            else
                trFamilyInfo.Style.Add("display", "none");
            txtFamilyRelation.Text = entity.FamilyRelation;
            txtSignature2Name.Text = entity.SignatureName2;
            txtGroup.Text = entity.ConsentFormGroup;
            txtType.Text = entity.ConsentFormType;
            divFormContent.InnerHtml = entity.ConsentFormLayout;
            hdnDivFormLayout.Value = entity.ConsentFormLayout;
            hdnFormValues.Value = entity.ConsentFormValue;

            txtSignature3Name.Text = entity.SignatureName3;
            txtSignature4Name.Text = entity.SignatureName4;

            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

            System.Web.UI.WebControls.Image imgSignature = new System.Web.UI.WebControls.Image();
            imgSignature.Height = 180;
            imgSignature.Width = 400;
            imgSignature.ImageUrl = entity.SignatureID1Type1;
            plSignature1.Controls.Add(imgSignature);

            imgSignature = new System.Web.UI.WebControls.Image();
            imgSignature.Height = 180;
            imgSignature.Width = 400;
            imgSignature.ImageUrl = entity.SignatureID2Type1;
            plSignature2.Controls.Add(imgSignature);

            imgSignature = new System.Web.UI.WebControls.Image();
            imgSignature.Height = 180;
            imgSignature.Width = 400;
            imgSignature.ImageUrl = entity.SignatureID3Type1;
            plSignature3.Controls.Add(imgSignature);

            imgSignature = new System.Web.UI.WebControls.Image();
            imgSignature.Height = 180;
            imgSignature.Width = 400;
            imgSignature.ImageUrl = entity.SignatureID4Type1;
            plSignature4.Controls.Add(imgSignature);
        }
    }
}