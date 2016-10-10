using UnityEngine;
using System.Collections;

public interface EditModulationCommand 
{
	void Execute (ModulationEditionTool activeTool, RaycastHit hit);
	void Undo ();
}
