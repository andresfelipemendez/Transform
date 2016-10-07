using UnityEngine;
using System.Collections;

public interface Command 
{
	void Execute (RaycastHit hit);
}
