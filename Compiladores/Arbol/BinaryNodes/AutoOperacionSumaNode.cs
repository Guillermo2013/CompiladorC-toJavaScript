using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
using Compiladores.Arbol.StatementNodes;
namespace Compiladores.Arbol.BinaryNodes
{
    class AutoOperacionSumaNode:BinaryOperatorNode
    {
       
        public override TiposBases ValidateSemantic()
        {

           TiposBases expresion2;
           if (OperadorDerecho is CallFuntionNode)
               (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;
           else if (OperadorIzquierdo is CallFuntionNode)
               (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;

               expresion2 = OperadorDerecho.ValidateSemantic();
           if (OperadorIzquierdo == null)
               return expresion2;
            if (!(OperadorIzquierdo is Identificadores.IdentificadoresExpressionNode))
                throw new SemanticoException(archivo+"no se puede asignar literales  fila " + token.Fila + " columna " + token.Columna);
            var expresion1 = OperadorIzquierdo.ValidateSemantic();
            if ( expresion1 is VoidTipo || expresion2 is VoidTipo|| expresion1 is EnumTipo || expresion2 is EnumTipo )
                throw new SemanticoException(archivo+"no se puede sumar" + expresion1 + " con " + expresion2 + "fila " + token.Fila + " columna " + token.Columna);

            if (expresion1 is StringTipo || expresion2 is StringTipo)
                return new StringTipo();
            if (expresion1 is IntTipo && expresion2 is IntTipo)
                return new StringTipo();
            if ((expresion1 is IntTipo && expresion2 is FloatTipo) || (expresion2 is IntTipo && expresion1 is FloatTipo))
                return new FloatTipo();
            if ((expresion1 is CharTipo && expresion2 is IntTipo) || (expresion2 is CharTipo && expresion1 is IntTipo))
                return new IntTipo();
            if (expresion1 == expresion2)
                return expresion1;
            
            throw new SemanticoException(archivo+"no se puede sumar" + expresion1 + " con " + expresion2 + "fila " + token.Fila + " columna " + token.Columna);
        }
        public override string GenerarCodigo()
        {
            return OperadorIzquierdo.GenerarCodigo() + " " + operador + " " + OperadorDerecho.GenerarCodigo();
        }
    }
}
