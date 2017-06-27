using Compiladores.Arbol.AccesoresNode;
using Compiladores.Arbol.BaseNode;
using Compiladores.Arbol.Identificadores;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Arbol.StatementNodes
{
    class ThisStatementNode:StatementNode
    {
        public ThisNode expresion;
        public ExpressionNode Asignacion;

        public override void ValidateSemantic()
        {
            var id = expresion.ValidateSemantic();
            if (Asignacion != null )
            {
            
                var expresionTipo = Asignacion.ValidateSemantic();
                if ((id is ClaseTipo && expresionTipo is ClaseTipo))
                {
                    if ((id as ClaseTipo).nombreClase != (expresionTipo as ClaseTipo).nombreClase)
                        throw new SemanticoException(archivo + "se tiene que asignar el mismo tipo " + (id as EnumTipo).nombreClase + "fila"
                            + expresion.token.Fila + "columna" + expresion.token.Columna);
                }
                else if ((id is EnumTipo && expresionTipo is EnumTipo))
                {
                    if ((id as EnumTipo).nombreClase != (expresionTipo as EnumTipo).nombreClase)
                        throw new SemanticoException(archivo + "se tiene que asignar el mismo tipo " + (id as EnumTipo).nombreClase + "fila"
                            + expresion.token.Fila + "columna" + expresion.token.Columna);
                }
                else if ((id is ArrayTipo) && (expresionTipo is ArrayTipo))
                {
                    var declarArray = (id as ArrayTipo).cantidad.ToArray();
                    var tipoDefinionArray = (expresionTipo as ArrayTipo).cantidad.ToArray();
                    if (tipoDefinionArray.Length != declarArray.Length)
                        throw new SemanticoException(archivo + "se tiene que asignar el mismo tipo al arreglo " + (expresion.expresion as IdentificadoresExpressionNode).nombre + "fila"
                            + Asignacion.token.Fila + "columna" + Asignacion.token.Columna);

                    for (int i = 0; i < declarArray.Length; i++)
                    {
                        if (tipoDefinionArray[i].Count != declarArray[i].Count)
                            throw new SemanticoException(archivo + "se tiene que asignar el mismo tipo al arreglo " + (expresion.expresion as IdentificadoresExpressionNode).nombre + "fila"
                                + Asignacion.token.Fila + "columna" + Asignacion.token.Columna);
                    }

                }
                if (id.GetType() != expresionTipo.GetType())
                    throw new SemanticoException(archivo + "no se puede asignar" + expresionTipo + "a " + id + "fila" + expresion.token.Fila + "columna" + expresion.token.Columna);

            }
            else
            {
                if (expresion.expresion is IdentificadoresExpressionNode)
                    validadFinal((expresion.expresion as IdentificadoresExpressionNode).ListaDeAccesores);
                else if (expresion.expresion is CallFuntionNode)
                    validadFinal((expresion.expresion as CallFuntionNode).ListaDeAccesores);
            }

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
            throw new SemanticoException(archivo + "la identificadores debe asignarse" + "fila" + this.token.Fila + "columna" + this.token.Columna);
        }
        public override string GenerarCodigo()
        {
            string valor = expresion.GenerarCodigo();
            if (Asignacion != null)
            {
                valor += " = " + Asignacion.GenerarCodigo();
            }
            valor += ";";
            return valor;
        }
    }
}
