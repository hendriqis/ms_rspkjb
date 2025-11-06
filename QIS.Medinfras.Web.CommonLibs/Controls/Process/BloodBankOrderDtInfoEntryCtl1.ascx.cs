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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class BloodBankOrderDtInfoEntryCtl1 : BaseViewPopupCtl
    {

        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnVisitID.Value = paramInfo[0];
            hdnTestOrderID.Value = paramInfo[1];

            string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0 ORDER BY ItemCode", hdnVisitID.Value, hdnTestOrderID.Value);
            List<vTestOrderDtBloodBank1> lstEntity = BusinessLayer.GetvTestOrderDtBloodBank1List(filterExpression);
            Methods.SetComboBoxField<vTestOrderDtBloodBank1>(cboItemID, lstEntity, "ItemName1", "ID");
            cboItemID.SelectedIndex = 0;

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
         Constant.StandardCode.KUALITAS_KANTONG_DARAH, Constant.StandardCode.KUALITAS_DARAH));

            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.KUALITAS_KANTONG_DARAH).ToList();
            List<StandardCode> lstCode2 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.KUALITAS_DARAH).ToList();


            Methods.SetComboBoxField<StandardCode>(cboPackingQuality, lstCode1, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBloodQuality, lstCode2, "StandardCodeName", "StandardCodeID");

            Helper.SetControlEntrySetting(txtBloodBagNo, new ControlEntrySetting(true, true, true), "mpEntry");
            Helper.SetControlEntrySetting(cboPackingQuality, new ControlEntrySetting(true, true, true), "mpEntry");
            Helper.SetControlEntrySetting(cboBloodQuality, new ControlEntrySetting(true, true, true), "mpEntry");
            BindGridView(1, true, ref PageCount); 
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            List<vTestOrderDtBloodBag> lstEntity = new List<vTestOrderDtBloodBag>();
            if (hdnTestOrderID.Value != "0")
            {
                string filterExpression = string.Format("TestOrderDtID = {0} AND IsDeleted = 0", cboItemID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvTestOrderDtBloodBagRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                lstEntity = BusinessLayer.GetvTestOrderDtBloodBagList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LabelNo");
                grdView.DataSource = lstEntity;
                grdView.DataBind();
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                result = "refresh|" + pageCount;
            }
            else
            {
                if (param[0] == "save")
                {
                    if (hdnIDCtl.Value.ToString() != "")
                    {
                        if (OnSaveEditRecord(ref errMessage))
                        {
                            result += "success";
                            BindGridView(1, true, ref pageCount);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                    else
                    {
                        if (OnSaveAddRecord(ref errMessage))
                        {
                            result += "success";
                            BindGridView(1, true, ref pageCount);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                }
                else if (param[0] == "delete")
                {
                    if (OnDeleteRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(TestOrderDtBloodBag entity)
        {
            entity.TestOrderDtID = Convert.ToInt32(cboItemID.Value.ToString());
            entity.LabelNo = txtBloodBagNo.Text;
            entity.ExpiredDate = Helper.GetDatePickerValue(txtExpiredDate);
            entity.GCPackingQuality = cboPackingQuality.Value.ToString();
            entity.GCBloodQuality = cboBloodQuality.Value.ToString();
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            String retval;
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtBloodBagDao entityDao = new TestOrderDtBloodBagDao(ctx);
            try
            {
                TestOrderDtBloodBag entity = new TestOrderDtBloodBag();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                int id = entityDao.Insert(entity);

                retval = id.ToString();
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtBloodBagDao entityDao = new TestOrderDtBloodBagDao(ctx);
            try
            {
                int id = Convert.ToInt32(hdnIDCtl.Value);
                TestOrderDtBloodBag entity = entityDao.Get(id);
                if (!entity.IsDeleted)
                {
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);
                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            Boolean result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtBloodBagDao entityDao = new TestOrderDtBloodBagDao(ctx);

            int id = Convert.ToInt32(hdnIDCtl.Value);
            TestOrderDtBloodBag entity = entityDao.Get(id);
            if (!entity.IsDeleted)
            {
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
            }
            return result;
        }
    }
}