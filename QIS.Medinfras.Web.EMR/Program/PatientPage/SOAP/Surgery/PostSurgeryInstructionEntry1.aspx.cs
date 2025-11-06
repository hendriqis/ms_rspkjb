﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PostSurgeryInstructionEntry1 : BasePagePatientPageList
    {
        protected int gridMedicationPageCount = 1;
        protected int gridLaboratoryPageCount = 1;
        protected int gridImagingPageCount = 1;
        protected static string _recordID = "0";
        protected static string _linkedVisitID;
        protected static string _visitNoteID;

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        protected string GetRecordID()
        {
            return _recordID;
        }

        public override string OnGetMenuCode()
        {
            string[] paramInfo = Page.Request.QueryString["id"].Split('|');
            switch (paramInfo[3])
            {
                case "anesthesy":
                    return Constant.MenuCode.EMR.PENGKAJIAN_ANESTESI;
                case "surgery":
                    return Constant.MenuCode.EMR.ASESMEN_PRA_BEDAH;
                default:
                    return Constant.MenuCode.EMR.ASESMEN_PRA_BEDAH;
            }
        }

        protected override void InitializeDataControl()
        {
            string[] paramInfo = Page.Request.QueryString["id"].Split('|');

            hdnTestOrderID.Value = paramInfo[0];
            hdnRecordID.Value = paramInfo[2];
            hdnMainRecordID.Value = paramInfo[2];

            txtInstructionDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtInstructionTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnImagingServiceUnitID.Value = AppSession.ImagingServiceUnitID;
            hdnLaboratoryServiceUnitID.Value = AppSession.LaboratoryServiceUnitID;

            SetEntityToControl();

            BindGridViewMedication(1, true, ref gridMedicationPageCount);
            BindGridViewLaboratory(1, true, ref gridLaboratoryPageCount);
            BindGridViewImaging(1, true, ref gridImagingPageCount);

            hdnLinkedVisitID.Value = _linkedVisitID;
            hdnRecordID.Value = _recordID;
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            #region Monitoring
            StringBuilder innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "postSurgeryMonitoring.html");

            divFormContent1.InnerHtml = innerHtml.ToString();
            hdnMonitoringLayout.Value = innerHtml.ToString();
            #endregion

            #region Nutrisi dan Transfusi
            innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "postSurgeryNutrition.html");

            divFormContent2.InnerHtml = innerHtml.ToString();
            hdnNutritionLayout.Value = innerHtml.ToString();
            #endregion

            #region Lain-lain
            innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "postSurgeryInstructionOther.html");

            divFormContent3.InnerHtml = innerHtml.ToString();
            hdnOtherInstructionLayout.Value = innerHtml.ToString();
            #endregion
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        private void SetEntityToControl()
        {
            vConsultVisit4 entityVisit = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnPatientInformation.Value = string.Format("{0} (MRN = {1}, REG = {2}, LOC = {3}, DOB = {4})", entityVisit.cfPatientNameInLabel, entityVisit.MedicalNo, entityVisit.RegistrationNo, entityVisit.ServiceUnitName, entityVisit.cfDateOfBirth);
            hdnDepartmentID.Value = entityVisit.DepartmentID;

            vPostSurgeryInstruction obj = BusinessLayer.GetvPostSurgeryInstructionList(string.Format("TestOrderID = {0} AND PostSurgeryInstructionID = {1} AND IsDeleted = 0", hdnTestOrderID.Value, hdnRecordID.Value)).FirstOrDefault();
            if (obj != null)
            {
                if (obj.PostSurgeryInstructionID == 0)
                {
                    hdnRecordID.Value = "0";
                    hdnParamedicID.Value = "0";
                    _recordID = "0";

                    PopulateFormContent();
                }
                else
                {
                    _recordID = obj.PostSurgeryInstructionID.ToString();
                    hdnRecordID.Value = obj.PostSurgeryInstructionID.ToString();
                    hdnParamedicID.Value = obj.ParamedicID.ToString();
                    txtInstructionDate.Text = obj.InstructionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtInstructionTime.Text = obj.InstructionTime;
                    divFormContent1.InnerHtml = obj.MonitoringInstructionFormLayout;
                    hdnMonitoringLayout.Value = obj.MonitoringInstructionFormLayout;
                    hdnMonitoringValue.Value = obj.MonitoringInstructionFormValue;
                    divFormContent2.InnerHtml = obj.NutritionInstructionFormLayout;
                    hdnNutritionLayout.Value = obj.NutritionInstructionFormLayout;
                    hdnNutritionFormValue.Value = obj.NutritionInstructionFormValue;
                    divFormContent3.InnerHtml = obj.OtherInstructionFormLayout;
                    hdnOtherInstructionLayout.Value = obj.OtherInstructionFormLayout;
                    hdnOtherInstructionValue.Value = obj.OtherInstructionFormValue;
                    txtRemarks.Text = obj.Remarks;

                    hdnIsChanged.Value = "0";
                    hdnIsSaved.Value = "0";
                }
            }
            else
            {
                PopulateFormContent();
                _recordID = "0";
            }
        }

        protected override void SetControlProperties()
        {
        }

        private void UpdateAssessment(IDbContext ctx)
        {
            PostSurgeryInstructionDao assessmentDao = new PostSurgeryInstructionDao(ctx);
            PostSurgeryInstruction entity = null;
            bool isNewAssessment = true;

            if (hdnRecordID.Value != "" && hdnRecordID.Value != "0")
            {
                entity = assessmentDao.Get(Convert.ToInt32(hdnRecordID.Value));
                isNewAssessment = false;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                PatientVisitNote oPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID IN ({0}) AND NutritionAssessmentID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnRecordID.Value)).FirstOrDefault();
                if (oPatientVisitNote != null)
                    hdnPatientVisitNoteID.Value = oPatientVisitNote.ID.ToString();
                else
                    hdnPatientVisitNoteID.Value = "0";

                _visitNoteID = hdnPatientVisitNoteID.Value;
            }
            else
            {
                entity = new PostSurgeryInstruction();
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;

                hdnPatientVisitNoteID.Value = "0";
                _visitNoteID = "0";
            }

            entity.InstructionDate = Helper.GetDatePickerValue(txtInstructionDate);
            entity.InstructionTime = txtInstructionTime.Text;
            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);

            entity.MonitoringInstructionFormLayout = hdnMonitoringLayout.Value;
            entity.MonitoringInstructionFormValue = hdnMonitoringValue.Value;
            entity.NutritionInstructionFormLayout = hdnNutritionLayout.Value;
            entity.NutritionInstructionFormValue = hdnNutritionFormValue.Value;
            entity.OtherInstructionFormLayout = hdnOtherInstructionLayout.Value;
            entity.OtherInstructionFormValue = hdnOtherInstructionValue.Value;
            entity.Remarks = txtRemarks.Text;

            if (hdnPatientVisitNoteID.Value != "0")
            {
                entity.PatientVisitNoteID = Convert.ToInt32(hdnPatientVisitNoteID.Value);
            }

            if (isNewAssessment)
            {
                hdnRecordID.Value = assessmentDao.InsertReturnPrimaryKeyID(entity).ToString();
                _recordID = hdnRecordID.Value;
            }
            else
            {
                assessmentDao.Update(entity);
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string message)
        {
            bool result = true;
            int emrV1ProcessType = 0;

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityOrderDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHdOriginalDao entityOrderHdOriginalDao = new PrescriptionOrderHdOriginalDao(ctx);
            PrescriptionOrderDtOriginalDao entityOrderDtOriginalDao = new PrescriptionOrderDtOriginalDao(ctx);
            PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
            PatientInstructionDao patientInstructionDao = new PatientInstructionDao(ctx);

            List<vPrescriptionOrderDt1> lstEntity = new List<vPrescriptionOrderDt1>();
            try
            {
                
                if (type == "save")
                {
                    if ((hdnRecordID.Value != "" && hdnRecordID.Value != "0") && !IsValidToSave(ref message))
                    {
                        result = false;
                        hdnIsSaved.Value = "0";
                        return result;
                    }

                    PostSurgeryInstruction obj = BusinessLayer.GetPostSurgeryInstructionList(string.Format("PostSurgeryInstructionID = {0} AND IsDeleted = 0", hdnRecordID.Value), ctx).FirstOrDefault();
                    if (obj != null)
                    {
                        hdnRecordID.Value = obj.PostSurgeryInstructionID.ToString();
                    }
                    else
                    {
                        hdnRecordID.Value = "0";
                    }

                    UpdateAssessment(ctx);

                    PatientVisitNote soapNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND PostSurgeryInstructionID = {2} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, hdnRecordID.Value), ctx).FirstOrDefault();
                    bool isSoapNoteNull = false;
                    if (soapNote == null)
                    {
                        isSoapNoteNull = true;
                        soapNote = new PatientVisitNote();
                    }

                    ControlToEntity(soapNote, hdnRecordID.Value);

                    if (isSoapNoteNull)
                    {
                        soapNote.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                        soapNote.VisitID = AppSession.RegisteredPatient.VisitID;
                        soapNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        soapNote.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;
                        soapNote.CreatedBy = AppSession.UserLogin.UserID;
                        hdnPatientVisitNoteID.Value = patientVisitNoteDao.InsertReturnPrimaryKeyID(soapNote).ToString();
                    }
                    else
                    {
                        soapNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        soapNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientVisitNoteDao.Update(soapNote);

                        hdnPatientVisitNoteID.Value = soapNote.ID.ToString();
                    }
                    _visitNoteID = hdnPatientVisitNoteID.Value;

                    if (!string.IsNullOrEmpty(txtRemarks.Text))
                    {
                        string filterExpInstruction = string.Format("PatientVisitNoteID = {0}", _visitNoteID);
                        PatientInstruction oInstruction = BusinessLayer.GetPatientInstructionList(filterExpInstruction, ctx).FirstOrDefault();

                        if (oInstruction == null)
                        {
                            oInstruction = new PatientInstruction();
                            oInstruction.VisitID = AppSession.RegisteredPatient.VisitID;
                            oInstruction.PatientVisitNoteID = Convert.ToInt32(_visitNoteID);
                            oInstruction.PhysicianID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            oInstruction.GCInstructionGroup = "X139^003";
                            oInstruction.Description = txtRemarks.Text;
                            oInstruction.InstructionDate = Helper.GetDatePickerValue(txtInstructionDate);
                            oInstruction.InstructionTime = txtInstructionTime.Text;
                            oInstruction.CreatedBy = AppSession.UserLogin.UserID;
                            patientInstructionDao.Insert(oInstruction);
                        }
                        else
                        {
                            oInstruction.VisitID = AppSession.RegisteredPatient.VisitID;
                            oInstruction.PatientVisitNoteID = Convert.ToInt32(_visitNoteID);
                            oInstruction.PhysicianID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            oInstruction.GCInstructionGroup = "X139^003";
                            oInstruction.Description = txtRemarks.Text;
                            oInstruction.InstructionDate = Helper.GetDatePickerValue(txtInstructionDate);
                            oInstruction.InstructionTime = txtInstructionTime.Text;
                            oInstruction.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientInstructionDao.Update(oInstruction);
                        }
                    }

                    ctx.CommitTransaction();

                    message = _recordID;
                    hdnIsSaved.Value = "1";
                    hdnIsChanged.Value = "0";
                }
                else if (type == "Propose")
                {
                    #region Proposed

                    PrescriptionOrderHd entity = entityOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));

                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        string filterExpression = string.Format("PrescriptionOrderID = {0} AND ParentID IS NULL AND OrderIsDeleted = 0 AND (IsAllergyAlert = 1 OR IsAdverseReactionAlert = 1 OR IsDuplicateTheraphyAlert = 1)", hdnPrescriptionOrderID.Value);
                        lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression, ctx);

                        if (lstEntity.Count > 0)
                        {
                            message = "There is item(s) should be confirmed due to allergy, adverse reaction and duplicate theraphy.";
                            result = false;
                        }
                        else
                        {

                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entity.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                            entity.SendOrderDateTime = DateTime.Now;
                            entity.SendOrderBy = AppSession.UserLogin.UserID;
                            entity.ProposedBy = AppSession.UserLogin.UserID;
                            entity.ProposedDate = DateTime.Now;
                            entityOrderHdDao.Update(entity);

                            //Log : Copy of Current Prescription Order
                            int historyID = 0;
                            if (entity.IsOrderedByPhysician)
                            {
                                PrescriptionOrderHdOriginal originalHd = new PrescriptionOrderHdOriginal();
                                CopyHeaderObject(entity, ref originalHd);
                                historyID = entityOrderHdOriginalDao.InsertReturnPrimaryKeyID(originalHd);
                            }

                            filterExpression = string.Format("PrescriptionOrderID = {0} AND OrderIsDeleted = 0", hdnPrescriptionOrderID.Value);
                            lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression, ctx);

                            List<PrescriptionOrderDtOriginal> lstOriginalDt = new List<PrescriptionOrderDtOriginal>();

                            foreach (vPrescriptionOrderDt1 item in lstEntity)
                            {
                                PrescriptionOrderDt orderDt = entityOrderDtDao.Get(item.PrescriptionOrderDetailID);
                                if (orderDt != null)
                                {
                                    orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                                    entityOrderDtDao.Update(orderDt);
                                    if (historyID > 0)
                                    {
                                        PrescriptionOrderDtOriginal originalDt = new PrescriptionOrderDtOriginal();
                                        CopyDetailObject(orderDt, ref originalDt);
                                        ////originalDt.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                                        originalDt.HistoryHeaderID = historyID;
                                        lstOriginalDt.Add(originalDt);
                                    }



                                }
                            }

                            #region Log Detail
                            if (lstOriginalDt.Count > 0)
                            {
                                foreach (PrescriptionOrderDtOriginal originalDt in lstOriginalDt)
                                {
                                    entityOrderDtOriginalDao.Insert(originalDt);
                                }
                            }
                            #endregion

                            #region Log PrescriptionTaskOrder
                            Helper.InsertPrescriptionOrderTaskLog(ctx, entity.PrescriptionOrderID, Constant.PrescriptionTaskLogStatus.Sent, AppSession.UserLogin.UserID, false);
                            #endregion

                            hdnIsOutstandingOrder.Value = "0";
                            result = true;
                        }

                        emrV1ProcessType = 1;

                        if (AppSession.IsPrintPHOrderTracerFromEMR)
                        {
                            PrintOrderTracer(entity.PrescriptionOrderID);
                        }
                    }

                    #endregion
                }
                else if (type == "ReOpen")
                {
                    #region ReOpen

                    PrescriptionOrderHd entity2 = entityOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));

                    if (entity2.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        entity2.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity2.GCOrderStatus = Constant.OrderStatus.OPEN;
                        entityOrderHdDao.Update(entity2);

                        string filterExpression = string.Format("PrescriptionOrderID = {0} AND OrderIsDeleted = 0", hdnPrescriptionOrderID.Value);
                        lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression, ctx);
                        foreach (vPrescriptionOrderDt1 item in lstEntity)
                        {
                            PrescriptionOrderDt orderDt = entityOrderDtDao.Get(item.PrescriptionOrderDetailID);
                            if (orderDt != null)
                            {
                                orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.OPEN;
                                entityOrderDtDao.Update(orderDt);
                            }
                        }

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        string filterHdOri = string.Format("PrescriptionOrderID = {0} AND GCTransactionStatus <> '{1}'", entity2.PrescriptionOrderID, Constant.TransactionStatus.VOID);
                        List<PrescriptionOrderHdOriginal> lstHdOri = BusinessLayer.GetPrescriptionOrderHdOriginalList(filterHdOri, ctx);
                        foreach (PrescriptionOrderHdOriginal hdOri in lstHdOri)
                        {
                            hdOri.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            hdOri.GCOrderStatus = Constant.OrderStatus.CANCELLED;
                            hdOri.GCVoidReason = Constant.DeleteReason.OTHER;
                            hdOri.VoidReason = "Re-open Prescription Order from EMR.";
                            hdOri.VoidBy = AppSession.UserLogin.UserID;
                            hdOri.VoidDate = DateTime.Now;
                            hdOri.LastUpdatedBy = AppSession.UserLogin.UserID;
                            hdOri.LastUpdatedDate = DateTime.Now;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityOrderHdOriginalDao.Update(hdOri);

                            string filterDtOri = string.Format("PrescriptionOrderID = {0} AND GCPrescriptionOrderStatus <> '{1}'", hdOri.PrescriptionOrderID, Constant.OrderStatus.CANCELLED);
                            List<PrescriptionOrderDtOriginal> lstDtOri = BusinessLayer.GetPrescriptionOrderDtOriginalList(filterDtOri, ctx);
                            foreach (PrescriptionOrderDtOriginal dtOri in lstDtOri)
                            {
                                dtOri.IsDeleted = true;
                                dtOri.GCPrescriptionOrderStatus = Constant.OrderStatus.CANCELLED;
                                dtOri.GCVoidReason = Constant.DeleteReason.OTHER;
                                dtOri.VoidReason = "Re-open Prescription Order from EMR.";
                                dtOri.VoidBy = AppSession.UserLogin.UserID;
                                dtOri.VoidDateTime = DateTime.Now;
                                dtOri.LastUpdatedBy = AppSession.UserLogin.UserID;
                                dtOri.LastUpdatedDate = DateTime.Now;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityOrderDtOriginalDao.Update(dtOri);
                            }
                        }

                        hdnIsOutstandingOrder.Value = "1";
                        result = true;

                        #region Log PrescriptionTaskOrder
                        Helper.InsertPrescriptionOrderTaskLog(ctx, entity2.PrescriptionOrderID, Constant.PrescriptionTaskLogStatus.Reopen, AppSession.UserLogin.UserID, false);
                        #endregion

                        emrV1ProcessType = 2;
                    }
                    else
                    {
                        message = "Status Order sudah berubah, haraf refresh halaman ini";
                        result = false;
                    }

                    #endregion
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
            }

            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
                hdnIsSaved.Value = "0";
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        private void ControlToEntity(PatientVisitNote entitypvn, string recordID)
        {
            string soapNote = string.Format("Salinan dari Instruksi Paska Bedah Terintegrasi");

            entitypvn.NoteDate = Helper.GetDatePickerValue(txtInstructionDate);
            entitypvn.NoteTime = txtInstructionTime.Text;
            entitypvn.InstructionText = txtRemarks.Text;
            entitypvn.NoteText = soapNote;
            entitypvn.PostSurgeryInstructionID = Convert.ToInt32(hdnRecordID.Value);
        }

        private void CopyHeaderObject(PrescriptionOrderHd source, ref PrescriptionOrderHdOriginal destination)
        {
            var fields = source.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var value = field.GetValue(source);
                destination.GetType().GetProperty(field.Name.Replace("_", "")).SetValue(destination, value, null);
            }
        }

        private void CopyDetailObject(PrescriptionOrderDt source, ref PrescriptionOrderDtOriginal destination)
        {
            var fields = source.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var value = field.GetValue(source);
                destination.GetType().GetProperty(field.Name.Replace("_", "")).SetValue(destination, value, null);
            }
        }

        private string PrintOrderTracer(int prescriptionOrderID)
        {

            string result = string.Empty;
            Healthcare entityHSU = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.ORDER_FARMASI);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;

            if (entityHSU.Initial == "RSPR") //KKDI
            {
                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    ZebraPrinting.PrintOrderFarmasiRSPR(prescriptionOrderID, printerUrl1);
                }
            }
            else if (entityHSU.Initial == "RSUKI") //RSUKI
            {
                ZebraPrinting.PrintOrderFarmasiRSUKI(prescriptionOrderID, printerUrl1);
            }
            else if (entityHSU.Initial == "JWCC")
            {
                SettingParameterDt oSetPar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode='{0}'", Constant.SettingParameter.PH0071)).FirstOrDefault();
                printerUrl1 = oSetPar.ParameterValue;
                ZebraPrinting.PrintOrderFarmasiJWCC(prescriptionOrderID, printerUrl1);
            }
            else
            {
                //if (entityHSU.Initial == "DEMO")
                //{
                //    ZebraPrinting.PrintOrderFarmasi(prescriptionOrderID, printerUrl1);
                //}
                ZebraPrinting.PrintOrderFarmasi(prescriptionOrderID, printerUrl1);
            }
            return result;
        }


        protected void cbpDeleteTestOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (e.Parameter != null && e.Parameter != "")
            {
                string testOrderID = "0";

                switch (e.Parameter)
                {
                    case "LB":
                        testOrderID = hdnLaboratoryTestOrderID.Value;
                        break;
                    case "IS":
                        testOrderID = hdnImagingTestOrderID.Value;
                        break;
                    default:
                        testOrderID = hdnDiagnosticTestOrderID.Value;
                        break;
                }

                if (testOrderID != "0")
                {
                    TestOrderHd entity = BusinessLayer.GetTestOrderHd(Convert.ToInt32(testOrderID));
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateTestOrderHd(entity);
                    result = string.Format("1|{0}", e.Parameter);
                }
                else
                {
                    result = string.Format("0|{0}|There is no record to be deleted !");
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpSendOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            int transactionID = Convert.ToInt32(param[2]);
            result = param[0] + "|" + param[1] + "|";
            try
            {
                if (param[0] == "sendOrder")
                {
                    if (param[1] != "PH")
                    {
                        TestOrderHd entity = BusinessLayer.GetTestOrderHdList(String.Format("TestOrderID = {0}", transactionID))[0];
                        if (entity.GCTransactionStatus != Constant.TransactionStatus.CLOSED)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            BusinessLayer.UpdateTestOrderHd(entity);
                            try
                            {
                                HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(entity.HealthcareServiceUnitID));
                                string ipAddress = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                                if (!String.IsNullOrEmpty(ipAddress))
                                {
                                    SendNotification(entity, ipAddress);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                result += string.Format("success|{0}", errMessage);
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

        private void SendNotification(TestOrderHd order, string ipAddress)
        {
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.AppendLine(string.Format("No  : {0}", order.TestOrderNo));
            sbMessage.AppendLine(string.Format("Fr  : {0}", string.Format("{0} ({1})", AppSession.RegisteredPatient.ServiceUnitName, AppSession.UserLogin.UserFullName)));
            sbMessage.AppendLine(string.Format("Px  : {0}", AppSession.RegisteredPatient.PatientName));
            sbMessage.AppendLine(string.Format("PDx :    "));
            sbMessage.AppendLine(string.Format("{0}", order.Remarks));
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(ipAddress), 6000);
            NetworkStream stream = client.GetStream();
            using (BinaryWriter w = new BinaryWriter(stream))
            {
                using (BinaryReader r = new BinaryReader(stream))
                {
                    w.Write(string.Format(@"{0}", sbMessage.ToString()).ToCharArray());
                }
            }
        }

        private bool IsValidToSave(ref string errMessage)
        {
            StringBuilder errMsg = new StringBuilder();
            if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
            {
                int paramedicID = Convert.ToInt32(hdnParamedicID.Value);
                if (AppSession.UserLogin.ParamedicID != paramedicID)
                {
                    errMsg.AppendLine("Perubahan Kajian Pasien hanya dapat dilakukan oleh Dokter yang melakukan Pengkajian");
                }
            }
            //if (string.IsNullOrEmpty(txtAnamnesisText.Text))
            //{
            //    errMsg.AppendLine("Anamnesis (Data Subjektif) harus diisi.");
            //}

            errMessage = errMsg.ToString();

            return (errMessage == string.Empty);
        }

        #region Laboratory
        private void BindGridViewLaboratory(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TransactionCode  = {1} AND GCTransactionStatus != '{2}' AND PostSurgeryInstructionID = '{3}'", AppSession.RegisteredPatient.VisitID, Constant.TransactionCode.LABORATORY_TEST_ORDER, Constant.TransactionStatus.VOID, hdnRecordID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd1> lstEntity = BusinessLayer.GetvTestOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            StringBuilder sbText = new StringBuilder();
            foreach (vTestOrderHd1 item in lstEntity)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbText.AppendLine(string.Format("- {0}", detail.ItemName1));
                }
            }
            hdnLaboratorySummary.Value = sbText.ToString();

            grdLaboratoryView.DataSource = lstEntity;
            grdLaboratoryView.DataBind();
        }

        protected void cbpLaboratoryView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewLaboratory(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewLaboratory(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = hdnLaboratorySummary.Value;
        }

        protected void grdLaboratoryView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd1 obj = (vTestOrderHd1)e.Row.DataItem;
                Repeater rptLaboratoryDt = (Repeater)e.Row.FindControl("rptLaboratoryDt");
                rptLaboratoryDt.DataSource = obj.cfTestOrderDetailList;
                rptLaboratoryDt.DataBind();
            }
        }

        private object GetTestOrderDt(int testOrderID)
        {
            List<vTestOrderDt> lstOrderDt = BusinessLayer.GetvTestOrderDtList(string.Format("TestOrderID = {0} ORDER BY ItemName1", testOrderID));
            return lstOrderDt;
        }
        #endregion

        #region Imaging
        private void BindGridViewImaging(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TransactionCode = {1} AND GCTransactionStatus != '{2}' AND PostSurgeryInstructionID = {3}", AppSession.RegisteredPatient.VisitID, Constant.TransactionCode.IMAGING_TEST_ORDER, Constant.TransactionStatus.VOID, hdnRecordID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd1> lstEntity = BusinessLayer.GetvTestOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            StringBuilder sbText = new StringBuilder();
            foreach (vTestOrderHd1 item in lstEntity)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbText.AppendLine(string.Format("- {0}", detail.ItemName1));
                }
            }
            hdnImagingSummary.Value = sbText.ToString();

            grdImagingView.DataSource = lstEntity;
            grdImagingView.DataBind();
        }

        protected void cbpImagingView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewImaging(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewImaging(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = hdnImagingSummary.Value;
        }

        protected void grdImagingView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd1 obj = (vTestOrderHd1)e.Row.DataItem;
                Repeater rptImagingDt = (Repeater)e.Row.FindControl("rptImagingDt");
                rptImagingDt.DataSource = obj.cfTestOrderDetailList;
                rptImagingDt.DataBind();
            }
        }
        #endregion

        #region Medication
        #region Prescription Hd
        private void BindGridViewMedication(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            hdnIsOutstandingOrder.Value = "0";
            string TransactionStatus = String.Format("'{0}','{1}'", Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            string filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus NOT IN ({1}) AND PostSurgeryInstructionID = {2}", AppSession.RegisteredPatient.VisitID, TransactionStatus, hdnMainRecordID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPrescriptionOrderHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPrescriptionOrderHd> lstEntity = BusinessLayer.GetvPrescriptionOrderHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionOrderID DESC");
            grdMedicationView.DataSource = lstEntity;
            grdMedicationView.DataBind();
        }

        protected void cbpMedicationView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewMedication(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewMedication(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }


        protected void cbpProposed_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (e.Parameter != null && e.Parameter != "")
            {
                int PrescriptionOrderID = Convert.ToInt32(e.Parameter);
            }
        }
        #endregion

        #region Prescription Dt
        private void BindGridViewMedicationDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnPrescriptionOrderID.Value != "")
            {
                filterExpression = string.Format("PrescriptionOrderID = {0} AND ParentID IS NULL", hdnPrescriptionOrderID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPrescriptionOrderDt1RowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            //string code = ddlViewType.SelectedValue;

            //if (code == "1")
            //    filterExpression += string.Format(" AND GCPrescriptionOrderStatus IN ('{0}','{1}','{2}') AND TakenQty > 0", Constant.OrderStatus.IN_PROGRESS, Constant.OrderStatus.COMPLETED, Constant.OrderStatus.CLOSED);
            //else if (code == "2")
            //    filterExpression += string.Format(" AND GCPrescriptionOrderStatus IN ('{0}','{1}','{2}') AND TakenQty = 0", Constant.OrderStatus.IN_PROGRESS, Constant.OrderStatus.COMPLETED, Constant.OrderStatus.CLOSED);
            //else if (code == "3")
            //    filterExpression += string.Format(" AND GCPrescriptionOrderStatus IN ('{0}')", Constant.OrderStatus.CANCELLED);

            List<vPrescriptionOrderDt10> lstEntity = BusinessLayer.GetvPrescriptionOrderDt10List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionOrderDetailID");
            grdMedicationViewDt.DataSource = lstEntity;
            grdMedicationViewDt.DataBind();
        }
        protected void cbpMedicationViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewMedicationDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "delete")
                {
                    OnDeleteEntityDt(ref errMessage, Convert.ToInt32(param[1]));
                    result = "delete|" + errMessage;
                }
                else // refresh
                {

                    BindGridViewMedicationDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHdDao prescriptionOrderHdDao = new PrescriptionOrderHdDao(ctx);
            try
            {
                PrescriptionOrderDt entityDt = entityDtDao.Get(ID);
                if (prescriptionOrderHdDao.Get(entityDt.PrescriptionOrderID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (entityDt.IsCompound != true)
                    {
                        if (!entityDt.IsDeleted)
                        {
                            entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                            entityDt.IsDeleted = true;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                            ctx.CommitTransaction();
                        }
                    }
                    else
                    {
                        List<PrescriptionOrderDt> lstChild = BusinessLayer.GetPrescriptionOrderDtList(string.Format(" ParentID = {0} ", entityDt.PrescriptionOrderDetailID));
                        if (!entityDt.IsDeleted)
                        {
                            entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                            entityDt.IsDeleted = true;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                        }
                        foreach (PrescriptionOrderDt dt in lstChild)
                        {
                            if (!dt.IsDeleted)
                            {
                                dt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                dt.IsDeleted = true;
                                dt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(dt);
                            }
                        }
                        ctx.CommitTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Order Resep " + prescriptionOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value)).PrescriptionOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
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

        #endregion
        protected void grdMedicationView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPrescriptionOrderHd entity = e.Row.DataItem as vPrescriptionOrderHd;
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    hdnIsOutstandingOrder.Value = "1";
                }
            }
        }

        protected void grdMedicationViewDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPrescriptionOrderDt10 entity = e.Row.DataItem as vPrescriptionOrderDt10;

                HtmlImage imgHAM = e.Row.FindControl("imgHAM") as HtmlImage;
                if (imgHAM != null)
                {
                    imgHAM.Visible = entity.IsHAM;
                }
            }
        }
        #endregion
    }
}
