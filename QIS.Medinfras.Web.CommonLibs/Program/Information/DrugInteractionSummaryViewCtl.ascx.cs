using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class DrugInteractionSummaryViewCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramID = param.Split('|');
            hdnID.Value = paramID[0].ToString();

            PrescriptionOrderHdInfo infoDao = BusinessLayer.GetPrescriptionOrderHdInfo(Convert.ToInt32(hdnID.Value));
            string path = AppConfigManager.QISLibsPhysicalDirectory;
            path += string.Format("\\Scripts\\MIMS\\Mims.html");
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(infoDao.DrugAlertResultInfo1);
            }

            //if (infoDao != null)
            //{
            //    //string data = Helper.ParsingHtmlRemoveLink(infoDao.DrugAlertResultInfo);
            //    divInteraction.InnerHtml = infoDao.DrugAlertResultInfo;
            //}
            //else
            //{
            //    divInteraction.InnerHtml = "";
            //}
        }
    }
}