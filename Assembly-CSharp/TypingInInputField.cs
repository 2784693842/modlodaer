using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001B7 RID: 439
[RequireComponent(typeof(TMP_InputField))]
public class TypingInInputField : MonoBehaviour
{
	// Token: 0x06000BF0 RID: 3056 RVA: 0x000634EC File Offset: 0x000616EC
	private void Awake()
	{
		this.InputField = base.GetComponent<TMP_InputField>();
		this.InputField.onDeselect.AddListener(new UnityAction<string>(this.OnDeselect));
		this.InputField.onSelect.AddListener(new UnityAction<string>(this.OnSelect));
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x0006353D File Offset: 0x0006173D
	private void OnSelect(string _Input)
	{
		GraphicsManager.CurrentTypingInput = this.InputField;
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x0006354A File Offset: 0x0006174A
	private void OnDeselect(string _Input)
	{
		if (GraphicsManager.CurrentTypingInput == this.InputField)
		{
			GraphicsManager.CurrentTypingInput = null;
		}
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x0006354A File Offset: 0x0006174A
	private void OnDisable()
	{
		if (GraphicsManager.CurrentTypingInput == this.InputField)
		{
			GraphicsManager.CurrentTypingInput = null;
		}
	}

	// Token: 0x040010F3 RID: 4339
	private TMP_InputField InputField;
}
