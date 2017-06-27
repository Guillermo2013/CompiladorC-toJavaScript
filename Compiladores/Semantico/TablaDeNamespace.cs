using Compiladores.Arbol.BaseNode;
using Compiladores.Arbol.StatementNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Semantico
{
    class TablaDeNamespace
    {
        public static TablaDeNamespace _instance = null;
        public Dictionary<string,List<StatementNode>> Tabla;
        public string archivo;
        public TablaDeNamespace()
        {
            Tabla = new Dictionary<string, List<StatementNode>>();
     
        }

        public static TablaDeNamespace Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TablaDeNamespace();
                }
                return _instance;
            }
        }

        public void DeclareNameSpace(string name, List<StatementNode> Cuerpo)
        {
            List<StatementNode> lista = new List<StatementNode>();
            foreach (StatementNode elemento in Cuerpo)
            {
                if (elemento is EnumNode)
                {
                    lista.Add(elemento);
                }
                else if (elemento is InterfaceNode)
                {
                    lista.Add(elemento);
                }
                else if (elemento is ClassNode)
                {
                    lista.Add(elemento);
                }
            }
          
                if (Tabla.ContainsKey(name))
                {
                    Tabla[name].AddRange(lista);
                }
                else
                {
                    Tabla.Add(name, lista);
                }

                var arraStatement = Tabla[name].ToArray();
                int i = 0;
                while (i < arraStatement.Length)
                {
                    string Abuscar ="";
                    if (arraStatement[i] is EnumNode)
                        Abuscar = (arraStatement[i] as EnumNode).identificador;
                    else if (arraStatement[i] is InterfaceNode)
                        Abuscar = (arraStatement[i] as InterfaceNode).nombre;
                    else if (arraStatement[i] is ClassNode)
                           Abuscar = (arraStatement[i] as ClassNode).nombre;
                    for (int j = i + 1; j < arraStatement.Length; j++)
                    {
                        string nombreRepetido = "";
                        if (arraStatement[j] is EnumNode)
                            nombreRepetido = (arraStatement[j] as EnumNode).identificador;
                        else if (arraStatement[j] is InterfaceNode)
                            nombreRepetido = (arraStatement[j] as InterfaceNode).nombre;
                        else if (arraStatement[j] is ClassNode)
                            nombreRepetido = (arraStatement[j] as ClassNode).nombre;
                        
                        if(nombreRepetido == Abuscar)
                            throw new SemanticoException( arraStatement[i].archivo+"la interface ,clases o enum no tiene que tener el mismo nombre" + nombreRepetido 
                                + " en el namespace" + name + " fila " + arraStatement[i].token.Fila + " columna" + arraStatement[i].token.Columna);
                    }
                    i++;
                }
           
        }

        
    }
}
