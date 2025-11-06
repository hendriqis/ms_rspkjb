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

namespace QIS.Medinfras.Web.CommonLibs.Information
{
    public partial class InfoDocumentCustomerCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            //string[] paramID = param[0].Split('|');
            string[] paramID = param.Split('|');
            hdnPayerID.Value = paramID[0].ToString();
            hdnContractID.Value = paramID[1].ToString();

            if (hdnPayerID.Value != "1")
            {
                getListIDCustomerDocument();
            }
            //else
            //{
            //    containerInfoDocumentContent.InnerText = "Tidak ada informasi instansi karena penjamin bayar PRIBADI";
            //}

        }
        public void getListIDCustomerDocument()
        {
            string lstID;
            string filterExpression = String.Format("BusinessPartnerID = {0} AND ContractID = {1} AND IsDeleted = 0", hdnPayerID.Value, hdnContractID.Value);
            //List<CustomerContract> lstBusinessPartnerID = BusinessLayer.GetCustomerContractList(filterExpression);
            List<Int32> lstBusinessPartnerID = BusinessLayer.GetvContractDocumentListIdx(filterExpression);

            String CustomerID = "";
            StringBuilder sb = new StringBuilder();
            foreach (Int32 p in lstBusinessPartnerID)
            {
                //sb.Append(p);
                //sb.Append('|');
                if (string.IsNullOrEmpty(CustomerID))
                {
                    //CustomerID = Convert.ToString(p.ContractID);
                    sb.Append(p);
                    sb.Append('|');
                }
                else
                {
                    //CustomerID += ", " + Convert.ToString(p.ContractID);
                }
            }

            lstID = sb.ToString();
            //hdnContractID.Value = CustomerID;
            hdnContractID.Value = lstID.Substring(0, (lstID.Length > 0 ? lstID.Length - 1 : lstID.Length));
        }
    }
}