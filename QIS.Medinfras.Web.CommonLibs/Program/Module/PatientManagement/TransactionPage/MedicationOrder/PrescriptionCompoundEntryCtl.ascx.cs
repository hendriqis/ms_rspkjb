using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PrescriptionCompoundEntryCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            hdnQueryString.Value = queryString;
            string[] param = queryString.Split('|');
            
            if (param[0] == "0")
            {
                hdnIsDrugChargesJustDistributionCP.Value = param[10];

                if (hdnIsDrugChargesJustDistributionCP.Value == "1")
                {
                    ledProduct.FilterExpression = string.Format("IsDeleted = 0 AND GCItemRequestType = '{0}'", Constant.ItemRequestType.DISTRIBUTION);
                }
                else
                {
                    ledProduct.FilterExpression = string.Format("IsDeleted = 0");
                }
            }
            else
            {
                hdnIsDrugChargesJustDistributionCP.Value = param[3];

                if (hdnIsDrugChargesJustDistributionCP.Value == "1")
                {
                    ledProduct.FilterExpression = string.Format("ItemID NOT IN (SELECT ItemID FROM PrescriptionOrderDt WHERE PrescriptionOrderID = {0} AND IsDeleted = 0) AND IsDeleted = 0 AND GCItemRequestType = '{1}'", param[0], Constant.ItemRequestType.DISTRIBUTION);
                }
                else
                {
                    ledProduct.FilterExpression = string.Format("ItemID NOT IN (SELECT ItemID FROM PrescriptionOrderDt WHERE PrescriptionOrderID = {0} AND IsDeleted = 0) AND IsDeleted = 0", param[0]);
                }
            }

            ledProduct.FilterExpression += string.Format(" AND ISNULL(GCItemStatus,'') != '{0}'", Constant.ItemStatus.IN_ACTIVE);

            if (param.Length > 4)
            {
                IsAdd = true;
                SetControlProperties();
            }
            else
            {
                IsAdd = false;
                SetControlProperties();
                int prescriptionDetailID = Convert.ToInt32(param[1]);
                PrescriptionOrderDt entity = BusinessLayer.GetPrescriptionOrderDt(prescriptionDetailID);
                EntityToControl(entity);
            }

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtCompoundMedicationName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboMedicationRouteCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDosingUnitCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboFrequencyTimelineCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsUsingSweetener, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(PrescriptionOrderDt entity)
        {
            txtDispenseQty.Text = entity.DispenseQty.ToString();
            txtCompoundMedicationName.Text = entity.CompoundDrugname;
            cboMedicationRouteCtl.Value = entity.GCRoute;
            txtDosingDose.Text = entity.NumberOfDosage.ToString();
            cboDosingUnitCtl.Value = entity.GCDosingUnit;
            txtDosingDuration.Text = entity.DosingDuration.ToString();
            //chkIsUsingSweetener.Checked = entity.IsUseSweetener;

            cboFrequencyTimelineCtl.Value = entity.GCDosingFrequency;
            txtFrequencyNumber.Text = entity.Frequency.ToString();

            txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = entity.StartTime;

            txtMedicationAdministration.Text = entity.MedicationAdministration;

        }

        private void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboCompoundUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimelineCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCompoundStrengthUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty.Equals("1")).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnitCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty.Equals("1")).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRouteCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");

            cboFrequencyTimelineCtl.Value = Constant.DosingFrequency.DAY;
            txtDosingDurationTimeline.Text = cboFrequencyTimelineCtl.Text;
           
            txtStartDate.Text = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = AppSession.RegisteredPatient.VisitTime;
            txtDispenseQty.Text = "1";
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            String[] listParam = hdnInlineEditingData.Value.Split('|');
            foreach (String param in listParam)
            {
                String[] dataSplit = param.Split(';');

                bool isChanged = dataSplit[0] == "1" ? true : false;
                int ID = Convert.ToInt32(dataSplit[1]);
                if (isChanged || ID > 0)
                {
                    if (Convert.ToDecimal(dataSplit[4]) <= 0 || Convert.ToDecimal(dataSplit[6]) <= 0)
                    {
                        result = false;
                        errMessage = "Dose Quantity or Compound Quantity should be greater than 0";
                    }
                }
            }

            if (result)
            {
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
                PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
                try
                {
                    int prescriptionID = -1;
                    string[] paramHeader = hdnQueryString.Value.ToString().Split('|');
                    if (paramHeader[0] == "0")
                    {
                        #region Save Header
                        DateTime prescriptionDate = Helper.DateInStringToDateTime(paramHeader[1]);
                        string prescriptionTime = paramHeader[2];
                        int paramedicID = Convert.ToInt32(paramHeader[3]);
                        string refillInstruction = paramHeader[4];

                        PrescriptionOrderHd entityHd = new PrescriptionOrderHd();
                        entityHd.PrescriptionDate = prescriptionDate;
                        entityHd.PrescriptionTime = prescriptionTime;
                        entityHd.ParamedicID = paramedicID;
                        entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                        entityHd.DispensaryServiceUnitID = Convert.ToInt16(paramHeader[7]);
                        entityHd.LocationID = Convert.ToInt16(paramHeader[8]);
                        entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
                        entityHd.ClassID = AppSession.RegisteredPatient.ClassID;
                        entityHd.GCPrescriptionType = paramHeader[9];
                        entityHd.GCRefillInstruction = refillInstruction;
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
                            default:
                                entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                                break;
                        }
                        entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        prescriptionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                        #endregion
                    }
                    else
                    {
                        prescriptionID = Convert.ToInt32(paramHeader[0]);
                        #region Delete
                        int prescriptionDetailID = Convert.ToInt32(paramHeader[1]);
                        StringBuilder listID = new StringBuilder();
                        foreach (String param in listParam)
                        {
                            int ID = Convert.ToInt32(param.Split(';')[1]);
                            if (ID > 0)
                            {
                                if (listID.ToString() != "")
                                    listID.Append(", ");
                                listID.Append(ID);
                            }
                        }
                        if (listID.ToString() != "")
                        {
                            string filterExpression = String.Format("(PrescriptionOrderDetailID = {0} OR ParentID = {0}) AND PrescriptionOrderDetailID NOT IN ({1}) AND IsCompound = 1 AND IsDeleted = 0", prescriptionDetailID, listID.ToString());

                            List<PrescriptionOrderDt> lstDeletedEntity = BusinessLayer.GetPrescriptionOrderDtList(filterExpression);
                            foreach (PrescriptionOrderDt deletedEntity in lstDeletedEntity)
                            {
                                deletedEntity.IsDeleted = true;
                                deletedEntity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(deletedEntity);
                            }
                        }
                        #endregion
                    }
                    retval = prescriptionID.ToString();

                    int ctr = 0;
                    int parentID = 0;
                    foreach (String param in listParam)
                    {
                        String[] data = param.Split(';');
                        bool isChanged = data[0] == "1" ? true : false;
                        int ID = Convert.ToInt32(data[1]);
                        if (isChanged || ID > 0)
                        {
                            bool isAdd;
                            PrescriptionOrderDt entityDt = null;
                            if (ID > 0)
                            {
                                isAdd = false;
                                entityDt = entityDtDao.Get(ID);
                            }
                            else
                            {
                                isAdd = true;
                                entityDt = new PrescriptionOrderDt();
                            }
                            entityDt.IsCompound = true;
                            entityDt.PrescriptionOrderID = prescriptionID;
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
                                entityDt.Dose = null;
                                entityDt.GCDoseUnit = null;
                            }
                            string GCItemUnit = data[14];

                            entityDt.GCCompoundUnit = data[7];
                            if (GCItemUnit != entityDt.GCCompoundUnit)
                            {
                                decimal dose = Convert.ToDecimal(data[10]);
                                entityDt.CompoundQty = Convert.ToDecimal(data[6].Replace(',', '.'));
                                entityDt.CompoundQtyInString = string.Format("{0}/{1}", entityDt.CompoundQty, dose);

                                entityDt.CompoundQty = entityDt.CompoundQty / dose;
                            }
                            else
                            {
                                entityDt.CompoundQty = Convert.ToDecimal(data[6].Replace(',', '.'));
                                entityDt.CompoundQtyInString = entityDt.CompoundQty.ToString();
                            }

                            entityDt.DrugName = data[9];

                            entityDt.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
                            entityDt.TakenQty = Convert.ToDecimal(txtDispenseQty.Text);
                            entityDt.ResultQty = entityDt.CompoundQty * entityDt.DispenseQty;
                            entityDt.ChargeQty = Math.Ceiling(entityDt.ResultQty);

                            entityDt.CompoundDrugname = txtCompoundMedicationName.Text;
                            entityDt.GCRoute = cboMedicationRouteCtl.Value.ToString();

                            entityDt.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
                            entityDt.GCDosingUnit = cboDosingUnitCtl.Value.ToString();
                            entityDt.IsUseSweetener = chkIsUsingSweetener.Checked;

                            entityDt.GCDosingFrequency = cboFrequencyTimelineCtl.Value.ToString();
                            entityDt.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);

                            entityDt.StartDate = Helper.GetDatePickerValue(txtStartDate);
                            entityDt.StartTime = txtStartTime.Text;

                            entityDt.DosingDuration = Convert.ToDecimal(txtDosingDuration.Text);

                            entityDt.MedicationAdministration = txtMedicationAdministration.Text;

                            entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;
                            entityDt.IsCreatedFromOrder = true;

                            if (ctr > 0)
                                entityDt.ParentID = parentID;
                            else
                                entityDt.ParentID = null;
                            if (isAdd)
                            {
                                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Insert(entityDt);

                                if (ctr < 1)
                                    entityDt.PrescriptionOrderDetailID = BusinessLayer.GetPrescriptionOrderDtMaxID(ctx);
                            }
                            else
                            {
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entityDt);
                            }
                            if (ctr < 1)
                                parentID = entityDt.PrescriptionOrderDetailID;

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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            String[] listParam = hdnInlineEditingData.Value.Split('|');
            foreach (String param in listParam)
            {
                String[] dataSplit = param.Split(';');

                bool isChanged = dataSplit[0] == "1" ? true : false;
                int ID = Convert.ToInt32(dataSplit[1]);
                if (isChanged || ID > 0)
                {
                    if (Convert.ToDecimal(dataSplit[4]) <= 0 || Convert.ToDecimal(dataSplit[6]) <= 0)
                    {
                        result = false;
                        errMessage = "Dose Quantity or Compound Quantity should be greater than 0";
                    }
                }
            }

            if (result)
            {
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
                PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);

                try
                {
                    int prescriptionID = -1;
                    string[] paramHeader = hdnQueryString.Value.ToString().Split('|');
                    if (paramHeader[0] != "" && paramHeader[0] != "0")
                    {
                        prescriptionID = Convert.ToInt32(paramHeader[0]);
                        #region Delete
                        int prescriptionDetailID = Convert.ToInt32(paramHeader[1]);
                        StringBuilder listID = new StringBuilder();
                        foreach (String param in listParam)
                        {
                            int ID = Convert.ToInt32(param.Split(';')[1]);
                            if (ID > 0)
                            {
                                if (listID.ToString() != "")
                                    listID.Append(", ");
                                listID.Append(ID);
                            }
                        }
                        if (listID.ToString() != "")
                        {
                            string filterExpression = String.Format("(PrescriptionOrderDetailID = {0} OR ParentID = {0}) AND PrescriptionOrderDetailID NOT IN ({1}) AND IsCompound = 1 AND IsDeleted = 0", prescriptionDetailID, listID.ToString());

                            List<PrescriptionOrderDt> lstDeletedEntity = BusinessLayer.GetPrescriptionOrderDtList(filterExpression);
                            foreach (PrescriptionOrderDt deletedEntity in lstDeletedEntity)
                            {
                                deletedEntity.IsDeleted = true;
                                deletedEntity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(deletedEntity);
                            }
                        }
                        #endregion
                    }
                    retval = prescriptionID.ToString();

                    int ctr = 0;
                    int parentID = 0;
                    foreach (String param in listParam)
                    {
                        String[] data = param.Split(';');
                        bool isChanged = data[0] == "1" ? true : false;
                        int ID = Convert.ToInt32(data[1]);
                        if (isChanged || ID > 0)
                        {
                            bool isAdd;
                            PrescriptionOrderDt entityDt = null;
                            if (ID > 0)
                            {
                                isAdd = false;
                                entityDt = entityDtDao.Get(ID);
                            }
                            else
                            {
                                isAdd = true;
                                entityDt = new PrescriptionOrderDt();
                            }
                            entityDt.IsCompound = true;
                            entityDt.PrescriptionOrderID = prescriptionID;
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
                                entityDt.Dose = null;
                                entityDt.GCDoseUnit = null;
                            }
                            string GCItemUnit = data[14];

                            entityDt.GCCompoundUnit = data[7];
                            if (GCItemUnit != entityDt.GCCompoundUnit)
                            {
                                decimal dose = Convert.ToDecimal(data[10]);
                                entityDt.CompoundQty = Convert.ToDecimal(data[6]);
                                entityDt.CompoundQtyInString = string.Format("{0}/{1}", entityDt.CompoundQty, dose);

                                entityDt.CompoundQty = entityDt.CompoundQty / dose;
                            }
                            else
                            {
                                entityDt.CompoundQty = Convert.ToDecimal(data[6]);
                                entityDt.CompoundQtyInString = entityDt.CompoundQty.ToString();
                            }

                            entityDt.DrugName = data[9];

                            entityDt.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
                            entityDt.TakenQty = Convert.ToDecimal(txtDispenseQty.Text);
                            entityDt.ResultQty = entityDt.CompoundQty * entityDt.DispenseQty;
                            entityDt.ChargeQty = Math.Ceiling(entityDt.ResultQty);

                            entityDt.CompoundDrugname = txtCompoundMedicationName.Text;
                            entityDt.GCRoute = cboMedicationRouteCtl.Value.ToString();

                            entityDt.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
                            entityDt.GCDosingUnit = cboDosingUnitCtl.Value.ToString();
                            entityDt.IsUseSweetener = chkIsUsingSweetener.Checked;

                            entityDt.GCDosingFrequency = cboFrequencyTimelineCtl.Value.ToString();
                            entityDt.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);

                            entityDt.DosingDuration = Convert.ToDecimal(txtDosingDuration.Text);

                            entityDt.StartDate = Helper.GetDatePickerValue(txtStartDate);
                            entityDt.StartTime = txtStartTime.Text;

                            entityDt.MedicationAdministration = txtMedicationAdministration.Text;

                            entityDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.OPEN;

                            if (ctr > 0)
                                entityDt.ParentID = parentID;
                            else
                                entityDt.ParentID = null;
                            if (isAdd)
                            {
                                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Insert(entityDt);

                                if (ctr < 1)
                                    entityDt.PrescriptionOrderDetailID = BusinessLayer.GetPrescriptionOrderDtMaxID(ctx);
                            }
                            else
                            {
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entityDt);
                            }
                            if (ctr < 1)
                                parentID = entityDt.PrescriptionOrderDetailID;

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
            }
            return result;
        }
    }
}