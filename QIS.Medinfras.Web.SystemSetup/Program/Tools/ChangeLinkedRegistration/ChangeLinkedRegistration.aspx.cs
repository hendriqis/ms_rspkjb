using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ChangeLinkedRegistration : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.CHANGE_LINKED_REGISTRATION;
        }

        protected override void InitializeDataControl()
        {

        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ADMISSION_SOURCE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            List<StandardCode> lstAdmissionSource = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ADMISSION_SOURCE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboAdmissionSource, lstAdmissionSource, "StandardCodeName", "StandardCodeID");

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                AppSession.UserLogin.HealthcareID,
                Constant.SettingParameter.IP_REGISTRATION_BLOCK_FROM_REGISTRATION_24JAM));
            hdnRegistrasiAsalDiblokJikaLebih24Jam.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IP_REGISTRATION_BLOCK_FROM_REGISTRATION_24JAM).ParameterValue;
        }

        protected string GetPageTitle()
        {
            return BusinessLayer.GetMenuMasterList(string.Format("MenuCode='{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRegistrationHour, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRegistrationNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMRN, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPatientName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPreferredName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtGender, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDOB, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAgeInYear, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAgeInMonth, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAgeInDay, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAddress, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboAdmissionSource, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtFromRegistrationNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFromMRN, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFromPatientName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFromPreferredName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFromGender, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFromDOB, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFromAgeInYear, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFromAgeInMonth, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFromAgeInDay, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFromAddress, new ControlEntrySetting(false, false, false));
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityDao = new RegistrationDao(ctx);

            if (type == "process")
            {
                try
                {
                    Registration entity = entityDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                    entity.GCAdmissionSource = cboAdmissionSource.Value.ToString();

                    if (hdnFirstFromRegistrationID.Value != "0" && hdnFirstFromRegistrationID.Value != "")
                    {
                        if (hdnFromRegistrationID.Value != "0" && hdnFromRegistrationID.Value != "")
                        {
                            if (hdnFirstFromRegistrationID.Value != hdnFromRegistrationID.Value)
                            {
                                #region Ada Old_FromRegistrationID && Ada New_FromRegistrationID

                                Registration entityFromFirst = entityDao.Get(Convert.ToInt32(hdnFirstFromRegistrationID.Value));
                                Registration entityFrom = entityDao.Get(Convert.ToInt32(hdnFromRegistrationID.Value));
                                Registration entityOld = new Registration();

                                if (entity.MRN == entityFrom.MRN)
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    string filterVisit = string.Format("RegistrationID IN ({0}) AND GCVisitStatus != '{1}'", entityFromFirst.RegistrationID, Constant.VisitStatus.CANCELLED);
                                    if (entity.LinkedRegistrationID != null && entity.LinkedRegistrationID != 0)
                                    {
                                        filterVisit = string.Format("RegistrationID IN ({0},{1}) AND GCVisitStatus != '{2}'", entity.LinkedRegistrationID, entityFromFirst.RegistrationID, Constant.VisitStatus.CANCELLED);
                                        entityOld = entityDao.Get(Convert.ToInt32(entity.LinkedRegistrationID));
                                    }

                                    List<ConsultVisit> lstVisitFrom = BusinessLayer.GetConsultVisitList(filterVisit, ctx);
                                    bool isHasChargesTransfer = false;
                                    foreach (ConsultVisit v in lstVisitFrom)
                                    {
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        string filterCharges = string.Format("VisitID = {0} AND IsChargesTransfered = 1 AND GCTransactionStatus != '{1}'", v.VisitID, Constant.TransactionStatus.VOID);
                                        List<PatientChargesHd> lstChargesHd = BusinessLayer.GetPatientChargesHdList(filterCharges, ctx);
                                        if (lstChargesHd.Count > 0)
                                        {
                                            isHasChargesTransfer = true;
                                        }
                                    }

                                    if (!isHasChargesTransfer)
                                    {
                                        entityFromFirst.LinkedToRegistrationID = null;
                                        entityFromFirst.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityDao.Update(entityFromFirst);

                                        entityOld.LinkedToRegistrationID = null;
                                        entityOld.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        //entityDao.Update(entityOld);

                                        entity.LinkedRegistrationID = Convert.ToInt32(hdnFromRegistrationID.Value);
                                        entity.IsNewPatient = hdnFromRegistrationIsNewPatient.Value == "1";
                                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityDao.Update(entity);

                                        entityFrom.LinkedToRegistrationID = entity.RegistrationID;
                                        entityFrom.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        //entityDao.Update(entityFrom);

                                        if (entityOld.RegistrationID == entityFrom.RegistrationID)
                                        {
                                            entityOld = entityFrom;
                                            entityDao.Update(entityOld);
                                        }
                                        else
                                        {
                                            entityDao.Update(entityOld);
                                            entityDao.Update(entityFrom);
                                        }

                                        ctx.CommitTransaction();
                                    }
                                    else
                                    {
                                        result = false;
                                        errMessage = "Maaf, Registrasi ini sudah melakukan transfer tagihan dari registrasi asal, harap batalkan transfer tagihan terlebih dahulu";
                                        ctx.RollBackTransaction();
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = "Maaf Data Pasien dari Registrasi Asal dan Tujuan Berbeda";
                                    ctx.RollBackTransaction();
                                }

                                #endregion
                            }
                        }
                        else
                        {
                            #region Ada Old_FromRegistrationID && TidakAda New_FromRegistrationID

                            Registration entityFrom = entityDao.Get(Convert.ToInt32(entity.LinkedRegistrationID));

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            string filterVisit = string.Format("RegistrationID = {0} AND GCVisitStatus != '{1}'", entityFrom.RegistrationID, Constant.VisitStatus.CANCELLED);
                            List<ConsultVisit> lstVisitFrom = BusinessLayer.GetConsultVisitList(filterVisit, ctx);
                            bool isHasChargesTransfer = false;
                            foreach (ConsultVisit v in lstVisitFrom)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                string filterCharges = string.Format("VisitID = {0} AND IsChargesTransfered = 1 AND GCTransactionStatus != '{1}'", v.VisitID, Constant.TransactionStatus.VOID);
                                List<PatientChargesHd> lstChargesHd = BusinessLayer.GetPatientChargesHdList(filterCharges, ctx);
                                if (lstChargesHd.Count > 0)
                                {
                                    isHasChargesTransfer = true;
                                }
                            }

                            if (!isHasChargesTransfer)
                            {
                                entityFrom.LinkedToRegistrationID = null;
                                entityFrom.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDao.Update(entityFrom);

                                entity.LinkedRegistrationID = null;
                                entity.IsNewPatient = hdnFromRegistrationIsNewPatient.Value == "1";
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDao.Update(entity);

                                ctx.CommitTransaction();
                            }
                            else
                            {
                                result = false;
                                errMessage = "Maaf, Registrasi ini sudah melakukan transfer tagihan dari registrasi asal, harap batalkan transfer tagihan terlebih dahulu";
                                ctx.RollBackTransaction();
                            }

                            #endregion
                        }
                    }
                    else
                    {
                        if (hdnFromRegistrationID.Value != "0" && hdnFromRegistrationID.Value != "")
                        {
                            #region TidakAda Old_FromRegistrationID && Ada New_FromRegistrationID

                            Registration entityFrom = entityDao.Get(Convert.ToInt32(hdnFromRegistrationID.Value));

                            if (entity.MRN == entityFrom.MRN)
                            {
                                entity.LinkedRegistrationID = Convert.ToInt32(hdnFromRegistrationID.Value);
                                entity.IsNewPatient = hdnFromRegistrationIsNewPatient.Value == "1";
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDao.Update(entity);

                                entityFrom.LinkedToRegistrationID = entity.RegistrationID;
                                entityFrom.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDao.Update(entityFrom);

                                ctx.CommitTransaction();
                            }
                            else
                            {
                                result = false;
                                errMessage = "Maaf Data Pasien dari Registrasi Asal dan Tujuan Berbeda";
                                ctx.RollBackTransaction();
                            }

                            #endregion
                        }
                        else
                        {
                            #region TidakAda Old_FromRegistrationID && TidakAda New_FromRegistrationID

                            Registration entityFrom = entityDao.Get(Convert.ToInt32(entity.LinkedRegistrationID));

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            string filterVisit = string.Format("RegistrationID = {0} AND GCVisitStatus != '{1}'", entityFrom.RegistrationID, Constant.VisitStatus.CANCELLED);
                            List<ConsultVisit> lstVisitFrom = BusinessLayer.GetConsultVisitList(filterVisit, ctx);
                            bool isHasChargesTransfer = false;
                            foreach (ConsultVisit v in lstVisitFrom)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                string filterCharges = string.Format("VisitID = {0} AND IsChargesTransfered = 1 AND GCTransactionStatus != '{1}'", v.VisitID, Constant.TransactionStatus.VOID);
                                List<PatientChargesHd> lstChargesHd = BusinessLayer.GetPatientChargesHdList(filterCharges, ctx);
                                if (lstChargesHd.Count > 0)
                                {
                                    isHasChargesTransfer = true;
                                }
                            }

                            if (!isHasChargesTransfer)
                            {
                                entityFrom.LinkedToRegistrationID = null;
                                entityFrom.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDao.Update(entityFrom);

                                entity.LinkedRegistrationID = null;
                                entity.IsNewPatient = hdnFromRegistrationIsNewPatient.Value == "1";
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDao.Update(entity);

                                ctx.CommitTransaction();
                            }
                            else
                            {
                                result = false;
                                errMessage = "Maaf Data Pasien dari Registrasi Asal dan Tujuan Berbeda";
                                ctx.RollBackTransaction();
                            }

                            #endregion
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
            }

            return result;
        }
    }
}