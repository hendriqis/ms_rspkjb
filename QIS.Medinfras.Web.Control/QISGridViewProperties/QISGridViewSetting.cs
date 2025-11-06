using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace QIS.Medinfras.Web.CustomControl
{
    public class QISGridViewSetting
    {
        private Int32 _VerticalScrollBarHeight;
        private Int32 _PageCount;

        [Category("Layout")]
        [NotifyParentProperty(true)]
        [Description("TableItem_HorizontalAlign")]
        public Int32 VerticalScrollBarHeight
        {
            get { return _VerticalScrollBarHeight; }
            set { _VerticalScrollBarHeight = value; }
        }

        [Category("Layout")]
        [NotifyParentProperty(true)]
        [Description("TableItem_HorizontalAlign")]
        public Int32 PageCount
        {
            get { return _PageCount; }
            set { _PageCount = value; }
        }
    }
}
