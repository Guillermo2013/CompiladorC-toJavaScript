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
    class ForNode:StatementNode
    {
        public List<StatementNode> expresionInicio;
        public ExpressionNode expresionCondicional;
        public List<StatementNode> expresionFinal;
        public List<StatementNode> cuerpo;
        public override void ValidateSemantic()
        {
            ContenidoStack._StackInstance.Stack.Push(new TablaSimbolos());

            if (expresionInicio != null)
            {
                foreach (var lista in expresionInicio)
                    lista .ValidateSemantic();
            }
            if (expresionCondicional != null)
            {
                var tipo = expresionCondicional.ValidateSemantic();
                if (!(tipo is BooleanTipo))
                {
                    throw new Semantico.SemanticoException("la expresion tiene que ser booleana" + expresionCondicional.token.Fila + "columna " + expresionCondicional.token.Columna);
                }
            }
            
            if (expresionFinal != null)
            {
                foreach (var lista in expresionFinal)
                    lista.ValidateSemantic();
            }
            ContenidoStack._StackInstance.Stack.Pop();

        }
        public override string GenerarCodigo()
        {
            string value = "for(";
            if(expresionInicio != null)
                foreach (var expresion in expresionInicio)
                {
                    if (expresion is DeclaracionVariableNode)
                        value += "var ";
                    value += expresion.GenerarCodigo();
                }
            value += ";";
            if (expresionCondicional != null)
                value += expresionCondicional.GenerarCodigo();
            value += ";";
            if(expresionFinal != null)
                foreach (var expresion in expresionFinal)
                {
                    value += expresion.GenerarCodigo();
                }
            value += ")\n{";
            foreach (var statemet in cuerpo)
            {
                value += "\n"+statemet.GenerarCodigo()+";";
            }
            value += "\n}";
            return value;
        }
    }
}
