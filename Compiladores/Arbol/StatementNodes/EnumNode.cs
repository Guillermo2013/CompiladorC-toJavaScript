﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
namespace Compiladores.Arbol.StatementNodes
{
    class EnumNode:StatementNode
    {
        public string encasulamiento;
        public List<EnumListNode> lista;
        public string identificador;
        public override void ValidateSemantic()
        {
            var arrayEnum = lista.ToArray();
            int i = 0;
         
            while (i < arrayEnum.Length)
            {
                var Abuscar = arrayEnum[i];
                for (int j = i + 1; j < arrayEnum.Length; j++)
                {
                    var nombre = (Abuscar as EnumListNode).identificador;
                    if (nombre == (arrayEnum[j] as EnumListNode).identificador)
                        throw new SemanticoException(archivo+"se repitio elemento " + nombre + "fila " + arrayEnum[j].token.Fila + "columna" + arrayEnum[j].token.Columna);
                }
                i++;
            }
            foreach (var listaEnum in lista)
                listaEnum.ValidateSemantic();
        }
        public override string GenerarCodigo()
        {
           string value = "var "+identificador+" = {";
           var listaArray = lista.ToArray();
            for(int i=0;i<listaArray.Length;i++)
            {
               value += listaArray[i].GenerarCodigo();
               if (listaArray[i].asignacion == null)
                   value += i.ToString();
                if (i < listaArray.Length - 1)
                    value += ",";
            }
            value += "};";
            return value;
        }
    }
}
