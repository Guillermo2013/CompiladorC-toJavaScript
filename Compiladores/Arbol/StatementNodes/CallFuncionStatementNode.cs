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
    class CallFuncionStatementNode:StatementNode
    {
        public string nombre;
        public List<ExpressionNode> parametros;
        public List<AccesorNode> ListaDeAccesores;
        public override void ValidateSemantic()
        {
            var encontro = false;
            encontro = validadFuncionLocal(encontro);
                encontro = validarHerencia(encontro);
                validadFinal(ListaDeAccesores);
            if (!encontro)
                throw new SemanticoException("no se encontro la funcion " + nombre + "fila" + this.token.Fila + "columna" + this.token.Columna);
        }

        private void validadFinal(List<AccesorNode> ListaDeAccesoresInterna)
        {
            foreach (var elementos in ListaDeAccesoresInterna)
            {
                if (elementos is PuntoAccesor)
                {
                    if ((elementos as PuntoAccesor).identificador is IdentificadoresExpressionNode)
                    {
                        if (((elementos as PuntoAccesor).identificador as IdentificadoresExpressionNode).ListaDeAccesores.Count == 0)
                        {
                            throw new SemanticoException("la identificadores debe asignarse"
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
            }
        }

        private bool validarHerencia(bool encontro)
        {
            foreach (var usingList in ContenidoStack._StackInstance.usingNombres)
            {
                if (TablaDeNamespace.Instance.Tabla.ContainsKey(usingList))
                {
                    var elementos = TablaDeNamespace.Instance.Tabla[usingList];
                    foreach (var elemento in elementos)
                    {
                        if (elemento is ClassNode)
                        {
                            if ((elemento as ClassNode).nombre == ContenidoStack.InstanceStack.claseActual)
                            {
                                var claseActuar = ContenidoStack.InstanceStack.claseActual;
                                foreach (var herencia in (elemento as ClassNode).herencia)
                                {
                                    ContenidoStack.InstanceStack.claseActual = (herencia as IdentificadoresExpressionNode).nombre;
                                    encontro = validadFuncionLocal(encontro);
                                    encontro = validarHerencia(encontro);
                                }
                                ContenidoStack.InstanceStack.claseActual = claseActuar;
                            }
                        }
                    }
                }
            }
            return encontro;
        }

        private bool validadFuncionLocal(bool encontro)
        {
            foreach (var usingList in ContenidoStack._StackInstance.usingNombres)
            {
                if (TablaDeNamespace.Instance.Tabla.ContainsKey(usingList))
                {
                    var elementos = TablaDeNamespace.Instance.Tabla[usingList];
                    foreach (var elemento in elementos)
                    {
                        if (elemento is ClassNode)
                            if ((elemento as ClassNode).nombre == ContenidoStack.InstanceStack.claseActual)
                            {
                                var elementosClase = (elemento as ClassNode).cuerpo;

                                foreach (var elementoClase in elementosClase)
                                {
                                    if (elementoClase is DeclaracionMetodo)
                                    {
                                        if ((elementoClase as DeclaracionMetodo).nombre == nombre &&
                                            (elementoClase as DeclaracionMetodo).encapsulamiento == "public")
                                        {
                                            if ((elementoClase as DeclaracionMetodo).parametros.Count == parametros.Count)
                                            {
                                                var abuscarParametro = (elementoClase as DeclaracionMetodo).parametros.ToArray();
                                                var metroParametro = parametros.ToArray();
                                                encontro = true;
                                                for (int z = 0; z < abuscarParametro.Length; z++)
                                                {
                                                    var tipo = metroParametro[z].ValidateSemantic().GetType();
                                                    var tipoB = abuscarParametro[z].tipo.ValidateSemantic().GetType();
                                                    if (tipoB != tipo)
                                                        throw new SemanticoException("se esperaba el tipo " + tipoB + "fila " + metroParametro[z].token.Fila
                                                            + "columna" + metroParametro[z].token.Columna);
                                                }

                                            }
                                            if (encontro == true)
                                            {
                                                if ((elementoClase as DeclaracionMetodo).tipo.ValidateSemantic() is ClaseTipo)
                                                {
                                                    foreach (var funcion in ListaDeAccesores)
                                                    {
                                                        if (funcion is PuntoAccesor)
                                                        {
                                                            (funcion as PuntoAccesor).clase = ((elementoClase as DeclaracionMetodo).tipo.ValidateSemantic() as ClaseTipo).nombreClase;
                                                             funcion.ValidateSemantic();

                                                        }
                                                    }
                                                }
                                            }
                                            
                                        }
                                    }
                                }

                            }
                    }
                }
            }
            return encontro;
        }
    }
}
