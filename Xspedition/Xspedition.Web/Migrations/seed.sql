DELETE FROM CaTypeFieldMap
DELETE FROM OptionTypeFieldMap
DELETE FROM PayoutTypeFieldMap

DELETE FROM CaTypeRegistry
DELETE FROM OptionTypeRegistry
DELETE FROM PayoutTypeRegistry
DELETE FROM FieldRegistry

DELETE FROM CaTypeDateConfiguration
DELETE FROM CaTimeline

DELETE FROM ScrubbingInfo
DELETE FROM NotificationInfo
DELETE FROM InstructionInfo
DELETE FROM ResponseInfo
DELETE FROM PaymentInfo

SET IDENTITY_INSERT CaTypeRegistry ON
--INSERT CATYPERegistry
INSERT INTO [dbo].[CaTypeRegistry]
           ([CaTypeRegistryId], [Code])
     VALUES
           (1, 'DRIP'), (2, 'DVOP'), (3, 'PINK'), (4, 'TEDA'),
		   (5, 'PCAL'), (6, 'RHTS'), (7, 'TEND'), (8, 'INTR'),
		   (9, 'DVCA'), (10, 'MEV')
SET IDENTITY_INSERT CaTypeRegistry OFF


SET IDENTITY_INSERT OptionTypeRegistry ON
--INSERT OPTIONTYPERegistry
INSERT INTO [dbo].[OptionTypeRegistry]
           ([OptionTypeRegistryId], [Code])
     VALUES
           (1, 'Cash'), (2, 'Security'), (3, 'Cash and Security'), (4, 'Exercise'),
		   (5, 'No Action')
SET IDENTITY_INSERT OptionTypeRegistry OFF


SET IDENTITY_INSERT PayoutTypeRegistry ON
--INSERT PAYOUTTYPERegistry
INSERT INTO [dbo].[PayoutTypeRegistry]
           ([PayoutTypeRegistryId], [Code])
     VALUES
           (1, 'Principal Cash'), (2, 'Security'), (3, 'Interest'), (4, 'Dividend')
SET IDENTITY_INSERT PayoutTypeRegistry OFF


SET IDENTITY_INSERT FieldRegistry ON
--INSERT FIELDRegistry
INSERT INTO [dbo].[FieldRegistry]
           ([FieldRegistryId]
		   ,[FieldDisplay]
           ,[FieldType])
     VALUES
			--CA Polja
           (1,'Announcement Date','DATE'),
		   (2,'Base Denomination','NUMBER'),
		   (3,'COAF','STRING'),
		   (4,'Effective Date','DATE'),
		   (5,'Ex Date','DATE'),
		   (6,'Interest Period','STRING'),
		   (7,'Lottery Date','DATE'),
		   (8,'Offeror','STRING'),
		   (9,'Publication Date','DATE'),
		   (10,'Record Date','DATE'),
			(11,'Interest Payable Date','DATE'),
			(12,'Redemption Date','DATE'),
			(13,'Price Type','STRING'),
			(14,'Minimum Price','NUMBER'),
			(15,'Maximum Price','NUMBER'),
			(16,'Increment','NUMBER'),
			
		   --Option polja
		   (101,'Expiration Date','DATE'),
		   (102,'Market date','DATE'),
		   (103,'Minimum Quantity To Instruct','NUMBER'),
		   (104,'Option Active','BOOL'),
		   (105,'Proration Rate','NUMBER'),
		   (106,'Response Due Date','DATE'),
		   (107,'Subscription Date','DATE'),
		   (108,'Option Currency','STRING'),

		   --Payout
		   (1001,'Currency','STRING'),
		   (1002,'Fractional Share Rule','NUMBER'),
		   (1003,'Gross Amount','NUMBER'),
		   (1004,'Interest Rate','NUMBER'),
		   (1005,'Net Amount','NUMBER'),
		   (1006,'New Shares','NUMBER'),
		   (1007,'Old Shares','NUMBER'),
		   (1008,'Payable Date','DATE'),
		   (1009,'Payment Date','DATE'),
		   (1010,'Payout Security ID','STRING'),
		   (1011,'Payout Security ID Type','STRING'),
		   (1012,'Price','NUMBER'),
		   (1013,'Rate Type','STRING'),
		   (1014,'Value Date','DATE'),
		   (1015,'Withholding Tax Rate','NUMBER')
SET IDENTITY_INSERT FieldRegistry OFF

