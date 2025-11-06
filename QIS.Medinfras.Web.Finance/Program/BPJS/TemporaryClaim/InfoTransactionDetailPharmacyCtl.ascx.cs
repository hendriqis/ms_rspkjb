using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InfoTransactionDetailPharmacyCtl : BaseContentPopupCtl
    {
        protected int PageCount = 1;

        private TemporaryClaim DetailPage
        {
            get { return (TemporaryClaim)Page; }
        }

        public override void InitializeControl(string param)
        {
            hdnRegistrationID.Value = param;

            string filterExpression = string.Format("RegistrationID = '{0}'", hdnRegistrationID.Value);
            vConsultVisitCasemix entity = BusinessLayer.GetvConsultVisitCasemixList(filterExpression).FirstOrDefault();

            txtRegistrationNo.Text = string.Format("{0}", entity.RegistrationNo);
            txtSEPNo.Text = string.Format("{0}", entity.NoSEP);
            txtPatient.Text = string.Format("({0}) {1}", entity.MedicalNo, entity.PatientName);

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            List<GetPharmacyChargesBPJSTransactionType> lstEntity = BusinessLayer.GetPharmacyChargesBPJSTransactionTypeList(Convert.ToInt32(hdnRegistrationID.Value));
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                GetPharmacyChargesBPJSTransactionType entity = e.Item.DataItem as GetPharmacyChargesBPJSTransactionType;

            }
        }
        #endregion

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string param = e.Parameter;
            string result = param + "|";
            string retval = "";

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }
    }
}