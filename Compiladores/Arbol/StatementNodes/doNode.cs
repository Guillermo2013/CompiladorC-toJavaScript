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
    class doNode:StatementNode
    {
        public ExpressionNode expresion;
        public List<StatementNode> cuerpo;
        public override void ValidateSemantic()
        {
            ContenidoStack.InstanceStack.Stack.Push(new TablaSimbolos());
            foreach (var sentencia in cuerpo)
                sentencia.ValidateSemantic();
            ContenidoStack.InstanceStack.Stack.Pop();
            var condicionalEvaluar = expresion.ValidateSemantic();
            if (!(condicionalEvaluar is BooleanTipo))
                throw new SemanticoException("la condiciona debe de ser tipo booleano fila " + expresion.token.Fila + " columna " + expresion.token.Columna);
        }
    }
}
