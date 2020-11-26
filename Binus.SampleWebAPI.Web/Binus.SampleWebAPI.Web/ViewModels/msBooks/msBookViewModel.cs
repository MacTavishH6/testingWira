using System.Collections.Generic;
using Binus.SampleWebAPI.Model.Backend.msBook;

namespace Binus.SampleWebAPI.Web.ViewModels.msBooks
{
    public class msBookViewModel
    {
        public msBookMSSQLOracle Book { get; set; }
        public List<msBookMSSQLOracle> Books { get; set; }

        public msBookMonggoDB BookMongoDB { get; set; }
        public List<msBookMonggoDB> BooksMongoDB { get; set; }

    }
}