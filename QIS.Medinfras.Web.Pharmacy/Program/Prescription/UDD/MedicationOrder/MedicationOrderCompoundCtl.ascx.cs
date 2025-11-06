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

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class MedicationOrderCompoundCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            hdnQueryString.Value = queryString;
            string[] param = queryString.Split('|');
            hdnLocationID.Value = param[9];
            ledProduct.FilterExpression = string.Format("LocationID = {0} AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '{1}'",hdnLocationID.Value, Constant.ItemStatus.IN_ACTIVE);
            
            if (param[0] == "add")
            {
                IsAdd = true;
                SetControlProperties();
                tblTemplate.Style.Add("display", "table");
            }
            else
            {
                IsAdd = true;
                SetControlProperties();
                int prescriptionDetailID = Convert.ToInt32(param[5]);
                PrescriptionOrderDt entity = BusinessLayer.GetPrescriptionOrderDt(prescriptionDetailID);
                EntityToControl(entity);
                tblTemplate.Style.Add("display", "none");
            }

            hdnParamedicID.Value = param[10];
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
            SetControlEntrySetting(cboEmbalace, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmbalaceQty, new ControlEntrySetting(true, true, false, "1"));
            SetControlEntrySetting(cboMedicationRouteCompoundCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCoenamRuleCompoundCtl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMedicationPurpose, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(PrescriptionOrderDt entity)
        {
            txtDispenseQty.Text = entity.DispenseQty.ToString();
            txtCompoundMedicationName.Text = entity.CompoundDrugname;
            cboMedicationRouteCompoundCtl.Value = entity.GCRoute;
            txtDosingDose.Text = entity.NumberOfDosage.ToString();
            cboDosingUnitCompoundCtl.Value = entity.GCDosingUnit;
            chkIsUsingSweetener.Checked = entity.IsUseSweetener;

            cboFrequencyTimelineCompoundCtl.Value = entity.GCDosingFrequency;
            txtFrequencyNumber.Text = entity.Frequency.ToString();
            txtDosingDuration.Text = entity.DosingDuration.ToString();

            txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = entity.StartTime;

            txtMedicationAdministration.Text = entity.MedicationAdministration;
            txtMedicationPurpose.Text = entity.MedicationPurpose;

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
            Methods.SetComboBoxField<EmbalaceHd>(cboEmbalace, lstEmbalace, "EmbalaceName", "EmbalaceID");
            cboEmbalace.SelectedIndex = 0;

            cboFrequencyTimelineCompoundCtl.Value = Constant.DosingFrequency.DAY;
            cboMedicationRouteCompoundCtl.SelectedIndex = 0;

            txtStartDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtDispenseQty.Text = "1";
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            //isAdd;0;genericName;itemID;doseqty;gcdoseunit;compoundqty;gcitemunit;conversionfactor;itemname;doselabel;doseunit;gcdoseunit;itemunit;gcitemunit

            bool result = true;
            String[] listParam = hdnInlineEditingData.Value.Split('|');

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);

            try
            {
                int prescriptionID = -1;
                int transactionID = -1;
                string transactionNo;
                string[] paramHeader = hdnQueryString.Value.ToString().Split('|');
                if (paramHeader[2] == "0")
                {
                    #region Save Header
                    DateTime prescriptionDate = Helper.DateInStringToDateTime(paramHeader[3]);
                    string prescriptionTime = paramHeader[4];
                    int paramedicID = Convert.ToInt32(hdnParamedicID.Value);

                    PrescriptionOrderHd entityHd = new PrescriptionOrderHd();
                    entityHd.PrescriptionDate = prescriptionDate;
                    entityHd.PrescriptionTime = prescriptionTime;
                    entityHd.ParamedicID = paramedicID;
                    entityHd.VisitID = Convert.ToInt32(paramHeader[7]);
                    entityHd.ClassID = Convert.ToInt32(paramHeader[14]);
                    entityHd.GCPrescriptionType = paramHeader[13];
                    entityHd.DispensaryServiceUnitID = Convert.ToInt32(paramHeader[11]);
                    entityHd.LocationID = Convert.ToInt32(paramHeader[9]);
                    entityHd.GCRefillInstruction = paramHeader[6];
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    entityHd.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                    entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                    entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    entityHd.LastUpdatedDate = DateTime.Now;
                    entityHd.IsCreatedBySystem = true;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    prescriptionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                    transactionID = prescriptionID;
                    transactionNo = entityHd.PrescriptionOrderNo;
                    #endregion
                }
                else
                {
                    prescriptionID = Convert.ToInt32(paramHeader[1]);
                    transactionID = Convert.ToInt32(paramHeader[2]);
                    if (transactionID > 0)
                    {
                        if (paramHeader[5] != "")
                        {
                            #region Delete
                            int prescriptionDetailID = Convert.ToInt32(paramHeader[5]);

                            string filterExpression = String.Format("(PrescriptionOrderDetailID = {0} OR ParentID = {0}) AND IsCompound = 1 AND IsDeleted = 0", prescriptionDetailID);
                            List<PrescriptionOrderDt> lstDeletedEntity = BusinessLayer.GetPrescriptionOrderDtList(filterExpression,ctx);
                            foreach (PrescriptionOrderDt deletedEntity in lstDeletedEntity)
                            {
                                deletedEntity.IsDeleted = true;
                                deletedEntity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(deletedEntity);
                            }
                            #endregion 
                        }
                    }
                }
                retval = transactionID.ToString();

                int ctr = 0;
                int parentID = 0;
                foreach (String param in listParam)
                {
                    String[] data = param.Split(';');
                    bool isChanged = data[0] == "1" ? true : false;
                    string itemID = param.Split(';')[3];
                    int ID = Convert.ToInt32(data[1]);
                    if (itemID != "")
                    {
                        PrescriptionOrderDt entityDt = new PrescriptionOrderDt();

                        entityDt.IsCompound = true;
                        entityDt.PrescriptionOrderID = prescriptionID;
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
                            entityDt.Dose = null;
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
                        entityDt.TakenQty = entityDt.DispenseQty;
                        if (GCItemUnit != entityDt.GCCompoundUnit)
                        {
                            decimal dose = Convert.ToDecimal(data[10]);
                            entityDt.CompoundQtyInString = data[6].Replace(',', '.');
                            entityDt.CompoundQty = qty / dose;
                            entityDt.ResultQty = qty * entityDt.TakenQty / dose;
                        }
                        else
                        {
                            entityDt.CompoundQty = qty;
                            entityDt.CompoundQtyInString = data[6].Replace(',', '.');
                            entityDt.ResultQty = qty * entityDt.TakenQty;
                        }

                        entityDt.DrugName = data[9];
                        entityDt.ChargeQty = Math.Ceiling(entityDt.ResultQty);

                        entityDt.CompoundDrugname = txtCompoundMedicationName.Text;
                        entityDt.GCRoute = cboMedicationRouteCompoundCtl.Value.ToString();

                        entityDt.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
                        entityDt.GCDosingUnit = cboDosingUnitCompoundCtl.Value.ToString();
                        entityDt.IsUseSweetener = chkIsUsingSweetener.Checked;

                        entityDt.GCDosingFrequency = cboFrequencyTimelineCompoundCtl.Value.ToString();
                        entityDt.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
                        entityDt.DosingDuration = Convert.ToInt16(txtDosingDuration.Text);

                        entityDt.StartDate = Helper.GetDatePickerValue(txtStartDate);
                        entityDt.StartTime = txtStartTime.Text;

                        entityDt.MedicationAdministration = txtMedicationAdministration.Text;
                        entityDt.MedicationPurpose = txtMedicationPurpose.Text;

                        entityDt.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;

                        if (ctr > 0)
                        {
                            entityDt.IsRFlag = false;
                            entityDt.ParentID = parentID;
                        }
                        else
                        {
                            entityDt.IsRFlag = true;
                            entityDt.ParentID = null;
                        }

                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);

                        entityDt.PrescriptionOrderDetailID = BusinessLayer.GetPrescriptionOrderDtMaxID(ctx);
                        if (ctr < 1)
                        {
                            parentID = entityDt.PrescriptionOrderDetailID;
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