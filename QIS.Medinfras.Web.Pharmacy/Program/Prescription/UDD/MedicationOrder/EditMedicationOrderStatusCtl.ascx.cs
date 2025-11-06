using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class EditMedicationOrderStatusCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            this.PopupTitle = "Medication Order Status";

            SetControlProperties();

            if (!string.IsNullOrEmpty(param))
            {
                hdnSelectedID.Value = param;
                int prescriptionOrderID = Convert.ToInt32(hdnSelectedID.Value);
                string filterExp = string.Format("PrescriptionOrderID = {0}",prescriptionOrderID);
                vPrescriptionOrderHd1 orderHd = BusinessLayer.GetvPrescriptionOrderHd1List(filterExp).FirstOrDefault();
                if (orderHd != null)
                {
                    txtMedicationDate.Text = orderHd.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtMedicationTime.Text = orderHd.PrescriptionTime;
                    txtPrescriptionOrderNo.Text = orderHd.PrescriptionOrderNo;
                    txtParamedicName.Text = orderHd.ParamedicName;
                }
            }
        }

        private void SetControlProperties()
        {
            txtCompleteDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtCompleteTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string recordID = hdnSelectedID.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            result = UpdateMedicationOrder(recordID);

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string UpdateMedicationOrder(string orderID)
        {
            string result = string.Empty;
            try
            {
                string validationErrMsg = string.Empty;
                if (IsValid(ref validationErrMsg))
                {
                    PrescriptionOrderHd orderHd = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(orderID));
                    if (orderHd != null)
                    {
                        if (orderHd.GCOrderStatus == Constant.OrderStatus.IN_PROGRESS)
                        {
                            orderHd.GCOrderStatus = Constant.OrderStatus.COMPLETED;
                            orderHd.CompleteDate = Helper.GetDatePickerValue(txtCompleteDate);
                            orderHd.CompleteTime = txtCompleteTime.Text;
                            orderHd.CompleteByName = AppSession.UserLogin.UserFullName;
                            BusinessLayer.UpdatePrescriptionOrderHd(orderHd);
                        }
                    }
                    else
                    {
                        string message = string.Format("Invalid Medication Order ID {0}", orderID);
                        result = string.Format("process|0|{0}||", message);
                    }
                    result = string.Format("process|1|||");
                }
                else
                {
                    string message = string.Format("Validation Error for Medication Order ID {0} : {1}", orderID, validationErrMsg);
                    result = string.Format("process|0|{0}||", message);
                }
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}||", ex.Message);
            }
            return result;
        }

        private bool IsValid(ref string errMessage)
        {
            if (string.IsNullOrEmpty(txtCompleteDate.Text))
            {
                errMessage = "Complete Date must be entried";
                return false;
            }

            if (string.IsNullOrEmpty(txtCompleteTime.Text) || txtCompleteTime.Text == "__:__")
            {
                errMessage = "Complete Time must be entried";
                return false;
            }
            else
            {
                Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                if (!reg.IsMatch(txtCompleteTime.Text))
                {
                    errMessage = "Complete time must be entried in correct format (hh:mm)";
                    return false;
                }
            } 

            return true;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }
    }
}