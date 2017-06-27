using System;
using RaimProgram.Base;
using RaimProgram;
namespace RaimProgram.Base.Derivatives
{
    public class Random{
        public float Nextfloat(){
            return 5.5f;
        }
        public Random(){}
    }
    public class Teacher : Person
    {
        private Random random;
        public String Name;
        public int Age;

        public Teacher()
        {
            random = new Random();
        }

        public Teacher(string name, int age)
        {
            Name = name;
            Age = age;
            random = new Random();
        }

        public float GetRandomGrumpiness(float lowerLimit, float upperLimit)
        {
            float range = upperLimit - lowerLimit;
            float number =  random.Nextfloat();
            return (number * range) + lowerLimit;
        }

        public override void SortPersons(Person persons, int size)
        {
            BubbleSort(persons, size);
        }

       

        public static void BubbleSort(Person[] persons, int size)
        {
            for (int pass = 1; pass < size; pass++)
            {
                for (int i = 0; i < size - pass; i++)
                {
                    if (persons[i].Age >= persons[i + 1].Age)
                    {
                        var temp = persons[i];
                        persons[i] = persons[i + 1];
                        persons[i + 1] = temp;
                    }
                }
            }
        }
    }
}          
