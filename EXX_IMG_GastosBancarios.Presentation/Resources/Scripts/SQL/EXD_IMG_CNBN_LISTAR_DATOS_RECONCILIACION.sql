CREATE PROCEDURE [dbo].[EXD_IMG_CNBN_LISTAR_DATOS_RECONCILIACION] --'7'
@docEntry int
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
		T1."U_COD_CUENTA",
		T0."U_NUM_SECUENCIA"
	from "@EXD_CBN1"		T0 
	inner join "@EXD_OCBN"	T1	on T0."DocEntry"		= T1."DocEntry"
	inner join CTE_PAGOS	T2  on T2.DocEntry			= T0."U_COD_PAGO_SAP"	and T2."ObjType" = T0."U_TIPO_PAGO" and T1."U_COD_CUENTA" = T2."Account"
	where T1."DocEntry" = @docEntry and isnull(T0."U_SELECCIONADO",'') = 'Y'
end;