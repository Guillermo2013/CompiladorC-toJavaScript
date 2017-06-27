using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Arbol.Identificadores;
using Compiladores.Semantico.Tipos;

namespace Compiladores.Arbol.StatementNodes
{
    class ClassNode : StatementNode
    {
        public string encasulamiento;
        public string nombre;
        public string modificador;
        public List<ExpressionNode> herencia;
        public List<StatementNode> cuerpo;
        
        public override void ValidateSemantic()
        {
            validateHerenciaNombre();
            var listaHerencia = ValidateHerencia().ToArray();
            var listaValidarAbstract = validarFuncionesAbstract().ToArray();
            ContenidoStack._StackInstance.claseActual = nombre;
            var cuerpoArray = validadFuncionRepetida();
            validarElementosInterface(listaHerencia, cuerpoArray);
            validarFuncionAbstracta(listaValidarAbstract, cuerpoArray);
               
                foreach (var statement in cuerpo)
                {
                    if (statement is ConstructorNode)
                    {
                        if (nombre != (statement as ConstructorNode).identificador)
                            throw new SemanticoException(archivo+"el constructo debe llamarse igual a la clase" + (statement as ConstructorNode).identificador
                                + "fila " + (statement as ConstructorNode).token.Fila + "columna" + (statement as ConstructorNode).token.Columna);
                        if (herencia.Count == 0 && (statement as ConstructorNode).baseParametos.Count != 0)
                            throw new SemanticoException(archivo+"el constructo no tiene una base" + (statement as ConstructorNode).identificador
                                + "fila " + (statement as ConstructorNode).token.Fila + "columna" + (statement as ConstructorNode).token.Columna);
                    }

                    statement.ValidateSemantic();
                    if (statement is DeclaracionVariableNode)
                        ContenidoStack._StackInstance.Stack.Pop();
                    if (statement is DeclaracionMetodo)
                    {
                        if ((statement as DeclaracionMetodo).modificar == "abstract" && modificador != "abstract")
                        {
                            throw new SemanticoException(archivo+"la funcion tiene que estar en clase abstract" + (statement as DeclaracionMetodo).nombre
                                + "fila " + (statement as DeclaracionMetodo).token.Fila + "columna" + (statement as DeclaracionMetodo).token.Columna);
                        }
                        else if ((statement as DeclaracionMetodo).modificar == "abstract" && modificador == "abstract")
                        {
                            if((statement as DeclaracionMetodo).cuerpo != null)
                                throw new SemanticoException(archivo+"la funcion abstacta no tiene definicion" + (statement as DeclaracionMetodo).nombre
                                + "fila " + (statement as DeclaracionMetodo).token.Fila + "columna" + (statement as DeclaracionMetodo).token.Columna);
                        }
                    }
                }
          

                
               
        }
        public override string GenerarCodigo()
        {
            string value = "class " + nombre;
            if (herencia != null && herencia.Count != 0)
            {
                value += " extends ";
                foreach (var lista in herencia)
                {
                   if(ValidateHerenciaClases((lista as IdentificadoresExpressionNode).nombre).Count == 0)
                    value += lista.GenerarCodigo();
                }
            }
            value += "\n{";
            if (cuerpo != null)
            {
                foreach (var lista in cuerpo)
                {
                    

                        if (lista is DeclaracionMetodo|| lista is ConstructorNode)
                        {
                            if(modificador == "static")
                                value += "\nstatic " + lista.GenerarCodigo();
                            value += "\t \n" + lista.GenerarCodigo();
                        }
                        
                }

            }
            value += "\n}";
            return value;
        }

