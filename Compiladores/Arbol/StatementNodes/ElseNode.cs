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
    class ElseNode:StatementNode
    {
        public List<StatementNode> cuerpo;
        public override void ValidateSemantic()
        {
            if (cuerpo != null)
            {
                ContenidoStack.InstanceStack.Stack.Push(new TablaSimbolos());
                foreach (var bloqueTrue in cuerpo)
                    if (bloqueTrue != null)
                        bloqueTrue.ValidateSemantic();
                ContenidoStack.InstanceStack.Stack.Pop();
            }
        }
        public override string GenerarCodigo()
        {
            string value = "else\n{";
            foreach (var tipo in cuerpo)
            {
                value += "\n"+tipo.GenerarCodigo();
            }
            value += "\n}";
            return value;
        }
    }
}
