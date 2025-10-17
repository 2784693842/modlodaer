using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000102 RID: 258
[CreateAssetMenu(menuName = "Survival/Character Perk")]
public class CharacterPerk : CompletableObject
{
	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06000867 RID: 2151 RVA: 0x00052A9C File Offset: 0x00050C9C
	public bool ShouldBeLockedOnRestart
	{
		get
		{
			return Application.isEditor && this.ResetLockInEditor;
		}
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00052AB0 File Offset: 0x00050CB0
	protected override void OnComplete(bool _Init)
	{
		bool isEditor = Application.isEditor;
		if (!GameLoad.Instance.SaveData.UnlockedPerks.Contains(UniqueIDScriptable.SaveID(this)))
		{
			GameLoad.Instance.SaveData.UnlockedPerks.Add(UniqueIDScriptable.SaveID(this));
			GameLoad.Instance.SaveMainDataToFile();
		}
		if (!_Init)
		{
			MBSingleton<GraphicsManager>.Instance.ShowPerkUnlocked(this);
		}
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x00018E36 File Offset: 0x00017036
	protected override void OnNotComplete(bool _Init)
	{
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x00052B12 File Offset: 0x00050D12
	public override bool IsAlive()
	{
		return base.IsAlive();
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00052B1A File Offset: 0x00050D1A
	public override bool IsHidden()
	{
		return false;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00018E36 File Offset: 0x00017036
	protected override void PlayNotification(float _Progress)
	{
	}

	// Token: 0x04000CB6 RID: 3254
	public int SunsCost;

	// Token: 0x04000CB7 RID: 3255
	public int MoonsCost;

	// Token: 0x04000CB8 RID: 3256
	[Header("FOR DEBUG")]
	public bool ResetLockInEditor;

	// Token: 0x04000CB9 RID: 3257
	[SpecialHeader("Effects", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public LocalizedString PerkName;

	// Token: 0x04000CBA RID: 3258
	public LocalizedString PerkDescription;

	// Token: 0x04000CBB RID: 3259
	public LocalizedString PerkUnlockConditions;

	// Token: 0x04000CBC RID: 3260
	public Sprite PerkIcon;

	// Token: 0x04000CBD RID: 3261
	public int DifficultyRating;

	// Token: 0x04000CBE RID: 3262
	public float DisplayPriority;

	// Token: 0x04000CBF RID: 3263
	public bool HiddenUntilUnlocked;

	// Token: 0x04000CC0 RID: 3264
	[Space]
	[FormerlySerializedAs("AddedStartingTime")]
	public int AddedStartingHours;

	// Token: 0x04000CC1 RID: 3265
	public CardData OverrideEnvironment;

	// Token: 0x04000CC2 RID: 3266
	public CardData OverrideWeather;

	// Token: 0x04000CC3 RID: 3267
	public CardData[] AddedCards;

	// Token: 0x04000CC4 RID: 3268
	public CardData[] EquippedCards;

	// Token: 0x04000CC5 RID: 3269
	public bool OverrideEquipment;

	// Token: 0x04000CC6 RID: 3270
	[StatModifierOptions(false, false)]
	public StatModifier[] StartingStatModifiers;

	// Token: 0x04000CC7 RID: 3271
	[StatModifierOptions(true, false)]
	public StatModifier[] PassiveStatModifiers;

	// Token: 0x04000CC8 RID: 3272
	public ActionModifier[] ActionModifiers;
}
