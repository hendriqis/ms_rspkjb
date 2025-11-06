using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CalculateCoverageLimitCtl : BaseContentPopupCtl
    {
        protected int PageCount = 1;

        private PatientBillSummaryGenerateBill DetailPage
        {
            get { return (PatientBillSummaryGenerateBill)Page; }
        }

        public override void InitializeControl(string param)
        {
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            List<GetPatientChargesHdDtRecalculateBillingGroup> lstEntity = BusinessLayer.GetPatientChargesHdDtRecalculateBillingGroupList(AppSession.RegisteredPatient.RegistrationID);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                GetPatientChargesHdDtRecalculateBillingGroup entity = e.Item.DataItem as GetPatientChargesHdDtRecalculateBillingGroup;

                HtmlInputText txtPatientAmount = (HtmlInputText)e.Item.FindControl("txtPatientAmount");
                HtmlInputText txtPayerAmount = (HtmlInputText)e.Item.FindControl("txtPayerAmount");
                TextBox txtLineAmount = e.Item.FindControl("txtLineAmount") as TextBox;

                txtPatientAmount.Value = entity.PatientAmount.ToString();
                txtPayerAmount.Value = entity.PayerAmount.ToString();
                txtLineAmount.Text = entity.LineAmount.ToString();
            }
        }
        #endregion

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string param = e.Parameter;
            string result = param + "|";
            string retval = "";

            string errMessage = "";
            if (param == "calculate")
            {
                if (OnCalculate(ref errMessage, ref retval))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }
            else
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }

        #region Save Calculate Entity
        private bool OnCalculate(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            //RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            //RegistrationPayerDao entityRegistrationPayerDao = new RegistrationPayerDao(ctx);
            RegistrationCoverageDao entityRegistrationCoverageDao = new RegistrationCoverageDao(ctx);

            try
            {
                int oRegistrationID = Convert.ToInt32(hdnRegistrationID.Value);

                //Registration reg = entityRegistrationDao.Get(oRegistrationID);
                //RegistrationPayer entityPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsPrimaryPayer = 1 AND IsDeleted = 0", hdnRegistrationID.Value), ctx).FirstOrDefault();

                decimal coverageAmount = 0;

                List<String> lstSelectedBillingGroupID = hdnSelectedBillingGroupID.Value.Split(',').ToList();
                List<String> lstSelectedPatientAmount = hdnSelectedPatientAmount.Value.Split(',').ToList();
                List<String> lstSelectedPayerAmount = hdnSelectedPayerAmount.Value.Split(',').ToList();
                List<String> lstSelectedLineAmount = hdnSelectedLineAmount.Value.Split(',').ToList();

                for (int i = 1; i < lstSelectedBillingGroupID.Count; i++)
                {
                    int oBillingGroupID = Convert.ToInt32(lstSelectedBillingGroupID[i]);

                    string searchCoverage = string.Format("RegistrationID = {0} AND BillingGroupID = {1}", oRegistrationID, oBillingGroupID);

                    string filterCheckAvailable = searchCoverage + " AND IsDeleted = 0";
                    RegistrationCoverage regCoverage = BusinessLayer.GetRegistrationCoverageList(filterCheckAvailable, ctx).LastOrDefault();

                    if (regCoverage == null)
                    {
                        RegistrationCoverage entityNew = new RegistrationCoverage();
                        entityNew.RegistrationID = oRegistrationID;
                        entityNew.BillingGroupID = oBillingGroupID;
                        entityNew.PatientAmount = Convert.ToDecimal(lstSelectedPatientAmount[i]);
                        entityNew.PayerAmount = Convert.ToDecimal(lstSelectedPayerAmount[i]);
                        entityNew.LineAmount = Convert.ToDecimal(lstSelectedLineAmount[i]);
                        entityNew.CreatedBy = AppSession.UserLogin.UserID;
                        entityRegistrationCoverageDao.Insert(entityNew);

                        coverageAmount += entityNew.PayerAmount;
                    }
                    else
                    {
                        List<RegistrationCoverage> lstCoverage = BusinessLayer.GetRegistrationCoverageList(searchCoverage, ctx);
                        foreach (RegistrationCoverage entityUpdate in lstCoverage)
                        {
                            entityUpdate.IsDeleted = true;
                            entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityRegistrationCoverageDao.Update(entityUpdate);
                        }


                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        RegistrationCoverage entityNew = new RegistrationCoverage();
                        entityNew.RegistrationID = oRegistrationID;
                        entityNew.BillingGroupID = oBillingGroupID;
                        entityNew.PatientAmount = Convert.ToDecimal(lstSelectedPatientAmount[i]);
                        entityNew.PayerAmount = Convert.ToDecimal(lstSelectedPayerAmount[i]);
                        entityNew.LineAmount = Convert.ToDecimal(lstSelectedLineAmount[i]);
                        entityNew.CreatedBy = AppSession.UserLogin.UserID;
                        entityRegistrationCoverageDao.Insert(entityNew);

                        coverageAmount += entityNew.PayerAmount;
                    }
                }

                //////reg.CoverageLimitAmount = coverageAmount;
                //////entityRegistrationDao.Update(reg);

                //////entityPayer.CoverageLimitAmount = coverageAmount;
                //////entityPayer.LastUpdatedBy = AppSession.UserLogin.UserID;
                //////entityRegistrationPayerDao.Update(entityPayer);

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
        #endregion
    }
}