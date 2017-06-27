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
        public List<ExpressionNode> parametros;
        public ExpressionNode inicializable;
    
 
        public override TiposBases ValidateSemantic()
        {
            var datotipo = tipo.ValidateSemantic();
            if (datotipo is ClaseTipo)
            {
                validarConstructor();
            }
            else if (datotipo is ArrayTipo)
            {
                var tamañoArray = (datotipo as ArrayTipo).cantidad.ToArray().ToArray();
                if (tamañoArray.Length > 1)
                {
                    for (int i = 0; i < tamañoArray[0].Count; i++)
                        if (tamañoArray[0][i] == 0)
                            throw new SemanticoException(archivo+"se tiene que definir al menos una de las dimensiones de los multiarreglos" +
                     tipo.token.Fila + "columna" + tipo.token.Columna);
                    for (int i = 1; i < tamañoArray.Length; i++)
                        for (int j = 1; j < tamañoArray[i].Count; j++)
                            if (tamañoArray[i][j] != 0)
                                throw new SemanticoException(archivo+"rango invalido se espera una , o ] " +
                         tipo.token.Fila + "columna" + tipo.token.Columna);
                }
                else
                {
                    Boolean esperarTipo = false;
                    if(tamañoArray[0][0] > 0)
                        esperarTipo = true;
                    for (int i = 0; i < tamañoArray[0].Count; i++)
                        if (tamañoArray[0][i] == 0 && esperarTipo )
                            throw new SemanticoException(archivo+"se espera una expression" +tipo.token.Fila + "columna" + tipo.token.Columna);
                        else if (tamañoArray[0][i] != 0 && esperarTipo == false)
                             throw new SemanticoException(archivo+"rango invalido se espera una , o ] " + tipo.token.Fila + "columna" + tipo.token.Columna);
                        if(esperarTipo == false && inicializable==null )
                            throw new SemanticoException(archivo+"se debe de asignar el arreglo " + tipo.token.Fila + "columna" + tipo.token.Columna);
                }
                if (inicializable != null)
                {
                     datotipo = inicializable.ValidateSemantic();
                    if(datotipo == null)
                        throw new SemanticoException(archivo+"se debe de asignar correctamente el arreglo " + tipo.token.Fila + "columna" + tipo.token.Columna);
                }
                   
              
            }
            return datotipo;
        }
        public override string GenerarCodigo()
        {
            string valor = "";
            valor += tipo.GenerarCodigo();
            if (parametros != null && parametros.Count>0)
            {
                valor += "(";
                var parametrosArray = parametros.ToArray();
                for (int i = 0;i<parametrosArray.Length ;i++)
                {
                    valor += parametrosArray[i].GenerarCodigo();
                    if (i < parametrosArray.Length - 1)
                        valor += ",";
                }
                    
                valor += ")";
            }
            if (inicializable != null)
            {
                valor = inicializable.GenerarCodigo();
            }
            return valor;
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
                                && (elemento as ClassNode).modificador != "abstract" && (elemento as ClassNode).modificador != "static")
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
                                                        throw new SemanticoException(archivo+"se esperaba el tipo " + tipoB + "fila " + metroParametro[z].token.Fila
                                                            + "columna" + metroParametro[z].token.Columna);
                                                }

                                            }
                                            else if ((elementoClase as ConstructorNode).parametro.Count == 0)
                                            {
                                                encontro = true;
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
                throw new SemanticoException(archivo+"no se encontro la clase o es publica o tiene los mismo tipos" + tipo.tipounico + "fila" + this.token.Fila + "columna" + this.token.Columna);
         }
    }
   
}
