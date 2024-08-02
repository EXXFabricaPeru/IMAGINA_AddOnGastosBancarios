CREATE PROCEDURE EXD_IMG_CNBN_LISTAR_UPDATE_PAGOS
(
	docEntry int,
	lineId int,
	codPago int,
	tipoPago int,
	estado varchar(200)
)
as
begin 
	

 UPDATE "@EXD_CBN1"
 SET "U_COD_PAGO_SAP"= codPago,
 "U_TIPO_PAGO"=tipoPago,
  "U_COD_ESTADO"=estado
 WHERE "DocEntry"=docEntry and "LineId"=lineId;
 
end;