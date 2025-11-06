using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AIOAlocationTransactionEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.AIO_ALOCATION_TRANSACTION_ENTRY;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.AIO_ALOCATION_TRANSACTION_ENTRY;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.AIO_ALOCATION_TRANSACTION_ENTRY;
                    return Constant.MenuCode.MedicalDiagnostic.AIO_ALOCATION_TRANSACTION_ENTRY;
                default: return Constant.MenuCode.Outpatient.AIO_ALOCATION_TRANSACTION_ENTRY;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            BindGridDetail();
        }

        private void BindGridDetail()
        {
            string filterGridDetail = string.Format("RegistrationID = {0} ORDER BY ID", hdnRegistrationID.Value);
            List<vPatientChargesDt14> lst = BusinessLayer.GetvPatientChargesDt14List(filterGridDetail);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem || e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPatientChargesDt14 entity = e.Item.DataItem as vPatientChargesDt14;
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');

            BindGridDetail();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemMasterDao itemMasterDao = new ItemMasterDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            ConsultVisitItemPackageBalanceDao cvipbDao = new ConsultVisitItemPackageBalanceDao(ctx);
            ConsultVisitItemPackageMovementDao cvipmDao = new ConsultVisitItemPackageMovementDao(ctx);

            try
            {
                if (type == "linkaio")
                {
                    #region LINK AIO

                    if (hdnSelectedTransactionDtID.Value != null && hdnSelectedTransactionDtID.Value != "" && hdnSelectedBalanceTariffDtID.Value != null && hdnSelectedBalanceTariffDtID.Value != "")
                    {
                        List<String> lstChargesDtID = hdnSelectedTransactionDtID.Value.Split(',').ToList();
                        lstChargesDtID.RemoveAt(0);

                        for (int i = 0; i < lstChargesDtID.Count(); i++)
                        {
                            PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(lstChargesDtID[i]));
                            ItemMaster im = itemMasterDao.Get(entityDt.ItemID);

                            string filterCVIPB = string.Format("ID = (SELECT cvip.ID FROM ConsultVisitItemPackage cvip WITH(NOLOCK) WHERE cvip.VisitID = {0}) AND IsBalanceTariff = 1 AND DtID = {1}",
                                                                    hdnVisitID.Value, hdnSelectedBalanceTariffDtID.Value);
                            ConsultVisitItemPackageBalance cvipb = BusinessLayer.GetConsultVisitItemPackageBalanceList(filterCVIPB, ctx).FirstOrDefault();
                            if (cvipb != null)
                            {
                                decimal balanceEnd = 0;
                                string filterCVIPM = string.Format("DtID = {0}", cvipb.DtID);
                                ConsultVisitItemPackageMovement lastCVIPM = BusinessLayer.GetConsultVisitItemPackageMovementList(filterCVIPM, ctx).LastOrDefault();
                                if (lastCVIPM != null)
                                {
                                    balanceEnd = lastCVIPM.BalanceEND;

                                    if (balanceEnd > 0)
                                    {
                                        string checkCVIPM = string.Format("DtID = {0} AND PatientChargesDtID = {1} AND BalanceOUT = {2} AND IsBalanceTariff = 1 AND IsUnlinkChargesTariff = 0", cvipb.DtID, entityDt.ID, entityDt.LineAmount);
                                        List<ConsultVisitItemPackageMovement> checkCVIPMList = BusinessLayer.GetConsultVisitItemPackageMovementList(checkCVIPM, ctx);
                                        if (checkCVIPMList.Count() == 0)
                                        {
                                            ConsultVisitItemPackageMovement newCVIPM = new ConsultVisitItemPackageMovement();
                                            newCVIPM.DtID = cvipb.DtID;
                                            newCVIPM.DtItemID = entityDt.ItemID;
                                            newCVIPM.PatientChargesHdID = entityDt.TransactionID;
                                            newCVIPM.PatientChargesDtID = entityDt.ID;
                                            newCVIPM.BalanceBEGIN = balanceEnd;
                                            newCVIPM.BalanceOUT = entityDt.LineAmount;
                                            newCVIPM.IsBalanceTariff = true;
                                            newCVIPM.CreatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            cvipmDao.Insert(newCVIPM);

                                            cvipb.BalanceOUT += entityDt.LineAmount;
                                            cvipb.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            cvipbDao.Update(cvipb);
                                        }
                                        else
                                        {
                                            result = false;
                                            errMessage = string.Format("Untuk detail item {0} ({1}) sudah di-LINK-kan.", im.ItemName1, im.ItemCode);
                                            Exception ex = new Exception(errMessage);
                                            Helper.InsertErrorLog(ex);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        result = false;
                                        errMessage = string.Format("Balance untuk detail item {0} ({1}) sudah habis.", im.ItemName1, im.ItemCode);
                                        Exception ex = new Exception(errMessage);
                                        Helper.InsertErrorLog(ex);
                                        break;
                                    }
                                }
                            }
                        }

                        if (result)
                        {
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = GetErrorMsgSelectTransactionFirst();
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }

                    #endregion
                }
                else if (type == "unlinkaio")
                {
                    #region UN-LINK AIO

                    if (hdnSelectedTransactionDtID.Value != null && hdnSelectedTransactionDtID.Value != "" && hdnSelectedBalanceTariffDtID.Value != null && hdnSelectedBalanceTariffDtID.Value != "")
                    {
                        List<String> lstChargesDtID = hdnSelectedTransactionDtID.Value.Split(',').ToList();
                        lstChargesDtID.RemoveAt(0);

                        for (int i = 0; i < lstChargesDtID.Count(); i++)
                        {
                            PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(lstChargesDtID[i]));
                            ItemMaster im = itemMasterDao.Get(entityDt.ItemID);

                            string filterChargesDt = string.Format("ID = {0}", entityDt.ID);
                            vPatientChargesDt14 chargesDt14 = BusinessLayer.GetvPatientChargesDt14List(filterChargesDt, ctx).FirstOrDefault();

                            string filterCVIPB = string.Format("ID = (SELECT cvip.ID FROM ConsultVisitItemPackage cvip WITH(NOLOCK) WHERE cvip.VisitID = {0}) AND IsBalanceTariff = 1 AND DtID = {1}",
                                                                    hdnVisitID.Value, hdnSelectedBalanceTariffDtID.Value);
                            ConsultVisitItemPackageBalance cvipb = BusinessLayer.GetConsultVisitItemPackageBalanceList(filterCVIPB, ctx).FirstOrDefault();
                            if (cvipb != null && chargesDt14 != null)
                            {
                                decimal balanceEnd = cvipb.BalanceEND;
                                string filterCVIPM = string.Format("DtID = {0} AND PatientChargesDtID = {1} AND BalanceOUT = {2} AND IsBalanceTariff = 1 AND IsUnlinkChargesTariff = 0", cvipb.DtID, entityDt.ID, entityDt.LineAmount);
                                ConsultVisitItemPackageMovement lastCVIPM = BusinessLayer.GetConsultVisitItemPackageMovementList(filterCVIPM, ctx).LastOrDefault();
                                if (lastCVIPM != null)
                                {
                                    if (balanceEnd > 0)
                                    {
                                        if (chargesDt14.MovementID != null && chargesDt14.MovementID != 0)
                                        {
                                            lastCVIPM.IsUnlinkChargesTariff = true;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            cvipmDao.Update(lastCVIPM);

                                            ConsultVisitItemPackageMovement newCVIPM = new ConsultVisitItemPackageMovement();
                                            newCVIPM.DtID = cvipb.DtID;
                                            newCVIPM.DtItemID = entityDt.ItemID;
                                            newCVIPM.PatientChargesHdID = entityDt.TransactionID;
                                            newCVIPM.PatientChargesDtID = entityDt.ID;
                                            newCVIPM.BalanceBEGIN = balanceEnd;
                                            newCVIPM.BalanceIN = entityDt.LineAmount;
                                            newCVIPM.IsBalanceTariff = true;
                                            newCVIPM.IsUnlinkChargesTariff = true;
                                            newCVIPM.CreatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            cvipmDao.Insert(newCVIPM);

                                            cvipb.BalanceIN += entityDt.LineAmount;
                                            cvipb.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            cvipbDao.Update(cvipb);
                                        }
                                        else
                                        {
                                            result = false;
                                            errMessage = string.Format("Untuk detail item {0} ({1}) sudah di-UNLINK-kan.", im.ItemName1, im.ItemCode);
                                            Exception ex = new Exception(errMessage);
                                            Helper.InsertErrorLog(ex);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        result = false;
                                        errMessage = string.Format("Balance untuk detail item {0} ({1}) sudah habis.", im.ItemName1, im.ItemCode);
                                        Exception ex = new Exception(errMessage);
                                        Helper.InsertErrorLog(ex);
                                        break;
                                    }
                                }
                            }
                        }

                        if (result)
                        {
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = GetErrorMsgSelectTransactionFirst();
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }

                    #endregion
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