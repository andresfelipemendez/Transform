using UnityEngine;
using System.Collections;

public interface EditModulationCommand 
{
	void Execute (TranformationManager manager, ModulationEditionTool tool, RaycastHit hit);
	void Undo (ModulationEditionTool tool);
}
