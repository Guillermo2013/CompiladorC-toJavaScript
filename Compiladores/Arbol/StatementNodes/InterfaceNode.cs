using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Arbol.Identificadores;

namespace Compiladores.Arbol.StatementNodes
{
    class InterfaceNode : StatementNode
    {
        public string encasulamiento;
        public string nombre;
        public List<ExpressionNode> herencia;
        public List<InterzaceHeaderNode> cuerpo;
        public override void ValidateSemantic()
        {
            validateHerenciaNombre();
             ValidateHerencia();

            var metodosArray = cuerpo.ToArray();
            int i = 0;
            while (i < metodosArray.Length)
            {
                var Abuscar = metodosArray[i];
                for (int j = i + 1; j < metodosArray.Length; j++)
                {
                    var nombre = (Abuscar as InterzaceHeaderNode).nombre;
                    if (nombre == (metodosArray[j] as InterzaceHeaderNode).nombre) {
                        var diferente = false;
                        if ((Abuscar as InterzaceHeaderNode).parametro.Count == (metodosArray[j] as InterzaceHeaderNode).parametro.Count)
                        {
                            var abuscarParametro = (Abuscar as InterzaceHeaderNode).parametro.ToArray();
                            var metroParametro = (metodosArray[j] as InterzaceHeaderNode).parametro.ToArray();
                            for (int z= 0; z < abuscarParametro.Length; z++)
                            {
                                if (abuscarParametro[i].tipo.ValidateSemantic().GetType() != metroParametro[i].tipo.ValidateSemantic().GetType())
                                    throw new SemanticoException(archivo+"no se puede redefenir el member llamado " + nombre + "fila" + Abuscar.token.Fila + "columna" + Abuscar.token.Columna);
                            }
                            
                           
                        }
                    
                    }
                      
                }
                i++;
            }
            
            
        }
        public override string GenerarCodigo()
        {
            return "";
        }

        private void validateHerenciaNombre()
        {
            var herenciaArray = herencia.ToArray();
            int i = 0;
            while (i < herenciaArray.Length)
            {
                var Abuscar = herenciaArray[i];
                for (int j = i + 1; j < herenciaArray.Length; j++)
                {
                    var nombre = (Abuscar as IdentificadoresExpressionNode).nombre;
                    if (nombre == (herenciaArray[j] as IdentificadoresExpressionNode).nombre)
                        throw new SemanticoException(archivo+"no repetiste interface" + nombre + "fila " + herenciaArray[j].token.Fila + "columna" + herenciaArray[j].token.Columna);
                }
                i++;
            }
        }

        private void ValidateHerencia()
        { 
          
            foreach (var nombreHerencia in herencia)
            {  
                bool encontrado = false;
                var herenciaNombre = (nombreHerencia as IdentificadoresExpressionNode).nombre;
                foreach (var namespaceLista in ContenidoStack._StackInstance.usingNombres)
                    if (TablaDeNamespace.Instance.Tabla.ContainsKey(namespaceLista))
                    {
                        var lista = TablaDeNamespace.Instance.Tabla[namespaceLista];
                        foreach (var listaclase in lista)
                            if (listaclase is InterfaceNode)
                                if (herenciaNombre == (listaclase as InterfaceNode).nombre)
                                {
                                    encontrado = true;
                                    if ((listaclase as InterfaceNode).encasulamiento != "public" && (listaclase as InterfaceNode).encasulamiento != "")
                                        throw new SemanticoException(archivo+"la inferzace no es publica " + herenciaNombre + " fila" + token.Fila + "columna" + token.Columna);
                                   
                                }
                    }
                
                    if (encontrado == false)
                        throw new SemanticoException(archivo+"no se encuentra la interface o no lo es " + herenciaNombre + " fila" + token.Fila + "columna" + token.Columna);
                }

            }
        }

    }

