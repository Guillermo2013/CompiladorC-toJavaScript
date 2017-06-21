using interfaceNamespace;
using enumNamespace;
using interface2namespace;
namespace claseNamespace
{
    public class Class1 : interface1
    {
        public Class1 test = new Class1();
        public string variable = "hola";
        public Class1(int a)
        {
          
        }
        public Class1()
        {
            
            
        }
        public Class1 metodo(int a)
        {
           

            return new Class1();
        }
        int test(string a) { return 0; }
        int test(int a, string b) { return 0; }
        int test2(string a) { return 0; }
    }
}



