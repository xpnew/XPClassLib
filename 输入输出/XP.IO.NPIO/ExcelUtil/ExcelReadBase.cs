using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Comm.Msgs;

namespace XP.IO.ExcelUtil
{
    public class ExcelReadBase: BaseEntityVSMsg<Comm.CommMsg>
    {
        private string _FilePath;

        public string PhysicalFilePath
        {
            get
            {
                return _FilePath;
            }
        }

        /// <summary>
        /// 内部使用的工作薄（excel）
        /// </summary>
        private IWorkbook _WorkBook;

        protected IWorkbook WorkBook
        {
            get { return _WorkBook; }
            set { _WorkBook = value; }
        }

        /// <summary>
        /// 原点（需要废弃）
        /// </summary>
        public OriginCell Origin { get; set; }

        /// <summary>

        /// <summary>
        /// 结果信息
        /// </summary>
        public ExcelResultInfo ResultInfo { get; set; }




        public ExcelReadBase()
        {

            _Init();
        }

        public ExcelReadBase(string path)
            : this()
        {
            _FilePath = path;

        }


        protected virtual void _Init()
        {
            ResultInfo = new ExcelResultInfo();
        }


        /// <summary>
        /// 尝试打开Excel文件，并且获取工作薄
        /// </summary>
        protected void TryWorkbook()
        {
            var ep = new ExcelProvider(_FilePath);

            if (ep.ResultInfo.Success)
            {
                this.WorkBook = ep.Workbook;
            }
            else
            {
                this.ResultInfo = ep.ResultInfo;
            }
            ep = null;
        }



    }
}
