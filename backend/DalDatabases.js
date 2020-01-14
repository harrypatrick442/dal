module.exports= new(function(configuration){
	const Dal =require('./Dal');
	const DatabaseConfiguration = require('configuration').DatabaseConfiguration;
	this.createDatabase = function(currentDatabaseConfiguration, name){
		return new Promise((resolve, reject)=>{
			dal.raw(statement).then(()=>{
				var statement = "If(db_id(N'"+name+"') IS NULL)CREATE DATABASE "+name+';';
				var dal = new Dal(currentDatabaseConfiguration);
				resolve(new DatabaseConfiguration({user:currentDatabaseConfiguration.getUser(), password:currentDatabaseConfiguration.getPassword(),
				server:currentDatabaseConfiguration.getServer(), database:name}));
			}).catch(reject);
		});
	};
	
	this.createDatabase = function(currentDatabaseConfiguration, name){
		return new Promise((resolve, reject)=>{
			var dal = new Dal(currentDatabaseConfiguration);
			var statement = "If(db_id(N'"+name+"') IS NOT NULL)DROP DATABASE "+name+';';
			dal.raw(statement).then(resolve).catch(reject);
		});
	};
})();
