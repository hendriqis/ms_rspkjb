using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class UpdatePhysicianTextCtl1 : BasePagePatientPageEntryCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            IsAdd = true;
            String filterExpression = string.Format("ParentID IN ('{0}')", Constant.StandardCode.PHYSICIAN_TEXT_TEMPLATE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboGCTemplateGroup, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboGCTemplateGroup.Value = paramInfo[0];
            txtTemplateText.Text = paramInfo[1];
            cboGCTemplateGroup.Enabled = false;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }

        private void ControlToEntity(PhysicianText entity)
        {
            entity.GCTextTemplateGroup = cboGCTemplateGroup.Value.ToString();
            entity.UserID = AppSession.UserLogin.UserID;
            entity.TemplateCode = txtTemplateCode.Text;
            entity.TemplateText = txtTemplateText.Text;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTemplateCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTemplateText, new ControlEntrySetting(true, true, true));
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PhysicianText entity = new PhysicianText();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPhysicianText(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }
    }
}