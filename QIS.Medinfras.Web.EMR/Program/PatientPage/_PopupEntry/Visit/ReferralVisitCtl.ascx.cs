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
using System.Data;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class ReferralVisitCtl : BaseViewPopupCtl
    {
        protected bool IsAllowEditPatientVisit = true;
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnItemCardFee.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU).ParameterValue;
                if (string.IsNullOrEmpty(hdnItemCardFee.Value)) hdnItemCardFee.Value = "0";
                txtMRN.ReadOnly = true;
                txtPatientName.ReadOnly = true;
                txtRegistrationNo.ReadOnly = true;

                hdnRegistrationID.Value = param;

                hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
                txtRegistrationNo.Text = AppSession.RegisteredPatient.RegistrationNo;
                txtMRN.Text = AppSession.RegisteredPatient.MedicalNo;
                txtPatientName.Text = AppSession.RegisteredPatient.PatientName;
                hdnClassID.Value = AppSession.RegisteredPatient.ChargeClassID.ToString();
                hdnBusinessPartnerID.Value = AppSession.RegisteredPatient.BusinessPartnerID.ToString();

                IsAllowEditPatientVisit = (AppSession.RegisteredPatient.VisitDate == DateTime.Today && AppSession.RegisteredPatient.GCRegistrationStatus != Constant.VisitStatus.CANCELLED && AppSession.RegisteredPatient.GCRegistrationStatus != Constant.VisitStatus.CLOSED);

                if (!IsAllowEditPatientVisit)
                    divContainerAddData.Style.Add("display", "none");

                BindGridView();

                txtPhysicianCode.Attributes.Add("validationgroup", "mpPatientVisit");
                txtClinicCode.Attributes.Add("validationgroup", "mpPatientVisit");
                txtVisitTypeCode.Attributes.Add("validationgroup", "mpPatientVisit");
                txtDiagnosisText.Attributes.Add("validationgroup", "mpPatientVisit");
                txtMedicalResumeText.Attributes.Add("validationgroup", "mpPatientVisit");
                txtPlanningResumeText.Attributes.Add("validationgroup", "mpPatientVisit");

                List<Specialty> lstSpecialty = BusinessLayer.GetSpecialtyList("IsDeleted = 0");
                Methods.SetComboBoxField<Specialty>(cboRegistrationEditSpecialty, lstSpecialty, "SpecialtyName", "SpecialtyID");
                cboRegistrationEditSpecialty.SelectedIndex = 0;
            }
        }

        private void BindGridView()
        {
            grdPatientVisitTransHd.DataSource = BusinessLayer.GetvReferralConsultVisitList(string.Format("RegistrationID = {0} AND GCVisitStatus != '{1}' ORDER BY VisitID ASC", hdnRegistrationID.Value, Constant.VisitStatus.CANCELLED));
            grdPatientVisitTransHd.DataBind();
        }

        protected void cbpPatientVisitTransHd_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnVisitID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else
            {
                result = "delete|";
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(ConsultVisit entity)
        {
            entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
            entity.SpecialtyID = cboRegistrationEditSpecialty.Value.ToString();
            entity.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
            entity.GCVisitReason = Constant.VisitReason.OTHER;
            entity.ClassID = entity.ChargeClassID = Convert.ToInt32(hdnClassID.Value);
            entity.RoomID = null;
            entity.BedID = null;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegDao = new RegistrationDao(ctx);
            ConsultVisitDao entityVisitDao = new ConsultVisitDao(ctx);
            PatientReferralDao entityReferralDao = new PatientReferralDao(ctx);
            SettingParameterDao entitySettingParameterDao = new SettingParameterDao(ctx);
            int visitID = 0;
            try
            {
                Registration reg = entityRegDao.Get(Convert.ToInt32(hdnRegistrationID.Value));

                #region Consult Visit
                ConsultVisit entityVisit = new ConsultVisit();
                ControlToEntity(entityVisit);
                entityVisit.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                entityVisit.VisitDate = entityVisit.ActualVisitDate = DateTime.Today;
                entityVisit.VisitTime = entityVisit.ActualVisitTime = DateTime.Now.ToString("HH:mm");

                string registrationStatus = "";
                if (entitySettingParameterDao.Get(Constant.SettingParameter.IS_OUTPATIENT_REGISTRATION_AUTOMATICALLY_CHECKED_IN).ParameterValue == "1")
                    registrationStatus = Constant.VisitStatus.CHECKED_IN;
                else
                    registrationStatus = Constant.VisitStatus.OPEN;

                entityVisit.ClassID = entityVisit.ChargeClassID = Convert.ToInt32(hdnClassID.Value);
                entityVisit.GCVisitStatus = registrationStatus;
                entityVisit.CreatedBy = AppSession.UserLogin.UserID;
                entityVisit.IsMainVisit = false;
                entityVisit.QueueNo = BusinessLayer.GetConsultVisitMaxQueueNo(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND VisitDate = '{2}'", entityVisit.HealthcareServiceUnitID, entityVisit.ParamedicID, entityVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx);
                entityVisit.QueueNo++;
                visitID = entityVisitDao.InsertReturnPrimaryKeyID(entityVisit);

                if (registrationStatus == Constant.VisitStatus.CHECKED_IN)
                {
                    entityVisit.VisitID = visitID;
                    Helper.InsertAutoBillItem(ctx, entityVisit, Constant.Facility.OUTPATIENT, Convert.ToInt32(hdnClassID.Value), reg.GCCustomerType, false, 0);
                } 
                #endregion

                #region Patient Referral Information
                PatientReferral entity = new PatientReferral();
                ReferralControlToEntity(entityVisit,entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityReferralDao.Insert(entity);
                #endregion


                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private void ReferralControlToEntity(ConsultVisit entityVisit,PatientReferral entity)
        {
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.ReferralDate = AppSession.RegisteredPatient.VisitDate;
            entity.ReferralTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            entity.GCRefferalType = "X075^01";
            entity.FromPhysicianID =  Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            entity.ToHealthcareServiceUnitID = entityVisit.HealthcareServiceUnitID;
            entity.ToVisitID = entityVisit.VisitID;
            entity.ToPhysicianID = Convert.ToInt32(hdnPhysicianID.Value);
            entity.DiagnosisText = txtDiagnosisText.Text;
            entity.MedicalResumeText = txtMedicalResumeText.Text;
            entity.PlanningResumeText = txtPlanningResumeText.Text;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                ConsultVisit entity = BusinessLayer.GetConsultVisit(Convert.ToInt32(hdnVisitID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateConsultVisit(entity);

                PatientReferral entityReferral = BusinessLayer.GetPatientReferral(Convert.ToInt32(hdnPatientReferralID.Value));
                if (entityReferral != null)
                {
                    entityReferral.DiagnosisText = txtDiagnosisText.Text;
                    entityReferral.MedicalResumeText = txtMedicalResumeText.Text;
                    entityReferral.PlanningResumeText = txtPlanningResumeText.Text;
                    entityReferral.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePatientReferral(entityReferral);
                }

                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            ConsultVisitDao entityVisitDao = new ConsultVisitDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientReferralDao patientReferralDao = new PatientReferralDao(ctx);
            try
            {
                string filterOutstanding = string.Format("VisitID = {0}", hdnVisitID.Value);
                vRegistrationAllInfo lstInfo = BusinessLayer.GetvRegistrationAllInfoList(filterOutstanding).FirstOrDefault();
                bool outstanding = (lstInfo.Charges + lstInfo.TestOrder + lstInfo.PrescriptionOrder + lstInfo.PrescriptionReturnOrder + lstInfo.ServiceOrder + lstInfo.Payment > 0);

                if (outstanding)
                {
                    result = false;
                    errMessage = "Kunjungan tidak dapat dibatalkan karena sudah memiliki order / transaksi / pembayaran.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                else
                {
                    string filterExpression = string.Format("RegistrationID = {0} AND GCTransactionDetailStatus != '{1}' AND IsDeleted = 0", hdnRegistrationID.Value, Constant.TransactionStatus.VOID);
                    List<vPatientChargesDt3> lstPatientChargesDt = BusinessLayer.GetvPatientChargesDt3List(filterExpression, ctx);
                    List<vPatientChargesDt3> lstPatientChargesDtTemp = lstPatientChargesDt.Where(t => !t.IsCreatedBySystem || (t.PatientBillingID != 0)).ToList();

                    string filterPayment = string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID);
                    List<PatientPaymentHd> lstPatientPaymentHd = BusinessLayer.GetPatientPaymentHdList(filterPayment, ctx);

                    string filterCC = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);
                    List<ChiefComplaint> lstCC = BusinessLayer.GetChiefComplaintList(filterCC, ctx);

                    string filterNCC = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);
                    List<NurseChiefComplaint> lstNCC = BusinessLayer.GetNurseChiefComplaintList(filterCC, ctx);

                    if (lstCC.Count > 0)
                    {
                        #region Cek Chief Complaint

                        result = false;
                        errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_REGISTRATION_CANNOT_VOID_BECAUSE_CHIEF_COMPLAINT_ANAMNESA);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();

                        #endregion
                    }
                    else
                    {
                        if (lstNCC.Count > 0)
                        {
                            #region Cek Anamnese

                            result = false;
                            errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_REGISTRATION_CANNOT_VOID_BECAUSE_CHIEF_COMPLAINT_ANAMNESA);
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();

                            #endregion
                        }
                        else
                        {
                            lstPatientChargesDtTemp = lstPatientChargesDt.Where(t => t.IsCreatedBySystem).ToList();
                            List<string> lstTransactionID = new List<string>();
                            foreach (vPatientChargesDt3 entityDt in lstPatientChargesDtTemp)
                            {
                                PatientChargesDt entityPatientChargesDt = patientChargesDtDao.Get(entityDt.ID);
                                entityPatientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                entityPatientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                patientChargesDtDao.Update(entityPatientChargesDt);
                                if (!lstTransactionID.Any(t => t == entityDt.TransactionID.ToString()))
                                {
                                    lstTransactionID.Add(entityDt.TransactionID.ToString());
                                }
                            }

                            foreach (string entityTransactionID in lstTransactionID)
                            {
                                List<PatientChargesDt> lstChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = '{0}'", entityTransactionID), ctx).ToList();
                                if (lstChargesDt.Where(t => t.GCTransactionDetailStatus != Constant.TransactionStatus.VOID).Count() == 0)
                                {
                                    PatientChargesHd entityPatientChargesHd = patientChargesHdDao.Get(Convert.ToInt32(entityTransactionID));
                                    entityPatientChargesHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    entityPatientChargesHd.VoidBy = AppSession.UserLogin.UserID;
                                    entityPatientChargesHd.VoidDate = DateTime.Now;
                                    entityPatientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientChargesHdDao.Update(entityPatientChargesHd);
                                }
                            }

                            ConsultVisit entityVisit = entityVisitDao.Get(Convert.ToInt32(hdnVisitID.Value));
                     
                            entityVisit.GCVisitStatus = Constant.VisitStatus.CANCELLED;
                            entityVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityVisitDao.Update(entityVisit);
                        }

                        PatientReferral entityReferral = patientReferralDao.Get(Convert.ToInt32(hdnPatientReferralID.Value));
                        if (entityReferral != null)
                        {
                            entityReferral.IsDeleted = true;
                            entityReferral.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientReferralDao.Update(entityReferral); 
                        }

                        ctx.CommitTransaction();

                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
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