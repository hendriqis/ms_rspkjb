using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QIS.Medinfras.Web.CustomControl
{
    public class TableItemStyle
    {
        HorizontalAlign _HorizontalAlign = HorizontalAlign.Left;
        VerticalAlign _VerticalAlign = VerticalAlign.Top;

        [Category("Layout")]
        [NotifyParentProperty(true)]
        [Description("TableItem_HorizontalAlign")]
        public HorizontalAlign HorizontalAlign
        {
            get { return _HorizontalAlign; }
            set { _HorizontalAlign = value; }
        }

        [NotifyParentProperty(true)]
        [Description("TableItem_VerticalAlign")]
        [Category("Layout")]
        public virtual VerticalAlign VerticalAlign
        {
            get { return _VerticalAlign; }
            set { _VerticalAlign = value; }
        }
    }
    public abstract class QISGridViewColumn
    {
        #region DataMembers

        string _Caption = string.Empty;
        string _Width = string.Empty;
        TableItemStyle _HeaderStyle = new TableItemStyle();
        TableItemStyle _ItemStyle = new TableItemStyle();

        #endregion

        #region Public Propeties

        [NotifyParentProperty(true)]
        [Description("Name of the FieldName")]
        public string Caption
        {
            get { return (_Caption != null) ? _Caption : string.Empty; }
            set { _Caption = value; }
        }

        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Styles")]
        [Description("DataControlField_HeaderStyle")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TableItemStyle HeaderStyle { get { return _HeaderStyle; } }

        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Styles")]
        [Description("DataControlField_ItemStyle")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TableItemStyle ItemStyle { get { return _ItemStyle; } }

        [NotifyParentProperty(true)]
        [Description("Name of the FieldName")]
        public string Width
        {
            get { return (_Width != null) ? _Width : string.Empty; }
            set { _Width = value; }
        }

        #endregion
    }

    public class QISGridViewDataColumn : QISGridViewColumn
    {
        string _FieldName = string.Empty;

        [NotifyParentProperty(true)]
        [Description("Name of the FieldName")]
        public string FieldName
        {
            get { return (_FieldName != null) ? _FieldName : string.Empty; }
            set { _FieldName = value; }
        }
    }

    public class QISGridViewTemplateColumn : QISGridViewColumn
    {
        [Browsable(false)]
        [TemplateContainer(typeof(IDataItemContainer), BindingDirection.TwoWay)]
        [DefaultValue("")]
        [Description("TemplateField_ItemTemplate")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate ItemTemplate { get; set; }

        [Browsable(false)]
        [TemplateContainer(typeof(IDataItemContainer), BindingDirection.TwoWay)]
        [DefaultValue("")]
        [Description("TemplateField_ItemTemplate")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate HeaderTemplate { get; set; }
    }
}