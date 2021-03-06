﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores
{
    public enum TokenTipos
    {
        EndOfFile,
        Asignacion,
        FinalDeSentencia,
        Separador,
        Negacion,
        YPorBit,
        OPorBit,
        NegacionPorBit,
        OExclusivoPorBit,
        ParentesisIzquierdo,
        ParentesisDerecho,
        CorcheteIzquierdo,
        CorcheteDerecho,
        LlaveIzquierdo,
        LlaveDerecho,
        Directiva,
        Punto,
        DosPuntos,
        OperacionMultiplicacion,
        OperacionSuma,
        OperacionResta,
        OperacionDivision,
        OperacionDivisionResiduo,
        RelacionalMenor,
        RelacionalMayor,
        AutoOperacionSuma,
        AutoOperacionResta,
        AutoOperacionMultiplicacion,
        AutoOperacionDivision,
        LogicaStruct,
        AutoOperacionIncremento,
        AutoOperacionDecremento,
        RelacionalMayorOIgual,
        RelacionalMenorOIgual,
        RelacionalIgual,
        RelacionalNoIgual,
        LogicosY,
        LogicosO,
        AutoDeplazamientoDerecha,
        PalabraReservadaAbstract,
        PalabraReservadaAs,
        PalabraReservadaBase,
        PalabraReservadaBool,
        PalabraReservadaBreak,
        PalabraReservadaByte,
        PalabraReservadaCase,
        PalabraReservadaCatch,
        PalabraReservadaChar,
        PalabraReservadaChecked,
        PalabraReservadaClase,
        PalabraReservadaConst,
        PalabraReservadaContinue,
        PalabraReservadaDecimal,
        PalabraReservadaDefault,
        PalabraReservadaDelegado,
        PalabraReservadaDo,
        PalabraReservadaDouble,
        PalabraReservadaElse,
        PalabraReservadaEnum,
        PalabraReservadaEvent,
        PalabraReservadaExplicit,
        PalabraReservadaExtern,
        PalabraReservadaFalse,
        PalabraReservadaFinally,
        PalabraReservadaFixed,
        PalabraReservadaFloat,
        PalabraReservadaFor,
        PalabraReservadaForeach,
        PalabraReservadaGoto,
        PalabraReservadaIf,
        PalabraReservadaImplicit,
        PalabraReservadaIn,
        PalabraReservadaInt,
        PalabraReservadaInterfaz,
        PalabraReservadaInternal,
        PalabraReservadaEs,
        PalabraReservadaBloquear,
        PalabraReservadaLong,
        PalabraReservadaNew,
        PalabraReservadaNull,
        PalabraReservadaOverride,
        PalabraReservadaParams,
        PalabraReservadaPrivate,
        PalabraReservadaProtected,
        PalabraReservadaPublic,
        PalabraReservadaReadonly,
        PalabraReservadaRef,
        PalabraReservadaReturn,
        PalabraReservadaSbyte,
        PalabraReservadaSealed,
        PalabraReservadaShort,
        PalabraReservadaSizeof,
        PalabraReservadaStackalloc,
        PalabraReservadaStatic,
        PalabraReservadaString,
        PalabraReservadaStruct,
        PalabraReservadaSwitch,
        PalabraReservadaThis,
        PalabraReservadaThrow,
        PalabraReservadaTrue,
        PalabraReservadaTry,
        PalabraReservadaTypeof,
        PalabraReservadaUint,
        PalabraReservadaUlong,
        PalabraReservadaUnchecked,
        PalabraReservadaUnsafe,
        PalabraReservadaUshort,
        PalabraReservadaUsing,
        PalabraReservadaVoid,
        PalabraReservadaVolatile,
        PalabraReservadaWhile,
        LiteralString,
        LiteralChar,
        NumeroDouble,
        NumeroFloat,
        NumeroSinSigno,
        NumeroLong,
        NumeroULong,
        NumeroDecimal,
        LiteralDate,
        NumeroOctal,
        NumeroBinario,
        NumeroHexagecimal,
        Numero,
        Identificador,
        NoEsNull,
        Interogacion,
        MiembroCondicionales,
        PalabraReservadaNamespace,
        DeplazamientoIzquierda,
        DeplazamientoDerecha,
        AutoOperacionResiduo,
        AutoOperacionAndPorBit,
        AutoOperacionOrPorBit,
        AutoOperacionOExclusivoPorBit,
        AutoDeplazamientoIzquierda,
        PalabraReservadaVirtual,
        PalabraReservadaVar,
        LogicoO,
        PalabraReservadaIs,


    }
}
