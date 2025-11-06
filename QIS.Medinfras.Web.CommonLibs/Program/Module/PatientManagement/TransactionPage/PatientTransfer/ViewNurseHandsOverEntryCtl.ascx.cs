using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program.PatientPage
{
    public partial class ViewNurseHandsOverEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnID.Value = param;
                hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
                SetControlProperties();
            }
        }

        private void SetControlProperties()
        {
            vPatientNurseTransfer obj = BusinessLayer.GetvPatientNurseTransferList(string.Format("ID = {0}", hdnID.Value)).FirstOrDefault();
            EntityToControl(obj);
        }

        private void EntityToControl(vPatientNurseTransfer obj)
        {
            txtTransferDate.Text = obj.TransferDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransferTime.Text = obj.TransferTime;
            txtFromParamedicInfo.Text = string.Format("{0} ({1})", obj.FromNurseName, obj.FromNurseCode);
            txtToParamedicInfo.Text = string.Format("{0} ({1})", obj.ToNurseName, obj.ToNurseCode);
            txtTransferType.Text = obj.PatientNurseTransferType;
            txtSituationText.Text = obj.Situation;
            txtBackgroundText.Text = obj.Background;
            txtAssessmentText.Text = obj.Assessment;
            txtRecommendationText.Text = obj.Recommendation;
        }
    }
}