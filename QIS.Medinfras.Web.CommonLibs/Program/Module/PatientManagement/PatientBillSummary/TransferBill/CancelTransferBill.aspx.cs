using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CancelTransferBill : BasePageTrx
    {
        private string pageTitle = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string GetPageTitle()
        {
            return pageTitle;
        }

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.CANCEL_BILL_TRANSFER_FROM_OTHER_UNIT;
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.CANCEL_BILL_TRANSFER_FROM_OTHER_UNIT;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.CANCEL_BILL_TRANSFER_FROM_OTHER_UNIT;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.CANCEL_BILL_TRANSFER_FROM_OTHER_UNIT;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.CANCEL_BILL_TRANSFER_FROM_OTHER_UNIT;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.CANCEL_BILL_TRANSFER_FROM_OTHER_UNIT;
                    return Constant.MenuCode.MedicalDiagnostic.CANCEL_BILL_TRANSFER_FROM_OTHER_UNIT;
                default: return Constant.MenuCode.Inpatient.CANCEL_BILL_TRANSFER_FROM_OTHER_UNIT;
            }
        }

        protected override void InitializeDataControl()
        {
            
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnLinkedRegistrationID.Value = AppSession.RegisteredPatient.LinkedRegistrationID.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            pageTitle = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            BindGrid();            
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGrid();
        }


        private string GetFilterExpression()
        {
            string filterExpression = "";

            //if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
            //    filterExpression = string.Format("(RegistrationID = {0} AND IsChargesTransfered = 1) AND GCTransactionStatus NOT IN ('{1}','{2}') AND IsDeleted = 0", hdnLinkedRegistrationID.Value, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            //else
            //    filterExpression = "1 = 0";

            filterExpression = string.Format("(LinkedToRegistrationID = {0} AND IsChargesTransfered = 1) AND GCTransactionStatus NOT IN ('{1}','{2}') AND IsDeleted = 0 AND PatientBillingID IS NULL", hdnRegistrationID.Value, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);

            return filterExpression;
        }

        private void BindGrid()
        {
            string filterExpression = GetFilterExpression();
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(filterExpression);
            if (lst.Count > 0) hdnRowCountData.Value = lst.Count.ToString();
            else hdnRowCountData.Value = "0";

            List<vPatientChargesDt8> lstService = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE || p.GCItemType == Constant.ItemGroupMaster.LABORATORY
                                                            || p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY || p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            ctlService.HideCheckBox();
            ctlService.BindGrid(lstService);

            List<vPatientChargesDt8> lstDrugMS = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            ctlDrugMS.HideCheckBox();
            ctlDrugMS.BindGrid(lstDrugMS);

            List<vPatientChargesDt8> lstLogistic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ctlLogistic.HideCheckBox();
            ctlLogistic.BindGrid(lstLogistic);
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            PatientBillDao patientBillDao = new PatientBillDao(ctx);

            if (type == "canceltransferbill")
            {
                try
                {
                    Registration entity = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                    List<Registration> lstFromReg = BusinessLayer.GetRegistrationList(string.Format("LinkedToRegistrationID = {0} AND GCRegistrationStatus != '{1}'", entity.RegistrationID, Constant.VisitStatus.CANCELLED), ctx);
                    foreach (Registration entityLinked in lstFromReg)
                    {
                        #region close bill
                        // ditutup oleh RN 20190522
                        //string filterExpression = string.Format("PatientBillingID IN (SELECT PatientBillingID FROM vPatientChargesHD WHERE RegistrationID = {0} AND PatientBillingID IN (SELECT PatientBillingID FROM PatientBill WHERE RegistrationID = {1} AND GCTransactionStatus = '{2}'))",
                        //                          hdnLinkedRegistrationID.Value, hdnRegistrationID.Value, Constant.TransactionStatus.OPEN);

                        string filterExpression = string.Format("RegistrationID IN ({0},{1}) AND GCTransactionStatus = '{2}'", entityLinked.RegistrationID, entityLinked.LinkedToRegistrationID, Constant.TransactionStatus.OPEN);
                        List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(filterExpression, ctx);
                        foreach (PatientBill patientBill in lstPatientBill)
                        {
                            patientBill.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientBill.LastUpdatedDate = DateTime.Now;
                            patientBillDao.Update(patientBill);
                        }

                        if (lstPatientBill.Count > 0)
                        {
                            String listBillingIDVoid = string.Join(",", lstPatientBill.Select(t => t.PatientBillingID));
                            List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PatientBillingID IN ({0})", listBillingIDVoid), ctx);
                            foreach (PatientChargesHd patientChargesHd in lstPatientChargesHd)
                            {
                                patientChargesHd.PatientBillingID = null;
                                patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                patientChargesHdDao.Update(patientChargesHd);
                            }
                        }
                        #endregion

                        string gcVisitStatusTo = Constant.VisitStatus.CHECKED_IN;

                        #region open registration and consult visit
                        string filter = string.Format("RegistrationID = {0} AND GCVisitStatus != '{1}'", entityLinked.RegistrationID, Constant.TransactionStatus.VOID);
                        List<ConsultVisit> entConsultList = BusinessLayer.GetConsultVisitList(filter, ctx);
                        foreach (ConsultVisit entConsult in entConsultList)
                        {
                            if (entConsult.DischargeDate != null && entConsult.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT)
                            {
                                entConsult.GCVisitStatus = Constant.VisitStatus.DISCHARGED;
                            }
                            else if (entConsult.PhysicianDischargedBy != null && entConsult.PhysicianDischargedDate != null && entConsult.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT)
                            {
                                entConsult.GCVisitStatus = Constant.VisitStatus.PHYSICIAN_DISCHARGE;
                            }
                            else
                            {
                                entConsult.GCVisitStatus = Constant.VisitStatus.CHECKED_IN;
                            }

                            gcVisitStatusTo = entConsult.GCVisitStatus;

                            entConsult.LastUpdatedBy = AppSession.UserLogin.UserID;
                            consultVisitDao.Update(entConsult);

                            #region change patient charges hd transfer status
                            filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus NOT IN ('{1}', '{2}') AND IsChargesTransfered = 1", entConsult.VisitID, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
                            List<PatientChargesHd> lstPatientChargesHdFinal = BusinessLayer.GetPatientChargesHdList(filterExpression, ctx);
                            foreach (PatientChargesHd enPch in lstPatientChargesHdFinal)
                            {
                                enPch.IsChargesTransfered = false;
                                enPch.LastUpdatedBy = AppSession.UserLogin.UserID;
                                patientChargesHdDao.Update(enPch);
                            }
                            #endregion
                        }

                        entityLinked.GCRegistrationStatus = gcVisitStatusTo;
                        entityLinked.IsChargesTransfered = false;
                        entityLinked.TransferAmount = 0;

                        entityLinked.ClosedBy = null;
                        entityLinked.ClosedDate = null;
                        entityLinked.ClosedTime = null;

                        entityLinked.LastUpdatedBy = AppSession.UserLogin.UserID;
                        registrationDao.Update(entityLinked);

                        #endregion
                    }

                    entity.SourceAmount = 0;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    registrationDao.Update(entity);

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
            }
            return result;
        }
    }
}