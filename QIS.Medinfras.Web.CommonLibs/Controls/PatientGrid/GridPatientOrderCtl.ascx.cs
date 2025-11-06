using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class GridPatientOrderCtl : System.Web.UI.UserControl
    {
        protected int PageCount = 1;
        public void InitializeControl()
        {
            SettingParameterDt setvarCito = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.CITO_PEMERIKSAAN_TAMPIL_PALING_ATAS);
            hdnSetvarCito.Value = setvarCito.ParameterValue;

            BindGridView(1, true, ref PageCount);
        }

        protected void cbpViewOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ((BasePagePatientOrder)Page).LoadAllWords();
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ((BasePagePatientOrder)Page).GetFilterExpressionTestOrder();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHdVisitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }
            if (hdnSetvarCito.Value == "0")
            {
                List<vTestOrderHdVisit> lstEntity = BusinessLayer.GetvTestOrderHdVisitList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "TestOrderID ASC");
                lvwViewOrder.DataSource = lstEntity;
                lvwViewOrder.DataBind();
            }
            else
            {
                List<vTestOrderHdVisit> lstEntity = BusinessLayer.GetvTestOrderHdVisitList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "IsCito DESC,TestOrderID ASC");
                lvwViewOrder.DataSource = lstEntity;
                lvwViewOrder.DataBind();
            }
        }

        protected void lvwViewOrder_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                //vTestOrderHdVisit entity = (vTestOrderHdVisit)e.Item.DataItem;
                //HtmlGenericControl spnProcessed = e.Item.FindControl("spnProcessed") as HtmlGenericControl;
                //if(entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                //    spnProcessed.Style.Add("display", "none");
            }
        }

        protected string GetLabel(string code)
        {
            return ((BasePagePatientOrder)Page).GetLabel(code);
        }

        protected void btnOpenTransactionDtOrder_Click(object sender, EventArgs e)
        {
            if (hdnTransactionOrderNo.Value != "")
            {
                vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", hdnTransactionOrderNo.Value)).FirstOrDefault();
                SetSessionRegisteredPatient(entity);
                ((BasePagePatientOrder)Page).OnGrdRowClickTestOrder(hdnTransactionOrderNo.Value, hdnTestOrderNo.Value);
            }
        }

        private void SetSessionRegisteredPatient(vConsultVisit4 entity)
        {
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.RegistrationID = entity.RegistrationID;
            pt.RegistrationNo = entity.RegistrationNo;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.ServiceUnitName = entity.ServiceUnitName;
            pt.RoomCode = entity.RoomCode;
            pt.BedCode = entity.BedCode;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            pt.ChargeClassID = entity.ChargeClassID;
            pt.DateOfBirth = entity.DateOfBirth;
            pt.BusinessPartnerID = entity.BusinessPartnerID;
            pt.GCCustomerType = entity.GCCustomerType;
            AppSession.RegisteredPatient = pt;
        }
    }
}