using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:/Users/Douglas Sandoval/Desktop/Compilador C#/ProyectoCompilado";
           DirectoryInfo dir = new DirectoryInfo(path);
           var listaArbol = new List<List<StatementNode>>();
           foreach (FileInfo f in dir.GetFiles("*.cs")) {
               string codigo= "";
               System.IO.StreamReader file = new System.IO.StreamReader(path+"/"+f.Name);
               codigo = file.ReadToEnd().ToString();  
               var lexico = new Lexico(codigo);
               var Parser = new Parser(lexico);
               listaArbol.Add(Parser.Code());
            }
 
           foreach (var listaStatement in listaArbol)
           {
              
               foreach (var statement in listaStatement)
               {
                   statement.ValidateSemantic();
               }
               ContenidoStack._StackInstance.usingNombres = new List<string>();
           }
     
        }
    }
}