using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRespawnable
{
	void SaveCheckpoint();

	void LoadCheckpoint();
}
