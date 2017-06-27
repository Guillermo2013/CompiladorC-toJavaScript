using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
namespace Compiladores.Arbol.Literales
{
    class LiteralBinaria : ExpressionNode
    {
        public byte valor { get; set; }
        public override TiposBases ValidateSemantic()
        {
            return new BinarioTipo();
        }
        public override string GenerarCodigo()
        {
            return valor.ToString().Replace("_","");
        }
    }
}
