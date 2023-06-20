using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Util;

namespace XP.Img.GDIPlus.Draw
{
    /// <summary>
    /// 多张图合并到一张图上
    /// </summary>
    public class MuitlPic2One : BaseDraw
    {
        public string PicPhysicalDir { get; set; }



        public List<PartDrawItem> PartList { get; set; }


        private List<PartDrawItem> _LeftPartList;
        private List<PartDrawItem> _RightPartList;



        public void AddItem(PartDrawItem item)
        {

        }


        public void AddImg(string filePhysical)
        {
            

        }

        /// <summary>
        /// 混合图片，重新随机、排列
        /// </summary>
        public void MixPart()
        {

            if (null == PartList || 0 == PartList.Count)
            {
                return;
            }

            int TotalNum = PartList.Count;

            if (5 > TotalNum)
            {
                return;
            }
            PartList = ArrayUtility.GetRandomList(PartList);
            _LeftPartList = new List<PartDrawItem>();
            _RightPartList = new List<PartDrawItem>();

            int Harf = PartList.Count / 2;

            Harf += PartList.Count % 2 * GeneralTool.Random(0, 1);

            for (int i = 0; i < Harf; i++)
            {
                _LeftPartList.Add(PartList[i]);
            }

            for (int i = Harf; i < PartList.Count; i++)
            {
                _RightPartList.Add(PartList[i]);
            }
        }


    }
}
