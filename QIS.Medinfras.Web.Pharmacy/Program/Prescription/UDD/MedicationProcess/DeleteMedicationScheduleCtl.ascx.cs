using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class DeleteMedicationScheduleCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            this.PopupTitle = "Hapus Jadwal Pemberian Obat";
            string[] paramInfo = param.Split('|');
            hdnVisitID.Value = paramInfo[0];
            hdnPrescriptionOrderDtID.Value = paramInfo[1];
            hdnPastMedicationID.Value = paramInfo[2];

            BindGridView();
        }

        protected void cbpDeleteScheduleView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("VisitID = {0} AND TransactionID IS NULL AND IsDeleted = 0", hdnVisitID.Value);
            
            if (hdnPrescriptionOrderDtID.Value != "" && hdnPrescriptionOrderDtID.Value != "0")
            {
                if (hdnPastMedicationID.Value != "" && hdnPastMedicationID.Value != "0")
                {
                    filterExpression += string.Format(" AND (PrescriptionOrderDetailID = {0} OR PastMedicationID = {1})", hdnPrescriptionOrderDtID.Value, hdnPastMedicationID.Value);
                }
                else
                {
                    filterExpression += string.Format(" AND PrescriptionOrderDetailID = {0}", hdnPrescriptionOrderDtID.Value);
                }
            }

            if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += " AND ";
                filterExpression += hdnFilterExpressionQuickSearch.Value;
            }

            filterExpression += string.Format(" ORDER BY PrescriptionOrderNo, DrugName, MedicationDate, SequenceNo");

            List<vMedicationScheduleVoid> lstEntity = BusinessLayer.GetvMedicationScheduleVoidList(filterExpression);
            lvwDeleteScheduleView.DataSource = lstEntity;
            lvwDeleteScheduleView.DataBind();
        }

        protected void cbpDeleteScheduleProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errorMessage = string.Empty;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            IDbContext ctx = DbFactory.Configure(true);
            MedicationScheduleDao scheduleDao = new MedicationScheduleDao(ctx);

            try
            {
                if (!string.IsNullOrEmpty(hdnSelectedID.Value))
                {
                    string filterExpression = string.Format("ID IN ({0})", hdnSelectedID.Value);
                    List<MedicationSchedule> lstSchedule = BusinessLayer.GetMedicationScheduleList(filterExpression, ctx);
                    if (lstSchedule.Count > 0)
                    {
                        foreach (MedicationSchedule oSchedule in lstSchedule)
                        {
                            oSchedule.IsDeleted = true;
                            oSchedule.GCDeleteReason = Constant.DeleteReason.OTHER;
                            oSchedule.DeleteReason = "Deleted from Delete Medication Schedule : " + txtDeleteReason.Text;
                            oSchedule.DeleteBy = AppSession.UserLogin.UserID;
                            oSchedule.DeleteDate = DateTime.Now;
                            oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                            scheduleDao.Update(oSchedule);
                        }
                        ctx.CommitTransaction();

                        string message = string.Format("Jadwal Pemberian Obat telah dihapus");
                        result = string.Format("process|1|{0}", message);
                    }
                    else
                    {
                        string message = string.Format("Tidak ada Jadwal Pemberian Obat yang dapat dihapus");
                        result = string.Format("process|0|{0}", message);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    string message = string.Format("Terjadi kesalahan pada saat hapus Jadwal Pemberian Obat");
                    result = string.Format("process|0|{0}", message);
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}", ex.InnerException.Message.ToString());
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }
    }
}