using System;

namespace ClaseExpresionesBasicas
{
	public class ClaseExpresionesBasicas
	{
        int es = 0;
        public ClaseExpresionesBasicas(){
            int a = 0;
            bool boleano = true;
            int[] array;
            string stringTernario = a == 1 ? "si se pudo" : "no se pudo"; 
            if(boleano){
                a = 3;
            }else {
                a = 2;
            }
            while (boleano)
            {
                foreach (var lista in array)
                {
                   var z = lista;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                int a;
                a = array[i];
            }
            do
            {
               char test;
               continue;
            } while (true);
            switch (a)
            {
                case 0:
                    a = 0;
                    break;
                case 1:
                    a = 1;
                    break;
                case 2:
                    a = 2;
                    break;
                case 3:
                    a = 3;
                    break;
                default:
                    a = 0;
            }
            Console.WriteLine(es);
        }
	}
}
