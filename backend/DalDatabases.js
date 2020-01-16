module.exports= new(function(configuration){
	const Dal =require('./Dal');
	const DatabaseConfiguration = require('configuration').DatabaseConfiguration;
	this.createDatabase = function(currentDatabaseConfiguration, name){
			var statement = "CREATE DATABASE "+name+';';
			return _createDatabase(currentDatabaseConfiguration, name, statement);
	};
	this.createOrRecreateDatabase = function(currentDatabaseConfiguration, name){
			var statement = "If(db_id(N'"+name+"') IS NOT NULL)DROP DATABASE "+name+"; CREATE DATABASE "+name+';';
			return _createDatabase(currentDatabaseConfiguration, name, statement);
	};
	
	this.deleteDatabase = function(currentDatabaseConfiguration, name){
		return new Promise((resolve, reject)=>{
			var dal = new Dal(currentDatabaseConfiguration);
			var statement = "If(db_id(N'"+name+"') IS NOT NULL)DROP DATABASE "+name+';';
			dal.raw(statement).then(resolve).catch(reject);
		});
	};
	function _createDatabase(currentDatabaseConfiguration, name, statement){
		return new Promise((resolve, reject)=>{
			var dal = new Dal(currentDatabaseConfiguration);
			console.log('about to do raw');
			console.log(statement);
			dal.raw(statement).then(()=>{
				console.log('did raw');
				resolve(new DatabaseConfiguration({user:currentDatabaseConfiguration.getUser(), password:currentDatabaseConfiguration.getPassword(),
				server:currentDatabaseConfiguration.getServer(), database:name}));
			}).catch(reject);
		});
	}
})();
