using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common
{
    public class FormQuestionModel
    {
        public string QuestionID { get; set; }
        public string QuestionName { get; set; }
        public string QuestionValue { get; set; }
        public string ShortName { get; set; }
        public string SortID { get; set; }
    }
    public class FormMCUResultModel
    {
        public string SortID { get; set; }
        public string QuestionName { get; set; }
       // public string QuestionValue { get; set; }
        List<formMCUResultValue> Value { get; set; }
    }

    public class formMCUResultValue {
        public string Value { get; set; }
    }
    
}
