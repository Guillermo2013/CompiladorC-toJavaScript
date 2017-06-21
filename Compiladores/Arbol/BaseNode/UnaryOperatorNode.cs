using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Arbol.BaseNode
{
    public  class UnaryOperatorNode : ExpressionNode
    {
        public string Value;
        public ExpressionNode Operando;
        public override Semantico.TiposBases ValidateSemantic()
        {
            throw new NotImplementedException();
        }
    }
}
