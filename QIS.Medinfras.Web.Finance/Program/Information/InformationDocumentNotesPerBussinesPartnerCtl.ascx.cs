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
    public partial class InformationDocumentNotesPerBussinesPartnerCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnBusinessPartnerID.Value = param;
            vBusinessPartners bp = BusinessLayer.GetvBusinessPartnersList(string.Format("BusinessPartnersID = {0}", hdnBusinessPartnerID.Value)).FirstOrDefault();
            txtBussinesPartnerName.Text = bp.BusinessPartnersName;
       
            BindGridView();    
        }
        private void BindGridView()
        {
            List<vDocumentLog> lst  = BusinessLayer.GetvDocumentLogList(string.Format("DocumentID IN (SELECT ContractID from vContractCoverage where BusinessPartnerID='{0}' ) AND GCDocumentType = '{1}' AND IsDeleted = 0", hdnBusinessPartnerID.Value, Constant.DocumentNoteType.CUSTOMER_CONTRACT));
            grdView.DataSource = lst;
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