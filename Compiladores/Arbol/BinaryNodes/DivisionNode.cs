﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
namespace Compiladores.Arbol.BinaryNodes
{
    class DivisionNode:BinaryOperatorNode
    {
        public override TiposBases ValidateSemantic()
        {
            if (OperadorDerecho is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;
            else if (OperadorIzquierdo is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;

            var expresion1 = OperadorIzquierdo.ValidateSemantic();
            var expresion2 = OperadorDerecho.ValidateSemantic();
            if ((expresion1 is IntTipo || expresion1 is FloatTipo) && (expresion2 is FloatTipo || expresion2 is IntTipo))
            {
                if (expresion1 is FloatTipo || expresion2 is FloatTipo)
                    return new FloatTipo();
                if (expresion1 is IntTipo || expresion2 is IntTipo)
                    return new IntTipo();
            }
            throw new SemanticoException(archivo+"no se puede restar" + expresion1 + " con " + expresion2 + "fila " + token.Fila + " columna " + token.Columna);
        }
        public override string GenerarCodigo()
        {
            return OperadorIzquierdo.GenerarCodigo() + " " + operador + " " + OperadorDerecho.GenerarCodigo();
        }
    }
}
