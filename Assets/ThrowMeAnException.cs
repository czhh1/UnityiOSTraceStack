using System;
using UnityEngine;
//using UnityEngine.CrashLog;
using System.Collections;
using System.Runtime.InteropServices;

public class ThrowMeAnException : MonoBehaviour 
{
	public UnityEngine.UI.Text lastCrash;

	[DllImport("__Internal")]
	private static extern void NativeCrash ();
	[DllImport("__Internal")]
	private static extern void ObjCCrash();

	[DllImport("__Internal")]
	private static extern IntPtr GetStackSymbols();

    void Awake()
    {
		//CrashReporting.Init("9802f629-f27b-4758-bbb1-208b1de3c45d","1.0.0");
    }
	void Start(){
		if (lastCrash != null && UnityEngine.CrashReport.lastReport != null && UnityEngine.CrashReport.lastReport .time != null && UnityEngine.CrashReport.lastReport .text != null) {
			lastCrash.text = UnityEngine.CrashReport.lastReport.time + "\n" + UnityEngine.CrashReport.lastReport.text;
		}
	}

	public void CrashForMe()
	{
		throw new Exception("Button press exception");
	}

	public void NativeNullCrash(){
		DebugTest ();
//		NativeCrash ();
	}

	public void PrintStackTrace(){
		lastCrash.text = this.GetNativeStackSymbols ();
	}

	public string GetNativeStackSymbols()
	{
		IntPtr ptr = GetStackSymbols ();
		if (ptr == IntPtr.Zero) {
			return null;
		}
		string str = Marshal.PtrToStringAnsi (ptr);
		return str;
	}

	public void ObjectiveCError(){
		ObjCCrash ();
	}


	private void DebugTest(){
		for (int i = 0; i < 10; ++i) {
			NativeCrash ();
		}
	}


	public void InifiniteAlloc(){
		System.Object[] obj = new System.Object[1024 * 1024];
		for (int i = 0; i < obj.Length; ++i) {
			obj [i] = new byte[1024];
		}
	}

	public void InfiniteLoop(){
		for (int i = 0;; ++i) {
			UnityEngine.Debug.Log (i);
		}
	}

	public void InfiniteRecursive(){
		Debug.Log("result" + InternalCall ( 1024 * 1024 * 512) );
	}

	private int InternalCall(int a){
		if (a <= 0) return 0;
		return a + (InternalCall(a - 1)) + (InternalCall(a - 2));
	}
}
