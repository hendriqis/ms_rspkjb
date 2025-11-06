using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TemplatePrescriptionCompoundEntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            hdnQueryString.Value = queryString;
            string[] param = queryString.Split('|');

            if (param.Length > 2)
            {
                IsAdd = true;
                SetControlProperties();
                int prescriptionTemplateDetailID = Convert.ToInt32(param[2]);
                PrescriptionTemplateDt entity = BusinessLayer.GetPrescriptionTemplateDt(prescriptionTemplateDetailID);
                EntityToControl(entity);
                tblTemplate.Style.Add("display", "none");
                hdnIsAddNew.Value = "0";
            }
            else
            {
                IsAdd = true;
                SetControlProperties();
                tblTemplate.Style.Add("display", "table");
                hdnIsAddNew.Value = "1";
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtCompoundMedicationName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(cboFrequencyTimelineCompoundCtl, new ControlEntrySetting(true, true, true, Constant.DosingFrequency.DAY));
            SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(cboDosingUnitCompoundCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDosingDuration, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(chkIsUsingSweetener, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsPRN, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboEmbalaceCompoundCtl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmbalaceQty, new ControlEntrySetting(true, true, false, "1"));
            SetControlEntrySetting(cboMedicationRouteCompoundCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCoenamRuleCompoundCtl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(PrescriptionTemplateDt entity)
        {
            txtDispenseQty.Text = entity.DispenseQty.ToString();
            txtCompoundMedicationName.Text = entity.CompoundDrugname;
            cboMedicationRouteCompoundCtl.Value = entity.GCRoute;
            txtDosingDose.Text = entity.NumberOfDosage.ToString();
            cboDosingUnitCompoundCtl.Value = entity.GCDosingUnit;
            chkIsUsingSweetener.Checked = entity.IsUseSweetener;
            chkIsPRN.Checked = entity.IsAsRequired;

            cboFrequencyTimelineCompoundCtl.Value = entity.GCDosingFrequency;
            txtFrequencyNumber.Text = entity.Frequency.ToString();
            txtDosingDuration.Text = entity.DosingDuration.ToString();
            cboCoenamRuleCompoundCtl.Value = entity.GCCoenamRule;

            if (entity.SignaID != null && entity.SignaID != 0)
            {
                hdnSignaIDCompound.Value = entity.SignaID.ToString();
                Signa signa = BusinessLayer.GetSigna(Convert.ToInt32(entity.SignaID));
                txtSignaLabelCompound.Text = signa.SignaLabel;
                txtSignaName1Compound.Text = signa.SignaName1;
            }

            chkIsMorning.Checked = entity.IsMorning;
            chkIsNoon.Checked = entity.IsNoon;
            chkIsEvening.Checked = entity.IsEvening;
            chkIsNight.Checked = entity.IsNight;

            if (entity.EmbalaceID != null && entity.EmbalaceID != 0)
            {
                EmbalaceHd embalace = BusinessLayer.GetEmbalaceHd((int)entity.EmbalaceID);
                cboEmbalaceCompoundCtl.Value = entity.EmbalaceID.ToString();
                txtEmbalaceQty.Text = entity.EmbalaceQty.ToString(Constant.FormatString.NUMERIC_2);
                if (!embalace.IsUsingRangePricing)
                {
                    txtEmbalaceQty.Attributes.Remove("readonly");
                }
                else
                {
                    txtEmbalaceQty.Attributes.Add("readonly", "readonly");
                }
            }

            txtMedicationAdministration.Text = entity.MedicationAdministration;
        }

        private void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.COENAM_RULE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboCompoundUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimelineCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCompoundStrengthUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnitCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1" || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRouteCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRuleCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            List<EmbalaceHd> lstEmbalace = BusinessLayer.GetEmbalaceHdList("IsDeleted = 0");
            lstEmbalace.Insert(0, new EmbalaceHd() { EmbalaceID = 0, EmbalaceName = "" });
            Methods.SetComboBoxField<EmbalaceHd>(cboEmbalaceCompoundCtl, lstEmbalace, "EmbalaceName", "EmbalaceID");
            cboEmbalaceCompoundCtl.SelectedIndex = 0;

            cboFrequencyTimelineCompoundCtl.Value = Constant.DosingFrequency.DAY;
            cboMedicationRouteCompoundCtl.SelectedIndex = 0;

            txtStartDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtDispenseQty.Text = "1";
        }

        private void ControlToEntity(PrescriptionTemplateDt entityDt, String[] data)
        {
            bool isChanged = data[0] == "1" ? true : false;
            string itemID = data[3];
            if (itemID != "")
            {
                entityDt.IsCompound = true;
                entityDt.GenericName = data[2];
                if (data[3] != "")
                    entityDt.ItemID = Convert.ToInt32(data[3]);
                else
                    entityDt.ItemID = null;
                if (data[5] != "")
                {
                    entityDt.Dose = Convert.ToDecimal(data[4]);
                    entityDt.GCDoseUnit = data[5];
                }
                else
                {
                    entityDt.Dose = 0;
                    entityDt.GCDoseUnit = null;
                }
                string GCItemUnit = data[14];

                entityDt.GCCompoundUnit = data[7];
                entityDt.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
                entityDt.TakenQty = entityDt.DispenseQty;
                if (GCItemUnit != entityDt.GCCompoundUnit)
                {
                    string compoundQty = data[6];
                    decimal qty = 0;
                    if (compoundQty.Contains('/'))
                    {
                        string[] compoundInfo = compoundQty.Split('/');
                        decimal num1 = Convert.ToDecimal(compoundInfo[0]);
                        decimal num2 = Convert.ToDecimal(compoundInfo[1]);
                        //qty = Math.Round(num1 / num2, 2);

                        decimal dose = Convert.ToDecimal(data[10]);
                        entityDt.CompoundQtyInString = data[6];
                        entityDt.CompoundQty = num1 / num2 / dose;
                        entityDt.ResultQty = num1 / num2 * entityDt.TakenQty / dose;
                    }
                    else
                    {
                        qty = Convert.ToDecimal(compoundQty);

                        decimal dose = Convert.ToDecimal(data[10]);
                        entityDt.CompoundQtyInString = data[6];
                        entityDt.CompoundQty = qty / dose;
                        entityDt.ResultQty = qty * entityDt.TakenQty / dose;
                    }
                }
                else
                {
                    string compoundQty = data[6];
                    decimal qty = 0;
                    if (compoundQty.Contains('/'))
                    {
                        string[] compoundInfo = compoundQty.Split('/');
                        decimal num1 = Convert.ToDecimal(compoundInfo[0]);
                        decimal num2 = Convert.ToDecimal(compoundInfo[1]);
                        //qty = Math.Round(num1 / num2, 2);

                        entityDt.CompoundQty = num1 / num2;
                        entityDt.CompoundQtyInString = data[6];
                        entityDt.ResultQty = num1 / num2 * entityDt.TakenQty;
                    }
                    else
                    {
                        qty = Convert.ToDecimal(compoundQty);

                        entityDt.CompoundQty = qty;
                        entityDt.CompoundQtyInString = data[6];
                        entityDt.ResultQty = qty * entityDt.TakenQty;
                    }
                }

                entityDt.DrugName = data[9];
                entityDt.ChargeQty = entityDt.ResultQty;
                if (data[15] == Constant.QuantityDeductionType.DIBULATKAN) entityDt.ChargeQty = Math.Ceiling(entityDt.ChargeQty);
                if (data[16] == Constant.QuantityDeductionType.DIBULATKAN) entityDt.ResultQty = Math.Ceiling(entityDt.ResultQty);
                entityDt.CompoundDrugname = txtCompoundMedicationName.Text;

                if (cboMedicationRouteCompoundCtl.Value != null)
                {
                    entityDt.GCRoute = cboMedicationRouteCompoundCtl.Value.ToString();
                }

                if (cboCoenamRuleCompoundCtl.Value != null)
                {
                    entityDt.GCCoenamRule = cboCoenamRuleCompoundCtl.Value.ToString();
                }

                if (hdnSignaIDCompound.Value != null && hdnSignaIDCompound.Value != "")
                {
                    entityDt.SignaID = Convert.ToInt32(hdnSignaIDCompound.Value);
                }
                else
                {
                    entityDt.SignaID = null;
                }

                if (entityDt.ConversionFactor != 0)
                {
                    entityDt.ConversionFactor = entityDt.ConversionFactor;
                }
                else
                {
                    entityDt.ConversionFactor = 1;
                }

                entityDt.NumberOfDosage = txtDosingDose.Text != "" ? Convert.ToDecimal(txtDosingDose.Text) : 0;
                entityDt.GCDosingUnit = cboDosingUnitCompoundCtl.Value.ToString();
                entityDt.IsUseSweetener = chkIsUsingSweetener.Checked;
                entityDt.IsAsRequired = chkIsPRN.Checked;
                entityDt.GCDosingFrequency = cboFrequencyTimelineCompoundCtl.Value.ToString();
                entityDt.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
                entityDt.DosingDuration = Convert.ToInt16(txtDosingDuration.Text);
                entityDt.MedicationAdministration = txtMedicationAdministration.Text;
                entityDt.IsRFlag = false;
                entityDt.ParentID = null;

                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            if (!string.IsNullOrEmpty(txtDosingDuration.Text) && !string.IsNullOrEmpty(txtDispenseQty.Text))
            {
                decimal dispenseQty = 0;
                bool isDecimal = decimal.TryParse(txtDispenseQty.Text, out dispenseQty);

                if (dispenseQty <= 0)
                {
                    result = false;
                    errMessage = "Dispense Quantity should be greater than 0";
                }
            }
            else
            {
                result = false;
                errMessage = "Medication Duration and Dispense Quantity should be greater than 0";
            }

            if (result)
            {
                String[] listParam = hdnInlineEditingData.Value.Split('|');

                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionTemplateHdDao entityHdDao = new PrescriptionTemplateHdDao(ctx);
                PrescriptionTemplateDtDao entityDtDao = new PrescriptionTemplateDtDao(ctx);
                try
                {
                    int prescriptionTemplateID = -1;
                    string[] paramHeader = hdnQueryString.Value.ToString().Split('|');
                    bool isAdd = paramHeader[0] == "";
                    if (paramHeader[0] == "")
                    {
                        #region Save Header
                        PrescriptionTemplateHd entityHd = new PrescriptionTemplateHd();
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        prescriptionTemplateID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                        #endregion
                    }
                    else
                    {
                        prescriptionTemplateID = Convert.ToInt32(paramHeader[1]);
                    }

                    if (paramHeader.Length > 2)
                    #region Delete
                    {
                        int prescriptionTemplateDetailID = Convert.ToInt32(paramHeader[2]);
                        StringBuilder listID = new StringBuilder();
                        foreach (String param in listParam)
                        {
                            string itemID = param.Split(';')[3];
                            if (itemID != "")
                            {
                                int ID = Convert.ToInt32(itemID);
                                if (ID > 0)
                                {
                                    if (listID.ToString() != "")
                                        listID.Append(", ");
                                    listID.Append(ID);
                                }
                            }
                        }
                        if (listID.ToString() != "")
                        {
                            string filterExpression = String.Format("(PrescriptionTemplateDetailID = {0} OR ParentID = {0}) AND PrescriptionTemplateDetailID NOT IN ({1}) AND IsCompound = 1 AND IsDeleted = 0", prescriptionTemplateDetailID, listID.ToString());

                            List<PrescriptionTemplateDt> lstDeletedEntity = BusinessLayer.GetPrescriptionTemplateDtList(filterExpression, ctx);
                            foreach (PrescriptionTemplateDt deletedEntity in lstDeletedEntity)
                            {
                                deletedEntity.IsDeleted = true;
                                deletedEntity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(deletedEntity);
                            }
                        }
                    }
                    #endregion

                    retval = prescriptionTemplateID.ToString();
                    short ctr = 0;
                    int parentID = 0;
                    int noOfItems = listParam.Count();

                    foreach (String param in listParam)
                    {
                        String[] data = param.Split(';');
                        bool isChanged = data[0] == "1" ? true : false;
                        string itemID = param.Split(';')[3];
                        int ID = Convert.ToInt32(data[1]);
                        if (itemID != "")
                        {
                            PrescriptionTemplateDt entityDt = new PrescriptionTemplateDt();

                            entityDt.IsCompound = true;
                            entityDt.PrescriptionTemplateID = prescriptionTemplateID;
                            entityDt.IsRFlag = (ctr < 1);
                            entityDt.GenericName = data[2];
                            if (data[3] != "")
                                entityDt.ItemID = Convert.ToInt32(data[3]);
                            else
                                entityDt.ItemID = null;
                            if (data[5] != "")
                            {
                                entityDt.Dose = Convert.ToDecimal(data[4]);
                                entityDt.GCDoseUnit = data[5];
                            }
                            else
                            {
                                entityDt.Dose = 0;
                                entityDt.GCDoseUnit = null;
                            }
                            string GCItemUnit = data[14];

                            string compoundQty = data[6].Replace(',', '.');
                            decimal qty = 0;
                            if (compoundQty.Contains('/'))
                            {
                                string[] compoundInfo = compoundQty.Split('/');
                                decimal num1 = Convert.ToDecimal(compoundInfo[0]);
                                decimal num2 = Convert.ToDecimal(compoundInfo[1]);
                                qty = Math.Round(num1 / num2, 2);
                            }
                            else
                            {
                                qty = Convert.ToDecimal(compoundQty);
                            }

                            entityDt.GCCompoundUnit = data[7];
                            entityDt.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
                            entityDt.TakenQty = Convert.ToDecimal(txtDispenseQty.Text);
                            if (GCItemUnit != entityDt.GCCompoundUnit)
                            {
                                decimal dose = Convert.ToDecimal(data[10]);
                                entityDt.CompoundQtyInString = data[6];
                                entityDt.CompoundQty = qty / dose;
                                entityDt.ResultQty = qty * entityDt.DispenseQty / dose;
                            }
                            else
                            {
                                entityDt.CompoundQty = qty;
                                entityDt.CompoundQtyInString = data[6];
                                entityDt.ResultQty = qty * entityDt.DispenseQty;
                            }

                            entityDt.DrugName = data[9];
                            entityDt.ChargeQty = entityDt.ResultQty;

                            if (data[15] == Constant.QuantityDeductionType.DIBULATKAN)
                                entityDt.ChargeQty = Math.Ceiling(entityDt.ChargeQty);

                            if (data[16] == Constant.QuantityDeductionType.DIBULATKAN)
                                entityDt.ResultQty = Math.Ceiling(entityDt.ResultQty);

                            entityDt.CompoundDrugname = txtCompoundMedicationName.Text;
                            entityDt.GCRoute = cboMedicationRouteCompoundCtl.Value.ToString();

                            if (hdnSignaIDCompound.Value != "" && hdnSignaIDCompound.Value != "0")
                            {
                                entityDt.SignaID = Convert.ToInt32(hdnSignaIDCompound.Value);
                            }
                            else
                            {
                                entityDt.SignaID = null;
                            }

                            entityDt.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
                            entityDt.GCDosingUnit = cboDosingUnitCompoundCtl.Value.ToString();

                            if (!String.IsNullOrEmpty(cboCoenamRuleCompoundCtl.Text))
                                entityDt.GCCoenamRule = !String.IsNullOrEmpty(cboCoenamRuleCompoundCtl.Value.ToString()) ? cboCoenamRuleCompoundCtl.Value.ToString() : null;
                            
                            entityDt.IsUseSweetener = chkIsUsingSweetener.Checked;
                            entityDt.IsAsRequired = chkIsPRN.Checked;

                            entityDt.GCDosingFrequency = cboFrequencyTimelineCompoundCtl.Value.ToString();
                            entityDt.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
                            entityDt.DosingDuration = Convert.ToInt16(txtDosingDuration.Text);

                            entityDt.IsMorning = chkIsMorning.Checked;
                            entityDt.IsNoon = chkIsNoon.Checked;
                            entityDt.IsEvening = chkIsEvening.Checked;
                            entityDt.IsNight = chkIsNight.Checked;

                            entityDt.MedicationAdministration = txtMedicationAdministration.Text;
                            entityDt.MedicationPurpose = txtCompoundMedicationName.Text;

                            if (cboEmbalaceCompoundCtl.Value != null)
                            {
                                entityDt.EmbalaceID = Convert.ToInt32(cboEmbalaceCompoundCtl.Value);
                                entityDt.EmbalaceQty = Convert.ToDecimal(txtEmbalaceQty.Text);
                            }
                            else
                            {
                                if (hdnIsEmbalaceQtyTerisiOtomatis.Value == "1")
                                {
                                    entityDt.EmbalaceQty = 1;
                                }
                            }

                            if (ctr > 0)
                            {
                                entityDt.ParentID = parentID;
                            }
                            else
                            {
                                entityDt.ParentID = null;
                                entityDt.TakenQty = entityDt.DispenseQty;
                            }

                            entityDt.CreatedBy = AppSession.UserLogin.UserID;

                            if (ctr < 1)
                            {
                                parentID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);
                            }
                            else
                            {
                                entityDtDao.Insert(entityDt);
                            }

                            ctr++;
                        }
                    }
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