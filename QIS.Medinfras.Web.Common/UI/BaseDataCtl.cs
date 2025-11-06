using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Common.UI
{
    public abstract class BaseDataCtl : System.Web.UI.UserControl
    {
        protected List<Words> words;
        protected void LoadWords()
        {
            words = Helper.LoadWords(this);
        }
        public string GetLabel(string code)
        {
            return Helper.GetWordsLabel(words, code);
        }
        protected override void OnLoad(EventArgs e)
        {
            LoadWords();
        }

        public abstract void InitializeDataControl(string param);
    }
}
