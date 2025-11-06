using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class PrintSuratKontrolCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            vConsultVisit9 entity = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
            txtMRN.ReadOnly = true;
            txtPatientName.ReadOnly = true;
            txtMRN.Text = entity.MedicalNo;
            txtPatientName.Text = entity.PatientName;
            hdnMRN.Value = entity.MRN.ToString();
            BindGridView();
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvRegistrationBPJSList(string.Format("MRN = {0} AND NoSuratRencanaKontrolBerikutnya IS NOT NULL ORDER BY RegistrationID DESC", hdnMRN.Value));
            grdView.DataBind();
        }

        protected void cbpPatientVisitNotes_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}