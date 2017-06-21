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
        public override void ValidateSemantic()
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
            if (tipo == null )
                throw new SemanticoException("no existe la variable " + nombre + "fila" + token.Fila + "columna" + token.Columna);
          
            if(ListaDeAccesores.Count == 0)
                throw new SemanticoException(" variable se asigna  " + nombre + "fila" + token.Fila + "columna" + token.Columna);
            foreach(var accesores in ListaDeAccesores){
                if (accesores is PuntoAccesor)
                {
                    (accesores as PuntoAccesor).clase = (tipo as ClaseTipo).nombreClase;
                     tipo = accesores.ValidateSemantic();
                   
                }
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
