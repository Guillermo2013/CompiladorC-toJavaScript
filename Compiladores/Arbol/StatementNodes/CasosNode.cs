using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
namespace Compiladores.Arbol.StatementNodes
{
    class CasosNode:ExpressionNode
    {
        public ExpressionNode expresion;
        public List<StatementNode> cuerpo;
        public override TiposBases ValidateSemantic()
        {
            var expresionTipo = expresion.ValidateSemantic();
            foreach (var statement in cuerpo)
                statement.ValidateSemantic();
            return expresionTipo;
        }
        public override string GenerarCodigo()
        {
            var value = "case " + expresion.GenerarCodigo() + " : ";
            foreach (var lista in cuerpo)
                value += "\n" + lista.GenerarCodigo();
            return value;
        }
    }
}
