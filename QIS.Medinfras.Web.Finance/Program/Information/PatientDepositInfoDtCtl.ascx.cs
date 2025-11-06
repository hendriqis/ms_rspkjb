using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class PatientDepositInfoDtCtl : BaseViewPopupCtl
    {
        private PatientDepositInfo DetailPage
        {
            get { return (PatientDepositInfo)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            string[] lstParam = param.Split('|');
            hdnParamPeriode.Value = lstParam[0];
            hdnParamMRN.Value = lstParam[1];

            string[] lstPeriode = hdnParamPeriode.Value.Split(';');
            hdnParamPeriodeFrom.Value = lstPeriode[0];
            hdnParamPeriodeTo.Value = lstPeriode[1];

            Patient oPatient = BusinessLayer.GetPatient(Convert.ToInt32(hdnParamMRN.Value));
            txtMedicalNoDt.Text = oPatient.MedicalNo;
            txtPatientNameDt.Text = oPatient.FullName;

            BindGridView();
        }

        private void BindGridView()
        {
            string filterPeriode = string.Format("{0}|{1}",
                                        Helper.GetDatePickerValue(hdnParamPeriodeFrom.Value).ToString(Constant.FormatString.DATE_FORMAT_112),
                                        Helper.GetDatePickerValue(hdnParamPeriodeTo.Value).ToString(Constant.FormatString.DATE_FORMAT_112));

            List<GetPatientDepositInfoDetail> lstEntity = BusinessLayer.GetPatientDepositInfoDetailList(filterPeriode, txtPatientNameDt.Text);
            grdPopupView.DataSource = lstEntity;
            grdPopupView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdPopupView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GetPatientDepositInfoDetail oData = e.Row.DataItem as GetPatientDepositInfoDetail;

                if (oData.GCPaymentMethod == "")
                {
                    e.Row.BackColor = Color.LightPink;
                }
            } 
        }
    }
}