        private void validarFuncionAbstracta(StatementNode[] listaValidarAbstract, StatementNode[] cuerpoArray)
        {
            if (modificador != "abstract")
                for (int i = 0; i < listaValidarAbstract.Length; i++)
                {
                    var encontrado = false;

                    if ((listaValidarAbstract[i] is DeclaracionMetodo))
                    {
                        if ((listaValidarAbstract[i] as DeclaracionMetodo).modificar == "abstract")
                        {
                            for (int j = 0; j < cuerpoArray.Length; j++)
                            {
                                if (cuerpoArray[j] is DeclaracionMetodo)
                                {
                                    if ((listaValidarAbstract[i] as DeclaracionMetodo).nombre == (cuerpoArray[j] as DeclaracionMetodo).nombre &&
                                        (listaValidarAbstract[i] as DeclaracionMetodo).tipo.ValidateSemantic().GetType() == (cuerpoArray[j] as DeclaracionMetodo).tipo.ValidateSemantic().GetType() &&
                                        (listaValidarAbstract[i] as DeclaracionMetodo).parametros.Count == (cuerpoArray[j] as DeclaracionMetodo).parametros.Count
                                        && (cuerpoArray[j] as DeclaracionMetodo).modificar == "override")
                                    {

                                        var abuscarParametro = (listaValidarAbstract[i] as DeclaracionMetodo).parametros.ToArray();
                                        var metroParametro = (cuerpoArray[j] as DeclaracionMetodo).parametros.ToArray();
                                        if (abuscarParametro.Length == 0)
                                            encontrado = true;
                                        for (int z = 0; z < abuscarParametro.Length; z++)
                                        {
                                            if (abuscarParametro[z].tipo.ValidateSemantic().GetType() != metroParametro[z].tipo.ValidateSemantic().GetType())
                                            {
                                                encontrado = false;
                                                z = abuscarParametro.Length;
                                            }
                                            else
                                            {
                                                encontrado = true;
                                            }

                                        }

                                    }
                                }

                            }

                        }
                        if (encontrado == false)
                            throw new SemanticoException(archivo+listaValidarAbstract[i] + "debe de implementar todo lo elementos de la clase abstracta fila" + token.Fila + "columna" + token.Columna);
                    }

                }
        }

        private List<StatementNode> validarFuncionesAbstract()
        {
            List<StatementNode> list = new List<StatementNode>(); ;
            foreach (var nombreHerencia in herencia)
            {
             
                var herenciaNombre = (nombreHerencia as IdentificadoresExpressionNode).nombre;
                if (herenciaNombre == nombre)
                    throw new SemanticoException(archivo+"no se puede heredar la misma clase  " + herenciaNombre + " fila" + token.Fila + "columna" + token.Columna);
                foreach (var namespaceLista in ContenidoStack._StackInstance.usingNombres)
                    if (TablaDeNamespace.Instance.Tabla.ContainsKey(namespaceLista))
                    {
                        var lista = TablaDeNamespace.Instance.Tabla[namespaceLista];
                        foreach (var listaclase in lista)
                            if (listaclase is ClassNode)
                            {
                                if (herenciaNombre == (listaclase as ClassNode).nombre && (listaclase as ClassNode).modificador == "abstract")
                                {

                                    if ((listaclase as ClassNode).encasulamiento != "public" && (listaclase as ClassNode).encasulamiento != "")
                                        throw new SemanticoException(archivo+"la clase abstract no es publica " + herenciaNombre + " fila" + token.Fila + "columna" + token.Columna);

                                    list.AddRange(ObtenerClasePadre(listaclase as ClassNode));

                                }
                            }
                    }

            }
            return list;
         
        }

        private StatementNode[] validadFuncionRepetida()
        {
            var cuerpoArray = cuerpo.ToArray();
            for (int i = 0; i < cuerpoArray.Length; i++)
            {
                if (cuerpoArray[i] is DeclaracionMetodo)
                    for (int j = i + 1; j < cuerpoArray.Length; j++)
                    {
                        if (cuerpoArray[j] is DeclaracionMetodo)
                        {
                            var metodobuscar = cuerpoArray[i] as DeclaracionMetodo;
                            var metodoEncontrar = cuerpoArray[j] as DeclaracionMetodo;
                            if (metodobuscar.nombre == metodoEncontrar.nombre)
                            {
                                if (metodoEncontrar.parametros.Count == metodobuscar.parametros.Count)
                                {
                                    var diferente = false;
                                    var abuscarParametro = metodobuscar.parametros.ToArray();
                                    var metroParametro = metodoEncontrar.parametros.ToArray();
                                    for (int z = 0; z < abuscarParametro.Length; z++)
                                    {
                                        if (abuscarParametro[i].tipo.ValidateSemantic().GetType() != metroParametro[i].tipo.ValidateSemantic().GetType())
                                            diferente = true;
                                    }

                                    if (!diferente)
                                        throw new SemanticoException(archivo+"no se puede redefenir la funcion llamado " + metodobuscar.nombre + "fila" + metodobuscar.token.Fila + "columna" + metodobuscar.token.Columna);
                                }
                            }
                        }
                    }
            }
            return cuerpoArray;
        }

