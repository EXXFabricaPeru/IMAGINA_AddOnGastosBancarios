CREATE PROCEDURE EXD_IMG_CNBN_LISTAR_DATOS_PAGOS
(
	docEntry int
)
as
begin 
	


	select T0.*,TO_VARCHAR(T1."U_FECHA_CONT",'yyyyMMdd') as "U_FECHA_CONT"
	from "@EXD_CBN1"	T0 
	left join "@EXD_OCBN"	T1	on T0."DocEntry"= T1."DocEntry"
	where T1."DocEntry" = docEntry
	and ifnull(T0."U_SELECCIONADO",'') = 'Y'
	--and T0."U_COD_ESTADO"='Procesado'
	AND IFNULL(T0."U_COD_PAGO_SAP",0)=0
	;
end;