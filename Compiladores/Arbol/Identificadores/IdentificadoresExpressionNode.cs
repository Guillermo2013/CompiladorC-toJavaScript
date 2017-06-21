﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
using Compiladores.Arbol.Literales;
using Compiladores.Arbol.AccesoresNode;
using Compiladores.Arbol.StatementNodes;
namespace Compiladores.Arbol.Identificadores
{
    class IdentificadoresExpressionNode:ExpressionNode
    {
        public string nombre;
        public List<AccesorNode> ListaDeAccesores = new List<AccesorNode>();
        public override TiposBases ValidateSemantic()
        {
            TiposBases tipo = null;
            foreach (var stack in ContenidoStack._StackInstance.Stack)
                if (stack.VariableExist(nombre))
                     tipo = stack.GetVariable(nombre);

            if (tipo == null)
            {
                tipo = variableLocal(tipo);
                tipo = validarHerencia(tipo);
            }
            if(tipo == null){
                tipo = enumVariable();
            }
            if (tipo is EnumTipo)
                return tipo;
            if(tipo == null)
                throw new SemanticoException("no existe la variable " + nombre + "fila" + token.Fila + "columna" + token.Columna);
            if (ListaDeAccesores.Count == 0)
            {
               return tipo;
            }
            else  if(!(tipo is ClaseTipo) && ListaDeAccesores.Count > 0)
                throw new SemanticoException("no la variable se asigna  " + nombre + "fila" + token.Fila + "columna" + token.Columna);
            foreach (var accesores in ListaDeAccesores)
            {
                if (accesores is PuntoAccesor)
                {
                    (accesores as PuntoAccesor).clase = (tipo as ClaseTipo).nombreClase;
                    tipo = accesores.ValidateSemantic();

                }
            } 
            return tipo;
        }

        private TiposBases enumVariable()
        {
            foreach (var usingList in ContenidoStack._StackInstance.usingNombres)
            {
                if (TablaDeNamespace.Instance.Tabla.ContainsKey(usingList))
                {
                    var elementos = TablaDeNamespace.Instance.Tabla[usingList];
                    foreach (var elemento in elementos)
                    {
                        if (elemento is EnumNode)
                        {
                            if ((elemento as EnumNode).identificador == nombre)
                            {
                                if (ListaDeAccesores.Count == 0)
                                {
                                    throw new SemanticoException("el enum " + nombre + "no se puede usar como variable fila" + token.Fila + "columna" + token.Columna);
                                }
                                else if (ListaDeAccesores.Count == 1)
                                {
                                    if (ListaDeAccesores[0] is PuntoAccesor)
                                    {
                                        if ((ListaDeAccesores[0] as PuntoAccesor).identificador is IdentificadoresExpressionNode)
                                        {
                                        var encontrado = false;
                                        foreach (var arreglo in (elemento as EnumNode).lista)
                                        {
                                            if (arreglo.identificador == ((ListaDeAccesores[0] as PuntoAccesor).identificador as IdentificadoresExpressionNode).nombre)
                                                return new EnumTipo() { nombreClase = nombre};
                                        } 
                                        if(encontrado == false)
                                            throw new SemanticoException("el enum " + ((ListaDeAccesores[0] as PuntoAccesor).identificador as IdentificadoresExpressionNode).nombre
                                                + "no se puede usar como arreglo fila"    + token.Fila + "columna" + token.Columna);
                                        }
                                        else
                                        {
                                            throw new SemanticoException("el enum " + nombre + "no se puede llamar funciones fila" + token.Fila + "columna" + token.Columna);
                                        }
                                    }
                                    else
                                    {
                                        throw new SemanticoException("el enum " + nombre + "no se puede usar como arreglo fila" + token.Fila + "columna" + token.Columna);
                                    }

                                }

                            }
                        }
                    }
                }
            }
            return null;
        }
        private TiposBases validarHerencia(TiposBases tipo)
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
                                    tipo = variableLocal(tipo);
                                    tipo = validarHerencia(tipo);
                                }
                                ContenidoStack.InstanceStack.claseActual = claseActuar;
                            }
                        }
                    }
                }
            }
            return tipo;
        }

        private TiposBases variableLocal(TiposBases tipo)
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
                                    if (elementoClase is DeclaracionVariableNode)
                                    {
                                        if ((elementoClase as DeclaracionVariableNode).identificador == nombre)
                                            tipo = (elementoClase as DeclaracionVariableNode).tipo.ValidateSemantic();
                                    }
                                }
                            }
                    }
                }
            }
            return tipo;
        }
    }
}
