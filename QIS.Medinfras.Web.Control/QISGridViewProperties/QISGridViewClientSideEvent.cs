using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace QIS.Medinfras.Web.CustomControl
{
    public class QISGridViewClientSideEvent
    {
        private String _RowClick;
        private String _RowDblClick;
        private String _Init;

        [Category("Layout")]
        [NotifyParentProperty(true)]
        [Description("TableItem_HorizontalAlign")]
        public String RowClick
        {
            get { return (_RowClick == null ? string.Empty : _RowClick); }
            set { _RowClick = value; }
        }

        [Category("Layout")]
        [NotifyParentProperty(true)]
        [Description("TableItem_HorizontalAlign")]
        public String RowDblClick
        {
            get { return (_RowDblClick == null ? string.Empty : _RowDblClick); ; }
            set { _RowDblClick = value; }
        }

        [Category("Layout")]
        [NotifyParentProperty(true)]
        [Description("TableItem_HorizontalAlign")]
        public String Init
        {
            get { return _Init; }
            set { _Init = value; }
        }
    }
}
