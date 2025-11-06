using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class CustomerMemberEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnBusinessPartnerID.Value = param;

            BusinessPartners hsu = BusinessLayer.GetBusinessPartners(Convert.ToInt32(param));
            txtCustomerName.Text = string.Format("{0} - {1}", hsu.BusinessPartnerCode, hsu.BusinessPartnerName);

            BindGridView(1, true, ref PageCount);
            //txtMedicalNo.Attributes.Add("validationgroup", "mpEntryPopup");

        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("BusinessPartnerID = {0} AND PatientName LIKE '%{1}%'", hdnBusinessPartnerID.Value, hdnFilterParam.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvCustomerMemberRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vCustomerMember> lstEntity = BusinessLayer.GetvCustomerMemberList(filterExpression, 8, pageIndex, "PatientName ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

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
                result = string.Format("refresh|{0}", pageCount);
            }
            else
            {
                if (param[0] == "save")
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else if (param[0] == "delete")
                {
                    if (OnDeleteRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }

                BindGridView(1, true, ref pageCount);
                result += "|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(CustomerMember entity)
        {
            entity.MRN = Convert.ToInt32(hdnMemberID.Value);
            entity.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerID.Value);
            entity.MemberNo = txtMemberNo.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                CustomerMember entity = new CustomerMember();
                ControlToEntity(entity);
                BusinessLayer.InsertCustomerMember(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                CustomerMember entity = BusinessLayer.GetCustomerMember(Convert.ToInt32(hdnBusinessPartnerID.Value), Convert.ToInt32(hdnMemberID.Value));
                ControlToEntity(entity);
                BusinessLayer.UpdateCustomerMember(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                BusinessLayer.DeleteCustomerMember(Convert.ToInt32(hdnBusinessPartnerID.Value), Convert.ToInt32(hdnMemberID.Value));
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}