        private void validarElementosInterface(InterzaceHeaderNode[] listaHerencia, StatementNode[] cuerpoArray)
        {
            for (int i = 0; i < listaHerencia.Length; i++)
            {
                var encontrado = false;
                for (int j = 0; j < cuerpoArray.Length; j++)
                {
                    if (cuerpoArray[j] is DeclaracionMetodo)
                    {
                        if (listaHerencia[i].nombre == (cuerpoArray[j] as DeclaracionMetodo).nombre &&
                            listaHerencia[i].tipo.ValidateSemantic().GetType() == (cuerpoArray[j] as DeclaracionMetodo).tipo.ValidateSemantic().GetType() &&
                            listaHerencia[i].parametro.Count == (cuerpoArray[j] as DeclaracionMetodo).parametros.Count)
                        {
                            encontrado = true;
                            var abuscarParametro = listaHerencia[i].parametro.ToArray();
                            var metroParametro = (cuerpoArray[j] as DeclaracionMetodo).parametros.ToArray();
                            for (int z = 0; z < abuscarParametro.Length; z++)
                            {
                                if (abuscarParametro[z].tipo.ValidateSemantic().GetType() != metroParametro[z].tipo.ValidateSemantic().GetType())
                                {
                                    encontrado = false;
                                    z = abuscarParametro.Length;
                                }
                                else
                                {
                                    encontrado = true;
                                }

                            }

                        }
                    }

                }
                if (encontrado == false)
                    throw new SemanticoException(archivo+"debe de implementar todo lo elementos de la interfaz fila" + token.Fila + "columna" + token.Columna);
            }
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
                        throw new SemanticoException(archivo+"no repetiste clase o interzace" + nombre + "fila " + herenciaArray[j].token.Fila + "columna" + herenciaArray[j].token.Columna);
                }
                i++;
            }
        }

         private List<InterzaceHeaderNode> ValidateHerencia()
        {
            List<InterzaceHeaderNode> list = new List<InterzaceHeaderNode>(); ;
            foreach (var nombreHerencia in herencia)
            {
                bool encontrado = false;
                var herenciaNombre = (nombreHerencia as IdentificadoresExpressionNode).nombre;
                if (herenciaNombre == nombre)
                    throw new SemanticoException(archivo+"no se puede heredar la misma clase  " + herenciaNombre + " fila" + token.Fila + "columna" + token.Columna);
                var clase = false;
                foreach (var namespaceLista in ContenidoStack._StackInstance.usingNombres)
                    if (TablaDeNamespace.Instance.Tabla.ContainsKey(namespaceLista))
                    {
                        var lista = TablaDeNamespace.Instance.Tabla[namespaceLista];
                        foreach (var listaclase in lista)
                            if (listaclase is InterfaceNode ) { 
                                if (herenciaNombre == (listaclase as InterfaceNode).nombre)
                                {
                                    encontrado = true;
                                    if ((listaclase as InterfaceNode).encasulamiento != "public" && (listaclase as InterfaceNode).encasulamiento != "")
                                        throw new SemanticoException(archivo+"la inferzace no es publica " + herenciaNombre + " fila" + token.Fila + "columna" + token.Columna);
                                    
                                     list = ObtenerInterfacePadre((listaclase as InterfaceNode));

                                }
                             }
                             else if (listaclase is ClassNode && lista.First() is ClassNode  )
                             {
                                 if (herenciaNombre == (listaclase as ClassNode).nombre)
                                {
                                    clase = true;
                                    encontrado = true;
                                    if ((listaclase as ClassNode).encasulamiento != "public" && (listaclase as ClassNode).encasulamiento != "")
                                        throw new SemanticoException(archivo+"la clase no es publica " + herenciaNombre + " fila" + token.Fila + "columna" + token.Columna);
                                  
                                }

                             }
                            else if (clase == true && !(listaclase is InterfaceNode))
                            {
                                throw new SemanticoException(archivo + "la clase tiene que ser clase " + herenciaNombre + " fila" + token.Fila + "columna" + token.Columna);
                            }
                    }
                
                    if (encontrado == false)
                        throw new SemanticoException(archivo+"no se encuentra la interface ,clase o enum  " + herenciaNombre + " fila" + token.Fila + "columna" + token.Columna);
                }
            return list;
         
            }

        private List<InterzaceHeaderNode> ObtenerInterfacePadre(InterfaceNode interfaceNode)
        {
            var lista = new List<InterzaceHeaderNode>();
            lista.AddRange(interfaceNode.cuerpo);
            if (interfaceNode.herencia != null)
            {
                foreach (var herenciaEle in interfaceNode.herencia)
                {
                    foreach (var namespaceEle in TablaDeNamespace._instance.Tabla)
                    {
                        foreach (var elemento in namespaceEle.Value)
                        {
                            if (elemento is InterfaceNode)
                            {
                                if ((elemento as InterfaceNode).nombre == (herenciaEle as IdentificadoresExpressionNode).nombre)
                                {
                                    lista.AddRange(ObtenerInterfacePadre(elemento as InterfaceNode));
                                }
                            }
                        }
                    }
                }
            }
            return lista;
        }

        private List<StatementNode> ObtenerClasePadre(ClassNode ClaseNode)
        {
            var lista = new List<StatementNode>();
            lista.AddRange(ClaseNode.cuerpo);
            if (ClaseNode.herencia != null)
            {
                foreach (var herenciaEle in ClaseNode.herencia)
                {
                    foreach (var namespaceEle in TablaDeNamespace._instance.Tabla)
                    {
                        foreach (var elemento in namespaceEle.Value)
                        {
                            if (elemento is ClassNode)
                            {
                                if ((elemento as ClassNode).nombre == (herenciaEle as IdentificadoresExpressionNode).nombre &&
                                    (elemento as ClassNode).modificador == "abstract")
                                {
                                    lista.AddRange(ObtenerClasePadre(elemento as ClassNode));
                                }
                            }
                        }
                    }
                }
            }
            return lista;
        }

        private List<InterzaceHeaderNode> ValidateHerenciaClases(string clase)
        {
            List<InterzaceHeaderNode> list = new List<InterzaceHeaderNode>(); ;
            foreach (var nombreHerencia in herencia)
            {
                bool encontrado = false;
                var herenciaNombre = clase;
                if (herenciaNombre == nombre)
                    throw new SemanticoException(archivo + "no se puede heredar la misma clase  " + herenciaNombre + " fila" + token.Fila + "columna" + token.Columna);
                foreach (var namespaceLista in ContenidoStack._StackInstance.usingNombres)
                    if (TablaDeNamespace.Instance.Tabla.ContainsKey(namespaceLista))
                    {
                        var lista = TablaDeNamespace.Instance.Tabla[namespaceLista];
                        foreach (var listaclase in lista)
                            if (listaclase is InterfaceNode)
                            {
                                if (herenciaNombre == (listaclase as InterfaceNode).nombre)
                                {
                                    encontrado = true;
                                    if ((listaclase as InterfaceNode).encasulamiento != "public" && (listaclase as InterfaceNode).encasulamiento != "")
                                        throw new SemanticoException(archivo + "la inferzace no es publica " + herenciaNombre + " fila" + token.Fila + "columna" + token.Columna);

                                    list = ObtenerInterfacePadre((listaclase as InterfaceNode));

                                }
                            }
                            else if (listaclase is ClassNode)
                            {
                                if (herenciaNombre == (listaclase as ClassNode).nombre)
                                {
                                    encontrado = true;
                                    if ((listaclase as ClassNode).encasulamiento != "public" && (listaclase as ClassNode).encasulamiento != "")
                                        throw new SemanticoException(archivo + "la clase no es publica " + herenciaNombre + " fila" + token.Fila + "columna" + token.Columna);

                                }

                            }
                    }

                if (encontrado == false)
                    throw new SemanticoException(archivo + "no se encuentra la interface ,clase o enum  " + herenciaNombre + " fila" + token.Fila + "columna" + token.Columna);
            }
            return list;

        }
    }
    }
 

