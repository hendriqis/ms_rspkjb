using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PrescriptionOrderChangesLogCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] localParam = param.Split('|');
                string registrationID = localParam[0];
                hdnPrescriptionOrderID.Value = localParam[1];
                hdnHealthcareServiceUnitIDCtl.Value = localParam[2];
                vPrescriptionOrderHd orderHd = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("PrescriptionOrderID = {0}", hdnPrescriptionOrderID.Value)).FirstOrDefault();
                if (orderHd  != null)
                {
                    txtMRN.ReadOnly = true;
                    txtPatientName.ReadOnly = true;
                    if (orderHd.MedicalNo == null || orderHd.MedicalNo == "")
                    {
                        txtMRN.Text = orderHd.GuestNo;
                    }
                    else
                    {
                        txtMRN.Text = orderHd.MedicalNo;
                    }
                    txtPatientName.Text = orderHd.PatientName;
                    hdnVisitID.Value = orderHd.VisitID.ToString();
                    txtPrescriptionOrderDate.Text = orderHd.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtPrescriptionOrderNo.Text = orderHd.PrescriptionOrderNo;
                    txtPrescriptionOrderTime.Text = orderHd.PrescriptionTime;
                    hdnPrescriptionOrderPhysicianID.Value = orderHd.ParamedicID.ToString();
                    txtParamedic.Text = orderHd.ParamedicName;
                    BindGridView();
                    SetControlProperties();
                }
            } 
        }

        private void SetControlProperties()
        {
            txtLogDate.Attributes.Add("validationgroup", "mpLogNotes");
            txtLogTime.Attributes.Add("validationgroup", "mpLogNotes");
            txtNoteText.Attributes.Add("validationgroup", "mpLogNotes");
            txtLogDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtLogTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        }

        private void BindGridView()
        {
            grdVisitNotes.DataSource = BusinessLayer.GetvPrescriptionOrderChangesLogList(string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", hdnPrescriptionOrderID.Value));
            grdVisitNotes.DataBind();
        }

        protected void cbpLogNotes_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnLogID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else
            {
                result = "delete|";
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PrescriptionOrderChangesLog entity)
        {
            entity.LogDate = Helper.GetDatePickerValue(txtLogDate);
            entity.LogTime = txtLogTime.Text;
            entity.NoteText = txtNoteText.Text;
            entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitIDCtl.Value);
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.PrescriptionOrderID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
            entity.IsNeedConfirmation = true;
            entity.ConfirmationPhysicianID = Convert.ToInt32(hdnPrescriptionOrderPhysicianID.Value);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PrescriptionOrderChangesLog entity = new PrescriptionOrderChangesLog();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPrescriptionOrderChangesLog(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PrescriptionOrderChangesLog entity = BusinessLayer.GetPrescriptionOrderChangesLog(Convert.ToInt32(hdnLogID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePrescriptionOrderChangesLog(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PrescriptionOrderChangesLog entity = BusinessLayer.GetPrescriptionOrderChangesLog(Convert.ToInt32(hdnLogID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePrescriptionOrderChangesLog(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }
    }
}