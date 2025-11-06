using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common
{
    [Serializable]
    public class ControlEntrySetting
    {
        private bool _IsEditAbleInAddMode;
        private bool _IsEditAbleInEditMode;
        private bool _IsRequired;
        private object _DefaultValue;

        public ControlEntrySetting(bool isEditAbleInAddMode, bool isEditAbleInEditMode)
            : this(isEditAbleInAddMode, isEditAbleInEditMode, false, "")
        {
        }

        public ControlEntrySetting(bool isEditAbleInAddMode, bool isEditAbleInEditMode, bool isRequired)
            : this(isEditAbleInAddMode, isEditAbleInEditMode, isRequired, "")
        {
        }

        public ControlEntrySetting(bool isEditAbleInAddMode, bool isEditAbleInEditMode, bool isRequired, object defaultValue)
        {
            _IsEditAbleInAddMode = isEditAbleInAddMode;
            _IsEditAbleInEditMode = isEditAbleInEditMode;
            _IsRequired = isRequired;
            _DefaultValue = defaultValue;
        }

        public bool IsEditAbleInAddMode
        {
            get { return _IsEditAbleInAddMode; }
            set { _IsEditAbleInAddMode = value; }
        }

        public bool IsEditAbleInEditMode
        {
            get { return _IsEditAbleInEditMode; }
            set { _IsEditAbleInEditMode = value; }
        }

        public bool IsRequired
        {
            get { return _IsRequired; }
            set { _IsRequired = value; }
        }

        public object DefaultValue
        {
            get { return _DefaultValue; }
            set { _DefaultValue = value; }
        }
    }
}
