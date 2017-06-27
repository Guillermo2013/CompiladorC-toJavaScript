using Compiladores.Arbol.AccesoresNode;
using Compiladores.Arbol.BaseNode;
using Compiladores.Arbol.StatementNodes;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Arbol.Identificadores
{
    class IdentificadoresStatementNode:StatementNode
    {
        public string nombre;
        public List<AccesorNode> ListaDeAccesores = new List<AccesorNode>();
        public bool print = false;
        public List<ExpressionNode> lista = new List<ExpressionNode>();
        bool tipoLocal = false;
        bool tipoHerencia = false;
        public List<AccesorNode> ListaDeAccesores2;
        public override void ValidateSemantic()
        {
            ListaDeAccesores2 = ListaDeAccesores;
            if (nombre == "Console")
            {
                if(ListaDeAccesores.First() is PuntoAccesor)
                {
                    if((ListaDeAccesores.First() as PuntoAccesor).identificador is CallFuntionNode  )
                        if(((ListaDeAccesores.First() as PuntoAccesor).identificador as CallFuntionNode).nombre == "WriteLine" ||
                            ((ListaDeAccesores.First() as PuntoAccesor).identificador as CallFuntionNode).nombre == "Write")
                        {
                            if(((ListaDeAccesores.First() as PuntoAccesor).identificador as CallFuntionNode).parametros.Count > 0)
                            print = true;
                            else 
                                throw new SemanticoException(archivo+"solo se acepta un argumento para imprimir "+token.Fila+"columna"+token.Columna);
                            lista = ((ListaDeAccesores.First() as PuntoAccesor).identificador as CallFuntionNode).parametros;
                            foreach (var i in lista)
                                i.ValidateSemantic();
                       }
                                
                }
            }
            if (print == false)
            {
                TiposBases tipo = null;
                foreach (var stack in ContenidoStack._StackInstance.Stack)
                    if (stack.VariableExist(nombre))
                        tipo = stack.GetVariable(nombre);

                if (tipo == null)
                {
                    
                    tipo = variableLocal(tipo);
                    if (tipo != null)
                        tipoLocal = true;
                    tipo = validarHerencia(tipo);
                    if (tipo != null)
                        tipoHerencia = true;
                }
                if (tipo == null)
                    throw new SemanticoException(archivo + "no existe la variable " + nombre + "fila" + token.Fila + "columna" + token.Columna);

                if (ListaDeAccesores.Count == 0)
                    throw new SemanticoException(archivo + " variable se asigna  " + nombre + "fila" + token.Fila + "columna" + token.Columna);
                foreach (var accesores in ListaDeAccesores)
                {
                    if (accesores is PuntoAccesor)
                    {
                        (accesores as PuntoAccesor).clase = (tipo as ClaseTipo).nombreClase;
                        tipo = accesores.ValidateSemantic();

                    }
                    else
                    {
                        
                        tipo = accesores.ValidateSemantic();
                    }
                    validadFinal(ListaDeAccesores);
                }
            }
        }
        public override string GenerarCodigo()
        {
            string value = "";
            if (print == true)
            {
                value +="console.log(";
                foreach (var list in lista)
                {
                    value+=list.GenerarCodigo();
                }
                value += ");";
                return value;
            }
            else
            {
                if (tipoLocal == true)
                    value += "this.";
                else if (tipoHerencia == true)
                    value += "super.";
                value += nombre;
                foreach (var list in this.ListaDeAccesores)
                {
                 
                    value += list.GenerarCodigo();
                }
                return value+";";
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
