using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class CancelPatientDischargeList : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.CANCEL_PATIENT_DISCHARGE;
        }

        protected string GetPageTitle()
        {
            return BusinessLayer.GetMenuMasterList(string.Format("MenuCode='{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void InitializeDataControl()
        {
            txtDischargeDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            Helper.SetControlEntrySetting(txtDischargeDate, new ControlEntrySetting(true, true, true), "mpBedStatus");

            BindGridView();

        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("GCVisitStatus IN ('{0}','{1}') AND DischargeDate = '{2}' AND GCDischargeCondition NOT IN ('{3}','{4}')",
                            Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.PHYSICIAN_DISCHARGE,
                            Helper.GetDatePickerValue(txtDischargeDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                            Constant.PatientOutcome.DEAD_BEFORE_48, Constant.PatientOutcome.DEAD_AFTER_48);
            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpCancelPatientDischarge_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
            BedDao entityBedDao = new BedDao(ctx);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            string result = "success";
            bool flagResult = true;
            try
            {
                List<vConsultVisit> lstConsultVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID IN ({0})", hdnSelectedVisit.Value), ctx);
                List<Bed> lstBed = BusinessLayer.GetBedList(string.Format("BedID IN (SELECT BedID FROM ConsultVisit WHERE VisitID IN ({0}))", hdnSelectedVisit.Value), ctx);
                List<Registration> lstRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID IN (SELECT RegistrationID FROM ConsultVisit WHERE VisitID IN ({0}))", hdnSelectedVisit.Value), ctx);
                foreach (vConsultVisit entity in lstConsultVisit)
                {
                    ConsultVisit entityCV = entityConsultVisitDao.Get(entity.VisitID);
                    Registration entityRegistration = lstRegistration.Where(p => p.RegistrationID == entity.RegistrationID).FirstOrDefault();
                    entityRegistration.GCRegistrationStatus = Constant.VisitStatus.CHECKED_IN;
                    entityRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityCV.GCVisitStatus = Constant.VisitStatus.CHECKED_IN;
                    entityCV.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityCV.DischargeTime = null;
                    entityCV.DischargeDate = Convert.ToDateTime("1900-01-01");
                    entityCV.GCDischargeMethod = null;
                    entityCV.GCDischargeCondition = null;
                    entityConsultVisitDao.Update(entityCV);
                    entityRegistrationDao.Update(entityRegistration);
                    if (entity.DepartmentID == Constant.Facility.INPATIENT)
                    {
                        Bed bed = lstBed.FirstOrDefault(p => p.BedID == entityCV.BedID);
                        if (bed.GCBedStatus != Constant.BedStatus.UNOCCUPIED)
                        {
                            result = string.Format("fail|{0}", string.Format("Bed {0} Cannot Be Used", bed.BedCode));
                            flagResult = false;
                            break;
                        }
                        bed.GCBedStatus = Constant.BedStatus.OCCUPIED;
                        bed.RegistrationID = entity.RegistrationID;
                        bed.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityBedDao.Update(bed);
                        if (AppSession.IsBridgingToQueue)
                        {
                            //If Bridging to Queue - Send Information
                            try
                            {
                                VisitInfo visitInfo = new VisitInfo();
                                vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", entityCV.VisitID), ctx).FirstOrDefault();
                                visitInfo = ConvertVisitToDTO(oVisit);
                                PatientData patientInfo = ConvertPatientToDTO(oVisit);
                                APIMessageLog entityAPILog = new APIMessageLog()
                                {
                                    MessageDateTime = DateTime.Now,
                                    Recipient = Constant.BridgingVendor.QUEUE,
                                    Sender = Constant.BridgingVendor.HIS,
                                    IsSuccess = true
                                };
                                QueueService oService = new QueueService();
                                string apiResult = oService.SendVisitInformation(entityRegistration, visitInfo, patientInfo);
                                string[] apiResultInfo = apiResult.Split('|');
                                if (apiResultInfo[0] == "0")
                                {
                                    entityAPILog.IsSuccess = false;
                                    entityAPILog.Response = apiResultInfo[1];
                                    Exception ex = new Exception(apiResultInfo[1]);
                                    Helper.InsertErrorLog(ex);
                                }
                                else entityAPILog.MessageText = apiResultInfo[1];
                                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            }
                            catch (Exception ex)
                            {
                                Helper.InsertErrorLog(ex);
                            }
                        }
                    }
                }

                if (flagResult) ctx.CommitTransaction();
                else ctx.RollBackTransaction();
            }
            catch (Exception ex)
            {
                result = string.Format("fail|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Bridging to Queue - Methods
        private VisitInfo ConvertVisitToDTO(vConsultVisit entityVisit)
        {
            VisitInfo visitInfo = new VisitInfo();
            visitInfo.VisitID = entityVisit.VisitID;
            visitInfo.VisitDate = entityVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            visitInfo.VisitTime = entityVisit.VisitTime;
            visitInfo.DepartmentCode = Constant.Facility.INPATIENT;
            visitInfo.ServiceUnitCode = entityVisit.ServiceUnitCode;
            visitInfo.ServiceUnitName = entityVisit.ServiceUnitName;
            visitInfo.RoomID = entityVisit.RoomID;
            visitInfo.RoomCode = entityVisit.RoomCode;
            visitInfo.RoomName = entityVisit.RoomName;
            visitInfo.PhysicianID = entityVisit.ParamedicID;
            visitInfo.PhysicianCode = entityVisit.ParamedicCode;
            visitInfo.PhysicianName = entityVisit.ParamedicName;
            visitInfo.SpecialtyName = entityVisit.SpecialtyName;
            visitInfo.BedCode = entityVisit.BedCode;
            visitInfo.ExtensionNo = entityVisit.ExtensionNo;
            return visitInfo;
        }
        private PatientData ConvertPatientToDTO(vConsultVisit oVisit)
        {
            PatientData oData = new PatientData();
            oData.PatientID = oVisit.MRN;
            oData.MedicalNo = oVisit.MedicalNo;
            oData.FirstName = oVisit.FirstName;
            oData.MiddleName = oVisit.MiddleName;
            oData.LastName = oVisit.LastName;
            oData.PrefferedName = oVisit.PreferredName;
            oData.Gender = string.Format("{0}^{1}", oVisit.GCGender.Substring(5), oVisit.Gender);
            oData.Religion = string.Empty;
            oData.MaritalStatus = string.Empty;
            oData.Nationality = string.Empty;

            oData.DateOfBirth = oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112);
            oData.CityOfBirth = oVisit.CityOfBirth;
            oData.HomeAddress = oVisit.HomeAddress;
            oData.MobileNo1 = oVisit.MobilePhoneNo1;
            oData.MobileNo2 = oVisit.MobilePhoneNo2;
            oData.EmailAddress = string.Empty;
            return oData;
        }
        #endregion
    }
}