function Get-ScriptDirectory {
    if ($psise) {
        Split-Path $psise.CurrentFile.FullPath
    }
    else {
        $global:PSScriptRoot
    }
}

#Need to fix adding role section
$sqlScriptPath = Get-ScriptDirectory
$sqlInstance = "SQL03"
$databaseName = "AutomationStandards"
$databasePath = "C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\Data" #no \ at end
$dbOwners = @("Observicing\GS-SQL-Admins")



#import SqlServer module
Import-Module -Name "SqlServer"


#Check if database exists - currently deleting if so for testing
$dbCheckSql = "
DECLARE @dbname nvarchar(128)
SET @dbname = N'{0}'

IF (EXISTS (SELECT name 
FROM master.dbo.sysdatabases 
WHERE ('[' + name + ']' = @dbname 
OR name = @dbname)))

SELECT 'db exists'" -f $databaseName


$dbExists = Invoke-SqlCmd -ServerInstance $sqlInstance -Query $dbCheckSql

if ($dbExists)
{
    $dropDatabaseSQL = "USE master; ALTER DATABASE $databaseName SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE $databaseName"
    Invoke-SqlCmd -ServerInstance $sqlInstance -Query $dropDatabaseSQL
}
 
# create variable with SQL to execute
$createDbsql = Get-Content "$($sqlScriptPath)\CreateDatabaseSchema.txt"
$createDbsql = $createDbsql -f $databaseName,$databasePath
<#
$createDbsql = "
CREATE DATABASE {0}
 CONTAINMENT = NONE
 ON  PRIMARY
( NAME = N'{0}', FILENAME = N'{1}\{0}.mdf' , SIZE = 1048576KB , FILEGROWTH = 262144KB )
 LOG ON
( NAME = N'{0}_log', FILENAME = N'{1}\{0}_log.ldf' , SIZE = 524288KB , FILEGROWTH = 131072KB )
GO

USE [master]
GO
ALTER DATABASE {0} SET RECOVERY SIMPLE WITH NO_WAIT
GO

ALTER AUTHORIZATION ON DATABASE::{0} TO [sa]

GO" -f $databaseName,$databasePath
#>
#Create Database
Invoke-SqlCmd -ServerInstance $sqlInstance -Query $createDbsql



##NEED TO FIX
<#


foreach ($u in $dbOwners)
{
    #$authorizationSql = "ALTER AUTHORIZATION ON DATABASE::{0} TO '[{1}]'" -f $databaseName,$u
    #Invoke-SqlCmd -ServerInstance $sqlInstance -Query $authorizationSql
    Add-RoleMember -Server $sqlInstance -MemberName "$u" -Database $databaseName -RoleName "db_owner"
}
#>

#Create Tables
$createtablesSql = Get-Content "$($sqlScriptPath)\CreateDatabaseTables.txt"
$createtablesSql = $createtablesSql -f $databaseName
Invoke-SqlCmd -ServerInstance $sqlInstance -Query $createtablesSql


$sqlDataTypes = Import-Csv "$sqlScriptPath\SQLDataTypes.csv"
#Populate Tables
for ($i = 0; $i -lt $sqlDataTypes.Count; $i++)
{
    $populateDataTypeSql = "
    INSERT INTO [{0}].[dbo].[StandardDataType] (DataTypeName,SQLDataType)
    VALUES ('{1}','{2}')" -f $databaseName,$($sqlDataTypes[$i].DataTypeName),$($sqlDataTypes[$i].SQLDataType)
    Invoke-SqlCmd -ServerInstance $sqlInstance -Query $populateDataTypeSql
}

$sqlStdGroups = Import-Csv "$sqlScriptPath\StandardsGroups.csv"
#Populate Tables
for ($i = 0; $i -lt $sqlStdGroups.Count; $i++)
{
    $populateStdGroupsSql = "
    INSERT INTO [{0}].[dbo].[StandardGroup] (StandardGroupName)
    VALUES ('{1}')" -f $databaseName,$($sqlStdGroups[$i].StandardGroupName)
    Invoke-SqlCmd -ServerInstance $sqlInstance -Query $populateStdGroupsSql
}

