using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Win.BaseForms
{
    public class FormCloseEventArgs : System.EventArgs
    {

        public FormResultDef FormResult { get; set; } = FormResultDef.Default;


        public FormCloseEventArgs() : base()
        {

        }

        public FormCloseEventArgs( FormResultDef result) : this()
        {
            FormResult = result;
        }


    }
}
