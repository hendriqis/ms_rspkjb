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
    public partial class ContractSummaryViewCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramID = param.Split('|');
            hdnPayerID.Value = paramID[0].ToString();
            hdnContractID.Value = paramID[1].ToString();

            if (hdnPayerID.Value != "1")
            {
                CustomerContract entity = BusinessLayer.GetCustomerContract(Convert.ToInt32(hdnContractID.Value));
                divContractSummary.InnerHtml = entity.ContractSummary;
            }
            else
            {
                divContractSummary.InnerText = "Tidak ada informasi instansi karena penjamin bayar PRIBADI";
            }
        }
    }
}