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
using Compiladores.Arbol.Literales;
namespace Compiladores.Arbol
{
    class CallFuntionNode:ExpressionNode
    {
        public string nombre;
        public List<ExpressionNode> parametros;
        public List<AccesorNode> ListaDeAccesores = new List<AccesorNode>();

        public string claseActual;
        public bool herencia;
        
        public override Semantico.TiposBases ValidateSemantic()
        {

         
            if (claseActual == null)
                claseActual = ContenidoStack.InstanceStack.claseActual;
            var encontro = false;
           TiposBases tiporeturno = null;
           validadFuncionLocal(ref encontro, ref tiporeturno);
           herencia = encontro;
           validarHerencia(ref encontro, ref tiporeturno);
            if (!encontro)
                throw new SemanticoException(archivo+"no se encontro la funcion " + nombre + "fila" + this.token.Fila + "columna" + this.token.Columna);
            foreach (var lista in ListaDeAccesores)
            {
                if ((lista is ArrayAccesor) && !(tiporeturno is ArrayTipo))
                {
                    throw new SemanticoException(archivo+"la funcion no revuelve un arreglo " + nombre + "fila" + this.token.Fila + "columna" + this.token.Columna);
                }
            }
            int remove = 0;
            if (tiporeturno is ArrayTipo && ListaDeAccesores.Count > 0)
            {
                var listaDeArray = new List<TiposBases>();
                
                foreach (var acessor in ListaDeAccesores)
                { 
                    if (acessor is ArrayAccesor)
                    {
                        listaDeArray.Add(acessor.ValidateSemantic());
                        
                    }
                    
                    remove++;
                }

             
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
                            (tiporeturno as ArrayTipo).cantidad.RemoveAt(i);
                        }

                    }
                    else if ((tiporeturno as ArrayTipo).cantidad.Count < listaDeArray.Count)
                        throw new SemanticoException(archivo+"el arreglo es de menos cantidad" + token.Fila + "columma" + token.Columna);

            }
            if(!(tiporeturno is ClaseTipo) && ListaDeAccesores.Count - remove> 0 )
                throw new SemanticoException(archivo+"returna " + tiporeturno + "no se puede instaciar una funcion si no retorna una clase " + nombre + "fila" + this.token.Fila + "columna" + this.token.Columna);
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
        public override string GenerarCodigo()
        {
            var valor = "";
            if (herencia == false)
                valor += "super.";
            valor += nombre + "(";
            if (parametros != null)
            {
                var parametroArray = parametros.ToArray();
                for (int i = 0; i<parametroArray.Length; i++)
                {
                    valor += parametroArray[i].GenerarCodigo();
                    if(i<parametroArray.Length)
                        valor+=",";
                }
            }
            
            valor += ")";
            foreach (var lista in ListaDeAccesores)
            {
                
                valor += lista.GenerarCodigo();
            }
            return valor;
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
                                                        throw new SemanticoException(archivo+"se esperaba el tipo " + tipoB + "fila " + metroParametro[z].token.Fila
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
