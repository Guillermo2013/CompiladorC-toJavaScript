using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Arbol.StatementNodes;
using Compiladores.Arbol.Literales;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
namespace Compiladores.Arbol
{
    class NewExpressionNode:ExpressionNode
    {
        public DefinicionTipoNode tipo;
        public List<List<ExpressionNode>> arraySize = new List<List<ExpressionNode>>();
        public List<ExpressionNode> parametros;
        public ExpressionNode inicializable;
        public override TiposBases ValidateSemantic()
        {
            if (arraySize.Count != 0)
            {
                if (!(inicializable is ArrayNode) && inicializable != null)
                    throw new SemanticoException("se debe inicializar con un arraglo fila "+inicializable.token.Fila +"columna "+ inicializable.token.Columna);

                tipo.array = arraySize;
            }
            else 
            {
                var tipodedato = tipo.ValidateSemantic();
                if (tipodedato is ClaseTipo)
                {
                    validarConstructor();
                }
            }
            return tipo.ValidateSemantic();
        }
        public void validarConstructor()
        {
        
            var encontro = false;
            foreach (var usingList in ContenidoStack._StackInstance.usingNombres)
            {
                
                if (TablaDeNamespace.Instance.Tabla.ContainsKey(usingList))
                {
                    var elementos = TablaDeNamespace.Instance.Tabla[usingList];
                    foreach (var elemento in elementos)
                    {
                        if (elemento is ClassNode)
                        {
                            
                            if ((elemento as ClassNode).nombre == tipo.tipounico && (elemento as ClassNode).encasulamiento == "public"
                                && (elemento as ClassNode).modificador != "abstract")
                            {
                                var elementosClase = (elemento as ClassNode).cuerpo;


                                foreach (var elementoClase in elementosClase)
                                {
                                    if (elementoClase is ConstructorNode)
                                    {
                                        if ((elementoClase as ConstructorNode).identificador == tipo.tipounico)
                                        {

                                            if ((elementoClase as ConstructorNode).parametro.Count == parametros.Count)
                                            {
                                                var abuscarParametro = (elementoClase as ConstructorNode).parametro.ToArray();
                                                var metroParametro = parametros.ToArray();
                                                encontro = true;
                                                for (int z = 0; z < abuscarParametro.Length; z++)
                                                {
                                                    var tipop = metroParametro[z].ValidateSemantic().GetType();
                                                    var tipoB = abuscarParametro[z].tipo.ValidateSemantic().GetType();
                                                    if (tipoB != tipop)
                                                        throw new SemanticoException("se esperaba el tipo " + tipoB + "fila " + metroParametro[z].token.Fila
                                                            + "columna" + metroParametro[z].token.Columna);
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
            if (encontro == false)
                throw new SemanticoException("no se encontro la clase o es publica o tiene los mismo tipos" + tipo.tipounico + "fila" + this.token.Fila + "columna" + this.token.Columna);
         }
    }
   
}
