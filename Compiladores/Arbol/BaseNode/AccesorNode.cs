using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Arbol.BaseNode
{
    public  class AccesorNode : ExpressionNode
    {
        public override Semantico.TiposBases ValidateSemantic()
        {
            return null;
        }
    }
}
