using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;

namespace Compiladores.Arbol.StatementNodes
{
    class StatementExpresion:StatementNode
    {
        public ExpressionNode UnarioOperador;
        public ExpressionNode inicializacion;
        public ExpressionNode expresion;
        public List<ExpressionNode> expresionP;
        public override void ValidateSemantic()
        {
            expresion.ValidateSemantic();
        }
        public override string GenerarCodigo()
        {
            return "";
        }       
    }
}
