using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace QIS.Medinfras.Web.CustomControl
{
    [Serializable]
    public class QISDataSourceBusinessLayer
    {
        public bool IsFilterExpressionChanged = true;
        private string _MethodName;
        private string _RowCountMethodName;
        private string _FilterExpression;

        [Category("Layout")]
        [NotifyParentProperty(true)]
        [Description("TableItem_HorizontalAlign")]
        public string MethodName
        {
            get { return _MethodName; }
            set { _MethodName = value; }
        }

        [Category("Layout")]
        [NotifyParentProperty(true)]
        [Description("TableItem_HorizontalAlign")]
        public string RowCountMethodName
        {
            get { return _RowCountMethodName; }
            set { _RowCountMethodName = value; }
        }

        [NotifyParentProperty(true)]
        [Description("TableItem_VerticalAlign")]
        [Category("Layout")]
        public string FilterExpression
        {
            get { return _FilterExpression; }
            set { IsFilterExpressionChanged = true; _FilterExpression = value; }
        }
    }
}
