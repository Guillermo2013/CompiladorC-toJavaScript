using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;

namespace Compiladores.Arbol.StatementNodes
{
    class ForNode:StatementNode
    {
        public List<StatementNode> expresionInicio;
        public ExpressionNode expresionCondicional;
        public List<StatementNode> expresionFinal;
        public List<StatementNode> cuerpo;
        public override void ValidateSemantic()
        {
            throw new NotImplementedException();
        }
    }
}
