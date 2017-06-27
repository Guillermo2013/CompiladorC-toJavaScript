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
    class IfNode:StatementNode
    {
        public ExpressionNode expresion;
        public List<StatementNode> cuerpo;
        public ElseNode elseVariable;
        public override void ValidateSemantic()
        {
        var condicionalTest = expresion.ValidateSemantic();
            if (!(condicionalTest is BooleanTipo))
                throw new SemanticoException(archivo+"la condicion debe de ser booleana fila "+token.Fila+"columna"+token.Columna);
            if (cuerpo != null)
            {
                ContenidoStack.InstanceStack.Stack.Push(new TablaSimbolos());
                foreach (var bloqueTrue in cuerpo)
                    if (bloqueTrue != null)
                        bloqueTrue.ValidateSemantic();
                ContenidoStack.InstanceStack.Stack.Pop();
            }
            if (elseVariable != null)
            {
               
                elseVariable.ValidateSemantic();
                
            }
        }
        public override string GenerarCodigo()
        {
            string value = "if(" + expresion.GenerarCodigo() + ")";
            value += "\n{";
            if(cuerpo != null)
                foreach (var lista in cuerpo)
                {
                    value += "\n"+lista.GenerarCodigo()+";";
                }
            value += "\n}";
            if (elseVariable != null)
                value += "\n" + elseVariable.GenerarCodigo();
            return value;
        }
    }
}
