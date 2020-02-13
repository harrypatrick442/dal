const {DatabaseTypes}=require('enums');
const  {throwNotImplemented}=require('core');
const Mysql =require('./Mysql');
const Mssql =require('./Mssql');
module.exports= new (function(){
	this.createTable = function(databaseConfiguration, table){
		switch(databaseConfiguration.getDatabaseType()){
			case DatabaseTypes.MYSQL:
				return new Mysql(databaseConfiguration).raw(table.getCreate(DatabaseTypes.MYSQL));
			case DatabaseTypes.MSSQL:	
				return new Mssql(databaseConfiguration).raw(table.getCreate(DatabaseTypes.MSSQL));
			default:
				throwNotImplemented();
		}
	};
})();
