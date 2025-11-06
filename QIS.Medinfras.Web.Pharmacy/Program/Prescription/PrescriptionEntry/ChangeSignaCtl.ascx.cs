using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class ChangeSignaCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] localParam = param.Split('|');
                hdnTransactionIDCtl.Value = localParam[0];
                hdnPrescriptionOrderIDCtl.Value = localParam[1];
                PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionIDCtl.Value));
                txtPrescriptionNo.ReadOnly = true;
                txtPrescriptionNo.Text = entity.TransactionNo;
                hdnVisitID.Value = entity.VisitID.ToString();
                BindGridView();
                SetControlProperties();
            } 
        }

        private void SetControlProperties()
        {
            //txtVisitNoteDate.Attributes.Add("validationgroup", "mpPatientVisitNotes");
            //txtVisitNoteTime.Attributes.Add("validationgroup", "mpPatientVisitNotes");
            //txtNoteText.Attributes.Add("validationgroup", "mpPatientVisitNotes");
            //txtVisitNoteDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            //txtVisitNoteTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        }

        private void BindGridView()
        {
            //grdSignaCtl.DataSource = BusinessLayer.GetvPatientVisitNoteList(string.Format("VisitID = {0} AND IsDeleted = 0 AND GCPatientNoteType = '{1}'", hdnVisitID.Value, Constant.PatientVisitNotes.PHARMACY_NOTES));
            //grdSignaCtl.DataBind();

            List<GetPrescriptionPrice> lstEntity = new List<GetPrescriptionPrice>();
            if (hdnPrescriptionOrderIDCtl.Value != "" && hdnPrescriptionOrderIDCtl.Value != "0" && hdnTransactionIDCtl.Value != "0")
            {
                lstEntity = BusinessLayer.GetPrescriptionPrice(Convert.ToInt32(hdnTransactionIDCtl.Value), Convert.ToInt32(hdnPrescriptionOrderIDCtl.Value));
            }
            lvwViewCtl.DataSource = lstEntity;
            lvwViewCtl.DataBind();
        }

        protected void cbpChangeSigna_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnPrescriptionDetailIDCtl.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                //else
                //{
                //    if (OnSaveAddRecord(ref errMessage))
                //        result += "success";
                //    else
                //        result += string.Format("fail|{0}", errMessage);
                //}
            }
            //else
            //{
            //    result = "delete|";
            //    if (OnDeleteRecord(ref errMessage))
            //        result += "success";
            //    else
            //        result += string.Format("fail|{0}", errMessage);
            //}

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PrescriptionOrderDt entity)
        {
            if (hdnSignaIDCtl.Value != "")
                entity.SignaID = Convert.ToInt32(hdnSignaIDCtl.Value);
            else
                entity.SignaID = null;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
                PrescriptionOrderDt orderDt = BusinessLayer.GetPrescriptionOrderDt(Convert.ToInt32(hdnPrescriptionDetailIDCtl.Value));
                ControlToEntity(orderDt);
                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                orderDtDao.Update(orderDt);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            return result;
        }
    }
}