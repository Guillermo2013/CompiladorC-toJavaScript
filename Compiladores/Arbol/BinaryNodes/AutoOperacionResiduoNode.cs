﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
using Compiladores.Arbol.Identificadores;
using Compiladores.Arbol.StatementNodes;
namespace Compiladores.Arbol.BinaryNodes
{
    class AutoOperacionResiduoNode:BinaryOperatorNode
    {
       
        public override TiposBases ValidateSemantic()
        {

            TiposBases expresion2;
            if (OperadorDerecho is CallFuntionNode)
            {
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;
                expresion2 = OperadorDerecho.ValidateSemantic();
            }
            else
                expresion2 = OperadorDerecho.ValidateSemantic();
            if (!(OperadorIzquierdo is IdentificadoresExpressionNode))
                throw new SemanticoException("no se puede asignar literales  fila " + token.Fila + " columna " + token.Columna);
            var expresion1 = OperadorIzquierdo.ValidateSemantic();

            if ((expresion1 is IntTipo || expresion1 is FloatTipo) && (expresion2 is FloatTipo || expresion2 is IntTipo))
                if (expresion1.GetType() == expresion2.GetType())
                    return expresion1;
            if (expresion1 is FloatTipo || expresion2 is FloatTipo)
                return new IntTipo();
            if (expresion1 is IntTipo && expresion1 is IntTipo)
                return new IntTipo();
            throw new SemanticoException("no se puede obtener residuo de esta division " + expresion1 + " con " + expresion2 + "fila " + token.Fila + " columna " + token.Columna);
        }

    }
}
