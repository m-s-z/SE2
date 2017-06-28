using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xsd2
{
    public partial class TaskField : Field
    {
        public TaskField()
        {
            this.timestamp = DateTime.Now.Date;
        }
    }
}
