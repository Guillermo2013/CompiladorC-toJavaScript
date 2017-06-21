using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico.Tipos;
using Compiladores.Semantico;
using Compiladores.Arbol.Literales;
using Compiladores.Arbol.BinaryNodes;

namespace Compiladores.Arbol.StatementNodes
{
    class DefinicionTipoNode:ExpressionNode
    {
        public string tipounico;
        public List<List<ExpressionNode>> array;
       
        public override TiposBases ValidateSemantic()
        {
            TiposBases tipo = new TiposBases();
            if (tipounico == "int")
                tipo = new IntTipo();
            else if (tipounico == "bool")
                tipo = new IntTipo();
            else if (tipounico == "string")
                tipo = new StringTipo();
            else if (tipounico == "float")
                tipo = new FloatTipo();
            else if (tipounico == "byte")
                tipo = new BinarioTipo();
            else if (tipounico == "char")
                tipo = new CharTipo();
            else if (tipounico == "char")
                tipo = new CharTipo();
            else if (tipounico == "var")
                tipo = new VarTipo();
            else
            {
                tipo = validarTipoIdentificador(); 

            }
            if(tipo == null)
                throw new SemanticoException("no existe este tipo de dato " + tipounico + "fila" + token.Fila
                                        + "columna" + token.Columna);
            if (array != null)
            {
                if (array.Count != 0)
                {
                    foreach (var listadelista in array)
                        foreach (var tamaño in listadelista)
                        {
                            if (!(tamaño is SeparadorNode) && tamaño != null)
                            {
                                var tamañoNumero = tamaño.ValidateSemantic();
                                if (!(tamaño is LiteralNumerico))
                                    throw new SemanticoException("la definicion tiene que ser int " + tamañoNumero + "fila" + tamaño.token.Fila
                                        + "columna" + tamaño.token.Columna);
                            }
                        }
                    return new ArrayTipo() { tipoArray = tipo, cantidad = array[0].Count - 1 };
                }
                else
                {
                    return tipo;
                }
            }
            else
                return tipo;
        }

        private TiposBases validarTipoIdentificador()
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
                            if ((elemento as EnumNode).identificador == tipounico)
                                return new EnumTipo() { nombreClase = tipounico };
                        }
                        else if (elemento is ClassNode)
                        {
                            if ((elemento as ClassNode).nombre == tipounico)
                                return new ClaseTipo() { nombreClase = tipounico };
                        }
                    }
                }
            }
            return null;
        }
        
    }

}
