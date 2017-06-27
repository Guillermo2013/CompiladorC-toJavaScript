using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;

namespace Compiladores.Arbol.StatementNodes
{
    class ContinueNode:StatementNode
    {
        public override void ValidateSemantic()
        {
           
        }
        public override string GenerarCodigo()
        {
            return "continue";
        }
    }
}
