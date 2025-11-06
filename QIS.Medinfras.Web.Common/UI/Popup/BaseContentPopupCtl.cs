using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Common.UI
{
    public abstract class BaseContentPopupCtl : System.Web.UI.UserControl 
    {
        public string PopupTitle { get; set; }
        public string TagProperty { get; set; }

        #region Words
        protected List<Words> words;
        protected void LoadWords()
        {
            words = Helper.LoadWords(this);
        }
        public string GetLabel(string code)
        {
            return Helper.GetWordsLabel(words, code);
        }
        #endregion

        public virtual void LoadMasterControl()
        {
        }

        public virtual void InitializeControl(string param)
        {
            LoadWords();
        }
    }
}
