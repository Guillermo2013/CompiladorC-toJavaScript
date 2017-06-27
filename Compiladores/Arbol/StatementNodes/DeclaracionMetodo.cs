using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
namespace Compiladores.Arbol.StatementNodes
{
    class DeclaracionMetodo:StatementNode
    {
        public string modificar;
        public string encapsulamiento;
        public DefinicionTipoNode tipo = new DefinicionTipoNode() ;
        public string nombre;
        public List<ParametrosNode> parametros;
        public List<StatementNode> cuerpo;
        public override void ValidateSemantic()
        {
          
            var lista = new Dictionary<string, TiposBases>();
            ContenidoStack.InstanceStack.Stack.Push(new TablaSimbolos());
            foreach (var statement in parametros)
            {
              var tipo =  statement.ValidateSemantic();
                ContenidoStack._StackInstance.Stack.Peek().DeclareVariable((statement as ParametrosNode).nombre,tipo);
                lista.Add((statement as ParametrosNode).nombre, tipo);
                
            }

            if (modificar != "abstract")
            {
                var encontraReturn = false;
                foreach (var statement in cuerpo)
                {
                    if ((statement is ReturnNode))
                    {
                        var tipoReturnToken = (statement as ReturnNode);
                        var tipoReturn = tipoReturnToken.expresion.ValidateSemantic();
                        var enviar = tipo.ValidateSemantic();
                        if (enviar is ClaseTipo && tipoReturn is ClaseTipo)
                        {
                            if ((tipoReturn as ClaseTipo).nombreClase != (enviar as ClaseTipo).nombreClase)
                                throw new SemanticoException(archivo+"el tipo de valor returno no es correcto de ve ser" + tipo.ValidateSemantic() + " fila " +
                                    tipoReturnToken.token.Fila + " columna " + tipoReturnToken.token.Columna);
                        }
                        else if ((enviar is ArrayTipo) && (tipoReturn is ArrayTipo))
                        {
                            var declarArray = (enviar as ArrayTipo).cantidad.ToArray();
                            var tipoDefinionArray = (tipoReturn as ArrayTipo).cantidad.ToArray();
                            if (tipoDefinionArray.Length != declarArray.Length)
                                throw new SemanticoException(archivo+"se tiene que asignar el mismo tipo al arreglo " + nombre + "fila"
                                    + tipoReturnToken.token.Fila + "columna" + tipoReturnToken.token.Columna);
                            for (int i = 0; i < declarArray.Length; i++)
                            {
                                if (tipoDefinionArray[i].Count != declarArray[i].Count)
                                    throw new SemanticoException(archivo+"no se returna el mismo tamaño de arreglo arreglo " + nombre + "fila"
                                        + tipoReturnToken.token.Fila + "columna" + tipoReturnToken.token.Columna);
                            }
                         
                        }
                        else if (tipoReturn.GetType() != enviar.GetType())
                            throw new SemanticoException(archivo+"el tipo de valor returno no es correcto de ve ser" + tipoReturn + " en ves de" + enviar + " fila " +
                                tipoReturnToken.token.Fila + " columna " + tipoReturnToken.token.Columna);
                        encontraReturn = true;
                    }
                    statement.ValidateSemantic();
                }
                if(!(tipo.ValidateSemantic() is VoidTipo)&& encontraReturn == false)
                    throw new SemanticoException(archivo+"se tiene que hacer retorno a una funcion" + nombre + token.Fila + " columna " + token.Columna);

            }

            ContenidoStack.InstanceStack.Stack.Pop();
        
        }
        public override string GenerarCodigo()
        {
            var valor = nombre + "(";
           if(parametros !=  null){
               var parametosArray = parametros.ToArray();
               for (int i = 0; i < parametosArray.Length; i++)
               {
                  
                   valor += parametosArray[i].GenerarCodigo();
                   if (i < parametosArray.Length - 1)
                       valor += ",";
               }
           }
           if (cuerpo != null)
           {
               valor += ")\n{";

               foreach (var lista in cuerpo)
               {

                   valor += "\n" + lista.GenerarCodigo();



               }

               valor += "\n}";
           }
           else
           {
               valor += ";";
           }
           return valor;
        }
    }
    
}
