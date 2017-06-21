using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Arbol.Identificadores;
using Compiladores.Semantico.Tipos;
using Compiladores.Arbol.StatementNodes;

namespace Compiladores.Arbol.BinaryNodes
{
    class AutoDeplazamientoIzquierdaNode:BinaryOperatorNode
    {
  
        public override TiposBases ValidateSemantic()
        {
            TiposBases Derecho;

            if (OperadorDerecho is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;
            else if (OperadorIzquierdo is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;
                Derecho = OperadorDerecho.ValidateSemantic();
            if (OperadorIzquierdo == null)
                return Derecho;
            if (!(OperadorIzquierdo is IdentificadoresExpressionNode))
                throw new SemanticoException("no se puede asignar literales  fila " + token.Fila + " columna " + token.Columna);
            var Izquierdo = OperadorIzquierdo.ValidateSemantic();
            if (!(Derecho is IntTipo))
                throw new SemanticoException(" El incremeto debe ser de tipo int fila " + OperadorDerecho.token.Fila + " columna +" + OperadorDerecho.token.Columna);
            if (Izquierdo is IntTipo || Izquierdo is CharTipo)
                return new IntTipo();
            throw new SemanticoException("El tipo de dato no puede hacerse corrimiento Derecha  fila " + token.Fila + " columna " + token.Columna);
        }

    }
}
