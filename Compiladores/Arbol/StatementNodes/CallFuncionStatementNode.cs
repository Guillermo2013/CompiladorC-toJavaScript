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
        public bool herencia;
        public override void ValidateSemantic()
        {
            var encontro = false;
            encontro = validadFuncionLocal(encontro);
            herencia = encontro;    
            encontro = validarHerencia(encontro);
                validadFinal(ListaDeAccesores);
            if (!encontro)
                throw new SemanticoException(archivo+"no se encontro la funcion " + nombre + "fila" + this.token.Fila + "columna" + this.token.Columna);
        }
        public override string GenerarCodigo()
        {
            var value = "";
            if (herencia == false)
                value += "super.";
             value += nombre + "(";
            if (parametros != null)
            {
                var parametrosArray = parametros.ToArray();
                for (int i = 0; i < parametrosArray.Length; i++)
                {
                    value += parametrosArray[i].GenerarCodigo();
                    if (i < parametrosArray.Length - 1)
                        value += ",";
                } 
            }
            value += ")";
            if (ListaDeAccesores != null)
                foreach (var lista in ListaDeAccesores)
                    value += lista.GenerarCodigo();
            return value+";";
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
                            throw new SemanticoException(archivo+"la identificadores debe asignarse"
                                + ((elementos as PuntoAccesor).identificador as IdentificadoresExpressionNode).nombre 
                                + "fila" + this.token.Fila + "columna" + this.token.Columna);
                        }
                        validadFinal(((elementos as PuntoAccesor).identificador as IdentificadoresExpressionNode).ListaDeAccesores);
                    }
                    else
                    {
                        validadFinal(((elementos as PuntoAccesor).identificador as CallFuntionNode).ListaDeAccesores);
                    }
                }else if( ListaDeAccesoresInterna.Last() is ArrayAccesor)
                    throw new SemanticoException(archivo+"la identificadores debe asignarse"
                                + ((elementos as PuntoAccesor).identificador as IdentificadoresExpressionNode).nombre
                                + "fila" + this.token.Fila + "columna" + this.token.Columna);
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
                                                        throw new SemanticoException(archivo+"se esperaba el tipo " + tipoB + "fila " + metroParametro[z].token.Fila
                                                            + "columna" + metroParametro[z].token.Columna);
                                                }

                                            }
                                            if (encontro == true)
                                            {
                                                foreach (var lista in ListaDeAccesores)
                                                {
                                                    if ((lista is ArrayAccesor) && !((elementoClase as DeclaracionMetodo).tipo.ValidateSemantic() is ArrayTipo))
                                                    {
                                                        throw new SemanticoException(archivo+"la funcion no revuelve un arreglo " + nombre + "fila" + this.token.Fila + "columna" + this.token.Columna);
                                                    }
                                                }
                                                var tiporeturno = (elementoClase as DeclaracionMetodo).tipo.ValidateSemantic();
                                                if (tiporeturno is ArrayTipo && ListaDeAccesores.Count > 0)
                                                {
                                                    var listaDeArray = new List<TiposBases>();
                                                    int remove = 0;
                                                    foreach (var acessor in ListaDeAccesores)
                                                    {
                                                        if (acessor is ArrayAccesor)
                                                        {
                                                            listaDeArray.Add(acessor.ValidateSemantic());

                                                        }

                                                        remove++;
                                                    }

                                                    ListaDeAccesores.RemoveRange(0, remove - 1);

                                                    if ((tiporeturno as ArrayTipo).cantidad.Count == listaDeArray.Count)
                                                    {
                                                        var cantidadArray = (tiporeturno as ArrayTipo).cantidad.ToArray().ToArray();
                                                        var listaArray = listaDeArray.ToArray();
                                                        for (int i = 0; i < cantidadArray.Length; i++)
                                                        {
                                                            if (cantidadArray[i].Count != (listaDeArray[i] as ArrayTipo).cantidad.First().Count)
                                                                throw new SemanticoException(archivo+"se tiene que asignar el mismo tipo al arreglo " + nombre + "fila"
                                                                         + token.Fila + "columna" + token.Columna);
                                                        }
                                                        tiporeturno = (tiporeturno as ArrayTipo).tipoArray;
                                                    }
                                                    else if ((tiporeturno as ArrayTipo).cantidad.Count >= listaDeArray.Count)
                                                    {
                                                        var cantidadArray = (tiporeturno as ArrayTipo).cantidad.ToArray().ToArray();
                                                        var listaArray = listaDeArray.ToArray();
                                                        for (int i = 0; i < listaDeArray.Count; i++)
                                                        {
                                                            if (cantidadArray[i].Count != (listaDeArray[i] as ArrayTipo).cantidad.First().Count)
                                                                throw new SemanticoException(archivo+"se tiene que asignar el mismo tipo al arreglo " + nombre + "fila"
                                                                    + token.Fila + "columna" + token.Columna);
                                                            tiporeturno = (tiporeturno as ArrayTipo).tipoArray;
                                                        }

                                                    }
                                                    else if ((tiporeturno as ArrayTipo).cantidad.Count < listaDeArray.Count)
                                                        throw new SemanticoException(archivo+"el arreglo es de menos cantidad" + token.Fila + "columma" + token.Columna);

                                                }
                                                if (tiporeturno is ClaseTipo)
                                                {
                                                    foreach (var funcion in ListaDeAccesores)
                                                    {
                                                        if (funcion is PuntoAccesor)
                                                        {
                                                            (funcion as PuntoAccesor).clase = (tiporeturno as ClaseTipo).nombreClase;
                                                             funcion.ValidateSemantic();

                                                        }
                                                    }
                                                }
                                                if (!(tiporeturno is ClaseTipo) && ListaDeAccesores.Count > 0)
                                                    throw new SemanticoException(archivo+"returna " + tiporeturno + "no se puede instaciar una funcion si no retorna una clase " + nombre + "fila" + this.token.Fila + "columna" + this.token.Columna);
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
