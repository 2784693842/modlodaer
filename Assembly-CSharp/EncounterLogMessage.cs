using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
[Serializable]
public struct EncounterLogMessage
{
	// Token: 0x17000108 RID: 264
	// (get) Token: 0x060004CA RID: 1226 RVA: 0x0003160B File Offset: 0x0002F80B
	public string MainLogDefaultText
	{
		get
		{
			return this.LogText.DefaultText;
		}
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x060004CB RID: 1227 RVA: 0x00031618 File Offset: 0x0002F818
	public string MainLogKey
	{
		get
		{
			return this.LogText.LocalizationKey;
		}
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x060004CC RID: 1228 RVA: 0x00031625 File Offset: 0x0002F825
	public float GetDuration
	{
		get
		{
			if (!this.Duration)
			{
				return 0.5f;
			}
			return this.Duration;
		}
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x00031648 File Offset: 0x0002F848
	public static EncounterLogMessage Duplicate(EncounterLogMessage _From)
	{
		EncounterLogMessage encounterLogMessage = _From;
		if (_From.AlternateLogTexts != null)
		{
			encounterLogMessage.AlternateLogTexts = new LocalizedString[_From.AlternateLogTexts.Length];
			_From.AlternateLogTexts.CopyTo(encounterLogMessage.AlternateLogTexts, 0);
		}
		return encounterLogMessage;
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x00031688 File Offset: 0x0002F888
	public static EncounterLogMessage DuplicateWithText(string _Log, EncounterLogMessage _Params)
	{
		EncounterLogMessage result = _Params;
		result.LogText = new LocalizedString
		{
			LocalizationKey = "IGNOREKEY",
			DefaultText = _Log
		};
		result.AlternateLogTexts = null;
		return result;
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x000316C4 File Offset: 0x0002F8C4
	public static EncounterLogMessage ApplyFormatArguments(EncounterLogMessage _FromMessage, params object[] _Arguments)
	{
		EncounterLogMessage encounterLogMessage = _FromMessage;
		encounterLogMessage.LogText = new LocalizedString
		{
			LocalizationKey = "IGNOREKEY",
			DefaultText = string.Format(_FromMessage.LogText, _Arguments)
		};
		if (_FromMessage.AlternateLogTexts != null)
		{
			encounterLogMessage.AlternateLogTexts = new LocalizedString[_FromMessage.AlternateLogTexts.Length];
		}
		if (encounterLogMessage.AlternateLogTexts != null)
		{
			for (int i = 0; i < encounterLogMessage.AlternateLogTexts.Length; i++)
			{
				encounterLogMessage.AlternateLogTexts[i] = new LocalizedString
				{
					LocalizationKey = "IGNOREKEY",
					DefaultText = string.Format(_FromMessage.AlternateLogTexts[i], _Arguments)
				};
			}
		}
		return encounterLogMessage;
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x00031780 File Offset: 0x0002F980
	public EncounterLogMessage(string _Log)
	{
		this.LogText = new LocalizedString
		{
			LocalizationKey = "IGNOREKEY",
			DefaultText = _Log
		};
		this.AlternateLogTexts = null;
		this.Duration = new OptionalFloatValue(false, 0f);
		this.ScreenShake = false;
		this.SoundEffects = null;
		this.TextSettings = new OptionalTextSettings(false, false, false, false);
		this.TextColor = new OptionalColorValue(false, Color.clear);
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x000317F8 File Offset: 0x0002F9F8
	public EncounterLogMessage(string _Log, float _Duration)
	{
		this.LogText = new LocalizedString
		{
			LocalizationKey = "IGNOREKEY",
			DefaultText = _Log
		};
		this.AlternateLogTexts = null;
		this.Duration = new OptionalFloatValue(true, _Duration);
		this.ScreenShake = false;
		this.SoundEffects = null;
		this.TextSettings = new OptionalTextSettings(false, false, false, false);
		this.TextColor = new OptionalColorValue(false, Color.clear);
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x0003186C File Offset: 0x0002FA6C
	public LocalizedString GetLogText()
	{
		if (this.AlternateLogTexts == null)
		{
			return this.LogText;
		}
		if (this.AlternateLogTexts.Length == 0)
		{
			return this.LogText;
		}
		int num = UnityEngine.Random.Range(0, this.AlternateLogTexts.Length + 1);
		if (num == this.AlternateLogTexts.Length)
		{
			return this.LogText;
		}
		return this.AlternateLogTexts[num];
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x000318C7 File Offset: 0x0002FAC7
	public static implicit operator string(EncounterLogMessage _Log)
	{
		return _Log.ToString();
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x000318D8 File Offset: 0x0002FAD8
	public override string ToString()
	{
		if (this.AlternateLogTexts == null)
		{
			return this.LogText.ToString();
		}
		if (this.AlternateLogTexts.Length == 0)
		{
			return this.LogText.ToString();
		}
		int num = UnityEngine.Random.Range(0, this.AlternateLogTexts.Length + 1);
		if (num == this.AlternateLogTexts.Length)
		{
			return this.LogText.ToString();
		}
		return this.AlternateLogTexts[num].ToString();
	}

	// Token: 0x04000614 RID: 1556
	[SerializeField]
	private LocalizedString LogText;

	// Token: 0x04000615 RID: 1557
	[SerializeField]
	private LocalizedString[] AlternateLogTexts;

	// Token: 0x04000616 RID: 1558
	private OptionalFloatValue Duration;

	// Token: 0x04000617 RID: 1559
	public OptionalTextSettings TextSettings;

	// Token: 0x04000618 RID: 1560
	public OptionalColorValue TextColor;

	// Token: 0x04000619 RID: 1561
	public bool ScreenShake;

	// Token: 0x0400061A RID: 1562
	public AudioClip[] SoundEffects;
}
