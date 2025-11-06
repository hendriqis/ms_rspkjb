using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SuratRujukanPenunjangCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramData = param.Split('|');
            hdnVisitIDCtl.Value = paramData[0];
            hdnTestOrderIDCtl.Value = paramData[1];
            hdnTransactionHdIDCtl.Value = paramData[2];
            txtTransactionNoCtl.Value = paramData[3];
            Helper.SetControlEntrySetting(txtValueDate, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboSpecimen, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboBussinesPartnerTo, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            
            txtValueDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnVisitIDCtl.Value)).FirstOrDefault();

            if (entity != null)
            {
                txtMedicalNo.Text = entity.MedicalNo;
                txtPatientName.Text = entity.PatientName;
            }

            List<vTestOrderDt> lstTestOrderDt = BusinessLayer.GetvTestOrderDtList(string.Format("TestOrderID='{0}' AND BusinessPartnerID > 0 AND  IsDeleted=0", hdnTestOrderIDCtl.Value));
            List<vTestOrderDt> lstBussinesPartnerData = lstTestOrderDt
                  .GroupBy(p => p.BusinessPartnerID)
                  .Select(g => g.First())
                  .ToList();

            Methods.SetComboBoxField<vTestOrderDt>(cboBussinesPartnerTo, lstBussinesPartnerData, "BusinessPartnerName", "BusinessPartnerID");
            cboBussinesPartnerTo.SelectedIndex = 0;

            List<Specimen> lstSpecimen = BusinessLayer.GetSpecimenList(string.Format("IsDeleted=0"));
            Methods.SetComboBoxField<Specimen>(cboSpecimen, lstSpecimen, "SpecimenName", "SpecimenID");
            cboSpecimen.SelectedIndex = 0;

            bindData(); 
        }

        private void bindData()
        {
            if (cboBussinesPartnerTo.Value != null)
            {
                List<vTestOrderDt> lstTestOrderDt = BusinessLayer.GetvTestOrderDtList(string.Format("TestOrderID='{0}' AND BusinessPartnerID='{1}' AND IsDeleted=0", hdnTestOrderIDCtl.Value, cboBussinesPartnerTo.Value.ToString()));
                grdDataOrderDtView.DataSource = lstTestOrderDt;
            }
            else {
                
                grdDataOrderDtView.DataSource = null;
            }
           
            grdDataOrderDtView.DataBind();
        }
        protected void cbpMedicalSickLeave_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpDataOrderDtView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                bindData();
                result = "refresh|";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}