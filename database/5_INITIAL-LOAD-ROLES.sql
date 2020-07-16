SET SEARCH_PATH TO USERMANAGEMENTAPI;

INSERT INTO ROLE (ID,CODE,NAME,DESCRIPTION) VALUES
(DEFAULT, 'root', 'Root', 'Root users can perform any action', current_date),
(DEFAULT, 'admin', 'Administrator', 'Admin users can perform any managerial action', current_date),
(DEFAULT, 'user', 'User', 'Registered users can perform a restrict set of actions', current_date),
(DEFAULT, 'guest', 'Guest', 'Guest users can perform read-only actions', current_date);

INSERT INTO USERMANAGEMENTAPI.USER (ID,NAME,LASTNAME,USERNAME,SECRET,EMAILADDRESS,PHONENUMBER,ROLEID,CREATEDAT,LASTMODIFIED) VALUES
(DEFAULT, 'Walanem', 'Figueiredo', 'wfig', '023082c33f41277a0a03cb4e156fb78c6c3882f7', 'wfig@gmail.com', '551133456709',1,current_date,current_date);