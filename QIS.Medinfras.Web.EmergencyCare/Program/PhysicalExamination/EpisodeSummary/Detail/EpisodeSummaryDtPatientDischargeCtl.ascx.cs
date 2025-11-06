using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EmergencyCare.Program
{
    public partial class EpisodeSummaryDtPatientDischargeCtl : BaseViewPopupCtl
    {
        protected int PageCountDtFollowUpVisit = 1;
        protected int PageCountDtPatientInstruction = 1;
        public override void InitializeDataControl(string param)
        {
            BindGrdDtFollowUpVisit(1, true, ref PageCountDtFollowUpVisit);
            BindGrdDtPatientInstruction(1, true, ref PageCountDtPatientInstruction);
        }

        #region Follow Up Visit Complaint
        private void BindGrdDtFollowUpVisit(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("FromVisitID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}')", AppSession.RegisteredPatient.VisitID, Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvAppointmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vAppointment> lstEntity = BusinessLayer.GetvAppointmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdDtFollowUpVisit.DataSource = lstEntity;
            grdDtFollowUpVisit.DataBind();
        }

        protected void cbpDtFollowUpVisit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGrdDtFollowUpVisit(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGrdDtFollowUpVisit(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Patient Instruction
        private void BindGrdDtPatientInstruction(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientInstructionRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientInstruction> lstEntity = BusinessLayer.GetvPatientInstructionList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PatientInstructionID DESC");
            grdDtPatientInstruction.DataSource = lstEntity;
            grdDtPatientInstruction.DataBind();
        }

        protected void cbpDtPatientInstruction_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGrdDtPatientInstruction(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGrdDtPatientInstruction(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}