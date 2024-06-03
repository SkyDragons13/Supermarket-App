using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Model
{
    public class AddedProdus
    {
        public string Produs_nume { get; set; }
        public int id { get; set; }
        public int Cod_bare { get; set; }
        public int Cantitate { get; set; }
        public int Pret { get; set; }
        public int PretTotal { get; set; }
    }
}
