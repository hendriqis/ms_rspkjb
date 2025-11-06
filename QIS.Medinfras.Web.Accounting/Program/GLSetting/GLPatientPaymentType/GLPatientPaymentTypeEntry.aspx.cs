using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLPatientPaymentTypeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_PATIENT_PAYMENT_TYPE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public string OnGetHealthcareSeriveUnitFilterExpression()
        {
            return String.Format("IsDeleted = 0 AND HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID);
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String[] param = Request.QueryString["id"].Split('|');
                String ID = param[0];
                hdnID.Value = ID;

                vGLPaymentTypeHd entity = BusinessLayer.GetvGLPaymentTypeHdList(String.Format("ID = {0}", hdnID.Value))[0];
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }

            txtGLAccountCode.Focus();
        }

        protected override void SetControlProperties()
        {
            List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsHasRegistration = 1 AND IsActive = 1");
            Methods.SetComboBoxField(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");

            String filterExpression = String.Format("ParentID IN ('{0}', '{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PAYMENT_TYPE, Constant.StandardCode.CUSTOMER_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            Methods.SetComboBoxField(cboGCPaymentType, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.PAYMENT_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCCustomerType, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCPaymentType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCCustomerType, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(hdnGLAccountID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccountCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGLAccountName, new ControlEntrySetting(false, false, true));  
        }

        private void EntityToControl(vGLPaymentTypeHd entity)
        {
            hdnDepartmentORI.Value = entity.DepartmentID;
            cboDepartment.Value = entity.DepartmentID;
            cboGCPaymentType.Value = entity.GCPaymentType;
            cboGCCustomerType.Value = entity.GCCustomerType;

            hdnGLAccountID.Value = entity.GLAccountID.ToString();
            txtGLAccountCode.Text = entity.GLAccountNo;
            txtGLAccountName.Text = entity.GLAccountName;
        }

        private void ControlToEntity(GLPaymentTypeHd entity)
        {
            entity.DepartmentID = cboDepartment.Value.ToString();
            entity.GCPaymentType = cboGCPaymentType.Value.ToString();
            entity.GCCustomerType = cboGCCustomerType.Value.ToString();

            if (hdnGLAccountID.Value != "" && hdnGLAccountID.Value != "0")
            {
                entity.GLAccountID = Convert.ToInt32(hdnGLAccountID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLPaymentTypeHdDao entityDao = new GLPaymentTypeHdDao(ctx);
            try
            {
                GLPaymentTypeHd entity = new GLPaymentTypeHd();
                ControlToEntity(entity);
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLPaymentTypeHdDao entityDao = new GLPaymentTypeHdDao(ctx);
            GLPaymentTypeDtDao entityDtDao = new GLPaymentTypeDtDao(ctx);
            try
            {
                if (hdnID.Value != "" && hdnID.Value != null)
                {
                    GLPaymentTypeHd entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);

                    if (hdnDepartmentORI.Value != entity.DepartmentID)
                    {
                        string filter = string.Format("ID = {0}", entity.ID);
                        List<GLPaymentTypeDt> lstDt = BusinessLayer.GetGLPaymentTypeDtList(filter, ctx);
                        foreach(GLPaymentTypeDt entityDt in lstDt)
                        {
                            entityDtDao.Delete(entityDt.ID, entityDt.HealthcareServiceUnitID);
                        }
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "ID tidak ditemukan.";
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