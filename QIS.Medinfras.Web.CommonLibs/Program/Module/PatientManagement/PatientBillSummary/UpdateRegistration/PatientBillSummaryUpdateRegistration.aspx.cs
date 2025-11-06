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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryUpdateRegistration : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_UPDATE_REGISTRATION;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_UPDATE_REGISTRATION;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_UPDATE_REGISTRATION;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_UPDATE_REGISTRATION;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_UPDATE_REGISTRATION;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_UPDATE_REGISTRATION;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_UPDATE_TRANSACTION_STATUS;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_UPDATE_REGISTRATION;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_UPDATE_REGISTRATION;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string OnGetParamedicFilterExpression()
        {
            return string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND IsDeleted = 0 AND (IsHasPhysicianRole = 1)", hdnHealthcareServiceUnitID.Value);
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            hdnRegistrationDate.Value = entity.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnRegistrationHour.Value = entity.RegistrationTime;
            txtRegistrationID.Text = entity.RegistrationNo;
            txtMRN.Text = string.Format("{0} - {1}", entity.MedicalNo, entity.PatientName);
            txtServiceUnit.Text = entity.ServiceUnitName;
            hdnPhysicianID.Value = hdnPhysicianIDORI.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            hdnReferrerID.Value = entity.ReferrerID.ToString();
            hdnReferrerParamedicID.Value = entity.ReferrerParamedicID.ToString();

            if (entity.ReferrerCode != "")
            {
                txtReferralDescriptionCode.Text = entity.ReferrerCode;
            }

            if (entity.ReferrerName != "")
            {
                txtReferralDescriptionName.Text = entity.ReferrerName;
            }

            if (entity.ReferrerParamedicCode != "")
            {
                txtReferralDescriptionCode.Text = entity.ReferrerParamedicCode;
            }

            if (entity.ReferrerParamedicName != "")
            {
                txtReferralDescriptionName.Text = entity.ReferrerParamedicName;
            }

            txtReferralNo.Text = entity.ReferralNo;

            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnVisitID.Value = entity.VisitID.ToString();
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            hdnDepartmentID.Value = entity.DepartmentID;

            if (entity.DepartmentID != Constant.Facility.INPATIENT)
                trChargeClass.Style.Add("display", "none");

            List<Specialty> lstSpecialty = BusinessLayer.GetSpecialtyList("IsDeleted = 0");
            Methods.SetComboBoxField<Specialty>(cboRegistrationEditSpecialty, lstSpecialty, "SpecialtyName", "SpecialtyID");
            cboRegistrationEditSpecialty.Value = entity.SpecialtyID.ToString();

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0 AND IsUsedInChargeClass = 1");
            Methods.SetComboBoxField<ClassCare>(cboChargeClassID, lstClassCare, "ClassName", "ClassID");
            cboChargeClassID.Value = entity.ChargeClassID.ToString();

            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REFERRAL, Constant.StandardCode.CUSTOMER_TYPE, Constant.StandardCode.ADMISSION_SOURCE, Constant.StandardCode.VISIT_REASON, Constant.StandardCode.ADMISSION_CONDITION);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "0", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboReferral, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.REFERRAL || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            cboReferral.SelectedIndex = 0;
            cboReferral.Value = entity.GCReferrerGroup.ToString();

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_IS_PARAMEDIC_IN_REGISTRATION_USE_SCHEDULE));
            hdnIsParamedicInRegistrationUseSchedule.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.RM_IS_PARAMEDIC_IN_REGISTRATION_USE_SCHEDULE).ParameterValue;
            if (hdnIsParamedicInRegistrationUseSchedule.Value == "1")
            {
                if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY || hdnDepartmentID.Value == Constant.Facility.INPATIENT || hdnDepartmentID.Value == Constant.Facility.PHARMACY)
                {
                    trParamedicHasSchedule.Style.Add("display", "none");
                }
                else
                {
                    trParamedicHasSchedule.Style.Remove("display");
                }
            }
            else
            {
                trParamedicHasSchedule.Style.Add("display", "none");
            }

            Helper.SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true), "mpEditReg");
            Helper.SetControlEntrySetting(cboRegistrationEditSpecialty, new ControlEntrySetting(true, true, true), "mpEditReg");
            Helper.SetControlEntrySetting(cboChargeClassID, new ControlEntrySetting(true, true, true), "mpEditReg");
            Helper.SetControlEntrySetting(cboReferral, new ControlEntrySetting(true, true), "mpEditReg");
            Helper.SetControlEntrySetting(txtReferralDescriptionCode, new ControlEntrySetting(true, true), "mpEditReg");
            Helper.SetControlEntrySetting(txtReferralNo, new ControlEntrySetting(true, true), "mpEditReg");
        }

        protected string GetDayNumber()
        {
            DateTime selectedDate = Helper.GetDatePickerValue(hdnRegistrationDate.Value);

            //daynumber diubah jika 0 jadi 7 karena di database jika hari minggu disimpan daynumber 7, sedangkan di coding daynumber untuk hari minggu adalah 0
            int daynumber = (int)selectedDate.DayOfWeek;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            return daynumber.ToString();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            ConsultVisitDao consVisitDao = new ConsultVisitDao(ctx);
            ParamedicMasterDao paramedicDao = new ParamedicMasterDao(ctx);
            RegistrationBPJSDao entityRegistrationBPJSDao = new RegistrationBPJSDao(ctx);
            PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);

            try
            {
                ConsultVisit entity = consVisitDao.Get(Convert.ToInt32(hdnVisitID.Value));
                Registration entityReg = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));

                List<PatientChargesDt> lstChargesDt = new List<PatientChargesDt>();
                if (entity.ParamedicID != Convert.ToInt32(hdnPhysicianID.Value))
                {
                    ParamedicMaster pm = paramedicDao.Get(Convert.ToInt32(entity.ParamedicID));
                    if (pm.IsDummy == true || pm.IsHasRevenueSharing == false)
                    {
                        string filterChargesHd = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND IsAutoTransaction = 1", entity.VisitID, Constant.TransactionStatus.VOID);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<PatientChargesHd> lstCharegesHd = BusinessLayer.GetPatientChargesHdList(filterChargesHd, ctx);
                        if (lstCharegesHd.Count > 0)
                        {
                            foreach (PatientChargesHd hd in lstCharegesHd)
                            {
                                //string filterChargesDt = string.Format("TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}' AND ParamedicID = '{2}'", hd.TransactionID, Constant.TransactionStatus.VOID, entity.ParamedicID);
                                //ctx.CommandType = CommandType.Text;
                                //ctx.Command.Parameters.Clear();
                                //List<PatientChargesDt> lstDt = BusinessLayer.GetPatientChargesDtList(filterChargesDt, ctx);
                                //if (lstDt.Count > 0)
                                //{
                                //    lstChargesDt.AddRange(lstDt);
                                //}
                                string filterCHargesDt = string.Format("TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}' AND ParamedicID = '{2}'", hd.TransactionID, Constant.TransactionStatus.VOID, entity.ParamedicID);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<PatientChargesDt> lstDt = BusinessLayer.GetPatientChargesDtList(filterCHargesDt, ctx);
                                foreach (PatientChargesDt e in lstDt)
                                {
                                    e.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                                    e.LastUpdatedBy = AppSession.UserLogin.UserID;

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    chargesDtDao.Update(e);
                                }
                            }
                        }
                    }
                    else
                    {
                        string filterChargesHd = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND IsAutoTransaction = 1", entity.VisitID, Constant.TransactionStatus.VOID);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<PatientChargesHd> lstCharegesHd = BusinessLayer.GetPatientChargesHdList(filterChargesHd, ctx);
                        if (lstCharegesHd.Count > 0)
                        {
                            foreach (PatientChargesHd hd in lstCharegesHd)
                            {
                                string filterCHargesDt = string.Format("TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}' AND ParamedicID = '{2}'", hd.TransactionID, Constant.TransactionStatus.VOID, entity.ParamedicID);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<PatientChargesDt> lstDt = BusinessLayer.GetPatientChargesDtList(filterCHargesDt, ctx);
                                if (lstDt.Count > 0)
                                {
                                    lstChargesDt.AddRange(lstDt);
                                }
                            }
                        }
                    }
                }
                else
                {
                    string filterChargesHd = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND IsAutoTransaction = 1", entity.VisitID, Constant.TransactionStatus.VOID);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<PatientChargesHd> lstCharegesHd = BusinessLayer.GetPatientChargesHdList(filterChargesHd, ctx);
                    if (lstCharegesHd.Count > 0)
                    {
                        foreach (PatientChargesHd hd in lstCharegesHd)
                        {
                            string filterCHargesDt = string.Format("TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}' AND ParamedicID = '{2}'", hd.TransactionID, Constant.TransactionStatus.VOID, entity.ParamedicID);
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<PatientChargesDt> lstDt = BusinessLayer.GetPatientChargesDtList(filterCHargesDt, ctx);
                            if (lstDt.Count > 0)
                            {
                                lstChargesDt.AddRange(lstDt);
                            }
                        }
                    }
                }

                if (lstChargesDt.Count <= 0)
                {
                    entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                    entity.ChargeClassID = Convert.ToInt32(cboChargeClassID.Value);

                    if (cboReferral.Value == null)
                    {
                        entityReg.GCReferrerGroup = null;
                    }
                    else
                    {
                        entityReg.GCReferrerGroup = cboReferral.Value.ToString();
                    }

                    if (hdnReferrerID.Value == "" || hdnReferrerID.Value == "0")
                        entityReg.ReferrerID = null;
                    else
                        entityReg.ReferrerID = Convert.ToInt32(hdnReferrerID.Value);

                    if (hdnReferrerParamedicID.Value == "" || hdnReferrerParamedicID.Value == "0")
                        entityReg.ReferrerParamedicID = null;
                    else
                        entityReg.ReferrerParamedicID = Convert.ToInt32(hdnReferrerParamedicID.Value);

                    entityReg.ReferralNo = txtReferralNo.Text;

                    entity.SpecialtyID = cboRegistrationEditSpecialty.Value.ToString();
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    consVisitDao.Update(entity);

                    entityReg.LastUpdatedBy = AppSession.UserLogin.UserID;
                    registrationDao.Update(entityReg);

                    RegistrationBPJS regBPJS = entityRegistrationBPJSDao.Get(entity.RegistrationID);
                    if (regBPJS != null)
                    {
                        ParamedicMaster entityParamedic = paramedicDao.Get(Convert.ToInt32(entity.ParamedicID));
                        if (entityParamedic != null)
                        {
                            if (entityParamedic.BPJSReferenceInfo != null && entityParamedic.BPJSReferenceInfo != "")
                            {
                                string[] bpjsInfo = entityParamedic.BPJSReferenceInfo.Split(';');
                                string[] hfisInfo = bpjsInfo[1].Split('|');
                                regBPJS.KodeDPJP = hfisInfo[0];
                            }
                        }

                        regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityRegistrationBPJSDao.Update(regBPJS);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, tidak dapat mengubah data registrasi karna masih memiliki Transaksi Auto Bill dari dokter saat ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
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