using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Arbol.Identificadores;
using Compiladores.Semantico;
using Compiladores.Arbol.BinaryNodes;
using Compiladores.Semantico.Tipos;
namespace Compiladores.Arbol.StatementNodes
{
    class AssignasionVariable: StatementNode
    {
        public IdentificadoresExpressionNode identificador;
        public ExpressionNode expresion;
 
        public override void ValidateSemantic()
        {
             var id = identificador.ValidateSemantic();
            if (expresion is CallFuntionNode)
            {
                TiposBases funcion = validarFuncion();
                if (id.GetType() != funcion.GetType())
                    throw new SemanticoException(archivo+"no se puede asignar" + funcion + "a "+id + "fila" + expresion.token.Fila + "columna" + expresion.token.Columna);
            }else{
                if(expresion is AutoDeplazamientoDerechaNode||expresion is AutoDeplazamientoIzquierdaNode || expresion is AutoOperacionAndPorBitNode||
                    expresion is AutoOperacionDivisionNode || expresion is AutoOperacionMultiplicacionNode || expresion is AutoOperacionOExclusivoPorBitNode||
                    expresion is AutoOperacionOrPorBitNode || expresion is AutoOperacionResiduoNode || expresion is AutoOperacionRestaNode || expresion is AutoOperacionSumaNode)
                {
                    (expresion as BinaryOperatorNode).OperadorIzquierdo = identificador;
                    expresion.ValidateSemantic();
                }
                else {

                    var expresionTipo = expresion.ValidateSemantic();
                    if ((id is ClaseTipo && expresionTipo is ClaseTipo))
                    {
                        if ((id as ClaseTipo).nombreClase != (expresionTipo as ClaseTipo).nombreClase)
                            throw new SemanticoException(archivo+"se tiene que asignar el mismo tipo " + (id as EnumTipo).nombreClase + "fila"
                                + expresion.token.Fila + "columna" + expresion.token.Columna);
                    }
                    else if ((id is EnumTipo && expresionTipo is EnumTipo))
                    {
                        if ((id as EnumTipo).nombreClase != (expresionTipo as EnumTipo).nombreClase)
                            throw new SemanticoException(archivo+"se tiene que asignar el mismo tipo " + (id as EnumTipo).nombreClase + "fila"
                                + expresion.token.Fila + "columna" + expresion.token.Columna);
                    }
                    else if ((id is ArrayTipo) && (expresionTipo is ArrayTipo))
                    {
                        var declarArray = (id as ArrayTipo).cantidad.ToArray();
                        var tipoDefinionArray = (expresionTipo as ArrayTipo).cantidad.ToArray();
                        if (tipoDefinionArray.Length != declarArray.Length)
                            throw new SemanticoException(archivo+"se tiene que asignar el mismo tipo al arreglo " + identificador.nombre + "fila"
                                + identificador.token.Fila + "columna" + identificador.token.Columna);
                       
                        for (int i = 0; i < declarArray.Length; i++)
                        {
                            if (tipoDefinionArray[i].Count != declarArray[i].Count)
                                throw new SemanticoException(archivo+"se tiene que asignar el mismo tipo al arreglo " + identificador.nombre + "fila"
                                    + identificador.token.Fila + "columna" + identificador.token.Columna);
                        }
                      
                    }
                    if (id.GetType() != expresionTipo.GetType())
                        throw new SemanticoException(archivo+"no se puede asignar" + expresionTipo + "a " + id + "fila" + expresion.token.Fila + "columna" + expresion.token.Columna);
                }
               
            }
            
        }
        public override string GenerarCodigo()
        {
            if (expresion is AutoDeplazamientoDerechaNode || expresion is AutoDeplazamientoIzquierdaNode || expresion is AutoOperacionAndPorBitNode ||
                     expresion is AutoOperacionDivisionNode || expresion is AutoOperacionMultiplicacionNode || expresion is AutoOperacionOExclusivoPorBitNode ||
                     expresion is AutoOperacionOrPorBitNode || expresion is AutoOperacionResiduoNode || expresion is AutoOperacionRestaNode || expresion is AutoOperacionSumaNode)
            {
                (expresion as BinaryOperatorNode).OperadorIzquierdo = identificador;
                return expresion.GenerarCodigo() + ";";
            }
            else
            {
                return identificador.GenerarCodigo() + " = " + expresion.GenerarCodigo()+";";
            }
        }
        public TiposBases validarFuncion()
        {
            var funcion = expresion as CallFuntionNode;
            TiposBases tiporeturno = null;
            foreach (var usingList in ContenidoStack._StackInstance.usingNombres)
            {
                if (TablaDeNamespace.Instance.Tabla.ContainsKey(usingList))
                {
                    var elementos = TablaDeNamespace.Instance.Tabla[usingList];
                    foreach (var elemento in elementos)
                    {
                        if (elemento is ClassNode)
                            if ((elemento as ClassNode).nombre == ContenidoStack.InstanceStack.claseActual)
                            {
                                var elementosClase = (elemento as ClassNode).cuerpo;
                                var encontro = false;
                               
                                foreach (var elementoClase in elementosClase)
                                {
                                    if (elementoClase is DeclaracionMetodo)
                                    {
                                        if ((elementoClase as DeclaracionMetodo).nombre == funcion.nombre)
                                        {
                                            tiporeturno = (elementoClase as DeclaracionMetodo).tipo.ValidateSemantic();
                                            if ((elementoClase as DeclaracionMetodo).parametros.Count == funcion.parametros.Count)
                                            {
                                                var abuscarParametro = (elementoClase as DeclaracionMetodo).parametros.ToArray();
                                                var metroParametro = funcion.parametros.ToArray();
                                                encontro = true;
                                                for (int z = 0; z < abuscarParametro.Length; z++)
                                                {
                                                    var tipo = metroParametro[z].ValidateSemantic().GetType();
                                                    var tipoB = abuscarParametro[z].tipo.ValidateSemantic().GetType();
                                                    if (tipoB != tipo)
                                                        throw new SemanticoException(archivo+"se esperaba el tipo " + tipoB + "fila " + metroParametro[z].token.Fila
                                                            + "columna" + metroParametro[z].token.Columna);
                                                }

                                            }
                                        }
                                    }
                                }
                                if (!encontro)
                                    throw new SemanticoException(archivo+"no se encontro la funcion " + funcion.nombre + "fila" + this.token.Fila + "columna" + this.token.Columna);
                                
                            }
                    }
                }
            }
            return tiporeturno;
        }
    }
}
