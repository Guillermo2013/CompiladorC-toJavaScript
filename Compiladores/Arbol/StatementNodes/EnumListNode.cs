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
    class EnumListNode:StatementNode
    {
        public string identificador;
        public ExpressionNode asignacion;
        public override void ValidateSemantic()
        {
            if (asignacion != null)
            {
                var tipo = asignacion.ValidateSemantic();
                if (!(tipo is IntTipo))
                    throw new SemanticoException("la expresion de " + identificador + " debe ser numerica fila " + token.Fila + " colunma" + token.Columna);
            }
          
        }
    }
}
