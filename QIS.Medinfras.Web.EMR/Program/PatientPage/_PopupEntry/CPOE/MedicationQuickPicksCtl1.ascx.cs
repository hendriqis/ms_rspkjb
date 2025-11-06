using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class MedicationQuickPicksCtl1 : BasePagePatientPageEntryCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        private string[] lstDosingUnitValue = null;
        private string[] lstDosingUnitText = null;

        public override void InitializeDataControl(string param)
        {
            IsAdd = true;
            hdnDateToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnParam.Value = param;
            string[] temp = param.Split('|');
            hdnTransactionID.Value = string.IsNullOrEmpty(temp[0]) ? "0" : temp[0];
            hdnLocationID.Value = temp[1];
            hdnDefaultGCMedicationRoute.Value = temp[2];
            hdnParamedicID.Value = temp[3];
            hdnRegistrationID.Value = temp[4];
            hdnVisitID.Value = temp[5];
            hdnChargeClassID.Value = temp[6];
            txtRemarks.Text = temp[7];

            //txtNotes.Text = temp[7];

            hdnDispensaryUnitID.Value = temp[8];
            hdnPrescriptionDate.Value = temp[9];
            hdnPrescriptionTime.Value = temp[10];
            hdnPrescriptionType.Value = temp[11];

            hdnUserLoginParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.EM_ORDER_RESEP_HANYA_BISA_PILIH_ITEM_STOK_RS, //1
                                                        Constant.SettingParameter.EM_IS_VALIDATION_EMPTY_STOCK_PRESCRIPTION_ORDER, //2
                                                        Constant.SettingParameter.EM_DEFAULT_DISPLAY_FILTER_ALL, //3
                                                        Constant.SettingParameter.EM_IS_ONLINE_PRESCRIPTION_ALLOW_PREVIEW_STOCK, //4
                                                        Constant.SettingParameter.EM0093, //5
                                                        Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS //6
                                                    );
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);

            hdnPrescriptionValidateStockAllRS.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM_ORDER_RESEP_HANYA_BISA_PILIH_ITEM_STOK_RS).ParameterValue;
            hdnValidationEmptyStockCtl.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM_IS_VALIDATION_EMPTY_STOCK_PRESCRIPTION_ORDER).ParameterValue;
            hdnDefaultDisplayFilterAll.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM_DEFAULT_DISPLAY_FILTER_ALL).ParameterValue;
            hdnIsAllowPreviewTariffQPCtl.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM_IS_ONLINE_PRESCRIPTION_ALLOW_PREVIEW_STOCK).ParameterValue;
            hdnIsLimitedCPOEItemForBPJS.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS).ParameterValue;

            hdnDefaultStartDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            hdnDepartmentIDCtl.Value = AppSession.RegisteredPatient.DepartmentID;

            hdnEM0093.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0093).ParameterValue;
            if (hdnEM0093.Value == "1")
            {
                trCalculate.Style.Remove("display");
                trCalculateSide.Style.Remove("display");
                thHarga.Style.Remove("display");
            }
            else
            {
                trCalculate.Style.Add("display", "none");
                trCalculateSide.Style.Add("display", "none");
                thHarga.Style.Add("display", "none");
            }

            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        public string GetItemUnitListCode()
        {
            return string.Join(",", lstDosingUnitValue);
        }
        public string GetItemUnitListText()
        {
            return string.Join(",", lstDosingUnitText);
        }

        public String IsAllowPreviewTariffQPCtl()
        {
            return hdnIsAllowPreviewTariffQPCtl.Value;
        }

        private void SetControlProperties()
        {
            bool isLimitedCPOEItemForBPJS = hdnIsLimitedCPOEItemForBPJS.Value != null ? (hdnIsLimitedCPOEItemForBPJS.Value == "1" ? true : false) : false;

            String filterExpression = string.Format("ParentID IN ('{0}')", Constant.StandardCode.ITEM_UNIT);
            lstDosingUnitValue = null;
            lstDosingUnitText = null;
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            List<StandardCode> lstItemUnit = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && (sc.TagProperty.Contains("PRE") || sc.TagProperty.Contains("1"))).ToList();
            lstDosingUnitValue = lstItemUnit.Select(lst => lst.StandardCodeID).ToArray();
            lstDosingUnitText = lstItemUnit.Select(lst => lst.StandardCodeName).ToArray();

            if (hdnDefaultDisplayFilterAll.Value == "1")
            {
                if (hdnIsLimitedCPOEItemForBPJS.Value == "1")
                {
                    if (AppSession.RegisteredPatient.GCCustomerType == AppSession.TipeCustomerBPJS)
                    {
                        rblItemType.SelectedValue = "2";
                        rblItemType.Enabled = !isLimitedCPOEItemForBPJS;
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

                if (AppSession.RegisteredPatient.GCCustomerType == AppSession.BusinessPartnerIDInhealth)
                {
                    rblItemType.SelectedValue = "3";
                    rblItemType.Enabled = !isLimitedCPOEItemForInhealth;
                }

                Customer entityCustomer = BusinessLayer.GetCustomer(AppSession.RegisteredPatient.BusinessPartnerID);
                if (AppSession.BusinessPartnerIDInhealth == entityCustomer.GCCustomerType)
                {
                    rblItemType.SelectedValue = "3";
                    rblItemType.Enabled = !isLimitedCPOEItemForInhealth;
                }
            }
            else
            {
                rblItemType.SelectedValue = "0";
            }

            if (AppSession.IsHasAllergy)
            {
                lblAllergy.Style.Add("color", "red");
                lblAllergy.Style.Add("font-weight", "bold");
            }
            else
            {
                lblAllergy.Style.Add("color", "black");
                lblAllergy.Style.Add("font-weight", "normal");
            }
            txtAllergyInfo.Text = AppSession.PatientAllergyInfo;

            BindCboLocation();
        }

        private void BindCboLocation()
        {
            if (Convert.ToInt32(hdnLocationID.Value) > 0)
            {
                Location loc = BusinessLayer.GetLocation(Convert.ToInt32(hdnLocationID.Value));
                List<Location> lstLocation = null;
                if (loc.IsHeader)
                    lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                else
                {
                    lstLocation = new List<Location>();
                    lstLocation.Add(loc);
                }
                Methods.SetComboBoxField<Location>(cboPopupLocation, lstLocation, "LocationName", "LocationID");
                cboPopupLocation.SelectedIndex = 0;
            }
        }

        protected void cboPopupLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
        }

        protected void cbpPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";

            if (hdnItemGroupDrugLogisticID.Value == "")
                filterExpression += string.Format("LocationID = '{0}' AND GCItemType IN ('{2}','{3}') AND IsDeleted = 0 AND (ItemName1 LIKE '%{1}%' OR GenericName LIKE '%{1}%') AND IsDeleted = 0", cboPopupLocation.Value, hdnFilterItem.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);
            else
                filterExpression += string.Format("LocationID = '{0}' AND GCItemType IN ('{3}','{4}') AND IsDeleted = 0 AND (ItemName1 LIKE '%{1}%' OR GenericName LIKE '%{1}%') AND ItemGroupID IN(SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{2}/%') AND IsDeleted = 0", cboPopupLocation.Value, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);

            if (rblItemSource.SelectedValue == "2")
            {
                // From History
                filterExpression += string.Format(" AND ItemID IN (SELECT ItemID FROM vPrescriptionOrderDt5 WHERE MRN = {0} AND IsRFlag = 1 AND IsCompound = 0)", AppSession.RegisteredPatient.MRN);
            }

            switch (rblItemType.SelectedValue)
            {
                case "1":
                    filterExpression += " AND IsFormularium = 1 ";
                    break;
                case "2":
                    filterExpression += " AND IsBPJSFormularium = 1 ";
                    break;
                case "3":
                    filterExpression += " AND IsInhealthFormularium = 1 ";
                    break;
                case "4":
                    filterExpression += " AND IsEmployeeFormularium = 1 ";
                    break;
                default:
                    break;
            }

            filterExpression += string.Format(" AND GCItemStatus = '{0}'", Constant.ItemStatus.ACTIVE);

            if (hdnPrescriptionValidateStockAllRS.Value == "1")
            {
                filterExpression += " AND TotalQtyOnHand > 0"; // nama field'nya TotalQtyOnHand tapi di query itu adalah qtyAllRS
            }

            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemBalanceQuickPick3 entity = e.Row.DataItem as vItemBalanceQuickPick3;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                Label txtItemUnit = (Label)e.Row.FindControl("txtItemUnit");
                Label txtQuantityEnd = (Label)e.Row.FindControl("txtQuantityEnd");

                if (lstSelectedMember.Contains(entity.ItemID.ToString()))
                    chkIsSelected.Checked = true;
                System.Drawing.Color foreColor = System.Drawing.Color.Black;
                System.Drawing.Color backColor = System.Drawing.Color.White;

                txtItemUnit.Text = entity.ItemUnit;
                txtQuantityEnd.Text = entity.TotalQtyOnHand.ToString();

                if (entity.IsBPJSFormularium)
                {
                    backColor = System.Drawing.ColorTranslator.FromHtml("#adff2f");
                    foreColor = System.Drawing.Color.Black;
                }
                else if (entity.IsFormularium)
                {
                    backColor = System.Drawing.ColorTranslator.FromHtml("#FFFFE0");
                    foreColor = System.Drawing.Color.Black;
                }
                else if (entity.IsInhealthFormularium)
                {
                    backColor = System.Drawing.ColorTranslator.FromHtml("#b0e2ff");
                    foreColor = System.Drawing.Color.Black;
                }
                else if (entity.IsEmployeeFormularium)
                {
                    backColor = System.Drawing.ColorTranslator.FromHtml("#faf0e6");
                    foreColor = System.Drawing.Color.Black;
                }

                if (hdnValidationEmptyStockCtl.Value == "0")
                {
                    if (entity.TotalQtyOnHand == 0)
                    {
                        foreColor = System.Drawing.Color.Red;
                        chkIsSelected.Visible = false;
                    }
                }
                else
                {
                    if (entity.TotalQtyOnHand == 0)
                    {
                        foreColor = System.Drawing.Color.Red;
                    }
                }

                e.Row.Cells[2].BackColor = backColor;
                e.Row.Cells[3].BackColor = backColor;
                e.Row.Cells[4].BackColor = backColor;
                e.Row.Cells[5].BackColor = backColor;
                e.Row.Cells[6].BackColor = backColor;
                e.Row.Cells[7].BackColor = backColor;
                e.Row.Cells[8].BackColor = backColor;
                e.Row.Cells[9].BackColor = backColor;
                e.Row.Cells[10].BackColor = backColor;

                e.Row.Cells[2].ForeColor = foreColor;
                e.Row.Cells[3].ForeColor = foreColor;
                e.Row.Cells[4].ForeColor = foreColor;
                e.Row.Cells[5].ForeColor = foreColor;
                e.Row.Cells[6].ForeColor = foreColor;
                e.Row.Cells[7].ForeColor = foreColor;
                e.Row.Cells[8].ForeColor = foreColor;
                e.Row.Cells[9].ForeColor = foreColor;
                e.Row.Cells[10].ForeColor = foreColor;

                HtmlImage imgHAM = e.Row.FindControl("imgHAM") as HtmlImage;
                if (imgHAM != null)
                {
                    imgHAM.Visible = entity.IsHAM;
                }

                HtmlImage imgAllergy = e.Row.FindControl("imgAllergy") as HtmlImage;
                HtmlInputText lblIsHasAllergyAlert = e.Row.FindControl("lblIsHasAllergyAlert") as HtmlInputText;
                if (imgAllergy != null)
                {
                    if ((!string.IsNullOrEmpty(entity.GenericName) && Methods.SearchLike(AppSession.PatientAllergyInfo, string.Format("{0}", entity.GenericName.TrimEnd()), 5)) || Methods.SearchLike(AppSession.PatientAllergyInfo, string.Format("{0}", entity.ItemName1.TrimEnd()), 5))
                    {
                        imgAllergy.Attributes["title"] = string.Format("Pasien memiliki alergi {0} ({1})", entity.GenericName, entity.ItemName1).Replace("()", "");
                        imgAllergy.Visible = true;
                        if (lblIsHasAllergyAlert != null)
                        {
                            lblIsHasAllergyAlert.Value = "1";
                        }
                    }
                    else
                    {
                        imgAllergy.Visible = false;
                        if (lblIsHasAllergyAlert != null)
                        {
                            lblIsHasAllergyAlert.Value = "0";
                        }
                    }
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
                HtmlInputText lblIsPPRA = e.Row.FindControl("lblIsPPRA") as HtmlInputText;
                if (imgPPRA != null)
                {
                    if (entity.IsRestrictiveAntibiotics == true)
                    {
                        imgPPRA.Visible = true;
                        if (lblIsPPRA != null)
                        {
                            lblIsPPRA.Value = "1";
                        }
                    }
                    else
                    {
                        imgPPRA.Visible = false;
                        if (lblIsPPRA != null)
                        {
                            lblIsPPRA.Value = "0";
                        }
                    }
                }

                if (hdnIsAllowPreviewTariffQPCtl.Value == "0")
                {
                    txtItemUnit.Attributes.Add("style", "display:none");
                    txtQuantityEnd.Attributes.Add("style", "display:none");
                }
                else
                {
                    txtItemUnit.Attributes.Remove("style");
                    txtQuantityEnd.Attributes.Remove("style");
                }
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (hdnTransactionID.Value != "0" && hdnTransactionID.Value != "")
            {
                List<vPrescriptionOrderDt> lstItemID = BusinessLayer.GetvPrescriptionOrderDtList(string.Format(
                    "PrescriptionOrderID = {0} AND GCPrescriptionOrderStatus != '{1}' AND IsDeleted = 0", hdnTransactionID.Value, Constant.TestOrderStatus.CANCELLED));
                string lstSelectedID = "";
                if (lstItemID.Count > 0)
                {
                    foreach (vPrescriptionOrderDt itm in lstItemID)
                        lstSelectedID += "," + itm.ItemID;
                    filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
                }
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceQuickPick1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }
            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vItemBalanceQuickPick3> lstEntity = BusinessLayer.GetvItemBalanceQuickPick3List(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }


        private bool CheckForItemAlert(vDrugInfo entityItem, ref string errMessage, ref Boolean isAllergyAlert, ref Boolean isAllergyAsError, ref Boolean isAdverseReactionAlert, ref Boolean isAdverseReactionAsError, ref Boolean isDuplicateTherapyAlert, ref Boolean isDuplicateTherapyAsError, decimal dosingDuration = 1)
        {
            bool isHasAlert = false;
            StringBuilder errMessageText = new StringBuilder();

            #region Check For Allergy
            string allergenName = string.Empty;

            bool isControlAdverseReaction = AppSession.PH0006 == "1" ? true : false;
            bool isControlTheraphyDuplication = AppSession.PH0008 == "1" ? true : false;
            bool isAllergyTreatAsError = AppSession.PH0033 == "1" ? true : false;


            #region Search Allergy Information in AppSession Object
            if (!string.IsNullOrEmpty(AppSession.PatientAllergyInfo))
            {
                if (AppSession.PatientAllergyInfo.Contains(entityItem.GenericName) || AppSession.PatientAllergyInfo.Contains(entityItem.ItemName))
                {
                    errMessageText.AppendLine(string.Format("Pasien memiliki alergi {0} ({1})", entityItem.GenericName, entityItem.ItemName1).Replace("()", ""));
                    isAllergyAlert = true;
                    isAllergyAsError = isAllergyTreatAsError;
                }
            }
            else
            {
                isAllergyAlert = false;
                isAllergyAsError = false;
            }
            #endregion

            #endregion

            //string prescriptionOrderId = hdnPrescriptionOrderID.Value;

            //if (isControlTheraphyDuplication)
            //{
            //    #region Duplicate Theraphy
            //    if (prescriptionOrderId != "0" && prescriptionOrderId != "")
            //    {
            //        string filterExp = string.Format("PrescriptionOrderID = {0}", prescriptionOrderId);
            //        List<vPrescriptionOrderDt1> itemlist = BusinessLayer.GetvPrescriptionOrderDt1List(filterExp);
            //        foreach (var item in itemlist)
            //        {
            //            //Generic Name
            //            if ((item.ItemID != entityItem.ItemID) && (item.GenericName.ToLower().TrimEnd() == entityItem.GenericName.ToLower().TrimEnd()) && !item.GenericName.Equals(string.Empty))
            //            {
            //                errMessage = string.Format("Duplikasi obat dengan nama generik {0} yang sama ({1})", item.GenericName.TrimEnd(), item.DrugName.TrimEnd());
            //                isDuplicateTherapyAlert = true;
            //                isDuplicateTherapyAsError = true;
            //                return false;
            //            }
            //            vDrugInfo drugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemCode = '{0}'", item.ItemCode)).FirstOrDefault();
            //            if (drugInfo != null)
            //            {
            //                //ATC Class
            //                if ((item.ItemID != entityItem.ItemID) && ((drugInfo.ATCClassCode == entityItem.ATCClassCode) && (!String.IsNullOrEmpty(entityItem.ATCClassCode))))
            //                {
            //                    errMessage = string.Format("Duplikasi obat dengan Kelompok/Kelas ATC {0} yang sama ({1})", drugInfo.ATCClassName.TrimEnd(), item.DrugName.TrimEnd());
            //                    isDuplicateTherapyAlert = true;
            //                    isDuplicateTherapyAsError = true;
            //                    return false;
            //                }
            //                //Kelompok Theraphy
            //                if ((item.ItemID != entityItem.ItemID) && (drugInfo.MIMSClassCode.ToLower().TrimEnd() == entityItem.MIMSClassCode.ToLower().TrimEnd()) && (!String.IsNullOrEmpty(entityItem.MIMSClassCode)))
            //                {
            //                    errMessage = string.Format("Duplikasi obat dengan Kelompok Terapi {0} yang sama ({1})", drugInfo.MIMSClassName.TrimEnd(), item.DrugName.TrimEnd());
            //                    isDuplicateTherapyAlert = true;
            //                    isDuplicateTherapyAsError = true;
            //                    return false;
            //                }
            //            }
            //        }
            //    }
            //    #endregion
            //}

            //#region psikotropika & narkotika
            //if (entityItem.GCDrugClass == Constant.DrugClass.MORPHIN || entityItem.GCDrugClass == Constant.DrugClass.NARKOTIKA || entityItem.GCDrugClass == Constant.DrugClass.PSIKOTROPIKA)
            //{
            //    int duration = 0;
            //    if (dosingDuration > 0)
            //        duration = Convert.ToInt32(dosingDuration);
            //    else
            //        duration = Convert.ToInt32(txtDosingDuration.Text);
            //    if (duration > Convert.ToInt32(setParMaxDurasiNarkotika.ParameterValue))
            //    {
            //        errMessage = string.Format("Obat {0} Mengandung Narkotika, pemakaian tidak boleh lebih dari {1} hari", entityItem.ItemName1.TrimEnd(), setParMaxDurasiNarkotika.ParameterValue);
            //        return false;
            //    }
            //}
            //#endregion

            //if (!isControlAdverseReaction)
            //{
            //    isAdverseReactionAsError = false;
            //}

            //#region Adverse Reaction
            //prescriptionOrderId = hdnPrescriptionOrderID.Value;
            //if (prescriptionOrderId != "0")
            //{
            //    filterExpression = string.Format("ItemID = {0}", entityItem.ItemID);
            //    List<DrugReaction> reactions = BusinessLayer.GetDrugReactionList(filterExpression);
            //    foreach (DrugReaction advReaction in reactions)
            //    {
            //        if (prescriptionOrderId != "" && prescriptionOrderId != null)
            //        {
            //            string filterExp = string.Format("PrescriptionOrderID = {0}", prescriptionOrderId);
            //            List<vPrescriptionOrderDt1> itemlist = BusinessLayer.GetvPrescriptionOrderDt1List(filterExp);
            //            foreach (var item in itemlist)
            //            {
            //                vDrugInfo drugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemCode = '{0}'", item.ItemCode)).FirstOrDefault();
            //                if (drugInfo != null)
            //                {
            //                    //Generic Name
            //                    if (drugInfo.GenericName.ToLower().TrimEnd().Contains(advReaction.AdverseReactionText1.ToLower().TrimEnd())
            //                        || advReaction.AdverseReactionText1.ToLower().TrimEnd().Contains(drugInfo.GenericName.ToLower().TrimEnd()))
            //                    {
            //                        errMessage = string.Format("Terjadi interaksi obat dengan {0} ({1}) \n Catatan Interaksi Obat: \n {2}", item.DrugName.TrimEnd(), drugInfo.GenericName, advReaction.AdverseReactionText2);
            //                        isAdverseReactionAlert = true;
            //                        return false;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //#endregion

            errMessage = errMessageText.ToString();
            return (errMessage == string.Empty);
        }

        private bool IsValidated(string lstDispenseQty, ref string result)
        {
            StringBuilder tempMsg = new StringBuilder();

            string message = string.Empty;

            if (string.IsNullOrEmpty(message))
            {
                if (hdnVisitID.Value != AppSession.RegisteredPatient.VisitID.ToString())
                {
                    tempMsg.AppendLine("Terjadi ketidakkonsistenan antara data pasien yang dipilih dengan data pasien saat ini.");
                }
                else
                {
                    #region Validate DispenseQty
                    string[] selectedDispenseQty = lstDispenseQty.Split(',');
                    foreach (string dispenseQty in selectedDispenseQty)
                    {
                        if (string.IsNullOrEmpty(dispenseQty))
                        {
                            tempMsg.AppendLine("There is medication with empty dispense quantity. \n");
                            break;
                        }
                        else
                        {
                            Decimal value;
                            if (!Decimal.TryParse(dispenseQty, out value))
                            {
                                tempMsg.AppendLine(string.Format("There is medication with invalid dispense quantity {0} \n", dispenseQty));
                                break;
                            }
                            else
                            {
                                if (value == 0)
                                {
                                    tempMsg.AppendLine(string.Format("There is medication with invalid dispense quantity {0} \n", dispenseQty));
                                    break;
                                }
                            }
                        }
                    }
                    #endregion
                }
            }

            message = tempMsg.ToString();

            if (!string.IsNullOrEmpty(message))
            {
                result = message;
            }
            return message == string.Empty;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            if (!IsValidated(hdnSelectedMemberDispenseQty.Value, ref errMessage))
            {
                result = false;
                retval = "0";
                return result;
            }

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityOrderDtDao = new PrescriptionOrderDtDao(ctx);

            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberSigna = hdnSelectedMemberSigna.Value.Split(',');
                string[] lstSelectedMemberCoenam = hdnSelectedMemberCoenam.Value.Split(',');
                string[] lstSelectedMemberPRN = hdnSelectedMemberPRN.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                string[] lstSelectedMemberDosingUnit = hdnSelectedMemberDosingUnit.Value.Split(',');
                string[] lstSelectedMemberDispenseQty = hdnSelectedMemberDispenseQty.Value.Split(',');
                string[] lstSelectedMemberRemarks = hdnSelectedMemberRemarks.Value.Split('|');
                string[] lstSelectedMemberStartTime = hdnSelectedMemberStartTime.Value.Split(',');
                string[] lstSelectedMemberRoute = hdnSelectedMemberRoute.Value.Split(',');

                int prescriptionOrderID = 0;
                int transactionID = 0;
                string transactionNo = string.Empty;

                if (hdnTransactionID.Value == "" || hdnTransactionID.Value == "0")
                {
                    PrescriptionOrderHd entityHd = new PrescriptionOrderHd();

                    DateTime prescriptionDate = Helper.DateInStringToDateTime(hdnPrescriptionDate.Value);
                    string prescriptionTime = hdnPrescriptionTime.Value;

                    entityHd.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                    entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    entityHd.PrescriptionDate = prescriptionDate;
                    entityHd.PrescriptionTime = prescriptionTime;
                    entityHd.ClassID = Convert.ToInt32(hdnChargeClassID.Value);
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.LocationID = Convert.ToInt32(cboPopupLocation.Value);
                    entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
                    entityHd.GCPrescriptionType = hdnPrescriptionType.Value;
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
                    entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    if (hdnDispensaryUnitID.Value != "" && hdnDispensaryUnitID.Value != null)
                        entityHd.DispensaryServiceUnitID = Convert.ToInt32(hdnDispensaryUnitID.Value);
                    else
                        entityHd.DispensaryServiceUnitID = 0;

                    entityHd.Remarks = txtRemarks.Text;
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    entityHd.IsCreatedBySystem = false;
                    entityHd.IsOrderedByPhysician = true;
                    prescriptionOrderID = entityOrderHdDao.InsertReturnPrimaryKeyID(entityHd);
                    transactionID = prescriptionOrderID;
                    transactionNo = entityHd.PrescriptionOrderNo;
                }
                else
                {
                    prescriptionOrderID = Convert.ToInt32(hdnTransactionID.Value);
                    PrescriptionOrderHd orderHd1 = entityOrderHdDao.Get(prescriptionOrderID);
                    if (!string.IsNullOrEmpty(txtRemarks.Text))
                    {
                        orderHd1.Remarks = txtRemarks.Text;
                        entityOrderHdDao.Update(orderHd1);
                    }
                }

                PrescriptionOrderHd orderHd = entityOrderHdDao.Get(prescriptionOrderID);
                if (orderHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<vDrugInfo> lstDrugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);
                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        vDrugInfo drugInfo = lstDrugInfo.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));

                        PrescriptionOrderDt entity = new PrescriptionOrderDt();
                        #region PrescriptionOrderDt
                        entity.IsRFlag = true;
                        entity.ItemID = Convert.ToInt32(itemID);
                        entity.DrugName = drugInfo.ItemName1;
                        entity.GenericName = drugInfo.GenericName;
                        if (drugInfo.GCDrugForm != null)
                        {
                            entity.GCDrugForm = drugInfo.GCDrugForm;
                        }
                        entity.SignaID = null;
                        entity.Dose = drugInfo.Dose;
                        if (!String.IsNullOrEmpty(drugInfo.GCDoseUnit))
                        {
                            entity.GCDoseUnit = drugInfo.GCDoseUnit;
                        }

                        bool isDay = true;
                        #region Default Frequency
                        string frequency = "1";
                        entity.GCDosingFrequency = Constant.DosingFrequency.DAY;
                        #endregion

                        bool isTwoDigit = (lstSelectedMemberSigna[ct].Substring(0, 2).Contains('q') || lstSelectedMemberSigna[ct].Substring(0, 2).Contains('d')) ? false : true;
                        if (isTwoDigit)
                            frequency = lstSelectedMemberSigna[ct].Substring(0, 2);
                        else
                            frequency = lstSelectedMemberSigna[ct].Substring(0, 1);

                        if (lstSelectedMemberSigna[ct].ToLower().Contains("qh"))
                        {
                            isDay = false;
                            entity.GCDosingFrequency = Constant.DosingFrequency.HOUR;
                        }
                        else
                        {
                            entity.GCDosingFrequency = Constant.DosingFrequency.DAY;
                        }
                        entity.Frequency = Convert.ToInt16(frequency);

                        if (hdnIsAutoMedicationFrequency.Value == "1")
                        {
                            switch (entity.Frequency)
                            {
                                case 1:
                                    entity.IsMorning = true;
                                    break;
                                case 2:
                                    entity.IsMorning = true;
                                    entity.IsNoon = true;
                                    break;
                                case 3:
                                    entity.IsMorning = true;
                                    entity.IsNoon = true;
                                    entity.IsNight = true;
                                    break;
                                case 4:
                                    entity.IsMorning = true;
                                    entity.IsNoon = true;
                                    entity.IsEvening = true;
                                    entity.IsNight = true;
                                    break;
                                default:
                                    entity.IsMorning = true;
                                    entity.IsNoon = true;
                                    entity.IsEvening = true;
                                    entity.IsNight = true;
                                    break;
                            }
                        }

                        entity.NumberOfDosage = Convert.ToDecimal(lstSelectedMemberQty[ct]);

                        entity.GCDosingUnit = lstSelectedMemberDosingUnit[ct];

                        if (lstSelectedMemberCoenam[ct] != "-")
                        {
                            switch (lstSelectedMemberCoenam[ct])
                            {
                                case "ac":
                                    entity.GCCoenamRule = Constant.CoenamRule.AC;
                                    break;
                                case "dc":
                                    entity.GCCoenamRule = Constant.CoenamRule.DC;
                                    break;
                                case "pc":
                                    entity.GCCoenamRule = Constant.CoenamRule.PC;
                                    break;
                                default:
                                    break;
                            }
                        }

                        entity.IsAsRequired = lstSelectedMemberPRN[ct] == "1";
                        entity.StartDate = DateTime.Now.Date;
                        string[] medicationTime = Methods.GetMedicationSequenceTime(entity.Frequency).Split('|');
                        entity.Sequence1Time = medicationTime[0];
                        entity.Sequence2Time = medicationTime[1];
                        entity.Sequence3Time = medicationTime[2];
                        entity.Sequence4Time = medicationTime[3];
                        entity.Sequence5Time = medicationTime[4];
                        entity.Sequence6Time = medicationTime[5];
                        if (medicationTime[0] != "-")
                        {
                            entity.StartTime = medicationTime[0];
                        }
                        else
                        {
                            entity.StartTime = "00:00";
                            entity.Sequence1Time = "00:00";
                        }

                        entity.DispenseQty = Convert.ToDecimal(lstSelectedMemberDispenseQty[ct]);

                        if (!entity.IsUsingUDD)
                        {
                            decimal duration = 1;
                            if (isDay)
                            {
                                duration = Math.Ceiling((decimal)(entity.DispenseQty / (entity.Frequency * entity.NumberOfDosage)));
                            }
                            else
                            {
                                decimal numberOfTaken = Math.Ceiling(Convert.ToDecimal(24 / entity.Frequency));
                                duration = Math.Ceiling((decimal)(entity.DispenseQty / (numberOfTaken * entity.NumberOfDosage)));
                            }
                            if (duration <= 999)
                            {
                                entity.DosingDuration = duration;
                            }
                            else
                            {
                                entity.DosingDuration = 999;
                            }
                            entity.TakenQty = entity.ResultQty = entity.ChargeQty = entity.DispenseQty;
                        }
                        else
                        {
                            entity.DispenseQty = 0;
                            entity.TakenQty = 0;
                        }

                        entity.GCRoute = lstSelectedMemberRoute[ct];
                        entity.MedicationPurpose = drugInfo.MedicationPurpose;
                        entity.MedicationAdministration = lstSelectedMemberRemarks[ct];
                        entity.EmbalaceID = null;
                        entity.EmbalaceQty = 0;
                        entity.GCPrescriptionOrderStatus = Constant.OrderStatus.OPEN;
                        entity.IsCreatedFromOrder = true;
                        #endregion

                        entity.PrescriptionOrderID = prescriptionOrderID;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityOrderDtDao.Insert(entity);

                        ct++;
                    }
                }
                else
                {
                    errMessage = "Transaksi Sudah Diproses. Tidak Bisa Ditambahkan Item";
                    result = false;
                }
                if (result == true)
                {
                    retval = prescriptionOrderID.ToString();
                    ctx.CommitTransaction();
                }
                //}
                //else
                //{
                //    result = false;
                //    errMessage = string.Format("Maaf ada perbedaan data antara session. Harap Refresh Halaman Ini.");
                //    Exception ex = new Exception(errMessage);
                //    ctx.RollBackTransaction();
                //    Helper.InsertErrorLog(ex);
                //}
                //}
                //else
                //{
                //    result = false;
                //    errMessage = string.Format("Maaf ada perbedaan data antara session. Harap Refresh Halaman Ini.");
                //    Exception ex = new Exception(errMessage);
                //    ctx.RollBackTransaction();
                //    Helper.InsertErrorLog(ex);
                //}
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                Helper.InsertErrorLog(ex);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}