using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class UpdateINACBGsAmountList : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.BPJS_UPDATE_INA_AMOUNT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsActive = 1 AND IsHasRegistration = 1");
            lstDepartment.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;

            List<StandardCode> lstVisitStatus = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID NOT IN ('{1}','{2}','{3}') AND IsDeleted = 0 AND IsActive = 1",
                                                                                                        Constant.StandardCode.VISIT_STATUS,
                                                                                                        Constant.VisitStatus.OPEN,
                                                                                                        Constant.VisitStatus.CANCELLED,
                                                                                                        Constant.VisitStatus.CLOSED));
            lstVisitStatus.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboVisitStatus, lstVisitStatus, "StandardCodeName", "StandardCodeID");
            cboVisitStatus.SelectedIndex = 0;

            BindGridView();
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("RegistrationDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112));

            if (cboDepartment.Value != null)
            {
                if (cboDepartment.Value.ToString() != "")
                {
                    filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value.ToString());
                }
            }
            
            if (hdnHealthcareServiceUnitID.Value != "" && hdnHealthcareServiceUnitID.Value != "0")
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = '{0}'", hdnHealthcareServiceUnitID.Value);
            }

            if (cboVisitStatus.Value != null)
            {
                if (cboVisitStatus.Value.ToString() != "")
                {
                    filterExpression += string.Format(" AND GCRegistrationStatus = '{0}'", cboVisitStatus.Value.ToString());
                }
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            List<vRegistrationBPJS3> lst = BusinessLayer.GetvRegistrationBPJS3List(filterExpression);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vRegistrationBPJS3 entity = e.Item.DataItem as vRegistrationBPJS3;

                TextBox txtINAHakPasien = (TextBox)e.Item.FindControl("txtINAHakPasien");
                TextBox txtINADitempati = (TextBox)e.Item.FindControl("txtINADitempati");

                txtINAHakPasien.Text = entity.INAHakPasien.ToString();
                txtINADitempati.Text = entity.INADitempati.ToString();

            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);

            try
            {
                if (hdnSelectedRegistrationID.Value != "")
                {
                    List<String> lstRegistrationID = hdnSelectedRegistrationID.Value.Split(',').ToList();
                    List<String> lstINAHakPasien = hdnSelectedINAHakPasien.Value.Split(',').ToList();
                    List<String> lstINADitempati = hdnSelectedINADitempati.Value.Split(',').ToList();

                    lstRegistrationID.RemoveAt(0);
                    lstINAHakPasien.RemoveAt(0);
                    lstINADitempati.RemoveAt(0);

                    if (type == "save")
                    {
                        #region SAVE

                        for (int i = 0; i < lstRegistrationID.Count(); i++)
                        {
                            RegistrationBPJS entity = registrationBPJSDao.Get(Convert.ToInt32(lstRegistrationID[i]));
                            entity.INAHakPasien = Convert.ToDecimal(lstINAHakPasien[i]);
                            entity.INADitempati = Convert.ToDecimal(lstINADitempati[i]);
                            registrationBPJSDao.Update(entity);
                        }

                        ctx.CommitTransaction();

                        #endregion
                    }
                }
                else
                {
                    result = false;
                    errMessage = GetErrorMsgSelectTransactionFirst();
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
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