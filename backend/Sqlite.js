module.exports = function(params){
	var readOnly = params.readOnly;
	var disposed = false;
	this.exec = function(sql){
		return new Promise((resolve, reject)=>{
			getDb().then((db)=>{
				db.exec(sql, function(err) {
					if(err){
						reject(err);
						return;
					}
					resolve();
				});	
			}).catch(reject);
		});
	};
	this.all = function(sql, parameters){
		return new Promise((resolve, reject)=>{
			getDb().then((db)=>{
				db.all(sql, paraemters, function(err, rows) {
					if(err){
						reject(err);
						return;
					}
					resolve(rows);
				});	
			}).catch(reject);
		});
	};
	this.each = function(sql, parameters, callback){
		return new Promise((resolve, reject)=>{
			var _done=false;
			getDb().then((db)=>{
				db.each(sql, parameters, function(err, row) {
					if(err){
						done(err);
						return;
					}
					callback(row);
				}, done);	
			}).catch(reject);
			function done(err, nRows){
				if(_done)return;
				_done=true;
				if(err)reject(err);
				else resolve(nRows);
			}
		});
	};
	this.beginGet = function(sql, parameters, callback){
			getDb().then((db)=>{
					var stmt = db.prepare(sql, parameters,(err)=>{
						if(err){
							reject(err);
							return;
						}
						stmt.get(function(err, row) {
							if(err){
								reject(err);
								return;
							}
							resolve(new GetHandle(stmt, callback);
						});
					});
				}, done);	
			}).catch(reject);
		});
	};
	this.run = function(sql, parameters){
		return new Promise((resolve, reject)=>{
			getDb.then((db)=>{
				db.run(sql, parameters);
			}).catch(reject);
		});
	};
	function getDb(){
		return new Promise((resolve, reject)=>{
			checkNotDisposed();
			var db = new sqlite3.Database(PATH, sqlite3.OPEN_READWRITE | sqlite3.OPEN_CREATE, (err)=>{
				if(err){reject(err);return;}
				resolve(db);
			});
		});
	}
	function dispose(){
		if(disposed)return;
		disposed=true;
		db.close();
	}
	function checkNotDisposed(){
		if(disposed)throw new Error('Already disposed');
	}
	function GetHandle(stmt){
		var ended=false;
		this.finalize = function(){
			if(ended)return;
			ended=true;
			stmt.finalize();
		};
		this.next = function(){
			if(ended)throw new Error('Already finalized');
			stmt.get();
		};
	}
};