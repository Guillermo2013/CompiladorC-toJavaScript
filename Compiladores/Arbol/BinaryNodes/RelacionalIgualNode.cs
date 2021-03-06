﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico.Tipos;
using Compiladores.Semantico;
namespace Compiladores.Arbol.BinaryNodes
{
    class RelacionalIgualNode:BinaryOperatorNode
    {
        public override TiposBases ValidateSemantic()
        {
            if (OperadorDerecho is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;
            else if (OperadorIzquierdo is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;

            var Derecho = OperadorDerecho.ValidateSemantic();
            var Izquierdo = OperadorIzquierdo.ValidateSemantic();
            if (Derecho is EnumTipo || Izquierdo is EnumTipo || Derecho is VoidTipo || Izquierdo is VoidTipo || Derecho is ClaseTipo || Izquierdo is ClaseTipo)
                throw new SemanticoException(archivo+" No se pude utilizar como variable " + Derecho + " con " + Izquierdo + "fila " + token.Fila + " columna " + token.Columna);
            return new BooleanTipo();
        }
        public override string GenerarCodigo()
        {
            return OperadorIzquierdo.GenerarCodigo() + " " + operador + " " + OperadorDerecho.GenerarCodigo();
        }
    }
}
