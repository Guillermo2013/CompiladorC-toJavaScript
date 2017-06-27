using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Arbol.StatementNodes;
using Compiladores.Arbol.Identificadores;
using Compiladores.Arbol;
using Compiladores.Arbol.UnaryNode;
using Compiladores.Arbol.Literales;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
using Compiladores.Arbol.BinaryNodes;
namespace Compiladores
{
    

    public class Parser
    {
        public Lexico lexico;
        public Token CurrentToken;
        public List<TokenTipos> tiposDeDatos = new List<TokenTipos>();
        public List<TokenTipos> EquilityOperator = new List<TokenTipos>();
        public List<TokenTipos> RelationalOperator = new List<TokenTipos>();
        public List<TokenTipos> IsAsOperator = new List<TokenTipos>();
        public List<TokenTipos> Type = new List<TokenTipos>();
        public List<TokenTipos> ShiftOperator = new List<TokenTipos>();
        public List<TokenTipos> AdditiveOperator = new List<TokenTipos>();
        public List<TokenTipos> AssignmentOperator = new List<TokenTipos>();
        public List<TokenTipos> MultiplicativeOperator = new List<TokenTipos>();
        public List<TokenTipos> UnaryOperator = new List<TokenTipos>();
        public List<TokenTipos> Literal = new List<TokenTipos>();
        public List<TokenTipos> Encapsulation = new List<TokenTipos>();
        public List<TokenTipos> OptionalModifier = new List<TokenTipos>();      
        public readonly Expression Expression;
        public string archivo;
       
        public Parser(Lexico lexico)
        {
            this.lexico = lexico;
            this.CurrentToken = lexico.ObtenerSiguienteToken();
            EquilityOperator.Add(TokenTipos.RelacionalIgual);
            EquilityOperator.Add(TokenTipos.RelacionalNoIgual);
            RelationalOperator.Add(TokenTipos.RelacionalMayor);
            RelationalOperator.Add(TokenTipos.RelacionalMayorOIgual);
            RelationalOperator.Add(TokenTipos.RelacionalMenor);
            RelationalOperator.Add(TokenTipos.RelacionalMenorOIgual);
            Type.Add(TokenTipos.PalabraReservadaInt);
            Type.Add(TokenTipos.PalabraReservadaChar);
            Type.Add(TokenTipos.PalabraReservadaFloat);
            Type.Add(TokenTipos.PalabraReservadaBool);
            Type.Add(TokenTipos.PalabraReservadaString);
            Type.Add(TokenTipos.PalabraReservadaVar);
            ShiftOperator.Add(TokenTipos.DeplazamientoDerecha);
            ShiftOperator.Add(TokenTipos.DeplazamientoIzquierda);
            AdditiveOperator.Add(TokenTipos.OperacionSuma);
            AdditiveOperator.Add(TokenTipos.OperacionResta);
            AssignmentOperator.Add(TokenTipos.Asignacion);
            AssignmentOperator.Add(TokenTipos.AutoOperacionSuma);
            AssignmentOperator.Add(TokenTipos.AutoOperacionResta);
            AssignmentOperator.Add(TokenTipos.AutoOperacionMultiplicacion);
            AssignmentOperator.Add(TokenTipos.AutoOperacionDivision);
            AssignmentOperator.Add(TokenTipos.AutoOperacionResiduo);
            AssignmentOperator.Add(TokenTipos.AutoOperacionOExclusivoPorBit);
            AssignmentOperator.Add(TokenTipos.AutoOperacionAndPorBit);
            AssignmentOperator.Add(TokenTipos.AutoOperacionOrPorBit);
            AssignmentOperator.Add(TokenTipos.AutoDeplazamientoDerecha);
            AssignmentOperator.Add(TokenTipos.AutoDeplazamientoIzquierda);
            MultiplicativeOperator.Add(TokenTipos.OperacionMultiplicacion);
            MultiplicativeOperator.Add(TokenTipos.OperacionDivision);
            MultiplicativeOperator.Add(TokenTipos.OperacionDivisionResiduo);
            UnaryOperator.Add(TokenTipos.OperacionSuma);
            UnaryOperator.Add(TokenTipos.OperacionResta);
            UnaryOperator.Add(TokenTipos.AutoOperacionIncremento);
            UnaryOperator.Add(TokenTipos.AutoOperacionDecremento);
            UnaryOperator.Add(TokenTipos.Negacion);
            UnaryOperator.Add(TokenTipos.NegacionPorBit);
            UnaryOperator.Add(TokenTipos.OperacionMultiplicacion);
            Literal.Add(TokenTipos.Numero);
            Literal.Add(TokenTipos.NumeroHexagecimal);
            Literal.Add(TokenTipos.NumeroOctal);
            Literal.Add(TokenTipos.NumeroFloat);
            Literal.Add(TokenTipos.Numero);
            Literal.Add(TokenTipos.LiteralChar);
            Literal.Add(TokenTipos.LiteralString);
            Literal.Add(TokenTipos.PalabraReservadaTrue);
            Literal.Add(TokenTipos.PalabraReservadaFalse);
            Encapsulation.Add(TokenTipos.PalabraReservadaPublic);
            Encapsulation.Add(TokenTipos.PalabraReservadaPrivate);
            Encapsulation.Add(TokenTipos.PalabraReservadaProtected);
            OptionalModifier.Add(TokenTipos.PalabraReservadaOverride);
            OptionalModifier.Add(TokenTipos.PalabraReservadaStatic);
            OptionalModifier.Add(TokenTipos.PalabraReservadaVirtual);
            OptionalModifier.Add(TokenTipos.PalabraReservadaAbstract);
            IsAsOperator.Add(TokenTipos.PalabraReservadaIs);
            IsAsOperator.Add(TokenTipos.PalabraReservadaAs);
            Expression = new Expression(this);
  
        }

        private Token builtInType()
        {
           
            if (Type.Contains(CurrentToken.Tipo))
            {
                var token = CurrentToken;
                getNextToken();
                return token;

            }
            else
            {
                throw new ParserException(archivo+"se esperaba un tipo de dato" + CurrentToken.Columna + " " + CurrentToken.Fila);
            }
        }
        private DefinicionTipoNode type()
        {
            if (Type.Contains(CurrentToken.Tipo))
            {
                var token = CurrentToken;
              var tipo =  builtInType();
                var array = Expression.optionalRankSpecifierList();
                return new DefinicionTipoNode() { archivo = archivo, tipounico = tipo.Lexema, array = array, token = token };
            }
            else if (CurrentToken.Tipo == TokenTipos.Identificador)
            {
                var token = CurrentToken;
                var identificar = qualifiedIdentifier();
                var array = Expression.optionalRankSpecifierList();
                return new DefinicionTipoNode() { archivo = archivo, tipounico = identificar, array = array, token = token };
            }
            return null;
        }

        private string qualifiedIdentifier()
        {
            if (CurrentToken.Tipo == TokenTipos.Identificador)
            {
                var identificador = CurrentToken.Lexema;
                getNextToken();
                var lista = string.Concat(identificador,identifierAttribute());
               
                return lista;
            }
            else {
                return "";
            }
        }

       
        public void getNextToken()
        {
            CurrentToken = lexico.ObtenerSiguienteToken();
        } 
        public List<StatementNode> Code()
        {
            return compilationUnit();
        }

