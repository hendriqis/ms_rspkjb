using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QIS.Medinfras.Web.CustomControl
{
    public class QISQuickEntryHint
    {
        #region DataMembers

        string _Text = string.Empty;
        string _TextField = string.Empty;
        string _ValueField = string.Empty;
        string _Description = string.Empty;
        string _FilterExpression = string.Empty;
        string _MethodName = string.Empty;

        #endregion

        #region Public Propeties

        [NotifyParentProperty(true)]
        [Description("Name of the FieldName")]
        public string Text
        {
            get { return (_Text != null) ? _Text : string.Empty; }
            set { _Text = value; }
        }

        [NotifyParentProperty(true)]
        [Description("Name of the FieldName")]
        public string TextField
        {
            get { return (_TextField != null) ? _TextField : string.Empty; }
            set { _TextField = value; }
        }

        [NotifyParentProperty(true)]
        [Description("Name of the FieldName")]
        public string ValueField
        {
            get { return (_ValueField != null) ? _ValueField : string.Empty; }
            set { _ValueField = value; }
        }

        [NotifyParentProperty(true)]
        [Description("Name of the FieldName")]
        public string Description
        {
            get { return (_Description != null) ? _Description : string.Empty; }
            set { _Description = value; }
        }

        [NotifyParentProperty(true)]
        [Description("Name of the FieldName")]
        public string FilterExpression
        {
            get { return (_FilterExpression != null) ? _FilterExpression : string.Empty; }
            set { _FilterExpression = value; }
        }

        [NotifyParentProperty(true)]
        [Description("Name of the FieldName")]
        public string MethodName
        {
            get { return (_MethodName != null) ? _MethodName : string.Empty; }
            set { _MethodName = value; }
        }

        private List<QISQuickEntryHintColumn> _Columns = new List<QISQuickEntryHintColumn>();
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<QISQuickEntryHintColumn> Columns
        {
            get { return _Columns; }
            set { _Columns = value; }
        }
        #endregion
    }
}