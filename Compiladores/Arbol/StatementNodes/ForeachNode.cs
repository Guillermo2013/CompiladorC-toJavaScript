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
       public string identificador { get; set; }
       public override void ValidateSemantic()
       {
           ContenidoStack.InstanceStack.Stack.Push(new TablaSimbolos());
           foreach (var stack in ContenidoStack.InstanceStack.Stack)
               if (stack.VariableExist(identificador))
               {
                   if (!(stack.GetVariable(identificador) is ArrayTipo))
                       throw new Semantico.SemanticoException("la lista tiene que ser un arreglo fila" + token.Fila + "columna " + token.Columna);
               }
           foreach (var sentencia in cuerpo)
           {
               sentencia.ValidateSemantic();
           }
           ContenidoStack.InstanceStack.Stack.Pop();
       }

       
    }
}
