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
using System.IO;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InformationContractDocumentPerBussinesPartnetCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnBusinessPartnerID.Value = param;
            vBusinessPartners bp = BusinessLayer.GetvBusinessPartnersList(string.Format("BusinessPartnersID = {0}", hdnBusinessPartnerID.Value)).FirstOrDefault();
            txtBussinesPartnerName.Text = bp.BusinessPartnersName;
            
            
          /////  BusinessPartners bp = BusinessLayer.GetBusinessPartners(customercontract.BusinessPartnerID);

            string path = string.Format("{0}BusinessPartner\\{1}\\", AppConfigManager.QISVirtualDirectory.Replace('/', '\\'), bp.BusinessPartnersCode); ;
            hdnVirtualDirectory.Value = path;

            BindGridView();    
        }

         

        private void BindGridView()
        {
            
            grdView.DataSource = BusinessLayer.GetvContractDocumentList(string.Format("BusinessPartnerID = {0} AND IsDeleted = 0", hdnBusinessPartnerID.Value));
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

           
            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;

        }

          
        
    }
}