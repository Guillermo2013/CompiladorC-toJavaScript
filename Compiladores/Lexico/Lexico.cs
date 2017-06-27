using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Compiladores
{
    public class Lexico
    {
        private string _codigoFuente;
        private int _columnaActual;
        private int _FilaActual;
        public int _cursor;
        private Dictionary<string, TokenTipos> _palabrasReservadas;
        private Dictionary<string, TokenTipos> simbolosUnarios;
        private Dictionary<string, TokenTipos> simbolosBinarios;
        private Dictionary<string, TokenTipos> simbolosTrinarios;
        public Lexico(string _codigoFuente)
        {
            // TODO: Complete member initialization
            this._codigoFuente = _codigoFuente;
            this._columnaActual = 1;
            this._FilaActual = 1;
            this._cursor = 0;
             this._palabrasReservadas = new Dictionary<string,TokenTipos>();
            this.simbolosUnarios = new Dictionary<string, TokenTipos>();
            this.simbolosBinarios = new Dictionary<string, TokenTipos>();
            this.simbolosTrinarios = new Dictionary<string, TokenTipos>();
            simbolosUnarios.Add("=", TokenTipos.Asignacion);
            simbolosUnarios.Add(";", TokenTipos.FinalDeSentencia);
            simbolosUnarios.Add(",", TokenTipos.Separador);
            simbolosUnarios.Add("!", TokenTipos.Negacion);
            simbolosUnarios.Add("&", TokenTipos.YPorBit);
            simbolosUnarios.Add("|", TokenTipos.OPorBit);
            simbolosUnarios.Add("~", TokenTipos.NegacionPorBit);
            simbolosUnarios.Add("^", TokenTipos.OExclusivoPorBit);
            simbolosUnarios.Add("(", TokenTipos.ParentesisIzquierdo);
            simbolosUnarios.Add(")", TokenTipos.ParentesisDerecho);
            simbolosUnarios.Add("{", TokenTipos.LlaveIzquierdo);
            simbolosUnarios.Add("}", TokenTipos.LlaveDerecho);
            simbolosUnarios.Add("[", TokenTipos.CorcheteIzquierdo);
            simbolosUnarios.Add("]", TokenTipos.CorcheteDerecho);
            simbolosUnarios.Add("#", TokenTipos.Directiva);
            simbolosUnarios.Add(".", TokenTipos.Punto);
            simbolosUnarios.Add(":", TokenTipos.DosPuntos);
            simbolosUnarios.Add("*", TokenTipos.OperacionMultiplicacion);
            simbolosUnarios.Add("+", TokenTipos.OperacionSuma);
            simbolosUnarios.Add("-", TokenTipos.OperacionResta);
            simbolosUnarios.Add("/", TokenTipos.OperacionDivision);
            simbolosUnarios.Add("%", TokenTipos.OperacionDivisionResiduo);
            simbolosUnarios.Add("<", TokenTipos.RelacionalMenor);
            simbolosUnarios.Add(">", TokenTipos.RelacionalMayor);
            simbolosUnarios.Add("?", TokenTipos.Interogacion);
            simbolosBinarios.Add("+=", TokenTipos.AutoOperacionSuma);
            simbolosBinarios.Add("-=", TokenTipos.AutoOperacionResta);
            simbolosBinarios.Add("*=", TokenTipos.AutoOperacionMultiplicacion);
            simbolosBinarios.Add("/=", TokenTipos.AutoOperacionDivision);
            simbolosBinarios.Add("%=", TokenTipos.AutoOperacionResiduo);
            simbolosBinarios.Add("&=", TokenTipos.AutoOperacionAndPorBit);
            simbolosBinarios.Add("|=", TokenTipos.AutoOperacionOrPorBit);
            simbolosBinarios.Add("^=", TokenTipos.AutoOperacionOExclusivoPorBit);
            simbolosBinarios.Add("++", TokenTipos.AutoOperacionIncremento);
            simbolosBinarios.Add("--", TokenTipos.AutoOperacionDecremento);
            simbolosBinarios.Add(">=", TokenTipos.RelacionalMayorOIgual);
            simbolosBinarios.Add("<=", TokenTipos.RelacionalMenorOIgual);
            simbolosBinarios.Add("||", TokenTipos.LogicosO);
            simbolosBinarios.Add("==", TokenTipos.RelacionalIgual);
            simbolosBinarios.Add("!=", TokenTipos.RelacionalNoIgual);
            simbolosBinarios.Add("&&", TokenTipos.LogicosY);
            simbolosBinarios.Add("<<", TokenTipos.DeplazamientoIzquierda);
            simbolosBinarios.Add(">>", TokenTipos.DeplazamientoDerecha);
            simbolosBinarios.Add("??", TokenTipos.NoEsNull);
            simbolosBinarios.Add("?.", TokenTipos.MiembroCondicionales);
            simbolosTrinarios.Add("<<=", TokenTipos.AutoDeplazamientoIzquierda);
            simbolosTrinarios.Add(">>=", TokenTipos.AutoDeplazamientoDerecha);
            _palabrasReservadas.Add("abstract", TokenTipos.PalabraReservadaAbstract);
            _palabrasReservadas.Add("as", TokenTipos.PalabraReservadaAs);
            _palabrasReservadas.Add("base", TokenTipos.PalabraReservadaBase);
            _palabrasReservadas.Add("bool", TokenTipos.PalabraReservadaBool);
            _palabrasReservadas.Add("break", TokenTipos.PalabraReservadaBreak);
            _palabrasReservadas.Add("byte", TokenTipos.PalabraReservadaByte);
            _palabrasReservadas.Add("case", TokenTipos.PalabraReservadaCase);
            _palabrasReservadas.Add("char", TokenTipos.PalabraReservadaChar);
            _palabrasReservadas.Add("class", TokenTipos.PalabraReservadaClase);
            _palabrasReservadas.Add("continue", TokenTipos.PalabraReservadaContinue);
            _palabrasReservadas.Add("decimal", TokenTipos.PalabraReservadaDecimal);
            _palabrasReservadas.Add("default", TokenTipos.PalabraReservadaDefault);
            _palabrasReservadas.Add("do", TokenTipos.PalabraReservadaDo);
            _palabrasReservadas.Add("else", TokenTipos.PalabraReservadaElse);
            _palabrasReservadas.Add("enum", TokenTipos.PalabraReservadaEnum);
            _palabrasReservadas.Add("false", TokenTipos.PalabraReservadaFalse);
            _palabrasReservadas.Add("float", TokenTipos.PalabraReservadaFloat);
            _palabrasReservadas.Add("for", TokenTipos.PalabraReservadaFor);
            _palabrasReservadas.Add("foreach", TokenTipos.PalabraReservadaForeach);
            _palabrasReservadas.Add("if", TokenTipos.PalabraReservadaIf);
            _palabrasReservadas.Add("in", TokenTipos.PalabraReservadaIn);
            _palabrasReservadas.Add("int", TokenTipos.PalabraReservadaInt);
            _palabrasReservadas.Add("interface", TokenTipos.PalabraReservadaInterfaz);
            _palabrasReservadas.Add("is", TokenTipos.PalabraReservadaIs);
            _palabrasReservadas.Add("new", TokenTipos.PalabraReservadaNew);
            _palabrasReservadas.Add("null", TokenTipos.PalabraReservadaNull);
            _palabrasReservadas.Add("override", TokenTipos.PalabraReservadaOverride);
            _palabrasReservadas.Add("private", TokenTipos.PalabraReservadaPrivate);
            _palabrasReservadas.Add("protected", TokenTipos.PalabraReservadaProtected);
            _palabrasReservadas.Add("public", TokenTipos.PalabraReservadaPublic);
            _palabrasReservadas.Add("ref", TokenTipos.PalabraReservadaRef);
            _palabrasReservadas.Add("return", TokenTipos.PalabraReservadaReturn);
            _palabrasReservadas.Add("static", TokenTipos.PalabraReservadaStatic);
            _palabrasReservadas.Add("string", TokenTipos.PalabraReservadaString);
            _palabrasReservadas.Add("struct", TokenTipos.PalabraReservadaStruct);
            _palabrasReservadas.Add("switch", TokenTipos.PalabraReservadaSwitch);
            _palabrasReservadas.Add("this", TokenTipos.PalabraReservadaThis);
            _palabrasReservadas.Add("true", TokenTipos.PalabraReservadaTrue);
            _palabrasReservadas.Add("using", TokenTipos.PalabraReservadaUsing);
            _palabrasReservadas.Add("void", TokenTipos.PalabraReservadaVoid);
            _palabrasReservadas.Add("virtual", TokenTipos.PalabraReservadaVirtual);
            _palabrasReservadas.Add("var", TokenTipos.PalabraReservadaVar);
            _palabrasReservadas.Add("volatile", TokenTipos.PalabraReservadaVolatile);
            _palabrasReservadas.Add("while", TokenTipos.PalabraReservadaWhile);
            _palabrasReservadas.Add("namespace", TokenTipos.PalabraReservadaNamespace);
            }
        
        public Token ObtenerSiguienteToken()
        {   
            char simboloTemporal = ObtenerSimboloActual();
            string lexema = "";
            while (char.IsWhiteSpace(simboloTemporal) )
            {
                _cursor++;
                _columnaActual++;
                if (simboloTemporal == '\n')
                {
                    _FilaActual++;
                    _columnaActual = 1;
                }
                simboloTemporal = ObtenerSimboloActual();
            }
            var colummnaT = _columnaActual;
            if (simboloTemporal == '\0')
            {
                return new Token { Tipo = TokenTipos.EndOfFile };
            }
            if (char.IsLetter(simboloTemporal)||simboloTemporal=='_')
            {
                lexema += simboloTemporal;
                _cursor++;
                return ObtenerIdentificador(lexema);
            }
            if (simbolosUnarios.ContainsKey(simboloTemporal.ToString()))
            {
                lexema += simboloTemporal;
                _cursor++;
                simboloTemporal = ObtenerSimboloActual();
                if (lexema[0] == '/' && simboloTemporal == '/')
                {
                    ObtenerTextoUnaLinea('/');
                    return ObtenerSiguienteToken();
                }
                if (lexema[0] == '/' && simboloTemporal == '*')
                {
                    var texto = ObtenerComentarioBloque('*', '/');
                    simboloTemporal = ObtenerSimboloActual();
                    if (simboloTemporal == '\0')
                    {
                        throw new LexicoException("Símbolo inesperado encontrado");
                    }
                    if (_codigoFuente[_cursor++] == '*' && _codigoFuente[_cursor++] == '/')
                    {
                        return ObtenerSiguienteToken();
                    }
                }
                Token TokenDeSimbolosTrinario = obtenerTokenDeDiccionario(simbolosTrinarios, lexema + simboloTemporal +_codigoFuente[_cursor+1]);
                if (TokenDeSimbolosTrinario != null) {
                    _columnaActual += TokenDeSimbolosTrinario.Lexema.Length;    
                    return TokenDeSimbolosTrinario;
                }
                Token TokenDeSimbolosBinarios = obtenerTokenDeDiccionario(simbolosBinarios, lexema + simboloTemporal);
                if (TokenDeSimbolosBinarios != null)
                {
                    _columnaActual += TokenDeSimbolosBinarios.Lexema.Length;
                    return TokenDeSimbolosBinarios;
                }
                if (simbolosUnarios.ContainsKey(lexema))
                    return new Token { Tipo = simbolosUnarios[lexema], Lexema = lexema, Columna = _columnaActual++, Fila = _FilaActual };
            }
            if (simboloTemporal == '"')
            {
                lexema += ObtenerTextoUnaLinea('"');
                _cursor++;
                if (lexema != "")
                {
                    _columnaActual += lexema.Length;
                    return new Token { Tipo = TokenTipos.LiteralString, Lexema = lexema, Columna = colummnaT, Fila = _FilaActual };
                }
            }
            if (simboloTemporal == '@' && _codigoFuente[_cursor+1]== '"')
            {
                _cursor++;
                lexema += ObtenerTextoMultiplesLineas('"');
                _cursor++;
                simboloTemporal = ObtenerSimboloActual();
                var filaString = _FilaActual;
                if (simboloTemporal == '\0')
                    {
                        throw new LexicoException("Símbolo inesperado encontrado");
                    }
                if (lexema != "")
                    return new Token { Tipo = TokenTipos.LiteralString, Lexema = "@"+'\"' + lexema + '\"', Columna = colummnaT, Fila = filaString };
            }
            if (simboloTemporal == '\'')
            {
                lexema += ObtenerTextoUnaLinea('\'');
                _cursor++;
                simboloTemporal = ObtenerSimboloActual();
                if (simboloTemporal == '\'')
                {
                    lexema += simboloTemporal;
                    _cursor++;
                }
                if (lexema.Length == 1)
                {
                    _columnaActual++;
                    return new Token { Tipo = TokenTipos.LiteralChar, Lexema =  lexema , Columna = colummnaT, Fila = _FilaActual };
                } if (lexema.Length > 1 && lexema.Contains('\\'))
                {
                    simboloTemporal = ObtenerSimboloActual();
                    if (lexema == "\\a" || lexema == "\\b" || lexema == "\\f" || lexema == "\\n" || lexema == "\\r" || lexema == "\\t" || lexema == "\\v" || lexema == "\\'" || lexema == "\\\"" ||
                        lexema == "\\\\" || lexema == "\\?" || lexema == "\\0")
                    {
                        _cursor++;
                        _columnaActual += lexema.Length;
                        return new Token { Tipo = TokenTipos.LiteralChar, Lexema =  lexema , Columna = colummnaT, Fila = _FilaActual };
                    }   
                }
            }
            if (char.IsDigit(simboloTemporal))
            {
                lexema += simboloTemporal;
                _cursor++;
                if (simboloTemporal == '0' && ObtenerSimboloActual() == 'x')
                {
                    lexema += ObtenerSimboloActual();
                    Token Hexagecimal = ObtenerDigitoHexagecimal(lexema);
                    _columnaActual += Hexagecimal.Lexema.Length;
                    if (Hexagecimal != null)
                        return Hexagecimal;
                    throw new LexicoException("Símbolo inesperado encontrado    ");
                }
                if (simboloTemporal == '0' && ObtenerSimboloActual() == 'b')
                {
                    lexema += ObtenerSimboloActual();
                    Token Binario = ObtenerDigitoBinario(lexema, colummnaT);
                    if (Binario != null)
                    {
                        _columnaActual += Binario.Lexema.Length;
                        return Binario;
                    }
                    throw new LexicoException("Símbolo inesperado encontrado");
                }
                Token numeroEnteroToken = ObtenerDigito(lexema);
                simboloTemporal = ObtenerSimboloActual();
                if (simboloTemporal == '.')
                {
                    lexema += simboloTemporal;
                    _cursor++;
                    numeroEnteroToken.Lexema += simboloTemporal;
                    lexema = "";
                    Token numeroDecimalToken = ObtenerDigito(lexema);
                    if (numeroDecimalToken.Lexema != "")
                    {
                        numeroEnteroToken.Lexema += numeroDecimalToken.Lexema;
                        simboloTemporal = ObtenerSimboloActual();
                 
                        if (char.ToUpper(simboloTemporal) == 'F')
                        {
                            numeroEnteroToken.Lexema += simboloTemporal;
                            _cursor++;
                            return new Token { Tipo = TokenTipos.NumeroFloat, Lexema = numeroEnteroToken.Lexema, Columna = colummnaT, Fila = _FilaActual };
                        }
                    }
                    throw new LexicoException("Símbolo inesperado encontrado");
                }
                
                else
                {
                    _columnaActual += numeroEnteroToken.Lexema.Length;
                    if (char.ToUpper(simboloTemporal) == 'F')
                    {
                        numeroEnteroToken.Lexema += simboloTemporal;
                        _cursor++;
                        _columnaActual++;
                        return new Token { Tipo = TokenTipos.NumeroFloat, Lexema = numeroEnteroToken.Lexema, Columna = colummnaT, Fila = _FilaActual };
                    }
                    else
                    {
                        return new Token { Tipo = TokenTipos.Numero, Lexema = numeroEnteroToken.Lexema, Columna = colummnaT, Fila = _FilaActual };
                    }
                }
            }  
            throw new LexicoException("Símbolo inesperado encontrado ");   
        }
        private Token obtenerTokenDeDiccionario(Dictionary<string, TokenTipos> _Diccionario, string _simbolo)
        {
            if (_Diccionario.ContainsKey(_simbolo))
            {
                _cursor = _cursor + _simbolo.Length - 1;
                return new Token { Tipo = _Diccionario[_simbolo], Lexema = _simbolo, Columna = _columnaActual, Fila = _FilaActual };
            }
            return null;
        }
        private Token ObtenerDigitoBinario(string lexema, int colummnaT)
        {
            _cursor++;
            var simboloTemporal = ObtenerSimboloActual();
            while (simboloTemporal == '0' || simboloTemporal == '1' || simboloTemporal == '_' )
            {
                if (simboloTemporal == '_')
                    if (_codigoFuente[_cursor + 1] != 0 )
                        if (_codigoFuente[_cursor + 1] != 1)
                            if (char.IsWhiteSpace(_codigoFuente[_cursor + 1]))
                                if (_codigoFuente[_cursor + 1] == '_')
                                    return null;
                            
                lexema += simboloTemporal;
                _cursor++;
                simboloTemporal = ObtenerSimboloActual();
            }
            return new Token { Tipo = TokenTipos.NumeroBinario, Lexema = lexema, Columna = colummnaT, Fila = _FilaActual };
        }
        private Token ObtenerDigitoHexagecimal(string lexema)
        {
            _cursor++;
            var simboloTemporal = ObtenerSimboloActual();
            while (char.IsDigit(simboloTemporal) || char.ToUpper(simboloTemporal) == 'A' || char.ToUpper(simboloTemporal) == 'B'
                || char.ToUpper(simboloTemporal) == 'C' || char.ToUpper(simboloTemporal) == 'D' || char.ToUpper(simboloTemporal) == 'E' || char.ToUpper(simboloTemporal) == 'F')
            {
                lexema += simboloTemporal;
                _cursor++;
                simboloTemporal = ObtenerSimboloActual();
            }
            return new Token { Tipo = TokenTipos.NumeroHexagecimal, Lexema = lexema, Columna = _columnaActual, Fila = _FilaActual };          
        }
        private Token ObtenerDigito(string lexema)
        {
             var simboloTemporal = ObtenerSimboloActual();
            while(char.IsDigit(simboloTemporal)){
                lexema += simboloTemporal;
                _cursor++;
                simboloTemporal = ObtenerSimboloActual();
            }
            return new Token { Tipo = TokenTipos.Numero, Lexema = lexema, Columna = _columnaActual, Fila = _FilaActual };
        }
        private Token ObtenerIdentificador(string lexema)
        {
            var simboloTemporal = ObtenerSimboloActual();
            while(char.IsLetterOrDigit(simboloTemporal)||simboloTemporal == '_'){
                lexema += simboloTemporal;
                _cursor++;
                simboloTemporal = ObtenerSimboloActual();
            }
            return new Token {Tipo = _palabrasReservadas.ContainsKey(lexema)?_palabrasReservadas[lexema]:TokenTipos.Identificador
                 , Lexema = lexema, Columna = _columnaActual, Fila = _FilaActual };
        }
        private char ObtenerSimboloActual()
        {
            if (_cursor < _codigoFuente.Length)
            {
                return _codigoFuente[_cursor];
            }
            return '\0';
        }
        private string ObtenerTextoUnaLinea(char Delimitador)
        {
            string _texto = "";
            _cursor++;
            var simboloTemporal = ObtenerSimboloActual();
           while (true)
           {
               if ((simboloTemporal == Delimitador && _codigoFuente[_cursor-1] != '\\') || simboloTemporal == '\0' || simboloTemporal == '\n')
                 {
                   return _texto;
                 }
                 _texto += simboloTemporal;
                 _cursor++;
                simboloTemporal = ObtenerSimboloActual();
             }             
        }
        private string ObtenerTextoMultiplesLineas(char Delimitador)
        {
            string _texto = "";
            _cursor++;
            var simboloTemporal = ObtenerSimboloActual();
            while (true)
            {
                if ((simboloTemporal == Delimitador && _codigoFuente[_cursor - 1] != '\\') || simboloTemporal == '\0')
                {
                    return _texto;
                }
                _texto += simboloTemporal;
                _cursor++;
                if (simboloTemporal == '\n')
                {
                    _FilaActual++;
                    _columnaActual = 1;
                }
                simboloTemporal = ObtenerSimboloActual();
            }
        }
        private string ObtenerComentarioBloque(char Delimitador,char Delimitador2)
        {
            string _texto = "";
            _cursor++;
           var simboloTemporal = ObtenerSimboloActual();
           while (true)
           {
                 if (simboloTemporal == Delimitador && _codigoFuente[_cursor+1] == Delimitador2 || simboloTemporal == '\0')
                 {
                   return _texto;
                 }
                 _texto += simboloTemporal;
                 _cursor++;
                 if (simboloTemporal == '\n')
                 {
                    _FilaActual++;
                    _columnaActual = 1;
                 }
                 simboloTemporal = ObtenerSimboloActual();
             }           
        } 
    }
}
