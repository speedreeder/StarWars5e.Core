using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class PlayerHandbookManager
    {
        private readonly ITableStorage _tableStorage;

        public PlayerHandbookManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task Parse()
        {
        }
    }
}
