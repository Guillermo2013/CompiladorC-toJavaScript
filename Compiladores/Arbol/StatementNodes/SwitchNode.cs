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
                if (statement is DefaulNode)
                    (statement as DefaulNode).cuerpo = (statement as CasosNode).cuerpo;
                var tipocaso = statement.ValidateSemantic();
                
                if (expresionTipo.GetType() != tipocaso.GetType() && !(statement is DefaulNode))
                    throw new SemanticoException(archivo+"la expresion de los cases tiene que ser " + expresionTipo + "fila" + statement.expresion.token.Fila
                        + "columna" + statement.expresion.token.Columna );
            }
            ContenidoStack.InstanceStack.Stack.Pop();
        }
        public override string GenerarCodigo()
        {
            string valor = "switch("+expresion.GenerarCodigo()+")";
            valor += "\n{";
            foreach (var elemento in casos)
            {
                valor += "\n" + elemento.GenerarCodigo();
            }
            valor += "\n}";
            return valor;
        }
    }
}
