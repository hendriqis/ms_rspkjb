using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class ERDischargeAssessment1 : BasePagePatientPageList
    {
        protected int gridVitalSignPageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.EMERGENCY_SOAP_DISCHARGE_ASSESSMENT_1;
        }

        protected string RegistrationDateTime = "";
        protected override void InitializeDataControl()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}')", Constant.StandardCode.PATIENT_OUTCOME, Constant.StandardCode.DISCHARGE_ROUTINE, Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL, Constant.StandardCode.REFERRAL);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            List<StandardCode> lstDischargeOutcome = lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME && p.IsActive == true).OrderBy(lst => lst.TagProperty).ToList();
            List<StandardCode> lstDischargeRoutine = lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE && p.IsActive == true).OrderBy(lst => lst.TagProperty).ToList();

            Methods.SetComboBoxField<StandardCode>(cboPatientOutcome, lstDischargeOutcome, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeRoutine, lstDischargeRoutine, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeReason, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboReferral, lstStandardCode.Where(sc => (sc.ParentID == Constant.StandardCode.REFERRAL && (sc.StandardCodeID == Constant.Referrer.RUMAH_SAKIT || sc.StandardCodeID == Constant.Referrer.FASKES)) || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = 'OUTPATIENT' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboClinic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");

            Helper.SetControlEntrySetting(txtDischargeDate, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(txtDischargeTime, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboDischargeRoutine, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboPatientOutcome, new ControlEntrySetting(true, true, true), "mpPatientDischarge");

            ConsultVisit entity = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.EMERGENCY)
            {
                if (!string.IsNullOrEmpty(entity.StartServiceTime))
                {
                    RegistrationDateTime = entity.StartServiceDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                    RegistrationDateTime += entity.StartServiceTime.Replace(":", "");
                }
                else
                {
                    RegistrationDateTime = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                    RegistrationDateTime += AppSession.RegisteredPatient.VisitTime.Replace(":", "");
                }
            }
            else
            {
                RegistrationDateTime = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                RegistrationDateTime += AppSession.RegisteredPatient.VisitTime.Replace(":", "");
            }

            if (entity.GCDischargeCondition != "" && entity.GCDischargeMethod != "")
            {
                EntityToControl(entity);
            }
            else
            {
                txtDischargeDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                txtAppointmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDateOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTimeOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }

            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
        }

        private void EntityToControl(ConsultVisit entity)
        {
            cboPatientOutcome.Value = entity.GCDischargeCondition;
            cboDischargeRoutine.Value = entity.GCDischargeMethod;
            txtDischargeDate.Text = entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDischargeTime.Text = entity.DischargeTime;
            if (entity.ReferralPhysicianID != null)
            {
                ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster((Int32)entity.ReferralPhysicianID);
                if (oParamedic != null)
                {
                    hdnParamedicID2.Value = entity.ReferralPhysicianID.ToString();
                    txtParamedicCode.Text = oParamedic.ParamedicCode;
                    txtParamedicName.Text = oParamedic.FullName;
                }
            }
            if (entity.ReferralUnitID != null)
            {
                cboClinic.Value = entity.ReferralUnitID.ToString();
                ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster((Int32)entity.ReferralPhysicianID);
                if (oParamedic != null)
                {
                    hdnPhysicianID.Value = entity.ReferralPhysicianID.ToString();
                    txtPhysicianCode.Text = oParamedic.ParamedicCode;
                    txtPhysicianName.Text = oParamedic.FullName;
                }
                txtAppointmentDate.Text = entity.ReferralDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            if (entity.GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48 || entity.GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48)
            {
                txtDateOfDeath.Text = entity.DateOfDeath.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTimeOfDeath.Text = entity.TimeOfDeath;
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            if (type == "process")
            {
                if (IsValidToDischarge())
                {
                    if ((cboPatientOutcome.Value.ToString() == Constant.PatientOutcome.DEAD_BEFORE_48 || cboPatientOutcome.Value.ToString() == Constant.PatientOutcome.DEAD_AFTER_48)
                            && (txtDateOfDeath.Text == "" || Helper.GetDatePickerValue(txtDateOfDeath).ToString(Constant.FormatString.DATE_FORMAT) == Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT))
                    {
                        result = false;
                        errMessage = "Maaf, jika pasien statusnya sudah meninggal, tolong lengkapi tanggal dan jam meninggalnya terlebih dahulu.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        IDbContext ctx = DbFactory.Configure(true);
                        RegistrationDao registrationDao = new RegistrationDao(ctx);
                        ConsultVisitDao visitDao = new ConsultVisitDao(ctx);
                        PatientDao patientDao = new PatientDao(ctx);
                        try
                        {
                            Registration entityRegis = registrationDao.Get(AppSession.RegisteredPatient.RegistrationID);
                            if (entityRegis.GCRegistrationStatus != Constant.VisitStatus.CANCELLED && entityRegis.GCRegistrationStatus != Constant.VisitStatus.DISCHARGED && entityRegis.GCRegistrationStatus != Constant.VisitStatus.CLOSED)
                            {
                                entityRegis.GCRegistrationStatus = Constant.VisitStatus.PHYSICIAN_DISCHARGE;
                                entityRegis.LastUpdatedBy = AppSession.UserLogin.UserID;
                                registrationDao.Update(entityRegis);

                                List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = '{0}'", entityRegis.RegistrationID), ctx);
                                foreach (ConsultVisit entity in lstConsultVisit)
                                {
                                    entity.GCVisitStatus = Constant.VisitStatus.PHYSICIAN_DISCHARGE;

                                    entity.GCDischargeCondition = cboPatientOutcome.Value.ToString();
                                    entity.GCDischargeMethod = cboDischargeRoutine.Value.ToString();
                                    entity.PhysicianDischargedBy = AppSession.UserLogin.UserID;
                                    entity.PhysicianDischargedDate = DateTime.Now;
                                    entity.DischargeDate = Helper.GetDatePickerValue(txtDischargeDate);
                                    entity.DischargeTime = txtDischargeTime.Text;
                                    entity.LOSInDay = Convert.ToDecimal(hdnLOSInDay.Value);
                                    entity.LOSInHour = Convert.ToDecimal(hdnLOSInHour.Value);
                                    entity.LOSInMinute = Convert.ToDecimal(hdnLOSInMinute.Value);
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                                    if (cboDischargeRoutine.Value.ToString() == Constant.DischargeMethod.DISCHARGED_TO_WARD)
                                    {
                                        entity.ReferralPhysicianID = Convert.ToInt32(hdnParamedicID2.Value);
                                        entity.IsRefferralProcessed = false;
                                    }
                                    else if (cboDischargeRoutine.Value.ToString() == Constant.DischargeMethod.REFFERRED_TO_EXTERNAL_PROVIDER)
                                    {
                                        entity.ReferralTo = Convert.ToInt32(hdnReferrerID.Value);
                                        entity.GCReferralDischargeReason = cboDischargeReason.Value.ToString();
                                        entity.ReferralDischargeReasonOther = txtDischargeOtherReason.Text;
                                        entity.ReferralDate = Helper.GetDatePickerValue(txtDischargeDate);
                                        entity.IsRefferralProcessed = false;
                                    }
                                    else if (cboDischargeRoutine.Value.ToString() == Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT)
                                    {
                                        entity.ReferralUnitID = Convert.ToInt32(cboClinic.Value.ToString());
                                        entity.ReferralPhysicianID = Convert.ToInt32(hdnPhysicianID.Value);
                                        entity.ReferralDate = Helper.GetDatePickerValue(txtAppointmentDate);
                                        entity.IsRefferralProcessed = false;
                                    }

                                    if (cboPatientOutcome.Value.ToString() == Constant.PatientOutcome.DEAD_BEFORE_48 || cboPatientOutcome.Value.ToString() == Constant.PatientOutcome.DEAD_AFTER_48)
                                    {
                                        entity.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeath);
                                        entity.TimeOfDeath = txtTimeOfDeath.Text;

                                        entity.ReferralUnitID = null;
                                        entity.ReferralPhysicianID = null;
                                        entity.ReferralDate = Helper.InitializeDateTimeNull();
                                        entity.IsRefferralProcessed = false;

                                        //Update Patient Death Status
                                        Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                                        if (oPatient != null)
                                        {
                                            oPatient.IsAlive = false;
                                            oPatient.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeath);
                                        }
                                        else
                                        {
                                            oPatient.IsAlive = true;
                                            oPatient.DateOfDeath = DateTime.MinValue;
                                        }
                                        oPatient.LastVisitDate = AppSession.RegisteredPatient.VisitDate;
                                        oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        patientDao.Update(oPatient);
                                    }

                                    visitDao.Update(entity);
                                }
                                result = true;
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                result = false;
                                errMessage = "Maaf, registrasi dengan nomor " + entityRegis.RegistrationNo + " statusnya sudah dipulangkan.";
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
                    }
                }
                else
                {
                    errMessage = "You must be Registered Physician and must entry Patient Chief Complaint, Diagnosis before discharge this patient";
                    result = false;
                }
            }
            return result;
        }

        private bool IsValidToDischarge()
        {
            bool isDPJPPhysician = AppSession.RegisteredPatient.ParamedicID == AppSession.UserLogin.ParamedicID;

            vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            bool isChiefComplaintExist = oChiefComplaint != null;

            vPatientDiagnosis oDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            bool isDiagnosisExist = oDiagnosis != null;

            return isChiefComplaintExist && isDiagnosisExist && isDPJPPhysician;
        }

        #region Vital Sign
        protected List<vVitalSignDt> GetVitalSignDt(Int32 ID)
        {
            return lstVitalSignDt.Where(p => p.ID == ID).ToList();
        }

        protected void cbpVitalSignView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewVitalSign(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewVitalSign(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
        }

        protected void cbpDeleteVitalSign_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (hdnVitalSignRecordID.Value != "")
            {
                VitalSignHd entity = BusinessLayer.GetVitalSignHd(Convert.ToInt32(hdnVitalSignRecordID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVitalSignHd(entity);
            }
            else
            {
                result = "0|There is no record to be deleted !";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;
            filterExpression += string.Format("VisitID = {0} AND IsDischargeVitalSign = 1 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID));
            grdVitalSignView.DataSource = lstEntity;
            grdVitalSignView.DataBind();
        }

        protected void grdVitalSignView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Row.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Row.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = GetVitalSignDt(obj.ID);
                rptVitalSignDt.DataBind();
            }
        }
        #endregion

        #region Prescription
        protected void cbpPrescriptionView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewPrescription(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    //if (Page.Request.QueryString.Count == 0)
                    //{
                    //    LoadHeaderInformation();
                    //}
                    BindGridViewPrescription(1, true, ref pageCount);
                    result = "refresh|" + pageCount + "|"; //+ txtPrescriptionNo.Text;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewPrescription(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;
            if (hdnPrescriptionOrderID.Value != "")
            {
                filterExpression += string.Format("PrescriptionOrderID = {0} AND OrderIsDeleted = 0", hdnPrescriptionOrderID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPrescriptionOrderDt1RowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionOrderDetailID");
            grdPrescriptionView.DataSource = lstEntity;
            grdPrescriptionView.DataBind();
        }

        protected void cbpSendOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int transactionID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            try
            {
                if (param[0] == "sendOrder")
                {
                    PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHdList(String.Format("PrescriptionOrderID = {0}", hdnPrescriptionOrderID.Value))[0];

                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        string filterExpression = string.Format("PrescriptionOrderID = {0} AND OrderIsDeleted = 0 AND (IsAllergyAlert = 1 OR IsAdverseReactionAlert = 1 OR IsDuplicateTheraphyAlert = 1) AND IsAlertConfirmed = 0", hdnPrescriptionOrderID.Value);
                        List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);

                        if (lstEntity.Count > 0)
                        {
                            errMessage = "There is item(s) should be confirmed due to allergy, adverse reaction and duplicate theraphy.";
                            result += string.Format("confirm|{0}", errMessage);
                        }
                        else
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            BusinessLayer.UpdatePrescriptionOrderHd(entity);

                            result += string.Format("success|{0}", errMessage);
                        }

                        //try
                        //{
                        //    HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(entity.DispensaryServiceUnitID));
                        //    string ipAddress = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                        //    if (!String.IsNullOrEmpty(ipAddress))
                        //    {
                        //        SendNotification(entity,ipAddress,"6000");
                        //    }
                        //}
                        //catch (Exception)
                        //{
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID;
        }
        #endregion
    }
}