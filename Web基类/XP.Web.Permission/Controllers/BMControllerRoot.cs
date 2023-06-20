using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.DB.BLL;
using XP.Web.ControllerBase;

namespace XP.Web.Permission.Controllers
{
    public class BMControllerRoot : BassVsMsg
    {


        public UserIntegrationBLL UserService
        {
            get { return new UserIntegrationBLL(); }
        }
    }
}
