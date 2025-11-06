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
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLMappingStandardCodeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_MAPPING_STANDARD_CODE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Request.QueryString.Count > 0)
            {
                hdnStandardCodeID.Value = Request.QueryString["id"];

                string filterParameter = string.Format("StandardCodeID = '{0}'", hdnStandardCodeID.Value);
                List<vGLStandardCodeCOA> lstEntity = BusinessLayer.GetvGLStandardCodeCOAList(filterParameter);
                if (lstEntity.Count() > 0)
                {
                    IsAdd = false;
                    EntityToControl(lstEntity.FirstOrDefault());
                }
                else
                {
                    IsAdd = true;
                    StandardCode sc = BusinessLayer.GetStandardCode(hdnStandardCodeID.Value);
                    txtStandardCodeID.Text = sc.StandardCodeID;
                    txtStandardCodeName.Text = sc.StandardCodeName;
                }

            }

            txtGLAccountNo.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtStandardCodeID, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtStandardCodeName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnGLAccountID_ORI, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnGLAccountID, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtGLAccountNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGLAccountName, new ControlEntrySetting(false, false, true));

        }

        private void EntityToControl(vGLStandardCodeCOA entity)
        {
            txtStandardCodeID.Text = entity.StandardCodeID;
            txtStandardCodeName.Text = entity.StandardCodeName;
            hdnGLAccountID_ORI.Value = hdnGLAccountID.Value = entity.GLAccountID.ToString();
            txtGLAccountNo.Text = entity.GLAccountNo;
            txtGLAccountName.Text = entity.GLAccountName;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLStandardCodeCOADao entityDao = new GLStandardCodeCOADao(ctx);

            try
            {
                if (hdnGLAccountID.Value != null && hdnGLAccountID.Value != "" && hdnGLAccountID.Value != "0")
                {
                    GLStandardCodeCOA entity = new GLStandardCodeCOA();
                    entity.StandardCodeID = txtStandardCodeID.Text;
                    entity.GLAccountID = Convert.ToInt32(hdnGLAccountID.Value);
                    entityDao.Insert(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Harap pilih COA terlebih dahulu.";
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLStandardCodeCOADao entityDao = new GLStandardCodeCOADao(ctx);

            try
            {
                if (hdnGLAccountID.Value != null && hdnGLAccountID.Value != "" && hdnGLAccountID.Value != "0")
                {
                    if (hdnGLAccountID_ORI.Value != null && hdnGLAccountID_ORI.Value != "" && hdnGLAccountID_ORI.Value != "0")
                    {
                        entityDao.Delete(txtStandardCodeID.Text, Convert.ToInt32(hdnGLAccountID_ORI.Value));
                    }

                    GLStandardCodeCOA entityNew = new GLStandardCodeCOA();
                    entityNew.StandardCodeID = txtStandardCodeID.Text;
                    entityNew.GLAccountID = Convert.ToInt32(hdnGLAccountID.Value);
                    entityDao.Insert(entityNew);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Harap pilih COA terlebih dahulu.";
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