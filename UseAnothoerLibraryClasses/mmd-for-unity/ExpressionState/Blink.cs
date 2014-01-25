using UnityEngine;
using System.Collections;

public class Blink : MorphBaseStatus {

	public Blink(MorphManager morphManager) : base (morphManager,"まばたき"){
	}
	
	public override void actionStart(){
	}
	public override void action(float time) {
		actionRun((Random.Range(0,(int)(3.0f/time))==0),time);
	}


}
