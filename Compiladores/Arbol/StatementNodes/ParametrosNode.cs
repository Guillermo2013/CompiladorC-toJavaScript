using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;

namespace Compiladores.Arbol.StatementNodes
{
    class ParametrosNode:ExpressionNode
    {
        public DefinicionTipoNode tipo;
        public string nombre;
        public override TiposBases ValidateSemantic()
        {
            return  tipo.ValidateSemantic();
            
        }
        public override string GenerarCodigo()
        {
            return nombre;
        }
    }
}
