using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PhysiotherapyActionPlanCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnAppointmentID.Value = param;
            vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", hdnAppointmentID.Value)).FirstOrDefault();
            txtPatientName.Text = entity.PatientName;
            txtMedicalNo.Text = entity.MedicalNo;
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsDeleted = 0 AND IsActive = 1",
                                                                                                        Constant.StandardCode.CUSTOMER_TYPE //1
                                                                                                    ));
            Methods.SetComboBoxField<StandardCode>(cboAppointmentPayer, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.CUSTOMER_TYPE).OrderByDescending(t => t.StandardCodeID).ToList(), "StandardCodeName", "StandardCodeID");
            cboAppointmentPayer.SelectedIndex = 0;
            txtValueDateFrom.Attributes.Add("validationgroup", "mpTrxPopup");
            txtValueDateTo.Attributes.Add("validationgroup", "mpTrxPopup");
        }

        protected string GetCustomerFilterExpression()
        {
            return string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM CustomerContract WHERE '{0}' BETWEEN StartDate AND EndDate AND IsDeleted = 0)", DateTime.Now.ToString("yyyyMMdd"));
        }

        protected void cbpMedicalSickLeave_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}