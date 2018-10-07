using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Info
{
    public class Info_Map
    {

        public struct BlockInfo
        {
            public int index;
            public int type;
            public int height;

            public BlockInfo(int _index, int _type, int _height)
            {
                index = _index;
                type = _type;
                height = _height;
            }
        }

        public List<BlockInfo> MapBlockInfo
        {
            get
            {
                return mapBlockInfo;
            }
            set
            {
                mapBlockInfo = value;
            }
        }

        #region VARIABLE_UserInfo
        private List<BlockInfo> mapBlockInfo = new List<BlockInfo>();
        #endregion
    }
}