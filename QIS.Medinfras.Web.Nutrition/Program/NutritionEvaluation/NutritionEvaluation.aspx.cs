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
using DevExpress.Web.ASPxEditors;
using System.Text;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class NutritionEvaluation : BasePageTrx
    {

        private List<StandardCode> lstEvaluation;
        
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.NUTRITION_EVALUATION;
            
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
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

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.MEAL_TIME));
            Methods.SetComboBoxField<StandardCode>(cboMealTime, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboMealTime.SelectedIndex = 0;

            lstEvaluation = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.MEAL_EVALUATION));
            lstEvaluation.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.NT_ORDER_MEAL_NOT_ONLY_INPATIENT);
            vSettingParameterDt lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp).FirstOrDefault();
            List<GetServiceUnitUserList> lstServiceUnit = new List<GetServiceUnitUserList>();
            if (lstParam.ParameterValue == "1")
            {
                lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Empty, string.Format("DepartmentID IN ('{0}','{1}','{2}')", Constant.Facility.INPATIENT, Constant.Facility.OUTPATIENT, Constant.Facility.EMERGENCY));
            }
            else
            {
                lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.INPATIENT, "");
            }
            lstServiceUnit.Insert(0, new GetServiceUnitUserList { ServiceUnitName = "", HealthcareServiceUnitID = 0 });
            Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

           
            txtDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            BindGridView();

        }
       
        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNutritionOrderDtCustom entity = e.Row.DataItem as vNutritionOrderDtCustom;

                
                ASPxComboBox cboHA = (ASPxComboBox)e.Row.FindControl("cboHA");
                ASPxComboBox cboLH = (ASPxComboBox)e.Row.FindControl("cboLH");
                ASPxComboBox cboLN = (ASPxComboBox)e.Row.FindControl("cboLN");
                ASPxComboBox cboSY = (ASPxComboBox)e.Row.FindControl("cboSY");
                ASPxComboBox cboBH = (ASPxComboBox)e.Row.FindControl("cboBH");
                ASPxComboBox cboFluid = (ASPxComboBox)e.Row.FindControl("cboFluid");
                ASPxComboBox cboSnack = (ASPxComboBox)e.Row.FindControl("cboSnack");
                ASPxComboBox cboDessert = (ASPxComboBox)e.Row.FindControl("cboDessert");

                cboHA.ClientInstanceName = string.Format("cboHA{0}", e.Row.DataItemIndex);
                cboLH.ClientInstanceName = string.Format("cboLH{0}", e.Row.DataItemIndex);
                cboLN.ClientInstanceName = string.Format("cboLN{0}", e.Row.DataItemIndex);
                cboSY.ClientInstanceName = string.Format("cboSY{0}", e.Row.DataItemIndex);
                cboBH.ClientInstanceName = string.Format("cboBH{0}", e.Row.DataItemIndex);
                cboFluid.ClientInstanceName = string.Format("cboFluid{0}", e.Row.DataItemIndex);
                cboSnack.ClientInstanceName = string.Format("cboSnack{0}", e.Row.DataItemIndex);
                cboDessert.ClientInstanceName = string.Format("cboDessert{0}", e.Row.DataItemIndex);

                if (lstEvaluation == null)
                {
                    lstEvaluation = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.MEAL_EVALUATION));
                    lstEvaluation.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
                }

                if(cboHA.Items.Count == 0)
                    Methods.SetComboBoxField<StandardCode>(cboHA, lstEvaluation, "StandardCodeName", "StandardCodeID");
                if (entity.GCCarbohydrate != "")
                {
                    cboHA.Value = entity.GCCarbohydrate;
                }
                else
                {
                    cboHA.SelectedIndex = 0;
                }
                if (cboLH.Items.Count == 0)
                    Methods.SetComboBoxField<StandardCode>(cboLH, lstEvaluation, "StandardCodeName", "StandardCodeID");
                if (entity.GCAnimalDish != "")
                {
                    cboLH.Value = entity.GCAnimalDish;
                }
                else
                {
                    cboLH.SelectedIndex = 0;
                }

                if (cboLN.Items.Count == 0)
                    Methods.SetComboBoxField<StandardCode>(cboLN, lstEvaluation, "StandardCodeName", "StandardCodeID");
                if (entity.GCVegetableDish != "")
                {
                    cboLN.Value = entity.GCVegetableDish;
                }
                else
                {
                    cboLN.SelectedIndex = 0;
                }

                if (cboSY.Items.Count == 0)
                    Methods.SetComboBoxField<StandardCode>(cboSY, lstEvaluation, "StandardCodeName", "StandardCodeID");
                if (entity.GCVegetables != "")
                {
                    cboSY.Value = entity.GCVegetables;
                }
                else
                {
                    cboSY.SelectedIndex = 0;
                }

                if (cboBH.Items.Count == 0)
                    Methods.SetComboBoxField<StandardCode>(cboBH, lstEvaluation, "StandardCodeName", "StandardCodeID");
                if (entity.GCFruits != "")
                {
                    cboBH.Value = entity.GCFruits;
                }
                else
                {
                    cboBH.SelectedIndex = 0;
                }

                if (cboFluid.Items.Count == 0)
                    Methods.SetComboBoxField<StandardCode>(cboFluid, lstEvaluation, "StandardCodeName", "StandardCodeID");
                if (entity.GCFluid != "")
                {
                    cboFluid.Value = entity.GCFluid;
                }
                else
                {
                    cboFluid.SelectedIndex = 0;
                }

                if (cboSnack.Items.Count == 0)
                    Methods.SetComboBoxField<StandardCode>(cboSnack, lstEvaluation, "StandardCodeName", "StandardCodeID");
                if (entity.GCSnack != "")
                {
                    cboSnack.Value = entity.GCSnack;
                }
                else
                {
                    cboSnack.SelectedIndex = 0;
                }

                if (cboDessert.Items.Count == 0)
                    Methods.SetComboBoxField<StandardCode>(cboDessert, lstEvaluation, "StandardCodeName", "StandardCodeID");
                if (entity.GCDessert != "")
                {
                    cboDessert.Value = entity.GCDessert;
                }
                else
                {
                    cboDessert.SelectedIndex = 0;
                }
            }
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

        private string GetFilterExpression()
        {
            String filterExpression = String.Format("ScheduleDate = '{0}' AND GCTransactionStatus NOT IN ('{1}','{2}') AND GCItemDetailStatus IN ('{3}','{4}')", Helper.GetDatePickerValue(txtDate.Text), Constant.TransactionStatus.VOID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.PROCESSED);
            if (Convert.ToInt32(cboServiceUnit.Value) != 0) filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}",cboServiceUnit.Value);
            if (cboMealTime.Value != null && cboMealTime.Value.ToString() != "") filterExpression += String.Format(" AND GCMealTime = '{0}'", cboMealTime.Value);
            
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
                if (SaveEvaluation(param,ref errMessage)) result += "success";
                else
                    result += string.Format("fail|{0}", errMessage); 
            }
           
            
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool SaveEvaluation(string[] param, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NutritionOrderDtDao entityDtDao = new NutritionOrderDtDao(ctx);
            try
            {
                String cboHA = param[1];
                String cboLH = param[2];
                String cboLN = param[3];
                String cboSY = param[4];
                String cboBH = param[5];
                String cboFluid = param[6];
                String cboSnack = param[7];
                String cboDessert = param[8];

                NutritionOrderDt entity = entityDtDao.Get(Convert.ToInt32(hdnNutritionOrderDtID.Value));
                entity.GCCarbohydrate = cboHA;
                entity.GCAnimalDish = cboLH;
                entity.GCVegetableDish = cboLN;
                entity.GCVegetables = cboSY;
                entity.GCFruits = cboBH;
                entity.GCFluid = cboFluid;
                entity.GCSnack = cboSnack;
                entity.GCDessert = cboDessert;
                entity.LastPrintedDate = DateTime.Now;
                entity.LastPrintedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);

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