using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ServiceUnitRoomEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnHealthcareServiceUnitID.Value = param;
            hdnIsBridgingToAplicares.Value = AppSession.IsBridgingToAPLICARES ? "1" : "0";

            vHealthcareServiceUnit entity = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value))[0];
            txtHealthcareName.Text = entity.HealthcareName;
            txtServiceUnitName.Text = entity.ServiceUnitName;

            string paramcode = "OP0002";
            SettingParameter setvar = BusinessLayer.GetSettingParameter(paramcode);
            ClassCare classOP = BusinessLayer.GetClassCare(Convert.ToInt32(setvar.ParameterValue));
            hdnDepartmentID.Value = entity.DepartmentID;
            if (entity.DepartmentID == "OUTPATIENT")
            {
                List<ClassCare> listClassID = BusinessLayer.GetClassCareList(string.Format("ClassID = {0}", classOP.ClassID));
                Methods.SetComboBoxField<ClassCare>(cboClassID, listClassID, "ClassName", "ClassID");
                cboClassID.SelectedIndex = 0;

                string filterChargeClass = string.Format("IsDeleted = 0 AND IsUsedInChargeClass = 1");
                if (entity.ChargeClassID != null && entity.ChargeClassID != 0)
                {
                    filterChargeClass += string.Format(" AND ClassID IN ({0},{1})", classOP.ClassID, entity.ChargeClassID);
                }
                else
                {
                    filterChargeClass += string.Format(" AND ClassID IN ({0})", classOP.ClassID);
                }
                List<ClassCare> listChargeClassID = BusinessLayer.GetClassCareList(filterChargeClass);
                Methods.SetComboBoxField<ClassCare>(cboChargeClassID, listChargeClassID, "ClassName", "ClassID");
                cboChargeClassID.SelectedIndex = 0;
            }
            else
            {
                List<ClassCare> listClassID = BusinessLayer.GetClassCareList(string.Format("IsDeleted = 0 ORDER BY ClassCode"));
                Methods.SetComboBoxField<ClassCare>(cboClassID, listClassID, "ClassName", "ClassID");
                cboClassID.SelectedIndex = 0;

                Methods.SetComboBoxField<ClassCare>(cboChargeClassID, listClassID.Where(p => p.IsUsedInChargeClass).ToList(), "ClassName", "ClassID");
                cboChargeClassID.SelectedIndex = 0;

            }


            BindGridView(1, true, ref PageCount);
            txtRoomCode.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", hdnHealthcareServiceUnitID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvServiceUnitRoomRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vServiceUnitRoom> lstEntity = BusinessLayer.GetvServiceUnitRoomList(filterExpression, 8, pageIndex, "RoomCode ASC");
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
            else
            {
                if (param[0] == "save")
                {
                    if (hdnID.Value.ToString() != "")
                    {
                        if (OnSaveEditRecord(ref errMessage))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                    else
                    {
                        if (OnSaveAddRecord(ref errMessage))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
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

        private void ControlToEntity(ServiceUnitRoom entity)
        {
            entity.RoomID = Convert.ToInt32(hdnRoomID.Value);
            entity.ISBORCalculation = chkISBORCalculation.Checked;
            entity.ClassID = Convert.ToInt32(cboClassID.Value);
            entity.ChargeClassID = Convert.ToInt32(cboChargeClassID.Value);
            entity.TemporaryBedDiscount = Convert.ToDecimal(txtDiscount.Text);
            entity.IsAplicares = chkIsAplicares.Checked;
            entity.AplicaresClassCode = txtAplicaresClassCode.Text;
            entity.AplicaresClassName = txtAplicaresClassName.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ServiceUnitRoomDao entityDao = new ServiceUnitRoomDao(ctx);
            try
            {
                ServiceUnitRoom entity = new ServiceUnitRoom();
                ControlToEntity(entity);
                entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);

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

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ServiceUnitRoomDao entityDao = new ServiceUnitRoomDao(ctx);
            try
            {
                ServiceUnitRoom entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ServiceUnitRoomDao entityDao = new ServiceUnitRoomDao(ctx);
            try
            {
                ServiceUnitRoom entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

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