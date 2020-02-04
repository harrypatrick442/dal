const DatabaseTypes = {
	SQL:'mssql',
	MSSQL:'mssql',
	MYSQL:'mysql',
	
};
DatabaseTypes.checkIsValid=function(){
	return Object.values(DatabaseTypes).indexOf(databaseType)>=0;
};
module.exports =  DatabaseTypes;
