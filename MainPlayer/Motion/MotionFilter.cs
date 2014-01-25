using UnityEngine;
using System.Collections;

public class MotionFilter {

	private long filter = 0;
	public long Filter {
		set{filter = value;}
		get{return filter;}
	}

	public MotionFilter (bool[] filter) {
		for (int i=0; i<filter.Length; i++) {
			if (filter[i]) {
				this.filter |= (1<<i+1);
			}
		}
	}


}
