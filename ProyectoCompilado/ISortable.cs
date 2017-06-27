using RaimProgram.Base;

namespace RaimProgram.Common.Sorting
{
    public interface ISortable
    {
       void SortPersons(Person[] persons, int size);
    }

    public interface IPrintable
    {
       void print();
    }
}              