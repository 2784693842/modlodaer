using System;
using UnityEngine;

// Token: 0x02000090 RID: 144
public class MenuCharacterButton : IndexButton
{
	// Token: 0x1700012F RID: 303
	// (get) Token: 0x06000630 RID: 1584 RVA: 0x00041855 File Offset: 0x0003FA55
	// (set) Token: 0x06000631 RID: 1585 RVA: 0x0004185D File Offset: 0x0003FA5D
	public PlayerCharacter AssociatedCharacter { get; private set; }

	// Token: 0x06000632 RID: 1586 RVA: 0x00041866 File Offset: 0x0003FA66
	public void Setup(int _Index, PlayerCharacter _Character, GlobalCharacterInfo _CharInfo, bool _Unlocked)
	{
		this.AssociatedCharacter = _Character;
		base.Sprite = _Character.CharacterPortrait;
		this.SetUnlocked(_Unlocked);
		base.Setup(_Index, _Character.NameAndDifficulty(_CharInfo, false), null, false);
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x00041894 File Offset: 0x0003FA94
	public void SetUnlocked(bool _Value)
	{
		GraphicsManager.SetActiveGroup(this.LockedObjects, !_Value);
		GraphicsManager.SetActiveGroup(this.UnlockedObjects, _Value);
	}

	// Token: 0x04000874 RID: 2164
	public GameObject[] LockedObjects;

	// Token: 0x04000875 RID: 2165
	public GameObject[] UnlockedObjects;
}
