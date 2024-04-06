using SAP.Interfaces;
using SAP.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Application.Models
{
    public record Patch : IPatch
    {
        public int Id { get; set; }
        public Uri? Uri { get; set; }
        public int Size { get; set; }
        public PatchState State { get; set; }
    }
}
