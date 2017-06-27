using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico.Tipos;
using Compiladores.Semantico;

namespace Compiladores.Arbol.UnaryNode
{
    class UnaryOperadorSuma:UnaryOperatorNode
    {
        public override Semantico.TiposBases ValidateSemantic()
        {
                
             var tipo = Operando.ValidateSemantic();
            if (!(tipo is IntTipo) && !(tipo is FloatTipo))
                throw new SemanticoException(archivo+"la expresion tiene que se un int fila" + token.Fila + "columana" + token.Columna);
            return tipo;
        }
        public override string GenerarCodigo()
        {
            string valor = this.Value;
            valor += this.Operando.GenerarCodigo();
            return valor;
        }
    }
}
