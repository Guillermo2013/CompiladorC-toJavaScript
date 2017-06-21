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
            foreach (var statement in cuerpo)
            {
                if ((statement is ReturnNode) && tipo != null)
                {
                    var tipoReturnToken = (statement as ReturnNode);
                    var tipoReturn = tipoReturnToken.expresion.ValidateSemantic();
                    var enviar = tipo.ValidateSemantic();
                    if (enviar is ClaseTipo && tipoReturn is ClaseTipo)
                    {
                        if ((tipoReturn as ClaseTipo).nombreClase != (enviar as ClaseTipo).nombreClase)
                            throw new SemanticoException("el tipo de valor no es correcto de ve ser" + tipo.ValidateSemantic() + " fila " +
                                tipoReturnToken.token.Fila + " columna " + tipoReturnToken.token.Columna);
                    }
                    else if (tipoReturn.GetType() != enviar.GetType())
                        throw new SemanticoException("el tipo de valor no es correcto de ve ser" + tipoReturn + " en ves de" + enviar + " fila " +
                            tipoReturnToken.token.Fila + " columna " + tipoReturnToken.token.Columna);
                }
                 statement.ValidateSemantic();
            }
            ContenidoStack.InstanceStack.Stack.Pop();
        
        }
    }
}
