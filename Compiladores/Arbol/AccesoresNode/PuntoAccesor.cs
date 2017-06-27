using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Arbol.Identificadores;
using Compiladores.Semantico;
using Compiladores.Arbol.StatementNodes;
namespace Compiladores.Arbol.AccesoresNode
{
    public class PuntoAccesor:AccesorNode
    {
        public ExpressionNode identificador;
        public string clase;
         
        public override TiposBases ValidateSemantic()
        {
            TiposBases tipo = null;
            if (identificador is CallFuntionNode)
            {
                (identificador as CallFuntionNode).claseActual = clase;
                (identificador as CallFuntionNode).ListaDeAccesores = (identificador as CallFuntionNode).ListaDeAccesores;
                tipo = (identificador as CallFuntionNode).ValidateSemantic();
            }
            else if (identificador is IdentificadoresExpressionNode)
            {
                var existe = false;
                foreach (var usingList in ContenidoStack._StackInstance.usingNombres)
                {

                    if (TablaDeNamespace.Instance.Tabla.ContainsKey(usingList))
                    {
                        var elementos = TablaDeNamespace.Instance.Tabla[usingList];
                        foreach (var elemento in elementos)
                        {
                            if (elemento is ClassNode)
                            {

                                if ((elemento as ClassNode).nombre == clase && (elemento as ClassNode).encasulamiento == "public")
                                {
                                    foreach (var elementoClase in (elemento as ClassNode).cuerpo)
                                    {
                                        if (elementoClase is DeclaracionVariableNode)
                                        {
                                            if ((elementoClase as DeclaracionVariableNode).identificador == (identificador as IdentificadoresExpressionNode).nombre
                                                && (elementoClase as DeclaracionVariableNode).encapsulamiento == "public") { 
                                                existe = true;
                                                if ((identificador as IdentificadoresExpressionNode).ListaDeAccesores.Count != 0)
                                                {
                                                    foreach (var listaAccesores in (identificador as IdentificadoresExpressionNode).ListaDeAccesores)
                                                    {
                                                        if (listaAccesores is PuntoAccesor)
                                                        {
                                                            (listaAccesores as PuntoAccesor).clase = (elemento as ClassNode).nombre;
                                                            tipo = listaAccesores.ValidateSemantic();

                                                        }
                                                    }

                                                }
                                                else
                                                    tipo = (elementoClase as DeclaracionVariableNode).tipo.ValidateSemantic();
                                            }

                                        }
                                    }
                                }
                            }
                        
                        }
                    }
                }
                if (!existe)
                    throw new SemanticoException(archivo+"no existe el la variable o no es publica " + (identificador as IdentificadoresExpressionNode).nombre + "fila"
                        + identificador.token.Fila + "columna" + identificador.token.Columna);
            }
            return tipo;
        }
        public override string GenerarCodigo()
        {
            return "."+identificador.GenerarCodigo();
        }

    }
   
}
