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
    class LiteralBooleana : ExpressionNode
    {
        public bool valor { get; set; }
        public override TiposBases ValidateSemantic()
        {
            return new BooleanTipo();
        }
        public override string GenerarCodigo()
        {
            return valor.ToString();
        }
    }
}
