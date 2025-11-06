using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Information
{
    public partial class DrugAlertInformationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramID = param.Split('|');
            hdnPrescriptionOrderIDCtlDrugAlert.Value = paramID[0].ToString();
            PrescriptionOrderHdInfo infoDao = BusinessLayer.GetPrescriptionOrderHdInfo(Convert.ToInt32(hdnPrescriptionOrderIDCtlDrugAlert.Value));

            string path = AppConfigManager.QISLibsPhysicalDirectory;
            path += string.Format("\\Scripts\\MIMS\\Mims.html");
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(infoDao.DrugAlertResultInfo1);
            }

            //if (infoDao != null)
            //{
            //    divInteraction.InnerHtml = infoDao.DrugAlertResultInfo;
            //    divDuplicate.InnerHtml = infoDao.DrugAlertResultInfo1;
            //}
            //else
            //{
            //    divInteraction.InnerHtml = "";
            //    divDuplicate.InnerHtml = "";
            //}

            if (paramID.Length > 1)
            {
                trButton.Style.Add("display", "none");
            }
        }
    }
}