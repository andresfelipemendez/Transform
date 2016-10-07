using UnityEngine;
using System.Collections;

public interface EditModulationCommand 
{
	void Execute (RaycastHit hit);
}
