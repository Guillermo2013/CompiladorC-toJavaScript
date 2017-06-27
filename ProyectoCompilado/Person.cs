using System;
using RaimProgram.Common.Sorting;

namespace RaimProgram
{
    public class Person : ISortable, IPrintable
    {
        public string Name;
        public int Age;
        private int _numberCall=0;

       
        public void print(){
          //  Console.WriteLine(ToString());
        }
        public Person(string name, int age)
        {
          //  Name = name;
           // Age = age;
        }
        public void PrintClassTypeWithNumberCall()
        {
            int numberCall = GetAndIncrementNumberCall();
            Console.WriteLine("Person" + " " + _numberCall);
        }
        public void fn(){
            Console.WriteLine("Person");
        }
        public void metodoPrivado(){
            Console.WriteLine("Soy el metodo Privado");
        }
        public int GetAndIncrementNumberCall()
        {
            _numberCall++;
            return _numberCall;
        }
       
        public  void SortPersons(Person[] persons, int size)
        {

        }
    }

    
}           
