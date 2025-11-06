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
    public partial class PatientBillSummaryRecalculationBillProcessCtl2 : BasePatientManagementRecalculationBillPackagePage
    {
        private const string SESSION_LIST_CHARGES_DT = "RecalculateListChargesDtPackage";

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                IsAdd = false;

                hdnRegistrationID.Value = param;
                Registration entity = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
                hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();

                string itemName = "";
                int i = 1;
                string filterPackage = string.Format("RegistrationID = '{0}' AND IsDeleted = 0", entity.RegistrationID);
                List<vConsultVisitItemPackage1> lstPackage = BusinessLayer.GetvConsultVisitItemPackage1List(filterPackage);
                foreach (vConsultVisitItemPackage1 l in lstPackage)
                {
                    if (String.IsNullOrEmpty(itemName))
                    {
                        if (l.IsUsingAccumulatedPrice)
                        {
                            itemName = string.Format("{0}. {1} (Akumulasi Harga)", i, l.ItemName1);
                        }
                        else
                        {
                            itemName = string.Format("{0}. {1}", i, l.ItemName1);
                        }
                    }
                    else
                    {
                        if (l.IsUsingAccumulatedPrice)
                        {
                            itemName += string.Format("\n{0}. {1} (Akumulasi Harga)", i, l.ItemName1);
                        }
                        else
                        {
                            itemName += string.Format("\n{0}. {1}", i, l.ItemName1);
                        }
                    }
                    i += 1;
                }
                txtItemName1.Text = itemName;

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
                ListPatientChargesDt = BusinessLayer.GetvPatientChargesDt8List(filterExpression, int.MaxValue, 1, "ChargesDepartmentID, ChargesServiceUnitName, TransactionNo, ItemName1");

                BindGrid();
            }
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";

            filterExpression = string.Format("(RegistrationID = {0} OR (LinkedToRegistrationID = {0} AND IsChargesTransfered = 1)) AND IsAIOTransaction = 0", hdnRegistrationID.Value);

            filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}') AND IsDeleted = 0", Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);

            return filterExpression;
        }

        protected override void BindGrid()
        {
            #region OLD
            //if (cboRecalculateType.Value.ToString() == "0") // Transaction Date
            //{
            //    #region TransactionDate

            //    DateTime dateFrom = Helper.GetDatePickerValue(txtTransactionDateFrom.Text);
            //    DateTime dateTo = Helper.GetDatePickerValue(txtTransactionDateTo.Text);

            //    List<vPatientChargesDt> lstService = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE && (p.TransactionDate >= dateFrom && p.TransactionDate <= dateTo)).ToList();
            //    ((TransactionDtServiceViewCtl)ctlService).BindGrid(lstService);

            //    List<vPatientChargesDt> lstDrugMS = ListPatientChargesDt.Where(p => (p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES) && (p.TransactionDate >= dateFrom && p.TransactionDate <= dateTo)).ToList();
            //    ((TransactionDtProductViewCtl)ctlDrugMS).BindGrid(lstDrugMS);

            //    List<vPatientChargesDt> lstLogistic = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC && (p.TransactionDate >= dateFrom && p.TransactionDate <= dateTo)).ToList();
            //    ((TransactionDtProductViewCtl)ctlLogistic).BindGrid(lstLogistic);

            //    List<vPatientChargesDt> lstLaboratory = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY && (p.TransactionDate >= dateFrom && p.TransactionDate <= dateTo)).ToList();
            //    ((TransactionDtServiceViewCtl)ctlLaboratory).BindGrid(lstLaboratory);

            //    List<vPatientChargesDt> lstImaging = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY && (p.TransactionDate >= dateFrom && p.TransactionDate <= dateTo)).ToList();
            //    ((TransactionDtServiceViewCtl)ctlImaging).BindGrid(lstImaging);

            //    List<vPatientChargesDt> lstMedicalDiagnostic = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC && (p.TransactionDate >= dateFrom && p.TransactionDate <= dateTo)).ToList();
            //    ((TransactionDtServiceViewCtl)ctlMedicalDiagnostic).BindGrid(lstMedicalDiagnostic);

            //    #endregion
            //}
            //else if (cboRecalculateType.Value.ToString() == "1") // Charge Class
            //{
            //    #region ChargeClass

            //    int classID = Convert.ToInt32(cboChargesClass.Value);

            //    List<vPatientChargesDt> lstService = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE && (classID == 0 ? p.ChargeClassID != classID : p.ChargeClassID == classID)).ToList();
            //    ((TransactionDtServiceViewCtl)ctlService).BindGrid(lstService);

            //    List<vPatientChargesDt> lstDrugMS = ListPatientChargesDt.Where(p => (p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES) && (classID == 0 ? p.ChargeClassID != classID : p.ChargeClassID == classID)).ToList();
            //    ((TransactionDtProductViewCtl)ctlDrugMS).BindGrid(lstDrugMS);

            //    List<vPatientChargesDt> lstLogistic = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC && (classID == 0 ? p.ChargeClassID != classID : p.ChargeClassID == classID)).ToList();
            //    ((TransactionDtProductViewCtl)ctlLogistic).BindGrid(lstLogistic);

            //    List<vPatientChargesDt> lstLaboratory = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY && (classID == 0 ? p.ChargeClassID != classID : p.ChargeClassID == classID)).ToList();
            //    ((TransactionDtServiceViewCtl)ctlLaboratory).BindGrid(lstLaboratory);

            //    List<vPatientChargesDt> lstImaging = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY && (classID == 0 ? p.ChargeClassID != classID : p.ChargeClassID == classID)).ToList();
            //    ((TransactionDtServiceViewCtl)ctlImaging).BindGrid(lstImaging);

            //    List<vPatientChargesDt> lstMedicalDiagnostic = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC && (classID == 0 ? p.ChargeClassID != classID : p.ChargeClassID == classID)).ToList();
            //    ((TransactionDtServiceViewCtl)ctlMedicalDiagnostic).BindGrid(lstMedicalDiagnostic);

            //    #endregion
            //}
            //else if (cboRecalculateType.Value.ToString() == "2") // Service Unit
            //{
            //    #region ServiceUnit

            //    int hsuID = Convert.ToInt32(cboServiceUnit.Value);

            //    List<vPatientChargesDt> lstService = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE && (hsuID == 0 ? p.HealthcareServiceUnitID != hsuID : p.HealthcareServiceUnitID == hsuID)).ToList();
            //    ((TransactionDtServiceViewCtl)ctlService).BindGrid(lstService);

            //    List<vPatientChargesDt> lstDrugMS = ListPatientChargesDt.Where(p => (p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES) && (hsuID == 0 ? p.HealthcareServiceUnitID != hsuID : p.HealthcareServiceUnitID == hsuID)).ToList();
            //    ((TransactionDtProductViewCtl)ctlDrugMS).BindGrid(lstDrugMS);

            //    List<vPatientChargesDt> lstLogistic = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC && (hsuID == 0 ? p.HealthcareServiceUnitID != hsuID : p.HealthcareServiceUnitID == hsuID)).ToList();
            //    ((TransactionDtProductViewCtl)ctlLogistic).BindGrid(lstLogistic);

            //    List<vPatientChargesDt> lstLaboratory = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY && (hsuID == 0 ? p.HealthcareServiceUnitID != hsuID : p.HealthcareServiceUnitID == hsuID)).ToList();
            //    ((TransactionDtServiceViewCtl)ctlLaboratory).BindGrid(lstLaboratory);

            //    List<vPatientChargesDt> lstImaging = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY && (hsuID == 0 ? p.HealthcareServiceUnitID != hsuID : p.HealthcareServiceUnitID == hsuID)).ToList();
            //    ((TransactionDtServiceViewCtl)ctlImaging).BindGrid(lstImaging);

            //    List<vPatientChargesDt> lstMedicalDiagnostic = ListPatientChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC && (hsuID == 0 ? p.HealthcareServiceUnitID != hsuID : p.HealthcareServiceUnitID == hsuID)).ToList();
            //    ((TransactionDtServiceViewCtl)ctlMedicalDiagnostic).BindGrid(lstMedicalDiagnostic);

            //    #endregion
            //}
            #endregion

            if (cboRecalculateType.Value.ToString() == "0") // Transaction Date
            {
                #region TransactionDate

                DateTime dateFrom = Helper.GetDatePickerValue(txtTransactionDateFrom.Text);
                DateTime dateTo = Helper.GetDatePickerValue(txtTransactionDateTo.Text);

                List<vPatientChargesDt8> lst = ListPatientChargesDt.Where(p => (p.TransactionDate >= dateFrom && p.TransactionDate <= dateTo)).ToList();
                lvwRecal.DataSource = lst;
                lvwRecal.DataBind();

                #endregion
            }
            else if (cboRecalculateType.Value.ToString() == "1") // Charge Class
            {
                #region ChargeClass

                int classID = Convert.ToInt32(cboChargesClass.Value);

                List<vPatientChargesDt8> lst = ListPatientChargesDt.Where(p => (classID == 0 ? p.ChargeClassID != classID : p.ChargeClassID == classID)).ToList();
                lvwRecal.DataSource = lst;
                lvwRecal.DataBind();

                #endregion
            }
            else if (cboRecalculateType.Value.ToString() == "2") // Service Unit
            {
                #region ServiceUnit

                int hsuID = Convert.ToInt32(cboServiceUnit.Value);

                List<vPatientChargesDt8> lst = ListPatientChargesDt.Where(p => (hsuID == 0 ? p.HealthcareServiceUnitID != hsuID : p.HealthcareServiceUnitID == hsuID)).ToList();
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

                List<vPatientChargesDt8> lst = ListPatientChargesDt.Where(p => itemType == "" ? p.GCItemType != itemType : p.GCItemType == itemType).ToList();
                lvwRecal.DataSource = lst;
                lvwRecal.DataBind();

                #endregion
            }
            else if (cboRecalculateType.Value.ToString() == "4") // Class
            {
                #region Class

                List<vPatientChargesDt8> lstEntityDt = new List<vPatientChargesDt8>();

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

                List<vPatientChargesDt8> lst = lstEntityDt;
                lvwRecal.DataSource = lst;
                lvwRecal.DataBind();

                #endregion
            }

            txtTotalPayer.Text = ListPatientChargesDt.Sum(p => p.PayerAmount).ToString("N");
            txtTotalPatient.Text = ListPatientChargesDt.Sum(p => p.PatientAmount).ToString("N");
            txtTotal.Text = ListPatientChargesDt.Sum(p => p.LineAmount).ToString("N");
        }

        protected void cbpRecalculationPatientBillProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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


                    OnProcessRecalculation(registrationID, chkIsUsedLastHNA.Checked, chkIsIncludeVariableTariff.Checked, chkIsResetItemTariff.Checked, listParamTemp, paramTo);
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
            int linkedRegistrationID = hdnLinkedRegistrationID.Value == "" ? 0 : Convert.ToInt32(hdnLinkedRegistrationID.Value);

            if (hdnParam.Value != "")
            {
                string[] listParam = hdnParam.Value.Split('|');
                int[] listParamTemp = Array.ConvertAll(listParam, int.Parse);

                return OnSaveRecord(ref errMessage, ref retval, registrationID, linkedRegistrationID, listParamTemp);
            }
            else
            {
                int[] i = new int[0];
                return OnSaveRecord(ref errMessage, ref retval, registrationID, linkedRegistrationID, i);
            }
        }
    }
}