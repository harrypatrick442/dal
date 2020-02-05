const DatabaseTypes = {
	SQL:'mssql',
	MSSQL:'mssql',
	MYSQL:'mysql',
	
};
DatabaseTypes.checkIsValid=function(databaseType){
	return Object.values(DatabaseTypes).indexOf(databaseType)>=0;
};
module.exports =  DatabaseTypes;
