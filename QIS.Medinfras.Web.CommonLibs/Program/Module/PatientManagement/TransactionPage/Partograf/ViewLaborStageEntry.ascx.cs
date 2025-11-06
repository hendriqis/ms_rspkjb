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
    public partial class ViewLaborStageEntry : BaseViewPopupCtl
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
            hdnAntenatalRecordID.Value = paramInfo[1];
            hdnStage.Value = paramInfo[2];

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "GCParamedicMasterType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0",
                Constant.ParamedicType.Nurse, Constant.ParamedicType.Physician, Constant.ParamedicType.Bidan, Constant.ParamedicType.AsistenLuar));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Nurse));
            }

            if (!string.IsNullOrEmpty(paramInfo[3]))
            {
                txtLaborStageDate.Text = paramInfo[3];
                txtLaborStageTime.Text = paramInfo[4];
            }

            txtParamedicInfo.Text = paramInfo[5];
            hdnDivHTML.Value = paramInfo[6];
            hdnFormValues.Value = paramInfo[7];
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}