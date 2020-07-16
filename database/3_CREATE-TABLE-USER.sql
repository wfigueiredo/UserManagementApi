SET search_path TO USERMANAGEMENTAPI;

DROP TABLE IF EXISTS USERMANAGEMENTAPI.USER;

CREATE TABLE USERMANAGEMENTAPI.USER
(	
    ID 					SERIAL 	   		    NOT NULL, 
	NAME 				VARCHAR(20) 		NOT NULL,
	LASTNAME 			VARCHAR(30) 		NOT NULL,
	USERNAME			VARCHAR(20)			NOT NULL,
	SECRET 				VARCHAR(100)		NOT NULL,
	EMAILADDRESS 		VARCHAR(50) 		NOT NULL,
	PHONENUMBER 		VARCHAR(20) 		NOT NULL,
	ROLEID				INTEGER				NOT NULL,
	CREATEDAT			TIMESTAMP(20)		NOT NULL,
	LASTMODIFIED		TIMESTAMP(20)		NOT NULL,
	CONSTRAINT PK_TB_USER PRIMARY KEY (ID),
	CONSTRAINT UNIQUE_TB_USER UNIQUE (USERNAME),
	CONSTRAINT FK_TB_ROLE FOREIGN KEY (ROLEID) REFERENCES USERMANAGEMENTAPI.ROLE(ID)
);