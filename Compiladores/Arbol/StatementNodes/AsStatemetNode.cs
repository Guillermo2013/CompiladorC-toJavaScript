using Compiladores.Arbol.AccesoresNode;
using Compiladores.Arbol.BaseNode;
using Compiladores.Arbol.BinaryNodes;
using Compiladores.Arbol.Identificadores;
using Compiladores.Semantico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Arbol.StatementNodes
{
    class AsStatemetNode:StatementNode
    {
        public AsNode asVariable;
        public ExpressionNode Asignasion;
        public override void ValidateSemantic()
        {
            var variable = asVariable.ValidateSemantic();
            if (Asignasion == null)
                validadFinal(asVariable.ListaDeAccesores);
            else {
                validadFinalAsig(asVariable.ListaDeAccesores);
                var asignacion = Asignasion.ValidateSemantic();
                if(variable.GetType()!=asignacion.GetType())
                    throw new SemanticoException(archivo + "no se puede asignar" + asignacion + "a " + variable + "fila" + token.Fila + "columna" + token.Columna);
            } 
                
        }
        public override string GenerarCodigo()
        {
            return "";
        }
        private void validadFinal(List<AccesorNode> ListaDeAccesoresInterna)
        {
            foreach (var elementos in ListaDeAccesoresInterna)
            {
                if (elementos is PuntoAccesor)
                {
                    if (((elementos as PuntoAccesor).identificador is IdentificadoresExpressionNode))
                    {
                        if (((elementos as PuntoAccesor).identificador as IdentificadoresExpressionNode).ListaDeAccesores.Count == 0)
                        {
                            throw new SemanticoException(archivo + "la identificadores debe asignarse"
                                + ((elementos as PuntoAccesor).identificador as IdentificadoresExpressionNode).nombre
                                + "fila" + this.token.Fila + "columna" + this.token.Columna);
                        }
                        validadFinal(((elementos as PuntoAccesor).identificador as IdentificadoresExpressionNode).ListaDeAccesores);
                    }
                    else
                    {
                        validadFinal(((elementos as PuntoAccesor).identificador as CallFuntionNode).ListaDeAccesores);
                    }
                }
                else if (ListaDeAccesoresInterna.Last() is ArrayAccesor)
                    throw new SemanticoException(archivo + "la identificadores debe asignarse"
                                + ((elementos as PuntoAccesor).identificador as IdentificadoresExpressionNode).nombre
                                + "fila" + this.token.Fila + "columna" + this.token.Columna);
            }
        }
        private void validadFinalAsig(List<AccesorNode> ListaDeAccesoresInterna)
        {
            foreach (var elementos in ListaDeAccesoresInterna)
            {
                if (elementos is PuntoAccesor)
                {
                    if (((elementos as PuntoAccesor).identificador is CallFuntionNode))
                    {
                        if (((elementos as PuntoAccesor).identificador as CallFuntionNode).ListaDeAccesores.Count == 0)
                        {
                            throw new SemanticoException(archivo + "la identificadores debe asignarse"
                                + ((elementos as PuntoAccesor).identificador as CallFuntionNode).nombre
                                + "fila" + this.token.Fila + "columna" + this.token.Columna);
                        }
                        validadFinalAsig(((elementos as PuntoAccesor).identificador as CallFuntionNode).ListaDeAccesores);
                    }
                    else
                    {
                        validadFinalAsig(((elementos as PuntoAccesor).identificador as IdentificadoresExpressionNode).ListaDeAccesores);
                    }
                }
                else if (!(ListaDeAccesoresInterna.Last() is ArrayAccesor))
                    throw new SemanticoException(archivo + "la identificadores debe asignarse"
                                + ((elementos as PuntoAccesor).identificador as IdentificadoresExpressionNode).nombre
                                + "fila" + this.token.Fila + "columna" + this.token.Columna);
            }
        }
    }
}
