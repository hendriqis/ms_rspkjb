using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QIS.Medinfras.Web.CustomControl
{
    public class QISQuickEntryHintColumn
    {
        #region DataMembers

        string _Caption = string.Empty;
        string _Width = string.Empty; 
        string _FieldName = string.Empty;
        #endregion

        #region Public Propeties

        [NotifyParentProperty(true)]
        [Description("Name of the FieldName")]
        public string Caption
        {
            get { return (_Caption != null) ? _Caption : string.Empty; }
            set { _Caption = value; }
        }

        [NotifyParentProperty(true)]
        [Description("Name of the FieldName")]
        public string Width
        {
            get { return (_Width != null) ? _Width : string.Empty; }
            set { _Width = value; }
        }

        [NotifyParentProperty(true)]
        [Description("Name of the FieldName")]
        public string FieldName
        {
            get { return (_FieldName != null) ? _FieldName : string.Empty; }
            set { _FieldName = value; }
        }

        #endregion
    }
}