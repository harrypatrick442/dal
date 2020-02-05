const {DatabaseTypes=require('./ DalProgrammabilityMysql, DalProgrammabilityMssql}= require('./../index');
module.exports= function(configuration){
	var TypedDalProgrammability;
	console.log(DalProgrammabilityMssql);
	switch(configuration.getDatabaseType()){
		case DatabaseTypes.MYSQL:
			TypedDalProgrammability = DalProgrammabilityMysql;
		break;
		case DatabaseTypes.MSSQL:
			TypedDalProgrammability = DalProgrammabilityMssql;
		break;
	}
	console.log(TypedDalProgrammability);
	const dal = new TypedDalProgrammability(configuration);
	console.log(dal);
	this.updateProgrammable = dal.updateProgrammable;
	this.readProgrammables = dal.readProgrammables;
};
