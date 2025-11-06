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
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MedicationOrderHistoryCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;

            string[] temp = hdnParam.Value.Split('|');
            hdnLocationID.Value = temp[0];
            hdnRegistrationID.Value = temp[1];
            hdnPrescriptionOrderID.Value = temp[2];
            hdnVisitID.Value = temp[3];
            hdnClassID.Value = temp[4];
            hdnParamedicID.Value = temp[5];
            hdnPrescriptionType.Value = temp[6];
            hdnDispensaryUnit.Value = temp[7];
            hdnDepartmentID.Value = temp[8];
            hdnIsDrugChargesJustDistributionHS.Value = temp[9];

            hdnMRN.Value = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value)).MRN.ToString();

            BindGridView();
        }

        private void BindGridView()
        {

            string filterExpression = "1=0";
            filterExpression = string.Format("MRN = {0}", hdnMRN.Value);
            filterExpression += string.Format(" AND LocationID = {0} AND IsDeleted = 0 AND GCTransactionStatus NOT IN ('{1}') ", hdnLocationID.Value, Constant.TransactionStatus.VOID);

            List<vPatientChargesDt5> lstvPatientChargesDt = BusinessLayer.GetvPatientChargesDt5List(filterExpression);

            string filterExpression2 = "IsRFlag = 1";

            if (lstvPatientChargesDt.Count != 0)
            {
                filterExpression2 += string.Format(" AND PrescriptionOrderDetailID IN ({0})", string.Join(",", lstvPatientChargesDt.Select(t => t.PrescriptionOrderDetailID)));
            }
            else
            {
                filterExpression2 += string.Format(" AND PrescriptionOrderDetailID IS NULL");
            }

            if (hdnPrescriptionOrderID.Value != "" && hdnPrescriptionOrderID.Value != "0")
            {
                filterExpression2 += string.Format(" AND ItemID NOT IN (SELECT ItemID FROM PrescriptionOrderDt WHERE PrescriptionOrderID = {0})", hdnPrescriptionOrderID.Value);
            }

            if (hdnIsDrugChargesJustDistributionHS.Value == "1")
            {
                filterExpression2 += string.Format(" AND ItemID IN (SELECT ItemID FROM ItemBalance WHERE GCItemRequestType = '{0}' AND IsDeleted = 0)", Constant.ItemRequestType.DISTRIBUTION);
            }

            List<vPrescriptionOrderDt> lstvPrescriptionOrderDt = BusinessLayer.GetvPrescriptionOrderDtList(filterExpression2);
            lstvPrescriptionOrderDt = lstvPrescriptionOrderDt.GroupBy(x => x.ItemID).Select(x => x.Last()).ToList();

            grdView.DataSource = lstvPrescriptionOrderDt;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPrescriptionOrderDt entity = e.Row.DataItem as vPrescriptionOrderDt;

                ASPxComboBox cboSignaName = e.Row.FindControl("cboSignaName") as ASPxComboBox;
                String filterExpression = string.Format("IsDeleted = 0");
                List<Signa> lstSigna = BusinessLayer.GetSignaList(filterExpression);
                Methods.SetComboBoxField<Signa>(cboSignaName, lstSigna.ToList(), "SignaName1", "SignaID");
                if (entity.SignaID.ToString() != "0")
                {
                    cboSignaName.Value = entity.SignaID.ToString();
                }
                else
                {
                    cboSignaName.SelectedIndex = 0;
                }

                HtmlGenericControl divSignaID = e.Row.FindControl("divSignaID") as HtmlGenericControl;
                divSignaID.InnerHtml = cboSignaName.Value.ToString();

                ASPxComboBox cboCoenamRule = e.Row.FindControl("cboCoenamRule") as ASPxComboBox;
                String filterExpression2 = string.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.COENAM_RULE);
                List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression2);
                lstStandardCode.Insert(0, new StandardCode { StandardCodeName = "", StandardCodeID = "0" });
                Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "0").ToList(), "StandardCodeName", "StandardCodeID");
                if (entity.GCCoenamRule.ToString() != "")
                {
                    cboCoenamRule.Value = entity.GCCoenamRule.ToString();
                }
                else
                {
                    cboCoenamRule.SelectedIndex = 0;
                }

                HtmlGenericControl divCoenamRuleID = e.Row.FindControl("divCoenamRuleID") as HtmlGenericControl;
                divCoenamRuleID.InnerHtml = cboCoenamRule.Value.ToString();
            }
        }

        protected void cbpViewMedicationOrderHistory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            errMessage = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);

            ItemMasterDao itemDao = new ItemMasterDao(ctx);

            try
            {
                List<PrescriptionOrderDt> lstPrescriptionOrderDt = new List<PrescriptionOrderDt>();
                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                string[] lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberSignaID = hdnSelectedMemberSignaID.Value.Split(',');
                string[] lstSelectedMemberCoenamRuleID = hdnSelectedMemberCoenamRuleID.Value.Split(',');
                string[] lstSelectedMemberDosingDuration = hdnSelectedMemberDosingDuration.Value.Split(',');
                string[] lstSelectedMemberGCDosingFrequency = hdnSelectedMemberGCDosingFrequency.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                string[] lstSelectedMemberGCItemUnit = hdnSelectedMemberGCItemUnit.Value.Split(',');

                bool isValidQty = true;
                foreach (string s in lstSelectedMemberQty)
                {
                    if (s == "0")
                    {
                        isValidQty = false;
                    }
                }

                if (isValidQty)
                {
                    int dt = 0;
                    foreach (String prescriptionOrderDtID in lstSelectedMember)
                    {
                        #region PrescriptionOrderDt

                        PrescriptionOrderDt orderDt = new PrescriptionOrderDt();

                        string tempItemID = BusinessLayer.GetPrescriptionOrderDt(Convert.ToInt32(prescriptionOrderDtID)).ItemID.ToString();

                        orderDt.ItemID = Convert.ToInt32(tempItemID);
                        orderDt.GenericName = BusinessLayer.GetDrugInfo(Convert.ToInt32(tempItemID)).GenericName;
                        orderDt.DrugName = BusinessLayer.GetItemMaster(Convert.ToInt32(tempItemID)).ItemName1;
                        orderDt.SignaID = Convert.ToInt32(lstSelectedMemberSignaID[dt]);
                        orderDt.GCDrugForm = BusinessLayer.GetDrugInfo(Convert.ToInt32(tempItemID)).GCDrugForm.ToString();
                        orderDt.Dose = BusinessLayer.GetDrugInfo(Convert.ToInt32(tempItemID)).Dose;
                        orderDt.GCDoseUnit = BusinessLayer.GetDrugInfo(Convert.ToInt32(tempItemID)).GCDoseUnit.ToString();
                        orderDt.NumberOfDosage = BusinessLayer.GetSigna(Convert.ToInt32(lstSelectedMemberSignaID[dt])).Dose;
                        orderDt.Frequency = BusinessLayer.GetSigna(Convert.ToInt32(lstSelectedMemberSignaID[dt])).Frequency;

                        if (lstSelectedMemberCoenamRuleID[dt] != "0")
                        {
                            orderDt.GCCoenamRule = lstSelectedMemberCoenamRuleID[dt];
                        }

                        orderDt.DispenseQty = Convert.ToDecimal(lstSelectedMemberQty[dt]);
                        orderDt.TakenQty = Convert.ToDecimal(lstSelectedMemberQty[dt]);
                        orderDt.ResultQty = Convert.ToDecimal(lstSelectedMemberQty[dt]);
                        orderDt.ChargeQty = Convert.ToDecimal(lstSelectedMemberQty[dt]);
                        orderDt.GCDosingUnit = lstSelectedMemberGCItemUnit[dt];
                        orderDt.DosingDuration = Convert.ToDecimal(lstSelectedMemberDosingDuration[dt]);
                        orderDt.GCDosingFrequency = lstSelectedMemberGCDosingFrequency[dt];
                        orderDt.GCRoute = BusinessLayer.GetPrescriptionOrderDt(Convert.ToInt32(prescriptionOrderDtID)).GCRoute;
                        orderDt.IsCreatedFromOrder = true;
                        orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;
                        orderDt.IsRFlag = true;
                        orderDt.StartDate = DateTime.Now;
                        orderDt.StartTime = DateTime.Now.ToString("HH:mm");
                        orderDt.CreatedBy = AppSession.UserLogin.UserID;
                        orderDt.CreatedDate = DateTime.Now;

                        lstPrescriptionOrderDt.Add(orderDt);

                        #endregion
                        dt++;
                    }

                    #region PrescriptionOrderHd
                    int hd = 0;
                    if (hdnPrescriptionOrderID.Value == "" || hdnPrescriptionOrderID.Value == "0")
                    {
                        #region PrescriptionOrderHd
                        PrescriptionOrderHd entityHd = new PrescriptionOrderHd();
                        if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                            entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                        else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                            entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                        else
                            entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                        entityHd.PrescriptionDate = DateTime.Now;
                        entityHd.PrescriptionTime = DateTime.Now.ToString("HH:mm");
                        entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                        entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                        entityHd.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                        entityHd.ClassID = Convert.ToInt32(hdnClassID.Value);
                        entityHd.LocationID = Convert.ToInt32(hdnLocationID.Value);
                        entityHd.DispensaryServiceUnitID = Convert.ToInt32(hdnDispensaryUnit.Value);
                        entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                        entityHd.GCPrescriptionType = hdnPrescriptionType.Value;
                        entityHd.GCOrderStatus = Constant.TestOrderStatus.OPEN;
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        entityHd.CreatedDate = DateTime.Now;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        hd = orderHdDao.InsertReturnPrimaryKeyID(entityHd);
                        #endregion
                    }
                    else
                    {
                        hd = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                    }

                    if (orderHdDao.Get(hd).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        retval = hd.ToString();
                        for (int ctr = 0; ctr < lstPrescriptionOrderDt.Count(); ctr++)
                        {
                            lstPrescriptionOrderDt[ctr].PrescriptionOrderID = hd;
                            orderDtDao.Insert(lstPrescriptionOrderDt[ctr]);
                        }

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                        ctx.RollBackTransaction();
                    }
                    #endregion
                }
                else
                {
                    result = false;
                    errMessage = "Quantity Resep tidak boleh kurang dari atau sama dengan 0 !";
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
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