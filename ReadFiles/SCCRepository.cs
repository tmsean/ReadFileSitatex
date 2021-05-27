using ReadFiles.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadFiles
{
    class SCCRepository
    {
        private readonly SCCContext _context = null;

        public SCCRepository(SCCContext context)
        {
            _context = context;
        }
        public static void AddNewDestination(DestinationTypeB model)
        {
            var newDestination = new DestinationTypeB()
            {
                Destionations = model.Destionations
            };
        }
    }
}
