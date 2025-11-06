using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class FetalMeasurementEntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "0")
            {
                IsAdd = false;
                SetControlProperties(paramInfo);
                FetalMeasurement entity = BusinessLayer.GetFetalMeasurement(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
                SetControlProperties(paramInfo);
                if (paramInfo.Length >= 7)
                {
                    //copy mode
                    if (!string.IsNullOrEmpty(paramInfo[6]))
                    {
                        int copyRecordID = Convert.ToInt32(paramInfo[6]);
                        FetalMeasurement entity = BusinessLayer.GetFetalMeasurement(Convert.ToInt32(copyRecordID));
                        EntityToControl(entity);
                    }
                }
            }
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnID.Value = paramInfo[0];
            hdnFetusID.Value = paramInfo[2];
            hdnLMPDate.Value = paramInfo[5];
            txtLMP.Text = paramInfo[5];
            txtPregnancyNo.Text = paramInfo[1];
            txtFetusNo.Text = paramInfo[3];
        }

        protected override void OnControlEntrySetting()
        {
            if (AppSession.RegisteredPatient.DepartmentID != Constant.Facility.INPATIENT)
            {
                SetControlEntrySetting(txtMeasurementDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                SetControlEntrySetting(txtMeasurementTime, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitTime));
            }
            else
            {
                SetControlEntrySetting(txtMeasurementDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                SetControlEntrySetting(txtMeasurementTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            }

            SetControlEntrySetting(txtBPD, new ControlEntrySetting(true, true, true,"0"));
            SetControlEntrySetting(txtAC, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtHC, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtHL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtFL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtCRL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtOFD, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtEFW, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtGS, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtFHR, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));

        }

        private void EntityToControl(FetalMeasurement entity)
        {
            txtMeasurementDate.Text = entity.MeasurementDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtMeasurementTime.Text = entity.MeasurementTime;
            txtGestationalWeek.Text = entity.GestationalWeek.ToString();
            txtAC.Text = entity.AC.ToString();
            txtBPD.Text = entity.BPD.ToString();
            txtHL.Text = entity.HL.ToString();
            txtHC.Text = entity.HC.ToString();
            txtFL.Text = entity.FL.ToString();
            txtEFW.Text = entity.EFW.ToString();
            txtOFD.Text = entity.OFD.ToString();
            txtCRL.Text = entity.CRL.ToString();
            txtFHR.Text = entity.FHR.ToString();
            txtGS.Text = entity.GS.ToString();
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(FetalMeasurement entity)
        {
            entity.FetusID = Convert.ToInt32(hdnFetusID.Value);
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.MeasurementDate = Helper.GetDatePickerValue(txtMeasurementDate);
            entity.MeasurementTime = txtMeasurementTime.Text;
            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            entity.GestationalWeek = Convert.ToInt16(Request.Form[txtGestationalWeek.UniqueID]);
            entity.AC = Convert.ToDecimal(txtAC.Text);
            entity.BPD = Convert.ToDecimal(txtBPD.Text);
            entity.HL = Convert.ToDecimal(txtHL.Text);
            entity.HC = Convert.ToDecimal(txtHC.Text);
            entity.FL = Convert.ToDecimal(txtFL.Text);
            entity.EFW = Convert.ToDecimal(txtEFW.Text);
            entity.OFD = Convert.ToDecimal(txtOFD.Text);
            entity.CRL = Convert.ToDecimal(txtCRL.Text);
            entity.FHR = Convert.ToDecimal(txtFHR.Text);
            entity.GS = Convert.ToDecimal(txtGS.Text);
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                FetalMeasurement entity = new FetalMeasurement();
                ControlToEntity(entity);

                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.CreatedDate = DateTime.Now;
                BusinessLayer.InsertFetalMeasurement(entity);
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
                FetalMeasurement entity = BusinessLayer.GetFetalMeasurement(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateFetalMeasurement(entity);
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