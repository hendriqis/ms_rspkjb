using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs;

namespace QIS.Medinfras.Web.Module.Program.Outpatient
{
    public partial class Registration : BasePageEntry
    {

        protected override void InitializeDataControl()
        {
            
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNoAppointment, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtJamAppointment, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtTanggalRegistrasi, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtJamRegistrasi, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.TIME_NOW));
            SetControlEntrySetting(txtNoRegistrasi, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtNoRM, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtGelar, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtNama, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtMarga, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtKdSeks, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTanggalLahir, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtUmurTahun, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtUmurBulan, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtUmurHari, new ControlEntrySetting(false, false, false));
        }

        protected override string OnGetMenuCode()
        {
            return "RJ001";
        }

        protected override int OnGetRowCount()
        {
            return BusinessLayer.Getvri_regRowCount("");
        }

        #region Load Entity
        protected override void OnLoadEntity(int PageIndex)
        {
            vri_reg entity = BusinessLayer.Getvri_reg("", PageIndex, "tglmasuk DESC");
            EntityToControl(entity);
        }

        private void EntityToControl(vri_reg entity)
        {
            txtTanggalRegistrasi.Text = entity.tglmasukinstring;
            txtJamRegistrasi.Text = entity.jammasuk;
            txtNoRegistrasi.Text = entity.noreg;
            txtNoRM.Text = entity.norm;
            txtNama.Text = entity.nama;
            txtMarga.Text = entity.marga;
            txtKdSeks.Text = entity.kdseks;
        }
        #endregion

        #region Save Entity
        private void ControlToEntity(ri_reg entity)
        {
            entity.noreg = txtNoRegistrasi.Text;
            entity.norm = txtNoRM.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ri_reg entity = new ri_reg();
                ControlToEntity(entity);
                return true;
            }
            catch(Exception ex) 
            {
                errMessage = ex.Message;
                return false;
            }
        }
        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                ri_reg entity = new ri_reg();
                ControlToEntity(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion

        #region Delete Entity
        protected override bool OnDeleteRecord(ref string errMessage)
        {
            return true;
        }
        #endregion
    }
}