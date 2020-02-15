const {DatabaseTypes }= require('enums');
const ProgrammableTypes = {
	STORED_PROCEDURE:'storedProcedure',
	SCALAR_FUNCTION:'scalarFunction',
	VIEW:'view'
};var _array;
ProgrammableTypes.getArray=function(){
	if(_array)return _array;
	_array = Object.values(ProgrammableTypes).filter(value=>typeof(value)!=='function');
	return _array;
};
ProgrammableTypes.toString = function(programmableType, databaseType){
	switch(databaseType){
		case DatabaseTypes.MYSQL:
			switch(programmableType){
				case ProgrammableTypes.STORED_PROCEDURE:
					return 'procedure';
				case ProgrammableTypes.SCALAR_FUNCTION:
					return 'function';
				case ProgrammableTypes.VIEW:
					return 'view';
			}
			break;
		default: throw new Error('Not a valid DatabaseType');
	}
	throw new Error('Not a valid programmable type '+programmableType);
};
ProgrammableTypes.parse=function(str){
	switch(str.toLowerCase()){
		case 'procedure':
			return ProgrammableTypes.STORED_PROCEDURE;
		case 'function':
			return ProgrammableTypes.SCALAR_FUNCTION;
		case 'view':
			return ProgrammableTypes.VIEW;
		default:
			throw new Error('Could not parse '+str);
	}
};
module.exports = ProgrammableTypes;