using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;

namespace Compiladores.Arbol.StatementNodes
{
    class ConstructorNode:StatementNode
    {
        public string encapsulamiento;
        public string identificador;
        public List<ParametrosNode> parametro;
        public List<ExpressionNode> baseParametos;
        public List<StatementNode> constructorCuerpo;
        public override void ValidateSemantic()
        {
            ContenidoStack.InstanceStack.Stack.Push(new TablaSimbolos());
            foreach (var statement in parametro)
            {
                var tipo = statement.ValidateSemantic();
                ContenidoStack._StackInstance.Stack.Peek().DeclareVariable((statement as ParametrosNode).nombre, tipo);
            }
            foreach (var statement in constructorCuerpo)
            {
                statement.ValidateSemantic();
                if (statement is ReturnNode)
                    throw new SemanticoException("no se puede hacer retorno en un constructor fila"+statement.token.Fila+"columna"+statement.token.Columna);
            }
            ContenidoStack.InstanceStack.Stack.Pop();
        }
       
    }
}
