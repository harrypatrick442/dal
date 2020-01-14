module.exports= function(configuration){
	const Dal =require('./Dal');
	const dal = new Dal(configuration);
	this.updateProgrammable = function(programmable){
		return dal.raw(programmable.getCreateOrAlterDefinition());
	};
};
