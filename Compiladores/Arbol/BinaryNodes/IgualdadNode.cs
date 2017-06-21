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
    public class IgualdadNode:BinaryOperatorNode
    {
        public override TiposBases ValidateSemantic()
        {
            if (OperadorDerecho is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;
            else if (OperadorIzquierdo is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;

            var Derecho = OperadorDerecho.ValidateSemantic();
            var Izquierdo = OperadorIzquierdo.ValidateSemantic();
            if (Derecho is ClaseTipo || Izquierdo is ClaseTipo || Derecho is EnumTipo || Izquierdo is EnumTipo || Derecho is VoidTipo || Izquierdo is VoidTipo)
                throw new SemanticoException(" No se pude compara " + Derecho + " con " + Izquierdo + "fila " + token.Fila + " columna " + token.Columna);
            return new BooleanTipo();
        }
    }
}
