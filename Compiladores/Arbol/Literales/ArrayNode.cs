using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Arbol.StatementNodes;
using Compiladores.Semantico.Tipos;
using Compiladores.Semantico;

namespace Compiladores.Arbol.Literales
{
    class ArrayNode : ExpressionNode
    {
        public List<ExpressionNode> expresion;
        public DefinicionTipoNode tipo;


        public override TiposBases ValidateSemantic()
        {
            if (expresion.Count == 0)
                return null;

            var expresionArray = expresion.ToArray();
            var cantidad2 = tipo.array.ToArray();
            if (!(expresionArray[0] is ArrayNode))
            {
                if (expresionArray[0] == null)
                {
                    throw new ParserException(archivo+"se debe inicializar con un objeto " + expresionArray[0].token.Fila + "columna" + expresionArray[0].token.Columna);
                }
                int i = 0;
                var es = expresionArray[0].ValidateSemantic();
                foreach (var valor in expresionArray)
                {
                    if (valor.ValidateSemantic() == es)
                        throw new ParserException(archivo+"el tipo de arreglo no valido fila " + valor.token.Fila + "columna" + valor.token.Columna);
                    i++;
                }

                var tamaño = new List<List<int>>();
                var tamaño1 = new List<int>();
                tamaño1.Add(i);
                tamaño.Add(tamaño1);
                if (es is ArrayTipo)
                    (es as ArrayTipo).cantidad.Insert(0, (tipo.ValidateSemantic() as ArrayTipo).cantidad.First());
                else
                    return new ArrayTipo() { tipoArray = es, cantidad = tamaño };
                return es;
               
            }
            else
            {

                var validarCantidad = cantidad2[0];
                int i = 0;
              var returnValor  = validar(expresionArray, cantidad2, validarCantidad, i);
              if (returnValor is ArrayTipo)
                  (returnValor as ArrayTipo).cantidad.Insert(0, (tipo.ValidateSemantic() as ArrayTipo).cantidad.First());
              return returnValor;

            }


        }
        public override string GenerarCodigo()
        {
            string valor = "[";
            valor += obtenerString(expresion);  
            valor += "]";
            return valor;
        }

        private string obtenerString(List<ExpressionNode> expresion)
        {
            string valor = "";
            var expresionArray = expresion.ToArray();
            for (int i = 0; i < expresion.Count; i++)
            {
                if (expresionArray[i] is ArrayNode)
                {
                   valor = "[";
                   valor += obtenerString((expresionArray[i] as ArrayNode).expresion);
                   valor += "]";
                }
                else if (!(expresionArray[i] is ArrayNode))
                {
                    valor += expresionArray[i].GenerarCodigo();
                }
                if (i != expresionArray.Length - 1)
                {
                    valor += ",";
                } 

            }
            return valor;
        }

        private TiposBases validar(ExpressionNode[] expresionArray, List<ExpressionNode>[] cantidad2, List<ExpressionNode> validarCantidad, int i)
        {
            List<TiposBases> lista = new List<TiposBases>();
            if (validarCantidad.Count > i)
            {
                if (expresionArray.Length == (validarCantidad[i] as LiteralNumerico).valor)
                {
                    foreach (var expresions in expresionArray)
                    {
                        if (expresions is ArrayNode)
                        {
                            var expresionSub = expresionArray.ToArray();
                            i++;
                           
                            lista.Add(validar((expresions as ArrayNode).expresion.ToArray(), cantidad2, validarCantidad, i));
                            
                        }

                    }

                }

            }
            if (expresionArray[0] == null)
            {
                throw new ParserException(archivo+"el tipo de arreglo no valido fila " + token.Fila + "columna" + token.Columna);
            }
            else
            {

                if (!(expresionArray[0] is ArrayNode))
                {
                    if (expresionArray[0] == null)
                    {
                        throw new ParserException(archivo+"se debe inicializar con un objeto " + expresionArray[0].token.Fila + "columna" + expresionArray[0].token.Columna);
                    }
                    
                    var es = expresionArray[0].ValidateSemantic();
                    foreach (var valor in expresionArray)
                    {
                        if (valor.ValidateSemantic() == es)
                            throw new ParserException(archivo+"el tipo de arreglo no valido fila " + valor.token.Fila + "columna" + valor.token.Columna);
                       
                    }


                    return es;
                }

                var primero = lista[0];
                foreach (var valor in lista)
                {

                    if ((primero is ArrayTipo) && (valor is ArrayTipo))
                    {
                        var declarArray = (primero as ArrayTipo).cantidad.ToArray();
                        var tipoDefinionArray = (valor as ArrayTipo).cantidad.ToArray();
                        if (tipoDefinionArray.Length != declarArray.Length)
                            throw new SemanticoException(archivo+"se tiene que asignar el mismo tipo al arreglo fila"                                + token.Fila + "columna" + token.Columna);
                        for (int j = 0; j < declarArray.Length; j++)
                        {
                            if (tipoDefinionArray[j].Count != declarArray[j].Count)
                                throw new SemanticoException(archivo+"se tiene que asignar el mismo tipo al arreglo fila"
                                    + token.Fila + "columna" + token.Columna);
                        }
                      
                    }else if (valor != primero)
                        throw new ParserException(archivo+"el tipo de arreglo no valido fila " + token.Fila + "columna" + token.Columna);
                    
                }
              
                return primero;
                
            }

        }
    }
}