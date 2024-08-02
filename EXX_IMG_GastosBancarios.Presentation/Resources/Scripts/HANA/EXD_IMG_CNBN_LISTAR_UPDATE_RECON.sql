CREATE PROCEDURE EXD_IMG_CNBN_LISTAR_UPDATE_RECON
(
	docEntry int,
	lineId int,
	estado varchar(200)
)
as
begin 
	

 UPDATE "@EXD_CBN1"
 SET 
  "U_COD_ESTADO"=estado
 WHERE "DocEntry"=docEntry and "LineId"=lineId;
 
end;