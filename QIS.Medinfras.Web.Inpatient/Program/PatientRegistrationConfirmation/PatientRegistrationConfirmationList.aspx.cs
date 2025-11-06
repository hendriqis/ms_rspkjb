using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class PatientRegistrationConfirmationList : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.PATIENT_REGISTRATION_CONFIRMATION;
        }

        private GetUserMenuAccess menu;
        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }
        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";

        protected int PageCount = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hdnIsBridgingToAplicares.Value = AppSession.IsBridgingToAPLICARES ? "1" : "0";

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.INPATIENT, "IsUsingRegistration = 1");
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                GetSettingParameter();

                BindGridView();
                Helper.SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, true), "mpBedStatus");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        protected void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IP_IS_TRANSFER_MEAL_ORDER, Constant.SettingParameter.SA_IS_BRIDGING_TO_IPTV));
            hdnIsTransferMealOrder.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_IS_TRANSFER_MEAL_ORDER).FirstOrDefault().ParameterValue;
            hdnIsBridgingToIPTV.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.SA_IS_BRIDGING_TO_IPTV).FirstOrDefault().ParameterValue;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "confirm")
                {
                    if (param[1] == "true")
                    {
                        if (OnConfirm(true, ref errMessage))
                        {
                            result += "success|";
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(errMessage))
                            {
                                result += string.Format("failed|Konfirmasi Gagal ({0})", errMessage);
                            }
                            else
                            {
                                result += "failed|Konfirmasi Gagal";
                            }
                        }
                    }
                    else
                    {
                        if (OnConfirm(false, ref errMessage))
                        {
                            result += "success|";
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(errMessage))
                            {
                                result += string.Format("failed|Konfirmasi Gagal ({0})", errMessage);
                            }
                            else
                            {
                                result += "failed|Konfirmasi Gagal";
                            }
                        }
                    }
                }
                else
                {
                    BindGridView();
                    result += "refresh|";
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string filterExpression = "";
            if (hdnRoomID.Value != "")
                filterExpression = string.Format("GCBedStatus = '{0}' AND RoomID = {1} AND IsPatientAccompany = 0", Constant.BedStatus.BOOKED, hdnRoomID.Value);
            else
                filterExpression = string.Format("GCBedStatus = '{0}' AND RoomID IN (SELECT RoomID FROM ServiceUnitRoom WHERE HealthcareServiceUnitID = {1} AND IsDeleted = 0 AND IsPatientAccompany = 0)", Constant.BedStatus.BOOKED, cboServiceUnit.Value);

            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            List<vBed> lstEntity = BusinessLayer.GetvBedList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpRegistrationConfirmation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "save")
                {
                    if (hdnIsTransferMealOrder.Value == "1")
                    {
                        List<NutritionOrderHd> lstNutritionOrderHd = BusinessLayer.GetNutritionOrderHdList(string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ConsultVisit WHERE RegistrationID IN ({0})) AND GCTransactionStatus IN ('{1}', '{2}') AND VisitID IN (SELECT cv.VisitID FROM dbo.ConsultVisit cv WHERE cv.RegistrationID IN ({0}))", hdnSelectedRegistration.Value, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL));

                        if (lstNutritionOrderHd.Count > 0)
                        {
                            result = "confirmation|";
                        }
                        else
                        {
                            if (OnConfirm(true, ref errMessage))
                            {
                                result += "success|";
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(errMessage))
                                {
                                    result += string.Format("failed|Konfirmasi Gagal ({0})", errMessage);
                                }
                                else
                                {
                                    result += "failed|Konfirmasi Gagal";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (OnConfirm(false, ref errMessage))
                        {
                            result += "success|";
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(errMessage))
                            {
                                result += string.Format("failed|Konfirmasi Gagal ({0})", errMessage);
                            }
                            else
                            {
                                result += "failed|Konfirmasi Gagal";
                            }
                        }
                    }
                }
                else
                {
                    BindGridView();
                    result = "refresh|";
                }
            }

            
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnConfirm(bool transfer, ref string errMessage)
        {
            List<CenterbackBedTransferDTO> lstCenterbackDTO = new List<CenterbackBedTransferDTO>();

            IDbContext ctx = DbFactory.Configure(true);
            ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
            BedDao entityBedDao = new BedDao(ctx);
            BedReservationDao entityDao = new BedReservationDao(ctx);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            PatientTransferDao entityPatientTransferDao = new PatientTransferDao(ctx);
            NutritionOrderHdDao entityNutritionOrderDao = new NutritionOrderHdDao(ctx);
            try
            {
                List<Registration> lstRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID IN ({0})", hdnSelectedRegistration.Value), ctx);
                List<Bed> lstBed = BusinessLayer.GetBedList(string.Format("RegistrationID IN ({0})", hdnSelectedRegistration.Value), ctx);
                List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID IN ({0})", hdnSelectedRegistration.Value), ctx);
                List<PatientTransfer> lstPatientTransfer = BusinessLayer.GetPatientTransferList(string.Format("RegistrationID IN ({0}) AND GCPatientTransferStatus = '{1}'", hdnSelectedRegistration.Value, Constant.PatientTransferStatus.OPEN), ctx);

                CenterbackBedTranferType cbProcessType = CenterbackBedTranferType.checkin;

                foreach (Registration registration in lstRegistration)
                {
                    registration.GCRegistrationStatus = Constant.VisitStatus.CHECKED_IN;
                    registration.LastUpdatedBy = AppSession.UserLogin.UserID;

                    ConsultVisit consultVisit = lstConsultVisit.FirstOrDefault(p => p.RegistrationID == registration.RegistrationID);
                    consultVisit.GCVisitStatus = Constant.VisitStatus.CHECKED_IN;
                    consultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;

                    List<Bed> lstUpdatedBed = lstBed.Where(p => p.RegistrationID == registration.RegistrationID).ToList();
                    foreach (Bed bed in lstUpdatedBed)
                    {
                        if (bed.GCBedStatus == Constant.BedStatus.BOOKED)
                        {
                            bed.GCBedStatus = Constant.BedStatus.OCCUPIED;
                            string username = AppSession.UserLogin.UserName;
                            string confirmDate = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
                            string confirmTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                            registration.TagProperty = username + ";" + confirmDate + ";" + confirmTime + "|";
                        }
                        else
                        {
                            if (bed.GCBedStatus == Constant.BedStatus.WAIT_TO_BE_TRANSFERRED)
                            {
                                bed.GCBedStatus = Constant.BedStatus.UNOCCUPIED;
                                bed.RegistrationID = null;
                            }
                        }
                        bed.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityBedDao.Update(bed);
                    }

                    PatientTransfer entityPatientTransfer = lstPatientTransfer.FirstOrDefault(p => p.RegistrationID == registration.RegistrationID);
                    if (entityPatientTransfer != null)
                    {
                        consultVisit.ParamedicID = entityPatientTransfer.ToParamedicID;
                        consultVisit.ClassID = entityPatientTransfer.ToClassID;
                        consultVisit.ChargeClassID = entityPatientTransfer.ToChargeClassID;
                        consultVisit.SpecialtyID = entityPatientTransfer.ToSpecialtyID;
                        consultVisit.RoomID = entityPatientTransfer.ToRoomID;
                        consultVisit.BedID = entityPatientTransfer.ToBedID;
                        consultVisit.HealthcareServiceUnitID = entityPatientTransfer.ToHealthcareServiceUnitID;

                        entityPatientTransfer.GCPatientTransferStatus = Constant.PatientTransferStatus.TRANSFERRED;
                        entityPatientTransfer.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPatientTransferDao.Update(entityPatientTransfer);

                        if (transfer)
                        {
                            List<NutritionOrderHd> lstNutritionOrderHd = BusinessLayer.GetNutritionOrderHdList(string.Format("HealthcareServiceUnitID = {0} AND GCTransactionStatus IN ('{1}', '{2}') AND VisitID IN (SELECT VisitID FROM ConsultVisit WHERE RegistrationID = {3})", entityPatientTransfer.FromHealthcareServiceUnitID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, entityPatientTransfer.RegistrationID), ctx);
                            if (lstNutritionOrderHd.Count > 0)
                            {
                                foreach (NutritionOrderHd order in lstNutritionOrderHd)
                                {
                                    order.HealthcareServiceUnitID = entityPatientTransfer.ToHealthcareServiceUnitID;
                                    entityNutritionOrderDao.Update(order);
                                }
                            }
                        }

                        if (entityPatientTransfer.ReservationID != null)
                        {
                            BedReservation entity = entityDao.Get(Convert.ToInt32(entityPatientTransfer.ReservationID));
                            entity.GCReservationStatus = Constant.Bed_Reservation_Status.COMPLETE;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
                            ctx.CommitTransaction();
                        }

                        if (hdnIsBridgingToIPTV.Value == "1")
                        {
                            cbProcessType = CenterbackBedTranferType.move;
                        }

                        if (AppSession.IsBridgingToQueue)
                        {
                            try
                            {
                                List<vBed> lst = BusinessLayer.GetvBedList(string.Format("BedID IN ({0},{1})", entityPatientTransfer.FromBedID, entityPatientTransfer.ToBedID), ctx);
                                if (lst.Count > 0)
                                {
                                    vBed fromBed = lst.Where(list => list.BedID == entityPatientTransfer.FromBedID).FirstOrDefault();
                                    vBed toBed = lst.Where(list => list.BedID == entityPatientTransfer.ToBedID).FirstOrDefault();
                                    VisitInfo visitInfo = new VisitInfo();
                                    visitInfo = ConvertVisitToDTO(consultVisit.VisitID, ctx);

                                    PatientTranferInfo transferInfo = new PatientTranferInfo();
                                    transferInfo.fromRoomID = fromBed.RoomID;
                                    transferInfo.fromRoomCode = fromBed.RoomCode;
                                    transferInfo.fromRoomName = fromBed.RoomName;
                                    transferInfo.fromBedID = fromBed.BedID;
                                    transferInfo.fromBedCode = fromBed.BedCode;
                                    transferInfo.fromExtensionNo = fromBed.ExtensionNo;

                                    transferInfo.toRoomID = toBed.RoomID;
                                    transferInfo.toRoomCode = toBed.RoomCode;
                                    transferInfo.toRoomName = toBed.RoomName;
                                    transferInfo.toBedID = toBed.BedID;
                                    transferInfo.toBedCode = toBed.BedCode;
                                    transferInfo.toExtensionNo = toBed.ExtensionNo;

                                    QueueService oService = new QueueService();
                                    APIMessageLog entityAPILog = new APIMessageLog()
                                    {
                                        MessageDateTime = DateTime.Now,
                                        Recipient = Constant.BridgingVendor.QUEUE,
                                        Sender = Constant.BridgingVendor.HIS,
                                        IsSuccess = true
                                    };
                                    string apiResult = oService.SendPatientTransferInformation(registration.RegistrationID, registration.RegistrationNo, (int)registration.MRN, visitInfo, transferInfo);
                                    string[] apiResultInfo = apiResult.Split('|');
                                    if (apiResultInfo[0] == "0")
                                    {
                                        entityAPILog.IsSuccess = false;
                                        entityAPILog.Response = apiResultInfo[1];
                                        Exception ex = new Exception(apiResultInfo[1]);
                                        Helper.InsertErrorLog(ex);
                                    }
                                    else
                                        entityAPILog.MessageText = apiResultInfo[1];

                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }
                            }
                            catch (Exception ex)
                            {
                                errMessage = ex.Message;
                                Helper.InsertErrorLog(ex);
                            }
                        }
                    }
                    else
                    {
                        consultVisit.StartServiceDate = DateTime.Now;
                        consultVisit.StartServiceTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        Helper.InsertAutoBillItem(ctx, consultVisit, Constant.Facility.INPATIENT, (int)consultVisit.ChargeClassID, registration.GCCustomerType, registration.IsPrintingPatientCard, 0);
                    }
                    entityRegistrationDao.Update(registration);
                    entityConsultVisitDao.Update(consultVisit);

                    if (hdnIsBridgingToIPTV.Value == "1")
                    {
                        string processType = string.Empty;
                        switch (cbProcessType)
                        {
                            case CenterbackBedTranferType.checkin:
                                processType = "checkin";
                                break;
                            case CenterbackBedTranferType.move:
                                processType = "move";
                                break;
                        }
                        CenterbackBedTransferDTO cbObj = new CenterbackBedTransferDTO()
                        {
                            HealthcareID = AppSession.UserLogin.HealthcareID,
                            ProcessType = processType,
                            RegistrationID = registration.RegistrationID
                        };
                        lstCenterbackDTO.Add(cbObj);
                    }
                }

                ctx.CommitTransaction();

                if (hdnIsBridgingToIPTV.Value == "1")
                {
                    GatewayService oService = new GatewayService();
                    APIMessageLog entityAPILog = new APIMessageLog()
                    {
                        MessageDateTime = DateTime.Now,
                        Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                        Sender = Constant.BridgingVendor.HIS,
                        IsSuccess = true
                    };
                    string apiResult = oService.IPTV_BedTransfer(lstCenterbackDTO);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.Response = apiResultInfo[1];
                        entityAPILog.MessageText = apiResultInfo[2];
                        Exception ex = new Exception(apiResultInfo[1]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                        entityAPILog.MessageText = apiResultInfo[2];

                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                }

                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                return false;
            }
            finally
            {
                ctx.Close();
            }
        }

        #region Bridging to Queue - Methods
        private VisitInfo ConvertVisitToDTO(int visitID, IDbContext ctx)
        {
            vConsultVisit oConsultVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", visitID), ctx).FirstOrDefault();
            VisitInfo visitInfo = new VisitInfo();
            visitInfo.VisitID = oConsultVisit.VisitID;
            visitInfo.VisitDate = oConsultVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            visitInfo.VisitTime = oConsultVisit.VisitTime;
            visitInfo.DepartmentCode = Constant.Facility.INPATIENT;
            visitInfo.ServiceUnitCode = oConsultVisit.ServiceUnitCode;
            visitInfo.ServiceUnitName = oConsultVisit.ServiceUnitName;
            visitInfo.RoomID = oConsultVisit.RoomID;
            visitInfo.RoomCode = oConsultVisit.RoomCode;
            visitInfo.RoomName = oConsultVisit.RoomName;
            visitInfo.PhysicianID = oConsultVisit.ParamedicID;
            visitInfo.PhysicianCode = oConsultVisit.ParamedicCode;
            visitInfo.PhysicianName = oConsultVisit.ParamedicName;
            visitInfo.SpecialtyName = oConsultVisit.SpecialtyName;
            visitInfo.BedCode = oConsultVisit.BedCode;
            visitInfo.DischargeDate = "";
            visitInfo.DischargeTime = "";
            return visitInfo;
        }
        #endregion
    }
}