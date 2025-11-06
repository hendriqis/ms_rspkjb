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
using System.Text;
using System.Globalization;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class ChangeMedicationSignaCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            this.PopupTitle = "Ubah Aturan Pemberian";
            string[] paramInfo = param.Split('|');
            hdnVisitID.Value = paramInfo[0];
            hdnSelectedID.Value = paramInfo[1];
            txtChangeInstructionDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtChangeInstructionTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtDefaultStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            SetControlProperties();
        }

        protected void cbpPopupChangeProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            string errorMessage = string.Empty;

            string instructionText = txtChangeInstructionText.Text;

            if (!string.IsNullOrEmpty(instructionText))
            {
                PatientInstruction entity = new PatientInstruction();
                entity.InstructionDate = Helper.GetDatePickerValue(txtChangeInstructionDate);
                entity.InstructionTime = txtChangeInstructionTime.Text;
                entity.GCInstructionGroup = Constant.PatientInstructionGroup.MEDICATION_ORDER_CHANGE_SIGNA;
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.PhysicianID = Convert.ToInt32(cboChangePhysician.Value);
                entity.Description = instructionText;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPatientInstruction(entity);

                string message = string.Format("Instruksi Dokter telah berhasil dibuat");
                result = string.Format("process|1|{0}", message);
            }
            else
            {
                string message = string.Format("Terjadi kesalahan pada saat pembuatan instruksi dokter.");
                result = string.Format("process|1|{0}", message);
            }

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }


        private void SetControlProperties()
        {
            //Load Physician
            string filterExpression = string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID);
            List<vParamedicTeam> lstPhysician = BusinessLayer.GetvParamedicTeamList(filterExpression);
            Methods.SetComboBoxField(cboChangePhysician, lstPhysician, "ParamedicName", "ParamedicID");
            cboChangePhysician.Value = AppSession.UserLogin.ParamedicID.ToString();
            cboChangePhysician.Enabled = false;

            BindGridView();
        }

        protected void lvwChangeView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPrescriptionOrderDt1 item = (vPrescriptionOrderDt1)e.Item.DataItem;
                TextBox txtStartDate = e.Item.FindControl("txtChangeStartDate") as TextBox;
                txtStartDate.Text = Helper.GetDatePickerValue(txtChangeInstructionDate.Text).AddDays(1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("VisitID = {0} AND PrescriptionOrderDetailID IN ({1})", hdnVisitID.Value, hdnSelectedID.Value);
            List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);
            lvwChangeView .DataSource = lstEntity;
            lvwChangeView.DataBind();
        }

        protected void cbpViewChange_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}