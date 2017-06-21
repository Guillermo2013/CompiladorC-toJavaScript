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
        public ExpressionNode inicializacion;
        public List<StatementNode> declarationList;
        public override void ValidateSemantic()
        {
            var tipoDefinion = tipo.ValidateSemantic();
            if(tipoDefinion is  VarTipo && declarationList.Count>1)
                throw new SemanticoException("no se permite multiples declaraciones " + tipo + "fila" + tipo.token.Fila +
                   "columna" + tipo.token.Columna);

            if (identificador != null && identificador != "")
            {
                ContenidoStack._StackInstance.Stack.Peek().DeclareVariable(identificador, tipoDefinion);
                if (inicializacion != null)
                {
                    var inicializacionTipo = inicializacion.ValidateSemantic();
                    if(tipoDefinion.GetType() != inicializacionTipo.GetType())
                        throw new SemanticoException("no se permite asignar declaraciones " + inicializacionTipo + "a  un tipo" + tipoDefinion
                            + "fila" + inicializacion.token.Fila + "columna" + inicializacion.token.Columna);
                }

            }
            foreach (var variable in declarationList)
            {
                var declaracion = variable as DeclaracionVariableNode;
                foreach (var stack in ContenidoStack._StackInstance.Stack)
                    if (stack.VariableExist(declaracion.identificador))
                        throw new SemanticoException("ya existe la variable" + declaracion.identificador + "fila" + declaracion.token.Fila +
                    "columna" + declaracion.token.Columna);

                if (declaracion.inicializacion != null)
                {
                    var declaracionInic = declaracion.inicializacion.ValidateSemantic();
                    if ((tipoDefinion is ClaseTipo && declaracionInic is ClaseTipo) )
                    {
                        if ((tipoDefinion as ClaseTipo).nombreClase != (declaracionInic as ClaseTipo).nombreClase)
                            throw new SemanticoException("se tiene que asignar el mismo tipo " + (tipoDefinion as EnumTipo).nombreClase + "fila"
                                + declaracion.inicializacion.token.Fila + "columna" + declaracion.inicializacion.token.Columna);
                    }
                    else if ((tipoDefinion is EnumTipo && declaracionInic is EnumTipo))
                    {
                        if((tipoDefinion as EnumTipo).nombreClase != (declaracionInic as EnumTipo).nombreClase)
                            throw new SemanticoException("se tiene que asignar el mismo tipo " + (tipoDefinion as EnumTipo).nombreClase + "fila" 
                                + declaracion.inicializacion.token.Fila + "columna" + declaracion.inicializacion.token.Columna);
                    }
                    else if (tipoDefinion.GetType() != declaracionInic.GetType() && !(tipoDefinion is VarTipo))
                        throw new SemanticoException("se tiene que asignar el mismo tipo " + tipoDefinion + "fila" + declaracion.inicializacion.token.Fila +
                     "columna" + declaracion.inicializacion.token.Columna);
                    else if (tipoDefinion is VarTipo)
                        tipoDefinion = declaracionInic;
                }
                ContenidoStack._StackInstance.Stack.Peek().DeclareVariable(declaracion.identificador, tipoDefinion);
               
                    
            }
            
        }
    }
}
