using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PrescriptionEntry1 : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.ONLINE_PRESCRIPTION;
        }

        #region List

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = true;
            IsAllowEdit = false;
            IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND IsDeleted = 0", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = AppSession.UserLogin.ParamedicID;
            hdnPhysicianID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnChargeClassID.Value = AppSession.RegisteredPatient.ChargeClassID.ToString();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            Int32 hour = DateTime.Now.Hour;
            Int32 minute = DateTime.Now.Minute;
            string hourInString = "";
            string minuteInString = "";
            if (hour < 10)
            {
                hourInString = string.Format("0{0}", hour);
            }
            else
            {
                hourInString = string.Format("{0}", hour);
            }

            if (minute < 10)
            {
                minuteInString = string.Format("0{0}", minute);
            }
            else
            {
                minuteInString = string.Format("{0}", minute);
            }

            hdnPrescriptionDate.Value = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnPrescriptionTime.Value = string.Format("{0}:{1}", hourInString, minuteInString);

            //hdnPrescriptionDate.Value = Helper.GetDatePickerValue(txtPrescriptionDate).ToString();
            //hdnPrescriptionTime.Value = txtPrescriptionTime.Text;

            String filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.REFILL_INSTRUCTION);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboRefillInstruction, lstStandardCode, "StandardCodeName", "StandardCodeID");

            filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')",
                                                AppSession.UserLogin.HealthcareID, //0
                                                Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS, //1
                                                Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN, //2
                                                Constant.SettingParameter.IP0013, //3
                                                Constant.SettingParameter.OP0016, //4
                                                Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS, //5
                                                Constant.SettingParameter.FN_PENJAMIN_INHEALTH, //6
                                                Constant.SettingParameter.EM_PEMBATASAN_CPOE_INHEALTH, //7
                                                Constant.SettingParameter.FM_USING_UDD_FOR_INPATIENT, //8
                                                Constant.SettingParameter.EM_ORDER_RESEP_BISA_PILIH_DISPENSARY_FARMASI, //9
                                                Constant.SettingParameter.EM_ORDER_RESEP_HANYA_BISA_PILIH_ITEM_STOK_RS, //10
                                                Constant.SettingParameter.EM_IS_VALIDATION_EMPTY_STOCK_PRESCRIPTION_ORDER, //11
                                                Constant.SettingParameter.IS_USING_DRUG_ALERT //12
                                            );
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);

            SettingParameterDt oParam1 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS).FirstOrDefault();
            SettingParameterDt oParam2 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault();
            SettingParameterDt oParam3 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_PEMBATASAN_CPOE_INHEALTH).FirstOrDefault();
            SettingParameterDt oParam4 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_INHEALTH).FirstOrDefault();
            SettingParameterDt oParam5 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FM_USING_UDD_FOR_INPATIENT).FirstOrDefault();
            SettingParameterDt oParam6 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_ORDER_RESEP_BISA_PILIH_DISPENSARY_FARMASI).FirstOrDefault();
            SettingParameterDt oParam7 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IS_USING_DRUG_ALERT).FirstOrDefault();

            hdnIsUsingDrugAlert.Value = oParam7.ParameterValue;
            hdnPrescriptionValidateStockAllRS.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_ORDER_RESEP_HANYA_BISA_PILIH_ITEM_STOK_RS).FirstOrDefault().ParameterValue;
            hdnValidationEmptyStock.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_IS_VALIDATION_EMPTY_STOCK_PRESCRIPTION_ORDER).FirstOrDefault().ParameterValue;
            hdnIsLimitedCPOEItemForBPJS.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS).FirstOrDefault().ParameterValue;

            string bpjsID = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault().ParameterValue;
            string bpjsType = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).FirstOrDefault().ParameterValue;
            bool isLimitedCPOEItemForBPJS = oParam1 != null ? (oParam1.ParameterValue == "1" ? true : false) : false;

            if (string.IsNullOrEmpty(bpjsID))
                bpjsID = "0";

            bool isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealth;

            if (!isLimitedCPOEItemForInhealth)
            {
                switch (AppSession.RegisteredPatient.DepartmentID)
                {
                    case Constant.Facility.INPATIENT:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthIP;
                        break;
                    case Constant.Facility.OUTPATIENT:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthOP;
                        break;
                    case Constant.Facility.EMERGENCY:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthER;
                        break;
                    case Constant.Facility.DIAGNOSTIC:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthMD;
                        break;
                    case Constant.Facility.PHARMACY:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthPH;
                        break;
                    case Constant.Facility.MEDICAL_CHECKUP:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthMC;
                        break;
                    default:
                        break;
                }
            }

            string inHealthCustomerType = oParam4 != null ? oParam4.ParameterValue : string.Empty;
            hdnIsUsingUDD.Value = oParam5.ParameterValue;

            hdnFilterExpressionItem.Value = string.Format("IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '{0}'", Constant.ItemStatus.IN_ACTIVE);

            if (AppSession.RegisteredPatient.DepartmentID != Constant.Facility.OUTPATIENT)
            {
                trTakenTime.Style.Add("display", "none");
            }

            if (hdnIsLimitedCPOEItemForBPJS.Value == "1")
            {
                if (AppSession.RegisteredPatient.GCCustomerType == bpjsType)
                {
                    if (hdnPrescriptionOrderID.Value != "" && hdnPrescriptionOrderID.Value != "1")
                    {
                        hdnFilterExpressionItem.Value = string.Format("ItemID NOT IN (SELECT ItemID FROM PrescriptionOrderDt WHERE PrescriptionOrderID = {0} AND IsDeleted = 0) AND IsBPJSFormularium = 1 AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '{1}'", hdnPrescriptionOrderID.Value, Constant.ItemStatus.IN_ACTIVE);
                    }
                    else
                    {
                        hdnFilterExpressionItem.Value = string.Format("IsBPJSFormularium = 1 AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '{0}'", Constant.ItemStatus.IN_ACTIVE);
                    }
                }
            }

            if (AppSession.RegisteredPatient.GCCustomerType == inHealthCustomerType && isLimitedCPOEItemForInhealth)
            {
                if (hdnPrescriptionOrderID.Value != "" && hdnPrescriptionOrderID.Value != "1")
                {
                    hdnFilterExpressionItem.Value = string.Format("ItemID NOT IN (SELECT ItemID FROM PrescriptionOrderDt WHERE PrescriptionOrderID = {0} AND IsDeleted = 0) AND IsInhealthFormularium = 1 AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '{1}'", hdnPrescriptionOrderID.Value, Constant.ItemStatus.IN_ACTIVE);
                }
                else
                {
                    hdnFilterExpressionItem.Value = string.Format("IsInhealthFormularium = 1 AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '{0}'", Constant.ItemStatus.IN_ACTIVE);
                }
            }

            //ledDrugName.FilterExpression = hdnFilterExpressionItem.Value;
            //txtQuickEntry.QuickEntryHints[0].FilterExpression = hdnFilterExpressionItem.Value;

            //Get Service Unit Dispensary Unit

            string filterDispensaryUnit = string.Format("IsDeleted = 0 AND DepartmentID = '{0}'", Constant.Facility.PHARMACY);
            if (hdnIsUsingUDD.Value == "1")
            {
                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    filterDispensaryUnit += " AND IsInpatientDispensary = 1";
                }
                else
                {
                    filterDispensaryUnit += " AND IsInpatientDispensary = 0";
                }
            }
            List<vHealthcareServiceUnitCustom> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitCustomList(filterDispensaryUnit);
            Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboDispensaryUnit, lstHealthcareServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");

            if (AppSession.RegisteredPatient.GCCustomerType == bpjsType)
            {
                hdnIsBPJS.Value = "1";

                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    hdnDefaultDispensaryServiceUnitID.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IP0013).FirstOrDefault().ParameterValue;
                    cboDispensaryUnit.Value = hdnDefaultDispensaryServiceUnitID.Value.ToString();
                }
                else
                {
                    if (AppSession.RegisteredPatient.DepartmentID != Constant.Facility.EMERGENCY)
                    {
                        hdnDefaultDispensaryServiceUnitID.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.OP0016).FirstOrDefault().ParameterValue;
                        cboDispensaryUnit.Value = hdnDefaultDispensaryServiceUnitID.Value.ToString();
                    }
                    if (string.IsNullOrEmpty(hdnDefaultDispensaryServiceUnitID.Value))
                    {
                        HealthcareServiceUnit oServiceUnit = BusinessLayer.GetHealthcareServiceUnit(AppSession.RegisteredPatient.HealthcareServiceUnitID);
                        cboDispensaryUnit.Value = oServiceUnit.DispensaryServiceUnitID.ToString();
                    }
                }
            }
            else
            {
                HealthcareServiceUnit oServiceUnit = BusinessLayer.GetHealthcareServiceUnit(AppSession.RegisteredPatient.HealthcareServiceUnitID);
                if (oServiceUnit != null)
                {
                    cboDispensaryUnit.Value = oServiceUnit.DispensaryServiceUnitID.ToString();
                }
            }

            try
            {
                if (AppSession.IsAutoRelocateDispensary == "1" && AppSession.AutoRelocateDispensaryTime != "" && AppSession.AutoRelocateDispensaryID != "")
                {
                    DateTime currentDate = DateTime.Now.Date;
                    string controlTime = AppSession.AutoRelocateDispensaryTime;

                    DateTime controlDateTime = DateTime.ParseExact(string.Format("{0} {1}", currentDate.ToString(Constant.FormatString.DATE_FORMAT_112), controlTime), "yyyyMMdd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime currentDateTime = DateTime.Now;
                    if (currentDateTime > controlDateTime)
                    {
                        cboDispensaryUnit.Value = Convert.ToInt32(AppSession.AutoRelocateDispensaryID).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
            }

            if (Page.Request.QueryString.Count > 0)
            {
                hdnPrescriptionOrderID.Value = Page.Request.QueryString["id"];
            }

            BindCboLocation();
            if (cboLocation.Value != null)
            {
                hdnLocationID.Value = cboLocation.Value.ToString();
            }

            hdnFilterExpressionItemBase.Value = hdnFilterExpressionItem.Value;

            hdnFilterExpressionItem.Value += string.Format(" AND LocationID = {0} AND GCItemType IN ('X001^002','X001^003') AND GCItemStatus = 'X181^001' AND IsDeleted = 0", cboLocation.Value.ToString());
            hdnFilterExpressionItemDrugInfo.Value += string.Format(" AND LocationID = {0} AND GCItemType IN ('X001^002','X001^003') AND GCItemStatus = 'X181^001' AND IsDeleted = 0", cboLocation.Value.ToString());

            if (hdnPrescriptionValidateStockAllRS.Value == "1")
            {
                hdnFilterExpressionItem.Value += " AND TotalQtyOnHand > 0";
                hdnFilterExpressionItemEdit.Value += " AND TotalQtyOnHand > 0";
                hdnFilterExpressionItemNewTransHd.Value += " AND TotalQtyOnHand > 0";
            }

            ledDrugName.FilterExpression = hdnFilterExpressionItemDrugInfo.Value;
            txtQuickEntry.QuickEntryHints[0].FilterExpression = hdnFilterExpressionItem.Value;

            if (oParam6.ParameterValue == "1")
            {
                cboDispensaryUnit.ClientEnabled = false;
            }

            BindGridView(1, true, ref PageCount);
        }

        private void LoadHeaderInformation()
        {
            string filterExpression;

            //hdnFilterExpressionItem.Value = string.Format("ItemID NOT IN (SELECT ItemID FROM PrescriptionOrderDt WHERE PrescriptionOrderID = {0} AND IsDeleted = 0) AND IsDeleted = 0", hdnPrescriptionOrderID.Value);
            PrescriptionOrderHd entity = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnPrescriptionOrderID.Value));

            txtPrescriptionNo.Text = entity.PrescriptionOrderNo;
            txtPrescriptionDate.Text = entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entity.PrescriptionTime;
            cboParamedicID.ClientEnabled = false;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            txtRemarks.Text = entity.Remarks;
            cboDispensaryUnit.Value = entity.DispensaryServiceUnitID.ToString();
            cboDispensaryUnit.ClientEnabled = false;
            cboLocation.Value = entity.LocationID.ToString();
            cboLocation.ClientEnabled = false;
            hdnDefaultDispensaryServiceUnitID.Value = entity.DispensaryServiceUnitID.ToString();

            cboPrescriptionType.Value = entity.GCPrescriptionType;
            //cboPrescriptionType.ClientEnabled = false;

            cboRefillInstruction.ClientEnabled = false;
            cboRefillInstruction.Value = entity.GCRefillInstruction.ToString();

            filterExpression = hdnFilterExpressionItem.Value;
            txtQuickEntry.QuickEntryHints[0].FilterExpression = filterExpression;
            ledDrugName.FilterExpression = filterExpression;

            hdnIsProposed.Value = entity.GCTransactionStatus == Constant.TransactionStatus.OPEN ? "0" : "1";
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnPrescriptionOrderID.Value != "")
            {
                filterExpression = hdnFilterExpression.Value;
                if (filterExpression != "")
                    filterExpression += " AND ";
                filterExpression += string.Format("PrescriptionOrderID = {0} AND ParentID IS NULL AND OrderIsDeleted = 0", hdnPrescriptionOrderID.Value);

                if (hdnPrescriptionValidateStockAllRS.Value == "1")
                {
                    hdnFilterExpressionItem.Value += " AND TotalQtyOnHand > 0";
                }

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPrescriptionOrderDt1RowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionOrderDetailID");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "refreshHeader")
                {
                    LoadHeaderInformation();
                    result = "refresh|" + pageCount + "|" + txtPrescriptionNo.Text;
                }
                else // refresh
                {
                    if (Page.Request.QueryString.Count == 0)
                    {
                        LoadHeaderInformation();
                    }
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount + "|" + txtPrescriptionNo.Text;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }


        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPrescriptionOrderDt1 entity = e.Row.DataItem as vPrescriptionOrderDt1;

                HtmlImage imgHAM = e.Row.FindControl("imgHAM") as HtmlImage;
                if (imgHAM != null)
                {
                    imgHAM.Visible = entity.IsHAM;
                }

                HtmlImage imgIsHasRestriction = e.Row.FindControl("imgIsHasRestriction") as HtmlImage;
                HtmlInputText lblHasRestrictionInformation = e.Row.FindControl("lblHasRestrictionInformation") as HtmlInputText;
                if (imgIsHasRestriction != null)
                {
                    if (entity.IsHasRestrictionInformation == true)
                    {
                        if (!string.IsNullOrEmpty(entity.RestrictionInformation))
                        {
                            imgIsHasRestriction.Attributes["title"] = string.Format("{0}", entity.RestrictionInformation);
                            imgIsHasRestriction.Visible = true;
                            if (lblHasRestrictionInformation != null)
                            {
                                lblHasRestrictionInformation.Value = "1";
                            }
                        }
                        else
                        {
                            imgIsHasRestriction.Visible = true;
                            imgIsHasRestriction.Attributes["title"] = string.Format("Drug Restriction");
                            if (lblHasRestrictionInformation != null)
                            {
                                lblHasRestrictionInformation.Value = "0";
                            }
                        }
                    }
                    else
                    {
                        imgIsHasRestriction.Visible = false;
                    }
                }

                HtmlImage imgPPRA = e.Row.FindControl("imgPPRA") as HtmlImage;
                if (imgPPRA != null)
                {
                    if (entity.IsRestrictiveAntibiotics == true)
                    {
                        imgPPRA.Visible = true;
                    }
                    else
                    {
                        imgPPRA.Visible = false;
                    }
                }
            }
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderDtOriginalDao entityDtOrginalDao = new PrescriptionOrderDtOriginalDao(ctx);
            try
            {
                if (hdnID.Value != "")
                {
                    string filterOrderDt = string.Format("(PrescriptionOrderDetailID = {0} OR ParentID = {0}) AND IsDeleted = 0", hdnID.Value);
                    List<PrescriptionOrderDt> lstPrescriptionDt = BusinessLayer.GetPrescriptionOrderDtList(filterOrderDt, ctx);
                    string PrescriptionOrderDetailID = "";
                    foreach (PrescriptionOrderDt entityDt in lstPrescriptionDt)
                    {
                        entityDt.IsDeleted = true;
                        entityDt.GCPrescriptionOrderStatus = Constant.OrderStatus.CANCELLED;
                        entityDt.GCVoidReason = Constant.DeleteReason.OTHER;
                        entityDt.VoidReason = "Deleted from order detail.";
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);

                        PrescriptionOrderDetailID += string.Format("{0},", entityDt.PrescriptionOrderDetailID);
                    }
                    //dt original
                    if (!string.IsNullOrEmpty(PrescriptionOrderDetailID))
                    {
                        PrescriptionOrderDetailID = PrescriptionOrderDetailID.Remove(PrescriptionOrderDetailID.Length - 1);
                        if (!string.IsNullOrEmpty(PrescriptionOrderDetailID))
                        {
                            string filterOrderDtoriginal = string.Format("PrescriptionOrderDetailID IN({0})", PrescriptionOrderDetailID);
                            List<PrescriptionOrderDtOriginal> lstDtOriginal = BusinessLayer.GetPrescriptionOrderDtOriginalList(filterOrderDtoriginal, ctx);
                            foreach (PrescriptionOrderDtOriginal entityDtOriginal in lstDtOriginal)
                            {
                                entityDtOriginal.IsDeleted = true;
                                entityDtOriginal.GCPrescriptionOrderStatus = Constant.OrderStatus.CANCELLED;
                                entityDtOriginal.GCVoidReason = Constant.DeleteReason.OTHER;
                                entityDtOriginal.VoidReason = "Deleted from order detail.";
                                entityDtOriginal.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtOrginalDao.Update(entityDtOriginal);
                            }
                        }
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Silahkan coba lagi.";
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
        #endregion

        #region Entry
        protected override void SetControlProperties()
        {
            if (hdnPrescriptionOrderID.Value != "")
            {
                string tempFilterExpression = string.Format("ItemID NOT IN (SELECT ItemID FROM PrescriptionOrderDt WHERE PrescriptionOrderID = {0} AND IsDeleted = 0)", hdnPrescriptionOrderID.Value);
                hdnFilterExpressionItemAdd.Value = tempFilterExpression;

                tempFilterExpression = string.Format("(ItemID NOT IN (SELECT ItemID FROM PrescriptionOrderDt WHERE PrescriptionOrderID = {0} AND IsDeleted = 0) OR ItemID = {{ItemID}})", hdnPrescriptionOrderID.Value);
                hdnFilterExpressionItemEdit.Value = tempFilterExpression;
            }
            else
            {
                string tempFilterExpression = string.Format("ItemID NOT IN (SELECT ItemID FROM PrescriptionOrderDt WHERE PrescriptionOrderID = {{PrescriptionOrderID}} AND IsDeleted = 0)");
                hdnFilterExpressionItemAdd.Value = tempFilterExpression;

                tempFilterExpression = string.Format("(ItemID NOT IN (SELECT ItemID FROM PrescriptionOrderDt WHERE PrescriptionOrderID = {{PrescriptionOrderID}} AND IsDeleted = 0) OR ItemID = {{ItemID}})");
                hdnFilterExpressionItemEdit.Value = tempFilterExpression;
            }

            if (hdnIsLimitedCPOEItemForBPJS.Value == "1")
            {
                if (AppSession.RegisteredPatient.BusinessPartnerID.ToString() == AppSession.BusinessPartnerIDBPJS.ToString())
                {
                    hdnFilterExpressionItemAdd.Value += " AND IsBPJSFormularium = 1";
                    hdnFilterExpressionItemEdit.Value += " AND IsBPJSFormularium = 1";
                }
            }

            bool isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealth;

            if (!isLimitedCPOEItemForInhealth)
            {
                switch (AppSession.RegisteredPatient.DepartmentID)
                {
                    case Constant.Facility.INPATIENT:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthIP;
                        break;
                    case Constant.Facility.OUTPATIENT:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthOP;
                        break;
                    case Constant.Facility.EMERGENCY:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthER;
                        break;
                    case Constant.Facility.DIAGNOSTIC:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthMD;
                        break;
                    case Constant.Facility.PHARMACY:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthPH;
                        break;
                    case Constant.Facility.MEDICAL_CHECKUP:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthMC;
                        break;
                    default:
                        break;
                }
            }

            if (AppSession.RegisteredPatient.GCCustomerType == AppSession.BusinessPartnerIDInhealth && isLimitedCPOEItemForInhealth)
            {
                hdnFilterExpressionItemAdd.Value += " AND IsInhealthFormularium = 1";
                hdnFilterExpressionItemEdit.Value += " AND IsInhealthFormularium = 1";
            }

            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.PRESCRIPTION_TYPE, Constant.StandardCode.DRUG_FORM, Constant.StandardCode.COENAM_RULE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDrugForm, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && (sc.TagProperty.Contains("PRE") || sc.TagProperty.Contains("1"))).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE).ToList(), "StandardCodeName", "StandardCodeID");

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                if (!AppSession.IsUsedInpatientPrescriptionTypeFilter)
                {
                    Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
                }
                else
                {
                    if (!string.IsNullOrEmpty(AppSession.InpatientPrescriptionTypeFilter))
                    {
                        string[] prescriptionType = AppSession.InpatientPrescriptionTypeFilter.Split(',');
                        Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).Where(x => !prescriptionType.Contains(x.StandardCodeID)).ToList(), "StandardCodeName", "StandardCodeID");
                    }
                    else
                    {
                        Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
                    }
                }
            }
            else
            {
                Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            }


            if (AppSession.RegisteredPatient.DepartmentID != Constant.Facility.INPATIENT)
            {
                cboPrescriptionType.Value = Constant.PrescriptionType.DISCHARGE_PRESCRIPTION;
            }
            else
            {
                if (AppSession.RegisteredPatient.IsPlanDischarge)
                {
                    cboPrescriptionType.Value = Constant.PrescriptionType.DISCHARGE_PRESCRIPTION;
                }
                else
                {
                    if (hdnPrescriptionOrderID.Value == "" || hdnPrescriptionOrderID.Value == "0")
                    {
                        cboPrescriptionType.SelectedIndex = 0;
                    }
                }
            }

            cboFrequencyTimeline.SelectedIndex = 1;
            cboDosingUnit.SelectedIndex = 0;
            cboMedicationRoute.SelectedIndex = 0;

            txtStartDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime1.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtStartTime.Text = txtStartTime.Text;

            txtAllergyInfo.Text = AppSession.PatientAllergyInfo;

            if (hdnPrescriptionOrderID.Value != "")
            {
                LoadHeaderInformation();
            }
            else
            {
                txtPrescriptionNo.Text = string.Empty;
                cboRefillInstruction.SelectedIndex = 0;
                txtPrescriptionDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtPrescriptionTime.Text = DateTime.Now.ToString("HH:mm");
                txtRemarks.Text = "";

                if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
                {
                    int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                    cboParamedicID.ClientEnabled = false;
                    cboParamedicID.Value = userLoginParamedic.ToString();
                }
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtGenericName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtStrengthAmount, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboFrequencyTimeline, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDosingUnit, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPurposeOfMedication, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDosingDuration, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtStartTime1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDispensaryUnit, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboLocation, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(cboCoenamRule, new ControlEntrySetting(true, true, false));
        }

        private void ControlToEntity(PrescriptionOrderDt entity)
        {
            entity.IsRFlag = chkIsRx.Checked;
            if (hdnDrugID.Value.ToString() != "")
            {
                entity.ItemID = Convert.ToInt32(hdnDrugID.Value);
            }
            else
            {
                entity.ItemID = null;
            }
            entity.DrugName = hdnDrugName.Value;
            entity.GenericName = txtGenericName.Text;

            {
                if (cboDrugForm.Value != null)
                    entity.GCDrugForm = cboDrugForm.Value.ToString();
            }

            if (hdnSignaID.Value != "")
            {
                entity.SignaID = Convert.ToInt32(hdnSignaID.Value);
            }
            else
            {
                entity.SignaID = null;
            }
            string strengthUnit = hdnStrengthUnit.Value;
            string strengthAmount = hdnStrengthAmount.Value;
            if (!String.IsNullOrEmpty(strengthUnit))
            {
                entity.Dose = Convert.ToDecimal(strengthAmount);
                entity.GCDoseUnit = strengthUnit;
            }
            else
            {
                entity.Dose = null;
                entity.GCDoseUnit = null;
            }
            entity.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
            entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            entity.GCDosingUnit = cboDosingUnit.Value.ToString();

            if (!string.IsNullOrEmpty(cboMedicationRoute.Value.ToString()))
            {
                entity.GCRoute = cboMedicationRoute.Value.ToString();
            }
            else
            {
                entity.GCRoute = Constant.MedicationRoute.OTHER;
            }

            if (cboCoenamRule.Value != null)
            {
                entity.GCCoenamRule = cboCoenamRule.Value.ToString();
            }
            else
            {
                entity.GCCoenamRule = null;
            }

            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.Sequence1Time = txtStartTime1.Text.Replace('.', ':');
            entity.Sequence2Time = txtStartTime2.Text.Replace('.', ':');
            entity.Sequence3Time = txtStartTime3.Text.Replace('.', ':');
            entity.Sequence4Time = txtStartTime4.Text.Replace('.', ':');
            entity.Sequence5Time = txtStartTime5.Text.Replace('.', ':');
            entity.Sequence6Time = txtStartTime6.Text.Replace('.', ':');
            entity.StartTime = entity.Sequence1Time != "-" ? entity.Sequence1Time : txtStartTime.Text;
            entity.IsMorning = chkIsMorning.Checked;
            entity.IsNoon = chkIsNoon.Checked;
            entity.IsEvening = chkIsEvening.Checked;
            entity.IsNight = chkIsNight.Checked;
            entity.MedicationPurpose = txtPurposeOfMedication.Text;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
            entity.DosingDuration = Convert.ToDecimal(txtDosingDuration.Text);
            entity.IsAsRequired = chkIsAsRequired.Checked;
            entity.IsIMM = chkIsIMM.Checked;
            entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
            entity.TakenQty = Convert.ToDecimal(txtDispenseQty.Text);
            entity.ResultQty = entity.DispenseQty;
            entity.ChargeQty = entity.DispenseQty;
            entity.GCPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;
        }

        protected void cboLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
            hdnLocationID.Value = cboLocation.Value.ToString();
        }

        private void BindCboLocation()
        {
            if (cboDispensaryUnit.Value != null)
            {
                Location location = BusinessLayer.GetLocationList(string.Format("LocationID IN (SELECT LocationID FROM HealthcareServiceUnit WHERE HealthcareServiceUnitID = {0})", cboDispensaryUnit.Value)).FirstOrDefault();

                if (location != null)
                {
                    int locationID = location.LocationID;
                    Location loc = BusinessLayer.GetLocation(locationID);
                    List<Location> lstLocation = null;
                    if (loc.IsHeader)
                        lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                    else
                    {
                        lstLocation = new List<Location>();
                        lstLocation.Add(loc);
                    }
                    Methods.SetComboBoxField<Location>(cboLocation, lstLocation, "LocationName", "LocationID");
                    cboLocation.SelectedIndex = 0;
                }
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            errMessage = string.Empty;
            vDrugInfo entityItem = null;
            entityItem = BusinessLayer.GetvDrugInfoList(string.Format("ItemID = {0}", hdnDrugID.Value))[0];
            bool result = true;

            if (!string.IsNullOrEmpty(txtDosingDuration.Text) && !string.IsNullOrEmpty(txtDispenseQty.Text) && !string.IsNullOrEmpty(txtDosingDose.Text))
            {
                decimal dispenseQty = 0;
                decimal doseQty = 0;
                decimal dosingDuration = 0;
                bool isDecimal = decimal.TryParse(txtDispenseQty.Text, out dispenseQty);
                bool isDecimalDose = decimal.TryParse(txtDosingDose.Text, out doseQty);
                bool isDecimalDosing = decimal.TryParse(txtDosingDuration.Text, out dosingDuration);

                if (dispenseQty <= 0)
                {
                    result = false;
                    errMessage = "Dispense Quantity should be greater than 0";
                    return result;
                }

                if (doseQty <= 0)
                {
                    result = false;
                    errMessage = "Dose should be greater than 0";
                    return result;
                }

                if (dosingDuration <= 0)
                {
                    result = false;
                    errMessage = "Medication Duration should be greater than 0";
                    return result;
                }
            }
            else
            {
                result = false;
                errMessage = "Medication Duration, Dispense Quantity and Dose should be greater than 0";
                return result;
            }

            bool isAllergyAlert = false;
            bool isAllergyTreatAsError = true;
            bool isAdverseReactionAlert = false;
            bool isAdverseReactionAsError = true;
            bool isDuplicateTheraphyAlert = false;
            bool isDuplicateTheraphyAsError = true;

            bool isValidationResult = CheckForItemAlert(entityItem, ref errMessage, ref isAllergyAlert, ref isAllergyTreatAsError, ref isAdverseReactionAlert, ref isAdverseReactionAsError, ref isDuplicateTheraphyAlert, ref isDuplicateTheraphyAsError);
            bool isItemValid = true;

            if (!isValidationResult)
            {
                DisplayMessageToUser(errMessage);
                isItemValid = IsValidToProceed(isAllergyAlert, isAllergyTreatAsError, isAdverseReactionAlert, isAdverseReactionAsError, isDuplicateTheraphyAlert, isDuplicateTheraphyAsError);
            }
            else
            {
                isItemValid = isValidationResult;
            }

            if (isItemValid)
            {
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
                PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
                //RegistrationDao registratioDao = new RegistrationDao(ctx);
                //ConsultVisitDao visitDao = new ConsultVisitDao(ctx);
                try
                {
                    //ConsultVisit visit = visitDao.Get(AppSession.RegisteredPatient.VisitID);
                    //if (visit != null)
                    //{
                    //Registration reg = registratioDao.Get(visit.RegistrationID);
                    //if (reg.MRN == AppSession.RegisteredPatient.MRN)
                    //{
                    PrescriptionOrderHd entityHd = null;
                    if (hdnPrescriptionOrderID.Value == "")
                    {
                        entityHd = new PrescriptionOrderHd();
                        entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                        entityHd.VisitID = AppSession.RegisteredPatient.VisitID;

                        //entityHd.PrescriptionDate = Helper.GetDatePickerValue(txtPrescriptionDate);
                        //entityHd.PrescriptionTime = txtPrescriptionTime.Text;

                        entityHd.PrescriptionDate = DateTime.Now;
                        Int32 hour = DateTime.Now.Hour;
                        Int32 minute = DateTime.Now.Minute;
                        string hourInString = "";
                        string minuteInString = "";
                        if (hour < 10)
                        {
                            hourInString = string.Format("0{0}", hour);
                        }
                        else
                        {
                            hourInString = string.Format("{0}", hour);
                        }

                        if (minute < 10)
                        {
                            minuteInString = string.Format("0{0}", minute);
                        }
                        else
                        {
                            minuteInString = string.Format("{0}", minute);
                        }
                        entityHd.PrescriptionTime = string.Format("{0}:{1}", hourInString, minuteInString);

                        entityHd.ClassID = AppSession.RegisteredPatient.ClassID;
                        entityHd.DispensaryServiceUnitID = Convert.ToInt32(cboDispensaryUnit.Value);
                        entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                        entityHd.LocationID = Convert.ToInt32(cboLocation.Value);
                        entityHd.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                        entityHd.GCRefillInstruction = cboRefillInstruction.Value.ToString();
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entityHd.GCOrderStatus = Constant.TestOrderStatus.OPEN;
                        switch (AppSession.RegisteredPatient.DepartmentID)
                        {
                            case Constant.Facility.EMERGENCY:
                                entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                                break;
                            case Constant.Facility.OUTPATIENT:
                                entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                                break;
                            case Constant.Facility.INPATIENT:
                                entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                                break;
                            case Constant.Facility.DIAGNOSTIC:
                                entityHd.TransactionCode = Constant.TransactionCode.OTHER_MEDICATION_ORDER;
                                break;
                            default:
                                entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                                break;
                        }
                        entityHd.Remarks = txtRemarks.Text;
                        entityHd.IsCreatedBySystem = false;
                        entityHd.IsOrderedByPhysician = true;
                        entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        entityHd.PrescriptionOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                    }
                    else
                    {
                        entityHd = entityHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                        entityHd.Remarks = txtRemarks.Text;
                        entityHdDao.Update(entityHd);
                    }
                    retval = entityHd.PrescriptionOrderID.ToString();

                    PrescriptionOrderDt entityDt = new PrescriptionOrderDt();
                    ControlToEntity(entityDt);
                    if (entityDt.GenericName == "")
                        entityDt.GenericName = entityItem.GenericName;
                    entityDt.PrescriptionOrderID = entityHd.PrescriptionOrderID;
                    entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;
                    entityDt.IsCreatedFromOrder = true;
                    entityDt.IsCompound = false;
                    entityDt.IsAllergyAlert = isAllergyAlert;
                    entityDt.IsAdverseReactionAlert = isAdverseReactionAlert;
                    entityDt.IsDuplicateTheraphyAlert = isDuplicateTheraphyAlert;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                    //}
                    //else
                    //{
                    //    result = false;
                    //    errMessage = string.Format("Maaf ada perbedaan data antara session. Harap Refresh Halaman Ini.");
                    //    Exception ex = new Exception(errMessage);
                    //    Helper.InsertErrorLog(ex);
                    //    ctx.RollBackTransaction();
                    //}
                    //}
                    //else
                    //{
                    //    result = false;
                    //    errMessage = string.Format("Maaf ada perbedaan data antara session. Harap Refresh Halaman Ini.");
                    //    Exception ex = new Exception(errMessage);
                    //    Helper.InsertErrorLog(ex);
                    //    ctx.RollBackTransaction();
                    //}
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
            else
            {
                result = false;
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            errMessage = string.Empty;
            vDrugInfo entityItem = null;
            entityItem = BusinessLayer.GetvDrugInfoList(string.Format("ItemID = {0}", hdnDrugID.Value))[0];
            bool result = true;

            decimal dispenseQty = 0;
            bool isDecimal = decimal.TryParse(txtDispenseQty.Text, out dispenseQty);

            if (dispenseQty <= 0)
            {
                result = false;
                errMessage = "Dispense Quantity should be greater than 0";
                return result;
            }

            bool isAllergyAlert = false;
            bool isAllergyTreatAsError = true;
            bool isAdverseReactionAlert = false;
            bool isAdverseReactionAsError = true;
            bool isDuplicateTheraphyAlert = false;
            bool isDuplicateTheraphyAsError = true;

            bool isValidationResult = CheckForItemAlert(entityItem, ref errMessage, ref isAllergyAlert, ref isAllergyTreatAsError, ref isAdverseReactionAlert, ref isAdverseReactionAsError, ref isDuplicateTheraphyAlert, ref isDuplicateTheraphyAsError);
            bool isItemValid = true;

            if (!isValidationResult)
            {
                DisplayMessageToUser(errMessage);
                isItemValid = IsValidToProceed(isAllergyAlert, isAllergyTreatAsError, isAdverseReactionAlert, isAdverseReactionAsError, isDuplicateTheraphyAlert, isDuplicateTheraphyAsError);
            }
            else
            {
                isItemValid = isValidationResult;
            }

            if (isItemValid)
            {
                try
                {
                    if (!String.IsNullOrEmpty(txtRemarks.Text))
                    {
                        PrescriptionOrderHd oHeader = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                        if (oHeader != null)
                        {
                            oHeader.Remarks = txtRemarks.Text;
                            BusinessLayer.UpdatePrescriptionOrderHd(oHeader);
                        }
                    }
                    PrescriptionOrderDt entity = BusinessLayer.GetPrescriptionOrderDt(Convert.ToInt32(hdnEntryID.Value));
                    ControlToEntity(entity);
                    if (entity.GenericName == "")
                        entity.GenericName = entityItem.GenericName;
                    entity.IsAllergyAlert = isAllergyAlert;
                    entity.IsAdverseReactionAlert = isAdverseReactionAlert;
                    entity.IsDuplicateTheraphyAlert = isDuplicateTheraphyAlert;
                    if (!entity.IsAllergyAlert && !entity.IsAdverseReactionAlert && !entity.IsDuplicateTheraphyAlert)
                    {
                        entity.IsAlertConfirmed = false;
                        entity.AlertConfirmedRemarks = null;
                        entity.AlertConfirmedBy = null;
                    }
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePrescriptionOrderDt(entity);
                    retval = hdnPrescriptionOrderID.Value;
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                }
            }
            return result;
        }

        protected override bool OnSaveQuickEntryRecord(string quickEntryText, ref string errMessage, ref string retval)
        {
            bool result = true;

            string[] param = quickEntryText.Split(';');

            if (param.Length < 5)
            {
                result = false;
                errMessage = "Quick Entry Format is not complete yet, please check your entry.";
                return result;
            }

            int itemID = 0;
            bool isNum = Int32.TryParse(param[0], out itemID);
            decimal dispenseQty = 0;
            bool isDispenseQtyValid = decimal.TryParse(param[3], out dispenseQty);

            if (!isNum)
            {
                result = false;
                errMessage = "You have not select any item yet";
                return result;
            }

            if (!isDispenseQtyValid)
            {
                result = false;
                errMessage = "Dispense Quantity should be greater than 0";
                return result;
            }
            else
            {
                if (dispenseQty <= 0)
                {
                    result = false;
                    errMessage = "Dispense Quantity should be greater than 0";
                    return result;
                }
            }

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            //RegistrationDao registratioDao = new RegistrationDao(ctx);
            //ConsultVisitDao visitDao = new ConsultVisitDao(ctx);
            try
            {

                vDrugInfo entityItem = BusinessLayer.GetvDrugInfoList(string.Format("ItemID = {0}", itemID), ctx)[0];

                string dosageInText = param[2].Replace(',', '.');
                decimal numberOfDosage = 0;
                string validationMsg = "Invalid dosing quantity or dosing quantity should be greater than 0";
                if (!string.IsNullOrEmpty(dosageInText))
                {
                    if (dosageInText.Contains('/'))
                    {
                        string[] dosageInTextInfo = dosageInText.Split('/');
                        decimal num1 = 0;
                        decimal num2 = 0;

                        bool isNumeric1 = decimal.TryParse(dosageInTextInfo[0], out num1);
                        bool isNumeric2 = decimal.TryParse(dosageInTextInfo[1], out num1);

                        if (!isNumeric1 || num1 == 0)
                        {
                            errMessage = validationMsg;
                            return false;
                        }
                        if (!isNumeric2 || num2 == 0)
                        {
                            errMessage = validationMsg;
                            return false;
                        }

                        numberOfDosage = Math.Round(num1 / num2, 2);
                    }
                    else
                    {
                        bool isNumeric = decimal.TryParse(dosageInText, out numberOfDosage);
                        if (!isNumeric || numberOfDosage == 0)
                        {
                            errMessage = validationMsg;
                            return false;
                        }
                    }
                }
                else
                {
                    errMessage = validationMsg;
                    return false;
                }

                decimal dosingDuration = CalculateDosingDuration(param[1], numberOfDosage.ToString(), param[3]);

                bool isAllergyAlert = false;
                bool isAllergyTreatAsError = true;
                bool isAdverseReactionAlert = false;
                bool isAdverseReactionAsError = true;
                bool isDuplicateTheraphyAlert = false;
                bool isDuplicateTheraphyAsError = true;

                bool isValidationResult = CheckForItemAlert(entityItem, ref errMessage, ref isAllergyAlert, ref isAllergyTreatAsError, ref isAdverseReactionAlert, ref isAdverseReactionAsError, ref isDuplicateTheraphyAlert, ref isDuplicateTheraphyAsError, dosingDuration);
                bool isItemValid = true;

                if (!isValidationResult)
                {
                    DisplayMessageToUser(errMessage);
                    isItemValid = IsValidToProceed(isAllergyAlert, isAllergyTreatAsError, isAdverseReactionAlert, isAdverseReactionAsError, isDuplicateTheraphyAlert, isDuplicateTheraphyAsError);
                }
                else
                {
                    isItemValid = isValidationResult;
                }

                if (isItemValid)
                {
                    //ConsultVisit visit = visitDao.Get(AppSession.RegisteredPatient.VisitID);
                    //if (visit != null)
                    //{
                    //Registration reg = registratioDao.Get(visit.RegistrationID);
                    //if (reg.MRN == AppSession.RegisteredPatient.MRN)
                    //{
                    PrescriptionOrderHd entityHd = null;
                    if (hdnPrescriptionOrderID.Value == "")
                    {
                        entityHd = new PrescriptionOrderHd();
                        entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                        entityHd.VisitID = AppSession.RegisteredPatient.VisitID;

                        //entityHd.PrescriptionDate = Helper.GetDatePickerValue(txtPrescriptionDate);
                        //entityHd.PrescriptionTime = txtPrescriptionTime.Text;

                        entityHd.PrescriptionDate = DateTime.Now;
                        Int32 hour = DateTime.Now.Hour;
                        Int32 minute = DateTime.Now.Minute;
                        string hourInString = "";
                        string minuteInString = "";
                        if (hour < 10)
                        {
                            hourInString = string.Format("0{0}", hour);
                        }
                        else
                        {
                            hourInString = string.Format("{0}", hour);
                        }

                        if (minute < 10)
                        {
                            minuteInString = string.Format("0{0}", minute);
                        }
                        else
                        {
                            minuteInString = string.Format("{0}", minute);
                        }
                        entityHd.PrescriptionTime = string.Format("{0}:{1}", hourInString, minuteInString);

                        entityHd.DispensaryServiceUnitID = Convert.ToInt32(cboDispensaryUnit.Value);
                        entityHd.LocationID = Convert.ToInt32(cboLocation.Value);
                        entityHd.ClassID = AppSession.RegisteredPatient.ClassID;
                        entityHd.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                        entityHd.GCRefillInstruction = cboRefillInstruction.Value.ToString();
                        entityHd.GCOrderStatus = Constant.TestOrderStatus.OPEN;
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        switch (AppSession.RegisteredPatient.DepartmentID)
                        {
                            case Constant.Facility.EMERGENCY:
                                entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                                break;
                            case Constant.Facility.OUTPATIENT:
                                entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                                break;
                            case Constant.Facility.INPATIENT:
                                entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                                break;
                            case Constant.Facility.DIAGNOSTIC:
                                entityHd.TransactionCode = Constant.TransactionCode.OTHER_MEDICATION_ORDER;
                                break;
                            default:
                                entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                                break;
                        }
                        entityHd.IsCreatedBySystem = false;
                        entityHd.IsOrderedByPhysician = true;
                        entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        entityHd.PrescriptionOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                    }
                    else
                    {
                        entityHd = entityHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                    }
                    retval = entityHd.PrescriptionOrderID.ToString();

                    PrescriptionOrderDt entityDt = new PrescriptionOrderDt();

                    entityDt.PrescriptionOrderID = entityHd.PrescriptionOrderID;
                    entityDt.IsRFlag = true;
                    entityDt.IsCompound = false;
                    entityDt.ItemID = itemID;
                    entityDt.DrugName = entityItem.ItemName1;
                    entityDt.GenericName = entityItem.GenericName;
                    entityDt.GCDrugForm = entityItem.GCDrugForm;
                    entityDt.Dose = entityItem.Dose;
                    entityDt.GCDoseUnit = entityItem.GCDoseUnit;

                    entityDt.StartDate = DateTime.Now.Date;
                    string[] medicationTime = Methods.GetMedicationSequenceTime(entityDt.Frequency).Split('|');
                    entityDt.Sequence1Time = medicationTime[0];
                    entityDt.Sequence2Time = medicationTime[1];
                    entityDt.Sequence3Time = medicationTime[2];
                    entityDt.Sequence4Time = medicationTime[3];
                    entityDt.Sequence5Time = medicationTime[4];
                    entityDt.Sequence6Time = medicationTime[5];
                    if (medicationTime[0] != "-")
                        entityDt.StartTime = medicationTime[0];
                    else
                        entityDt.StartTime = "00:00";

                    bool isDay = true;
                    bool isPrn = false;
                    bool isIMM = false;
                    if (param[1].ToLower().Contains("prn"))
                    {
                        isPrn = true;
                        entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                        entityDt.Frequency = 1;
                        entityDt.IsMorning = true;
                    }
                    else
                    {
                        if (!param[1].ToLower().Contains("qh") && !param[1].ToLower().Contains("dd"))
                        {
                            if (hdnIsAutoMedicationFrequency.Value == "1")
                            {
                                switch (param[1])
                                {
                                    case Constant.PrescriptionFrequency.OD:
                                        entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                        entityDt.Frequency = 1;
                                        entityDt.IsMorning = true;
                                        break;
                                    case Constant.PrescriptionFrequency.QD:
                                        entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                        entityDt.Frequency = 1;
                                        entityDt.IsMorning = true;
                                        break;
                                    case Constant.PrescriptionFrequency.BID:
                                        entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                        entityDt.Frequency = 2;
                                        entityDt.IsMorning = true;
                                        entityDt.IsNoon = true;
                                        break;
                                    case Constant.PrescriptionFrequency.TID:
                                        entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                        entityDt.Frequency = 3;
                                        entityDt.IsMorning = true;
                                        entityDt.IsNoon = true;
                                        entityDt.IsNight = true;
                                        break;
                                    case Constant.PrescriptionFrequency.QID:
                                        entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                        entityDt.Frequency = 4;
                                        entityDt.IsMorning = true;
                                        entityDt.IsNoon = true;
                                        entityDt.IsEvening = true;
                                        entityDt.IsNight = true;
                                        break;
                                    default:
                                        entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                        entityDt.Frequency = 1;
                                        entityDt.IsMorning = true;
                                        break;
                                }
                            }
                            else
                            {
                                switch (param[1])
                                {
                                    case Constant.PrescriptionFrequency.OD:
                                        entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                        entityDt.Frequency = 1;
                                        break;
                                    case Constant.PrescriptionFrequency.QD:
                                        entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                        entityDt.Frequency = 1;
                                        break;
                                    case Constant.PrescriptionFrequency.BID:
                                        entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                        entityDt.Frequency = 2;
                                        break;
                                    case Constant.PrescriptionFrequency.TID:
                                        entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                        entityDt.Frequency = 3;
                                        break;
                                    case Constant.PrescriptionFrequency.QID:
                                        entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                        entityDt.Frequency = 4;
                                        break;
                                    default:
                                        entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                                        entityDt.Frequency = 1;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            #region Default Frequency
                            string frequency = "1";
                            entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                            #endregion

                            if (param[1].ToLower().Contains("qh"))
                            {
                                frequency = param[1].Substring(2);
                                isDay = false;
                                entityDt.GCDosingFrequency = Constant.DosingFrequency.HOUR;
                            }
                            else
                            {
                                frequency = param[1].Substring(0, 1);
                                entityDt.GCDosingFrequency = Constant.DosingFrequency.DAY;
                            }
                            entityDt.Frequency = Convert.ToInt16(frequency);

                            if (hdnIsAutoMedicationFrequency.Value == "1")
                            {
                                switch (entityDt.Frequency)
                                {
                                    case 1:
                                        entityDt.IsMorning = true;
                                        break;
                                    case 2:
                                        entityDt.IsMorning = true;
                                        entityDt.IsNoon = true;
                                        break;
                                    case 3:
                                        entityDt.IsMorning = true;
                                        entityDt.IsNoon = true;
                                        entityDt.IsNight = true;
                                        break;
                                    case 4:
                                        entityDt.IsMorning = true;
                                        entityDt.IsNoon = true;
                                        entityDt.IsEvening = true;
                                        entityDt.IsNight = true;
                                        break;
                                    default:
                                        entityDt.IsMorning = true;
                                        entityDt.IsNoon = true;
                                        entityDt.IsEvening = true;
                                        entityDt.IsNight = true;
                                        break;
                                }
                            }
                        }
                    }

                    entityDt.NumberOfDosage = numberOfDosage;
                    entityDt.NumberOfDosageInString = param[2];
                    entityDt.GCDosingUnit = param[4];
                    if (!string.IsNullOrEmpty(entityItem.GCMedicationRoute))
                        entityDt.GCRoute = entityItem.GCMedicationRoute;
                    else
                        entityDt.GCRoute = Constant.MedicationRoute.OTHER;

                    entityDt.DispenseQty = Convert.ToDecimal(param[3]);
                    entityDt.IsCreatedFromOrder = true;

                    if (!entityItem.IsExternalMedication)
                    {
                        if (!isPrn)
                        {
                            if (isDay)
                            {
                                entityDt.DosingDuration = Math.Ceiling(entityDt.DispenseQty / (entityDt.Frequency * entityDt.NumberOfDosage));
                                //entityDt.DispenseQty = (decimal)(entityDt.Frequency * entityDt.NumberOfDosage * entityDt.DosingDuration);
                            }
                            else
                            {
                                decimal numberOfTaken = Math.Ceiling(Convert.ToDecimal(24 / entityDt.Frequency));
                                entityDt.DosingDuration = Math.Ceiling(entityDt.DispenseQty / (numberOfTaken * entityDt.NumberOfDosage));
                                //entityDt.DispenseQty = (decimal)(numberOfTaken * entityDt.NumberOfDosage * entityDt.DosingDuration);
                            }
                        }
                        else
                        {
                            entityDt.DosingDuration = 1;
                        }
                    }
                    else
                    {
                        entityDt.DosingDuration = 1;
                    }

                    entityDt.IsAsRequired = isPrn;
                    entityDt.IsIMM = isIMM;
                    entityDt.TakenQty = entityDt.DispenseQty;
                    entityDt.ResultQty = entityDt.DispenseQty;
                    entityDt.ChargeQty = entityDt.DispenseQty;
                    if (param.Length == 6)
                    {
                        entityDt.MedicationAdministration = param[5];
                    }

                    entityDt.IsAllergyAlert = isAllergyAlert;
                    entityDt.IsAdverseReactionAlert = isAdverseReactionAlert;
                    entityDt.IsDuplicateTheraphyAlert = isDuplicateTheraphyAlert;

                    entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;

                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                    //}
                    //else
                    //{
                    //    result = false;
                    //    errMessage = string.Format("Maaf ada perbedaan data antara session. Harap Refresh Halaman Ini.");
                    //    Exception ex = new Exception(errMessage);
                    //    Helper.InsertErrorLog(ex);
                    //    ctx.RollBackTransaction();
                    //}
                    //}
                    //else
                    //{
                    //    result = false;
                    //    errMessage = string.Format("Maaf ada perbedaan data antara session. Harap Refresh Halaman Ini.");
                    //    Exception ex = new Exception(errMessage);
                    //    Helper.InsertErrorLog(ex);
                    //    ctx.RollBackTransaction();
                    //}
                }
                else
                {
                    result = false;
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

        private void DisplayMessageToUser(string errMessage)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("alert('");
            sb.Append(errMessage);
            sb.Append("')};");
            sb.Append("</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        }

        private bool IsValidToProceed(bool isAllergyAlert, bool isAllergyTreatAsError, bool isAdverseReactionAlert, bool isAdverseReactionAsError, bool isDuplicateTheraphyAlert, bool isDuplicateTheraphyAsError)
        {
            bool isAllergyPassed = false;
            if (isAllergyAlert)
                isAllergyPassed = !isAllergyTreatAsError;
            else
                isAllergyPassed = true;

            bool isAdverseReactionPassed = false;
            if (isAdverseReactionAlert)
                isAdverseReactionPassed = !isAdverseReactionAsError;
            else
                isAdverseReactionPassed = true;

            bool isDuplicateTheraphyPassed = false;
            if (isDuplicateTheraphyAlert)
                isDuplicateTheraphyPassed = !isDuplicateTheraphyAsError;
            else
                isDuplicateTheraphyPassed = true;

            return (isAllergyPassed && isAdverseReactionPassed && isDuplicateTheraphyPassed);

        }

        private decimal CalculateDosingDuration(string frequency, string dosage, string dispenseQty)
        {
            bool isDay = true;
            bool isPrn = false;
            int dosingFrequency = 1;
            if (frequency.ToLower().Contains("prn"))
            {
                isPrn = true;
                dosingFrequency = 1;
            }
            else
            {
                if (!frequency.ToLower().Contains("qh") && !frequency.ToLower().Contains("dd"))
                {
                    switch (frequency)
                    {
                        case Constant.PrescriptionFrequency.QD:
                            dosingFrequency = 1;
                            break;
                        case Constant.PrescriptionFrequency.BID:
                            dosingFrequency = 2;
                            break;
                        case Constant.PrescriptionFrequency.TID:
                            dosingFrequency = 3;
                            break;
                        case Constant.PrescriptionFrequency.QID:
                            dosingFrequency = 4;
                            break;
                        default:
                            dosingFrequency = 1;
                            break;
                    }
                }
                else
                {
                    if (frequency.ToLower().Contains("qh"))
                    {
                        dosingFrequency = Convert.ToInt16(frequency.Substring(2));
                        isDay = false;
                    }
                    else
                    {
                        dosingFrequency = Convert.ToInt16(frequency.Substring(0, 1));
                    }
                }
            }

            decimal dispense = Convert.ToDecimal(dispenseQty);

            if (!isPrn)
            {
                if (isDay)
                {
                    return Math.Ceiling(dispense / (dosingFrequency * Convert.ToDecimal(dosage)));
                }
                else
                {
                    decimal numberOfTaken = Math.Ceiling(Convert.ToDecimal(24 / dosingFrequency));
                    return Math.Ceiling(dispense / (numberOfTaken * Convert.ToDecimal(dosage)));
                }
            }
            else
            {
                return 1;
            }
        }

        private bool CheckForItemAlert(vDrugInfo entityItem, ref string errMessage, ref Boolean isAllergyAlert, ref Boolean isAllergyAsError, ref Boolean isAdverseReactionAlert, ref Boolean isAdverseReactionAsError, ref Boolean isDuplicateTherapyAlert, ref Boolean isDuplicateTherapyAsError, decimal dosingDuration = 1)
        {
            #region Check For Allergy
            string filterExpression = string.Format("MRN = {0} AND GCAllergenType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, Constant.AllergenType.DRUG);
            string allergenName = string.Empty;

            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}')", Constant.SettingParameter.FM_KONTROL_ADVERSE_REACTION, Constant.SettingParameter.FM_MAKSIMUM_DURASI_NARKOTIKA, Constant.SettingParameter.FM_KONTROL_DUPLIKASI_TERAPI, Constant.SettingParameter.FM_KONTROL_ERROR_ALERGI));
            SettingParameter setParMaxDurasiNarkotika = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_MAKSIMUM_DURASI_NARKOTIKA).FirstOrDefault();

            bool isControlAdverseReaction = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_KONTROL_ADVERSE_REACTION).FirstOrDefault().ParameterValue == "0" ? false : true;
            bool isControlTheraphyDuplication = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_KONTROL_DUPLIKASI_TERAPI).FirstOrDefault().ParameterValue == "0" ? false : true;
            bool isAllergyTreatAsError = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_KONTROL_ERROR_ALERGI).FirstOrDefault().ParameterValue == "1" ? true : false;

            List<PatientAllergy> lstPatientAllergy = null;
            if (entityItem.GenericName != string.Empty)
                filterExpression += string.Format(" AND (Allergen LIKE '%{0}%' OR Allergen LIKE '%{1}%')", entityItem.GenericName, entityItem.ItemName1);
            else
                filterExpression += string.Format(" AND Allergen LIKE '%{0}%'", entityItem.ItemName1);

            lstPatientAllergy = BusinessLayer.GetPatientAllergyList(filterExpression);

            if (lstPatientAllergy.Count > 0)
            {
                errMessage = string.Format("Pasien memiliki alergi {0} ({1})", entityItem.GenericName, entityItem.ItemName1).Replace("()", "");
                isAllergyAlert = true;
                isAllergyAsError = isAllergyTreatAsError;
                return false;
            }
            else
            {
                filterExpression = string.Format("ItemID = {0}", entityItem.ItemID);
                List<DrugContent> contents = BusinessLayer.GetDrugContentList(filterExpression);
                foreach (DrugContent item in contents)
                {
                    if (item.Keyword != null && item.Keyword != "")
                    {
                        filterExpression = string.Format("MRN = {0} AND GCAllergenType = '{1}' AND Allergen LIKE '%{2}%' AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, Constant.AllergenType.DRUG, item.Keyword);
                        lstPatientAllergy = BusinessLayer.GetPatientAllergyList(filterExpression);
                        if (lstPatientAllergy.Count > 0)
                        {
                            errMessage = string.Format("Pasien memiliki alergi {0} ({1})", item.ContentText, entityItem.ItemName1).Replace("()", "");
                            isAllergyAlert = true;
                            isAllergyAsError = isAllergyTreatAsError;
                            return false;
                        }
                    }
                }
            }
            #endregion

            string prescriptionOrderId = hdnPrescriptionOrderID.Value;

            if (isControlTheraphyDuplication)
            {
                #region Duplicate Theraphy
                if (prescriptionOrderId != "0" && prescriptionOrderId != "")
                {
                    string filterExp = string.Format("PrescriptionOrderID = {0}", prescriptionOrderId);
                    List<vPrescriptionOrderDt1> itemlist = BusinessLayer.GetvPrescriptionOrderDt1List(filterExp);
                    foreach (var item in itemlist)
                    {
                        //Generic Name
                        if ((item.ItemID != entityItem.ItemID) && (item.GenericName.ToLower().TrimEnd() == entityItem.GenericName.ToLower().TrimEnd()) && !item.GenericName.Equals(string.Empty))
                        {
                            errMessage = string.Format("Duplikasi obat dengan nama generik {0} yang sama ({1})", item.GenericName.TrimEnd(), item.DrugName.TrimEnd());
                            isDuplicateTherapyAlert = true;
                            isDuplicateTherapyAsError = true;
                            return false;
                        }
                        vDrugInfo drugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemCode = '{0}'", item.ItemCode)).FirstOrDefault();
                        if (drugInfo != null)
                        {
                            //ATC Class
                            if ((item.ItemID != entityItem.ItemID) && ((drugInfo.ATCClassCode == entityItem.ATCClassCode) && (!String.IsNullOrEmpty(entityItem.ATCClassCode))))
                            {
                                errMessage = string.Format("Duplikasi obat dengan Kelompok/Kelas ATC {0} yang sama ({1})", drugInfo.ATCClassName.TrimEnd(), item.DrugName.TrimEnd());
                                isDuplicateTherapyAlert = true;
                                isDuplicateTherapyAsError = true;
                                return false;
                            }
                            //Kelompok Theraphy
                            if ((item.ItemID != entityItem.ItemID) && (drugInfo.MIMSClassCode.ToLower().TrimEnd() == entityItem.MIMSClassCode.ToLower().TrimEnd()) && (!String.IsNullOrEmpty(entityItem.MIMSClassCode)))
                            {
                                errMessage = string.Format("Duplikasi obat dengan Kelompok Terapi {0} yang sama ({1})", drugInfo.MIMSClassName.TrimEnd(), item.DrugName.TrimEnd());
                                isDuplicateTherapyAlert = true;
                                isDuplicateTherapyAsError = true;
                                return false;
                            }
                        }
                    }
                }
                #endregion
            }

            #region psikotropika & narkotika
            if (entityItem.GCDrugClass == Constant.DrugClass.MORPHIN || entityItem.GCDrugClass == Constant.DrugClass.NARKOTIKA || entityItem.GCDrugClass == Constant.DrugClass.PSIKOTROPIKA)
            {
                int duration = 0;
                if (dosingDuration > 0)
                    duration = Convert.ToInt32(dosingDuration);
                else
                    duration = Convert.ToInt32(txtDosingDuration.Text);
                if (duration > Convert.ToInt32(setParMaxDurasiNarkotika.ParameterValue))
                {
                    errMessage = string.Format("Obat {0} Mengandung Narkotika, pemakaian tidak boleh lebih dari {1} hari", entityItem.ItemName1.TrimEnd(), setParMaxDurasiNarkotika.ParameterValue);
                    return false;
                }
            }
            #endregion

            if (!isControlAdverseReaction)
            {
                isAdverseReactionAsError = false;
            }

            #region Adverse Reaction
            prescriptionOrderId = hdnPrescriptionOrderID.Value;
            if (prescriptionOrderId != "0")
            {
                filterExpression = string.Format("ItemID = {0}", entityItem.ItemID);
                List<DrugReaction> reactions = BusinessLayer.GetDrugReactionList(filterExpression);
                foreach (DrugReaction advReaction in reactions)
                {
                    if (prescriptionOrderId != "" && prescriptionOrderId != null)
                    {
                        string filterExp = string.Format("PrescriptionOrderID = {0}", prescriptionOrderId);
                        List<vPrescriptionOrderDt1> itemlist = BusinessLayer.GetvPrescriptionOrderDt1List(filterExp);
                        foreach (var item in itemlist)
                        {
                            vDrugInfo drugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemCode = '{0}'", item.ItemCode)).FirstOrDefault();
                            if (drugInfo != null)
                            {
                                //Generic Name
                                if (drugInfo.GenericName.ToLower().TrimEnd().Contains(advReaction.AdverseReactionText1.ToLower().TrimEnd())
                                    || advReaction.AdverseReactionText1.ToLower().TrimEnd().Contains(drugInfo.GenericName.ToLower().TrimEnd()))
                                {
                                    errMessage = string.Format("Terjadi interaksi obat dengan {0} ({1}) \n Catatan Interaksi Obat: \n {2}", item.DrugName.TrimEnd(), drugInfo.GenericName, advReaction.AdverseReactionText2);
                                    isAdverseReactionAlert = true;
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            #endregion


            return (errMessage == string.Empty);
        }

        protected void cbpSendOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int transactionID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityOrderDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHdOriginalDao entityOrderHdOriginalDao = new PrescriptionOrderHdOriginalDao(ctx);
            PrescriptionOrderDtOriginalDao entityOrderDtOriginalDao = new PrescriptionOrderDtOriginalDao(ctx);

            try
            {
                if (param[0] == "sendOrder")
                {
                    #region SendOrder

                    PrescriptionOrderHd entity = entityOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));

                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        //string filterExpression = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0 AND (IsAllergyAlert = 1 OR IsAdverseReactionAlert = 1 OR IsDuplicateTheraphyAlert = 1) AND IsAlertConfirmed = 0", hdnPrescriptionOrderID.Value);
                        //List<PrescriptionOrderDt> lstEntity = BusinessLayer.GetPrescriptionOrderDtList(filterExpression, ctx);

                        string filterExpression = string.Format("PrescriptionOrderID = {0} AND OrderIsDeleted = 0 AND (IsAllergyAlert = 1 OR IsAdverseReactionAlert = 1 OR IsDuplicateTheraphyAlert = 1) AND IsAlertConfirmed = 0", hdnPrescriptionOrderID.Value);
                        List<vPrescriptionOrderDt1> lstEntityAPI = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression, ctx);

                        if (lstEntityAPI.Count > 0)
                        {
                            errMessage = "There is item(s) should be confirmed due to allergy, adverse reaction and duplicate theraphy.";
                            result += string.Format("confirm|{0}", errMessage);
                        }
                        else
                        {
                            bool isValidToProceed = true;
                            if (AppSession.EM0088 == "1")
                            {
                                filterExpression = string.Format("PrescriptionOrderID = {0} AND OrderIsDeleted = 0 AND IsRestrictiveAntibiotics = 1 AND (PPRAFormStatus NOT IN ('1','2') OR PPRAFormStatus IS NULL)", hdnPrescriptionOrderID.Value);
                                List<vPrescriptionOrderDt1> lstPPRAItem = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression, ctx);
                                if (lstPPRAItem.Count > 0)
                                {
                                    errMessage = "Terdapat item yang termasuk dalam kategori Program Pengendalian Resistensi Antimikroba (PPRA) yang perlu dilakukan pengisian Form PPRA";
                                    result += string.Format("confirmPPRA|{0}", errMessage);
                                    isValidToProceed = false;
                                }
                            }
                            if (isValidToProceed)
                            {
                                entity.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                                entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                entity.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                                entity.SendOrderDateTime = DateTime.Now;
                                entity.SendOrderBy = AppSession.UserLogin.UserID;
                                entity.ProposedBy = AppSession.UserLogin.UserID;
                                entity.ProposedDate = DateTime.Now;
                                entity.Remarks = txtRemarks.Text;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                                if (hdnIsUsingDrugAlert.Value == "1")
                                {
                                    entity.DrugAlertConfirmedBy = AppSession.UserLogin.UserID;
                                    entity.DrugAlertConfirmedDate = DateTime.Now;
                                }
                                entityOrderHdDao.Update(entity);

                                //Log : Copy of Current Prescription Order
                                #region Log Header
                                int historyID = 0;
                                if (entity.IsOrderedByPhysician)
                                {
                                    PrescriptionOrderHdOriginal originalHd = new PrescriptionOrderHdOriginal();
                                    CopyHeaderObject(entity, ref originalHd);
                                    historyID = entityOrderHdOriginalDao.InsertReturnPrimaryKeyID(originalHd);
                                }
                                #endregion

                                filterExpression = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", hdnPrescriptionOrderID.Value);
                                List<PrescriptionOrderDt> lstEntity = BusinessLayer.GetPrescriptionOrderDtList(filterExpression, ctx);

                                List<PrescriptionOrderDtOriginal> lstOriginalDt = new List<PrescriptionOrderDtOriginal>();

                                foreach (PrescriptionOrderDt item in lstEntity)
                                {
                                    PrescriptionOrderDt orderDt = entityOrderDtDao.Get(item.PrescriptionOrderDetailID);
                                    if (orderDt != null)
                                    {
                                        orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                                        orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityOrderDtDao.Update(orderDt);

                                        if (historyID > 0)
                                        {
                                            PrescriptionOrderDtOriginal originalDt = new PrescriptionOrderDtOriginal();
                                            CopyDetailObject(orderDt, ref originalDt);
                                            ///originalDt.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
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
                                Helper.InsertPrescriptionOrderTaskLog(ctx, transactionID, Constant.PrescriptionTaskLogStatus.Sent, AppSession.UserLogin.UserID, false);
                                #endregion

                                ctx.CommitTransaction();

                                result += string.Format("success|{0}", errMessage);
                            }
                        }

                        if (AppSession.SA0137 == "1")
                        {
                            if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                            {
                                BridgingToMedinfrasV1(1, entity, lstEntityAPI);
                            }
                        }

                        try
                        {
                            if (AppSession.IsPrintPHOrderTracerFromEMR)
                            {
                                PrintOrderTracer(entity.PrescriptionOrderID);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    #endregion
                }
                else if (param[0] == "saveHeader")
                {
                    #region saveHeader

                    PrescriptionOrderHd entity = entityOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.Remarks = txtRemarks.Text;
                        entity.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityOrderHdDao.Update(entity);

                        ctx.CommitTransaction();
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result += string.Format("fail|{0}", errMessage);
            }
            finally
            {
                ctx.Close();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID;
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
            string printerUrl1 = "";
            if (lstPrinter.Count > 0)
            {
                printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;
            }

            if (entityHSU.Initial == "RSPR") //RSPR
            {
                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    ZebraPrinting.PrintOrderFarmasiRSPR(prescriptionOrderID, printerUrl1);
                }
            }
            else if (entityHSU.Initial == "RSDI") //RSDI
            {
                SettingParameterDt oSetPar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode='{0}'", Constant.SettingParameter.PH0071)).FirstOrDefault();
                printerUrl1 = oSetPar.ParameterValue;
                ZebraPrinting.PrintOrderFarmasiRSDI(prescriptionOrderID, printerUrl1);
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
            else if (entityHSU.Initial == "RSRT")
            {
                List<SettingParameterDt> lstSeparDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID='{0}' AND ParameterCode IN('{1}','{2}')",
                   AppSession.UserLogin.HealthcareID,
                  Constant.SettingParameter.PH0071, //rajal
                  Constant.SettingParameter.PH0078 //ranap
                  ));

                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    SettingParameterDt oSetPar = lstSeparDt.Where(p => p.ParameterCode == Constant.SettingParameter.PH0078).FirstOrDefault();
                    if (oSetPar != null)
                    {
                        printerUrl1 = oSetPar.ParameterValue;
                        if (!string.IsNullOrEmpty(printerUrl1) && printerUrl1 != "-")  /// tanda (-) untuk kosongin supaya tidak baca
                        {
                            ZebraPrinting.PrintOrderFarmasiRSRT(prescriptionOrderID, printerUrl1);
                        }
                    }

                }
                else
                {

                    SettingParameterDt oSetPar = lstSeparDt.Where(p => p.ParameterCode == Constant.SettingParameter.PH0071).FirstOrDefault();
                    if (oSetPar != null)
                    {
                        printerUrl1 = oSetPar.ParameterValue;
                        if (!string.IsNullOrEmpty(printerUrl1))
                        {
                            ZebraPrinting.PrintOrderFarmasiRSRT(prescriptionOrderID, printerUrl1);
                        }

                    }
                }
            }
            else if (entityHSU.Initial == "PHS")
            {
                List<SettingParameterDt> lstOSetPar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.PH0071, Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN));

                SettingParameterDt oSetPar = lstOSetPar.Where(t => t.ParameterCode == Constant.SettingParameter.PH0071).FirstOrDefault();
                string[] printUrl = oSetPar.ParameterValue.Split('|');
                string bpjsID = lstOSetPar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault().ParameterValue;
                Customer bp = BusinessLayer.GetCustomer(AppSession.RegisteredPatient.BusinessPartnerID);
                printerUrl1 = printUrl[0];

                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    printerUrl1 = printUrl[2];
                }
                else
                {
                    if (bp.GCCustomerType == Constant.CustomerType.BPJS)
                    {
                        printerUrl1 = printUrl[1];
                    }
                }
                ZebraPrinting.PrintOrderFarmasiRSPM(prescriptionOrderID, printerUrl1);
            }
            else if (entityHSU.Initial == "BROS")
            {
                List<SettingParameterDt> lstOSetPar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.PH0071, Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN));

                SettingParameterDt oSetPar = lstOSetPar.Where(t => t.ParameterCode == Constant.SettingParameter.PH0071).FirstOrDefault();
                string[] printUrl = oSetPar.ParameterValue.Split('|');
                string bpjsID = lstOSetPar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault().ParameterValue;
                Customer bp = BusinessLayer.GetCustomer(AppSession.RegisteredPatient.BusinessPartnerID);
                printerUrl1 = printUrl[0];

                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
                {
                    printerUrl1 = printUrl[2];
                }
                else if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.EMERGENCY)
                {
                    printerUrl1 = printUrl[0];
                }
                else
                {
                    if (bp.GCCustomerType == Constant.CustomerType.BPJS)
                    {
                        printerUrl1 = printUrl[0];
                    }
                    else
                    {
                        printerUrl1 = printUrl[1];
                    }
                }
                ZebraPrinting.PrintOrderFarmasi(prescriptionOrderID, printerUrl1);
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
        #endregion

        private void BridgingToMedinfrasV1(int ProcessType, PrescriptionOrderHd entity, List<vPrescriptionOrderDt1> lstEntityAPI)
        {
            APIMessageLog apiLog = new APIMessageLog();
            apiLog.MessageDateTime = DateTime.Now;
            apiLog.Sender = Constant.BridgingVendor.HIS;
            apiLog.Recipient = Constant.BridgingVendor.MEDINFRAS_API;

            MedinfrasV1Service oService = new MedinfrasV1Service();
            string serviceResult = oService.OnSendOrderMedicalDiagnosticServices(ProcessType, null, entity, lstEntityAPI);
            string[] serviceResultInfo = serviceResult.Split('|');
            if (serviceResultInfo[0] == "1")
            {
                apiLog.IsSuccess = true;
                apiLog.MessageText = serviceResultInfo[1];
                apiLog.Response = serviceResultInfo[2];
            }
            else
            {
                apiLog.IsSuccess = false;
                apiLog.MessageText = serviceResultInfo[1];
                apiLog.Response = serviceResultInfo[2];
                apiLog.ErrorMessage = serviceResultInfo[2];
            }
            BusinessLayer.InsertAPIMessageLog(apiLog);
        }
    }
}