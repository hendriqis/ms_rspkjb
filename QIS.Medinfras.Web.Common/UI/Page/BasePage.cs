using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Xml.Linq;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Common.UI
{
    public abstract class BasePage : Page
    {
        protected List<Words> words;

        protected void LoadWords()
        {
            words = Helper.LoadWords(this);
        }

        public List<Words> GetWords()
        {
            return words;
        }

        public string GetLabel(string code)
        {
            return Helper.GetWordsLabel(words, code);
        }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            if (!Page.IsCallback)
            {
                LoadWords();
            }
        }
    }
}
