using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class MergeMedicalRecord : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.MERGE_MEDICAL_RECORD;
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
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtMRN1, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtMRN2, new ControlEntrySetting(false, false, true));
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;

            SettingParameterDt  oParam = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_NOMOR_RM_DIGUNAKAN_KEMBALI);
            if (oParam != null)
            {
                IDbContext ctx = DbFactory.Configure(true);
                try
                {
                    int mrn1 = Convert.ToInt32(hdnMRN1.Value);
                    int mrn2 = Convert.ToInt32(hdnMRN2.Value);
                    bool isReused = oParam.ParameterValue == "1" ? true : false;
                    BusinessLayer.MergeMedicalNumber(mrn1, mrn2, isReused, AppSession.UserLogin.UserID, ctx);
                    result = true;
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    ctx.RollBackTransaction();
                    errMessage = ex.Message;
                    result = false;
                }
                finally
                {
                    ctx.Close();
                }
            }
            else
            {
                errMessage = "Parameter System yang diperlukan tidak ditemukan!";
                result = false;
            }
            return result;    
        }
    }
}