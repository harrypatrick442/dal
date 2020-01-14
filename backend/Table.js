const Core = require('core');
const TableColumnTypes = require('./TableColumnTypes');
const Table = function(params){
	var name = params.name;
	if(!name)throw new Error('No name provided');
	var columns = params.columns;
	if(!columns)throw new Error('No columns provided');
	this.getCreate = getCreate;
	this.getUpdate = getUpdate;
	function getUpdate(){
		var str = "IF(object_id('"+name+"') is not null) begin drop table "+table+";";
		str+=getCreate();
		return str;
	}
	function getCreate(){
		var str = 'CREATE TABLE '+name+'(';
		var first = true;
		var primaryKeyColumns=[];
		columns.forEach((column)=>{
			if(first)str+=',';
			else first = false;
			str+='['+column.getName()+'] '+column.getType()+getPrecisionScaleLength(column);
			str+=column.getNullable()?' NULL':' NOT NULL';
		});
		if(column.getPrimaryKey())
			primaryKeyColumns.push(column);
		if(primaryKeyColumns.length>0){
			str+=', PRIMARY KEY(';
			primaryKeyColumns.forEach((primaryKeyColumn)=>{
				str+='['+primaryKeyColumn.getName()+']';
			});
			str+=')';
		}
		
		str +=')';
		return str;
	};
	function getPrecisionScaleLength(column){
		var type = column.getType();
		if(type===TableColumnTypes.DECIMAL){
			var precision = column.getPrecision();
			if(precision===undefined||precision===null)throw new Error('Precision not supplied for '+type);
			var scale = column.getScale();
			if(scale===undefined||scale===null)throw new Error('Scale not supplied for '+type);
			return '('+preicsion+','+scale+')';
		}
		var length = column.getLength();
		if(length===undefined||length===null)return null;
		return '('+length+')';
	}
};

module.exports = Table;