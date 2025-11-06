using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class BodyDiagramEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.BODY_DIAGRAM;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                SetControlProperties();
                hdnID.Value = ID;
                BodyDiagram entity = BusinessLayer.GetBodyDiagram(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtDiagramCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }


        protected override void SetControlProperties()
        {
            List<StandardCode> lststd = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.BODY_DIAGRAM_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboDiagramGroup, lststd.Where(sc => sc.ParentID == Constant.StandardCode.BODY_DIAGRAM_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
            cboDiagramGroup.SelectedIndex = 0;
        }
        /*IRENE*/
        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtDiagramCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDiagramName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDiagramGroup, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtImageURL, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false)); ;
        }

        private void EntityToControl(BodyDiagram entity)
        {
            txtDiagramCode.Text = entity.DiagramCode;
            txtDiagramName.Text = entity.DiagramName;
            cboDiagramGroup.Value = entity.GCBodyDiagramGroup;
            txtImageURL.Text = entity.ImageUrl;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(BodyDiagram entity)
        {
            entity.DiagramCode = txtDiagramCode.Text;
            entity.DiagramName = txtDiagramName.Text;
            entity.GCBodyDiagramGroup = cboDiagramGroup.Value.ToString();
            entity.ImageUrl = txtImageURL.Text;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("DiagramCode = '{0}'", txtDiagramCode.Text);
            List<BodyDiagram> lst = BusinessLayer.GetBodyDiagramList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Body Diagram with Code " + txtDiagramCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            BodyDiagramDao entityDao = new BodyDiagramDao(ctx);
            bool result = false;
            try
            {
                BodyDiagram entity = new BodyDiagram();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetBodyDiagramMaxID(ctx).ToString();//memunculkan yg terakhr diadd
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                BodyDiagram entity = BusinessLayer.GetBodyDiagram(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateBodyDiagram(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}