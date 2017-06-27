using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
namespace Compiladores.Arbol.AccesoresNode
{
    class ArrayAccesor:AccesorNode
    {
        public List<ExpressionNode> dimension { get; set; }
        public override TiposBases ValidateSemantic()
        {
            var tipo = new ArrayTipo();
            tipo.cantidad.Add(new List<int>());
            foreach (var expression in dimension)
            {
                if (expression == null)
                    throw new SemanticoException(archivo+"la se expera una expresion" + expression.token.Fila + "columna" + expression.token.Columna);
                else if (!(expression.ValidateSemantic() is IntTipo))
                  throw new SemanticoException(archivo+"la expresion tiene que ser int fila" + expression.token.Fila + "columna" + expression.token.Columna);
                
                tipo.cantidad[0].Add(0);
            }
            return tipo;  
        }
        public override string GenerarCodigo()
        {
            var value = "";
            foreach (var lista in dimension)
            {
                value += "["+lista.GenerarCodigo()+"]";

            }
            return value;
        }
    }
}
