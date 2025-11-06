using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
namespace QIS.Medinfras.Web.Outpatient.Program
{
    public partial class PatientAppointmentInformationCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            hdnregistrationDate.Value = param.Split('|')[0];
            hdnhealthcareServiceUnitID.Value = param.Split('|')[1];
            hdnphysicianID.Value = param.Split('|')[2];
                
            BindGridView(1, true, ref PageCount);   

        }

        #region Binding Source
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";

            //KARENA HANYA MUNCUL KETIKA DOKTER, KLIKIK & TANGGAL DIPILIH 
            //if (hdnphysicianID.Value == null || hdnphysicianID.Value == "" || hdnphysicianID.Value =="0") 
            //{
            //   // filterExpression = string.Format("HealthcareServiceUnitID = {0} AND StartDate = '{1}' ", hdnhealthcareServiceUnitID.Value, Helper.GetDatePickerValue(hdnregistrationDate.Value).ToString(Constant.FormatString.DATE_FORMAT_112));
               
            //}
            //else
            //{
                filterExpression = string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}' ", hdnhealthcareServiceUnitID.Value, hdnphysicianID.Value, Helper.GetDatePickerValue(hdnregistrationDate.Value).ToString(Constant.FormatString.DATE_FORMAT_112));
            //}

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvAppointmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }

            List<vAppointment> lstEntity = BusinessLayer.GetvAppointmentList(filterExpression, Constant.GridViewPageSize.GRID_CTL, pageIndex, "HealthcareServiceUnitID, ParamedicID, QueueNo");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpViewCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}