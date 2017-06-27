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
    public class SumaNode:BinaryOperatorNode
    {
        public override TiposBases ValidateSemantic()
        {
            if (OperadorDerecho is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;
            else if (OperadorIzquierdo is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;

            var expresion1 = OperadorIzquierdo.ValidateSemantic();
            var expresion2 = OperadorDerecho.ValidateSemantic();

            if (expresion1 is EnumTipo || expresion2 is EnumTipo || expresion1 is VoidTipo || expresion2 is VoidTipo||
                expresion1 is ClaseTipo || expresion2 is ClaseTipo)
                throw new SemanticoException(archivo+"no se puede sumar" + expresion1 + " con " + expresion2 + "fila " + token.Fila + " columna " + token.Columna);

            if (expresion1 is StringTipo || expresion2 is StringTipo)
                if (!(expresion2 is EnumTipo) && !(expresion1 is EnumTipo))
                        if (!(expresion2 is VoidTipo) && !(expresion1 is VoidTipo))
                            return new StringTipo();

            if (expresion1 is CharTipo && expresion2 is CharTipo)
                return new StringTipo();
            if (expresion1.GetType() == expresion2.GetType())
                return expresion1;
            if ((expresion1 is IntTipo && expresion2 is FloatTipo) || (expresion2 is IntTipo && expresion1 is FloatTipo))
                return new FloatTipo();
            if ((expresion1 is CharTipo && expresion2 is IntTipo) || (expresion2 is CharTipo && expresion1 is IntTipo))
                return new IntTipo();
            if ((expresion1 is BooleanTipo && expresion2 is IntTipo) || (expresion2 is BooleanTipo && expresion1 is IntTipo))
                return new IntTipo();

            throw new SemanticoException(archivo+"no se puede sumar" + expresion1 + " con " + expresion2 + "fila " + token.Fila + " columna " + token.Columna);
        }
        public override string GenerarCodigo()
        {
            return OperadorIzquierdo.GenerarCodigo() + " " + operador + " " + OperadorDerecho.GenerarCodigo();
        }
    }
}
