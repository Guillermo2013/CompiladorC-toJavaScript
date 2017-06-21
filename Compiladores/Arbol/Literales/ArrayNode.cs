using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Arbol.StatementNodes;
using Compiladores.Semantico.Tipos;

namespace Compiladores.Arbol.Literales
{
    class ArrayNode:ExpressionNode
    {
       public List<ExpressionNode> expresion;
       public DefinicionTipoNode tipo;
       
       public override Semantico.TiposBases ValidateSemantic()
       {
        
           return tipo.ValidateSemantic(); 
       }
       
    }
}
