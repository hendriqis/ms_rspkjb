using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class EditQtyItemPackageDetail : BaseViewPopupCtl
    {
        private int ItemServiceID = 0;

        public override void InitializeDataControl(string param)
        {
            string[] data = param.Split('|');
            hdnChargesDtID.Value = data[0];

            vItemService vItemService = BusinessLayer.GetvItemServiceList(String.Format("ItemID = {0} AND IsDeleted = 0", Convert.ToInt32(data[1]))).FirstOrDefault();
            ItemServiceID = vItemService.ItemID;
            txtItemServiceName.Text = string.Format("{0} - {1}", vItemService.ItemCode, vItemService.ItemName1);
            txtItemServiceName2.Text = string.Format("{0} - {1}", vItemService.ItemCode, vItemService.ItemName1);
            txtItemServiceName3.Text = string.Format("{0} - {1}", vItemService.ItemCode, vItemService.ItemName1);

            BindGridView();
        }

        private void BindGridView()
        {
            //List<vItemServiceDt> lstItemService = BusinessLayer.GetvItemServiceDtList(string.Format("ItemID = {0} AND IsDeleted = 0 AND GCItemType IN ('{1}','{2}','{3}') ORDER BY ItemID ASC", ItemServiceID, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM));

            string filterDt = string.Format("PatientChargesDtID = {0} AND IsDeleted = 0 ORDER BY ID ASC", hdnChargesDtID.Value);
            List<vPatientChargesDtPackage> lstItemService = BusinessLayer.GetvPatientChargesDtPackageList(filterDt);
            
            grdView.DataSource = from i in lstItemService
                                 where i.GCItemType != Constant.ItemType.OBAT_OBATAN && i.GCItemType != Constant.ItemType.BARANG_MEDIS && i.GCItemType != Constant.ItemType.BARANG_UMUM && i.GCItemType != Constant.ItemType.BAHAN_MAKANAN
                                 select i;
            grdView.DataBind();
            grdViewObat.DataSource = from i in lstItemService
                                     where i.GCItemType == Constant.ItemType.OBAT_OBATAN || i.GCItemType == Constant.ItemType.BARANG_MEDIS
                                     select i;
            grdViewObat.DataBind();
            grdViewBarang.DataSource = from i in lstItemService
                                       where i.GCItemType == Constant.ItemType.BARANG_UMUM || i.GCItemType == Constant.ItemType.BAHAN_MAKANAN
                                       select i;
            grdViewBarang.DataBind();
        }
        
        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void grdViewObat_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void grdViewBarang_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }

        private void SetControlEntrySetting()
        {
            Helper.SetControlEntrySetting(hdnDetailItemID, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDetailItemCode, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDetailItemName1, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(hdnParamedicID, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtParamedicName, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtChargedQuantity, new ControlEntrySetting(false, true, true), "mpEntryPopup");

            Helper.SetControlEntrySetting(hdnDetailItemIDObat, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDetailItemCodeObat, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDetailItemName1Obat, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(hdnParamedicIDObat, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtParamedicCodeObat, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtParamedicNameObat, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtChargedQuantity, new ControlEntrySetting(false, true, true), "mpEntryPopup");

            Helper.SetControlEntrySetting(hdnDetailItemIDBarang, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDetailItemCodeBarang, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDetailItemName1Barang, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(hdnParamedicIDBarang, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtParamedicCodeBarang, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtParamedicNameBarang, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtChargedQuantity, new ControlEntrySetting(false, true, true), "mpEntryPopup");
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtPackageDao entityDtDao = new PatientChargesDtPackageDao(ctx);
            try
            {
                string filterPackage = string.Format("PatientChargesDtID = '{0}' AND IsDeleted = 0", hdnChargesDtID.Value);
                List<PatientChargesDtPackage> lstDtPackage = BusinessLayer.GetPatientChargesDtPackageList(filterPackage, ctx);
                PatientChargesDtPackage entity = lstDtPackage.Where(t => t.ID == Convert.ToInt32(hdnID.Value)).FirstOrDefault();

                entity.ChargedQuantity = Convert.ToDecimal(txtChargedQuantity.Text);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);

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

        protected void cbpEntryPopupViewObat_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnIDObat.Value.ToString() != "")
                {
                    if (OnSaveEditRecordObat(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveEditRecordObat(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtPackageDao entityDtDao = new PatientChargesDtPackageDao(ctx);
            try
            {
                string filterPackage = string.Format("PatientChargesDtID = '{0}' AND IsDeleted = 0", hdnChargesDtID.Value);
                List<PatientChargesDtPackage> lstDtPackage = BusinessLayer.GetPatientChargesDtPackageList(filterPackage, ctx);
                PatientChargesDtPackage entity = lstDtPackage.Where(t => t.ID == Convert.ToInt32(hdnIDObat.Value)).FirstOrDefault();

                entity.ChargedQuantity = Convert.ToDecimal(txtChargedQuantityObat.Text);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);

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

        protected void cbpEntryPopupViewBarang_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnIDBarang.Value.ToString() != "")
                {
                    if (OnSaveEditRecordBarang(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveEditRecordBarang(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesDtPackageDao entityDtDao = new PatientChargesDtPackageDao(ctx);
            try
            {
                string filterPackage = string.Format("PatientChargesDtID = '{0}' AND IsDeleted = 0", hdnChargesDtID.Value);
                List<PatientChargesDtPackage> lstDtPackage = BusinessLayer.GetPatientChargesDtPackageList(filterPackage, ctx);
                PatientChargesDtPackage entity = lstDtPackage.Where(t => t.ID == Convert.ToInt32(hdnIDBarang.Value)).FirstOrDefault();

                entity.ChargedQuantity = Convert.ToDecimal(txtChargedQuantityBarang.Text);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);

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