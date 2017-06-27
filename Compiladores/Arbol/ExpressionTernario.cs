using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;

namespace Compiladores.Arbol
{
    public class ExpressionTernario:ExpressionNode
    {
        public ExpressionNode expresion;
        public ExpressionNode expresionTrue;
        public ExpressionNode expresionFalse;
        public override TiposBases ValidateSemantic()
        {
            var expresion1 = expresion.ValidateSemantic();
            if (!(expresion1 is BooleanTipo)) 
                throw new SemanticoException(archivo+"la expresion debe ser booleana no "+expresion1+ "fila "+expresion.token.Fila+"columna"+expresion.token.Columna);
            var trueExpresion = expresionTrue.ValidateSemantic();
            var falseExpresion = expresionTrue.ValidateSemantic();
            if(trueExpresion.GetType() != falseExpresion.GetType())
                throw new SemanticoException(archivo+"la expresiones tiene que ser del mismo tipo " +trueExpresion+"no " + falseExpresion + "fila " + expresionFalse.token.Fila + "columna" + expresionFalse.token.Columna);
            return trueExpresion;
        }
        public override string GenerarCodigo()
        {
            string valor = "("+expresion.GenerarCodigo()+")";
            valor += "?";
            valor += expresionTrue.GenerarCodigo();
            valor += ":";
            valor += expresionFalse.GenerarCodigo();
            return valor;
        }
    }
}
