module.exports= function(configuration){
	const Dal =require('./Dal');
	const dal = new Mysql(configuration);
	const {DatabaseTypes}=require('enums');
	const {throwNotImplemented} = require('core');
	this.updateProgrammable = function(programmable){
		if(programmable.getDatabaseType()!==DatabaseTypes.MSSQL)throw new Error('Programmable was not for DatabaseType MSSQL');
		return dal.raw(programmable.getCreateOrAlterDefinition());
	};
	this.readProgrammables = function(){
		throwNotImplemented();
	};
};
