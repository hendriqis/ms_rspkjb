using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PrescriptionReturnOrderEntryCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = hdnParam.Value.Split('|');

            hdnVisitID.Value = temp[0];
            hdnLocationID.Value = temp[1];
            hdnDepartmentID.Value = temp[2];
            hdnHealthcareServiceUnitID.Value = temp[3];
            hdnRegistrationID.Value = temp[4];
            hdnPhysicianID.Value = temp[5];
            hdnTransactionDate.Value = temp[6];
            hdnTransactionTime.Value = temp[7];
            hdnReturnType.Value = temp[8];
            hdnPrescriptionReturnOrderID.Value = temp[9];
            hdnLinkedRegistrationID.Value = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value)).LinkedRegistrationID.ToString();
            BindGridView();
        }

        protected List<GetPrescriptionReturnOrderRemainingQty> lstEntity = null;
        private void BindGridView()
        {
            //string filterExpression = "";
            //if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
            //    filterExpression = string.Format("(RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value);
            //else
            //    filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            //filterExpression += string.Format(" AND LocationID = {0} AND IsDeleted = 0 ORDER BY ItemName1", hdnLocationID.Value);

            lstEntity = BusinessLayer.GetPrescriptionReturnOrderRemainingQtyList(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnLocationID.Value));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //GetPrescriptionReturnOrderRemainingQty entity = e.Row.DataItem as GetPrescriptionReturnOrderRemainingQty;
                //ASPxComboBox cboChargeClass = e.Row.FindControl("cboChargeClass") as ASPxComboBox;
                //var tempLst = from bs in lstEntity.Where(p => p.ItemID == entity.ItemID)
                //              group bs by bs.ChargeClassID into g
                //              select new ClassCare
                //              {
                //                  ClassID = g.Key,
                //                  ClassName = g.First().ChargeClassName
                //              };
                //Methods.SetComboBoxField<ClassCare>(cboChargeClass, tempLst.ToList(), "ClassName", "ClassID");
                //cboChargeClass.SelectedIndex = 0;

                //HtmlGenericControl divChargeClassID = e.Row.FindControl("divChargeClassID") as HtmlGenericControl;
                //divChargeClassID.InnerHtml = cboChargeClass.Value.ToString();
            }
        }

        protected void cbpViewDrugMSReturn_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionReturnOrderDtDao returnOrderDtDao = new PrescriptionReturnOrderDtDao(ctx);
            PrescriptionReturnOrderHdDao returnOrderHdDao = new PrescriptionReturnOrderHdDao(ctx);
            
            ItemMasterDao itemDao = new ItemMasterDao(ctx);
            try
            {
                if (hdnPrescriptionReturnOrderID.Value.ToString() == "0" || hdnPrescriptionReturnOrderID.Value.ToString() == "")
                {
                    List<PrescriptionReturnOrderDt> lstReturnOrderDt = new List<PrescriptionReturnOrderDt>();
                    int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                    int visitID = Convert.ToInt32(hdnVisitID.Value);
                    string[] lstSelectedMember = hdnSelectedMember.Value.Split(',');
                    string[] lstSelectedMemberItemID = hdnSelectedMemberItemID.Value.Split(',');
                    string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                    string[] lstSelectedMemberChargeClassID = hdnSelectedMemberChargeClassID.Value.Split(',');
                    string[] lstSelectedMemberGCItemUnit = hdnSelectedMemberGCItemUnit.Value.Split(',');

                    int ct = 0;
                    foreach (String chargesDtID in lstSelectedMember)
                    {
                        #region PrescriptionReturnOrderDt
                        PrescriptionReturnOrderDt returnOrderDt = new PrescriptionReturnOrderDt();
                        returnOrderDt.PatientChargesDtID = Convert.ToInt32(chargesDtID);
                        returnOrderDt.ItemID = Convert.ToInt32(lstSelectedMemberItemID[ct]);
                        returnOrderDt.ItemQty = Convert.ToDecimal(lstSelectedMemberQty[ct]) * -1;
                        returnOrderDt.GCItemUnit = lstSelectedMemberGCItemUnit[ct];
                        returnOrderDt.IsCreatedFromOrder = true;
                        returnOrderDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.OPEN;
                        returnOrderDt.IsDeleted = false;
                        returnOrderDt.LastUpdatedBy = returnOrderDt.CreatedBy = AppSession.UserLogin.UserID;
                        returnOrderDt.LastUpdatedDate = returnOrderDt.CreatedDate = DateTime.Now;
                        lstReturnOrderDt.Add(returnOrderDt);
                        #endregion
                        ct++;
                    }

                    #region PrescriptionReturnOrderHd
                    int prescriptionReturnOrderID = 0;
                    if (hdnPrescriptionReturnOrderID.Value == "" || hdnPrescriptionReturnOrderID.Value == "0")
                    {
                        PrescriptionReturnOrderHd returnOrderHd = new PrescriptionReturnOrderHd();
                        returnOrderHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_RETURN_ORDER;
                        returnOrderHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                        returnOrderHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                        returnOrderHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                        returnOrderHd.FromVisitID = Convert.ToInt32(hdnVisitID.Value);
                        returnOrderHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                        returnOrderHd.OrderDate = Helper.GetDatePickerValue(hdnTransactionDate.Value);
                        returnOrderHd.OrderTime = hdnTransactionTime.Value;
                        returnOrderHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        returnOrderHd.GCOrderStatus = Constant.TestOrderStatus.OPEN;
                        returnOrderHd.GCPrescriptionReturnType = hdnReturnType.Value.ToString();
                        returnOrderHd.PrescriptionReturnOrderNo = BusinessLayer.GenerateTransactionNo(returnOrderHd.TransactionCode, returnOrderHd.OrderDate, ctx);
                        returnOrderHd.LastUpdatedBy = returnOrderHd.CreatedBy = AppSession.UserLogin.UserID;
                        returnOrderHd.LastUpdatedDate = returnOrderHd.CreatedDate = DateTime.Now;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        returnOrderHdDao.Insert(returnOrderHd);
                        prescriptionReturnOrderID = BusinessLayer.GetPrescriptionReturnOrderHdMaxID(ctx);
                    }
                    else
                    {
                        prescriptionReturnOrderID = Convert.ToInt32(hdnPrescriptionReturnOrderID.Value);
                    }

                    retval = prescriptionReturnOrderID.ToString();
                    for (int ctr = 0; ctr < lstReturnOrderDt.Count(); ctr++)
                    {
                        lstReturnOrderDt[ctr].PrescriptionReturnOrderID = prescriptionReturnOrderID;
                        returnOrderDtDao.Insert(lstReturnOrderDt[ctr]);
                    }
                    #endregion

                    ctx.CommitTransaction();
                }
                else if (hdnPrescriptionReturnOrderID.Value.ToString() != "0" || hdnPrescriptionReturnOrderID.Value.ToString() != "")
                {
                    if (returnOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionReturnOrderID.Value)).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        List<PrescriptionReturnOrderDt> lstReturnOrderDt = new List<PrescriptionReturnOrderDt>();
                        int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                        int visitID = Convert.ToInt32(hdnVisitID.Value);
                        string[] lstSelectedMember = hdnSelectedMember.Value.Split(',');
                        string[] lstSelectedMemberItemID = hdnSelectedMemberItemID.Value.Split(',');
                        string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                        string[] lstSelectedMemberChargeClassID = hdnSelectedMemberChargeClassID.Value.Split(',');
                        string[] lstSelectedMemberGCItemUnit = hdnSelectedMemberGCItemUnit.Value.Split(',');

                        int ct = 0;
                        foreach (String chargesDtID in lstSelectedMember)
                        {
                            #region PrescriptionReturnOrderDt
                            PrescriptionReturnOrderDt returnOrderDt = new PrescriptionReturnOrderDt();
                            returnOrderDt.PatientChargesDtID = Convert.ToInt32(chargesDtID);
                            returnOrderDt.ItemID = Convert.ToInt32(lstSelectedMemberItemID[ct]);
                            returnOrderDt.ItemQty = Convert.ToDecimal(lstSelectedMemberQty[ct]) * -1;
                            returnOrderDt.GCItemUnit = lstSelectedMemberGCItemUnit[ct];
                            returnOrderDt.IsCreatedFromOrder = true;
                            returnOrderDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.OPEN;
                            returnOrderDt.IsDeleted = false;
                            returnOrderDt.LastUpdatedBy = returnOrderDt.CreatedBy = AppSession.UserLogin.UserID;
                            returnOrderDt.LastUpdatedDate = returnOrderDt.CreatedDate = DateTime.Now;
                            lstReturnOrderDt.Add(returnOrderDt);
                            #endregion
                            ct++;
                        }

                        #region PrescriptionReturnOrderHd
                        int prescriptionReturnOrderID = 0;
                        if (hdnPrescriptionReturnOrderID.Value == "" || hdnPrescriptionReturnOrderID.Value == "0")
                        {
                            PrescriptionReturnOrderHd returnOrderHd = new PrescriptionReturnOrderHd();
                            returnOrderHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_RETURN_ORDER;
                            returnOrderHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                            returnOrderHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                            returnOrderHd.FromVisitID = Convert.ToInt32(hdnVisitID.Value);
                            returnOrderHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                            returnOrderHd.OrderDate = Helper.GetDatePickerValue(hdnTransactionDate.Value);
                            returnOrderHd.OrderTime = hdnTransactionTime.Value;
                            returnOrderHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            returnOrderHd.GCOrderStatus = Constant.TestOrderStatus.OPEN;
                            returnOrderHd.GCPrescriptionReturnType = hdnReturnType.Value.ToString();
                            returnOrderHd.PrescriptionReturnOrderNo = BusinessLayer.GenerateTransactionNo(returnOrderHd.TransactionCode, returnOrderHd.OrderDate, ctx);
                            returnOrderHd.LastUpdatedBy = returnOrderHd.CreatedBy = AppSession.UserLogin.UserID;
                            returnOrderHd.LastUpdatedDate = returnOrderHd.CreatedDate = DateTime.Now;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            returnOrderHdDao.Insert(returnOrderHd);
                            prescriptionReturnOrderID = BusinessLayer.GetPrescriptionReturnOrderHdMaxID(ctx);
                        }
                        else
                        {
                            prescriptionReturnOrderID = Convert.ToInt32(hdnPrescriptionReturnOrderID.Value);
                        }

                        retval = prescriptionReturnOrderID.ToString();
                        for (int ctr = 0; ctr < lstReturnOrderDt.Count(); ctr++)
                        {
                            lstReturnOrderDt[ctr].PrescriptionReturnOrderID = prescriptionReturnOrderID;
                            returnOrderDtDao.Insert(lstReturnOrderDt[ctr]);
                        }
                        #endregion

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Order Retur Resep " + returnOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionReturnOrderID.Value)).PrescriptionReturnOrderNo + " tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
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