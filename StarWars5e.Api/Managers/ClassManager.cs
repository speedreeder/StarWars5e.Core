using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Class;

namespace StarWars5e.Api.Managers
{
    public class ClassManager : IClassManager
    {
        public Task<IEnumerable<Class>> SearchClasses()
        {
            throw new NotImplementedException();
        }
    }
}
