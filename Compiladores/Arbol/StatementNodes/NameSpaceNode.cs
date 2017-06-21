using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Arbol.StatementNodes;
using Compiladores.Semantico.Tipos;
namespace Compiladores.Arbol.StatementNodes
{
    class NameSpaceNode : StatementNode
    {
        public string nombre;
        public List<StatementNode> statemenList;
        public override void ValidateSemantic()
        {
            if (ContenidoStack.InstanceStack.usingNombres.Count == 0)
                ContenidoStack.InstanceStack.DeclareUsing(nombre);
            else{
                if (!ContenidoStack.InstanceStack.VariableUsing(nombre))
                    ContenidoStack.InstanceStack.DeclareUsing(nombre);
            }

            foreach (var sentencias in statemenList)
            {
                ContenidoStack.InstanceStack.Stack.Push(new TablaSimbolos());
                sentencias.ValidateSemantic();
                ContenidoStack.InstanceStack.Stack.Pop();
            }
                
        }
    }
}
