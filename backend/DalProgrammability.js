module.exports= function(configuration){
	const dal = new Dal(configuration);
	this.executeDefinition = executeDefinition;
	function executeDefinition(programmable){
		return new Promise(function(resolve, reject){
			dal.raw(definition).then(function(result){
				resolve(result.recordset);
			}).catch(reject);
		});
	}
	function dropProgrammable(programmable){
		dal.raw(
	}
};
