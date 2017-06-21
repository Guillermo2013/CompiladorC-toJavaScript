using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
namespace Compiladores.Arbol.StatementNodes
{
    class UnaryStatemetNode:StatementNode
    {
        public UnaryOperatorNode expresion;
       
        public override void ValidateSemantic()
        {
            
        }
    }
}
