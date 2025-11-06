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
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using DevExpress.Web.ASPxEditors;
using System.Globalization;
using System.Data.SqlClient;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class CensusPerDayProcess : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.CENSUS_PER_DAY_PROCESS;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            txtPeriod.Text = DateTime.Now.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected override void SetControlProperties()
        {
            List<vPatientCensusMaxDateTime> lstEntity = BusinessLayer.GetvPatientCensusMaxDateTimeList("1=1");
            if (lstEntity.Count > 0)
            {
                divCensusDate.InnerHtml = lstEntity.FirstOrDefault().CensusDate.ToString(Constant.FormatString.DATE_FORMAT) + " " + lstEntity.FirstOrDefault().CencusTime;
            }
            else
            {
                divCensusDate.InnerHtml = "-";
            }


            List<vRegistrationStatusOutstandingCensus> lstReg = BusinessLayer.GetvRegistrationStatusOutstandingCensusList("1=1 ORDER BY ActualDischargeDateTime DESC, RegistrationID ASC");
            grdView.DataSource = lstReg;
            grdView.DataBind();
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            if (ProcessCensus(ref errMessage))
                result += "success";
            else
                result += string.Format("fail|{0}", errMessage);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool ProcessCensus(ref string errMessage)
        {
            try
            {
                bool result = BusinessLayer.ProcessPatientCensus(Helper.GetDatePickerValue(txtPeriod));
                if (result)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}