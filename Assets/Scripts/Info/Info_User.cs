using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Info
{
    public class Info_User
    {

        public string user_id
        {
            get
            {
                return ID;
            }
            set
            {
                ID = value;
            }
        }
        public string user_name
        {
            get
            {
                return NAME;
            }
            set
            {
                NAME = value;
            }
        }

        #region VARIABLE_UserInfo
        private string ID;
        private string NAME;
        #endregion
    }
}
