using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    public partial class PrescriptionOrderEntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.HealthcareServiceUnitID));
            hdnDefaultDispensaryServiceUnitID.Value = lstHealthcareServiceUnit.FirstOrDefault().DispensaryServiceUnitID.ToString();

            cboDispensaryUnit.Value = hdnDefaultDispensaryServiceUnitID.Value;

            if (param != "")
            {
                IsAdd = false;
                hdnID.Value = param;
                vPrescriptionOrderHd entity = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("PrescriptionOrderID = {0} AND GCTransactionStatus <> '{1}'", Convert.ToInt32(param),Constant.TransactionStatus.VOID)).FirstOrDefault();
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
                SetControlProperties();

                vParamedicMaster pm = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", AppSession.RegisteredPatient.ParamedicID)).FirstOrDefault();
                hdnPhysicianID.Value = pm.ParamedicID.ToString();
                txtPhysicianCode.Text = pm.ParamedicCode;
                txtPhysicianName.Text = pm.ParamedicName;
            }
        }

        protected void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.REFILL_INSTRUCTION,Constant.StandardCode.PRESCRIPTION_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboRefillInstruction, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboRefillInstruction.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboPrescriptionType.SelectedIndex = 0;

            List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboDispensaryUnit, lstHealthcareServiceUnit.Where(x => x.DepartmentID == "PHARMACY").ToList(), "ServiceUnitName", "HealthcareServiceUnitID");
            if (cboDispensaryUnit.Value == null)
                cboDispensaryUnit.Value = hdnDefaultDispensaryServiceUnitID.Value;

            cboDispensaryUnit.ClientEnabled = false;

            txtPhysicianCode.Enabled = false;
            BindCboLocation();
        }

        protected void cboLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
        }

        private void BindCboLocation()
        {
            Location location = BusinessLayer.GetLocationList(string.Format("LocationID IN (SELECT LocationID FROM HealthcareServiceUnit WHERE HealthcareServiceUnitID = {0})", cboDispensaryUnit.Value)).FirstOrDefault();

            if (location != null)
            {
                int locationID = location.LocationID;
                Location loc = BusinessLayer.GetLocation(locationID);
                List<Location> lstLocation = null;
                if (loc.IsHeader)
                    lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                else
                {
                    lstLocation = new List<Location>();
                    lstLocation.Add(loc);
                }
                Methods.SetComboBoxField<Location>(cboLocation, lstLocation, "LocationName", "LocationID");
                cboLocation.SelectedIndex = 0;
                cboLocation.ClientEnabled = false;
            }
        }


        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtPrescriptionOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPrescriptionDate, new ControlEntrySetting(true, true, false, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtPrescriptionTime, new ControlEntrySetting(true, true, false, Constant.DefaultValueEntry.TIME_NOW));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboRefillInstruction, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(vPrescriptionOrderHd entity)
        {
            hdnID.Value = entity.PrescriptionOrderID.ToString();
            txtPrescriptionOrderNo.Text = entity.PrescriptionOrderNo;
            txtPrescriptionDate.Text = entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entity.PrescriptionTime;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            cboPrescriptionType.Value = entity.GCPrescriptionType;
            cboRefillInstruction.Value = entity.GCRefillInstruction;
            txtNotes.Text = entity.Remarks;
            cboDispensaryUnit.Value = entity.DispensaryServiceUnitID.ToString();
            cboLocation.Value = entity.LocationID.ToString();
        }

        private void ControlToEntity(PrescriptionOrderHd entityHd)
        {
            entityHd.PrescriptionDate = Helper.GetDatePickerValue(txtPrescriptionDate.Text);
            entityHd.PrescriptionTime = txtPrescriptionTime.Text;
            entityHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
            entityHd.GCRefillInstruction = cboRefillInstruction.Value.ToString();
            entityHd.GCPrescriptionType = cboPrescriptionType.Value.ToString();
            entityHd.DispensaryServiceUnitID = Convert.ToInt32(cboDispensaryUnit.Value);
            entityHd.LocationID = Convert.ToInt32(cboLocation.Value);
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PrescriptionOrderHd entityHd = new PrescriptionOrderHd();
                ControlToEntity(entityHd);
                entityHd.VisitID = Convert.ToInt32(AppSession.RegisteredPatient.VisitID);
                entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entityHd.ClassID = Convert.ToInt32(AppSession.RegisteredPatient.ClassID);
                switch (AppSession.RegisteredPatient.DepartmentID)
                {
                    case Constant.Facility.EMERGENCY:
                        entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                        break;
                    case Constant.Facility.OUTPATIENT:
                        entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                        break;
                    case Constant.Facility.INPATIENT:
                        entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                        break;
                    default:
                        entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                        break;
                }
                entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate);
                entityHd.GCPrescriptionType = Constant.PrescriptionType.MEDICATION_ORDER;
                entityHd.GCOrderStatus = Constant.TestOrderStatus.OPEN;
                entityHd.Remarks = txtNotes.Text;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPrescriptionOrderHd(entityHd);
                
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                PrescriptionOrderHd entityHd = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnID.Value));
                
                ControlToEntity(entityHd);
                entityHd.VisitID = Convert.ToInt32(AppSession.RegisteredPatient.VisitID);
                entityHd.ClassID = Convert.ToInt32(AppSession.RegisteredPatient.ClassID);
                entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate);
                entityHd.Remarks = txtNotes.Text;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                
                BusinessLayer.UpdatePrescriptionOrderHd(entityHd);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}