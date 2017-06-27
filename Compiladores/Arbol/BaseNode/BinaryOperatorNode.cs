using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Arbol.BaseNode
{
    public  class BinaryOperatorNode : ExpressionNode
    {
        public string operador { get; set; }
        public ExpressionNode OperadorDerecho;
        public ExpressionNode OperadorIzquierdo;
        public override Semantico.TiposBases ValidateSemantic()
        {
            throw new NotImplementedException();
        }
        public override string GenerarCodigo()
        {
            return "";
        }

    }
}
