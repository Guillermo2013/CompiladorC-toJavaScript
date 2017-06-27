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
                    throw new SemanticoException(archivo+"no se puede hacer retorno en un constructor fila"+statement.token.Fila+"columna"+statement.token.Columna);
            }
            ContenidoStack.InstanceStack.Stack.Pop();
        }
        public override string GenerarCodigo()
        {
            string value = "constructor(";
            if (parametro != null)
            {
                var parametroArray = parametro.ToArray();
                for (int i = 0; i < parametroArray.Length; i++)
                {
                    value += parametroArray[i].GenerarCodigo();
                    if (i < parametroArray.Length - 1)
                        value += ",";
                }
            }
            value += ")\n{";
            if (baseParametos != null && baseParametos.Count != 0)
            {
                value += "\n super(";
                var parametroArray = baseParametos.ToArray();
                for (int i = 0; i < parametroArray.Length; i++)
                {
                    value += parametroArray[i].GenerarCodigo();
                    if (i < parametroArray.Length - 1)
                        value += ",";
                }
                value += ");";
            }
            if (constructorCuerpo != null)
                foreach (var lista in constructorCuerpo)
                {
                    value += "\n"+lista.GenerarCodigo();

                }
                    
            value += "\n}";
            return value;
        }
    }
}
