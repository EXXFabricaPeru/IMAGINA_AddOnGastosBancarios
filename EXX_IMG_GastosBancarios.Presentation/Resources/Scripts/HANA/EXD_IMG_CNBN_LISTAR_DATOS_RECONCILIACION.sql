CREATE PROCEDURE EXD_IMG_CNBN_LISTAR_DATOS_RECONCILIACION
(
	docEntry int
)
as
begin 
	
	with CTE_PAGOS
	as
	(
		select 
			T0."DocEntry",T0."ObjType",T0."TransId", T2."Line_ID",T2."Account"
		from OVPM T0 
		inner join OJDT T1 on T0."TransId" = T1."TransId"
		inner join JDT1 T2 on T2."TransId" = T1."TransId"

		union all

		select 
			T0."DocEntry",T0."ObjType",T0."TransId", T2."Line_ID",T2."Account"
		from ORCT T0 
		inner join OJDT T1 on T0."TransId" = T1."TransId"
		inner join JDT1 T2 on T2."TransId" = T1."TransId"
	)
	select 
		T2."TransId",
		T2."Line_ID" ,
		T0."U_COD_CUENTA",
		T0."U_NUM_SECUENCIA",
		T0."LineId",
		T0."U_COD_BANCO",
		T0."U_COD_ESTADO",
		T0."U_COD_EMPRESA",
		T0."U_COD_CUENTA"
	from "@EXD_CBN1"		T0 
	inner join "@EXD_OCBN"	T1	on T0."DocEntry"		= T1."DocEntry"
	inner join CTE_PAGOS	T2  on T2."DocEntry"		= T0."U_COD_PAGO_SAP"	and T2."ObjType" = T0."U_TIPO_PAGO" and T0."U_COD_CUENTA" = T2."Account"
	where T1."DocEntry" = docEntry
	and ifnull(T0."U_SELECCIONADO",'') = 'Y'
	--and T0."U_COD_ESTADO"='Procesado'
	and IFNULL(T0."U_COD_PAGO_SAP",0) != 0
	;
end;