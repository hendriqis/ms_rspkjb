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
using QIS.Data.Core.Dal;
using Newtonsoft.Json;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxEditors;
using System.Data;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class QueueEditCtl : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value)).FirstOrDefault();

            GetSettingParameter();
            EntityToControl(entity);
            BindGridView();
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}', '{1}')", Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY, Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE));
            hdnIsBridgingToGateway.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).FirstOrDefault().ParameterValue;
            hdnProviderGatewayService.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).FirstOrDefault().ParameterValue;
        }

        private void BindGridView()
        {
            List<vConsultVisit9> lstEntity = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0} AND GCVisitStatus != '{1}' ORDER BY VisitID", hdnRegistrationID.Value, Constant.VisitStatus.CANCELLED));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private void EntityToControl(vRegistration entity)
        {
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtPatientName.Text = entity.PatientName;
            txtMedicalNo.Text = entity.MedicalNo;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            string errMessage = string.Empty;
            if (e.Parameter != null && e.Parameter != "")
            {
                if (param[0] == "changepage")
                {
                    BindGridView();
                    result = "changepage";
                }
                else if (param[0] == "update")
                {
                    string id = param[1];
                    string session = string.Empty;
                    if (param.Length > 2)
                    {
                        session = param[2];
                    }
                    if (OnGetNewQueueNo(id, session, ref errMessage))
                    {
                        result = "update|success|";
                    }
                    else
                    {
                        result = string.Format("update|failed|{0}", errMessage);
                    }
                    BindGridView();

                }
                else if (param[0] == "print")
                {
                    string id = param[1];
                    PrintBuktiPendaftaranKunjungan(id);
                    BindGridView();
                }
                else // refresh
                {
                    BindGridView();
                }
            }
            
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnGetNewQueueNo(string id, string session, ref string errMessage)
        {
            bool result = false;
            bool isQueueValid = true;
            int newQueue = 0;
            int newSession = Convert.ToInt32(session);
            bool isBPJS = false;

            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", id)).FirstOrDefault();
            if (entity.GCVisitStatus != Constant.VisitStatus.CANCELLED && entity.GCVisitStatus != Constant.VisitStatus.CLOSED)
            {
                QueueLastNo checkQueue = BusinessLayer.GetQueueLastNoList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduledDate = '{2}' AND Session = {3}", entity.HealthcareServiceUnitID, entity.ParamedicID, entity.VisitDate, session)).FirstOrDefault();
                short oldQueue = entity.QueueNo;

                int checkQueueNo = 0;
                if (checkQueue != null)
                {
                    checkQueueNo = checkQueue.QueueNo;
                }

                bool isChangeSession = false;
                if (entity.Session != Convert.ToInt16(session))
                {
                    isChangeSession = true;
                }
                if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                {
                    isBPJS = true;
                }

                if (hdnIsBridgingToGateway.Value == "1")
                {
                    //Healthcare entityHSU = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
                    if (Constant.HealthcareGatewayProvider.RSDOSOBA == hdnProviderGatewayService.Value)
                    {
                        if (string.IsNullOrEmpty(session))
                        {
                            newSession = Convert.ToInt32(session);
                        }
                        else
                        {
                            newSession = BusinessLayer.GetRegistrationSession(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.VisitDate, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));
                        }
                        string queue = BridgingToGatewayGetQueueNo(entity.MedicalNo, entity.ParamedicCode, entity.GCCustomerType, DateTime.Now, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT), "DT", entity.HealthcareServiceUnitID.ToString(), newSession);
                        string[] queueSplit = queue.Split('|');
                        if (queueSplit[0] == "1")
                        {
                            newQueue = Convert.ToInt16(queueSplit[1]);
                            result = true;
                            isQueueValid = true;
                        }
                        else
                        {
                            errMessage = queueSplit[1];
                            result = false;
                            isQueueValid = false;
                        }
                    }
                }

                IDbContext ctx = DbFactory.Configure(true);
                AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
                ConsultVisitDao entityCVDao = new ConsultVisitDao(ctx);

                if (isQueueValid)
                {
                    if (result)
                    {
                        if (entity != null)
                        {
                            if ((entity.QueueNo < checkQueueNo) || isChangeSession)
                            {
                                try
                                {
                                    if (entity.IsMainVisit)
                                    {
                                        if (entity.AppointmentID != 0)
                                        {
                                            Appointment entityApm = BusinessLayer.GetAppointment(entity.AppointmentID);
                                            if (hdnIsBridgingToGateway.Value == "1")
                                            {
                                                if (Constant.HealthcareGatewayProvider.RSDOSOBA == hdnProviderGatewayService.Value)
                                                {
                                                    if (!isQueueValid)
                                                    {
                                                        entityApm.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, entity.ParamedicID, entity.VisitDate, Convert.ToInt32(session), false, isBPJS, 0, 1, ctx));
                                                        ctx.CommandType = CommandType.Text;
                                                        ctx.Command.Parameters.Clear();

                                                        newQueue = Convert.ToInt32(entityApm.QueueNo);
                                                        //entityApm.QueueNo = Convert.ToInt16(checkQueue.QueueNo + 1);
                                                    }
                                                    else
                                                    {
                                                        entityApm.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                entityApm.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, entity.ParamedicID, entity.VisitDate, Convert.ToInt32(session), false, isBPJS, 0, 1, ctx));
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();

                                                newQueue = Convert.ToInt32(entityApm.QueueNo);
                                                entityApm.Session = Convert.ToInt16(session);
                                                //entityApm.QueueNo = Convert.ToInt16(checkQueue.QueueNo + 1);
                                            }
                                            entityApm.ReferenceQueueNo = oldQueue;
                                            entityApm.QueueChangedBy = AppSession.UserLogin.UserID;
                                            entityApm.QueueChangedDate = DateTime.Now;
                                            entityApm.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            entityApm.LastUpdatedDate = DateTime.Now;
                                            entityAppointmentDao.Update(entityApm);
                                        }
                                    }
                                    else
                                    {
                                        if (Constant.HealthcareGatewayProvider.RSDOSOBA == hdnProviderGatewayService.Value)
                                        {
                                            if (!isQueueValid)
                                            {
                                                newQueue = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, entity.ParamedicID, entity.VisitDate, entity.Session, 1));
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                            }
                                        }
                                        else
                                        {
                                            newQueue = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, entity.ParamedicID, entity.VisitDate, Convert.ToInt32(session), false, isBPJS, 0, 1, ctx));
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                        }
                                    }

                                    ConsultVisit entityCV = BusinessLayer.GetConsultVisit(entity.VisitID);
                                    if (hdnIsBridgingToGateway.Value == "1")
                                    {
                                        if (Constant.HealthcareGatewayProvider.RSDOSOBA == hdnProviderGatewayService.Value)
                                        {
                                            if (!isQueueValid)
                                            {
                                                //entityCV.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, entity.ParamedicID, entity.VisitDate, entity.Session, 1));
                                                entityCV.QueueNo = Convert.ToInt16(newQueue);
                                            }
                                            else
                                            {
                                                entityCV.QueueNo = Convert.ToInt16(newQueue);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (newQueue == 0)
                                        {
                                            entityCV.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, entity.ParamedicID, entity.VisitDate, Convert.ToInt32(session), false, isBPJS, 0, 1, ctx));
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            //entityCV.QueueNo = Convert.ToInt16(newQueue);
                                        }
                                    }
                                    entityCV.Session = Convert.ToInt16(newSession);
                                    entityCV.ReferenceQueueNo = oldQueue;
                                    entityCV.QueueChangedBy = AppSession.UserLogin.UserID;
                                    entityCV.QueueChangedDate = DateTime.Now;
                                    entityCV.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityCV.LastUpdatedDate = DateTime.Now;
                                    entityCVDao.Update(entityCV);

                                    ctx.CommitTransaction();

                                    result = true;
                                }
                                catch (Exception ex)
                                {
                                    errMessage = ex.Message;
                                    ctx.RollBackTransaction();
                                }
                                finally
                                {
                                    ctx.Close();
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = "Sudah mendapatkan nomor antrian terakhir";
                            }
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
                else
                {
                    ctx.RollBackTransaction();
                }
            }
            else
            {
                if (entity.GCVisitStatus == Constant.VisitStatus.CANCELLED)
                {
                    errMessage = "Tidak bisa ubah nomor antrian karena registrasi sudah batal";
                }
                else if (entity.GCVisitStatus == Constant.VisitStatus.CLOSED)
                {
                    errMessage = "Tidak bisa ubah nomor antrian karena registrasi sudah tutup";
                }
            }

            return result;
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = false;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            return result;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vConsultVisit9 entity = e.Row.DataItem as vConsultVisit9;
                ASPxComboBox cboSession = e.Row.FindControl("cboSession") as ASPxComboBox;
                HtmlGenericControl divSession = e.Row.FindControl("divSelectedSession") as HtmlGenericControl;

                int dayNumber = (int)entity.VisitDate.DayOfWeek;
                if (dayNumber == 0)
                {
                    dayNumber = 7;
                }
                OperationalTime entityOT = BusinessLayer.GetOperationalTimeList(string.Format("OperationalTimeID = (SELECT OperationalTimeID FROM ParamedicSchedule WHERE HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2})", entity.HealthcareServiceUnitID, entity.ParamedicID, dayNumber)).FirstOrDefault();

                int totalSession = 0;
                if (entityOT != null)
                {
                    totalSession = TotalSession(entityOT);
                }
                else
                {
                    totalSession = 1;
                }

                List<Variable> lstVariable = new List<Variable>();
                for (int i = 0; i < totalSession; i++)
                {
                    lstVariable.Add(new Variable { Code = (i + 1).ToString(), Value = string.Format("Sesi {0}", i + 1) });
                }

                int getSession = BusinessLayer.GetRegistrationSession(entity.HealthcareServiceUnitID, entity.ParamedicID, entity.VisitDate, entity.VisitTime);

                Methods.SetComboBoxField<Variable>(cboSession, lstVariable, "Value", "Code");
                cboSession.SelectedIndex = getSession - 1;
                divSession.InnerHtml = getSession.ToString();
                divSession.InnerText = getSession.ToString();
            }
        }

        private int TotalSession(OperationalTime entityOT)
        {
            int result = 1;

            if (!string.IsNullOrEmpty(entityOT.StartTime5) && !string.IsNullOrEmpty(entityOT.EndTime5))
            {
                result = 5;
            }
            else if (!string.IsNullOrEmpty(entityOT.StartTime4) && !string.IsNullOrEmpty(entityOT.EndTime4))
            {
                result = 4;
            }
            else if (!string.IsNullOrEmpty(entityOT.StartTime3) && !string.IsNullOrEmpty(entityOT.EndTime3))
            {
                result = 3;
            }
            else if (!string.IsNullOrEmpty(entityOT.StartTime2) && !string.IsNullOrEmpty(entityOT.EndTime2))
            {
                result = 2;
            }
            else if (!string.IsNullOrEmpty(entityOT.StartTime1) && !string.IsNullOrEmpty(entityOT.EndTime1))
            {
                result = 1;
            }

            return result;
        }

        private string PrintBuktiPendaftaranKunjungan(string id)
        {
            //Get Printer Address
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExpression = string.Format("VisitID = {0} AND GCVisitStatus != ('{1}')", id, Constant.VisitStatus.CANCELLED);
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.BUKTI_PENDAFTARAN);

            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

            List<vConsultVisit9> lstoVisitPrint = BusinessLayer.GetvConsultVisit9List(filterExpression);

            if (lstPrinter.Count > 0)
            {
                foreach (vConsultVisit9 obj in lstoVisitPrint)
                {
                    ZebraPrinting.PrintBuktiPendaftaranKunjunganBros(obj, lstPrinter[0].PrinterName);
                }
            }
            else
            {
                return string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
            }

            return string.Empty;
        }

        private string BridgingToGatewayGetQueueNo(string medicalNo, string paramedicCode, string customerType, DateTime date, string hour, string via, string healthcareServiceUnitID, int session)
        {
            String queue = "";

            if (hdnIsBridgingToGateway.Value == "1")
            {
                GatewayService oService = new GatewayService();
                APIMessageLog entityAPILog = new APIMessageLog();
                entityAPILog.Sender = "MEDINFRAS";
                entityAPILog.Recipient = "QUEUE ENGINE";
                string apiResult = oService.GetQueueNo(medicalNo, paramedicCode, customerType, date, hour, via, healthcareServiceUnitID, session);
                string[] apiResultInfo = apiResult.Split('|');
                if (apiResultInfo[0] == "0")
                {
                    queue = string.Format("{0}|{1}", apiResultInfo[0], apiResultInfo[1]);
                    entityAPILog.MessageDateTime = DateTime.Now;
                    entityAPILog.IsSuccess = false;
                    entityAPILog.MessageText = apiResultInfo[2];
                    entityAPILog.Response = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    Exception ex = new Exception(apiResultInfo[1]);
                    Helper.InsertErrorLog(ex);
                }
                else
                {
                    queue = apiResult;
                    entityAPILog.MessageDateTime = DateTime.Now;
                    entityAPILog.MessageText = apiResultInfo[2];
                    entityAPILog.Response = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                }
            }

            return queue;
        }
    }
}