/*
-- Corporate Action polja po CA TIPOVIMA 

	INSERT INTO [dbo].[CaTypeFieldMap] 
		([CaTypeRegistryId],[FieldRegistryId])
    VALUES
		(<CaTypeRegistryId, int,>, <FieldRegistryId, int,>),  --Primer (1, 2)   => 1 je corporate action type iz tabele [CaTypeRegistry], 2 je id polja iz [FieldRegistry] tabele 
	
		*/
	
		
	INSERT INTO [dbo].[CaTypeFieldMap]
		([CaTypeRegistryId],[FieldRegistryId])
     VALUES
		(1,1),
		(1,3),
		(1,5),
		(1,10),
		(2,1),
		(2,3),
		(2,5),
		(2,10),		
		(3,1),
		(3,3),
		(3,10),
		(4,3),
		(4,4),
		(4,8),
		(4,9),
		(4,13),
		(4,14),
		(4,15),
		(4,16),
		(5,2),
		(5,9),
		(5,10),
		(5,12),
		(5,7),
		(5,3),
		(6,3),
		(6,10),
		(6,9),
		(7,3),
		(7,4),
		(7,8),
		(7,9),
		(8,1),
		(8,6),
		(8,10),
		(8,11),
		(8,3),
		(9,1),
		(9,3),
		(9,5),
		(9,10),
		(10,3),
		(10,4),
		(10,8),
		(10,9)	


--Option polja po OPTION TIPOVIMA

/*
INSERT INTO [dbo].[OptionTypeFieldMap]
           ([OptionTypeRegistryId],[FieldRegistryId])
     VALUES
           (<OptionTypeRegistryId, int,>,<FieldRegistryId, int,>),
*/

INSERT INTO [dbo].[OptionTypeFieldMap]
           ([OptionTypeRegistryId],[FieldRegistryId])
     VALUES
		(1,101),
		(1,102),
		(1,103),
		(1,104),
		(1,106),
		(1,108),
		(2,101),
		(2,102),
		(2,103),
		(2,104),
		(2,105),
		(2,106),
		(2,107),
		(3,101),
		(3,102),
		(3,107),
		(3,104),
		(3,106),
		(4,101),
		(4,102),
		(4,103),
		(4,104),
		(4,106),
		(4,107),
		(5,101),
		(5,104)
		

--Payout polja po PAYOUT TIPOVIMA

/*
INSERT INTO [dbo].[PayoutTypeFieldMap]
           ([PayoutTypeRegistryId],[FieldRegistryId])
     VALUES
           (<PayoutTypeRegistryId, int,>,<FieldRegistryId, int,>),
*/

INSERT INTO [dbo].[PayoutTypeFieldMap]
           ([PayoutTypeRegistryId],[FieldRegistryId])
     VALUES
		(1,1001),
		(1,1003),
		(1,1005),
		(1,1008),
		(1,1009),
		(1,1014),
		(2,1002),
		(2,1006),
		(2,1007),
		(2,1008),
		(2,1010),
		(2,1011),
		(2,1013),
		(3,1001),
		(3,1003),
		(3,1004),
		(3,1005),
		(3,1008),
		(3,1009),
		(3,1014),
		(3,1015),
		(4,1001),
		(4,1003),
		(4,1005),
		(4,1009),
		(4,1014),
		(4,1015),
		(4,1008)


