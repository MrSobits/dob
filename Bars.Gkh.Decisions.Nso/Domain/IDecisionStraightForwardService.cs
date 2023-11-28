using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Bars.B4;

namespace Bars.Gkh.Decisions.Nso.Domain
{
    public interface IDecisionStraightForwardService
    {
        IDataResult GetConfirm(BaseParams baseParams);
    }
}
