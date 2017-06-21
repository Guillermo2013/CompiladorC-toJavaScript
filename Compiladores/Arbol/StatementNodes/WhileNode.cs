using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico.Tipos;
using Compiladores.Semantico;

namespace Compiladores.Arbol.StatementNodes
{
    class WhileNode:StatementNode
    {
        public ExpressionNode expresion;
        public List<StatementNode> cuerpo;
        public override void ValidateSemantic()
        {
            var condicionalTipe = expresion.ValidateSemantic();
            if (condicionalTipe is BooleanTipo)
            {
                ContenidoStack.InstanceStack.Stack.Push(new TablaSimbolos());
                foreach (var sentencias in cuerpo)
                    sentencias.ValidateSemantic();
                ContenidoStack.InstanceStack.Stack.Pop();
            }
            else
                throw new SemanticoException("la condiciona debe de ser tipo booleano fila " + expresion.token.Fila + " columna " + expresion.token.Columna);
        }
    }
}
