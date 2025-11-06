using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class NutritionDistributionLink: BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.NUTRITION_DISTRIBUTION;
            
        }

        protected String GetItemDetailStatus()
        {
            return Constant.TransactionStatus.PROCESSED;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowEdit = false;
            base.SetCRUDMode(ref IsAllowAdd, ref IsAllowEdit, ref IsAllowDelete);
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.MEAL_TIME));
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboMealTime, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboMealTime.SelectedIndex = 1;



            List<vServiceUnitLink> lstServiceUnit = BusinessLayer.GetvServiceUnitLinkList("IsDeleted = 0");
            lstServiceUnit.Insert(0, new vServiceUnitLink { ServiceUnitName = "", ServiceUnitCode = "" });
            Methods.SetComboBoxField<vServiceUnitLink>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "ServiceUnitCode");
            cboServiceUnit.SelectedIndex = 1;

            List<vInpatientClassLink> lstClass = BusinessLayer.GetvInpatientClassLinkList("IsDeleted = 0");
            lstClass.Insert(0, new vInpatientClassLink { ClassName = "", ClassCode = "" });
            Methods.SetComboBoxField<vInpatientClassLink>(cboClass, lstClass, "ClassName", "ClassCode");
          


            txtDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            BindGridView();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();
            filterExpression += " ORDER BY ServiceUnitCode, BedCode ";
            List<vNutritionOrderDtCustom> lstEntity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression);
            hdnLstNutritionOrderDtID.Value = String.Join(",", lstEntity.Select(p => p.NutritionOrderDtID).ToList());
            hdnLstNutritionOrderHdID.Value = String.Join(",", lstEntity.Select(p => p.NutritionOrderHdID).ToList());
            grdView.DataSource = lstEntity;
            grdView.DataBind();

        }

        private void UpdateNutritionOrder()
        {
            string filterExpression = String.Format("NutritionOrderDate = '{0}' AND GCTransactionStatus = '{1}'", DateTime.Now.ToString("yyyyMMdd"), Constant.TransactionStatus.CLOSED);
            if (cboServiceUnit.Value != null && cboServiceUnit.Value.ToString() != "") filterExpression += String.Format(" AND LinkField LIKE '%|{0}'", cboServiceUnit.Value);
            if (cboMealTime.Value != null && cboMealTime.Value.ToString() != "") filterExpression += String.Format(" AND GCMealTime = '{0}'", cboMealTime.Value);
            if (cboClass.Value != null && cboClass.Value.ToString() != "") filterExpression += String.Format(" AND LinkField LIKE'%|{0}'", cboClass.Value);
            List<vNutritionOrderDtCustom> lstEntity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression);
            foreach (vNutritionOrderDtCustom obj in lstEntity)
            {
                NutritionOrderDt entity = new NutritionOrderDt();
                entity.IsReadyToPrint = true;
                entity.LastPrintedDate = DateTime.Now;
                entity.LastPrintedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNutritionOrderDt(entity);
            }
            
        }

        private void PrintSlipAll() 
        {
            string filterExpression = String.Format("NutritionOrderDate = '{0}' AND GCTransactionStatus = '{1}'", DateTime.Now.ToString("yyyyMMdd"), Constant.TransactionStatus.CLOSED);
            //if (Convert.ToInt32(cboServiceUnit.Value) != 0) filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            if (cboServiceUnit.Value != null && cboServiceUnit.Value.ToString() != "") filterExpression += String.Format(" AND LinkField LIKE '%|{0}|%'", cboServiceUnit.Value);
            if (cboMealTime.Value != null && cboMealTime.Value.ToString() != "") filterExpression += String.Format(" AND GCMealTime = '{0}'", cboMealTime.Value);
            if (cboClass.Value != null && cboClass.Value.ToString() != "") filterExpression += String.Format(" AND LinkField LIKE'%|{0}'", cboClass.Value);
            List<vNutritionOrderDtCustom> lstEntity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression);
            foreach (vNutritionOrderDtCustom obj in lstEntity) 
            {
                NutritionOrderDt entity = BusinessLayer.GetNutritionOrderDt(obj.NutritionOrderDtID);
                entity.IsReadyToPrint = true;
                entity.LastPrintedDate = DateTime.Now;
                entity.LastPrintedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNutritionOrderDt(entity);
            }
        }

        private void PrintSlip()
        {
            NutritionOrderDt entity = BusinessLayer.GetNutritionOrderDt(Convert.ToInt32(hdnNutritionOrderDtID.Value));
            entity.IsReadyToPrint = true;
            entity.LastPrintedDate = DateTime.Now;
            entity.LastPrintedBy = AppSession.UserLogin.UserID;
            BusinessLayer.UpdateNutritionOrderDt(entity);
        }

        private string GetFilterExpression()
        {
            String filterExpression = String.Format("NutritionOrderDate = '{0}' AND GCTransactionStatus NOT IN ('{1}','{2}') AND GCItemDetailStatus IN ('{3}','{4}') ", DateTime.Now.ToString("yyyyMMdd"), Constant.TransactionStatus.VOID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.PROCESSED);
            //if (Convert.ToInt32(cboServiceUnit.Value) != 0) filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}",cboServiceUnit.Value);
            if (cboServiceUnit.Value != null &&  cboServiceUnit.Value.ToString() != "") filterExpression += String.Format(" AND LinkField LIKE '%|{0}|%'", cboServiceUnit.Value);
            if (cboMealTime.Value != null && cboMealTime.Value.ToString() != "") filterExpression += String.Format(" AND GCMealTime = '{0}'", cboMealTime.Value);
            if (cboClass.Value != null && cboClass.Value.ToString() != "") filterExpression += String.Format(" AND LinkField LIKE'%|{0}'", cboClass.Value);
            
            return filterExpression;
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (param[1] == "single")
                {
                    if (ChangeWorkListStatus(ref errMessage)) result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (ChangeAllWorkListStatus(ref errMessage)) result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }   
            }
            else if(param[0] == "print")
            {
                if (param[1] == "single")
                {
                    PrintSlip();                    
                }
                else 
                {
                    PrintSlipAll();
                }
            }
            
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public bool ChangeWorkListStatus(ref string errMessage) 
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
            try
            {
                NutritionOrderDt entity = entityDtDao.Get(Convert.ToInt32(hdnNutritionOrderDtID.Value));
                entity.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                entityDtDao.Update(entity);
                
                string filterExpression = String.Format("NutritionOrderHdId = {0} AND GCItemDetailStatus NOT IN ('{1}','{2}')", hdnNutritionOrderHdID.Value,Constant.TransactionStatus.CLOSED,Constant.TransactionStatus.VOID);
                int count = BusinessLayer.GetNutritionOrderDtRowCount(filterExpression, ctx);
                
                if (count == 0) 
                {
                    NutritionOrderHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnNutritionOrderHdID.Value));
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                    entityHdDao.Update(entityHd);
                }
                ctx.CommitTransaction();
            }
            catch(Exception ex)
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

        public bool ChangeAllWorkListStatus(ref string errMessage) 
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            NutritionOrderHdDao entityHdDao = new NutritionOrderHdDao(ctx);
            
            try
            {
                String filterExpression = String.Format("NutritionOrderDtID IN ({0})", hdnLstNutritionOrderDtID.Value);
                if (cboMealTime.Value != null && cboMealTime.Value.ToString() != "") filterExpression += String.Format(" AND GCMealTime = '{0}'", cboMealTime.Value);
                List<NutritionOrderDt> lstEntity = BusinessLayer.GetNutritionOrderDtList(filterExpression, ctx);
                foreach (NutritionOrderDt obj in lstEntity)
                {
                    if (obj.GCItemDetailStatus == Constant.TransactionStatus.PROCESSED) 
                    {
                        obj.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                        obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(obj);
                    }
                }

                filterExpression = String.Format("NutritionOrderHdID IN ({0})", hdnLstNutritionOrderHdID.Value);
                if (cboServiceUnit.Value != null && cboServiceUnit.Value.ToString() != "") filterExpression += String.Format(" AND LinkField LIKE '%|{0}|%'", cboServiceUnit.Value);
                List<NutritionOrderHd> lstEntityHd = BusinessLayer.GetNutritionOrderHdList(filterExpression, ctx);
                foreach(NutritionOrderHd obj in lstEntityHd)
                {
                    filterExpression = String.Format("NutritionOrderHdId = {0} AND GCItemDetailStatus NOT IN ('{1}','{2}')", obj.NutritionOrderHdID, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
                    int count = BusinessLayer.GetNutritionOrderDtRowCount(filterExpression, ctx);

                    if (count == 0)
                    {
                        obj.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                        entityHdDao.Update(obj);
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