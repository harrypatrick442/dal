module.exports = (function(params){
	const logPath =params.logPath, databaseName=params.databaseName, 
	const log = new Log({path:logPath});
	this.build = function(){
		var newDatabase;
		createDatabase(host, databaseName).then((database)=>{
			newDatabase = database;
			populateDatabaseWithProgrammabes().then(()=>{
				
			}).catch(error);	
		}).catch(error);
		
		function error(err){
			if(newDatabase){
				DalDatabases.deleteDatabase(newDatabase).then(doReject).catch((err)=>{
					log.error(new Error('Error deleting database '+newDatabase.getName()+' while cleaning up after creating new shard faile'));
					doReject();
				});
				return;
			}
			doReject();
			function doReject(){
				log.error(err);
				reject(err);
			}}
		}
	};
	function createDatabase(host, name){
		return DalDatabases.createDatabase();
	}
	function populateDatabaseWithProgrammables(){
		return new Promimse((resolve, reject)=>{
			getProgrammables().then((programmables)=>{
				var iterator = new Iterator(programmables);
				nextProgrammable();
				function nextProgrammable(){
					if(!iterator.hasNext()){
						resolve();
						return;
					}
					var programmable = iterator.next();
					populateDatabaseWithProgrammable(programmable).then(nextProgrammable).catch(reject);
				}
			}).catch(reject);
		});
	}
	function populateDatabaseWithProgrammable(){
		return new Promise((resolve, reject)=>{
			DalProgrammability.executeDefinition(programmable.getCreateDefinition()).then(resolve).catch(reject);
		);
	}
})();