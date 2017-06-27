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
    class DeplazamientoIzquierdaNode:BinaryOperatorNode
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
                throw new SemanticoException(archivo+" la expresion debe ser de tipo Int ");
            if (expresion2 is IntTipo || expresion2 is CharTipo || expresion2 is BinarioTipo)
                return expresion2;
            throw new SemanticoException(archivo+" no se puede hacer corrimiento de tipos " + expresion2);
        }
        public override string GenerarCodigo()
        {
            return OperadorIzquierdo.GenerarCodigo() + " " + operador + " " + OperadorDerecho.GenerarCodigo();
        }
    }
}
