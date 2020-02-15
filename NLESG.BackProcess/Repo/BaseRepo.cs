using System;
using System.Collections.Generic;
using System.Text;
using SportData.CSGO;
using SportData.LOL;

namespace NLESG.BackProcess.Repo
{
    public class BaseRepo
    {
        public SportDataCSGOClient sportDataCSGOClient = null;
        public SportDataLOLClient sportDataLOLClient = null;

        public BaseRepo()
        {
            sportDataCSGOClient = new SportDataCSGOClient();
            sportDataLOLClient = new SportDataLOLClient();
        }
    }
}
