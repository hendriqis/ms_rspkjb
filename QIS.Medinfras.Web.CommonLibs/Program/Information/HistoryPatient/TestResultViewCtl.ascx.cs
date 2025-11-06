using System;
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
    public partial class TestResultViewCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] splitparam = param.Split('|');
            int transactionID = Convert.ToInt32(splitparam[0]);
            int itemID = Convert.ToInt32(splitparam[1]);
            string language = splitparam[2].ToString();

            string paramHD = string.Format("ChargeTransactionID = {0} AND IsDeleted = 0", transactionID);
            ImagingResultHd entityHd = BusinessLayer.GetImagingResultHdList(paramHD).FirstOrDefault();

            if (entityHd != null)
            {
                if (language == "IND")
                {
                    ImagingResultDt entity = BusinessLayer.GetImagingResultDt(entityHd.ID, itemID);
                    divResultSummary.InnerHtml = entity.TestResult1;
                }
                else
                {
                    ImagingResultDt entity = BusinessLayer.GetImagingResultDt(entityHd.ID, itemID);
                    divResultSummary.InnerHtml = entity.TestResult2;
                }
            }
        }
    }
}