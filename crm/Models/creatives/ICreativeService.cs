using crm.Models.geoservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.creatives
{
    public interface ICreativeService
    {
        Task Upload(GEO geo, string path);
    }

    public enum CreativeType
    {
        video,
        picture,
        any
    }
}
