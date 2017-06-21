using Compiladores.Semantico.Tipos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Semantico
{
    public class ContenidoStack
    {
        public static ContenidoStack _StackInstance = null;
        public Stack<TablaSimbolos> Stack;
        public List<string> usingNombres;
        public string claseActual;


        private ContenidoStack()
        {
            usingNombres = new List<string>();
            Stack = new Stack<TablaSimbolos>();
            Stack.Push( new TablaSimbolos());
            
        }

        public static ContenidoStack InstanceStack
        {
            get
            {
                if (_StackInstance == null)
                {
                    _StackInstance = new ContenidoStack();
                }
                return _StackInstance;
            }
        }
        public bool VariableUsing(string name)
        {
            if (usingNombres.Count == 0)
                return false;
            return usingNombres.Contains(name);
        }
        public void DeclareUsing(string name)
        {
            usingNombres.Add(name);

        }

    }
}