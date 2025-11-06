using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class TestOrderItemEditCtl1 : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnTestOrderType.Value = param.Split('|')[0];
            hdnTestOrderID.Value = param.Split('|')[1];
            hdnHealthcareServiceUnitID.Value = param.Split('|')[2];

            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = {0} AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE));
            hdnIsUsingMultiVisitScheduleOrder.Value = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE).FirstOrDefault().ParameterValue;

            List<PatientDiagnosis> lstDiagnose = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID));
            Methods.SetComboBoxField<PatientDiagnosis>(cboDiagnose, lstDiagnose, "DiagnosisText", "DiagnoseID");

            string filterExpression = string.Empty;
            if (!string.IsNullOrEmpty(hdnHealthcareServiceUnitID.Value) && hdnHealthcareServiceUnitID.Value != "0")
            {
                filterExpression = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", hdnHealthcareServiceUnitID.Value);
            }

            List<vServiceUnitItem> lstItem = BusinessLayer.GetvServiceUnitItemList(filterExpression);
            Methods.SetComboBoxField<vServiceUnitItem>(cboServiceUnitItem, lstItem, "ItemName1", "ItemID");

            TestOrderHd orderHD = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
            if (orderHD != null)
            {
                txtOrderNo.Text = orderHD.TestOrderNo;
                chkIsPathologicalAnatomyTest.Checked = orderHD.IsPathologicalAnatomyTest;
                if (hdnIsUsingMultiVisitScheduleOrder.Value == "1")
                {
                    vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", orderHD.HealthcareServiceUnitID)).FirstOrDefault();
                    if (hsu != null)
                    {
                        if (hsu.IsAllowMultiVisitSchedule)
                        {
                            chkIsMultiVisitScheduleOrder.Checked = orderHD.IsMultiVisitScheduleOrder;
                            trMultiVisitScheduleOrder.Attributes.Remove("style");
                        }
                        else
                        {
                            trMultiVisitScheduleOrder.Attributes.Add("style", "display:none");
                        }
                    }
                    else
                    {
                        trMultiVisitScheduleOrder.Attributes.Add("style", "display:none");
                    }
                }
                else
                {
                    trMultiVisitScheduleOrder.Attributes.Add("style", "display:none");
                }
            }

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnTestOrderID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderDt> lstEntity = BusinessLayer.GetvTestOrderDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpViewPopUpCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }
            else if (param[0] == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            else if (param[0] == "saveHeader")
            {
                if (hdnTestOrderID.Value.ToString() != "")
                {
                    if (OnSaveEditHeaderRecord(ref errMessage))
                    {
                        result += "success";                        
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                {
                    result += "success";
                    BindGridView(1, true, ref pageCount);
                }
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(TestOrderDt entity)
        {
            entity.ItemID = Convert.ToInt32(cboServiceUnitItem.Value);
            entity.ItemQty = Convert.ToDecimal(txtItemQty.Text);
            if (cboDiagnose.Value != null && cboDiagnose.Value.ToString() != "")
            {
                entity.DiagnoseID = cboDiagnose.Value.ToString();
            }
            entity.IsCITO = chkIsCITO.Checked;
            entity.Remarks = txtRemarks.Text;
        }

        private string GetItemFilterExpression()
        {
            if (!string.IsNullOrEmpty(hdnHealthcareServiceUnitID.Value) && hdnHealthcareServiceUnitID.Value != "0")
            {
                return string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", hdnHealthcareServiceUnitID.Value);
            }
            else
            {
                return "IsDeleted = 0";
            }
        }

        private bool OnSaveEditHeaderRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            try
            {
                TestOrderHd entity = entityHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));
                entity.IsPathologicalAnatomyTest = chkIsPathologicalAnatomyTest.Checked;
                entity.IsMultiVisitScheduleOrder = chkIsMultiVisitScheduleOrder.Checked;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(entity);
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
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                TestOrderDt entity = entityDtDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);
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
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtDao entityDtDao = new TestOrderDtDao(ctx);
            try
            {
                TestOrderDt entity = entityDtDao.Get(Convert.ToInt32(hdnID.Value));
                entity.GCTestOrderStatus = Constant.OrderStatus.CANCELLED;
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);
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
            return result;
        }
    }
}