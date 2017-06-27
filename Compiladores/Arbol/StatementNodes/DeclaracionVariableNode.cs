using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Arbol.Identificadores;
using Compiladores.Semantico.Tipos;

namespace Compiladores.Arbol.StatementNodes
{
    class DeclaracionVariableNode:StatementNode
    {
        public string modificar;
        public string encapsulamiento;
        public DefinicionTipoNode tipo = new DefinicionTipoNode();
        public string identificador;
        public ExpressionNode inicializacion ;
        public List<StatementNode> declarationList;
        public override void ValidateSemantic()
        {
           
            var tipoDefinion = tipo.ValidateSemantic();
            if(tipoDefinion is  VarTipo && declarationList.Count>1)
                throw new SemanticoException(archivo+"no se permite multiples declaraciones " + tipo + "fila" + tipo.token.Fila +
                   "columna" + tipo.token.Columna);

            if (identificador != null && identificador != "")
            {
                foreach (var stack in ContenidoStack._StackInstance.Stack)
                    if (stack.VariableExist(identificador))
                        throw new SemanticoException(archivo+"ya existe la variable" + identificador + "fila" +token.Fila +
                    "columna" + token.Columna);
                ContenidoStack._StackInstance.Stack.Peek().DeclareVariable(identificador, tipoDefinion);
                if (inicializacion != null)
                {
                    var declaracionInic = inicializacion.ValidateSemantic();
                    tipoDefinion = validarAssignacion(tipoDefinion, this, declaracionInic);
                }

            }
            if(declarationList != null)
            foreach (var variable in declarationList)
            {
                var declaracion = variable as DeclaracionVariableNode;
                foreach (var stack in ContenidoStack._StackInstance.Stack)
                    if (stack.VariableExist(declaracion.identificador))
                        throw new SemanticoException(archivo+"ya existe la variable" + declaracion.identificador + "fila" + declaracion.token.Fila +
                    "columna" + declaracion.token.Columna);

                if (declaracion.inicializacion != null)
                {
                    var declaracionInic = declaracion.inicializacion.ValidateSemantic();
                    tipoDefinion = validarAssignacion(tipoDefinion, declaracion, declaracionInic);
                }
                ContenidoStack._StackInstance.Stack.Peek().DeclareVariable(declaracion.identificador, tipoDefinion);
               
                    
            }
            
        }

        public override string GenerarCodigo()
        {
            string valor= "";
            if (identificador != null)
                valor += identificador;
            if (inicializacion != null)
                valor += " = "+inicializacion.GenerarCodigo();
            if (identificador != null && inicializacion == null )
                valor += "= "+this.tipo.GenerarCodigo();
          
            if (declarationList != null && declarationList.Count>0)
            {
                if (identificador != null)
                    valor += ",";
                
                var list = declarationList.ToArray();
                for (int i = 0; i < declarationList.Count; i++)
                {
                    (list[i] as DeclaracionVariableNode).tipo = this.tipo;
                    valor += list[i].GenerarCodigo();
                    if(i< list.Length-1)
                        valor += ",";
                }

            }

            if (declarationList != null)
            {
                
                    valor = valor.Insert(0, "var ");
                    valor += ";";
                
            }
            
            return valor;
        }
        private TiposBases validarAssignacion(TiposBases tipoDefinion, DeclaracionVariableNode declaracion, TiposBases declaracionInic)
        {
            if ((tipoDefinion is ClaseTipo && declaracionInic is ClaseTipo))
            {
                if ((tipoDefinion as ClaseTipo).nombreClase != (declaracionInic as ClaseTipo).nombreClase)
                    throw new SemanticoException(archivo+"se tiene que asignar el mismo tipo " + (tipoDefinion as EnumTipo).nombreClase + "fila"
                        + declaracion.inicializacion.token.Fila + "columna" + declaracion.inicializacion.token.Columna);
            }
            else if ((tipoDefinion is EnumTipo && declaracionInic is EnumTipo))
            {
                if ((tipoDefinion as EnumTipo).nombreClase != (declaracionInic as EnumTipo).nombreClase)
                    throw new SemanticoException(archivo+"se tiene que asignar el mismo tipo " + (tipoDefinion as EnumTipo).nombreClase + "fila"
                        + declaracion.inicializacion.token.Fila + "columna" + declaracion.inicializacion.token.Columna);
            }
            else if ((declaracionInic is ArrayTipo) && (tipoDefinion is ArrayTipo))
            {
                var declarArray = (declaracionInic as ArrayTipo).cantidad.ToArray();
                var tipoDefinionArray = (tipoDefinion as ArrayTipo).cantidad.ToArray();
                if (tipoDefinionArray.Length != declarArray.Length)
                    throw new SemanticoException(archivo+"se tiene que asignar el mismo tipo al arreglo " + declaracion.identificador + "fila"
                        + declaracion.inicializacion.token.Fila + "columna" + declaracion.inicializacion.token.Columna);
                for (int i = 0; i < declarArray.Length; i++)
                {
                    if (tipoDefinionArray[i].Count != declarArray[i].Count)
                        throw new SemanticoException(archivo+"se tiene que asignar el mismo tipo al arreglo " + declaracion.identificador + "fila"
                            + declaracion.inicializacion.token.Fila + "columna" + declaracion.inicializacion.token.Columna);
                }
                if ((declaracionInic as ArrayTipo).tipoArray.GetType() != (tipoDefinion as ArrayTipo).tipoArray.GetType())
                    throw new SemanticoException(archivo+"se no se esta asignando el mismo tipo " + declaracion.identificador + "fila"
                           + declaracion.token.Fila + "columna" + declaracion.token.Columna);
                tipoDefinion = declaracionInic;
            }
            else if (tipoDefinion.GetType() != declaracionInic.GetType() && !(tipoDefinion is VarTipo))
                throw new SemanticoException(archivo+"se tiene que asignar el mismo tipo " + tipoDefinion + "fila" + declaracion.inicializacion.token.Fila +
             "columna" + declaracion.inicializacion.token.Columna);
            else if (tipoDefinion is VarTipo)
                tipoDefinion = declaracionInic;
            return tipoDefinion;
        }
    }
}
