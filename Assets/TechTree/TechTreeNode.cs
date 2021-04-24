using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;

public class TechTreeNode : Node {

	public string title;
	public string description;
	public Image image;
	public int cost;
	public int tier;

	[Input] public TechTreeNode parent;
	[Output] public TechTreeNode[] childeren;

	// Use this for initialization
	protected override void Init() {
		base.Init();
		// something new
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return 0f;
	}
}