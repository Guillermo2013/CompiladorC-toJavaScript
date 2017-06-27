using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Arbol.StatementNodes;
using Compiladores.Arbol.Identificadores;
namespace Compiladores.Arbol
{
    class ThisNode : ExpressionNode
    {
        public ExpressionNode expresion;
        public string claseActual;
        public override TiposBases ValidateSemantic()
        {
            if (claseActual == null)
                claseActual = ContenidoStack.InstanceStack.claseActual;
            TiposBases tiporeturno = null;
            tiporeturno = variableLocal(tiporeturno);
            tiporeturno = validarHerencia(tiporeturno);
            if (tiporeturno == null)
                throw new SemanticoException(archivo+"no existe la variable fila" + token.Fila + "columna" + token.Columna);
            return tiporeturno;
        }

        public override string GenerarCodigo()
        {
            return "this."+expresion.GenerarCodigo();
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

        private TiposBases variableLocal(TiposBases tiporeturno)
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
                                    if (elementoClase is DeclaracionVariableNode && (expresion is IdentificadoresExpressionNode))
                                    {
                                        if ((elementoClase as DeclaracionVariableNode).identificador == (expresion as IdentificadoresExpressionNode).nombre)
                                            tiporeturno = (elementoClase as DeclaracionVariableNode).tipo.ValidateSemantic();

                                    }
                                    else if (elementoClase is DeclaracionMetodo && (expresion is CallFuntionNode))
                                    {
                                        if ((elementoClase as DeclaracionMetodo).nombre == (expresion as CallFuntionNode).nombre)
                                        {
                                            tiporeturno = (elementoClase as DeclaracionMetodo).tipo.ValidateSemantic();
                                            if ((elementoClase as DeclaracionMetodo).parametros.Count == (expresion as CallFuntionNode).parametros.Count)
                                            {
                                                var abuscarParametro = (elementoClase as DeclaracionMetodo).parametros.ToArray();
                                                var metroParametro = (expresion as CallFuntionNode).parametros.ToArray();
                                                for (int z = 0; z < abuscarParametro.Length; z++)
                                                {
                                                    var tipo = metroParametro[z].ValidateSemantic().GetType();
                                                    var tipoB = abuscarParametro[z].tipo.ValidateSemantic().GetType();
                                                    if (tipoB != tipo)
                                                        throw new SemanticoException(archivo+"se esperaba el tipo " + tipoB + "fila " + metroParametro[z].token.Fila
                                                            + "columna" + metroParametro[z].token.Columna);
                                                }

                                            }
                                            tiporeturno = (expresion as CallFuntionNode).ValidateSemantic();
                                        }
                                    }
                                }
                            }

                    }
                }
                
            }
            return tiporeturno;
        }
    }   
}