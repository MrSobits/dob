using Bars.B4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.DomainService
{
    public interface ISubpoena
    {

        IDataResult ComissionListSubpoena(BaseParams baseParams);

        IDataResult ListView(BaseParams baseParams);
    }
}
