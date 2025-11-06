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
    public partial class RekapPasienKeluar : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.BRIDGING_SIRANAP_REKAPPASIENKELUAR;
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

            GetRekapPasienKeluar entity = BusinessLayer.GetRekapPasienKeluarList(observasiDate).FirstOrDefault();
            if (entity == null)
            {
                txtAps.Text = "0";
               
                txtDirujuk.Text = "0";
             
                txtDiscarded.Text = "0";
               
                txtIsman.Text = "0";
               
                txtMeninggalDiscardedKomorbid.Text = "0";
               
                txtMeninggalDiscardedTanpaKomorbid.Text = "0";
              
                txtMeninggalKomorbid.Text = "0";
               
                txtMeninggalProbAnakKomorbid.Text = "0";
               
                txtMeninggalProbAnakTanpaKomorbid.Text = "0";
              
                txtMeninggalProbBalitaKomorbid.Text = "0";
               
                txtMeninggalProbBalitaTanpaKomorbid.Text = "0";
             
                txtMeninggalProbBayiKomorbid.Text = "0";
               
                txtMeninggalProbBayiTanpaKomorbid.Text = "0";
              
                txtmeninggalProbDwsKomorbid.Text = "0";
                
                txtMeninggalProbDwsTanpaKomorbid.Text = "0";
               
                txtMeninggalProbDwsTanpaKomorbid.Text = "0";
              
                txtMeninggalProbLansiaKomorbid.Text = "0";
                
                txtMeninggalProbLansiaTanpaKomorbid.Text = "0";
               
                txtMeninggalProbNeoKomorbid.Text = "0";
                
                txtMeninggalProbNeoTanpaKomorbid.Text = "0";
               
                txtMeninggalProbPrekomorbid.Text = "0";
               
                txtMeninggalProbPreTanpaKomorbid.Text = "0";
                
                txtSembuh.Text = "0";
                
                txtMeninggalTanpaKomorbid.Text = "0";
                txtMeninggalProbRemajaKomorbid.Text = "0";
                txtMeninggalProbRemajaTanpaKomorbid.Text = "0";
              

            }
            else
            {
                //txtICUdenganVentilatorSuspectL.Text = entity.icu_dengan_ventilator_suspect_l.ToString("G29");

                txtAps.Text = entity.aps.ToString("G29");

                txtDirujuk.Text = entity.dirujuk.ToString("G29");

                txtDiscarded.Text = entity.discarded.ToString("G29");
                txtIsman.Text = entity.isman.ToString("G29");
                txtMeninggalDiscardedKomorbid.Text = entity.meninggal_disarded_komorbid.ToString("G29");

                txtMeninggalDiscardedTanpaKomorbid.Text = entity.meninggal_discarded_tanpa_komorbid.ToString("G29");

                txtMeninggalKomorbid.Text = entity.meninggal_komorbid.ToString("G29");

                txtMeninggalProbAnakKomorbid.Text = entity.meninggal_prob_anak_komorbid.ToString("G29");

                txtMeninggalProbAnakTanpaKomorbid.Text = entity.meninggal_prob_anak_tanpa_komorbid.ToString("G29");

                txtMeninggalProbBalitaKomorbid.Text = entity.meninggal_prob_balita_komorbid.ToString("G29");

                txtMeninggalProbBalitaTanpaKomorbid.Text = entity.meninggal_prob_balita_tanpa_komorbid.ToString("G29");

                txtMeninggalProbBayiKomorbid.Text = entity.meninggal_prob_bayi_komorbid.ToString("G29");

                txtMeninggalProbBayiTanpaKomorbid.Text = entity.meninggal_prob_bayi_tanpa_komorbid.ToString("G29");

                txtmeninggalProbDwsKomorbid.Text = entity.meninggal_prob_dws_komorbid.ToString("G29");

                txtMeninggalProbDwsTanpaKomorbid.Text = entity.meninggal_prob_dws_tanpa_komorbid.ToString("G29");

                txtMeninggalProbLansiaKomorbid.Text = entity.meninggal_prob_lansia_komorbid.ToString("G29");

                txtMeninggalProbLansiaTanpaKomorbid.Text = entity.meninggal_prob_lansia_tanpa_komorbid.ToString("G29");

                txtMeninggalProbNeoKomorbid.Text = entity.meninggal_prob_neo_komorbid.ToString("G29");

                txtMeninggalProbNeoTanpaKomorbid.Text = entity.meninggal_prob_neo_tanpa_komorbid.ToString("G29");

                txtMeninggalProbPrekomorbid.Text = entity.meninggal_prob_pre_komorbid.ToString("G29");

                txtMeninggalProbPreTanpaKomorbid.Text = entity.meninggal_prob_pre_tanpa_komorbid.ToString("G29");

                txtSembuh.Text = entity.sembuh.ToString("G29");

                txtMeninggalTanpaKomorbid.Text = entity.meninggal_tanpa_komorbid.ToString("G29");
                txtMeninggalProbRemajaKomorbid.Text = entity.meninggal_prob_remaja_komorbid.ToString("G29");
                txtMeninggalProbRemajaTanpaKomorbid.Text = entity.meninggal_prob_remaja_tanpa_komorbid.ToString("G29");
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtAps, new ControlEntrySetting(true, true, true, "0"));
           
            SetControlEntrySetting(txtDirujuk, new ControlEntrySetting(true, true, true, "0"));
             
            SetControlEntrySetting(txtDiscarded, new ControlEntrySetting(true, true, true, "0"));
             
            SetControlEntrySetting(txtIsman, new ControlEntrySetting(true, true, true, "0"));
             
            SetControlEntrySetting(txtMeninggalDiscardedKomorbid, new ControlEntrySetting(true, true, true, "0"));
             
            SetControlEntrySetting(txtMeninggalDiscardedTanpaKomorbid, new ControlEntrySetting(true, true, true, "0"));
             
            SetControlEntrySetting(txtMeninggalKomorbid, new ControlEntrySetting(true, true, true, "0"));
           
            SetControlEntrySetting(txtMeninggalProbAnakKomorbid, new ControlEntrySetting(true, true, true, "0"));
            
            SetControlEntrySetting(txtMeninggalProbAnakTanpaKomorbid, new ControlEntrySetting(true, true, true, "0"));
           
            SetControlEntrySetting(txtMeninggalProbBalitaKomorbid, new ControlEntrySetting(true, true, true, "0"));
            
            SetControlEntrySetting(txtMeninggalProbBalitaTanpaKomorbid, new ControlEntrySetting(true, true, true, "0"));
            
            SetControlEntrySetting(txtMeninggalProbBayiKomorbid, new ControlEntrySetting(true, true, true, "0"));
         
            SetControlEntrySetting(txtMeninggalProbBayiTanpaKomorbid, new ControlEntrySetting(true, true, true, "0"));
           
            SetControlEntrySetting(txtmeninggalProbDwsKomorbid, new ControlEntrySetting(true, true, true, "0"));
           
            SetControlEntrySetting(txtMeninggalProbDwsTanpaKomorbid, new ControlEntrySetting(true, true, true, "0"));
            
            SetControlEntrySetting(txtMeninggalProbLansiaKomorbid, new ControlEntrySetting(true, true, true, "0"));
            
            SetControlEntrySetting(txtMeninggalProbLansiaTanpaKomorbid, new ControlEntrySetting(true, true, true, "0"));
          
            SetControlEntrySetting(txtMeninggalProbNeoKomorbid, new ControlEntrySetting(true, true, true, "0"));
           
            SetControlEntrySetting(txtMeninggalProbNeoTanpaKomorbid, new ControlEntrySetting(true, true, true, "0"));
           
            SetControlEntrySetting(txtMeninggalProbPrekomorbid, new ControlEntrySetting(true, true, true, "0"));
          
            SetControlEntrySetting(txtMeninggalProbPreTanpaKomorbid, new ControlEntrySetting(true, true, true, "0"));
         
            SetControlEntrySetting(txtSembuh, new ControlEntrySetting(true, true, true, "0"));
         
            SetControlEntrySetting(txtMeninggalTanpaKomorbid, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtMeninggalProbRemajaTanpaKomorbid, new ControlEntrySetting(true, true, true, "0"));
            
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

                GetRekapPasienKeluar entity = BusinessLayer.GetRekapPasienKeluarList(observasiDate).FirstOrDefault();
                if (entity == null)
                {
                    txtAps.Text = "0";

                    txtDirujuk.Text = "0";

                    txtDiscarded.Text = "0";

                    txtIsman.Text = "0";

                    txtMeninggalDiscardedKomorbid.Text = "0";

                    txtMeninggalDiscardedTanpaKomorbid.Text = "0";

                    txtMeninggalKomorbid.Text = "0";

                    txtMeninggalProbAnakKomorbid.Text = "0";

                    txtMeninggalProbAnakTanpaKomorbid.Text = "0";

                    txtMeninggalProbBalitaKomorbid.Text = "0";

                    txtMeninggalProbBalitaTanpaKomorbid.Text = "0";

                    txtMeninggalProbBayiKomorbid.Text = "0";

                    txtMeninggalProbBayiTanpaKomorbid.Text = "0";

                    txtmeninggalProbDwsKomorbid.Text = "0";

                    txtMeninggalProbDwsTanpaKomorbid.Text = "0";

                    txtMeninggalProbDwsTanpaKomorbid.Text = "0";

                    txtMeninggalProbLansiaKomorbid.Text = "0";

                    txtMeninggalProbLansiaTanpaKomorbid.Text = "0";

                    txtMeninggalProbNeoKomorbid.Text = "0";

                    txtMeninggalProbNeoTanpaKomorbid.Text = "0";

                    txtMeninggalProbPrekomorbid.Text = "0";

                    txtMeninggalProbPreTanpaKomorbid.Text = "0";

                    txtSembuh.Text = "0";

                    txtMeninggalTanpaKomorbid.Text = "0";
                    txtMeninggalProbRemajaTanpaKomorbid.Text = "0";
                    txtMeninggalProbRemajaKomorbid.Text = "0";
                }
                else
                {


                    txtAps.Text = entity.aps.ToString("G29");

                    txtDirujuk.Text = entity.dirujuk.ToString("G29");

                    txtDiscarded.Text = entity.discarded.ToString("G29");
                    txtIsman.Text = entity.isman.ToString("G29");
                    txtMeninggalDiscardedKomorbid.Text = entity.meninggal_disarded_komorbid.ToString("G29");

                    txtMeninggalDiscardedTanpaKomorbid.Text = entity.meninggal_discarded_tanpa_komorbid.ToString("G29");

                    txtMeninggalKomorbid.Text = entity.meninggal_komorbid.ToString("G29");

                    txtMeninggalProbAnakKomorbid.Text = entity.meninggal_prob_anak_komorbid.ToString("G29");

                    txtMeninggalProbAnakTanpaKomorbid.Text = entity.meninggal_prob_anak_tanpa_komorbid.ToString("G29");

                    txtMeninggalProbBalitaKomorbid.Text = entity.meninggal_prob_balita_komorbid.ToString("G29");

                    txtMeninggalProbBalitaTanpaKomorbid.Text = entity.meninggal_prob_balita_tanpa_komorbid.ToString("G29");

                    txtMeninggalProbBayiKomorbid.Text = entity.meninggal_prob_bayi_komorbid.ToString("G29");

                    txtMeninggalProbBayiTanpaKomorbid.Text = entity.meninggal_prob_bayi_tanpa_komorbid.ToString("G29");

                    txtmeninggalProbDwsKomorbid.Text = entity.meninggal_prob_dws_komorbid.ToString("G29");

                    txtMeninggalProbDwsTanpaKomorbid.Text = entity.meninggal_prob_dws_tanpa_komorbid.ToString("G29");

                    txtMeninggalProbLansiaKomorbid.Text = entity.meninggal_prob_lansia_komorbid.ToString("G29");

                    txtMeninggalProbLansiaTanpaKomorbid.Text = entity.meninggal_prob_lansia_tanpa_komorbid.ToString("G29");

                    txtMeninggalProbNeoKomorbid.Text = entity.meninggal_prob_neo_komorbid.ToString("G29");

                    txtMeninggalProbNeoTanpaKomorbid.Text = entity.meninggal_prob_neo_tanpa_komorbid.ToString("G29");

                    txtMeninggalProbPrekomorbid.Text = entity.meninggal_prob_pre_komorbid.ToString("G29");

                    txtMeninggalProbPreTanpaKomorbid.Text = entity.meninggal_prob_pre_tanpa_komorbid.ToString("G29");

                    txtSembuh.Text = entity.sembuh.ToString("G29");

                    txtMeninggalTanpaKomorbid.Text = entity.meninggal_tanpa_komorbid.ToString("G29");
                    txtMeninggalProbRemajaTanpaKomorbid.Text = entity.meninggal_prob_remaja_tanpa_komorbid.ToString("G29");
                    txtMeninggalProbRemajaKomorbid.Text = entity.meninggal_prob_remaja_komorbid.ToString("G29") ;
                   /////// txtMeninggalProbRemajaTanpaKomorbid.Text = entity.meninggal_prob_remaja_tanpa_komorbid.ToString("G29") ;
              
                }
                #endregion
            }
            return result;
        }
    }
}