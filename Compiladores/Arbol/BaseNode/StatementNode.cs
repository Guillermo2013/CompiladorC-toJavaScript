using Compiladores.Semantico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Arbol.BaseNode
{
    public abstract class StatementNode
    {
         public Token token = null;
         public abstract void ValidateSemantic();
    }
}
