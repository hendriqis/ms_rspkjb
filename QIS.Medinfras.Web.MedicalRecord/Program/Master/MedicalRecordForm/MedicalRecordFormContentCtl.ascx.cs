using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class MedicalRecordFormContentCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            IsAdd = false;
            string[] paramInfo = param.Split('|');
            hdnFormID.Value = paramInfo[0];
            txtFormInfo.Text = string.Format("{0} - {1}",paramInfo[1],paramInfo[2]);
            hdnFormContent.Value = paramInfo[3];

            txtContent.Text = HttpUtility.HtmlDecode(hdnFormContent.Value);
        }

        private void ControlToEntity(MedicalRecordForm entity)
        {
            entity.MedicalFormContent = Helper.GetHTMLEditorText(txtContent);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                MedicalRecordForm entity = BusinessLayer.GetMedicalRecordFormList(string.Format("FormID = {0}", hdnFormID.Value)).FirstOrDefault();
                ControlToEntity(entity);

                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMedicalRecordForm(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}