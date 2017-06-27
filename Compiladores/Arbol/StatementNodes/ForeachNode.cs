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
    class ForeachNode:StatementNode
    {
       public DefinicionTipoNode tipo;
       public List<StatementNode> cuerpo;
       public ExpressionNode expresion;
       public string identificador;
       public override void ValidateSemantic()
       {
           ContenidoStack.InstanceStack.Stack.Push(new TablaSimbolos());
           var encontro = false;
           foreach (var stack in ContenidoStack.InstanceStack.Stack)
               if (!stack.VariableExist(identificador))
               {
                   encontro = true;
                   if (!(expresion.ValidateSemantic() is ArrayTipo))
                       throw new Semantico.SemanticoException("la lista tiene que ser un arreglo fila" + token.Fila + "columna " + token.Columna);
                    if(tipo.ValidateSemantic().GetType() != (expresion.ValidateSemantic() as ArrayTipo).tipoArray.GetType() &&
                        !(tipo.ValidateSemantic() is VarTipo))
                        throw new Semantico.SemanticoException("la variable no concuerdan con el tipo" + token.Fila + "columna " + token.Columna);
               }
           if (encontro == false)
           {
               throw new SemanticoException(archivo + "ya existe la variable" + identificador + "fila" + token.Fila +"columna" + token.Columna);
           }
           if (!(tipo.ValidateSemantic() is VarTipo))
           ContenidoStack._StackInstance.Stack.Peek().DeclareVariable(identificador, tipo.ValidateSemantic());
           else
               ContenidoStack._StackInstance.Stack.Peek().DeclareVariable(identificador, (expresion.ValidateSemantic() as ArrayTipo).tipoArray);
           foreach (var sentencia in cuerpo)
           {
               sentencia.ValidateSemantic();
           }
           ContenidoStack.InstanceStack.Stack.Pop();
       }
       public override string GenerarCodigo()
       {

           string value = tipo.GenerarCodigo() + " " + identificador;
           value += "\n for(" + identificador + " in " + expresion.GenerarCodigo()+" )";
           value += "\n{";
           foreach (var lista in cuerpo)
           {
               value += "\n" + lista.GenerarCodigo(); 
           }
           value += "\n}";
           return value;
       }
       
    }
}
