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
    public partial class TransactionPageSummaryUpdateRegistration : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EmergencyCare.TRANSACTION_PAGE_SUMMARY_UPDATE_REGISTRATION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string OnGetParamedicFilterExpression()
        {
            return string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND IsDeleted = 0", hdnHealthcareServiceUnitID.Value);
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            txtRegistrationID.Text = entity.RegistrationNo;
            txtMRN.Text = string.Format("{0} - {1}", entity.MedicalNo, entity.PatientName);
            txtServiceUnit.Text = entity.ServiceUnitName;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
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

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0");
            Methods.SetComboBoxField<ClassCare>(cboChargeClassID, lstClassCare, "ClassName", "ClassID");
            cboChargeClassID.Value = entity.ChargeClassID.ToString();

            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REFERRAL, Constant.StandardCode.CUSTOMER_TYPE, Constant.StandardCode.ADMISSION_SOURCE, Constant.StandardCode.VISIT_REASON, Constant.StandardCode.ADMISSION_CONDITION);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboReferral, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.REFERRAL || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            cboReferral.Value = entity.GCReferrerGroup.ToString();

            Helper.SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true), "mpEditReg");
            Helper.SetControlEntrySetting(cboRegistrationEditSpecialty, new ControlEntrySetting(true, true, true), "mpEditReg");
            Helper.SetControlEntrySetting(cboChargeClassID, new ControlEntrySetting(true, true, true), "mpEditReg");
            Helper.SetControlEntrySetting(cboReferral, new ControlEntrySetting(true, true, false), "mpEditReg");
            Helper.SetControlEntrySetting(txtReferralDescriptionCode, new ControlEntrySetting(true, true, false), "mpEditReg");
            Helper.SetControlEntrySetting(txtReferralNo, new ControlEntrySetting(true, true, false), "mpEditReg");
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

            try
            {
                ConsultVisit entity = consVisitDao.Get(Convert.ToInt32(hdnVisitID.Value));
                Registration entityReg = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));

                List<PatientChargesDt> lstChargesDt = new List<PatientChargesDt>();
                if (entity.ParamedicID != Convert.ToInt32(hdnPhysicianID.Value))
                {
                    string filterChargesHd = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND IsAutoTransaction = 1", entity.VisitID, Constant.TransactionStatus.VOID);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<PatientChargesHd> lstCharegesHd = BusinessLayer.GetPatientChargesHdList(filterChargesHd, ctx);
                    if (lstCharegesHd.Count > 0)
                    {
                        foreach (PatientChargesHd hd in lstCharegesHd)
                        {
                            string filterCHargesDt = string.Format("TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", hd.TransactionID, Constant.TransactionStatus.VOID);
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