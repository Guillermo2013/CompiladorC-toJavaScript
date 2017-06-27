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
            if(cuerpo != null ) 
            foreach (var statement in cuerpo)
                statement.ValidateSemantic();
            return new TiposBases();
        }
        public override string GenerarCodigo()
        {
            string value = "default:";
            if (cuerpo != null) 
            foreach (var list in cuerpo)
            {
                value += "\n" + list.GenerarCodigo();
            }
            return value;
        }
    }
}
