using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace QIS.Medinfras.Web.CustomControl
{
    public class QISSearchTextBoxClientSideEvent
    {
        private String _LostFocus;

        [Category("Layout")]
        [NotifyParentProperty(true)]
        [Description("TableItem_HorizontalAlign")]
        public String LostFocus
        {
            get { return (_LostFocus == null ? "" : _LostFocus); }
            set { _LostFocus = value; }
        }
        private String _ValueChanged;

        [Category("Layout")]
        [NotifyParentProperty(true)]
        [Description("TableItem_HorizontalAlign")]
        public String ValueChanged
        {
            get { return (_ValueChanged == null ? "" : _ValueChanged); }
            set { _ValueChanged = value; }
        }

        private String _Init;

        [Category("Layout")]
        [NotifyParentProperty(true)]
        [Description("TableItem_HorizontalAlign")]
        public String Init
        {
            get { return (_Init == null ? "" : _Init); }
            set { _Init = value; }
        }
    }
}
