using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class CurrentMedicationList : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            if (Page.Request.QueryString.Count > 0)
                return Constant.MenuCode.EMR.HEALTH_RECORD_CURRENT_MEDICATION;
            return Constant.MenuCode.EMR.CURRENT_MEDICATION;
        }

        #region List
        protected override void InitializeDataControl()
        {
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);

            string code = ddlViewType.SelectedValue;
            if (code == "1")
                filterExpression += string.Format(" AND (EndDate IS NULL OR EndDate >= '{0}')", DateTime.Now.ToString("yyyyMMdd"));
            else if (code == "2")
                filterExpression += string.Format(" AND EndDate < '{0}'", DateTime.Now.ToString("yyyyMMdd"));

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPastMedicationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPastMedication> lstEntity = BusinessLayer.GetvPastMedicationList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
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
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                PastMedication entity = BusinessLayer.GetPastMedication(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdatePastMedication(entity);
                return true;
            }
            return false;
        }
        #endregion

        #region Entry
        protected override void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0", Constant.StandardCode.DRUG_FORM, Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboForm, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboStrengthUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            //SetControlEntrySetting(ledDrugName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGenericName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboForm, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStrengthAmount, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboStrengthUnit, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboFrequencyTimeline, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDosingUnit, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUntilNow, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDuration, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPurposeOfMedication, new ControlEntrySetting(true, true, false));
        }

        private void ControlToEntity(PastMedication entity)
        {
            if (hdnDrugID.Value.ToString() != "")
            {
                entity.ItemID = Convert.ToInt32(hdnDrugID.Value);
                entity.DrugName = "";
            }
            else
            {
                entity.ItemID = null;
                entity.DrugName = hdnDrugName.Value;
            }
            entity.GenericName = txtGenericName.Text;
            entity.GCDrugForm = cboForm.Value.ToString();
            if (cboStrengthUnit.Value != null && cboStrengthUnit.Value.ToString() != "")
            {
                entity.Dose = Convert.ToDecimal(txtStrengthAmount.Text);
                entity.GCDoseUnit = cboStrengthUnit.Value.ToString();
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
            if (chkIsUntilNow.Checked)
                entity.DosingDuration = 0;
            else
            {
                if (txtDuration.Text != "")
                    entity.DosingDuration = Convert.ToDecimal(txtDuration.Text);
            }
            entity.GCRoute = cboMedicationRoute.Value.ToString();
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.MedicationPurpose = txtPurposeOfMedication.Text;
            if (entity.DosingDuration != 0)
                entity.EndDate = entity.StartDate.AddDays(Convert.ToDouble(entity.DosingDuration));
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PastMedication entity = new PastMedication();
                ControlToEntity(entity);
                entity.MRN = AppSession.RegisteredPatient.MRN;
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPastMedication(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                PastMedication entity = BusinessLayer.GetPastMedication(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePastMedication(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveQuickEntryRecord(string quickEntryText, ref string errMessage)
        {
            try
            {
                string[] param = quickEntryText.Split(';');
                PastMedication entity = new PastMedication();

                int itemID = 0;
                bool isNum = Int32.TryParse(param[0], out itemID);
                if (isNum)
                {
                    entity.DrugName = "";
                    entity.ItemID = itemID;
                }
                else
                {
                    entity.ItemID = null;
                    entity.DrugName = param[0];
                }
                entity.Dose = Convert.ToDecimal(param[1]);
                entity.GCDoseUnit = param[2];
                entity.StartDate = Helper.DateInStringToDateTime(param[4]);
                entity.GCRoute = Constant.MedicationRoute.ORAL;

                int signaID = 0;
                isNum = Int32.TryParse(param[3], out signaID);
                if (!isNum)
                    throw new Exception("Signa Is Not Valid");
                Signa entitySigna = BusinessLayer.GetSigna(signaID);
                entity.GCDosingFrequency = entitySigna.GCDosingFrequency;
                entity.Frequency = entitySigna.Frequency;
                entity.NumberOfDosage = entitySigna.Dose;
                entity.GCDosingUnit = entitySigna.GCDoseUnit;
                entity.GCDrugForm = entitySigna.GCDrugForm;
                
                if (param.Count() > 5 && param[5] != "")
                    entity.DosingDuration = Convert.ToDecimal(param[5]);
                else
                    entity.DosingDuration = 0;

                if (entity.DosingDuration != 0)
                    entity.EndDate = entity.StartDate.AddDays(Convert.ToDouble(entity.DosingDuration));

                //entity.DrugName = BusinessLayer.GetItemMaster((int)entity.ItemID).ItemName1;

                entity.MRN = AppSession.RegisteredPatient.MRN;
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPastMedication(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion
    }
}