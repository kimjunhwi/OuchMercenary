﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ReadOnlys;


public class ToggleUIBase : MonoBehaviour 
{
	public GameObject [] togglePanel;

	public Toggle [] toggle;




	public virtual void ActivePanel<T> (T _chapterIndex) {}
}