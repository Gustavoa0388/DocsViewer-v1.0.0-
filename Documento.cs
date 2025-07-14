using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsViewer
{
    public class Documento
    {
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public DateTime Data { get; set; }
        public string Usuario { get; set; }        // Novo campo
        public double TamanhoMb { get; set; }      // Novo campo (em MB, pode ser decimal também)

        // ...outros campos


    }
}
