using System;
using System.Collections.Generic;
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
    public partial class QueuePharmacyEditCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        protected List<DataTemp1> lstTemp = new List<DataTemp1>();

        //private APInvoiceSupplierProcess DetailPage
        //{
        //    get { return (APInvoiceSupplierProcess)Page; }
        //}

        protected string DateTimeNowDatePicker()
        {
            return DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        public override void InitializeDataControl(string param)
        {
            string[] filter = param.Split('|');
            
            hdnRegistrationIDCtl.Value = filter[0];

            vRegistration10 reg = BusinessLayer.GetvRegistration10List(string.Format("RegistrationID = '{0}'", hdnRegistrationIDCtl.Value)).FirstOrDefault();
            if (reg != null) {
                txtPatientName.Text = reg.PatientName;
                txtRM.Text = reg.MedicalNo;
                txtRegistrationNo.Text = reg.RegistrationNo;
            }


            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = string.Format("RegistrationID='{0}' AND GCTransactionStatus != '{1}' AND GCOrderStatus != '{2}'", hdnRegistrationIDCtl.Value, Constant.TransactionStatus.VOID, Constant.OrderStatus.CANCELLED);

            List<vPrescriptionOrderHd> lstEntity = BusinessLayer.GetvPrescriptionOrderHdList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPrescriptionOrderHd entity = e.Item.DataItem as vPrescriptionOrderHd;
                TextBox txtNoAntrian = e.Item.FindControl("txtNoAntrian") as TextBox;

                if (!string.IsNullOrEmpty(entity.ReferenceNo))
                {
                    string[] data = entity.ReferenceNo.Split('|');
                    txtNoAntrian.Text = data[1];
                }
                else {
                    txtNoAntrian.Text = "";
                }

            }
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Save Entity
        protected void setDataBeforeSave()
        {
            string[] paramNew = hdnDataSave.Value.Split('$');
            lstTemp = new List<DataTemp1>();

            for (int i = 0; i < paramNew.Length; i++)
            {
                if (!String.IsNullOrEmpty(paramNew[i]))
                {
                    string[] paramNewSplit = paramNew[i].Split('|');
                    int keyNew = Convert.ToInt32(paramNewSplit[1]);
                    string QueueNo = paramNewSplit[2];
                     
                    DataTemp1 oData = new DataTemp1();
                    oData.Key = Convert.ToInt32(keyNew);
                    oData.QueueNo = QueueNo;
                    lstTemp.Add(oData);
                }
            }
        }

        private void ControlToEntity(IDbContext ctx, List<PurchaseInvoiceDt> lstEntityDt)
        {
            PrescriptionOrderHdDao entityPOHdDao = new PrescriptionOrderHdDao(ctx);

            #region new
            setDataBeforeSave();
            foreach (DataTemp1 row in lstTemp) {
                PrescriptionOrderHd oData = BusinessLayer.GetPrescriptionOrderHd(row.Key);
                if (oData != null) {
                    if (!string.IsNullOrEmpty(oData.ReferenceNo)) {
                        string[] data = oData.ReferenceNo.Split('|');
                        if (data.Length == 2)
                        {
                            string itterNo = data[0];
                            string queueNodata = string.Format("{0}|{1}", itterNo, row.QueueNo);
                            oData.ReferenceNo = queueNodata; 
                        }
                        else {
                            oData.ReferenceNo = string.Format("|{0}", row.QueueNo);
                        }
                       
                    }
                    entityPOHdDao.Update(oData);
                }
            }
             
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao entityPOHdDao = new PrescriptionOrderHdDao(ctx);

            //int purchaseInvoiceID = 0;
             try
            {
                #region new
                setDataBeforeSave();
                foreach (DataTemp1 row in lstTemp)
                {
                    PrescriptionOrderHd oData = BusinessLayer.GetPrescriptionOrderHd(row.Key);
                    if (oData != null)
                    {
                        if (!string.IsNullOrEmpty(oData.ReferenceNo))
                        {
                            string[] data = oData.ReferenceNo.Split('|');
                            if (data.Length == 2)
                            {
                                string itterNo = data[0];
                                string queueNodata = string.Format("{0}|{1}", itterNo, row.QueueNo);
                                oData.ReferenceNo = queueNodata;
                            }
                            else
                            {
                                oData.ReferenceNo = string.Format("|{0}", row.QueueNo);
                            }

                        }
                        else {
                            string itterNo = "";
                            string queueNodata = string.Format("{0}|{1}", itterNo, row.QueueNo);
                            oData.ReferenceNo = queueNodata;
                        }
                        entityPOHdDao.Update(oData);
                       
                    }
                }
                ctx.CommitTransaction();
                #endregion
            }
             catch (Exception ex)
             {
                 errMessage = ex.Message;
                 Helper.InsertErrorLog(ex);
                 result = false;
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

    public class DataTemp1
    {
        public int Key { get; set; }
        public string QueueNo { get; set; }
    }
}