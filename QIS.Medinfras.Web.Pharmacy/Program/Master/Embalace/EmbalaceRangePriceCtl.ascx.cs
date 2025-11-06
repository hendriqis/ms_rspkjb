using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Text;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class EmbalaceRangePriceCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override void InitializeDataControl(string queryString)
        {
            string[] param = queryString.Split('|');
            hdnEmbalaceID.Value = param[0];
            EmbalaceHd entity = BusinessLayer.GetEmbalaceHd(Convert.ToInt32(hdnEmbalaceID.Value));
            EntityToControl(entity);
            BindListView(CurrPage, true, ref PageCount);
        }

        private void BindListView(int pageIndex, bool isCountPageCount, ref int pageCount) 
        {
            String filterExpression = String.Format("EmbalaceID = {0}",hdnEmbalaceID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetEmbalaceDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<EmbalaceDt> lstEntity = BusinessLayer.GetEmbalaceDtList(filterExpression);            
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        //protected override void OnControlEntrySetting()
        //{
        //    SetControlEntrySetting(txtCompoundDrugName, new ControlEntrySetting(true, true, false));
        //    SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, true));
        //    //SetControlEntrySetting(txtPopupDosingDose, new ControlEntrySetting(true, true, true));
        //    //SetControlEntrySetting(cboPopupDosingUnit, new ControlEntrySetting(true, true, true));
        //    SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false));
        //    SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true));
        //    SetControlEntrySetting(cboFrequencyTimeline, new ControlEntrySetting(true, true, true));
        //    SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true));
        //    SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true));
        //    SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true));
        //    SetControlEntrySetting(chkIsUsingSweetener, new ControlEntrySetting(true, true, false));
        //    SetControlEntrySetting(cboPopupDrugForm, new ControlEntrySetting(true, true, true));
        //}

        private void EntityToControl(EmbalaceHd entity)
        {
            txtEmbalaceCode.Text = entity.EmbalaceCode;
            txtEmbalaceName.Text = entity.EmbalaceName;
        }

        private void ControlToEntity(EmbalaceDt entity)
        {
            entity.EmbalaceID = Convert.ToInt32(hdnEmbalaceID.Value);
            entity.StartingQty = Convert.ToInt32(txtStartingQty.Text);
            entity.EndingQty = Convert.ToInt32(txtEndingQty.Text);
            entity.Tariff = Convert.ToDecimal(txtTariff.Text);
        }

        private void SetControlProperties()
        {
            //String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT,Constant.StandardCode.COENAM_RULE);
            //List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            //lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            //Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField<StandardCode>(cboPopupDrugForm, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT).ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            
            //Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            //List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList(string.Format("IsDeleted = 0"));
            //Methods.SetComboBoxField<ClassCare>(cboPopupChargeClass, lstClassCare, "ClassName", "ClassID");
            //cboPopupChargeClass.Value = 1;
            //cboFrequencyTimeline.Value = Constant.DosingFrequency.DAY;

            //txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            //txtStartTime.Text = DateTime.Now.ToString("HH:mm");

            
            //txtDispenseQty.Text = "1";
        }

        protected void cboCompoundUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            //List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN ( (SELECT GCDoseUnit FROM vDrugInfo WHERE ItemID = {1}),(SELECT GCItemUnit FROM vDrugInfo WHERE ItemID = {2})))", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value,hdnItemID.Value));
            //Methods.SetComboBoxField<StandardCode>(cboCompoundUnit, lst, "StandardCodeName", "StandardCodeID");
            //cboCompoundUnit.SelectedIndex = -1;
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (OnSaveAddEditRecord(ref errMessage))
                {
                    result += "success";
                    BindListView(CurrPage, true, ref PageCount);
                }
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (e.Parameter == "delete")
            {
                result = "delete|";
                if (OnSaveDeleteRecord(ref errMessage))
                {
                    result += "success";
                    BindListView(CurrPage, true, ref PageCount);
                }
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected bool OnSaveDeleteRecord(ref string errMessage) 
        {
            bool result = true;

            try
            {
                BusinessLayer.DeleteEmbalaceDt(Convert.ToInt32(hdnEmbalaceID.Value),
                    Convert.ToInt32(txtStartingQty.Text), Convert.ToInt32(txtEndingQty.Text));
            }
            catch (Exception ex) 
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        protected bool OnSaveAddEditRecord(ref string errMessage)
        {
            bool result = true;

            try
            {
                EmbalaceDt entity = new EmbalaceDt();
                ControlToEntity(entity);
                if (hdnIsAdd.Value == "1")
                    BusinessLayer.InsertEmbalaceDt(entity);
                else
                    BusinessLayer.UpdateEmbalaceDt(entity);
            }
            catch (Exception ex) 
            {
                result = false;
                errMessage = ex.Message;
            }

            return result;
        }
    }
}