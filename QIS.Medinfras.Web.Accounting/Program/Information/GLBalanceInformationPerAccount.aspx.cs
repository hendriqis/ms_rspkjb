using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLBalanceInformationPerAccount : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.BALANCE_INFORMATION_PER_ACCOUNT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            #region Data Month
            cboMonth.DataSource = Enumerable.Range(1, 12).Select(a => new
            {
                MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
                MonthNumber = a
            });
            cboMonth.TextField = "MonthName";
            cboMonth.ValueField = "MonthNumber";
            cboMonth.EnableCallbackMode = false;
            cboMonth.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboMonth.DropDownStyle = DropDownStyle.DropDownList;
            cboMonth.DataBind();
            cboMonth.Value = DateTime.Now.Month.ToString();

            cboYear.DataSource = Enumerable.Range(DateTime.Now.Year - 99, 100).Reverse();
            cboYear.EnableCallbackMode = false;
            cboYear.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboYear.DropDownStyle = DropDownStyle.DropDownList;
            cboYear.DataBind();
            cboYear.SelectedIndex = 0;
            #endregion

            List<HealthcareParameter> lstSerVarDt = BusinessLayer.GetHealthcareParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')",
                            Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_DEPARTMENT, Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_SERVICE_UNIT, Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_BUSINESS_PARTNER));
            hdnHealthcare.Value = "001";
            hdnDepartmentID.Value = lstSerVarDt.Where(a => a.ParameterCode == Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_DEPARTMENT).FirstOrDefault().ParameterValue;
            hdnServiceUnitID.Value = lstSerVarDt.Where(a => a.ParameterCode == Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_SERVICE_UNIT).FirstOrDefault().ParameterValue;
            hdnBusinessPartnerID.Value = lstSerVarDt.Where(a => a.ParameterCode == Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_BUSINESS_PARTNER).FirstOrDefault().ParameterValue;

            List<Healthcare> lstH = BusinessLayer.GetHealthcareList("GLAccountNoSegment IS NOT NULL");
            lstH.Insert(0, new Healthcare { HealthcareID = "", HealthcareName = "" });
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, lstH, "HealthcareName", "HealthcareID");
            cboHealthcare.Value = hdnHealthcare.Value;

            //List<Department> lstD = BusinessLayer.GetDepartmentList("GLAccountNoSegment IS NOT NULL AND IsActive = 1");
            //lstD.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
            //Methods.SetComboBoxField<Department>(cboDepartment, lstD, "DepartmentName", "DepartmentID");
            //cboDepartment.Value = hdnDepartment.Value;

            //List<ServiceUnitMaster> lstSU = BusinessLayer.GetServiceUnitMasterList("GLAccountNoSegment IS NOT NULL AND IsDeleted = 0");
            //lstSU.Insert(0, new ServiceUnitMaster { ServiceUnitID = 0, ServiceUnitName = "" });
            //Methods.SetComboBoxField<ServiceUnitMaster>(cboServiceUnit, lstSU, "ServiceUnitName", "ServiceUnitID");
            //cboServiceUnit.Value = hdnServiceUnit.Value;

            //List<BusinessPartners> lstBP = BusinessLayer.GetBusinessPartnersList("GLAccountNoSegment IS NOT NULL AND IsDeleted = 0");
            //lstBP.Insert(0, new BusinessPartners { BusinessPartnerID = 0, BusinessPartnerName = "" });
            //Methods.SetComboBoxField<BusinessPartners>(cboBusinessPartner, lstBP, "BusinessPartnerName", "BusinessPartnerID");
            //cboBusinessPartner.Value = hdnBusinessPartner.Value;

            List<TempDataSource> lst = new List<TempDataSource>();
            lst.Insert(0, new TempDataSource { SourceID = "1", SourceName = "Transaksi APPROVED" });
            lst.Insert(1, new TempDataSource { SourceID = "2", SourceName = "Transaksi NON-VOID" });
            Methods.SetComboBoxField<TempDataSource>(cboStatus, lst, "SourceName", "SourceID");
            cboStatus.SelectedIndex = 1;

            SetTotalText();
            BindGridView();
        }

        protected void cbpTotal_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            SetTotalText();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh|" + PageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void SetTotalText()
        {
            decimal debit = 0, credit = 0;

            List<GetGLBalanceDtInformationPerPeriode> lstEntity = null;

            if (hdnGLAccountID.Value == "")
            {
                PageCount = 0;
                lstEntity = new List<GetGLBalanceDtInformationPerPeriode>();
                grdView.DataSource = lstEntity;
                grdView.DataBind();
            }
            else
            {
                string subLedger = "0";
                if (hdnSubLedgerDtID.Value != "" && hdnSubLedgerDtID.Value != "0")
                {
                    subLedger = hdnSubLedgerDtID.Value;
                }
                else
                {
                    if (hdnSubLedgerID.Value != "" && hdnSubLedgerID.Value != "0" && hdnSubLedgerID.Value != "1")
                    {
                        subLedger = hdnSubLedgerID.Value;
                    }
                }

                string healthcare = "0", department = "0";
                int serviceUnit = 0, businessPartner = 0;

                if (hdnHealthcare.Value != "" && hdnHealthcare.Value != null)
                {
                    healthcare = hdnHealthcare.Value;
                }
                //if (hdnDepartment.Value != "" && hdnDepartment.Value != null)
                //{
                //    department = hdnDepartment.Value;
                //}
                //if (hdnServiceUnit.Value != "" && hdnServiceUnit.Value != null)
                //{
                //    serviceUnit = Convert.ToInt32(hdnServiceUnit.Value);
                //}
                //if (hdnBusinessPartner.Value != "" && hdnBusinessPartner.Value != null)
                //{
                //    businessPartner = Convert.ToInt32(hdnBusinessPartner.Value);
                //}

                lstEntity = BusinessLayer.GetGLBalanceDtInformationPerPeriodeList(
                                        Convert.ToInt32(hdnGLAccountID.Value),
                                        Convert.ToInt32(subLedger),
                                        healthcare,
                                        department,
                                        serviceUnit,
                                        businessPartner,
                                        Convert.ToInt32(cboYear.Value),
                                        Convert.ToInt32(cboMonth.Value),
                                        cboStatus.Value.ToString(),
                                        1);

                debit = lstEntity.Sum(a => a.DEBITAmount);
                credit = lstEntity.Sum(b => b.CREDITAmount);
            }
            
            txtTotalBalanceDEBIT.Text = debit.ToString("N");
            txtTotalBalanceCREDIT.Text = credit.ToString("N");
        }
        
        private void BindGridView()
        {
            List<GetGLBalanceDtInformationPerPeriode> lstEntity = null;

            if (hdnGLAccountID.Value == "")
            {
                PageCount = 0;
                lstEntity = new List<GetGLBalanceDtInformationPerPeriode>();
                grdView.DataSource = lstEntity;
                grdView.DataBind();
            }
            else
            {
                string subLedger = "0";
                if (hdnSubLedgerDtID.Value != "" && hdnSubLedgerDtID.Value != "0")
                {
                    subLedger = hdnSubLedgerDtID.Value;
                }
                else
                {
                    if (hdnSubLedgerID.Value != "" && hdnSubLedgerID.Value != "0" && hdnSubLedgerID.Value != "1")
                    {
                        subLedger = hdnSubLedgerID.Value;
                    }
                }

                string healthcare = "0", department = "0";
                int serviceUnit = 0, businessPartner = 0;

                if (hdnHealthcare.Value != "" && hdnHealthcare.Value != null)
                {
                    healthcare = hdnHealthcare.Value;
                }
                //if (hdnDepartment.Value != "" && hdnDepartment.Value != null)
                //{
                //    department = hdnDepartment.Value;
                //}
                //if (hdnServiceUnit.Value != "" && hdnServiceUnit.Value != null)
                //{
                //    serviceUnit = Convert.ToInt32(hdnServiceUnit.Value);
                //}
                //if (hdnBusinessPartner.Value != "" && hdnBusinessPartner.Value != null)
                //{
                //    businessPartner = Convert.ToInt32(hdnBusinessPartner.Value);
                //}
                
                lstEntity = BusinessLayer.GetGLBalanceDtInformationPerPeriodeList(
                                        Convert.ToInt32(hdnGLAccountID.Value),
                                        Convert.ToInt32(subLedger),
                                        healthcare,
                                        department,
                                        serviceUnit,
                                        businessPartner,
                                        Convert.ToInt32(cboYear.Value),
                                        Convert.ToInt32(cboMonth.Value),
                                        cboStatus.Value.ToString(),
                                        1);
                grdView.DataSource = lstEntity;
                grdView.DataBind();
            }
        }

        protected class TempDataSource
        {
            String _SourceID;
            String _SourceName;

            public String SourceID
            {
                get { return _SourceID; }
                set { _SourceID = value; }
            }

            public String SourceName
            {
                get { return _SourceName; }
                set { _SourceName = value; }
            }
        }
    }
}