using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class RekapPasienMasuk : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.BRIDGING_SIRANAP_REKAPPASIENMASUK;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        private GetUserMenuAccess menu;
        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            txtObservasiDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        private void LoadParameterValues()
        {
            String observasiDate = Helper.GetDatePickerValue(txtObservasiDate).ToString(Constant.FormatString.DATE_FORMAT_112);

            GetRekapPasienMasukSiranap entity = BusinessLayer.GetRekapPasienMasukSiranapList(observasiDate).FirstOrDefault();
            if (entity == null)
            {
                txtIGDSuspectL.Text = "0";
                txtIGDSuspectP.Text = "0";
                txtIGDConfirmL.Text = "0";
                txtIGDConfirmP.Text = "0";
                txtRJSuspectL.Text = "0";
                txtRJSuspectP.Text = "0";
                txtRJConfirmL.Text = "0";
                txtRJConfirmP.Text = "0";
                txtRISuspectL.Text = "0";
                txtRISuspectP.Text = "0";
                txtRIConfirmL.Text = "0";
                txtRIConfirmP.Text = "0";
            }
            else
            {
                txtIGDSuspectL.Text = entity.igd_suspect_l.ToString("G29");
                txtIGDSuspectP.Text = entity.igd_suspect_p.ToString("G29");
                txtIGDConfirmL.Text = entity.igd_confirm_l.ToString("G29");
                txtIGDConfirmP.Text = entity.igd_confirm_p.ToString("G29");
                txtRJSuspectL.Text = entity.rj_suspect_l.ToString("G29");
                txtRJSuspectP.Text = entity.rj_suspect_p.ToString("G29");
                txtRJConfirmL.Text = entity.rj_confirm_l.ToString("G29");
                txtRJConfirmP.Text = entity.rj_confirm_p.ToString("G29");
                txtRISuspectL.Text = entity.ri_suspect_l.ToString("G29");
                txtRISuspectP.Text = entity.ri_suspect_p.ToString("G29");
                txtRIConfirmL.Text = entity.ri_confirm_l.ToString("G29");
                txtRIConfirmP.Text = entity.ri_confirm_p.ToString("G29");
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtIGDSuspectL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtIGDSuspectP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtIGDConfirmL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtIGDConfirmP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtRJSuspectL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtRJSuspectP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtRJConfirmL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtRJConfirmP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtRISuspectL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtRISuspectP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtRIConfirmL, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtRIConfirmP, new ControlEntrySetting(true, true, true, "0"));

            LoadParameterValues();
            IsLoadFirstRecord = true;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;

            if (type == "refresh")
            {
                #region refresh
                String observasiDate = Helper.GetDatePickerValue(txtObservasiDate).ToString(Constant.FormatString.DATE_FORMAT_112);

                GetRekapPasienMasukSiranap entity = BusinessLayer.GetRekapPasienMasukSiranapList(observasiDate).FirstOrDefault();
                if (entity == null)
                {
                    txtIGDSuspectL.Text = "0";
                    txtIGDSuspectP.Text = "0";
                    txtIGDConfirmL.Text = "0";
                    txtIGDConfirmP.Text = "0";
                    txtRJSuspectL.Text = "0";
                    txtRJSuspectP.Text = "0";
                    txtRJConfirmL.Text = "0";
                    txtRJConfirmP.Text = "0";
                    txtRISuspectL.Text = "0";
                    txtRISuspectP.Text = "0";
                    txtRIConfirmL.Text = "0";
                    txtRIConfirmP.Text = "0";
                }
                else
                {
                    txtIGDSuspectL.Text = entity.igd_suspect_l.ToString("G29");
                    txtIGDSuspectP.Text = entity.igd_suspect_p.ToString("G29");
                    txtIGDConfirmL.Text = entity.igd_confirm_l.ToString("G29");
                    txtIGDConfirmP.Text = entity.igd_confirm_p.ToString("G29");
                    txtRJSuspectL.Text = entity.rj_suspect_l.ToString("G29");
                    txtRJSuspectP.Text = entity.rj_suspect_p.ToString("G29");
                    txtRJConfirmL.Text = entity.rj_confirm_l.ToString("G29");
                    txtRJConfirmP.Text = entity.rj_confirm_p.ToString("G29");
                    txtRISuspectL.Text = entity.ri_suspect_l.ToString("G29");
                    txtRISuspectP.Text = entity.ri_suspect_p.ToString("G29");
                    txtRIConfirmL.Text = entity.ri_confirm_l.ToString("G29");
                    txtRIConfirmP.Text = entity.ri_confirm_p.ToString("G29");
                }
                #endregion
            }
            return result;
        }
    }
}