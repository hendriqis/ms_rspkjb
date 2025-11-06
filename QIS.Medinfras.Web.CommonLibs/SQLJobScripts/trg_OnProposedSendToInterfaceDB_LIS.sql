/****** Object:  Trigger [dbo].[trg_OnProposeSendToInterfaceDB_LIS]    Script Date: 02/03/2017 12:33:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO









-- =============================================
-- Author:		MEDINFRAS
-- Create date: 04-06-2016
-- Description:	
-- =============================================
ALTER TRIGGER [dbo].[trg_OnProposeSendToInterfaceDB_LIS] 
   ON  [dbo].[PatientChargesHd] 
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	DECLARE @TransactionID             INT,
			@TransactionCode         VARCHAR(5),
			@TransactionNo           VARCHAR(30), 
	        @old_GCTransactionStatus VARCHAR(20),
            @new_GCTransactionStatus VARCHAR(20)

    -- Insert statements for trigger here
	SELECT @TransactionID           = TransactionID,
	       @TransactionCode         = TransactionCode,
		   @TransactionNo           = TransactionNo, 
		   @old_GCTransactionStatus = GCTransactionStatus
	FROM Deleted

	SELECT @new_GCTransactionStatus = GCTransactionStatus
	FROM Inserted

	--Laboratory Test Order
	IF ((@TransactionCode = '2203') OR (@TransactionCode = '2903')) BEGIN
		IF (@old_GCTransactionStatus = 'X121^001') AND (@new_GCTransactionStatus = 'X121^002') BEGIN
			-- CHECK IF ALREADY SEND TO LIS OR NOT
			DECLARE @IsExist VARCHAR(30) = '',
			        @ORDER_CONTROL VARCHAR(2) = 'NW'

			SELECT @IsExist = ONO FROM [MEDINFRAS_LINK]..LIS_ORDER WHERE ONO =  @TransactionNo

			IF @IsExist <> '' BEGIN
				DELETE [MEDINFRAS_LINK]..LIS_ORDER WHERE ONO = @TransactionNo
			END

			-- PROPOSED
			INSERT [MEDINFRAS_LINK]..LIS_ORDER(ONO,MESSAGE_DT,ORDER_CONTROL,PID,APID,FIRST_NAME,LAST_NAME,PNAME,[ADDRESS],PTYPE,BIRTH_DT,SEX,REQUEST_DT,[SOURCE],CLINICIAN,ROOM_NO,[PRIORITY],COMMENT,VISITNO,ORDER_TEST_ID,ORDER_STATUS,CREATED_BY)
			SELECT a.TransactionNo,	
				   CONVERT(varchar(8),GetDate(),112)+REPLACE(LEFT(CONVERT(varchar(15),GETDATE(),114),8),':',''), 
				   @ORDER_CONTROL,
				   b.MRN,
				   b.MedicalNo,
				   b.FirstName,
				   b.LastName,
				   b.PatientName,
				   SUBSTRING(b.StreetName + ',' + b.City,1,200),
				   CASE 
						WHEN b.DepartmentID = 'INPATIENT'  THEN 'IP'
						WHEN b.DepartmentID = 'OUTPATIENT' THEN 'OP'
						WHEN b.DepartmentID = 'EMERGENCY'  THEN 'ER'
						WHEN b.DepartmentID = 'DIAGNOSTIC' THEN 'MD'
						WHEN b.DepartmentID = 'MCU'        THEN 'MC'
				   END,
				   CONVERT(varchar(8),b.DateOfBirth,112),
				   SUBSTRING(b.GCGender,6,1),
				   CONVERT(varchar(8),a.TestOrderDate,112)+REPLACE(a.TestOrderTime,':','')+'00',
				   b.ServiceUnitCode + '^' + b.ServiceUnitName,
				   CASE 
						WHEN b.DepartmentID <> 'DIAGNOSTIC' THEN ISNULL(c.ParamedicCode,b.ParamedicCode) + '^' + ISNULL(c.ParamedicName,b.ParamedicName)
						ELSE
							CASE WHEN r.ReferrerParamedicID IS NOT NULL THEN (SELECT p.ParamedicCode + '^' + p.FullName FROM ParamedicMaster p WHERE p.ParamedicID = r.ReferrerParamedicID)
							ELSE
								ISNULL(c.ParamedicCode,b.ParamedicCode) + '^' + ISNULL(c.ParamedicName,b.ParamedicName)
						END
				   END,	
				   ISNULL(b.BedCode,''),
				   CASE WHEN ISNULL(c.IsCITO,0) = 1 THEN 'U' ELSE 'R' END,
				   ISNULL(c.Remarks,''),
				   b.RegistrationNo,
				   STUFF((SELECT '~'+dt.ItemCode FROM vPatientChargesDt dt WHERE dt.TransactionID = a.TransactionID FOR XML PATH ('')),1,1,''),
				   '0',
				   'script'
			FROM vPatientChargesHd a
			INNER JOIN vConsultVisit b ON a.VisitID = b.VisitID
			INNER JOIN Registration r ON b.RegistrationID = r.RegistrationID
			LEFT JOIN vTestOrderHd c ON a.TestOrderID = c.TestOrderID AND a.TransactionID = c.ChargesID
			WHERE a.TransactionID = @TransactionID

			-- insert to brigding status
			DECLARE @TrxNo VARCHAR(30)

			SET @TrxNo = ''

			SELECT @TrxNo = TransactionNo
			FROM BridgingStatus
			WHERE TransactionNo = @TransactionNo 

			IF @TrxNo = '' BEGIN
				INSERT BridgingStatus SELECT @TransactionNo,@TransactionID,GETDATE(),0,NULL
			END
		END
	END
END










GO


