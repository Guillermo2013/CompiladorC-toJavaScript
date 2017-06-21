using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;

namespace Compiladores.Arbol.BinaryNodes
{
    class DeplazamientoDerechaNode:BinaryOperatorNode
    {
        public override TiposBases ValidateSemantic()
        {
            if (OperadorDerecho is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;
            else if (OperadorIzquierdo is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;

            var expresion1 = OperadorDerecho.ValidateSemantic();
            var expresion2 = OperadorIzquierdo.ValidateSemantic();
       
            if (!(expresion1 is IntTipo))
                throw new SemanticoException(" la expresion debe ser de tipo Int ");
            if (expresion2 is IntTipo || expresion2 is CharTipo || expresion2 is BinarioTipo)
                return expresion2;
            throw new SemanticoException(" no se puede hacer corrimiento de tipos " + expresion2);
        }
    }
}
