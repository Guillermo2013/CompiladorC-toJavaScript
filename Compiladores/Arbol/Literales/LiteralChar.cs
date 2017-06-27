using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico.Tipos;
namespace Compiladores.Arbol.Literales
{
    class LiteralChar : ExpressionNode
    {
        public char valor { get; set; }
        public override Semantico.TiposBases ValidateSemantic()
        {
            return new CharTipo();
        }
        public override string GenerarCodigo()
        {
            return '\'' + valor.ToString() + '\'';
        }
    }
}
