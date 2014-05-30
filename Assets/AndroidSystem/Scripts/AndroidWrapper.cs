using UnityEngine;
using System;


public class AndroidWrapper : IDisposable {

	protected AndroidJavaObject mJavaObject;
	protected bool disposed;
	
	protected AndroidWrapper() {
		
	}
	
	~AndroidWrapper() {
		Dispose (false);
	}
	
	public AndroidJavaObject JavaObject {
		get {
			return mJavaObject;
		}
	}
	
	public void Dispose() {
		Dispose (true);
		GC.SuppressFinalize(this);
	}
	
	protected virtual void Dispose(bool disposing) {
		
		if (!this.disposed) {
			if (disposing) {
				
				if (mJavaObject != null) {
					mJavaObject.Dispose();
					mJavaObject = null;
				}
				
			}
		}
		this.disposed = true;
	}
	
}
