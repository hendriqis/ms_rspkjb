using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class ReopenOrderStatusCtl : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnPrescriptionOrderIDCtl.Value = paramInfo[1];
            txtTransactionNo.Text = paramInfo[2];

            if (!string.IsNullOrEmpty(hdnPrescriptionOrderIDCtl.Value))
            {
                string filterExpression = string.Format("PrescriptionOrderID = {0}", hdnPrescriptionOrderIDCtl.Value);
                vPrescriptionOrderHd3 entity = BusinessLayer.GetvPrescriptionOrderHd3List(filterExpression).FirstOrDefault();
                if (entity != null)
                {
                    EntityToControl(entity);
                }
            }
        }

        private void EntityToControl(vPrescriptionOrderHd3 entity)
        {
            txtOrderDateTime.Text = string.Format("{0} {1}", entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), entity.PrescriptionTime);
            txtPrescripionType.Text = entity.PrescriptionType;
            txtTransactionStatus.Text = entity.TransactionStatus;
            txtOrderStatus.Text = entity.OrderStatus;
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            try
            {
                PrescriptionOrderHd orderHd = orderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderIDCtl.Value));
                if (orderHd != null)
                {
                    List<PatientChargesHd> lstChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PrescriptionOrderID = {0} AND GCTransactionStatus <> '{1}'", orderHd.PrescriptionOrderID, Constant.TransactionStatus.VOID), ctx);
                    if (lstChargesHd.Count() == 0)
                    {
                        List<MedicationSchedule> lstMedSch = BusinessLayer.GetMedicationScheduleList(string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", orderHd.PrescriptionOrderID), ctx);
                        if (lstMedSch.Count() == 0)
                        {
                            List<PrescriptionOrderDt> lstOrderDt = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0 AND GCPrescriptionOrderStatus <> '{1}'", orderHd.PrescriptionOrderID, Constant.OrderStatus.CANCELLED), ctx);
                            foreach (PrescriptionOrderDt orderDt in lstOrderDt)
                            {
                                orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(orderDt);
                            }

                            orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            orderHd.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                            orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            orderHdDao.Update(orderHd);
                            ctx.CommitTransaction();

                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("Order resep {0} ini tidak dapat dilakukan re-open karena masih ada Jadwal Pemberian Obat (Medication Schedule) yg masih valid", orderHd.PrescriptionOrderNo);
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Order resep {0} ini tidak dapat dilakukan re-open karena masih ada Medication Charges yg masih valid", orderHd.PrescriptionOrderNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }

                retval = hdnPrescriptionOrderIDCtl.Value;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = "0|" + errMessage;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }
    }
}