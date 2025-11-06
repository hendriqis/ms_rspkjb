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
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoVoidAppointmentCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            string[] res = param.Split('|');
            string ParamedicId = res[0];
            string AppointmentDate = res[1];
            string ServiceUnit = res[2];
            txtParamedicId.ReadOnly = true;
            txtInfoVoidAppointmentDate.ReadOnly = true;
            txtServiceUnitName.ReadOnly = true;
            hdnParamedicID.Value = ParamedicId;
            hdnAppointmentDate.Value = AppointmentDate;
            hdnAppointmentDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
            hdnServiceUnit.Value = ServiceUnit;
            //if (hdnParamedicID.Value != "")
            //{
                ParamedicMaster pm = BusinessLayer.GetParamedicMaster(Convert.ToInt32(hdnParamedicID.Value));
                vHealthcareServiceUnit vsu = BusinessLayer.GetvHealthcareServiceUnitList(String.Format("HealthcareServiceUnitID = {0}", ServiceUnit)).FirstOrDefault();

                txtParamedicId.Text = pm.FullName;
                txtInfoVoidAppointmentDate.Text = string.Format("{0}", hdnAppointmentDate.Value);
                txtInfoVoidAppointmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceUnitName.Text = vsu.ServiceUnitName;
            //}
            //else
            //{
            //    txtParamedicId.Text = "-";
            //    txtInfoVoidAppointmentDate.Text = string.Format("{0}", hdnAppointmentDate.Value);
            //    txtInfoVoidAppointmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            //    txtServiceUnitName.Text = hdnServiceUnit.Value;
            //}
            BindGridView(1, true, ref PageCount);
        }
        List<vAppointment> lstVoidAppointment = null;
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";
            if (hdnParamedicID.Value != "")
            {
                filterExpression = string.Format("ParamedicID = {0} AND StartDate = '{1}' AND GCAppointmentStatus = '{2}' AND HealthcareServiceUnitID = '{3}'", hdnParamedicID.Value, hdnAppointmentDate.Value, Constant.AppointmentStatus.DELETED, hdnServiceUnit.Value);
                
            }
            else
            {
                filterExpression = string.Format("StartDate = '{0}' AND GCAppointmentStatus = '{1}' AND HealthcareServiceUnitID = '{2}'", hdnAppointmentDate.Value, Constant.AppointmentStatus.DELETED, hdnServiceUnit.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvAppointmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }

            List<vAppointment> lstEntity = BusinessLayer.GetvAppointmentList(filterExpression, Constant.GridViewPageSize.GRID_CTL, pageIndex);
            if (lstEntity.Count > 0)
                filterExpression = string.Format("AppointmentID IN ({0}) AND ParamedicID = {1}", string.Join(",", lstEntity.Select(p => p.AppointmentID)), hdnParamedicID.Value);
            else
                filterExpression = "1 = 0";
            lstVoidAppointment = BusinessLayer.GetvAppointmentList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }
        
        protected void cbpInfoVoidAppointmentView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}