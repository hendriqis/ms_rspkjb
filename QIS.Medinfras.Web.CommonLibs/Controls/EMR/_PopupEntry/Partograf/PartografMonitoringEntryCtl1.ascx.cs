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
    public partial class PartografMonitoringEntryCtl1 : BaseEntryPopupCtl3
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "0")
            {
                IsAdd = false;
                SetControlProperties(paramInfo);
                vPartograf entity = BusinessLayer.GetvPartografList(string.Format("ID = {0}", hdnID.Value)).FirstOrDefault();
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
                        vPartograf entity = BusinessLayer.GetvPartografList(string.Format("ID = {0}", copyRecordID)).FirstOrDefault();
                        EntityToControl(entity);
                    }
                }
            }
        }

        private void SetControlProperties(string[] paramInfo)
        {
            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsActive = 1 AND IsDeleted = 0",
        Constant.StandardCode.AIR_KETUBAN,
        Constant.StandardCode.PENYUSUPAN,
        Constant.StandardCode.DURASI_KONTRAKSI,
        Constant.StandardCode.FREKUENSI_KONTRAKSI));
            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.AIR_KETUBAN).ToList();
            Methods.SetComboBoxField<StandardCode>(cboAirKetuban, lstCode1, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode2 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.PENYUSUPAN).ToList();
            Methods.SetComboBoxField<StandardCode>(cboPenyusupan, lstCode2, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode3 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.DURASI_KONTRAKSI).ToList();
            Methods.SetComboBoxField<StandardCode>(cboDurasiKontraksi, lstCode3, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstCode4 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.FREKUENSI_KONTRAKSI).ToList();
            Methods.SetComboBoxField<StandardCode>(cboFrekuensiKontraksi, lstCode4, "StandardCodeName", "StandardCodeID");

            hdnID.Value = paramInfo[0];
            hdnFetusID.Value = paramInfo[2];
            hdnLMPDate.Value = paramInfo[5];
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

            SetControlEntrySetting(txtDJJ, new ControlEntrySetting(true, true, false,string.Empty));
            SetControlEntrySetting(cboAirKetuban, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboPenyusupan, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPembukaan, new ControlEntrySetting(true, true, false, string.Empty));
            SetControlEntrySetting(rblTurunKepala, new ControlEntrySetting(true, true, false, "0"));
            SetControlEntrySetting(cboFrekuensiKontraksi, new ControlEntrySetting(true, true, false, string.Empty));
        }

        private void EntityToControl(vPartograf entity)
        {
            txtMeasurementDate.Text = entity.LogDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtMeasurementTime.Text = entity.LogTime;
            txtDJJ.Text = entity.DJJ == 0 ? string.Empty :  entity.DJJ.ToString();
            if (!string.IsNullOrEmpty(entity.GCAirKetuban))
            {
                cboAirKetuban.Value = entity.GCAirKetuban;
            }
            if (!string.IsNullOrEmpty(entity.GCPenyusupan))
            {
                cboPenyusupan.Value = entity.GCPenyusupan;
            }
            if (entity.Pembukaan != 0)
            {
                txtPembukaan.Text = entity.Pembukaan.ToString();
            }
            rblTurunKepala.SelectedValue = entity.TurunKepala ? "1" : "0";
            if (!string.IsNullOrEmpty(entity.GCTipeKontraksi))
            {
                cboDurasiKontraksi.Value = entity.GCTipeKontraksi;
            }
            if (!string.IsNullOrEmpty(entity.GCFrekuensiKontraksi))
            {
                cboFrekuensiKontraksi.Value = entity.GCFrekuensiKontraksi;
            }
        }

        private void ControlToEntity(Partograf entity)
        {
            entity.FetusID = Convert.ToInt32(hdnFetusID.Value);
            entity.LogDate = Helper.GetDatePickerValue(txtMeasurementDate);
            entity.LogTime = txtMeasurementTime.Text;
            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            if (!string.IsNullOrEmpty(txtDJJ.Text) && txtDJJ.Text != "0" && Methods.IsNumeric(txtDJJ.Text))
            {
                entity.DJJ = Convert.ToDecimal(txtDJJ.Text);
            }
            if (cboAirKetuban.Value != null)
            {
                entity.GCAirKetuban = cboAirKetuban.Value.ToString();
            }
            if (cboPenyusupan.Value  != null)
            {
                entity.GCPenyusupan = cboPenyusupan.Value.ToString();
            }
            if (!string.IsNullOrEmpty(txtPembukaan.Text) && txtPembukaan.Text != "0" && Methods.IsNumeric(txtPembukaan.Text))
            {
                entity.Pembukaan = Convert.ToInt16(txtPembukaan.Text);
            }
            entity.TurunKepala = rblTurunKepala.SelectedValue == "1" ? true : false;
            if (cboDurasiKontraksi.Value != null)
            {
                entity.GCTipeKontraksi = cboDurasiKontraksi.Value.ToString();
            }
            if (cboFrekuensiKontraksi.Value != null)
            {
                entity.GCFrekuensiKontraksi = cboFrekuensiKontraksi.Value.ToString();
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            try
            {
                Partograf entity = new Partograf();
                ControlToEntity(entity);

                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.CreatedDate = DateTime.Now;
                BusinessLayer.InsertPartograf(entity);
                retVal = hdnFetusID.Value;
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            try
            {
                Partograf entity = BusinessLayer.GetPartograf(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePartograf(entity);
                retVal = hdnFetusID.Value;
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