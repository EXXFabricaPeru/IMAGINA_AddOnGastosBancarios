CREATE PROCEDURE EXD_IMG_CNBN_LISTAR_OPERACIONES_PENDIENTES
(
	idSucursal int,
	codCuenta varchar(50),
	fechaDesde date,
	fechaHasta date
)
AS
BEGIN
	select
		ROW_NUMBER()over(order by T0."Sequence") "Id"
		,T0."ExternCode"
		,T1."Glosa"
		,case when T0."DebAmount" > 0 then T0."DebAmount" else T0."CredAmnt" end as "Importe"
		,T1."NroCuenta"
		,TO_VARCHAR(cast(T0."DueDate" as date),'yyyyMMdd') as "DueDate"
		,T0."Ref"
		,case when T0."DebAmount" > 0 then 'D' else 'C' end as "TipoImporte"
		,T0."Sequence"
		,T0."Memo"
	from OBNK T0 left join 
	(
		select  
		TX1."U_COD_OPE_BANC" as "CodOpe",
		TX1."U_GLOSA" as "Glosa",
		TX1."U_NRO_CUENTA" as "NroCuenta",
		TX0."U_COD_CUENTA" as "CodCtaBanco"
		from  "@EXD_OMCB" TX0 inner join "@EXD_MCB1" TX1 on TX0."Code" = TX1."Code"
	) T1 on T0."ExternCode" = T1."CodOpe" and T0."AcctCode" = T1."CodCtaBanco"
	where coalesce("BankMatch",'0') = '0' 
	and T0."AcctCode" = :codCuenta
	and T0."DueDate" between :fechaDesde and :fechaHasta
	and coalesce((select max('Y') from "@EXD_CBN1" TX0 inner join "@EXD_OCBN" TX1 on TX0."DocEntry" = TX1."DocEntry" 
	where TX1."U_COD_CUENTA" = T0."AcctCode" and ifnull(TX0."U_NUM_SECUENCIA",'-1') = T0."Sequence"),'N') = 'N';
END;