/*
INSERT INTO [dbo].[CaTypeDateConfiguration]
			([CaType],[ProcessType],[FieldRegistryId],[DateOffset],[DateType])
*/
INSERT INTO [dbo].[CaTypeDateConfiguration]
			([CaType],[ProcessType],[FieldRegistryId],[DateOffset],[DateType])
	 VALUES
		-- DRIP
			-- Scrubbing
				(1,0,1,0,'S'), (1,0,1,3,'T'), (1,0,1,6,'C'),
			-- Notification
				(1,1,5,-5,'S'), (1,1,5,0,'T'), (1,1,10,0,'C'),
			-- Response
				(1,2,106,-6,'S'), (1,2,106,-3,'T'), (1,2,106,0,'C'),
			-- Instruction
				(1,3,102,-4,'S'), (1,3,102,-1,'T'), (1,3,102,2,'C'),
			-- Payment
				(1,4,1008,0,'S'), (1,4,1008,3,'T'), (1,4,1008,7,'C'),
		-- DVOP
			-- Scrubbing
				(2,0,1,0,'S'), (2,0,1,3,'T'), (2,0,1,6,'C'),
			-- Notification
				(2,1,5,-5,'S'), (2,1,5,0,'T'), (2,1,10,0,'C'),
			-- Response
				(2,2,106,-6,'S'), (2,2,106,-3,'T'), (2,2,106,0,'C'),
			-- Instruction
				(2,3,102,-4,'S'), (2,3,102,-1,'T'), (2,3,102,2,'C'),
			-- Payment
				(2,4,1008,0,'S'), (2,4,1008,3,'T'), (2,4,1008,7,'C'),
		-- INTR
			-- Scrubbing
				(8,0,1,0,'S'), (8,0,1,3,'T'), (8,0,1,6,'C'),
			-- Notification
				(8,1,10,-7,'S'), (8,1,10,-3,'T'), (8,1,10,0,'C'),
			-- Payment
				(8,4,1008,0,'S'), (8,4,1008,3,'T'), (8,4,1008,7,'C'),
		-- TEDA
			-- Scrubbing
				(4,0,9,0,'S'), (4,0,9,3,'T'), (4,0,9,6,'C'),
			-- Notification
				(4,1,4,-7,'S'), (4,1,4,-3,'T'), (4,1,4,0,'C'),
			-- Response
				(4,2,106,-6,'S'), (4,2,106,-3,'T'), (4,2,106,0,'C'),
			-- Instruction
				(4,3,102,-4,'S'), (4,3,102,-1,'T'), (4,3,102,1,'C'),
			-- Payment
				(4,4,1008,0,'S'), (4,4,1008,3,'T'), (4,4,1008,7,'C'),
		-- TEND
			-- Scrubbing
				(7,0,9,0,'S'), (7,0,9,3,'T'), (7,0,9,6,'C'),
			-- Notification
				(7,1,4,-7,'S'), (7,1,4,-3,'T'), (7,1,4,0,'C'),
			-- Response
				(7,2,106,-6,'S'), (7,2,106,-3,'T'), (7,2,106,0,'C'),
			-- Instruction
				(7,3,102,-4,'S'), (7,3,102,-1,'T'), (7,3,102,2,'C'),
			-- Payment
				(7,4,1008,0,'S'), (7,4,1008,3,'T'), (7,4,1008,7,'C'),
		-- MEV
			-- Scrubbing
				(10,0,9,0,'S'), (10,0,9,3,'T'), (10,0,9,6,'C'),
			-- Notification
				(10,1,4,-7,'S'), (10,1,4,-3,'T'), (10,1,4,0,'C'),
			-- Response
				(10,2,106,-6,'S'), (10,2,106,-3,'T'), (10,2,106,0,'C'),
			-- Instruction
				(10,3,102,-4,'S'), (10,3,102,-1,'T'), (10,3,102,2,'C'),
			-- Payment
				(10,4,1008,0,'S'), (10,4,1008,3,'T'), (10,4,1008,7,'C'),
		-- PCAL
			-- Scrubbing
				(5,0,9,0,'S'), (5,0,9,3,'T'), (5,0,9,6,'C'),
			-- Notification
				(5,1,10,-7,'S'), (5,1,10,-3,'T'), (5,1,10,0,'C'),
			-- Payment
				(5,4,1008,0,'S'), (5,4,1008,3,'T'), (5,4,1008,7,'C'),
		-- RSUB
			-- Scrubbing
				(6,0,9,0,'S'), (6,0,9,3,'T'), (6,0,9,6,'C'),
			-- Notification
				(6,1,10,-7,'S'), (6,1,10,-3,'T'), (6,1,10,0,'C'),
			-- Response
				(6,2,106,-6,'S'), (6,2,106,-3,'T'), (6,2,106,0,'C'),
			-- Instruction
				(6,3,102,-4,'S'), (6,3,102,-1,'T'), (6,3,102,2,'C'),
			-- Payment
				(6,4,1008,0,'S'), (6,4,1008,3,'T'), (6,4,1008,7,'C'),
		-- PINK
			-- Scrubbing
				(3,0,1,0,'S'), (3,0,1,3,'T'), (3,0,1,6,'C'),
			-- Notification
				(3,1,10,-7,'S'), (3,1,10,-3,'T'), (3,1,10,0,'C'),
			-- Payment
				(3,4,1008,0,'S'), (3,4,1008,3,'T'), (3,4,1008,7,'C'),
		-- DVCA
			-- Scrubbing
				(9,0,1,0,'S'), (9,0,1,3,'T'), (9,0,1,6,'C'),
			-- Notification
				(9,1,5,-5,'S'), (9,1,5,0,'T'), (9,1,10,0,'C'),
			-- Payment
				(9,4,1008,0,'S'), (9,4,1008,3,'T'), (9,4,1008,7,'C')