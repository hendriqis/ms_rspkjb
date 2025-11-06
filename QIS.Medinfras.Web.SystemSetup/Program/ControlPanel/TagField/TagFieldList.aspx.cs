using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class TagFieldList : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.BUSINESS_OBJECT_CUSTOM_ATTRIBUTE;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            OnSetControlProperties();
            BindGrdView();
        }

        private void OnSetControlProperties()
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(String.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.BUSINESS_OBJECT_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboBusinessObject, lst, "StandardCodeName", "StandardCodeID");
        }

        private class TagFieldItem
        {
            public int Index { get; set; }
            public string TagFieldID { get; set; }
            public string Value { get; set; }
        }

        private List<TagFieldItem> GetListTagFieldItem(TagField entity)
        {
            List<TagFieldItem> lst = new List<TagFieldItem>();
            for (int i = 1; i <= 20; i++)
            {
                TagFieldItem item = new TagFieldItem();
                item.Index = i;
                item.TagFieldID = String.Format("Tagfield{0}", i);
                item.Value = entity.GetType().GetProperty("TagField" + i).GetValue(entity, null).ToString();
                lst.Add(item); 
            }
            return lst;
        }

        private void BindGrdView()
        {
            string businessObj = hdnBusinessObject.Value;
            List<TagFieldItem> lst;
            if (businessObj != "")
            { 
                TagField entity = BusinessLayer.GetTagField(businessObj);
                if (entity == null)
                {
                    entity = new TagField { GCBusinessObjectType = businessObj };
                    lst = new List<TagFieldItem>();
                    for (int i = 1; i <= 20; i++)
                    {
                        TagFieldItem item = new TagFieldItem();
                        item.Index = i;
                        item.TagFieldID = String.Format("Tagfield{0}", i);
                        item.Value = "";
                        lst.Add(item);
                    }
                }
                else
                    lst = GetListTagFieldItem(entity);
            }
            else
            {
                lst = new List<TagFieldItem>();
            }
            grdView.DataSource = lst;
            grdView.DataBind();
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGrdView();
        }

        protected override bool OnCustomButtonClick(string type, ref string retval, ref string errMessage)
        {
            if (type == "save")
            {
                try
                {
                    BindGrdView();

                    bool IsAdd = false;
                    TagField entity = BusinessLayer.GetTagField(cboBusinessObject.Value.ToString());
                    if (entity == null)
                    {
                        IsAdd = true;
                        entity = new TagField();
                        entity.GCBusinessObjectType = cboBusinessObject.Value.ToString();
                    }
                    int ctr = 1;
                    foreach (GridViewRow row in grdView.Rows)
                    {
                        TextBox txt = (TextBox)row.FindControl("txtValue");
                        String value = Request.Form[txt.UniqueID];
                        entity.GetType().GetProperty("TagField" + ctr).SetValue(entity, value, null);
                        ctr++;
                    }
                    if (IsAdd)
                        BusinessLayer.InsertTagField(entity);
                    else
                        BusinessLayer.UpdateTagField(entity);

                    return true;
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    return false;
                }
            }
            return false;
        }
    }
}