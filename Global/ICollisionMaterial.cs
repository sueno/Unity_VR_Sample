using UnityEngine;
using System.Collections;

public interface ICollisionMaterial {
	
	int PlayerID {
		get;
	}
	
	string name {
		get;
	}

	ICollisionMaterial ColObj {
		get;
	}

	GameObject ParentObj {
		get;
	}

	Mob Owner {
		set;
		get;
	}
}
