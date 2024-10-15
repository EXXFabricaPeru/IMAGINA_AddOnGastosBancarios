CREATE PROCEDURE EXD_IMG_CNBN_LISTAR_OPERACIONES_PENDIENTES
(
	idSucursal int,
	idBanco int,
	codCuenta varchar(50),
	fechaDesde date,
	fechaHasta date
)
AS
BEGIN
	
	--select '123' from dummy;
	WITH TEMP_TABLE
	AS (select distinct
		--ROW_NUMBER()over(order by T0."Sequence") "Id"
		--ROW_NUMBER()over(order by D1."BankCode" asc ,D1."Branch" asc , T0."AcctCode" asc) "Id"
		T0."ExternCode"
		,T1."Glosa"
		,case when T0."DebAmount" > 0 then T0."DebAmount" else T0."CredAmnt" end as "Importe"
		,T1."NroCuenta"
		,TO_VARCHAR(cast(T0."DueDate" as date),'yyyyMMdd') as "DueDate"
		,T0."Ref"
		,case when T0."DebAmount" > 0 then 'D' else 'C' end as "TipoImporte"
		,T0."Sequence"
		,T0."Memo"
		,T1."CodProyecto"
		,T1."CodDim1"
		,T1."CodDim2"
		,T1."CodDim3"
		,T1."CodDim4"
		,T1."CodDim5"
		,T1."CodParPre"
		,T1."NomParPre"
		,T1."CodMPTraBan"
		,D1."BankCode"--banco
		,DC."BankName"--desde bacno
		,D1."Branch"--banco
		,BL."BPLName"--des empresa
		,T0."AcctCode" --cuenta
		,D1."Account" --banco
		,CT."ActCurr"--banco	
		,T0."AcctCode"||T0."Sequence" as "ID"	
	from OBNK T0 
	INNER join 
	(
		select  distinct
		CASE WHEN IFNULL(TX1."U_COD_OPE_BANC",'')='' THEN TX1."U_GLOSA" ELSE TX1."U_COD_OPE_BANC" END as "CodOpe2",
		TX1."U_COD_OPE_BANC" as "CodOpe",
		TX1."U_GLOSA" as "Glosa",
		--TX1."U_GLOSA" as "Glosa2",
		TX1."U_NRO_CUENTA" as "NroCuenta",
		TX1."U_COD_CUENTA" as "CodCtaBanco",
		TX1."U_COD_PROYECTO"	as "CodProyecto",
		TX1."U_COD_DIM1"		as "CodDim1",
		TX1."U_COD_DIM2"		as "CodDim2",
		TX1."U_COD_DIM3"		as "CodDim3",
		TX1."U_COD_DIM4"		as "CodDim4",
		TX1."U_COD_DIM5"		as "CodDim5",
		TX1."U_COD_PARPRE"		as "CodParPre",
		TX1."U_NOM_PARPRE"		as "NomParPre",
		TX1."U_COD_MPTRABAN"	as "CodMPTraBan",
		CASE WHEN IFNULL(TX1."U_COD_OPE_BANC",'') ='' THEN 1 ELSE 2 END "Match"
		from  --"@EXD_OMCB" TX0 inner join 
		"@EXD_MCB1" TX1 --on TX0."Code" = TX1."Code"
		where IFNULL(TX1."U_CREAR_CONTA",'')='Y' --AND
	) T1 on ( T1."Match"=2 AND  IFNULL(T1."CodOpe",'') = T0."ExternCode" )	AND T0."AcctCode" = T1."CodCtaBanco"
	/*OR (T0."Memo" like '%'||T1."CodOpe2"||'%' oR T0."Memo2" like '%'||T1."CodOpe2"||'%'))
	and*/
	---(T1."CodOpe" = T0."ExternCode" OR (T0."Memo" like '%'||T1."CodOpe"||'%' oR T0."Memo2" like '%'||T1."CodOpe"||'%')) and
	INNER JOIN DSC1 D1 On D1."GLAccount"=T0."AcctCode" 
	INNER JOIN OBPL BL ON Bl."BPLId"= D1."Branch" --select "BPLId","BPLName" from OBPL
 	INNER join OACT CT on D1."GLAccount" =CT."AcctCode"  
 	INNER join ODSC DC on DC."BankCode"= D1."BankCode"
 	LEFT JOIN "@EXD_CBN1" CB1 on 
 			CB1."U_NUM_SECUENCIA"=T0."Sequence" AND 
 			D1."BankCode"=CB1."U_COD_BANCO" AND 
 			T0."Ref"=CB1."U_NUM_OPERACION" AND
 			T0."Memo"=CB1."U_INFO_DETALLADA" AND
 			T0."DueDate"=CB1."U_FECHA_OPERACION" AND
 			CASE WHEN T0."DebAmount">0 THEN T0."DebAmount" ELSE T0."CredAmnt" END =CB1."U_IMPORTE"
 	
	where 
	coalesce("BankMatch",'0') = '0' and 
	( :idBanco is null or  DC."BankCode" = :idBanco ) and 
	( :codCuenta is null or  T0."AcctCode" = :codCuenta) AND 
	( :idSucursal is null or  D1."Branch" = :idSucursal) and 
	T0."DueDate" between :fechaDesde and :fechaHasta
	and 
	(
		coalesce(
		(	select max('Y') from "@EXD_CBN1" TX0 inner join "@EXD_OCBN" TX1 on TX0."DocEntry" = TX1."DocEntry" 
				where 
					TX0."U_COD_CUENTA" = T0."AcctCode"  and 
					ifnull(TX0."U_NUM_SECUENCIA",'-1') = T0."Sequence" AND
					T0."Ref"=TX0."U_NUM_OPERACION" AND
 					T0."Memo"=TX0."U_INFO_DETALLADA" AND
 					T0."DueDate"=TX0."U_FECHA_OPERACION" AND
 					CASE WHEN T0."DebAmount">0 THEN T0."DebAmount" ELSE T0."CredAmnt" END =TX0."U_IMPORTE" AND
					IFNULL(TX0."U_COD_ESTADO",'')='Reconciliado'
			),'N') = 'N' --AND
			--T0."Ref"=CB1."U_NUM_OPERACION"  
		or 
		(
		IFNULL("U_COD_ESTADO",'')='Reconciliado' AND 
		IFNULL(T0."BankMatch",0)=0 and 
		(
		(select count(*) from "OVPM" OV 
				where OV."DocEntry" = IFNULL(CB1."U_COD_PAGO_SAP",0) and OV."Canceled"='Y' and CB1."U_TIPO_PAGO"=46 ) >0 
				OR
			(select count(*) from "ORCT" OV 
				where OV."DocEntry" = IFNULL(CB1."U_COD_PAGO_SAP",0) and OV."Canceled"='Y' and CB1."U_TIPO_PAGO"=24 ) >0
		)
		--(select count(*) from "OVPM" OV 
		--	where OV."DocEntry" = IFNULL(CB1."U_COD_PAGO_SAP",0) and OV."Canceled"='Y' and CB1."U_TIPO_PAGO"=46 ) >0 
	 	)
	)
			
					
--	order by 1;
	
	
	UNION  all
	
		select distinct
		--ROW_NUMBER()over(order by T0."Sequence") "Id"
		--ROW_NUMBER()over(order by D1."BankCode" asc ,D1."Branch" asc , T0."AcctCode" asc) "Id"
		T0."ExternCode"
		,T1."Glosa"
		,case when T0."DebAmount" > 0 then T0."DebAmount" else T0."CredAmnt" end as "Importe"
		,T1."NroCuenta"
		,TO_VARCHAR(cast(T0."DueDate" as date),'yyyyMMdd') as "DueDate"
		,T0."Ref"
		,case when T0."DebAmount" > 0 then 'D' else 'C' end as "TipoImporte"
		,T0."Sequence"
		,T0."Memo"
		,T1."CodProyecto"
		,T1."CodDim1"
		,T1."CodDim2"
		,T1."CodDim3"
		,T1."CodDim4"
		,T1."CodDim5"
		,T1."CodParPre"
		,T1."NomParPre"
		,T1."CodMPTraBan"
		,D1."BankCode"--banco
		,DC."BankName"--desde bacno
		,D1."Branch"--banco
		,BL."BPLName"--des empresa
		,T0."AcctCode" --cuenta
		,D1."Account" --banco
		,CT."ActCurr"--banco
		,T0."AcctCode"||T0."Sequence" as "ID"			
	from OBNK T0 
	INNER join 
	(
		select  distinct
		CASE WHEN IFNULL(TX1."U_COD_OPE_BANC",'')='' THEN TX1."U_GLOSA" ELSE TX1."U_COD_OPE_BANC" END as "CodOpe2",
		TX1."U_COD_OPE_BANC" as "CodOpe",
		TX1."U_GLOSA" as "Glosa",
		--TX1."U_GLOSA" as "Glosa2",
		TX1."U_NRO_CUENTA" as "NroCuenta",
		TX1."U_COD_CUENTA" as "CodCtaBanco",
		TX1."U_COD_PROYECTO"	as "CodProyecto",
		TX1."U_COD_DIM1"		as "CodDim1",
		TX1."U_COD_DIM2"		as "CodDim2",
		TX1."U_COD_DIM3"		as "CodDim3",
		TX1."U_COD_DIM4"		as "CodDim4",
		TX1."U_COD_DIM5"		as "CodDim5",
		TX1."U_COD_PARPRE"		as "CodParPre",
		TX1."U_NOM_PARPRE"		as "NomParPre",
		TX1."U_COD_MPTRABAN"	as "CodMPTraBan",
		CASE WHEN IFNULL(TX1."U_COD_OPE_BANC",'') ='' THEN 1 ELSE 2 END "Match"
		from  --"@EXD_OMCB" TX0 inner join 
		"@EXD_MCB1" TX1 --on TX0."Code" = TX1."Code"
		where IFNULL(TX1."U_CREAR_CONTA",'')='Y' --AND
	) T1 on 
	
	
	(T1."Match"=1 AND (IFNULL(T0."Memo",'') like '%'||IFNULL(T1."Glosa",'')||'%' 
	--oR IFNULL(T0."Memo2",'')like '%'||IFNULL(T1."Glosa",'')||'%'
)
	)
	 and

	---(T1."CodOpe" = T0."ExternCode" OR (T0."Memo" like '%'||T1."CodOpe"||'%' oR T0."Memo2" like '%'||T1."CodOpe"||'%')) and
	T0."AcctCode" = T1."CodCtaBanco"
	INNER JOIN DSC1 D1 On D1."GLAccount"=T0."AcctCode" 
	INNER JOIN OBPL BL ON Bl."BPLId"= D1."Branch" --select "BPLId","BPLName" from OBPL
 	INNER join OACT CT on D1."GLAccount" =CT."AcctCode"  
 	INNER join ODSC DC on DC."BankCode"= D1."BankCode"
 	LEFT JOIN "@EXD_CBN1" CB1 on CB1."U_NUM_SECUENCIA"=T0."Sequence" AND D1."BankCode"=CB1."U_COD_BANCO"
	where 
	coalesce("BankMatch",'0') = '0' and 
	( :idBanco is null or  DC."BankCode" = :idBanco )
	and ( :codCuenta is null or  T0."AcctCode" = :codCuenta)
AND ( :idSucursal is null or  D1."Branch" = :idSucursal)
	
	and T0."DueDate" between :fechaDesde and :fechaHasta
	and 
	(
	coalesce(
	(	select max('Y') from "@EXD_CBN1" TX0 inner join "@EXD_OCBN" TX1 on TX0."DocEntry" = TX1."DocEntry" 
				where 
					TX0."U_COD_CUENTA" = T0."AcctCode"  and 
					ifnull(TX0."U_NUM_SECUENCIA",'-1') = T0."Sequence" AND
					T0."Ref"=TX0."U_NUM_OPERACION" AND
 					T0."Memo"=TX0."U_INFO_DETALLADA" AND
 					T0."DueDate"=TX0."U_FECHA_OPERACION" AND
 					CASE WHEN T0."DebAmount">0 THEN T0."DebAmount" ELSE T0."CredAmnt" END =TX0."U_IMPORTE" AND
					IFNULL(TX0."U_COD_ESTADO",'')='Reconciliado'
			),'N') = 'N' --AND
	
	OR 
	(IFNULL("U_COD_ESTADO",'')='Reconciliado' AND IFNULL(T0."BankMatch",0)=0
	 and 
	-- (select count(*) from "OVPM" OV where OV."DocEntry" = CB1."U_COD_PAGO_SAP" 
	--and OV."Canceled"='Y' ) >0)
	(select count(*) from "OVPM" OV 
				where OV."DocEntry" = IFNULL(CB1."U_COD_PAGO_SAP",0) and OV."Canceled"='Y' and CB1."U_TIPO_PAGO"=46 ) >0 
				OR
			(select count(*) from "ORCT" OV 
				where OV."DocEntry" = IFNULL(CB1."U_COD_PAGO_SAP",0) and OV."Canceled"='Y' and CB1."U_TIPO_PAGO"=24 ) >0
		)
	
	)
		and IFNULL(T1."CodOpe",'') =''
	and IFNULL(T0."ExternCode",'') =''
	)
	--select * from TEMP_TABLE
	, TEMP_TABLE2 as (
			select distinct
		--ROW_NUMBER()over(order by T0."Sequence") "Id"
		--ROW_NUMBER()over(order by D1."BankCode" asc ,D1."Branch" asc , T0."AcctCode" asc) "Id"
		T0."ExternCode"
		,T1."Glosa"
		,case when T0."DebAmount" > 0 then T0."DebAmount" else T0."CredAmnt" end as "Importe"
		,T1."NroCuenta"
		,TO_VARCHAR(cast(T0."DueDate" as date),'yyyyMMdd') as "DueDate"
		,T0."Ref"
		,case when T0."DebAmount" > 0 then 'D' else 'C' end as "TipoImporte"
		,T0."Sequence"
		,T0."Memo"
		,T1."CodProyecto"
		,T1."CodDim1"
		,T1."CodDim2"
		,T1."CodDim3"
		,T1."CodDim4"
		,T1."CodDim5"
		,T1."CodParPre"
		,T1."NomParPre"
		,T1."CodMPTraBan"
		,D1."BankCode"--banco
		,DC."BankName"--desde bacno
		,D1."Branch"--banco
		,BL."BPLName"--des empresa
		,T0."AcctCode" --cuenta
		,D1."Account" --banco
		,CT."ActCurr"--banco
		,T0."AcctCode"||T0."Sequence" as "ID"			
	from OBNK T0 
	INNER join 
	(
		select  distinct
		CASE WHEN IFNULL(TX1."U_COD_OPE_BANC",'')='' THEN TX1."U_GLOSA" ELSE TX1."U_COD_OPE_BANC" END as "CodOpe2",
		TX1."U_COD_OPE_BANC" as "CodOpe",
		TX1."U_GLOSA" as "Glosa",
		--TX1."U_GLOSA" as "Glosa2",
		TX1."U_NRO_CUENTA" as "NroCuenta",
		TX1."U_COD_CUENTA" as "CodCtaBanco",
		TX1."U_COD_PROYECTO"	as "CodProyecto",
		TX1."U_COD_DIM1"		as "CodDim1",
		TX1."U_COD_DIM2"		as "CodDim2",
		TX1."U_COD_DIM3"		as "CodDim3",
		TX1."U_COD_DIM4"		as "CodDim4",
		TX1."U_COD_DIM5"		as "CodDim5",
		TX1."U_COD_PARPRE"		as "CodParPre",
		TX1."U_NOM_PARPRE"		as "NomParPre",
		TX1."U_COD_MPTRABAN"	as "CodMPTraBan",
		CASE WHEN IFNULL(TX1."U_COD_OPE_BANC",'') ='' THEN 1 ELSE 2 END "Match"
		from  --"@EXD_OMCB" TX0 inner join 
		"@EXD_MCB1" TX1 --on TX0."Code" = TX1."Code"
		where IFNULL(TX1."U_CREAR_CONTA",'')='Y' --AND
	) T1 on 
	
	
	(T1."Match"=1 AND (IFNULL(T0."Memo2",'') like '%'||IFNULL(T1."Glosa",'')||'%' 
	--oR IFNULL(T0."Memo2",'')like '%'||IFNULL(T1."Glosa",'')||'%'
)
	)
	 and

	---(T1."CodOpe" = T0."ExternCode" OR (T0."Memo" like '%'||T1."CodOpe"||'%' oR T0."Memo2" like '%'||T1."CodOpe"||'%')) and
	T0."AcctCode" = T1."CodCtaBanco"
	INNER JOIN DSC1 D1 On D1."GLAccount"=T0."AcctCode" 
	INNER JOIN OBPL BL ON Bl."BPLId"= D1."Branch" --select "BPLId","BPLName" from OBPL
 	INNER join OACT CT on D1."GLAccount" =CT."AcctCode"  
 	INNER join ODSC DC on DC."BankCode"= D1."BankCode"
 	LEFT JOIN "@EXD_CBN1" CB1 on CB1."U_NUM_SECUENCIA"=T0."Sequence" AND D1."BankCode"=CB1."U_COD_BANCO"
	where 
	coalesce("BankMatch",'0') = '0' and 
	( :idBanco is null or  DC."BankCode" = :idBanco )
	and ( :codCuenta is null or  T0."AcctCode" = :codCuenta)
AND ( :idSucursal is null or  D1."Branch" = :idSucursal)
	
	and T0."DueDate" between :fechaDesde and :fechaHasta
	and 
	(
	coalesce((	select max('Y') from "@EXD_CBN1" TX0 inner join "@EXD_OCBN" TX1 on TX0."DocEntry" = TX1."DocEntry" 
				where 
					TX0."U_COD_CUENTA" = T0."AcctCode"  and 
					ifnull(TX0."U_NUM_SECUENCIA",'-1') = T0."Sequence" AND
					T0."Ref"=TX0."U_NUM_OPERACION" AND
 					T0."Memo"=TX0."U_INFO_DETALLADA" AND
 					T0."DueDate"=TX0."U_FECHA_OPERACION" AND
 					CASE WHEN T0."DebAmount">0 THEN T0."DebAmount" ELSE T0."CredAmnt" END =TX0."U_IMPORTE" AND
					IFNULL(TX0."U_COD_ESTADO",'')='Reconciliado'
			),'N') = 'N' --AND
	
	OR 
	(IFNULL("U_COD_ESTADO",'')='Reconciliado' AND IFNULL(T0."BankMatch",0)=0
	 and 
	 --(select count(*) from "OVPM" OV where OV."DocEntry" = CB1."U_COD_PAGO_SAP" 
	--and OV."Canceled"='Y' ) >0)
	(select count(*) from "OVPM" OV 
				where OV."DocEntry" = IFNULL(CB1."U_COD_PAGO_SAP",0) and OV."Canceled"='Y' and CB1."U_TIPO_PAGO"=46 ) >0 
				OR
			(select count(*) from "ORCT" OV 
				where OV."DocEntry" = IFNULL(CB1."U_COD_PAGO_SAP",0) and OV."Canceled"='Y' and CB1."U_TIPO_PAGO"=24 ) >0
		)
	)
		and IFNULL(T1."CodOpe",'') =''
	and IFNULL(T0."ExternCode",'') =''
	)
	,TEMP_TABLE3 AS(
	
	select --ROW_NUMBER()over(order by "BankCode" asc ,"Branch" asc , "AcctCode" asc) "Id",
	* from TEMP_TABLE
	UNION ALL 
	select --ROW_NUMBER()over(order by "BankCode" asc ,"Branch" asc , "AcctCode" asc) "Id",
	* from TEMP_TABLE2
	WHERE "ID" NOT IN(SELECT "ID" FROM TEMP_TABLE))
	
	select ROW_NUMBER()over(order by "BankCode" asc ,"Branch" asc , "AcctCode" asc) "Id",
	* from TEMP_TABLE3
	order by 1
;
	

           
END;