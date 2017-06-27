using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
using Compiladores.Arbol.StatementNodes;
using Compiladores.Arbol.Identificadores;
namespace Compiladores.Arbol
{
    class CastNode:ExpressionNode
    {
        public string tipo;
        public ExpressionNode expresion;
        public override TiposBases ValidateSemantic()
        {

            var tipoExpresion = expresion.ValidateSemantic();
            if (tipoExpresion is IntTipo && tipo == "float")
                return new FloatTipo();
            else if (tipoExpresion is FloatTipo && tipo == "int")
                return new IntTipo();
            else if (tipoExpresion is CharTipo && tipo == "int")
                return new IntTipo();
            else if (tipoExpresion is IntTipo && tipo == "char")
                return new CharTipo();
            else if ( tipo == "string")
                return new StringTipo();
           
            else if (tipoExpresion is ClaseTipo)
            {
                if ((tipoExpresion as ClaseTipo).nombreClase != tipo)
                    tipoExpresion = validarCasteoHerencia((tipoExpresion as ClaseTipo).nombreClase);
                else if ((tipoExpresion as ClaseTipo).nombreClase == tipo)
                    return tipoExpresion;
                else
                    return null;
                if (tipoExpresion == null)
                    throw new SemanticoException(archivo+"no se puede castear" + tipo + "cont" + tipoExpresion + " fila" + expresion.token.Fila + "columna" + expresion.token.Columna);
                else
                    return tipoExpresion;
            }
            else
                throw new SemanticoException(archivo+"no se puede castear" + tipo + "cont" + tipoExpresion + " fila" + expresion.token.Fila + "columna" + expresion.token.Columna);
        }
        public override string GenerarCodigo()
        {
            var tipoExpresion = expresion.ValidateSemantic();
            string valor="";
            if (tipoExpresion is IntTipo && tipo == "float")
                valor = expresion.GenerarCodigo() + ".toFixed(2)";
            else if (tipoExpresion is FloatTipo && tipo == "int")
                valor = "~~" + expresion.GenerarCodigo();
            else if (tipoExpresion is CharTipo && tipo == "int")
                valor = expresion.GenerarCodigo() + ".charCodeAt(0)";
            else if (tipoExpresion is IntTipo && tipo == "char")
                valor = "String.fromCharCode(97 + " + expresion.GenerarCodigo() + ")";
            else if ( tipo == "string")
                valor = "("+expresion.GenerarCodigo() + ").toString()";
           
            else if (tipoExpresion is ClaseTipo)
                valor = expresion.GenerarCodigo() + "=" + "( Object.create(" + tipo + "))";
            return valor;
        }
        private TiposBases validarCasteoHerencia(string p)
        {
            foreach (var usingList in ContenidoStack._StackInstance.usingNombres)
            {
                if (TablaDeNamespace.Instance.Tabla.ContainsKey(usingList))
                {
                    var elementos = TablaDeNamespace.Instance.Tabla[usingList];
                    foreach (var elemento in elementos)
                    {
                        if (elemento is ClassNode)
                            if ((elemento as ClassNode).nombre == p)
                            {
                                var elementosClase = (elemento as ClassNode).herencia;


                                foreach (var herenciaElemento in elementosClase)
                                {
                                    string nombrePadre = (herenciaElemento as IdentificadoresExpressionNode).nombre;
                                    if (nombrePadre == tipo)
                                        return new ClaseTipo() { nombreClase = tipo };
                                }


                            }
                    }
                }
            }
            return null;
        }
       
    }
}
