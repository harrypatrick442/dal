module.exports= function(configuration){
	this.createDatabase = function(currentDatabaseConfiguration){
		var statement = 'CREATE DATABASE '+name+';';
		var dal = new Dal(currentDatabaseConfiguration);
		return dal.raw(statement);
	};
};
