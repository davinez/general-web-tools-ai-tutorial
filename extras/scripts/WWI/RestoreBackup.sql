
USE master;

-- Using local system

-- Step 1

/*
If a error shows mentioning that access is unauthorized, then we need to:
1- Get SQL Server user that is used as "Log On"
2- Add that user to the directory - security tab
For more info: https://stackoverflow.com/a/52675801

Same for the path where the logical name will create their corresponding file, we need to add to that directory
the "write" permission
*/

-- Get the logical names from the backup file
RESTORE FILELISTONLY
FROM DISK = 'C:\Users\6134483\projects\db-backups-tutorials\WideWorldImporters-Full.bak'; 

-- Step 2

-- Specify the logical names of the database files
DECLARE @Primary NVARCHAR(128);
DECLARE @UserData NVARCHAR(128);
DECLARE @Log NVARCHAR(128);
DECLARE @InMemoryData NVARCHAR(128);

-- Set the logical names (replace with actual names from the previous step)
SET @Primary = 'WWI_Primary';
SET @UserData = 'WWI_UserData';
SET @Log = 'WWI_Log';
SET @InMemoryData = 'WWI_InMemory_Data_1';

-- Restore the database
RESTORE DATABASE WideWorldImporters
FROM DISK = 'C:\Users\6134483\projects\db-backups-tutorials\WideWorldImporters-Full.bak'
WITH MOVE @Primary TO 'C:\Users\6134483\projects\db-backups-tutorials\WWI-Data\WideWorldImporters.mdf',
     MOVE @UserData TO 'C:\Users\6134483\projects\db-backups-tutorials\WWI-Data\userdata.ndf',
     MOVE @Log TO 'C:\Users\6134483\projects\db-backups-tutorials\WWI-Data\WideWorldImporters.ldf',
     MOVE @InMemoryData TO 'C:\Users\6134483\projects\db-backups-tutorials\WWI-Data\WWI_InMemory_Data_1';


-- using docker container

USE master;

-- Step 1

-- Get the logical names from the backup file
RESTORE FILELISTONLY
FROM DISK = 'C:\Users\6134483\projects\db-backups-tutorials\WideWorldImporters-Full.bak'; 

-- Step 2

-- Specify the logical names of the database files
DECLARE @Primary NVARCHAR(128);
DECLARE @UserData NVARCHAR(128);
DECLARE @Log NVARCHAR(128);
DECLARE @InMemoryData NVARCHAR(128);

-- Set the logical names (replace with actual names from the previous step)
SET @Primary = 'WWI_Primary';
SET @UserData = 'WWI_UserData';
SET @Log = 'WWI_Log';
SET @InMemoryData = 'WWI_InMemory_Data_1';

-- Restore the database
RESTORE DATABASE WideWorldImporters
FROM DISK = '/var/opt/mssql\backups/WideWorldImporters-Full.bak'
WITH MOVE @Primary TO '/var/opt/mssql/data/WideWorldImporters.mdf',
     MOVE @UserData TO '/var/opt/mssql/log/userdata.ndf',
     MOVE @Log TO '/var/opt/mssql/data/WideWorldImporters.ldf',
     MOVE @InMemoryData TO '/var/opt/mssql/data/WWI_InMemory_Data_1';






