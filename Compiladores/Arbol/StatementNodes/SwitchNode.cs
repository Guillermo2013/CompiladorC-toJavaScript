using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Arbol.StatementNodes;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
namespace Compiladores.Arbol.StatementNodes
{
    class SwitchNode:StatementNode
    {
        public ExpressionNode expresion;
        public List<CasosNode> casos;
        public override void ValidateSemantic()
        {
            ContenidoStack.InstanceStack.Stack.Push(new TablaSimbolos());
            var expresionTipo = expresion.ValidateSemantic();
            foreach (CasosNode statement in casos)
            {
                var tipocaso = statement.ValidateSemantic();
                if (expresionTipo.GetType() != tipocaso.GetType())
                    throw new SemanticoException("la expresion de los cases tiene que ser " + expresionTipo + "fila" + statement.expresion.token.Fila
                        + "columna" + statement.expresion.token.Columna );
            }
            ContenidoStack.InstanceStack.Stack.Pop();
        }
 
    }
}
