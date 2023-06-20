using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.IO.ExcelUtil
{
    public class CellFormula
    {
        public CellFormula()
        {
            this.UsedFormula = false;
        }
        public bool UsedFormula { get; set; }

        public string FormulaString { get; set; }

    }
}
