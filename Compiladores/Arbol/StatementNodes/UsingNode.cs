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
    class UsingNode : StatementNode
    {
        public string identificador;
        public override void ValidateSemantic()
        {
            if (ContenidoStack.InstanceStack.usingNombres.Count == 0)
                ContenidoStack.InstanceStack.DeclareUsing(identificador);
            else {
                if (ContenidoStack.InstanceStack.VariableUsing(identificador))
                        throw new SemanticoException("se se agrego es namespace"+identificador +" fila"+ token.Fila + " columna " + token.Columna);
                ContenidoStack.InstanceStack.DeclareUsing(identificador);
            }
        }
    }
}
