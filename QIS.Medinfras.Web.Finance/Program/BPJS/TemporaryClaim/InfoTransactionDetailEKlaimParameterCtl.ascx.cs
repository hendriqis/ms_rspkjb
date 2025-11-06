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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InfoTransactionDetailEKlaimParameterCtl : BaseContentPopupCtl
    {
        protected int PageCount = 1;

        private TemporaryClaim DetailPage
        {
            get { return (TemporaryClaim)Page; }
        }

        public override void InitializeControl(string param)
        {
            hdnRegistrationID.Value = param;

            string filterExpression = string.Format("RegistrationID = '{0}'", hdnRegistrationID.Value);
            vConsultVisitCasemix entity = BusinessLayer.GetvConsultVisitCasemixList(filterExpression).FirstOrDefault();

            txtRegistrationNo.Text = string.Format("{0}", entity.RegistrationNo);
            txtSEPNo.Text = string.Format("{0}", entity.NoSEP);
            txtPatient.Text = string.Format("({0}) {1}", entity.MedicalNo, entity.PatientName);

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            List<GetPatientChargesHdDtEKlaimParameterDetail> lstEntity = BusinessLayer.GetPatientChargesHdDtEKlaimParameterDetailList(Convert.ToInt32(hdnRegistrationID.Value));
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            txtTotalPatientAmount.Text = lstEntity.Sum(a => a.PatientAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalPayerAmount.Text = lstEntity.Sum(a => a.PayerAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalLineAmount.Text = lstEntity.Sum(a => a.LineAmount).ToString(Constant.FormatString.NUMERIC_2);

            txtTotalPatientObatDitagihkanAmount.Text = lstEntity.Where(a => a.EKlaimParameterName == "Obat (Ditagihkan)" && a.EKlaimParameterCode == "EK013").ToList().Sum(a => a.PatientAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalPayerObatDitagihkanAmount.Text = lstEntity.Where(a => a.EKlaimParameterName == "Obat (Ditagihkan)" && a.EKlaimParameterCode == "EK013").ToList().Sum(a => a.PayerAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalLineObatDitagihkanAmount.Text = lstEntity.Where(a => a.EKlaimParameterName == "Obat (Ditagihkan)" && a.EKlaimParameterCode == "EK013").ToList().Sum(a => a.LineAmount).ToString(Constant.FormatString.NUMERIC_2);

            txtTotalPatientTanpaObatDitagihkanAmount.Text = lstEntity.Where(a => a.EKlaimParameterName != "Obat (Ditagihkan)").ToList().Sum(a => a.PatientAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalPayerTanpaObatDitagihkanAmount.Text = lstEntity.Where(a => a.EKlaimParameterName != "Obat (Ditagihkan)").ToList().Sum(a => a.PayerAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalLineTanpaObatDitagihkanAmount.Text = lstEntity.Where(a => a.EKlaimParameterName != "Obat (Ditagihkan)").ToList().Sum(a => a.LineAmount).ToString(Constant.FormatString.NUMERIC_2);
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                GetPatientChargesHdDtEKlaimParameterDetail entity = e.Item.DataItem as GetPatientChargesHdDtEKlaimParameterDetail;

            }
        }
        #endregion

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
           /// string param = e.Parameter;
            string result = "";
            string retval = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "SaveEklaimParameter")
            {
                if(SaveItemMaster(ref errMessage)){
                    result += string.Format("success|");
                }else{
                      result += string.Format("fail|", errMessage);
                }
              
            }
            else
            {
                BindGridView();

            
            }
            
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }

        private bool SaveItemMaster(ref string errMessage) {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemMasterDao itemMasterDao = new ItemMasterDao(ctx);
            try
            {
                int itemID = Convert.ToInt32(hdnItemMasterID.Value);
                ItemMaster entity = itemMasterDao.Get(itemID);
                entity.EKlaimParameterID = Convert.ToInt32(hdnEKlaimParameterID.Value);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                itemMasterDao.Update(entity);
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