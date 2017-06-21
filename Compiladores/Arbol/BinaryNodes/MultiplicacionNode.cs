using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico.Tipos;
using Compiladores.Semantico;

namespace Compiladores.Arbol.BinaryNodes
{
    public class MultiplicacionNode:BinaryOperatorNode
    {
        public override TiposBases ValidateSemantic()
        {
            var expresion1 = OperadorIzquierdo.ValidateSemantic();
            var expresion2 = OperadorDerecho.ValidateSemantic();
            if ((expresion1 is IntTipo || expresion1 is FloatTipo) && (expresion2 is FloatTipo || expresion2 is IntTipo))
                if (expresion1.GetType() == expresion2.GetType())
                    return expresion1;
            if (expresion1 is FloatTipo || expresion2 is FloatTipo)
                return new FloatTipo();
            if (expresion1 is IntTipo && expresion2 is IntTipo)
                return new IntTipo();
            throw new SemanticoException("no se puede restar" + expresion1 + " con " + expresion2 + "fila " + token.Fila + " columna " + token.Columna);
        }
    }
}
