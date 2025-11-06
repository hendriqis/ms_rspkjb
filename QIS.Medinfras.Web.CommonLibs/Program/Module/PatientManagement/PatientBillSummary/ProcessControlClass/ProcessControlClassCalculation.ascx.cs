using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ProcessControlClassCalculation : BasePatientManagementRecalculationControlClassBillPage
    {
        private const string SESSION_LIST_CHARGES_DT = "CalculateListChargesControlClass";

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                IsAdd = false;

                hdnRegistrationID.Value = param;
                vRegistration entity = BusinessLayer.GetvRegistrationList(String.Format("RegistrationID = {0}", hdnRegistrationID.Value)).FirstOrDefault();
                hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();

                List<Variable> lstRecalType = new List<Variable>();
                lstRecalType.Add(new Variable { Code = "0", Value = "Transaction Date" });
                lstRecalType.Add(new Variable { Code = "1", Value = "Charge Class" });
                lstRecalType.Add(new Variable { Code = "2", Value = "Service Unit" });
                lstRecalType.Add(new Variable { Code = "3", Value = "Item Type" });
                lstRecalType.Add(new Variable { Code = "4", Value = "Class" });
                Methods.SetComboBoxField<Variable>(cboRecalculateType, lstRecalType, "Value", "Code");
                cboRecalculateType.Value = "0";

                txtTransactionDateFrom.Text = entity.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTransactionDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                string filterCC = string.Format("ClassID IN (SELECT ChargeClassID FROM vPatientChargesDt WHERE RegistrationID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}')",
                                                    param, Constant.TransactionStatus.VOID);
                List<ClassCare> lstCC = BusinessLayer.GetClassCareList(filterCC);
                lstCC.Insert(0, new ClassCare { ClassName = "", ClassID = 0 });
                Methods.SetComboBoxField<ClassCare>(cboChargesClass, lstCC, "ClassName", "ClassID");
                cboChargesClass.SelectedIndex = 0;

                string filterSU = string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vPatientChargesHd WHERE RegistrationID = {0} AND ISNULL(GCTransactionStatus,'') != '{1}')",
                                                    param, Constant.TransactionStatus.VOID);
                List<vHealthcareServiceUnit> lstSU = BusinessLayer.GetvHealthcareServiceUnitList(filterSU);
                lstSU.Insert(0, new vHealthcareServiceUnit { ServiceUnitName = "", HealthcareServiceUnitID = 0 });
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstSU, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                string filterIT = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.ITEM_TYPE);
                List<StandardCode> lstIT = BusinessLayer.GetStandardCodeList(filterIT);
                lstIT.Insert(0, new StandardCode { StandardCodeName = "", StandardCodeID = "" });
                Methods.SetComboBoxField<StandardCode>(cboItemType, lstIT, "StandardCodeName", "StandardCodeID");
                cboItemType.SelectedIndex = 0;

                string filterC = string.Format("IsDeleted = 0 AND IsUsedInChargeClass = 0");
                List<ClassCare> lstC = BusinessLayer.GetClassCareList(filterC);
                lstC.Insert(0, new ClassCare { ClassName = "", ClassID = 0 });
                Methods.SetComboBoxField<ClassCare>(cboClass, lstC, "ClassName", "ClassID");
                cboClass.SelectedIndex = 0;

                string filterTCC = string.Format("IsDeleted = 0 AND IsUsedInChargeClass = 1");
                List<ClassCare> lstTCC = BusinessLayer.GetClassCareList(filterTCC);
                lstTCC.Insert(0, new ClassCare { ClassName = "", ClassID = 0 });
                Methods.SetComboBoxField<ClassCare>(cboToChargesClass, lstTCC, "ClassName", "ClassID");
                cboToChargesClass.Value = "";

                String filterExpression = GetFilterExpression();
                ListPatientChargesDt = BusinessLayer.GetvPatientChargesClassCoverageDtList(filterExpression,int.MaxValue,1,"ChargesDepartmentID, ChargesServiceUnitName, TransactionNo, ItemName1");

                BindGrid();
            }
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
            {
                filterExpression = string.Format("(RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value);
            }
            else
            {
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            }

            filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}') AND IsDeleted = 0", Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);

            return filterExpression;
        }

        protected override void BindGrid()
        {
            if (cboRecalculateType.Value.ToString() == "0") // Transaction Date
            {
                #region TransactionDate

                DateTime dateFrom = Helper.GetDatePickerValue(txtTransactionDateFrom.Text);
                DateTime dateTo = Helper.GetDatePickerValue(txtTransactionDateTo.Text);

                List<vPatientChargesClassCoverageDt> lst = ListPatientChargesDt.Where(p => (p.TransactionDate >= dateFrom && p.TransactionDate <= dateTo)).ToList();
                lvwRecal.DataSource = lst;
                lvwRecal.DataBind();

                #endregion
            }
            else if (cboRecalculateType.Value.ToString() == "1") // Charge Class
            {
                #region ChargeClass

                int classID = Convert.ToInt32(cboChargesClass.Value);

                List<vPatientChargesClassCoverageDt> lst = ListPatientChargesDt.Where(p => (classID == 0 ? p.ChargeClassID != classID : p.ChargeClassID == classID)).ToList();
                lvwRecal.DataSource = lst;
                lvwRecal.DataBind();

                #endregion
            }
            else if (cboRecalculateType.Value.ToString() == "2") // Service Unit
            {
                #region ServiceUnit

                int hsuID = Convert.ToInt32(cboServiceUnit.Value);

                List<vPatientChargesClassCoverageDt> lst = ListPatientChargesDt.Where(p => (hsuID == 0 ? p.HealthcareServiceUnitID != hsuID : p.HealthcareServiceUnitID == hsuID)).ToList();
                lvwRecal.DataSource = lst;
                lvwRecal.DataBind();

                #endregion
            }
            else if (cboRecalculateType.Value.ToString() == "3") // Item Type
            {
                #region ItemType

                string itemType = "";

                if (cboItemType.Value != null)
                {
                    itemType = cboItemType.Value.ToString();
                }

                List<vPatientChargesClassCoverageDt> lst = ListPatientChargesDt.Where(p => itemType == "" ? p.GCItemType != itemType : p.GCItemType == itemType).ToList();
                lvwRecal.DataSource = lst;
                lvwRecal.DataBind();

                #endregion
            }
            else if (cboRecalculateType.Value.ToString() == "4") // Class
            {
                #region Class

                List<vPatientChargesClassCoverageDt> lstEntityDt = new List<vPatientChargesClassCoverageDt>();

                int classID = Convert.ToInt32(cboClass.Value);

                if (classID != 0 && classID != null)
                {
                    for (int i = 0; i < ListPatientChargesDt.Count(); i++)
                    {
                        PatientChargesDtInfo dtInfo = BusinessLayer.GetPatientChargesDtInfo(ListPatientChargesDt[i].ID);
                        if (dtInfo.ClassID == classID)
                        {
                            lstEntityDt.Add(ListPatientChargesDt[i]);
                        }
                    }
                }
                else
                {
                    lstEntityDt = ListPatientChargesDt;
                }

                List<vPatientChargesClassCoverageDt> lst = lstEntityDt;
                lvwRecal.DataSource = lst;
                lvwRecal.DataBind();

                #endregion
            }

            txtTotalPayer.Text = ListPatientChargesDt.Sum(p => p.PayerAmount).ToString("N");
            txtTotalPatient.Text = ListPatientChargesDt.Sum(p => p.PatientAmount).ToString("N");
            txtTotal.Text = ListPatientChargesDt.Sum(p => p.LineAmount).ToString("N");
        }

        protected void cbpProcessControlClass_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "recal")
            {
                #region Recalculation
                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                if (hdnParam.Value != "")
                {
                    string[] listParam = hdnParam.Value.Split('|');
                    int[] listParamTemp = Array.ConvertAll(listParam, int.Parse);

                    string tempToClassID = hdnToClassID.Value == "" ? "0" : hdnToClassID.Value;
                    string tempToClassName = hdnToClassName.Value == "" ? "-" : hdnToClassName.Value;
                    string paramTo = tempToClassID + ";" + tempToClassName;


                    OnProcessRecalculationControlClass(registrationID, chkIsIncludeVariableTariff.Checked, chkIsResetItemTariff.Checked, listParamTemp, paramTo);
                    result += "success";
                }
                #endregion
            }
            else
            {
                BindGrid();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            return OnSaveRecord(ref errMessage, ref retval, registrationID, Convert.ToInt32(hdnLinkedRegistrationID.Value));
        }
    }
}