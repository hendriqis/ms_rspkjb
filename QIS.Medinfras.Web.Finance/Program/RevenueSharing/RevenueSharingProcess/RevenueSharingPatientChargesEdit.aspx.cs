using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingPatientChargesEdit : BasePageTrx
    {
        private string[] lstSelectedMember = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_CHARGES_EDIT_REVENUE;
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

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                                    "IsDeleted = 0 AND IsActive = 1 AND ParentID IN ('{0}','{1}','{2}','{3}')",
                                                                    Constant.StandardCode.REVENUE_REDUCTION,
                                                                    Constant.StandardCode.REVENUE_PAYMENT_METHOD,
                                                                    Constant.StandardCode.REVENUE_PERIODE_TYPE,
                                                                    Constant.StandardCode.CLINIC_GROUP
                                                                ));

            Methods.SetComboBoxField<StandardCode>(cboClinicGroup, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.CLINIC_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
            cboClinicGroup.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboPeriodeType, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.REVENUE_PERIODE_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboPeriodeType.Value = Constant.RevenuePeriodeType.TANGGAL_PELUNASAN;

            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

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
            lstPaidType.Add(new Variable { Code = "0", Value = "Semua" });
            lstPaidType.Add(new Variable { Code = "1", Value = "Belum Lunas" });
            lstPaidType.Add(new Variable { Code = "2", Value = "Sudah Lunas" });
            Methods.SetComboBoxField<Variable>(cboPaidType, lstPaidType, "Value", "Code");
            cboPaidType.Value = "2";

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
            if (chkIsFilter.Checked)
            {
                ExCustomerID = hdnPayerID.Value.ToString() != "" ? Convert.ToInt32(hdnPayerID.Value) : 0;
            }
            else
            {
                CustomerID = hdnPayerID.Value.ToString() != "" ? Convert.ToInt32(hdnPayerID.Value) : 0;
            }

            string DepartmentID = hdnParamDepartmentID.Value.ToString() != "" ? hdnParamDepartmentID.Value : "";
            string PaidType = cboPaidType.Value.ToString();

            string ClinicGroup = "%%";
            if (chkIsFilterClinicGroup.Checked)
            {
                ClinicGroup = cboClinicGroup.Value.ToString();
            }

            int RevenueSharingID = hdnRevenueSharingID.Value.ToString() != "" ? Convert.ToInt32(hdnRevenueSharingID.Value) : 0;

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
                                                                            0,
                                                                            0,
                                                                            1, //PageIndex
                                                                            10000, //NumRows
                                                                            cboSortBy.Value.ToString()
                                                                        );
            lstSelectedMember = hdnSelectedMember.Value.Split(',');

            lvwView.DataSource = lst;
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
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);

            String[] paramIDList = hdnSelectedMember.Value.Substring(1).Split(',');
            String[] revSharingIDList = hdnSelectedMemberRevenueSharingID.Value.Substring(1).Split(',');

            try
            {
                for (int i = 0; i < paramIDList.Length; i++)
                {
                    int revSharingID = Convert.ToInt32(revSharingIDList[i]);

                    if (hdnRevenueSharingUpdateToID.Value != "" && hdnRevenueSharingUpdateToID.Value != "0" && hdnRevenueSharingUpdateToID.Value != null)
                    {
                        revSharingID = Convert.ToInt32(hdnRevenueSharingUpdateToID.Value);
                    }

                    if (revSharingID != 0)
                    {
                        PatientChargesDt chargesDt = patientChargesDtDao.Get(Convert.ToInt32(paramIDList[i]));

                        chargesDt.RevenueSharingID = revSharingID;
                        chargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        chargesDt.LastUpdatedDate = DateTime.Now;

                        patientChargesDtDao.Update(chargesDt);
                    }
                }

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