#Create sample data
<#
$sqlAdForestLevels = Get-Content "$sqlScriptPath\CreateForestFunctionalLevels.txt"
$sqlAdForestLevels = $sqlAdForestLevels -f $databaseName
Invoke-SqlCmd -ServerInstance $sqlInstance -Query $sqlAdForestLevels
#>
$sqlSampleTables = Get-Content "$sqlScriptPath\CreateSampleTables.txt"
$sqlSampleTables = $sqlSampleTables -f $databaseName
Invoke-SqlCmd -ServerInstance $sqlInstance -Query $sqlSampleTables


$sqlAdForestLevelsData = Import-Csv "$sqlScriptPath\ForestFunctionalLevels.csv"
#Populate Tables
for ($i = 0; $i -lt $sqlAdForestLevelsData.Count; $i++)
{
    $populateSqlAdForestLevelsData = "INSERT INTO [{0}].[dbo].[StForestFunctionalLevels] (CreatedBy, 
    ModifiedBy, Enabled, SortOrder, ForestFunctionalLevel)
    VALUES ('{1}','{2}','{3}','{4}','{5}')" -f $databaseName,$sqlAdForestLevelsData[$i].CreatedBy,
    $sqlAdForestLevelsData[$i].ModifiedBy,$sqlAdForestLevelsData[$i].Enabled,
    $sqlAdForestLevelsData[$i].SortOrder,$sqlAdForestLevelsData[$i].ForestFunctionalLevel
   # INSERT INTO [{0}].[dbo].[StForestFunctionalLevels] (CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, Enabled, SortOrder, ForestFunctionalLevel)
   # VALUES ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')" -f $databaseName,$($sqlAdForestLevelsData[$i].CreatedOn,$sqlAdForestLevelsData[$i].CreatedBy,
   # $sqlAdForestLevelsData[$i].ModifiedOn,$sqlAdForestLevelsData[$i].ModifiedBy,$sqlAdForestLevelsData[$i].Enabled,$sqlAdForestLevelsData[$i].SortOrder,
   # $sqlAdForestLevelsData[$i].ForestFunctionalLevel)
    Invoke-SqlCmd -ServerInstance $sqlInstance -Query $populateSqlAdForestLevelsData
}

$sqlStdAdDomains = Import-Csv "$sqlScriptPath\ActiveDirectoryDomains.csv"
#Populate Tables
for ($i = 0; $i -lt $sqlStdAdDomains.Count; $i++)
{
    $populateStdGroupsSql = "
    INSERT INTO [{0}].[dbo].[StActiveDirectoryDomains] ([DomainShortName],[DomainLongName],[ForestFunctionalLevel],[Enabled],[CreatedBy],[ModifiedBy])
    VALUES ('{1}','{2}','{3}',{4},'{5}','{6}')" -f $databaseName,$sqlStdAdDomains[$i].DomainShortName,$sqlStdAdDomains[$i].DomainLongName,$sqlStdAdDomains[$i].ForestFunctionalLevel,
    $sqlStdAdDomains[$i].Enabled,$sqlStdAdDomains[$i].CreatedBy,$sqlStdAdDomains[$i].ModifiedBy
    Invoke-SqlCmd -ServerInstance $sqlInstance -Query $populateStdGroupsSql
}

<#
$sqlForestLevelsStandard = "INSERT INTO [{0}].[DBO].[Standard] (StandardGroupID,DBTableName,	StandardName,	StandardDefinition,	ManageRoles,
	VersionConfig,	VersionValue,	NotifiyOwner,	ViewerRoles,	UsageCount,	CreatedBy,	ModifiedBy)
