using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MultiVisitAppointmentVoidCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            IsAdd = true;
            hdnID.Value = queryString;
            vDiagnosticVisitSchedule sch = BusinessLayer.GetvDiagnosticVisitScheduleList(string.Format("ID = {0}", queryString)).FirstOrDefault();
            vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", sch.AppointmentID))[0];
            SetControlProperties();
            EntityToControl(sch, entity);
        }

        protected void SetControlProperties()
        {
//            string filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DELETE_REASON);
            string filterExpression = string.Format("StandardCodeID = '{0}' AND IsDeleted = 0", Constant.AppointmentDeleteReason.OTHER);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboDeleteReason, lstSc, "StandardCodeName", "StandardCodeID");
            cboDeleteReason.SelectedIndex = 0;

            txtOtherDeleteReason.Text = "";
            GetSettingParameter();
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(String.Format("ParameterCode IN ('{0}', '{1}', '{2}', '{3}')", 
                Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS, 
                Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY, 
                Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE,
                Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE));
            hdnIsBridgingToMedinfrasMobileApps.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
            hdnIsBridgingToGateway.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).FirstOrDefault().ParameterValue;
            hdnProviderGatewayService.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).FirstOrDefault().ParameterValue;
            hdnIsUsingMultiVisitSchedule.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE).FirstOrDefault().ParameterValue;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtOtherDeleteReason, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDeleteReason, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(vDiagnosticVisitSchedule sch, vAppointment entity)
        {
            txtAppointmentNo.Text = entity.AppointmentNo;
            txtPatientName.Text = entity.cfPatientName;
            txtServiceUnit.Text = entity.ServiceUnitName;
            txtPhysician.Text = entity.ParamedicName;
            txtSequenceNo.Text = sch.SequenceNo.ToString();
            txtItemName.Text = sch.ItemName1;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            DiagnosticVisitSchedule sch = BusinessLayer.GetDiagnosticVisitSchedule(Convert.ToInt32(hdnID.Value));
            if (sch != null)
            {
                Appointment apm = BusinessLayer.GetAppointment(Convert.ToInt32(sch.AppointmentID));
                if (apm != null)
                {
                    if (apm.GCAppointmentStatus == Constant.AppointmentStatus.STARTED)
                    {
                        DraftTestOrderHd orderHd = BusinessLayer.GetDraftTestOrderHdList(string.Format("AppointmentID = {0} AND GCTransactionStatus IN ('{1}')", apm.AppointmentID, Constant.TransactionStatus.OPEN)).FirstOrDefault();
                        if (orderHd != null)
                        {
                            List<DraftTestOrderDt> lstOrderDt = BusinessLayer.GetDraftTestOrderDtList(string.Format("DraftTestOrderID = {0} AND IsDeleted = 0", orderHd.DraftTestOrderID));
                            if (lstOrderDt.Count > 0)
                            {
                                IDbContext ctx = DbFactory.Configure(true);
                                DiagnosticVisitScheduleDao diagVisitSchDao = new DiagnosticVisitScheduleDao(ctx);
                                DraftTestOrderHdDao orderHdDao = new DraftTestOrderHdDao(ctx);
                                DraftTestOrderDtDao orderDtDao = new DraftTestOrderDtDao(ctx);
                                AppointmentDao apmDao = new AppointmentDao(ctx);

                                try
                                {
                                    DraftTestOrderDt orderDt = lstOrderDt.Where(w => w.ItemID == sch.ItemID).FirstOrDefault();
                                    if (orderDt != null)
                                    {
                                        orderDt.IsDeleted = true;
                                        orderDt.GCDraftTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                        orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        orderDt.LastUpdatedDate = DateTime.Now;
                                        orderDtDao.Update(orderDt);
                                    }

                                    if (lstOrderDt.Count == 1)
                                    {
                                        orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                        orderHd.VoidBy = AppSession.UserLogin.UserID;
                                        orderHd.VoidDate = DateTime.Now;
                                        orderHd.GCVoidReason = cboDeleteReason.Value.ToString();
                                        orderHd.VoidReason = txtOtherDeleteReason.Text;
                                        orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        orderHd.LastUpdatedDate = DateTime.Now;
                                        orderHdDao.Update(orderHd);

                                        apm.DeleteReason = txtOtherDeleteReason.Text;
                                        apm.GCDeleteReason = cboDeleteReason.Value.ToString();
                                        apm.GCAppointmentStatus = Constant.AppointmentStatus.DELETED;
                                        apm.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        apm.LastUpdatedDate = DateTime.Now;
                                        apmDao.Update(apm);
                                    }

                                    sch.ScheduledDate = new DateTime(1900, 1, 1);
                                    sch.AppointmentID = null;
                                    sch.GCDiagnosticScheduleStatus = Constant.DiagnosticVisitScheduleStatus.OPEN;
                                    sch.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    sch.LastUpdatedDate = DateTime.Now;
                                    diagVisitSchDao.Update(sch);

                                    ctx.CommitTransaction();

                                    retval = string.Format("{0}|{1}|{2}|{3}", sch.ID, sch.GCDiagnosticScheduleStatus, sch.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), "0");
                                }
                                catch (Exception ex)
                                {
                                    result = false;
                                    ctx.RollBackTransaction();
                                    Helper.InsertErrorLog(ex);
                                }
                                finally
                                {
                                    ctx.Close();
                                }
                            }
                        }
                    }
                    else
                    {
                        switch (apm.GCAppointmentStatus)
                        {
                            case Constant.AppointmentStatus.DELETED:
                                errMessage = string.Format("Booking {0} sudah dibatalkan", apm.AppointmentNo);
                                break;
                            case Constant.AppointmentStatus.CANCELLED:
                                errMessage = string.Format("Booking {0} sudah dibatalkan", apm.AppointmentNo);
                                break;
                            case Constant.AppointmentStatus.COMPLETE:
                                errMessage = string.Format("Booking {0} sudah diregistrasikan. Harap refresh kembali halaman ini.", apm.AppointmentNo);
                                break;
                        }
                        result = false;
                    }
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Data booking tidak ditemukan");
                }
            }
            else
            {
                result = false;
                errMessage = string.Format("Data tidak ditemukan (ID = {0})", hdnID.Value);
            }


            return result;
        }
    }
}