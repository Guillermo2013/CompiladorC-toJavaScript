using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;

namespace Compiladores.Arbol.StatementNodes
{
    class InterzaceHeaderNode:StatementNode
    {
        public DefinicionTipoNode tipo;
        public string nombre;
        public List<ParametrosNode> parametro;
        public override void ValidateSemantic()
        {
            throw new NotImplementedException();
        }
    }
    
}
