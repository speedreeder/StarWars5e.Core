using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentBackgroundsManager
    {
        private readonly ITableStorage _tableStorage;

        public ExpandedContentBackgroundsManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task Parse()
        {
        }
    }
}
