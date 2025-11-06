using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace QIS.Medinfras.Web.Common.UI
{
    public abstract class BaseMPPopupEntry : System.Web.UI.UserControl
    {
        public abstract Control GetPanelEntryPopup();
        public abstract void SetIsAdd(bool IsAdd);
    }
}
