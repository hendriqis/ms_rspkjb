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
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransactionPageVisitOrder : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
           return Constant.MenuCode.Outpatient.PATIENT_TRANSACTION_VISIT_ORDER; 
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowNextPrev = IsAllowVoid = false;
        }

        protected bool IsAllowEditPatientVisit = true;
        protected override void InitializeDataControl()
        {
            hdnItemCardFee.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU).ParameterValue;

            vConsultVisit consultVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnRegistrationID.Value = consultVisit.RegistrationID.ToString();

            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = '{0}'", consultVisit.RegistrationID))[0];
            hdnClassID.Value = entity.ClassID.ToString();
            hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();

            IsAllowEditPatientVisit = (entity.RegistrationDate == DateTime.Today || entity.GCRegistrationStatus == Constant.VisitStatus.CANCELLED);
            if (!IsAllowEditPatientVisit)
                divContainerAddData.Style.Add("display", "none");

            BindGridView();

            txtPhysicianCode.Attributes.Add("validationgroup", "mpPatientVisit");
            txtClinicCode.Attributes.Add("validationgroup", "mpPatientVisit");
            txtVisitTypeCode.Attributes.Add("validationgroup", "mpPatientVisit");

            List<Specialty> lstSpecialty = BusinessLayer.GetSpecialtyList("IsDeleted = 0");
            Methods.SetComboBoxField<Specialty>(cboRegistrationEditSpecialty, lstSpecialty, "SpecialtyName", "SpecialtyID");
            cboRegistrationEditSpecialty.SelectedIndex = 0;
        }

        private void BindGridView()
        {
            grdPatientVisitTransHd.DataSource = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND GCVisitStatus != '{1}' ORDER BY VisitID ASC", hdnRegistrationID.Value, Constant.VisitStatus.CANCELLED));
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
            entity.VisitReason = txtVisitReason.Text;
            entity.ClassID = entity.ChargeClassID = Convert.ToInt32(hdnClassID.Value);
            entity.RoomID = null;
            entity.BedID = null;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            Registration entityReg = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));

            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegDao = new RegistrationDao(ctx);
            ConsultVisitDao entityVisitDao = new ConsultVisitDao(ctx);
            SettingParameterDao entitySettingParameterDao = new SettingParameterDao(ctx);
            try
            {
                Registration reg = entityRegDao.Get(Convert.ToInt32(hdnRegistrationID.Value));

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

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityVisit.Session = BusinessLayer.GetRegistrationSession(entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entityVisit.VisitDate, entityVisit.VisitTime, ctx);

                bool isBPJS = false;
                if (entityReg.GCCustomerType == Constant.CustomerType.BPJS)
                {
                    isBPJS = true;
                }

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                entityVisit.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entityVisit.VisitDate, Convert.ToInt32(entityVisit.Session), false, isBPJS, 0, ctx, 1));

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                //entityVisit.QueueNo = BusinessLayer.GetConsultVisitMaxQueueNo(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND VisitDate = '{2}'", entityVisit.HealthcareServiceUnitID, entityVisit.ParamedicID, entityVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx);
                //entityVisit.QueueNo++;
                entityVisitDao.Insert(entityVisit);

                if (registrationStatus == Constant.VisitStatus.CHECKED_IN)
                {
                    entityVisit.VisitID = BusinessLayer.GetConsultVisitMaxID(ctx);
                    Helper.InsertAutoBillItem(ctx, entityVisit, Constant.Facility.OUTPATIENT, Convert.ToInt32(hdnClassID.Value), reg.GCCustomerType, false, 0);
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                ConsultVisit entity = BusinessLayer.GetConsultVisit(Convert.ToInt32(hdnVisitID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateConsultVisit(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            //bool result = true;

            //IDbContext ctx = DbFactory.Configure(true);
            //ConsultVisitDao entityVisitDao = new ConsultVisitDao(ctx);
            //PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            //try
            //{
            //    string filterExpression = string.Format("VisitID = {0} AND ItemID NOT IN (SELECT ItemID FROM ServiceUnitAutoBillItem WHERE HealthcareServiceUnitID = {1} AND IsDeleted = 0) AND ItemID != {2} AND IsDeleted = 0", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value, hdnItemCardFee.Value);
            //    int count = BusinessLayer.GetvPatientChargesDtRowCount(filterExpression, ctx);
            //    if (count > 0)
            //    {
            //        errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_OPENED_TRANSACTION_VALIDATION);
            //        result = false;
            //    }
            //    else
            //    {
            //        ConsultVisit entityVisit = entityVisitDao.Get(Convert.ToInt32(hdnVisitID.Value));
            //        entityVisit.GCVisitStatus = Constant.VisitStatus.CANCELLED;
            //        entityVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
            //        entityVisitDao.Update(entityVisit);

            //        List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", entityVisit.VisitID, Constant.TransactionStatus.VOID), ctx);
            //        foreach (PatientChargesHd chargesHd in lstPatientChargesHd)
            //        {
            //            chargesHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
            //            chargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
            //            patientChargesHdDao.Update(chargesHd);
            //        }
            //    }

            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            ConsultVisitDao entityVisitDao = new ConsultVisitDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);

            try
            {
                string filterExpression = string.Format("VisitID = {0} AND GCTransactionDetailStatus != '{1}' AND IsDeleted = 0", hdnVisitID.Value, Constant.TransactionStatus.VOID);
                List<vPatientChargesDt3> lstPatientChargesDt = BusinessLayer.GetvPatientChargesDt3List(filterExpression, ctx);
                List<vPatientChargesDt3> lstPatientChargesDtTemp = lstPatientChargesDt.Where(t => !t.IsCreatedBySystem || (t.PatientBillingID != 0)).ToList();

                if (lstPatientChargesDtTemp.Count > 0)
                {
                    errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_OPENED_TRANSACTION_VALIDATION);
                    result = false;
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

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}