using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
namespace Compiladores.Arbol.StatementNodes
{
    class DefaulNode:CasosNode
    {
        public  List<StatementNode> cuerpo;
        public override TiposBases ValidateSemantic()
        {
            foreach (var statement in cuerpo)
                statement.ValidateSemantic();
            return null;
        }
        public abstract class Class1 : Class2
        {
            public abstract int funcion();
            int test;
        }
        public class Class2
        {
           public int funcion2() { return 0; }
           public int test2;
        }
    }
}
