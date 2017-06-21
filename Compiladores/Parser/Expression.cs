using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.Literales;
using Compiladores.Arbol.Identificadores;
using Compiladores.Arbol.AccesoresNode;
using Compiladores.Arbol.BinaryNodes;
using Compiladores.Arbol.UnaryNode;
using Compiladores.Arbol.BaseNode;
using Compiladores.Arbol;
using Compiladores.Arbol.StatementNodes;
namespace Compiladores
{
    namespace hola {}

   public class Expression
    {
        private readonly Parser Parse;

       public Expression(Parser parser)
        {
            Parse = parser;
        }
       public ExpressionNode Expressions()
        {
           
           return conditionalExpressions();
        }

        private ExpressionNode conditionalExpressions()
        {
            var expressionDerecho = nullCoalescingExpressions();
            return conditionalExpressionsP(expressionDerecho);
        }

        private ExpressionNode conditionalExpressionsP(ExpressionNode expresion)
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.Interogacion)
            {
                var token = Parse.CurrentToken;   
               Parse.getNextToken();
               var expresionTrue = Expressions();
                if (Parse.CurrentToken.Tipo != TokenTipos.DosPuntos)
                    throw new ParserException("Se esperaba dos puntos en operacion ternaria"+Parse.CurrentToken.Columna + " " + Parse.CurrentToken.Fila);
                Parse.getNextToken();
                var expresionFalse = Expressions();
                return new ExpressionTernario() { expresion = expresion, expresionTrue = expresionTrue, expresionFalse = expresionFalse, token = token };
            }
            else {
                return expresion;
            }  
        }

        private ExpressionNode nullCoalescingExpressions()
        {
          
           var expressionIzquierda = conditionalOrExpressions();
          return nullCoalescingExpressionsP(expressionIzquierda);
        }

        private ExpressionNode nullCoalescingExpressionsP(ExpressionNode expresion)
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.NoEsNull)
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                var expresionDerecho = nullCoalescingExpressions();
                return new NoEsNullNode() { operador = Parse.CurrentToken.Lexema, OperadorIzquierdo = expresion, OperadorDerecho = expresionDerecho , token = token 
                };
            }
            else {
                return expresion;
            }

        }

        private ExpressionNode conditionalOrExpressions()
        {
           var expressionIzquierda = conditionalAndExpressions();
           return conditionalOrExpressionsP(expressionIzquierda);
        }
        private ExpressionNode conditionalOrExpressionsP(ExpressionNode expresion)
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.LogicosO)
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
              var expresionDerecho = nullCoalescingExpressions();
              var expresionIzquierda = new LogicosONode() { operador = Parse.CurrentToken.Lexema, OperadorIzquierdo = expresion, OperadorDerecho = expresionDerecho ,token = token};
               return conditionalOrExpressionsP(expresionIzquierda);
            }
            else {
                return expresion;
            }
        }
        private ExpressionNode conditionalAndExpressions()
        {
            var expresion = inclusiveOrExpressions();
            return conditionalAndExpressionsP(expresion);
        }

        private  ExpressionNode conditionalAndExpressionsP(ExpressionNode expresion)
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.LogicosY)
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
               var expresionDerecho = inclusiveOrExpressions();
               var expresionIzquierda = new LogicosYNode() { operador = Parse.CurrentToken.Lexema, OperadorIzquierdo = expresion, OperadorDerecho = expresionDerecho ,token = token};
               return conditionalAndExpressionsP(expresionIzquierda);
            }
            else {
                return expresion;
            }
        }

        private ExpressionNode inclusiveOrExpressions()
        {
            var expresion = exclusiveOrExpressions();
            return inclusiveOrExpressionsP(expresion);
        }

        private ExpressionNode inclusiveOrExpressionsP(ExpressionNode expresion)
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.OPorBit)
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                var expresionDerecho = exclusiveOrExpressions();
                var expresionIzquierda = new OPorBitNode() { operador = Parse.CurrentToken.Lexema, OperadorIzquierdo = expresion, OperadorDerecho = expresionDerecho,token = token };
                return inclusiveOrExpressionsP(expresionIzquierda);
            }
            else {
                return expresion;
            }
        }

        private ExpressionNode exclusiveOrExpressions()
        {
            var expresion = andExpressions();
            return exclusiveOrExpressionsP(expresion);
        }

        private ExpressionNode exclusiveOrExpressionsP(ExpressionNode expresion)
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.OExclusivoPorBit)
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                var expresionDerecho = andExpressions();
                var expresionIzquierda = new OExclusivoPorBitNode() { operador = Parse.CurrentToken.Lexema, OperadorIzquierdo = expresion, OperadorDerecho = expresionDerecho ,token = token};
                return exclusiveOrExpressionsP(expresionIzquierda);

            }
            else {
                return expresion;
            }
        }

        private ExpressionNode andExpressions()
        {
            var expresion = equalityExpressions();
            return andExpressionsP(expresion);
        }

        private ExpressionNode andExpressionsP(ExpressionNode expresion)
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.YPorBit)
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                var expresionDerecho = equalityExpressions();
                var expresionIzquierda = new YPorBitNode() { operador = Parse.CurrentToken.Lexema, OperadorIzquierdo = expresion, OperadorDerecho = expresionDerecho ,token = token};
                return andExpressionsP(expresionIzquierda);
            }
            else {
                return expresion;
            }
        }

        private ExpressionNode equalityExpressions()
        {
            var expresion = relationalExpressions();
            return equalityExpressionsP(expresion);
        }

        private ExpressionNode equalityExpressionsP(ExpressionNode expresion)
        {
            if(Parse.EquilityOperator.Contains(Parse.CurrentToken.Tipo)){
                var equilityOperator = ExpressionsEqualityOperator();
                equilityOperator.OperadorIzquierdo = expresion;
                equilityOperator.OperadorDerecho = relationalExpressions();
                return equalityExpressionsP(equilityOperator);
            }else{
                return expresion;
            }
        }

        private BinaryOperatorNode ExpressionsEqualityOperator()
        {
            if (Parse.EquilityOperator.Contains(Parse.CurrentToken.Tipo))
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                if (token.Tipo == TokenTipos.RelacionalIgual)
                    return new RelacionalIgualNode() { operador = token.Lexema ,token = token  };
                else if (token.Tipo == TokenTipos.RelacionalNoIgual)
                    return new RelacionalNoIgualNode() { operador = token.Lexema, token = token };
                
                }
            return null;
        }

        private ExpressionNode relationalExpressions()
        {
            var espresion = shiftExpressions();
            return relationalExpressionsP(espresion);
        }

        private ExpressionNode relationalExpressionsP(ExpressionNode expresion)
        {
            if (Parse.RelationalOperator.Contains(Parse.CurrentToken.Tipo))
            {
                var operador = ExpressionsRelationalOperator();
                operador.OperadorDerecho = shiftExpressions(); 
                operador.OperadorIzquierdo = expresion;
                return relationalExpressionsP(operador);
            }
            else if (Parse.IsAsOperator.Contains(Parse.CurrentToken.Tipo))
            {
                var operador = IsAsOperators();
                operador.OperadorIzquierdo = expresion;
                operador.OperadorDerecho = type();
               return relationalExpressionsP(operador);
            }else{
                return expresion;
            }
            
        }

        public ExpressionNode type()
        {
            if (Parse.Type.Contains(Parse.CurrentToken.Tipo)||Parse.CurrentToken.Tipo == TokenTipos.Identificador)
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                return new DefinicionTipoNode() { tipounico = token.Lexema,token = token };
            }
            else
            {
                throw new ParserException("se esperaba un tipo de dato"+Parse.CurrentToken.Columna + " " + Parse.CurrentToken.Fila);
            }
        }

        private BinaryOperatorNode IsAsOperators()
        {
            if (Parse.IsAsOperator.Contains(Parse.CurrentToken.Tipo))
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                if (token.Tipo == TokenTipos.PalabraReservadaAs)
                    return new AsNode() { operador = token.Lexema, token = token };
                if (token.Tipo == TokenTipos.PalabraReservadaIs)
                    return new IsNode() { operador = token.Lexema, token = token };
            }
            return null;
        }

        private BinaryOperatorNode ExpressionsRelationalOperator()
        {
            if (Parse.RelationalOperator.Contains(Parse.CurrentToken.Tipo))
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                if (token.Tipo == TokenTipos.RelacionalMayor)
                    return new RelacionalMayorNode() { operador = token.Lexema, token = token };
                if (token.Tipo == TokenTipos.RelacionalMayorOIgual)
                    return new RelacionalMayorOIgualNode() { operador = token.Lexema, token = token };
                if (token.Tipo == TokenTipos.RelacionalMenor)
                    return new RelacionalMenorNode() { operador = token.Lexema, token = token };
                if (token.Tipo == TokenTipos.RelacionalMayorOIgual)
                    return new RelacionalMenorOIgualNode() { operador = token.Lexema, token = token };
            }
            return null;
        }

        private ExpressionNode shiftExpressions()
        {
            var expresion = additiveExpressions();
            return shiftExpressionsP(expresion);

        }
        private ExpressionNode shiftExpressionsP(ExpressionNode expresion)
        {
            if (Parse.ShiftOperator.Contains(Parse.CurrentToken.Tipo))
            {
             var operador = ExpressionsShiftOperator();
             operador.OperadorIzquierdo = expresion;
             operador.OperadorDerecho = additiveExpressions();
             return shiftExpressionsP(operador);
          }else{
              return expresion; 
          }
        }

        private BinaryOperatorNode ExpressionsShiftOperator()
        {
            if (Parse.ShiftOperator.Contains(Parse.CurrentToken.Tipo))
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                if (token.Tipo == TokenTipos.DeplazamientoDerecha)
                    return new DeplazamientoDerechaNode() { operador = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.DeplazamientoIzquierda)
                    return new DeplazamientoIzquierdaNode() { operador = token.Lexema, token = token };
            }
            return null;
        }

        private ExpressionNode additiveExpressions()
        {
            var expresion = multiplicativeExpressions();
            return additiveExpressionsP(expresion);
        }

        private ExpressionNode additiveExpressionsP(ExpressionNode expresion)
        {
            if (Parse.AdditiveOperator.Contains(Parse.CurrentToken.Tipo))
            {
                var operador = additiveOperators();
                operador.OperadorIzquierdo = expresion;
                operador.OperadorDerecho =  multiplicativeExpressions();
                return additiveExpressionsP(operador);
            }
            else { 
                return expresion;
            }
        }

        private BinaryOperatorNode additiveOperators()
        {
            if (Parse.AdditiveOperator.Contains(Parse.CurrentToken.Tipo))
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                if (token.Tipo == TokenTipos.OperacionSuma)
                    return new SumaNode() { operador = token.Lexema, token = token };
                if (token.Tipo == TokenTipos.OperacionResta)
                    return new RestaNode() { operador = token.Lexema, token = token };
            }
            return null;
        }

        private ExpressionNode multiplicativeExpressions()
        {
            var expresion = unaryExpressions();
            return multiplicativeExpressionsFactorized(expresion);
        }

        private ExpressionNode multiplicativeExpressionsFactorized(ExpressionNode expresion)
        {


            if (Parse.AssignmentOperator.Contains(Parse.CurrentToken.Tipo))
            {
                var operador = assignmentOperator();
                operador.OperadorIzquierdo = expresion;
                operador.OperadorDerecho = Expressions();
                return multiplicativeExpressionsP(operador);
            }
            else 
            {
                return multiplicativeExpressionsP(expresion);
            }
        }

        private ExpressionNode multiplicativeExpressionsP(ExpressionNode expresion)
        {
            if (Parse.MultiplicativeOperator.Contains(Parse.CurrentToken.Tipo))
            {
               var operador = multiplicativeOperators();
               operador.OperadorIzquierdo = expresion;
                operador.OperadorDerecho = unaryExpressions();
               return multiplicativeExpressionsP(operador);
            }
            else {
                return expresion;
            }
        }

        private BinaryOperatorNode multiplicativeOperators()
        {
            if (Parse.MultiplicativeOperator.Contains(Parse.CurrentToken.Tipo))
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                if (token.Tipo == TokenTipos.OperacionMultiplicacion)
                    return new MultiplicacionNode() { operador = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.OperacionDivision)
                    return new DivisionNode() { operador = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.OperacionDivisionResiduo)
                    return new ResiduoNode() { operador = token.Lexema, token = token };
            }
            return null;
        }

        public BinaryOperatorNode assignmentOperator()
        {
            if (Parse.AssignmentOperator.Contains(Parse.CurrentToken.Tipo))
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                if (token.Tipo == TokenTipos.Asignacion)
                    return new AsignacionNode() { operador = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.AutoOperacionSuma)
                    return new AutoOperacionSumaNode() { operador = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.AutoOperacionResta)
                    return new AutoOperacionRestaNode() { operador = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.AutoOperacionMultiplicacion)
                    return new AutoOperacionMultiplicacionNode() { operador = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.AutoOperacionDivision)
                    return new AutoOperacionDivisionNode() { operador = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.AutoOperacionResiduo)
                    return new AutoOperacionResiduoNode() { operador = token.Lexema };
                else if (token.Tipo == TokenTipos.AutoOperacionOExclusivoPorBit)
                    return new AutoOperacionOExclusivoPorBitNode() { operador = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.AutoOperacionAndPorBit)
                    return new AutoOperacionAndPorBitNode() { operador = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.AutoOperacionOrPorBit)
                    return new AutoOperacionOrPorBitNode() { operador = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.AutoDeplazamientoDerecha)
                    return new AutoDeplazamientoDerechaNode() { operador = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.AutoDeplazamientoIzquierda)
                    return new AutoDeplazamientoIzquierdaNode() { operador = token.Lexema, token = token };
            }
            return null;
        }

        public ExpressionNode unaryExpressions()
        {
            if (Parse.UnaryOperator.Contains(Parse.CurrentToken.Tipo))
            {
                var operador = ExpressionsUnaryOperator();
                operador.Operando = unaryExpressions();
                return operador;
            }
            
            else if(Parse.CurrentToken.Tipo == TokenTipos.ParentesisIzquierdo ||
                Parse.CurrentToken.Tipo == TokenTipos.PalabraReservadaNew||
                Parse.Literal.Contains(Parse.CurrentToken.Tipo)||
                Parse.CurrentToken.Tipo == TokenTipos.PalabraReservadaThis||
                Parse.CurrentToken.Tipo == TokenTipos.Identificador)
            {
                return primaryExpressions();
            }
            return null;
        }

        public UnaryOperatorNode ExpressionsUnaryOperator()
        {
            if (Parse.UnaryOperator.Contains(Parse.CurrentToken.Tipo))
            {
                var token = Parse.CurrentToken; 
                Parse.getNextToken();
                if (token.Tipo == TokenTipos.AutoOperacionIncremento)
                    return new UnaryAutoOperacionIncrementoNode() { Value = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.AutoOperacionDecremento)
                    return new UnaryAutoOperacionDecrementoNode() { Value = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.Negacion)
                    return new UnaryNegacionNode() { Value = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.NegacionPorBit)
                    return new UnaryNegacionPorBitNode() { Value = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.NegacionPorBit)
                    return new UnaryNegacionPorBitNode() { Value = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.OperacionSuma)
                    return new UnaryOperadorSuma() { Value = token.Lexema, token = token };
                else if (token.Tipo == TokenTipos.OperacionResta)
                    return new UnarioOperadorResta() { Value = token.Lexema, token = token };

             }
            return null;

        }

        public ExpressionNode primaryExpressions()
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.PalabraReservadaNew)
            {
                Parse.getNextToken();
              var newToken = instanceExpressions(new NewExpressionNode());
              return primaryExpressionsP(newToken);
            }
            else if (Parse.Literal.Contains(Parse.CurrentToken.Tipo))
            {
              return  literal();
               
            }
            else if (Parse.CurrentToken.Tipo == TokenTipos.PalabraReservadaThis)
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                var expresion = primaryExpressionsP(null);
                return new ThisNode() { expresion = expresion, token = token };
            }

            else if (Parse.CurrentToken.Tipo == TokenTipos.Identificador)
            {
                var Identificador = Parse.CurrentToken.Lexema;
                Parse.getNextToken();
                var identificador =  optionalFunctOrArrayCall(Identificador);
                return primaryExpressionsP(identificador);
            }
            else if (Parse.CurrentToken.Tipo == TokenTipos.ParentesisIzquierdo) {
                Parse.getNextToken();
                if (Parse.Type.Contains(Parse.CurrentToken.Tipo))
                {
                    var tipo = Parse.CurrentToken;
                    Parse.getNextToken();
                    if (Parse.CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
                    {
                        throw new ParserException("se esperaba un parentesis" + Parse.CurrentToken.Columna + " " + Parse.CurrentToken.Fila);
                    }
                    Parse.getNextToken();
                    return new CastNode() { tipo = tipo.Lexema,expresion = primaryExpressions(), token = tipo};
                }
                else
                {
                 var expresion = Expressions();
                    if (Parse.CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
                    {
                        throw new ParserException("se esperaba un parentesis" + Parse.CurrentToken.Columna + " " + Parse.CurrentToken.Fila);
                    }
                    Parse.getNextToken();
                    return expresion;
                }
            }
            else 
                throw new ParserException("Se esperaba un new ,Literal, identificador"+Parse.CurrentToken.Columna + " " + Parse.CurrentToken.Fila);
          
        }
        private NewExpressionNode instanceExpressions(NewExpressionNode expresion)
        {
            if (Parse.Type.Contains(Parse.CurrentToken.Tipo) || Parse.CurrentToken.Tipo == TokenTipos.Identificador)
            {
                expresion.tipo = type() as DefinicionTipoNode;
               return instanceExpressionsFactorized(expresion);
            }
            throw new ParserException("se espere un tipo de dato");
        }

        private NewExpressionNode instanceExpressionsFactorized(NewExpressionNode expresion)
        {
            
            if (Parse.CurrentToken.Tipo == TokenTipos.CorcheteIzquierdo)
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                expresion.arraySize.Add( ExpressionsList());
                if (Parse.CurrentToken.Tipo != TokenTipos.CorcheteDerecho)
                {
                    throw new ParserException("Se esperaba un Corchete "+Parse.CurrentToken.Columna + " " + Parse.CurrentToken.Fila);
                }
                Parse.getNextToken();
             
                expresion.inicializable = new ArrayNode() { expresion = optionalArrayInitializer() , tipo = expresion.tipo, token = token};
                return expresion;
            }
            else if (Parse.CurrentToken.Tipo == TokenTipos.ParentesisIzquierdo)
            {
                Parse.getNextToken();
              var argumentlist =  argumentList();
                if (Parse.CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
                {
                    throw new ParserException("Se esperaba un Corchete "+Parse.CurrentToken.Columna + " " + Parse.CurrentToken.Fila);
                }
                Parse.getNextToken();
                expresion.parametros = argumentlist;
                return expresion;
            }else{
                var token = Parse.CurrentToken;
                expresion.arraySize = rankSpecifierList();
                expresion.inicializable = new ArrayNode() { expresion = arrayInitializer(),token = token };
                return expresion;
            }
            
        }

        private List<List<ExpressionNode>> rankSpecifierList()
        {
            var expresion = rankSpecifier();
            var expresionLista = optionalRankSpecifierList();
            expresionLista.Insert(0, expresion);
            return expresionLista;
        }

        private List<ExpressionNode> rankSpecifier()
        {
         
                var comas = optionalCommaList();
                if (Parse.CurrentToken.Tipo != TokenTipos.CorcheteDerecho)
                {
                    throw new ParserException("se esperaba un corchete"+Parse.CurrentToken.Columna + " " + Parse.CurrentToken.Fila);                   
                }
                Parse.getNextToken();
                return comas;
            
        }

        private List<ExpressionNode> optionalCommaList()
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.Separador)
            {
               return   commaList();
            }
            else {
                return new List<ExpressionNode>();
            }
         }

        private List<ExpressionNode> commaList()
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.Separador)
            {
                 var token = Parse.CurrentToken;
                Parse.getNextToken();
                var lista = new List<ExpressionNode>();
                var listaExpresion = optionalCommaList();
                listaExpresion.Insert(0, new SeparadorNode() {  token = token});
                return listaExpresion;
            }
            else
            {
                return new List<ExpressionNode>();
            }
        }

        private List<ExpressionNode> optionalArrayInitializer()
        {
            if(Parse.CurrentToken.Tipo == TokenTipos.LlaveIzquierdo){
               return arrayInitializer();
            }else{
                return new List<ExpressionNode>();
            }
            
        }

        public List<ExpressionNode> arrayInitializer()
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.LlaveIzquierdo)
            {
                Parse.getNextToken();
                var array = optionalVariableInitializerList();

                if (Parse.CurrentToken.Tipo != TokenTipos.LlaveDerecho)
                    throw new ParserException("se esperaba llaves " + Parse.CurrentToken.Columna + " " + Parse.CurrentToken.Fila);
                Parse.getNextToken();
                return array;
            }
            else
            {
                return new List<ExpressionNode>();
            }
        }

        public List<ExpressionNode> optionalVariableInitializerList()
        {
           return variableInitializerList();
        }

       public List<ExpressionNode> variableInitializerList()
        {
           var expresion = variableInitializer();
           var expresionList = variableInitializerListP();
           expresionList.Insert(0, expresion);
           return expresionList;
        }
       public ExpressionNode variableInitializer()
        {
           var lista = new List<List<ExpressionNode>>();
            if (Parse.CurrentToken.Tipo == TokenTipos.LlaveIzquierdo)
            {
                var token = Parse.CurrentToken;
                return new ArrayNode() { expresion = arrayInitializer(),token = token }; 
                
            }
            else
            {
                return Expressions();      
            }

        }
       private List<ExpressionNode> variableInitializerListP()
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.Separador)
            {
                Parse.getNextToken();
               return variableInitializerList();
            }
            else
            {
                return new List<ExpressionNode>();

            }
           
        }
        public List<List<ExpressionNode>> optionalRankSpecifierList()
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.CorcheteIzquierdo)
            {
                Parse.getNextToken();

              return rankSpecifierList();
            }
            else {
                return new List<List<ExpressionNode>>();
            }
        }

        public ExpressionNode literal()
        {
            if (Parse.Literal.Contains(Parse.CurrentToken.Tipo))
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                if (token.Tipo == TokenTipos.Numero || token.Tipo == TokenTipos.NumeroHexagecimal)
                    return new LiteralNumerico() { valor = int.Parse(token.Lexema), token = token };
                else if (token.Tipo == TokenTipos.NumeroFloat)
                    return new LiteralesFloat() { valor = float.Parse(token.Lexema), token = token };
                else if (token.Tipo == TokenTipos.LiteralChar)
                    return new LiteralChar() { valor = char.Parse(token.Lexema), token = token };
                else if (token.Tipo == TokenTipos.NumeroBinario)
                    return new LiteralBinaria() { valor = byte.Parse(token.Lexema), token = token };
                else
                    return new LiteralesStrings() { valor = token.Lexema, token = token };
            }else
                throw new ParserException("Se esperaba un Literal" + Parse.CurrentToken.Columna + " " + Parse.CurrentToken.Fila);
        
        }

        public ExpressionNode primaryExpressionsP(ExpressionNode expresion)
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.Punto)
            {
                Parse.getNextToken();
                if (Parse.CurrentToken.Tipo != TokenTipos.Identificador)
                {
                    throw new ParserException("Se esperaba un identificador"+Parse.CurrentToken.Columna + " " + Parse.CurrentToken.Fila);
                }
                var identificadorString = Parse.CurrentToken.Lexema;
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                var identificador = optionalFunctOrArrayCall(identificadorString);
                var expresionList = primaryExpressionsP(identificador);
                if (expresion is IdentificadoresExpressionNode)
                    (expresion as IdentificadoresExpressionNode).ListaDeAccesores.Add(new PuntoAccesor() { identificador = identificador , token = token});
                else if (expresion is CallFuntionNode)
                    (expresion as CallFuntionNode).ListaDeAccesores.Add(new PuntoAccesor() { identificador = identificador ,token = token });
                else
                    return new PuntoExpresion() { operador = ".", OperadorIzquierdo = expresion, OperadorDerecho = identificador, token = token };
                return expresion;
            }
            else if (Parse.CurrentToken.Tipo == TokenTipos.AutoOperacionIncremento || Parse.CurrentToken.Tipo == TokenTipos.AutoOperacionDecremento)
            {
                var lista = new List<ExpressionNode>();
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                if (Parse.CurrentToken.Tipo == TokenTipos.AutoOperacionDecremento)
                {
                    var unaryDec = new UnaryAutoOperacionDecrementoNode() { Value = token.Lexema , Operando = expresion ,token = token};
                    return unaryDec;
                }
                else
                {
                    var unaryInc = new UnaryAutoOperacionIncrementoNode() { Value = token.Lexema, Operando = expresion, token = token };
                    return unaryInc;
                }
           
            }
            else
            {
                return expresion;
            }
        }

        public ExpressionNode optionalFunctOrArrayCall(string identificador)
        {

            if (Parse.CurrentToken.Tipo == TokenTipos.ParentesisIzquierdo)
            {
                var token = Parse.CurrentToken;
                Parse.getNextToken();
                var argumentlist = argumentList();
                if (Parse.CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
                {
                    throw new ParserException("Se esperaba un parentesis derecho"+Parse.CurrentToken.Columna + " " + Parse.CurrentToken.Fila);
                }
                Parse.getNextToken();
                return new CallFuntionNode() { nombre = identificador, parametros = argumentlist, token = token };
            
            }
            else if (Parse.CurrentToken.Tipo == TokenTipos.CorcheteIzquierdo)
            {
                var token = Parse.CurrentToken;
                var listaArrays = optionalArrayAccessList();
                return new IdentificadoresExpressionNode() { nombre = identificador, ListaDeAccesores = listaArrays, token = token }; 
            }
            else {
                var token = Parse.CurrentToken;
                return new IdentificadoresExpressionNode() { nombre = identificador, token = token };
            }
        }

        public List<AccesorNode> optionalArrayAccessList()
        {
             if (Parse.CurrentToken.Tipo == TokenTipos.CorcheteIzquierdo)
             {
                 var token = Parse.CurrentToken;
                Parse.getNextToken();
               var expresionList = ExpressionsList();
                if (Parse.CurrentToken.Tipo != TokenTipos.CorcheteDerecho)
                {
                    throw new ParserException("Se esperaba un corchete derecho "+Parse.CurrentToken.Columna + " " + Parse.CurrentToken.Fila);
                }
                var ArrayNode = new ArrayAccesor() { dimension = expresionList, token = token };
                Parse.getNextToken();
               var arrayAccessList =  optionalArrayAccessList();
               arrayAccessList.Insert(0, ArrayNode);
               return arrayAccessList;
             }
             else {
                 return new List<AccesorNode>();
             }
        }

        private List<ExpressionNode> ExpressionsList()
        {
           var expresion =  Expressions();
           var expresionList = optionalExpressionsList();
            expresionList.Insert(0,expresion);
            return expresionList;
        }

        private List<ExpressionNode> optionalExpressionsList()
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.Separador)
            {
                Parse.getNextToken();
                return ExpressionsList();

            }
            else {
                return new List<ExpressionNode>();
            
            }
                
        }

        private List<ExpressionNode> argumentListP()
        {
            if (Parse.CurrentToken.Tipo == TokenTipos.Separador)
            {
                Parse.getNextToken();
                var expresion = Expressions();
                var expresionList = argumentListP();
                expresionList.Insert(0, expresion);
                return expresionList;

            }
            else {
                return new List<ExpressionNode>();
            };
        }
        public List<ExpressionNode> argumentList()
        {
            if (Parse.CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
            {
               var expresion = Expressions();
               var expresionList = argumentListP();
               expresionList.Insert(0, expresion);
               return expresionList;
            }
            else {
                return new List<ExpressionNode>();
            }
        }
      
    }
}
    
