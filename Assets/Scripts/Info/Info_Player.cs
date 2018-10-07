using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Info
{
    public static class Info_Player
    {
        public static string user_id
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
        public static string user_name
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

        #region VARIABLE_Info
        private static string ID;
        private static string NAME;
        #endregion
    }
}

