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
    public partial class InfoTransactionDetailEKlaimParameterFromBillingCtl : BaseContentPopupCtl
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
            List<GetPatientChargesHdDtEKlaimParameterDetailPerBilling> lstEntity = BusinessLayer.GetPatientChargesHdDtEKlaimParameterDetailPerBillingList(Convert.ToInt32(hdnRegistrationID.Value));
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            txtTotalPatientAmount.Text = lstEntity.Sum(a => a.PatientAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalPayerAmount.Text = lstEntity.Sum(a => a.PayerAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalLineAmount.Text = lstEntity.Sum(a => a.LineAmount).ToString(Constant.FormatString.NUMERIC_2);

            txtTotalPatientAmount.Text = lstEntity.Sum(a => a.PatientAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalPayerAmount.Text = lstEntity.Sum(a => a.PayerAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalLineAmount.Text = lstEntity.Sum(a => a.LineAmount).ToString(Constant.FormatString.NUMERIC_2);

            txtTotalPatientObatDitagihkanAmount.Text = lstEntity.Where(a => a.EKlaimParameterName == "Obat (Ditagihkan)" && a.EKlaimParameterCode == "EK013").ToList().Sum(a => a.PatientAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalPayerObatDitagihkanAmount.Text = lstEntity.Where(a => a.EKlaimParameterName == "Obat (Ditagihkan)" && a.EKlaimParameterCode == "EK013").ToList().Sum(a => a.PayerAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalLineObatDitagihkanAmount.Text = lstEntity.Where(a => a.EKlaimParameterName == "Obat (Ditagihkan)" && a.EKlaimParameterCode == "EK013").ToList().Sum(a => a.LineAmount).ToString(Constant.FormatString.NUMERIC_2);

            txtTotalPatientTanpaObatDitagihkanAmount.Text = lstEntity.Where(a => a.EKlaimParameterName != "Obat (Ditagihkan)").ToList().Sum(a => a.PatientAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalPayerTanpaObatDitagihkanAmount.Text = lstEntity.Where(a => a.EKlaimParameterName != "Obat (Ditagihkan)").ToList().Sum(a => a.PayerAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalLineTanpaObatDitagihkanAmount.Text = lstEntity.Where(a => a.EKlaimParameterName != "Obat (Ditagihkan)").ToList().Sum(a => a.LineAmount).ToString(Constant.FormatString.NUMERIC_2);
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                GetPatientChargesHdDtEKlaimParameterDetailPerBilling entity = e.Item.DataItem as GetPatientChargesHdDtEKlaimParameterDetailPerBilling;

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