using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReproducingSlowDataGridRendering
{
    class ExampleTableRow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime? RandomDate { get; set; }
        public bool Toggle { get; set; }
        public bool? OptionalToggle { get; set; }

    }
}
