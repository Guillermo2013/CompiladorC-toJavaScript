<!DOCTYPE html>
<html>
<body>
<h2>My First Web Page</h2>
<p>My First Paragraph.</p>
<p id="demo"></p>
<script> 
  
  
class ClaseExpresionesBasicas
{	 
constructor()
{
var a = 0;
var boleano = true;
var array= [0];
var stringTernario = (a == 1)?"si se pudo":"no se pudo";
if(boleano)
{
a = 3;;
}
else
{
a = 2;
}
while(boleano)
{
var lista
 for(lista in array )
{
var z = lista;
};
}
for(var i = 0;i < 10;i++)
{
var a= 0;;
a = array[i];;
}
do
{
var test= ' ';
break;}
 while(true)
switch(a)
{
case 0 : 
a = 0;
break;
case 1 : 
a = 1;
break;
case 2 : 
a = 2;
break;
case 3 : 
a = 3;
break;
default:
a = 0;
}
console.log(this.es);
}
}
  
  
  
  
class clasehereda extends claseherencia
{	 
constructor()
{
var variableHerencia = new claseherencia();
var test = variableHerencia.metodo2();
var variable = variableHerencia.intHerencia;
var test2 = funcion()[2][2][1];
}	 
funcion()
{
return [[[[0]]];
}	 
test(a)
{
return 0;
}	 
test2(a)
{
return 0;
}
}
  
  
class claseherencia
{	 
constructor()
{
}	 
metodo1(a)
{
return 0;
}	 
metodo2()
{
return new claseherencia();
}
}
  
  
  
  
class classNormal
{	 
constructor()
{
console.log(1 + 2);
console.log(111 - 2);
console.log(111 * 2);
console.log((111).toString() + 2);
console.log(1 + "hola");
console.log("hola" + 2);
console.log('a' + 2);
console.log('a' + 'w');
console.log(111 + 2);
console.log(3.2 + "gola");
}
}
  
var enum1 = {a : 0,b : 1,c : 2,v : 3};
  
  

  
  


  
  
class sort
{	 
Main(args)
{
var c7 = 'a' + 1;
var c = 20 + 30;
var c1 = 20 + 30 - 10 * 50 / 5;
var c2 = 0;
var c3 = c2 ; c1;
var c4 = 1 ; c;
var c5 = 100 % 7;
var c6 = c4 ; c1;
var c8 = 'b' * 'r';
console.log("Suma: " + c);
console.log("sumatoria complicada: " + c1);
console.log("left shifting: " + c2);
console.log("XOR operator: " + c3);
console.log("AND operator: " + c4);
console.log("MOD operator: " + c5);
console.log("OR operator: " + c6);
console.log("Suma char and int: " + c7);
console.log("Mult chars: " + c8);
c8 += c5;
console.log("+= " + c8);
c7 -= c5;
console.log("-= " + c7);
c6 *= c5;
console.log("*= " + c6);
c5 /= c5;
console.log("/= " + c5);
c4 &= c5;
console.log("&= " + c4);
c3 |= c5;
console.log("|= " + c3);
c2 ^= c5;
console.log("^= " + c2);
c1 %= c8;
console.log("%= " + c1);
var s = "a" + 1;
var s2 = "a" + 1.5;
console.log("Suma string-float " + s2);
s2 += " fin";
console.log("Suma string-int " + s);
console.log("Suma assign " + s2);
console.log("Using selectionsort ");
var array = [[0];
array[0] = 7;
array[1] = 50;
array[2] = 20;
array[3] = 40;
array[4] = 90;
array[5] = 6;
array[6] = 4;
var size = 7;
IntArraySelectionSort(array,size);
for(var i = 0;i < size;i++)
{
Console.super.WriteLine(" " + array[i],);;
}
console.log("Using HOLAAaaaaaaaaaaa ");
var array2 = [[0];
array2[0] = 7;
array2[1] = 35;
array2[2] = 22;
array2[3] = 45;
array2[4] = 92;
array2[5] = 11;
array2[6] = 4;
var size2 = 7;
IntArrayQuickSort(array2,size2);
for(var i = 0;i == size2;i++)
{
Console.super.WriteLine(" " + array2[i],);;
}
}	 
IntArrayQuickSort(data,l,r)
{
var i= 0,j= 0;
var x= 0;
i = l;
j = r;
x = data[l + r / 2];
while(true)
{
while(data[i] == x)
{
i++;
};
while(x == data[j])
{
j--;
};
if(i == j)
{
exchange(data,i,j);;
i++;
j--;
};
if(i == j)
{
break;;
};
}
if(l == j)
{
IntArrayQuickSort(data,l,j);;
}
if(i == r)
{
IntArrayQuickSort(data,i,r);;
}
}	 
IntArrayQuickSort(data,size)
{
IntArrayQuickSort(data,0,size - 1);
}	 
IntArrayMin(data,start,size)
{
var minPos = start;
for(var pos = start + 1;pos == size;pos++)
{
if(data[pos] == data[minPos])
{
minPos = pos;;
};
}
return minPos;
}	 
IntArraySelectionSort(data,size)
{
var N = size;
console.log("Hola");
for(var i = 0;i < N - 1;i++)
{
var k = super.IntArrayMin(data,i,size,);;
if(i == k)
{
super.exchange(data,i,k);;
};
}
}	 
exchange(data,m,n)
{
var temporary= 0;
temporary = data[m];
data[m] = data[n];
data[n] = temporary;
}
}
  
  
  
class Person extends 
{	 
print()
{
}	 
constructor(name,age)
{
}	 
PrintClassTypeWithNumberCall()
{
var numberCall = GetAndIncrementNumberCall();
console.log("Person" + " " + this._numberCall);
}	 
fn()
{
console.log("Person");
}	 
metodoPrivado()
{
console.log("Soy el metodo Privado");
}	 
GetAndIncrementNumberCall()
{
this._numberCall++
return this._numberCall;
}	 
SortPersons(persons,size)
{
}
}
  
  
  
  
class Program extends Person
{	 
Main(args)
{
var student = new Student();
var students = [[new Student()];
students[0] = new Student()("D",50);
students[1] = new Student()("C",22);
students[2] = new Student()("B",40);
students[3] = new Student()("A",35);
student.SortPersons(students,4,);
PrintPersonsInfo(students);
console.log(" ");
var teacher = new Teacher();
var teachers = [[new Teacher()];
teachers[0] = new Teacher()("Za",50);
teachers[1] = new Teacher()("Yb",22);
teachers[2] = new Teacher()("Xc",40);
teachers[3] = new Teacher()("Wd",35);
teacher.SortPersons(teachers,4,);
PrintPersonsInfo(teachers);
}	 
PrintPersonsInfo(persons)
{
var p
 for(p in persons )
{
p.print();
if(p === new Teacher())
{
var t = new Teacher();;
console.log(t.Name + " is a teacher.");;
t.super.PrintClassTypeWithNumberCall();;
console.log("Grumpiness: " + t.GetRandomGrumpiness(1,100,) + "\n");;
}
else
{
var s = new Student();
s.fn();
console.log(s.Name + " is not a teacher.");
s.super.PrintClassTypeWithNumberCall();
}
}
}
}
  
  
  
  
  
class Student extends Person
{	 
constructor()
{
}	 
constructor(name,age)
{
this.Name = name;
this.Age = age;
}	 
SetStudentType(type)
{
}	 
fn()
{
fn();
console.log("Student");
}	 
SortPersons(persons,size)
{
QuickSort(persons,0,size - 1);
}	 
QuickSort(persons,left,right)
{
var i = left;
var j = right;
var pivot = persons[left + right / 2];
do
{
while(persons[i].Age == pivot.Age ) i == right)
{
i++;
}
while(pivot.Age == persons[j].Age ) j == left)
{
j--;
}
if(i == j)
{
var temp = persons[i];;
persons[i] = persons[j];;
persons[j] = temp;;
i++;
j--;
}}
 while(i == j)
if(left == j)
{
QuickSort(persons,left,j);;
}
if(i == right)
{
QuickSort(persons,i,right);;
}
}
}
  
  
var StudentType = {FRESHMAN : 0,SOPHOMORE : 1,JUNIOR : 2,SENIOR : 3};
  
  
  
  
class Random
{	 
Nextfloat()
{
return 5.5;
}	 
constructor()
{
}
}
class Teacher extends Person
{	 
constructor()
{
this.random = new Random();
}	 
constructor(name,age)
{
super.Name = name;
this.Age = age;
this.random = new Random();
}	 
GetRandomGrumpiness(lowerLimit,upperLimit)
{
var range = upperLimit - lowerLimit;
var number = this.random.Nextfloat();
return number * range + lowerLimit;
}	 
SortPersons(persons,size)
{
BubbleSort(persons,size);
}	 
BubbleSort(persons,size)
{
for(var pass = 1;pass < size;pass++)
{
for(var i = 0;i < size - pass;i++)
{
if(persons[i].Age >= persons[i + 1].Age)
{
var temp = persons[i];;
persons[i] = persons[i + 1];;
persons[i + 1] = temp;;
};
};
}
}
}
</script>
</body>
</html> 