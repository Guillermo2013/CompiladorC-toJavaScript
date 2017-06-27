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
        public List<List<ExpressionNode>> array = new List<List<ExpressionNode>>();
       
        public override TiposBases ValidateSemantic()
        {
            TiposBases tipo = new TiposBases();
            if (tipounico == "int")
                tipo = new IntTipo();
            else if (tipounico == "bool")
                tipo = new BooleanTipo();
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
            else if (tipounico == "void")
                tipo = new VoidTipo();
            else
            {
                tipo = validarTipoIdentificador(); 

            }
           
           
                if (array.Count != 0)
                {
                    var lista = new List<List<int>>();
                    foreach (var listadelista in array)
                    {
                        var lista2 = new List<int>();

                        if (listadelista.Count > 0)
                        {
                            if (listadelista[0] is SeparadorNode)
                                lista2.Add(0);
                        }else if(listadelista.Count == 0)
                            lista2.Add(0);
                        foreach (var tamaño in listadelista)
                        {
                            if (!(tamaño is SeparadorNode) && tamaño != null)
                            {
                                var tamañoNumero = tamaño.ValidateSemantic();
                                if (!(tamaño is LiteralNumerico))
                                    throw new SemanticoException(archivo+"la definicion tiene que ser int " + tamañoNumero + "fila" + tamaño.token.Fila
                                        + "columna" + tamaño.token.Columna);
                                lista2.Add(int.Parse(tamaño.token.Lexema));
                            }
                            else
                                lista2.Add(0);
                            
                        }
                       
                        lista.Add(lista2);
                    }
                    
                    return new ArrayTipo() { tipoArray = tipo, cantidad = lista };
                }
                else
                {
                    return tipo;
                }
            
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
        public override string GenerarCodigo()
        {
            string valor = "";
            if (tipounico == "int")
                valor = "0";
            else if (tipounico == "bool")
                valor = "true";
            else if (tipounico == "string")
                valor = '\"'+" "+'\"';
            else if (tipounico == "float")
                valor = "0.0";
            else if (tipounico == "byte")
                valor = "01";
            else if (tipounico == "char")
                valor = "' '";
            else if (tipounico == "var")
                valor = "var";
            else if (tipounico == "void")
                valor = " ";
            else if (tipounico != null)
                valor = "new " + tipounico + "()";
            if (array != null && array.Count >0)
            {
                string valor2 = "";
                var i = 0;
                if(array.Count == 1)
                    if(array.First().Count == 0)
                        return "["+valor+"]";
                foreach (var lista in array)
                {
                    foreach (var lista2 in lista)
                    {
                        i++;
                    }
                    i++;
                }
                i--;
                for (int j = i * 2; j > 0; j--)
                {
                    if (j >= i)
                        valor2 += "[";
                    if (j == i)
                        valor2 += valor;
                    if (j <= i)
                        valor2 += "]"; 
                        
                }
                return valor2;
            }
            return valor;
        }
        
    }

}