        private List<StatementNode> compilationUnit()
        {
            var directivas = optionalUsingDirective();
            var nameSpaces = optionalNamespaceMemberDeclaration();
            directivas.AddRange(nameSpaces);
            return directivas;
        }

        private List<StatementNode> optionalNamespaceMemberDeclaration()
        {
              return  namespaceMemberDeclaration();
        }

        private List<StatementNode> namespaceMemberDeclaration()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaNamespace)
            {
               var statement = namespaceDeclaration();
               var statementlist = optionalNamespaceMemberDeclaration();
               statementlist.Insert(0, statement);
               return statementlist;     
            }
            else if (CurrentToken.Tipo == TokenTipos.PalabraReservadaEnum || CurrentToken.Tipo == TokenTipos.PalabraReservadaClase ||
                CurrentToken.Tipo == TokenTipos.PalabraReservadaInterfaz || CurrentToken.Tipo == TokenTipos.PalabraReservadaPublic ||
                CurrentToken.Tipo == TokenTipos.PalabraReservadaProtected || CurrentToken.Tipo == TokenTipos.PalabraReservadaPrivate)
            {
                var statement = typeDeclarationList();
                var statementlist = optionalNamespaceMemberDeclaration();
                statementlist.InsertRange(0, statement);
                return statementlist;  
            }else{
                return new List<StatementNode>();
            }
            
        }

        private List<StatementNode> typeDeclarationList()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaEnum || CurrentToken.Tipo == TokenTipos.PalabraReservadaClase ||
                CurrentToken.Tipo == TokenTipos.PalabraReservadaInterfaz || CurrentToken.Tipo == TokenTipos.PalabraReservadaPublic ||
                CurrentToken.Tipo == TokenTipos.PalabraReservadaProtected || CurrentToken.Tipo == TokenTipos.PalabraReservadaPrivate)
            {
             var statement = typeDeclaration();
             var statementlist = typeDeclarationList();
             statementlist.Insert(0,statement);
             return statementlist;
            }
            else {
                return new List<StatementNode>();
            }

            
        }

        private StatementNode typeDeclaration()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaEnum || CurrentToken.Tipo == TokenTipos.PalabraReservadaClase ||
                CurrentToken.Tipo == TokenTipos.PalabraReservadaInterfaz || CurrentToken.Tipo == TokenTipos.PalabraReservadaPublic ||
                CurrentToken.Tipo == TokenTipos.PalabraReservadaProtected || CurrentToken.Tipo == TokenTipos.PalabraReservadaPrivate)
            {
               var encapsulamiento = encapsulationModifier();
               return groupDeclaration(encapsulamiento);
            }else

                throw new ParserException(archivo+"se espera una interface , enum o clase" + CurrentToken.Columna + " " + CurrentToken.Fila);
            

        }

        private StatementNode namespaceDeclaration()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaNamespace)
            {
                var token = CurrentToken;
                getNextToken();
                var identificador = CurrentToken.Lexema;
                if(CurrentToken.Tipo != TokenTipos.Identificador){
                    throw new ParserException(archivo+"se esperaba un identificador" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var identificadorlist = identifierAttribute();
                var lista =  namespaceBody();
                TablaDeNamespace.Instance.DeclareNameSpace(string.Concat(identificador, identificadorlist), lista);
                return new NameSpaceNode() { archivo = archivo, nombre = string.Concat(identificador, identificadorlist), statemenList = lista, token = token };

            }
            return null;
        }

        private List<StatementNode> namespaceBody()
        {
            if (CurrentToken.Tipo == TokenTipos.LlaveIzquierdo)
            {
                getNextToken();
                var usingList = optionalUsingDirective();
                var namespaceList = optionalNamespaceMemberDeclaration();
                namespaceList.InsertRange(0, usingList);
                if (CurrentToken.Tipo != TokenTipos.LlaveDerecho)
                {
                    throw new ParserException(archivo+"se esperaba una llaver" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                return namespaceList;
            }else
                throw new ParserException(archivo+"se esperaba un parentesis " + CurrentToken.Columna + " " + CurrentToken.Fila);
        }

        public string identifierAttribute()
        {
            if (CurrentToken.Tipo == TokenTipos.Punto)
            {
                var punto = CurrentToken.Lexema;
                getNextToken();
                var identificador = CurrentToken.Lexema;
                if (CurrentToken.Tipo != TokenTipos.Identificador)
                {
                    throw new ParserException(archivo+"se espera un identificador" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var listaIdentificador = identifierAttribute();
                var id = string.Concat(punto, identificador, listaIdentificador);
                return id;
            }
            else {
                return "";
            }
        }

        private List<StatementNode> optionalUsingDirective()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaUsing)
            {
                return usingDirective();
            }
            else {
                return new List<StatementNode>();
            }
        }

        private List<StatementNode> usingDirective()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaUsing)
            {
                var token = CurrentToken;
                getNextToken();
                 if (CurrentToken.Tipo != TokenTipos.Identificador)
                {
                    throw new ParserException(archivo+"Se espera un identificador" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                 var identificador =  CurrentToken.Lexema;
                 getNextToken();
                var listaDeidentificador = identifierAttribute();
               
                if (CurrentToken.Tipo != TokenTipos.FinalDeSentencia)
                {
                    throw new ParserException(archivo+"Se espera un ;" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var usingList = new UsingNode() { archivo = archivo , identificador = string.Concat(identificador, listaDeidentificador), token = token };
               var listaUsing = optionalUsingDirective();
               listaUsing.Insert(0, usingList);
               return listaUsing;
            }
            else
            {
                return new List<StatementNode>();
            }
        }
        private StatementNode groupDeclaration(Token encapsulamiento)
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaClase || CurrentToken.Tipo == TokenTipos.PalabraReservadaAbstract)
             return classDeclaration(encapsulamiento);
            else if (CurrentToken.Tipo == TokenTipos.PalabraReservadaInterfaz)
             return  interfaceDeclaration(encapsulamiento);
            else if (CurrentToken.Tipo == TokenTipos.PalabraReservadaEnum)
             return  enumDeclaration(encapsulamiento);
            else
                throw new ParserException(archivo+"se espera un class , enum o interfaz" + CurrentToken.Columna + " " + CurrentToken.Fila);
        }

        private Token encapsulationModifier()
        {
            if (Encapsulation.Contains(CurrentToken.Tipo))
            {
                var encapsulamiento = CurrentToken;
                getNextToken();
                return encapsulamiento;
            }
            else {
                return null;
            }
        }
        private StatementNode enumDeclaration(Token encapsulamiento)
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaEnum)
            {
                var token = CurrentToken;
                getNextToken();
                var identificador = CurrentToken;
                if (CurrentToken.Tipo != TokenTipos.Identificador)
                {
                    throw new ParserException(archivo+"se espera identificador" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var enumbody = enumBody();
                optionalBodyEnd();
                return new EnumNode() { archivo = archivo, encasulamiento = encapsulamiento != null ? encapsulamiento.Lexema : "public", identificador = identificador.Lexema, lista = enumbody, token = token };
            }
            return null;
        }

        private List<EnumListNode> enumBody()
        {
            if (CurrentToken.Tipo == TokenTipos.LlaveIzquierdo)
            {
                getNextToken();
                var enumBodyList = optionalAssignableIdentifiersList();
                if (CurrentToken.Tipo != TokenTipos.LlaveDerecho)
                {
                    throw new ParserException(archivo+"se esperaba una llave" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                return enumBodyList;
            }
            else
                throw new ParserException(archivo+"se esperaba un llave" + CurrentToken.Columna + " " + CurrentToken.Fila);
        }

        private List<EnumListNode> optionalAssignableIdentifiersList()
        {
            if (CurrentToken.Tipo == TokenTipos.Identificador)
            {
                var token = CurrentToken;
                getNextToken();
               return assignmentOptions(token);
            }
            else {
                return new List<EnumListNode>();
            }
        }

        private List<EnumListNode> assignmentOptions(Token identificador)
        {
            if(CurrentToken.Tipo == TokenTipos.Asignacion){
                
                getNextToken();
                var expresion = Expression.Expressions();
                var enumId = new EnumListNode() { archivo = archivo,  identificador = identificador.Lexema, asignacion = expresion, token = identificador };
               var lista = optionalAssignableIdentifiersListP();
               lista.Insert(0, enumId);
               return lista;
            }
            else 
            {
                var enumId = new EnumListNode() { archivo = archivo, identificador = identificador.Lexema, token = identificador };
                var listar = optionalAssignableIdentifiersListP();
                listar.Insert(0, enumId);
                return listar;
            }
            
           
        }

        private List<EnumListNode> optionalAssignableIdentifiersListP()
        {
            if (CurrentToken.Tipo == TokenTipos.Separador)
            {
                getNextToken();
                if (CurrentToken.Tipo == TokenTipos.Identificador)
                    return optionalAssignableIdentifiersList();
                else
                    throw new ParserException(archivo+"se esperaba un identificador" + CurrentToken.Columna + " " + CurrentToken.Fila);
            }
            else {
                return new List<EnumListNode>();
            }
        }
        private StatementNode interfaceDeclaration(Token encapsulamiento)
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaInterfaz)
            {
                var token = CurrentToken;
                getNextToken();
                var identificador = CurrentToken;
                if (CurrentToken.Tipo != TokenTipos.Identificador)
                {
                    throw new ParserException(archivo+"se esperaba un identificador" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
               var herencia = inheritanceBase();
                var interfaz = interfaceBody();
                optionalBodyEnd();
                return new InterfaceNode() { archivo = archivo, encasulamiento = encapsulamiento != null ? encapsulamiento.Lexema : "", nombre = identificador.Lexema, herencia = herencia, cuerpo = interfaz, token = token };
            }
            return null;
        }

        private List<InterzaceHeaderNode> interfaceBody()
        {
            if (CurrentToken.Tipo == TokenTipos.LlaveIzquierdo)
            {
                getNextToken();
               var metodos = interfaceMethodDeclarationList();
                if (CurrentToken.Tipo != TokenTipos.LlaveDerecho)
                {
                    throw new ParserException(archivo+"se esperaba una llave " + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                return metodos;
            }else
                throw new ParserException(archivo+"se esperaba una llave" + CurrentToken.Columna + " " + CurrentToken.Fila);  
        }

        private List<InterzaceHeaderNode> interfaceMethodDeclarationList()
        {
            if (CurrentToken.Tipo != TokenTipos.LlaveDerecho)
            {
               var metodo = interfaceMethodHeader();
                if (CurrentToken.Tipo != TokenTipos.FinalDeSentencia)
                {
                    throw new ParserException(archivo+"se esperaba un : ;" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
               var lista = interfaceMethodDeclarationList();
               lista.Insert(0, metodo);
               return lista;
            }else{
                return new List<InterzaceHeaderNode>();
            }
        }

        private InterzaceHeaderNode interfaceMethodHeader()
        {
            var token = CurrentToken;
           var tipo = typeOrVoid();
           var nombre = CurrentToken;
            if (CurrentToken.Tipo != TokenTipos.Identificador)
            {
                throw new ParserException(archivo+"se esperaba un : identificador" + CurrentToken.Columna + " " + CurrentToken.Fila);
            }
            getNextToken();
            if (CurrentToken.Tipo != TokenTipos.ParentesisIzquierdo)
            {
                throw new ParserException(archivo+"se esperaba un : ( " + CurrentToken.Columna + " " + CurrentToken.Fila);
            }
            getNextToken();
            var parametros = fixedParameters();
            if (CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
            {
                throw new ParserException(archivo+"se esperaba un : ( " + CurrentToken.Columna + " " + CurrentToken.Fila);
            }
            getNextToken();
            return new InterzaceHeaderNode() { archivo = archivo, tipo = tipo, nombre = nombre.Lexema, parametro = parametros, token = token };
        }

        private List<ParametrosNode> fixedParameters()
        {
            if (Type.Contains(CurrentToken.Tipo) || CurrentToken.Tipo == TokenTipos.Identificador)
            {
               var statement = fixedParameter();
               var lista = fixedParamatersP();
               lista.Insert(0, statement);
               return lista;
            }
            else {
                return new List<ParametrosNode>();
            }
            
        }

        private List<ParametrosNode> fixedParamatersP()
        {
            if (CurrentToken.Tipo == TokenTipos.Separador)
            {
                getNextToken();
                var statement = fixedParameter();
                var lista = fixedParamatersP();
                lista.Insert(0, statement);
                return lista;
            }else{
                return new List<ParametrosNode>();
            
            }
        }

        private ParametrosNode fixedParameter()
        {
            if (Type.Contains(CurrentToken.Tipo) || CurrentToken.Tipo == TokenTipos.Identificador)
            {
                var token = CurrentToken;
                var tipo = type();
                var nombre = CurrentToken;   
                if (CurrentToken.Tipo != TokenTipos.Identificador)
                    throw new ParserException(archivo+"se esperaba un identificador" + CurrentToken.Columna + " " + CurrentToken.Fila);
                getNextToken();
                return new ParametrosNode() { archivo = archivo, tipo = tipo, nombre = nombre.Lexema, token = token };
            }else
                throw new ParserException(archivo+"se esperaba un tipo de dato " + CurrentToken.Columna + " " + CurrentToken.Fila);
            
        }



        private DefinicionTipoNode typeOrVoid()
        {
            if(Type.Contains(CurrentToken.Tipo)||CurrentToken.Tipo ==  TokenTipos.Identificador){
               return type();
            }
            else if (CurrentToken.Tipo == TokenTipos.PalabraReservadaVoid)
            {
                var tipo = CurrentToken;
                getNextToken();
                return new DefinicionTipoNode() { archivo = archivo, tipounico = tipo.Lexema, token = tipo };
            }else
                throw new ParserException(archivo+"se espera un tipo de dato o la palabra void" + CurrentToken.Columna + " " + CurrentToken.Fila);
        }


        private StatementNode classDeclaration(Token encapsulamiento)
        {
            var token = CurrentToken;
             var modifier = classModifier();
            if (CurrentToken.Tipo != TokenTipos.PalabraReservadaClase)
            {
                throw new ParserException(archivo+"se espraba la palabra clase" + CurrentToken.Columna + " " + CurrentToken.Fila);
            }
            getNextToken();
            var identificador = CurrentToken;
            if (CurrentToken.Tipo != TokenTipos.Identificador)
            {
                throw new ParserException(archivo+"se espraba un identificador" + CurrentToken.Columna + " " + CurrentToken.Fila);
            }
            getNextToken();
            var herencia = inheritanceBase();
            var cuerpo = classBody();
            optionalBodyEnd();
            return new ClassNode() { archivo = archivo, encasulamiento = encapsulamiento != null ? encapsulamiento.Lexema : null, modificador = modifier != null ? modifier.Lexema : null, nombre = identificador.Lexema, herencia = herencia, cuerpo = cuerpo, token = token };
            
        }

        private void optionalBodyEnd()
        {
            if (CurrentToken.Tipo == TokenTipos.FinalDeSentencia)
            {
                getNextToken();
            }
            else { }
        }

        private List<StatementNode> classBody()
        {
            if (CurrentToken.Tipo == TokenTipos.LlaveIzquierdo)
            {
                getNextToken();
               var lista =  optionalClassMemberDeclarationList();
                if (CurrentToken.Tipo != TokenTipos.LlaveDerecho)
                {
                    throw new ParserException(archivo+"se esperaba una llave " + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                return lista;
            }else
                throw new ParserException(archivo+"se esperaba una llave" + CurrentToken.Columna + " " + CurrentToken.Fila);
        }
        private List<ExpressionNode> inheritanceBase()
        {
            if (CurrentToken.Tipo == TokenTipos.DosPuntos)
            {
                getNextToken();
                return identifiersList();
            }
            else {

                return new List<ExpressionNode>();
            }
        }

        private List<ExpressionNode> identifiersList()
        {
            if (CurrentToken.Tipo == TokenTipos.Identificador)
            {
                var token = CurrentToken;
                getNextToken();
                var identificador = new IdentificadoresExpressionNode() { archivo = archivo, nombre = token.Lexema, token = token };
               var lista = identifiersListP();
               lista.Insert(0, identificador);
               return lista;
            }
            else
                throw new ParserException(archivo+"se espera una claseo o interface herencia" + CurrentToken.Columna + " " + CurrentToken.Fila);
        }

        private List<ExpressionNode> identifiersListP()
        {
            if (CurrentToken.Tipo == TokenTipos.Separador)
            {
                getNextToken();
                var token = CurrentToken;
                if (CurrentToken.Tipo != TokenTipos.Identificador) {
                    throw new ParserException(archivo+"se esperaba un identificador" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var identificador = new IdentificadoresExpressionNode() { archivo = archivo, nombre = token.Lexema, token = token };
                var lista = identifiersListP();
                lista.Insert(0, identificador);
                return lista;
            }
            else {
                return new List<ExpressionNode>();
            }
        }

        private Token classModifier()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaAbstract)

            {
                var token = CurrentToken;
                getNextToken();
                return token;
            }
            else {
                return null;
            }
        }
        private List<StatementNode> optionalClassMemberDeclarationList()
        {
            
            if (CurrentToken.Tipo != TokenTipos.LlaveDerecho)
            {
               var declaracion = classMemberDeclaration();
               var lista = optionalClassMemberDeclarationList();
               lista.Insert(0, declaracion);
               return lista;
            }
            else {
                return new List<StatementNode>();
            }
            
        }
        
        private StatementNode classMemberDeclaration()
        {
            var token = encapsulationModifier();
           return classMemberDeclarationOptions(token);
        }

        private StatementNode classMemberDeclarationOptions(Token encapsulamiento)
        {
           
            if (CurrentToken.Tipo == TokenTipos.Identificador)
            {
                var identificador = CurrentToken;
                getNextToken();
                List<List<ExpressionNode>> array = new List<List<ExpressionNode>>();
                if (CurrentToken.Tipo == TokenTipos.CorcheteIzquierdo)
                {
                    array = Expression.optionalRankSpecifierList();
                  
                }
                    
                if (CurrentToken.Tipo == TokenTipos.ParentesisIzquierdo){
                    var constructor = constructorDeclarator(encapsulamiento, identificador);
                    constructor.constructorCuerpo = maybeEmptyBlock();
                    return constructor;
                }
                else if (CurrentToken.Tipo == TokenTipos.Identificador||CurrentToken.Tipo == TokenTipos.CorcheteIzquierdo)
                {
                    var nombre = CurrentToken;
                    getNextToken();
                    var metodoOvariable = fieldOrMethod(nombre);
                    metodoOvariable.archivo = archivo;
                    if (metodoOvariable is DeclaracionMetodo)
                    {
                        (metodoOvariable as DeclaracionMetodo).encapsulamiento = (encapsulamiento != null) ? encapsulamiento.Lexema : null;
                        (metodoOvariable as DeclaracionMetodo).tipo.tipounico = identificador.Lexema;
                        if (array.Count != 0)
                            (metodoOvariable as DeclaracionMetodo).tipo.array.AddRange(array);

                    }
                    else
                    {
                        (metodoOvariable as DeclaracionVariableNode).encapsulamiento = (encapsulamiento != null) ? encapsulamiento.Lexema : null; 
                        (metodoOvariable as DeclaracionVariableNode).tipo.tipounico = identificador.Lexema;
                        if(array.Count !=0)
                            (metodoOvariable as DeclaracionMetodo).tipo.array.AddRange(array);
                    }
                    metodoOvariable.token = nombre;
                    return metodoOvariable;
                }else
                    throw new ParserException(archivo+"se espera un identificador o un parentesis" + CurrentToken.Columna + " " + CurrentToken.Fila);    
            }
            else 
            {
                var token = CurrentToken;
                var modificar = optionalModifier();
                var tipo = typeOrVoid();
                var identificador = CurrentToken;
                if (CurrentToken.Tipo != TokenTipos.Identificador)
                {
                    throw new ParserException(archivo+"se esperaba un identificador" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var metodoOvariable = fieldOrMethod(identificador);
                if (metodoOvariable is DeclaracionMetodo)
                {
                    (metodoOvariable as DeclaracionMetodo).encapsulamiento =(encapsulamiento!=null)?encapsulamiento.Lexema:null;
                    (metodoOvariable as DeclaracionMetodo).tipo = tipo ;
                    (metodoOvariable as DeclaracionMetodo).modificar = (modificar != null) ? modificar.Lexema : null;
                }
                else
                {
                    (metodoOvariable as DeclaracionVariableNode).encapsulamiento = (encapsulamiento!=null)?encapsulamiento.Lexema:null;
                    (metodoOvariable as DeclaracionVariableNode).tipo = tipo;
                    (metodoOvariable as DeclaracionVariableNode).modificar = (modificar != null) ? modificar.Lexema : null;
                }
                metodoOvariable.token = token;
                return metodoOvariable;
            }
            
        }

       
        private StatementNode fieldOrMethod(Token identificador)
        {                    
            if(CurrentToken.Tipo == TokenTipos.ParentesisIzquierdo)
                return methodDeclaration(identificador);
            else if(CurrentToken.Tipo == TokenTipos.FinalDeSentencia || CurrentToken.Tipo == TokenTipos.Asignacion)
                return fieldDeclaration(identificador);
            else
                throw new ParserException(archivo+"se espera un funcion o un inizalisacion" + CurrentToken.Columna + " " + CurrentToken.Fila);
        }

        private DeclaracionMetodo methodDeclaration(Token identificador)
        {
            if (CurrentToken.Tipo == TokenTipos.ParentesisIzquierdo)
            {
                var token = CurrentToken;
                getNextToken();
                var parametros = fixedParameters();
                 if (CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
                 {
                     throw new ParserException(archivo+"se esperaba un parentesis " + CurrentToken.Columna + " " + CurrentToken.Fila);
                 }
                 getNextToken();
                var cuerpo = maybeEmptyBlock();
                return new DeclaracionMetodo() { archivo = archivo, cuerpo = cuerpo, nombre = identificador.Lexema, parametros = parametros, token = token };
             }
            return null;
        }

        private DeclaracionVariableNode fieldDeclaration(Token identificador)
        {
            var token = CurrentToken;
                var asignasion = variableAssigner();
                var declaracion = variableDeclaratorListP();
                
                if (CurrentToken.Tipo != TokenTipos.FinalDeSentencia)
                {
                    throw new ParserException(archivo+"se espera un ;" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                return new DeclaracionVariableNode() { archivo = archivo, identificador = identificador.Lexema, declarationList = declaracion, inicializacion = asignasion, token = token };
        }

        private List<StatementNode>  variableDeclaratorListP()
        {
            if (CurrentToken.Tipo == TokenTipos.Separador)
            {
                getNextToken();
               return variableDeclaratorList();
            } else{

                return new List<StatementNode>();
            } 
        }

        private List<StatementNode> variableDeclaratorList()
        {
            if (CurrentToken.Tipo == TokenTipos.Identificador)
            {
                var identificador = CurrentToken;
               
                getNextToken();
                var expresion = variableAssigner();
                var lista = variableDeclaratorListP();

                var declaracion = new DeclaracionVariableNode() { archivo = archivo, identificador = identificador.Lexema, inicializacion = expresion, token = identificador };
                lista.Insert(0, declaracion);
                return lista;
              
            }else
            throw new ParserException(archivo+"se espera un identificador"+CurrentToken.Columna+" "+CurrentToken.Fila);
        }

        private ExpressionNode variableAssigner()
        {
            if (AssignmentOperator.Contains(CurrentToken.Tipo))
            {
                if (CurrentToken.Tipo != TokenTipos.Asignacion)
                {
                    var assignment = Expression.assignmentOperator();
                    assignment.OperadorDerecho = Expression.variableInitializer();
                    return assignment;
                }
                getNextToken();
                return Expression.variableInitializer();
            }
            else {

                return null;
            }
        }      

        private Token optionalModifier()
        {
            if (OptionalModifier.Contains(CurrentToken.Tipo))
            {
                var token = CurrentToken;
                getNextToken();
                return token;
            }
            else { 
            return null;}
        }
        private  List<StatementNode> maybeEmptyBlock()
        {
            if (CurrentToken.Tipo == TokenTipos.LlaveIzquierdo)
            {
                getNextToken();
                var lista =  optionalStatementList();
                if (CurrentToken.Tipo != TokenTipos.LlaveDerecho)
                {
                    throw new ParserException(archivo+"se espera una llave" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                return lista;
            }
            else if (CurrentToken.Tipo == TokenTipos.FinalDeSentencia) {
                getNextToken();
                return null;
            }else
                throw new ParserException(archivo+"se esperaban unos llaves o punto y coma" + CurrentToken.Columna + " " + CurrentToken.Fila);
        }



        private ConstructorNode constructorDeclarator(Token encapsulamiento , Token identificador)
        {
            var token = CurrentToken;
                     if (CurrentToken.Tipo != TokenTipos.ParentesisIzquierdo)
                    {
                        throw new ParserException(archivo+"se esperaba unos parentesis :(" + CurrentToken.Columna + " " + CurrentToken.Fila);
                    }
                    getNextToken();
                   var parametros = fixedParameters();
                    if (CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
                    {
                        throw new ParserException(archivo+"se esperaba unos parentesis :(" + CurrentToken.Columna + " " + CurrentToken.Fila);
                    }
                    getNextToken();
                    var inicializacion = constructorInitializer();
                    return new ConstructorNode() { archivo = archivo ,parametro = parametros, identificador = identificador.Lexema, encapsulamiento = (encapsulamiento != null)?encapsulamiento.Lexema:""
                    ,baseParametos = inicializacion , token = token};
        }

        private List<ExpressionNode> constructorInitializer()
        {
            if (CurrentToken.Tipo == TokenTipos.DosPuntos)
            {
                getNextToken();
                if (CurrentToken.Tipo != TokenTipos.PalabraReservadaBase)
                {
                    throw new ParserException(archivo+"se esperban la palabra base" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                if (CurrentToken.Tipo != TokenTipos.ParentesisIzquierdo)
                {
                    throw new ParserException(archivo+"se esperban  ( " + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
               var argumentos = Expression.argumentList();
                if (CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
                {
                    throw new ParserException(archivo+"se esperban  ) " + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                return argumentos;
            }
            else {
                return new List<ExpressionNode>();
            }
            
        }

        
        private List<StatementNode> optionalStatementList()
        {
            if (CurrentToken.Tipo != TokenTipos.LlaveDerecho && CurrentToken.Tipo != TokenTipos.PalabraReservadaCase
                && CurrentToken.Tipo != TokenTipos.PalabraReservadaDefault)
                return statementList();
            else {
                return new List<StatementNode>();
            }
        }

        private List<StatementNode> statementList()
        {
            var statementvar = statement();
            var lista = optionalStatementList();
            lista.InsertRange(0, statementvar);
            return lista;
        }

        private List<StatementNode> statement()
        {
            if(Type.Contains(CurrentToken.Tipo) || CurrentToken.Tipo == TokenTipos.PalabraReservadaVar)
            {
                
                var statement = localVariableDeclaration();
                if (CurrentToken.Tipo != TokenTipos.FinalDeSentencia)
                {
                    throw new ParserException(archivo+"se esperaba un ; " + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var lista = new List<StatementNode>();
                lista.Add(statement);
                return lista;
            }
            else if (CurrentToken.Tipo != TokenTipos.PalabraReservadaCase || CurrentToken.Tipo != TokenTipos.PalabraReservadaDefault)
            {
               return embeddedStatement();
            }
            return null;
        }

        

        private StatementNode localVariableDeclaration()
        {
            var token = CurrentToken;
           var tipo = TypeOrVar();
            var declaration = variableDeclaratorList();
            return new DeclaracionVariableNode() { archivo = archivo, tipo = tipo, declarationList = declaration, token = token };
        }

        private DefinicionTipoNode TypeOrVar()
        {
            if (Type.Contains(CurrentToken.Tipo) || CurrentToken.Tipo == TokenTipos.Identificador)
            {
               return type();
            }
            else if (CurrentToken.Tipo == TokenTipos.PalabraReservadaVoid)
            {

                var definicion = new DefinicionTipoNode() { archivo = archivo, tipounico = CurrentToken.Lexema, token = CurrentToken };
                getNextToken();
                return definicion;
            }
            else
                throw new ParserException(archivo+"se espreba un tipo o la palabra void" + CurrentToken.Columna + " " + CurrentToken.Fila);

        }
        private List<StatementNode> embeddedStatement()
        {
            if (CurrentToken.Tipo == TokenTipos.FinalDeSentencia || CurrentToken.Tipo == TokenTipos.LlaveIzquierdo)
            {
               return maybeEmptyBlock();
            }
            else if (CurrentToken.Tipo == TokenTipos.PalabraReservadaIf || CurrentToken.Tipo == TokenTipos.PalabraReservadaSwitch)
            {
                return selectionStatement();
            }else if (CurrentToken.Tipo == TokenTipos.PalabraReservadaDo || CurrentToken.Tipo == TokenTipos.PalabraReservadaWhile||
                CurrentToken.Tipo == TokenTipos.PalabraReservadaFor || CurrentToken.Tipo == TokenTipos.PalabraReservadaForeach)
            {
                return iterationStatement();
            }else if(CurrentToken.Tipo == TokenTipos.PalabraReservadaReturn||CurrentToken.Tipo == TokenTipos.PalabraReservadaBreak
                     || CurrentToken.Tipo == TokenTipos.PalabraReservadaContinue)
            {
              var jump =  jumpStatement();
              var lista = new List<StatementNode>();
              lista.Add(jump);
                if (CurrentToken.Tipo != TokenTipos.FinalDeSentencia)
                {
                    throw new ParserException(archivo+"se esperaba un ; " + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                return lista;
            }
            else 
            {
                var lista = new List<StatementNode>();
              var statemet =  statementExpression();
           

                if (CurrentToken.Tipo != TokenTipos.FinalDeSentencia)
                {
                    throw new ParserException(archivo+"se esperaba un ; " + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                lista.Add(statemet);
                return lista;
            }
         
        }

        private StatementNode jumpStatement()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaContinue || CurrentToken.Tipo == TokenTipos.PalabraReservadaBreak)
            {
                var token = CurrentToken;
                getNextToken();
                if (CurrentToken.Tipo == TokenTipos.PalabraReservadaContinue)
                    return new ContinueNode() { archivo = archivo, token = token };
                else
                    return new BreakNode() { archivo = archivo, token = token };
            }
            else if (CurrentToken.Tipo == TokenTipos.PalabraReservadaReturn)
            {
                var token = CurrentToken;
                getNextToken();
               var expresion = optionalExpression();
               return new ReturnNode() { archivo = archivo, expresion = expresion, token = token };
            }
            return null;
        }

        private List<StatementNode> iterationStatement()
        {
            var lista = new List<StatementNode>();
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaWhile)
            {
                var statementiteration = whileStatement();
                lista.Add(statementiteration);
                return lista;
            }else if (CurrentToken.Tipo == TokenTipos.PalabraReservadaDo)
            {
                 var statementiteration = DoStatement();
                lista.Add(statementiteration);
                return lista;
               
            }
            else if (CurrentToken.Tipo == TokenTipos.PalabraReservadaFor)
            {
                var statementiteration = ForStatement();
                lista.Add(statementiteration);
                return lista;
           
            }
            
            else if (CurrentToken.Tipo == TokenTipos.PalabraReservadaForeach)
            {
   
                var statementiteration = ForeachStatement();
                lista.Add(statementiteration);
                return lista;
              
            }
            return null;

        }

        private StatementNode ForeachStatement()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaForeach)
            {
                var token = CurrentToken;
                getNextToken();
                if (CurrentToken.Tipo != TokenTipos.ParentesisIzquierdo)
                {
                    throw new ParserException(archivo+"se esperaba un parentesis" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var tipo = type();
                if (CurrentToken.Tipo != TokenTipos.Identificador)
                {
                    throw new ParserException(archivo+"se esperaba un identificador" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                var identificador = CurrentToken.Lexema;
                getNextToken();
                if (CurrentToken.Tipo != TokenTipos.PalabraReservadaIn)
                {
                    throw new ParserException(archivo+"se esperaba un la palabra in " + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var expresion = Expression.Expressions();
                if (CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
                {
                    throw new ParserException(archivo+"se esperaba un parentesis" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var cuerpo = embeddedStatement();
                return new ForeachNode() { archivo = archivo, cuerpo = cuerpo, tipo = tipo, identificador = identificador, expresion = expresion, token = token };   
            }
            return null;
        }

        private StatementNode DoStatement()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaDo)
            {
                var token = CurrentToken;
                getNextToken();
               var cuerpo = embeddedStatement();
                if (CurrentToken.Tipo != TokenTipos.PalabraReservadaWhile)
                {
                    throw new ParserException(archivo+"se esperaba la palabra while" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                if (CurrentToken.Tipo != TokenTipos.ParentesisIzquierdo)
                {
                    throw new ParserException(archivo+"se esperaba un parentesis" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var expresion = Expression.Expressions();
                if (CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
                {
                    throw new ParserException(archivo+"se esperaba un parentesis" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                if (CurrentToken.Tipo != TokenTipos.FinalDeSentencia)
                {
                    throw new ParserException(archivo+"se esperaba un ; " + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                return new doNode() { archivo = archivo, expresion = expresion, cuerpo = cuerpo, token = token };
            }
            else { 
                return null;
            }
        }

        private StatementNode ForStatement()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaFor)
            {
                var token = CurrentToken;
                getNextToken();
                if (CurrentToken.Tipo != TokenTipos.ParentesisIzquierdo)
                {
                    throw new ParserException(archivo+"se esperaba un parentesis" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var expresion1 = optionalForInitializer();
                if (CurrentToken.Tipo != TokenTipos.FinalDeSentencia)
                {
                    throw new ParserException(archivo+"se esperaba un ; " + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var expresion2 = optionalExpression();
               
                if (CurrentToken.Tipo != TokenTipos.FinalDeSentencia)
                {
                    throw new ParserException(archivo+"se esperaba un ; " + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var expresion3 = optionalStatementExpressionList();
                if (CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
                {
                    throw new ParserException(archivo+"se esperaba un parentesis" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
               var cuerpo = embeddedStatement();
               return new ForNode() { archivo = archivo, cuerpo = cuerpo, expresionInicio = expresion1, expresionCondicional = expresion2, expresionFinal = expresion3, token = token };
            }
                return null;

        }

        private List<StatementNode> optionalStatementExpressionList()
        {
            if (CurrentToken.Tipo != TokenTipos.FinalDeSentencia||CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
            {
                return statementExpressionList();
            }
            else {
                return null;
            }
        }

        private ExpressionNode optionalExpression()
        {
            if (CurrentToken.Tipo != TokenTipos.FinalDeSentencia || CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
            {
                return Expression.Expressions();
            }
            else {
                return null;
            }
        }

        private List<StatementNode> optionalForInitializer()
        {
            if (Type.Contains(CurrentToken.Tipo) || CurrentToken.Tipo == TokenTipos.PalabraReservadaVar || CurrentToken.Tipo == TokenTipos.Identificador) {
                var identificador = CurrentToken;
                var tipo = type();
            
                if (CurrentToken.Tipo == TokenTipos.Identificador)
                {

                    var lista = variableDeclaratorList();
                    foreach (var list in lista)
                    {
                        (list as DeclaracionVariableNode).tipo = tipo;
                    }
                    return lista;
                }
                else if(CurrentToken.Tipo == TokenTipos.Asignacion)
                {
                    var list = new List<StatementNode>();
                   var asignacion = variableAssigner();
                   var declaracion = new DeclaracionVariableNode() { archivo = archivo, identificador = identificador.Lexema, inicializacion = asignacion, token = identificador };
                   list.Add(declaracion);
                   return list;
                }
            }
            else if (CurrentToken.Tipo != TokenTipos.FinalDeSentencia)
            {
               return statementExpressionList();
            }
            else {
                return new List<StatementNode>();
            }
            return null;
        }

        private List<StatementNode> statementExpressionList()
        {
           var expresion = statementExpression();
           var lista = statementExpressionListP();
           lista.Add(expresion);
           return lista;
        }

        private List<StatementNode> statementExpressionListP()
        {
            if (CurrentToken.Tipo == TokenTipos.Separador)
            {
                getNextToken();
                var expresion = statementExpression();
                var lista = statementExpressionListP();
                lista.Add(expresion);
                return lista;
            }
            else {

                return new List<StatementNode>() ;
            }
        }

        private StatementNode statementExpression()
        {
           if (CurrentToken.Tipo == TokenTipos.Identificador)
            {
                var tipoPosible = CurrentToken;
                getNextToken();
                List<List<ExpressionNode>> array = new List<List<ExpressionNode>>();
                if (CurrentToken.Tipo == TokenTipos.CorcheteIzquierdo)
                {
                    getNextToken();
                    if (CurrentToken.Tipo == TokenTipos.Separador)
                    {

                        array = Expression.optionalRankSpecifierList();
                    }
                    else { 
                        lexico._cursor=lexico._cursor-2;
                        getNextToken();
                    }
                    
                }
                if (CurrentToken.Tipo == TokenTipos.Identificador)
                {
                    var identificador = CurrentToken;
                    getNextToken();
                    var tipo = new DefinicionTipoNode() { archivo = archivo, tipounico = tipoPosible.Lexema, token = tipoPosible, array = array };
                    var espresion = variableAssigner();
                    var lista = variableDeclaratorListP();
                    return new DeclaracionVariableNode() { archivo = archivo, tipo = tipo, identificador = identificador.Lexema, inicializacion = espresion, declarationList = lista, token = tipoPosible };
                }

                var id = Expression.optionalFunctOrArrayCall(tipoPosible.Lexema);
                var identificadorT = Expression.primaryExpressionsP(id);

                if (identificadorT is UnaryOperatorNode)
                    return new UnaryStatemetNode() { archivo = archivo, expresion = identificadorT as UnaryOperatorNode, token = tipoPosible };
                else if (identificadorT is CallFuntionNode)
                    return new CallFuncionStatementNode()
                    {
                        archivo = archivo,
                        nombre = (id as CallFuntionNode).nombre,
                        ListaDeAccesores = (id as CallFuntionNode).ListaDeAccesores,
                        parametros = (id as CallFuntionNode).parametros,
                        token = tipoPosible
                    };
                ExpressionNode asignasion = null; 
                var listaExpresion = new List<ExpressionNode>();
                if (AssignmentOperator.Contains(CurrentToken.Tipo)){
                         asignasion = variableAssigner();
                         return new AssignasionVariable() { archivo = archivo, identificador = identificadorT as IdentificadoresExpressionNode, expresion = asignasion, token = tipoPosible };
                }
                else    if((identificadorT as IdentificadoresExpressionNode).ListaDeAccesores.Count == 0) 
                {
                  listaExpresion = statementExpressionP();
                  return new CallFuncionStatementNode()
                  {
                      archivo = archivo,
                      nombre = (identificadorT as IdentificadoresExpressionNode).nombre,
                      ListaDeAccesores = (identificadorT as IdentificadoresExpressionNode).ListaDeAccesores,
                      parametros = listaExpresion,
                      token = tipoPosible
                  };
                }
                else
                {
                    return new IdentificadoresStatementNode()
                    {
                        archivo = archivo,
                        nombre = (identificadorT as IdentificadoresExpressionNode).nombre,
                        ListaDeAccesores = (identificadorT as IdentificadoresExpressionNode).ListaDeAccesores
                    };
                }  
                
            }
           else
           {
               var token = CurrentToken;
               if (CurrentToken.Tipo == TokenTipos.AutoOperacionDecremento || CurrentToken.Tipo == TokenTipos.AutoOperacionIncremento)
               {
                    var unary = optionalUnaryExpression();
                    if (unary is UnaryOperatorNode)
                    {
                        return new UnaryStatemetNode() { archivo = archivo, expresion = unary as UnaryOperatorNode, token = token };
                    }
               } 
               else if (CurrentToken.Tipo == TokenTipos.PalabraReservadaThis){
                   getNextToken();
                   getNextToken();
                   var id2 = CurrentToken;
                   if (CurrentToken.Tipo != TokenTipos.Identificador)
                   {
                       throw new ParserException("se esperaba un identificador fila" + CurrentToken.Fila + "columana" + CurrentToken.Columna);
                   }
                   getNextToken();
                   var id = Expression.optionalFunctOrArrayCall(id2.Lexema);
                   var identificadorT = Expression.primaryExpressionsP(id);
                 
                   ExpressionNode asignasion = null;
                   if (AssignmentOperator.Contains(CurrentToken.Tipo))
                   {

                      asignasion = variableAssigner();
                   }
                   return new ThisStatementNode() { expresion = new ThisNode() { expresion = identificadorT }, Asignacion = asignasion };
               }
               else if (CurrentToken.Tipo == TokenTipos.ParentesisIzquierdo) {
                   var expresion = Expression.primaryExpressions();
                   if (!(expresion is AsNode))
                   {
                       throw new ParserException(archivo + "expresion no aceptada " + CurrentToken.Columna + " " + CurrentToken.Fila);
                   }
                   ExpressionNode asignasion = null;
                   var listaExpresion = new List<ExpressionNode>();
                   if (AssignmentOperator.Contains(CurrentToken.Tipo)){
                       asignasion = variableAssigner();
                       return new AsStatemetNode() { asVariable = expresion as AsNode, archivo = archivo, Asignasion = asignasion };
                   }   
                   else if (CurrentToken.Tipo == TokenTipos.AutoOperacionDecremento || CurrentToken.Tipo == TokenTipos.AutoOperacionIncremento)
                    {
                       
                        getNextToken();
                        var unary = new UnaryOperatorNode();
                        if (CurrentToken.Tipo == TokenTipos.AutoOperacionDecremento)
                            unary = new UnaryAutoOperacionDecrementoNode() { archivo = archivo, token = token , Operando = expresion };
                        else
                            unary = new UnaryAutoOperacionIncrementoNode() { archivo = archivo, token = token, Operando = expresion };

                        return new UnaryStatemetNode() { archivo = archivo, expresion = unary as UnaryOperatorNode, token = token };
                    }

                   return new AsStatemetNode() { asVariable = expresion as AsNode, archivo = archivo };
               }
               else
               {
                   throw new SemanticoException("statement no aceptado" + token.Fila + "columna" + token.Columna);
               } 
             
              
            }
           return null;
           }
        private List<ExpressionNode> statementExpressionP()
        {
            if (CurrentToken.Tipo == TokenTipos.ParentesisIzquierdo)
            {
                getNextToken();
               var argumentos = Expression.argumentList();
                if (CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
                {
                    throw new ParserException(archivo+"se esperaba un parentesis" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
            }
            else if (CurrentToken.Tipo == TokenTipos.AutoOperacionDecremento || CurrentToken.Tipo == TokenTipos.AutoOperacionIncremento)
            {
                var token = CurrentToken;
                getNextToken();
                var unary = new UnaryOperatorNode();
                if (CurrentToken.Tipo == TokenTipos.AutoOperacionDecremento)
                    unary = new UnaryAutoOperacionDecrementoNode() { archivo = archivo, token = token };
                else
                    unary = new UnaryAutoOperacionIncrementoNode() { archivo = archivo, token = token };
                var lista = new List<ExpressionNode>();
                lista.Add(unary);
                return lista;
            }
            else {
                return new List<ExpressionNode>();
            }
            return null;
        }

       

        private ExpressionNode optionalUnaryExpression()
        {
            if (UnaryOperator.Contains(CurrentToken.Tipo))
            {
               var operador = Expression.ExpressionsUnaryOperator();
                operador.Operando = Expression.unaryExpressions();
                return operador; 
            }
            else if (CurrentToken.Tipo == TokenTipos.ParentesisIzquierdo)
            {
                var token = CurrentToken;
                getNextToken();
               var tipo = type();
                if (CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
                {
                    throw new ParserException(archivo+"se esperaba un parentesis" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                return new CastNode() { archivo = archivo, tipo = tipo.tipounico, token = token };

            }

            else {
                return null;
            }
        }

        private StatementNode whileStatement()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaWhile)
            {
                var token = CurrentToken;
                getNextToken();
                if (CurrentToken.Tipo != TokenTipos.ParentesisIzquierdo)
                {
                    throw new ParserException(archivo+"se esperaba un parentesis" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
               var expresion = Expression.Expressions();
              
                if (CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
                {
                    throw new ParserException(archivo+"se esperaba un parentesis" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var cuerpo = embeddedStatement();
                return new WhileNode() { archivo = archivo, cuerpo = cuerpo, expresion = expresion, token = token };
            }
            return null;
        }

        private List<StatementNode> selectionStatement()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaIf)
            {
               var lista = new List<StatementNode>();
               lista.Add(ifStatement());
               return lista;
            }
            else if (CurrentToken.Tipo == TokenTipos.PalabraReservadaSwitch)
            {
                var lista = new List<StatementNode>();
                lista.Add(switchStatement());
                return lista;
            }
            return null;
        }

        private StatementNode switchStatement()
        {
            if(CurrentToken.Tipo == TokenTipos.PalabraReservadaSwitch){
                var token = CurrentToken;
                getNextToken();
                if (CurrentToken.Tipo != TokenTipos.ParentesisIzquierdo)
                {
                    throw new ParserException(archivo+"se esperaba un parentesis" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
               var expresion = Expression.Expressions();
                if (CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
                {
                    throw new ParserException(archivo+"se esperaba un parentesis" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                if (CurrentToken.Tipo != TokenTipos.LlaveIzquierdo)
                {
                    throw new ParserException(archivo+"se esperaba un llave" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
               var casos = optionalSwitchSectionList();
                if (CurrentToken.Tipo != TokenTipos.LlaveDerecho)
                {
                    throw new ParserException(archivo+"se esperaba un llave" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                return new SwitchNode() { archivo = archivo, expresion = expresion, casos = casos, token = token };
            }
            return null;
        }

        private List<CasosNode> optionalSwitchSectionList()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaCase || CurrentToken.Tipo == TokenTipos.PalabraReservadaDefault)
            {
               var casos = switchLabelList();
               var cuerpo = statementList();
                casos.Last().cuerpo = cuerpo;
               var lista = optionalSwitchSectionList();
               lista.InsertRange(0, casos);
               return lista;
            }
            else {
                return new List<CasosNode>();
            }
        }

        private List<CasosNode> switchLabelList()
        {
           var caso = switchLabel();
           var lista = switchLabelListP();
           lista.Insert(0, caso);
           return lista;
        }

        private List<CasosNode> switchLabelListP()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaCase || CurrentToken.Tipo == TokenTipos.PalabraReservadaDefault)
            {
                return switchLabelList();
            }
            else {
                return new List<CasosNode>();
            }
        }

        private CasosNode switchLabel()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaCase)
            {
                var token = CurrentToken;
                getNextToken();
              var expresion =  Expression.Expressions();
                if (CurrentToken.Tipo != TokenTipos.DosPuntos)
                {
                    throw new ParserException(archivo+"se esperaba : " + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                return new CasosNode() { archivo = archivo, expresion = expresion, token = token };
            }
            else if (CurrentToken.Tipo == TokenTipos.PalabraReservadaDefault)
            {
                var token = CurrentToken;
                getNextToken();
                if (CurrentToken.Tipo != TokenTipos.DosPuntos)
                {
                    throw new ParserException(archivo+"se esperaba : " + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                return new DefaulNode() { archivo = archivo, token = token };
            } return null;
        }

        private StatementNode ifStatement()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaIf)
            {
                var token = CurrentToken;
                getNextToken();
                if (CurrentToken.Tipo != TokenTipos.ParentesisIzquierdo)
                {
                    throw new ParserException(archivo+"se esperaba un parentesis" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var expresion = Expression.Expressions();
                if (CurrentToken.Tipo != TokenTipos.ParentesisDerecho)
                {
                    throw new ParserException(archivo+"se esperaba un parentesis" + CurrentToken.Columna + " " + CurrentToken.Fila);
                }
                getNextToken();
                var cuerpo = embeddedStatement();
               var elsevar =  optionalElsePart();
               return new IfNode() { archivo = archivo, expresion = expresion, cuerpo = cuerpo, elseVariable = elsevar, token = token };
            }
            return null;
            
        }

        private ElseNode optionalElsePart()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaElse)
            {
                return elsePart();
            }
            else {
                return null;
            }
        }

        private  ElseNode elsePart()
        {
            if (CurrentToken.Tipo == TokenTipos.PalabraReservadaElse)
            {
                var token = CurrentToken;
                getNextToken();
               var cuerpo = embeddedStatement();
               return new ElseNode() { archivo = archivo, cuerpo = cuerpo, token = token };
            }
            return null;
        }
    }
    
}