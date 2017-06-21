using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Arbol.StatementNodes;
using Compiladores.Semantico.Tipos;
using Compiladores.Arbol.AccesoresNode;
using Compiladores.Arbol.Identificadores;
namespace Compiladores.Arbol
{
    class CallFuntionNode:ExpressionNode
    {
        public string nombre;
        public List<ExpressionNode> parametros;
        public List<AccesorNode> ListaDeAccesores = new List<AccesorNode>();
        public string claseActual;
        public override Semantico.TiposBases ValidateSemantic()
        {
            if (claseActual == null)
                claseActual = ContenidoStack.InstanceStack.claseActual;
            var encontro = false;
           TiposBases tiporeturno = null;
           validadFuncionLocal(ref encontro, ref tiporeturno);
           validarHerencia(ref encontro, ref tiporeturno);
            if (!encontro)

                throw new SemanticoException("no se encontro la funcion " + nombre + "fila" + this.token.Fila + "columna" + this.token.Columna);

            if(!(tiporeturno is ClaseTipo) && ListaDeAccesores.Count > 0 )
                throw new SemanticoException("no se puede instaciar otra llamada a funcion " + nombre + "fila" + this.token.Fila + "columna" + this.token.Columna);
            if (tiporeturno is ClaseTipo)
            {
                foreach (var funcion in ListaDeAccesores)
                {
                    if (funcion is PuntoAccesor)
                    {
                        (funcion as PuntoAccesor).clase  =(tiporeturno as ClaseTipo).nombreClase;
                        tiporeturno = funcion.ValidateSemantic();
                        
                    }
                }
            }
            return tiporeturno;
        }
        private void validarHerencia(ref bool encontro, ref TiposBases tiporeturno)
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
                            if ((elemento as ClassNode).nombre == claseActual)
                            {
                               
                                foreach (var herencia in (elemento as ClassNode).herencia)
                                {
                                  claseActual = (herencia as IdentificadoresExpressionNode).nombre;
                                    validadFuncionLocal(ref encontro, ref tiporeturno);
                                   validarHerencia(ref encontro, ref tiporeturno);
                                }
                                
                            }
                        }
                    }
                }
            }
          
        }

        private void validadFuncionLocal(ref bool encontro, ref TiposBases tiporeturno)
        {
            foreach (var usingList in ContenidoStack._StackInstance.usingNombres)
            {
                if (TablaDeNamespace.Instance.Tabla.ContainsKey(usingList))
                {
                    var elementos = TablaDeNamespace.Instance.Tabla[usingList];
                    foreach (var elemento in elementos)
                    {
                        if (elemento is ClassNode)
                            if ((elemento as ClassNode).nombre == claseActual)
                            {
                                var elementosClase = (elemento as ClassNode).cuerpo;


                                foreach (var elementoClase in elementosClase)
                                {
                                    if (elementoClase is DeclaracionMetodo)
                                    {
                                        if ((elementoClase as DeclaracionMetodo).nombre == nombre &&
                                            (elementoClase as DeclaracionMetodo).encapsulamiento == "public")
                                        {
                                            tiporeturno = (elementoClase as DeclaracionMetodo).tipo.ValidateSemantic();
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
        }
    }  
}