VALUES (2,	'StForestFunctionalLevels',	'Forest Functional Levels',	'AD Forest Functional Levels',	
'Admin',	0,	0,	0,	'Admin',	0,	'atester', 'atester')" -f $databaseName
Invoke-SqlCmd -ServerInstance $sqlInstance -Query $sqlForestLevelsStandard
#>
$sampleStandardData = Import-Csv "$sqlScriptPath\SampleStandardData.csv"
for ($i = 0; $i -lt $sampleStandardData.Count; $i++)
{
    $sqlStandardData = "INSERT INTO [{0}].[DBO].[Standard] (
       [StandardGroupID]
      ,[DBTableName]
      ,[StandardName]
      ,[StandardDefinition]
      ,[ManageRoles]
      ,[VersionConfig]
      ,[VersionValue]
      ,[Tags]
      ,[NotifiyOwner]
      ,[OwnerEmail]
      ,[ViewerRoles]
      ,[UsageCount]
      ,[CreatedBy]
      ,[ModifiedBy]
      )
      VALUES ({1},'{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},'{11}',{12},'{13}','{14}')" -f $databaseName,$sampleStandardData[$i].StandardGroupID,$sampleStandardData[$i].DBTableName,
      $sampleStandardData[$i].StandardName,$sampleStandardData[$i].StandardDefinition,$sampleStandardData[$i].ManageRoles,$sampleStandardData[$i].VersionConfig,
      $sampleStandardData[$i].VersionValue,$sampleStandardData[$i].Tags,$sampleStandardData[$i].NotifiyOwner,$sampleStandardData[$i].OwnerEmail,
      $sampleStandardData[$i].ViewerRoles,$sampleStandardData[$i].UsageCount,$sampleStandardData[$i].CreatedBy,$sampleStandardData[$i].ModifiedBy
      Invoke-SqlCmd -ServerInstance $sqlInstance -Query $sqlStandardData
}

#Run This Last
$sampleStandardConfigs = Import-Csv "$sqlScriptPath\SampleStandardConfigs.csv"
for ($i = 0; $i -lt $sampleStandardConfigs.Count; $i++)
{
    $sqlStandardConfig = "INSERT INTO [{0}].[DBO].[StandardConfig] (      
           [StandardID]
          ,[FieldName]
          ,[DisplayName]
          ,[DataTypeID]
          ,[SortOrder]
          ,[VersionNumber]
          ,[UseToolTip]
          ,[ToolTip]
          ,[UseStandardData]
          ,[AllowMultiSelect]
          ,[StandardLUID]
          ,[StandardLUValue]
          ,[StandardUseFilter]
          ,[StandardFilterSQL]
          ,[CreatedBy]
          ,[ModifiedBy]
          )
          VALUES ({1},'{2}','{3}',{4},{5},{6},{7},'{8}',{9},{10},{11},'{12}',{13},'{14}','{15}','{16}')" -f $databaseName,$sampleStandardConfigs[$i].StandardId,$sampleStandardConfigs[$i].FieldName,
          $sampleStandardConfigs[$i].DisplayName,$sampleStandardConfigs[$i].DataTypeID,$sampleStandardConfigs[$i].SortOrder,$sampleStandardConfigs[$i].VersionNumber,
          $sampleStandardConfigs[$i].UseToolTip,$sampleStandardConfigs[$i].ToolTip,$sampleStandardConfigs[$i].UseStandardData,$sampleStandardConfigs[$i].AllowMultiSelect,
          $sampleStandardConfigs[$i].StandardLUID,$sampleStandardConfigs[$i].StandardLUValue,$sampleStandardConfigs[$i].StandardUseFilter,
          $sampleStandardConfigs[$i].StandardFilterSQL,$sampleStandardConfigs[$i].CreatedBy,$sampleStandardConfigs[$i].ModifiedBy
          Invoke-SqlCmd -ServerInstance $sqlInstance -Query $sqlStandardConfig
}