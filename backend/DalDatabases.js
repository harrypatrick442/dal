module.exports= new(function(configuration){
	const Dal =require('./Dal');
	this.createDatabase = function(currentDatabaseConfiguration, name){
		return new Promise((resolve, reject)=>{
			var statement = "If(db_id(N'"+name+"') IS NULL)CREATE DATABASE "+name+';';
			var dal = new Dal(currentDatabaseConfiguration);
			dal.raw(statement).then(()=>{
				resolve(new DatabaseConfiguration({user:currentDatabaseConfiguration.getUser(), password:currentDatabaseConfiguration.getPassword(),
				server:currentDatabaseConfiguration.getServer(), database:name}));
			}).catch(reject);
		});
	};
})();
