using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Arbol.Identificadores;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
using Compiladores.Arbol.AccesoresNode;

namespace Compiladores.Arbol.BinaryNodes
{
    class AsNode:BinaryOperatorNode
    {
        public List<AccesorNode> ListaDeAccesores = new List<AccesorNode>();
        public override TiposBases ValidateSemantic()
        {
            if (!(OperadorIzquierdo is IdentificadoresExpressionNode))
                throw new SemanticoException(archivo+"no se puede castear literales  fila " + token.Fila + " columna " + token.Columna);
            var expresion1 = OperadorIzquierdo.ValidateSemantic();
            var expresion2 = OperadorDerecho.ValidateSemantic();
            if (ListaDeAccesores.Count == 0)
            {
                return expresion2;
            }
            else if (!(expresion2 is ClaseTipo) && ListaDeAccesores.Count > 0)
                throw new SemanticoException(archivo + "no la variable se asigna  fila" + token.Fila + "columna" + token.Columna);
            if ((expresion2 is ClaseTipo) && ListaDeAccesores.Count > 0)
                foreach (var accesores in ListaDeAccesores)
                {
                    if (accesores is PuntoAccesor)
                    {
                        (accesores as PuntoAccesor).clase = (expresion2 as ClaseTipo).nombreClase;
                        expresion2 = accesores.ValidateSemantic();

                    }
                   
                }
            return expresion2;
           
        }
        public override string GenerarCodigo()
        {
            string valor = OperadorIzquierdo.GenerarCodigo()+"="+"( Object.create(" + OperadorDerecho.GenerarCodigo() + "))";
            foreach (var list in ListaDeAccesores)
                valor += list.GenerarCodigo();
            return valor;
        }
    }
}
