using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Text;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InformasiKontrakInstansiCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramID = param.Split('|');
            hdnPayerID.Value = paramID[0].ToString();
            hdnContractID.Value = paramID[1].ToString();

            //CustomerContract entityCC = BusinessLayer.GetCustomerContract(Convert.ToInt32(hdnContractID.Value));
            CustomerContract entityCC = BusinessLayer.GetCustomerContractList(string.Format("ContractID = {0} AND BusinessPartnerID = {1}", hdnContractID.Value, hdnPayerID.Value)).FirstOrDefault();
        }
    }
}