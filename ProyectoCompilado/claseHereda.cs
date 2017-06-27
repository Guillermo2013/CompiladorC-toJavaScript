using System;
using ClasseHerencia;
using interfaceNamespace;
namespace clasehereda
{
    public class clasehereda : claseherencia,interface1
    {
        public clasehereda()
	    {
            claseherencia variableHerencia = new claseherencia();
            claseherencia test = variableHerencia.metodo2();
            int variable = variableHerencia.intHerencia;
            var test2 = funcion()[2, 2, 1];
	    }
        public int[, ,] funcion()
        {
            return new int[2,3,4];
        }
        public int test(int a)
        {                                                                                                                                                                                                                                                                                                              
            return 0;
        }
        public int test2(string  a)
        {
            return 0;
        }
    }
}

