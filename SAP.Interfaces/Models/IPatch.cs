using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Interfaces.Models
{
    public interface IPatch
    {
        public int Id { get; set; }
        public Uri? Uri { get; set; }
        public int Size { get; set; }
        public PatchState State { get; set; }
    }
}
