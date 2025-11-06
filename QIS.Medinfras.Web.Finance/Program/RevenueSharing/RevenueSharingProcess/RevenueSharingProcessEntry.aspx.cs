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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingProcessEntry : BasePageTrx
    {
        private string[] lstSelectedMember = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_PROCESS;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
        
        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected string GetClosedTransactionStatus() 
        {
            return Constant.TransactionStatus.CLOSED;
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnParamedicID.Value = AppSession.ParamedicID.ToString();

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsActive = 1 AND IsHasRegistration = 1");
            lstDepartment.Add(new Department { DepartmentID = "", DepartmentName = "ALL" });
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.Value = "ALL";
            hdnParamDepartmentID.Value = "";

            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                                    AppSession.UserLogin.HealthcareID,
                                                                    Constant.SettingParameter.FN_TRANSRS_ALLOW_FILTER_PAID_TYPE,
                                                                    Constant.SettingParameter.FN_TRANSRS_ADD_FILTER_BPJS_STATUS
                                                                ));

            hdnIsAllowChangeFilterPaidType.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_TRANSRS_ALLOW_FILTER_PAID_TYPE).FirstOrDefault().ParameterValue;
            hdnIsFilterBPJSStatus.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_TRANSRS_ADD_FILTER_BPJS_STATUS).FirstOrDefault().ParameterValue;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                                    "IsDeleted = 0 AND IsActive = 1 AND ParentID IN ('{0}','{1}','{2}','{3}')",
                                                                    Constant.StandardCode.REVENUE_REDUCTION,
                                                                    Constant.StandardCode.REVENUE_PAYMENT_METHOD,
                                                                    Constant.StandardCode.REVENUE_PERIODE_TYPE,
                                                                    Constant.StandardCode.CLINIC_GROUP
                                                                ));

            Methods.SetComboBoxField<StandardCode>(cboReduction, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.REVENUE_REDUCTION).ToList(), "StandardCodeName", "StandardCodeID");
            cboReduction.Value = Constant.RevenueReduction.REDUCTION_0;

            Methods.SetComboBoxField<StandardCode>(cboPaymentMethod, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.REVENUE_PAYMENT_METHOD).ToList(), "StandardCodeName", "StandardCodeID");
            cboPaymentMethod.Value = Constant.RevenuePaymentMethod.TUNAI;

            Methods.SetComboBoxField<StandardCode>(cboClinicGroup, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.CLINIC_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
            cboClinicGroup.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboPeriodeType, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.REVENUE_PERIODE_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboPeriodeType.Value = Constant.RevenuePeriodeType.TANGGAL_PELUNASAN;

            txtRevenueSharingDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            txtPeriodFrom.Text = AppSession.ParamedicMasterRevenueSharingProcess.PeriodeStart.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = AppSession.ParamedicMasterRevenueSharingProcess.PeriodeEnd.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            
            int rsID = AppSession.ParamedicMasterRevenueSharingProcess.RevenueSharingID;
            if(rsID != 0 && rsID != null)
            {
                RevenueSharingHd rsh = BusinessLayer.GetRevenueSharingHd(rsID);
                hdnRevenueSharingID.Value = rsh.RevenueSharingID.ToString();
                txtRevenueSharingCode.Text = rsh.RevenueSharingCode;
                txtRevenueSharingName.Text = rsh.RevenueSharingName;
            }

            BusinessPartners bp = BusinessLayer.GetBusinessPartners(1);
            hdnPayerID.Value = bp.BusinessPartnerID.ToString();
            txtPayerCode.Text = bp.BusinessPartnerCode;
            txtPayerName.Text = bp.BusinessPartnerName;

            List<Variable> lstPaidType = new List<Variable>();
            if (hdnIsAllowChangeFilterPaidType.Value == "1")
            {
                lstPaidType.Add(new Variable { Code = "0", Value = "Semua" });
                lstPaidType.Add(new Variable { Code = "1", Value = "Belum Lunas" });
                lstPaidType.Add(new Variable { Code = "2", Value = "Sudah Lunas" });
            }
            else
            {
                lstPaidType.Add(new Variable { Code = "2", Value = "Sudah Lunas" });
            }
            Methods.SetComboBoxField<Variable>(cboPaidType, lstPaidType, "Value", "Code");
            cboPaidType.Value = "2";

            List<Variable> lstRegistrationClosed = new List<Variable>();
            lstRegistrationClosed.Add(new Variable { Code = "0", Value = "Semua" });
            lstRegistrationClosed.Add(new Variable { Code = "1", Value = "Belum Tutup Registrasi" });
            lstRegistrationClosed.Add(new Variable { Code = "2", Value = "Sudah Tutup Registrasi" });
            Methods.SetComboBoxField<Variable>(cboRegistrationStatusFilter, lstRegistrationClosed, "Value", "Code");
            cboRegistrationStatusFilter.Value = "2";

            List<Variable> lstBPJSStatus = new List<Variable>();
            lstBPJSStatus.Add(new Variable { Code = "0", Value = "Semua" });
            lstBPJSStatus.Add(new Variable { Code = "1", Value = "Sudah Klaim Sementara" });
            lstBPJSStatus.Add(new Variable { Code = "2", Value = "Sudah Approve Final Klaim" });
            Methods.SetComboBoxField<Variable>(cboBPJSStatusFilter, lstBPJSStatus, "Value", "Code");
            cboBPJSStatusFilter.Value = "0";

            if (hdnIsFilterBPJSStatus.Value == "1")
            {
                trBPJSFilter.Style.Remove("display");
            }
            else
            {
                trBPJSFilter.Style.Add("display", "none");
            }

            List<Variable> lstSortBy = new List<Variable>();
            lstSortBy.Add(new Variable { Code = "1", Value = "Nama Pasien ASC" });
            lstSortBy.Add(new Variable { Code = "2", Value = "Tgl Registrasi ASC" });
            lstSortBy.Add(new Variable { Code = "3", Value = "No Registrasi ASC" });
            lstSortBy.Add(new Variable { Code = "4", Value = "Tgl Transaksi ASC" });
            Methods.SetComboBoxField<Variable>(cboSortBy, lstSortBy, "Value", "Code");
            cboSortBy.Value = "1";

            //BindGridView();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            int ParamedicID = AppSession.ParamedicID;

            string PeriodeType = cboPeriodeType.Value.ToString();

            DateTime FromDate = Helper.GetDatePickerValue(txtPeriodFrom.Text);
            DateTime ToDate = Helper.GetDatePickerValue(txtPeriodTo.Text);

            int CustomerID = 0;
            int ExCustomerID = 0;
            if (chkIsFilterPayerExclusion.Checked)
            {
                ExCustomerID = hdnPayerID.Value.ToString() != "" ? Convert.ToInt32(hdnPayerID.Value) : 0;
            }
            else
            {
                CustomerID = hdnPayerID.Value.ToString() != "" ? Convert.ToInt32(hdnPayerID.Value) : 0;
            }

            string DepartmentID = hdnParamDepartmentID.Value.ToString() != "" ? hdnParamDepartmentID.Value : "";
            string PaidType = cboPaidType.Value.ToString();
            
            int RegStatus = Convert.ToInt16(cboRegistrationStatusFilter.Value);
            int BPJSStatus = 0;
            if (hdnIsFilterBPJSStatus.Value == "1")
            {
                BPJSStatus = Convert.ToInt16(cboBPJSStatusFilter.Value);
            }

            string ClinicGroup = "%%";
            if (chkIsFilterClinicGroup.Checked)
            {
                ClinicGroup = cboClinicGroup.Value.ToString();
            }

            int RevenueSharingID = hdnRevenueSharingID.Value.ToString() != "" ? Convert.ToInt32(hdnRevenueSharingID.Value) : 0;

            int oNumRows = (int.MaxValue - 1);

            List<GetPatientChargesHdRevenueSharing1> lst = BusinessLayer.GetPatientChargesHdRevenueSharing1List(
                                                                            ParamedicID,
                                                                            DepartmentID,
                                                                            CustomerID,
                                                                            ExCustomerID,
                                                                            PaidType,
                                                                            ClinicGroup,
                                                                            PeriodeType,
                                                                            FromDate,
                                                                            ToDate,
                                                                            txtPeriodeTimeFrom.Text,
                                                                            txtPeriodeTimeTo.Text,
                                                                            RevenueSharingID,
                                                                            RegStatus,
                                                                            BPJSStatus,
                                                                            1, //PageIndex
                                                                            oNumRows, //NumRows
                                                                            cboSortBy.Value.ToString()
                                                                        );
            lstSelectedMember = hdnSelectedMember.Value.Split(',');

            if(cboSortBy.Value.ToString() == "1")
            {
                lvwView.DataSource = lst.OrderBy(a => a.PatientName).ToList();
            }
            else if (cboSortBy.Value.ToString() == "2")
            {
                lvwView.DataSource = lst.OrderBy(a => a.RegistrationDate).ThenBy(b => b.RegistrationNo).ToList();
            }
            else if (cboSortBy.Value.ToString() == "3")
            {
                lvwView.DataSource = lst.OrderBy(a => a.RegistrationNo).ToList();
            }
            else if (cboSortBy.Value.ToString() == "4")
            {
                lvwView.DataSource = lst.OrderBy(a => a.TransactionDate).ThenBy(b => b.TransactionNo).ToList();
            }
            else
            {
                lvwView.DataSource = lst.OrderBy(a => a.TransactionDate).ThenBy(b => b.PatientName).ToList();
            }

            lvwView.DataBind();
            
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                GetPatientChargesHdRevenueSharing1 entity = e.Item.DataItem as GetPatientChargesHdRevenueSharing1;

                CheckBox chkIsSelected = e.Item.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.TransactionID.ToString()))
                {
                    chkIsSelected.Checked = true;
                }
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            try
            {
                DateTime RevenueSharingDate = Helper.GetDatePickerValue(txtRevenueSharingDate.Text);

                int ParamedicID = AppSession.ParamedicID;
                string DepartmentID = hdnParamDepartmentID.Value.ToString() != "" ? hdnParamDepartmentID.Value : "";
                string GCReduction = cboReduction.Value.ToString();
                string GCPaymentMethod = cboPaymentMethod.Value.ToString();
                string GCPeriodeType = cboPeriodeType.Value.ToString();

                string GCClinicGroup = "%%";
                if (chkIsFilterClinicGroup.Checked)
                {
                    GCClinicGroup = cboClinicGroup.Value.ToString();
                }

                DateTime PeriodeDateStart = Helper.GetDatePickerValue(txtPeriodFrom.Text);
                DateTime PeriodeDateEnd = Helper.GetDatePickerValue(txtPeriodTo.Text);

                string lstTransactionDtID = string.Empty;
                string lstTransactionDtItemID = string.Empty;
                string[] selectedArr = hdnSelectedMember.Value.Substring(1).Split(',');

                for (int i = 0; i < selectedArr.Length; i++)
                {
                    if (!lstTransactionDtID.Contains(selectedArr[i].Split('|')[0]))
                    {
                        lstTransactionDtID += selectedArr[i].Split('|')[0] + ",";
                    }
                    if (!lstTransactionDtItemID.Contains(selectedArr[i].Split('|')[1]))
                    {
                        lstTransactionDtItemID += selectedArr[i].Split('|')[1] + ",";
                    }

                }
                lstTransactionDtID = lstTransactionDtID.Remove(lstTransactionDtID.Length - 1, 1);
                lstTransactionDtItemID = lstTransactionDtItemID.Remove(lstTransactionDtItemID.Length - 1, 1);

                string lstSelectedMemberRemarksDt = hdnSelectedMemberRemarksDt.Value.Substring(1);

                retval = BusinessLayer.GenerateParamedicRevenueSharing(
                                            RevenueSharingDate,
                                            ParamedicID,
                                            DepartmentID,
                                            GCReduction,
                                            GCPaymentMethod,
                                            GCClinicGroup,
                                            GCPeriodeType,
                                            PeriodeDateStart,
                                            PeriodeDateEnd,
                                            lstTransactionDtID.Trim().Replace("\n", "").Replace(" ", "").Replace("\r", ""),
                                            lstTransactionDtItemID.Trim().Replace("\n", "").Replace(" ", "").Replace("\r", ""),
                                            lstSelectedMemberRemarksDt,
                                            AppSession.UserLogin.UserID
                                        );
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}