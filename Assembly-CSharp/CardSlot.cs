using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000004 RID: 4
public class CardSlot : MonoBehaviour, IDropHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000004 RID: 4 RVA: 0x000020C1 File Offset: 0x000002C1
	// (set) Token: 0x06000005 RID: 5 RVA: 0x000020C9 File Offset: 0x000002C9
	public Rect CurrentRect { get; private set; }

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000006 RID: 6 RVA: 0x000020D2 File Offset: 0x000002D2
	// (set) Token: 0x06000007 RID: 7 RVA: 0x000020EE File Offset: 0x000002EE
	public bool RaycastTarget
	{
		get
		{
			return this.RaycastGraphic && this.RaycastGraphic.raycastTarget;
		}
		set
		{
			if (this.RaycastGraphic)
			{
				this.RaycastGraphic.raycastTarget = value;
			}
			if (this.CardSwap)
			{
				this.CardSwap.SetRaycastTarget(value);
			}
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000008 RID: 8 RVA: 0x00002122 File Offset: 0x00000322
	public Transform GetParent
	{
		get
		{
			if (this.ExternalCardParent)
			{
				return this.ExternalCardParent;
			}
			if (this.CardParent)
			{
				return this.CardParent;
			}
			return base.transform;
		}
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002152 File Offset: 0x00000352
	public void PlayCardLandParticles()
	{
		if (this.CardLandParticles)
		{
			UnityEngine.Object.Instantiate<ParticleSystem>(this.CardLandParticles, base.transform).transform.position += Vector3.back;
		}
	}

	// Token: 0x0600000A RID: 10 RVA: 0x0000218C File Offset: 0x0000038C
	private void Awake()
	{
		this.RectTr = base.GetComponent<RectTransform>();
		this.GraphicsM = MBSingleton<GraphicsManager>.Instance;
		this.GM = MBSingleton<GameManager>.Instance;
		if (this.CardSwap)
		{
			if (this.RaycastGraphic)
			{
				this.CardSwap.SetRaycastTarget(this.RaycastGraphic.raycastTarget);
			}
			else
			{
				this.CardSwap.SetRaycastTarget(false);
			}
		}
		if (this.LiquidImage)
		{
			this.LocalLiquidImagePos = this.LiquidImage.transform.localPosition;
		}
		if (this.SlotText)
		{
			this.LocalSlotTextPos = this.SlotText.transform.localPosition;
		}
	}

	// Token: 0x0600000B RID: 11 RVA: 0x0000224C File Offset: 0x0000044C
	private void OnDisable()
	{
		if (this.HighlightGraphics)
		{
			this.HighlightGraphics.SetActive(false);
		}
		if (this.SlotImage)
		{
			this.SlotImage.enabled = false;
		}
		if (this.GM && Application.isPlaying)
		{
			this.GM.StartCoroutine(this.ResetTextParents());
		}
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000022B1 File Offset: 0x000004B1
	private IEnumerator ResetTextParents()
	{
		yield return null;
		this.SetTextsParent(base.transform);
		yield break;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000022C0 File Offset: 0x000004C0
	public void SetCardParent(RectTransform _Parent)
	{
		if (!_Parent)
		{
			return;
		}
		this.ExternalCardParent = new GameObject(base.name + "_CardParent", new Type[]
		{
			typeof(RectTransform)
		}).transform;
		this.ExternalCardParent.SetParent(_Parent);
		this.ExternalCardParent.localScale = Vector3.one;
		this.ExternalCardParent.position = base.transform.position;
	}

	// Token: 0x0600000E RID: 14 RVA: 0x0000233C File Offset: 0x0000053C
	public void SetTextsParent(Transform _Parent)
	{
		if (this.LiquidImage)
		{
			if (this.LiquidImage.transform.parent != _Parent)
			{
				this.LiquidImage.transform.SetParent(_Parent);
			}
			else
			{
				this.LiquidImage.transform.SetAsLastSibling();
			}
			this.LiquidImage.transform.localPosition = this.LocalLiquidImagePos;
		}
		if (this.SlotText)
		{
			if (this.SlotText.transform.parent != _Parent)
			{
				this.SlotText.transform.SetParent(_Parent);
			}
			else
			{
				this.SlotText.transform.SetAsLastSibling();
			}
			this.SlotText.transform.localPosition = this.LocalSlotTextPos;
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002410 File Offset: 0x00000610
	public void ImageListUpdated()
	{
		if (!this.ParentSlotData)
		{
			if (this.SlotImage)
			{
				this.SlotImage.enabled = false;
				this.SlotImage.overrideSprite = null;
			}
			return;
		}
		bool flag = this.ParentSlotData.AlternatingImages != null;
		if (flag)
		{
			flag = false;
			for (int i = 0; i < this.ParentSlotData.AlternatingImages.Count; i++)
			{
				if (this.ParentSlotData.AlternatingImages[i])
				{
					flag = true;
				}
			}
		}
		this.AlternatingImageTimer = 0f;
		this.AlternatingImageIndex = 0;
		if (flag)
		{
			if (this.SlotImage)
			{
				this.SlotImage.overrideSprite = this.ParentSlotData.AlternatingImages[0];
				this.SlotImage.enabled = true;
				return;
			}
		}
		else if (this.SlotImage)
		{
			this.SlotImage.enabled = false;
			this.SlotImage.overrideSprite = null;
		}
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002509 File Offset: 0x00000709
	public void ClickHelp()
	{
		if (!this.ParentSlotData)
		{
			return;
		}
		if (this.ParentSlotData.HelpPage)
		{
			this.GM.OpenGuide(this.ParentSlotData.HelpPage);
		}
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002544 File Offset: 0x00000744
	public void OnDrop(PointerEventData _Pointer)
	{
		if (this.HighlightGraphics)
		{
			this.HighlightGraphics.SetActive(false);
		}
		if (this.ParentSlotData == null)
		{
			return;
		}
		if (this.ParentSlotData.AssignedCard)
		{
			if (!this.ParentSlotData.AssignedCard.CurrentContainer)
			{
				return;
			}
			if (this.ParentSlotData.AssignedCard.CurrentContainer.CardModel.CardType != CardTypes.Blueprint && this.ParentSlotData.AssignedCard.CurrentContainer.CardModel.CardType != CardTypes.EnvImprovement && this.ParentSlotData.AssignedCard.CurrentContainer.CardModel.CardType != CardTypes.EnvDamage)
			{
				return;
			}
		}
		if (!_Pointer.pointerDrag || _Pointer.button != PointerEventData.InputButton.Left || this.SlotType == SlotsTypes.Exploration)
		{
			return;
		}
		InGameDraggableCard draggedCard = GameManager.DraggedCard;
		if (!draggedCard)
		{
			return;
		}
		if (!draggedCard.CanBeDragged)
		{
			return;
		}
		if (!draggedCard.CardModel)
		{
			return;
		}
		if (!this.ParentSlotData.CanReceiveCard(draggedCard, false))
		{
			if (this.SlotType == SlotsTypes.Equipment)
			{
				string text = this.GraphicsM.CharacterWindow.ReasonForNotEquipping(draggedCard.CardModel, draggedCard);
				if (!string.IsNullOrEmpty(text))
				{
					this.GraphicsM.ShowImpossibleToInspect(draggedCard, text);
					return;
				}
			}
			else if (this.SlotType == SlotsTypes.Base && this.GM.MaxEnvWeight > 0f && this.GM.CurrentEnvWeight + draggedCard.CurrentWeight > this.GM.MaxEnvWeight && draggedCard.CurrentSlotInfo.SlotType != SlotsTypes.Base)
			{
				this.GraphicsM.ShowImpossibleToInspect(draggedCard, LocalizedString.InventoryCannotCarry(this.GM.CurrentEnvironmentCard));
			}
			return;
		}
		if (this.SlotType != SlotsTypes.Inventory || !this.GraphicsM.InspectedCard || this.GraphicsM.InspectedCard.IsLegacyInventory)
		{
			this.ParentSlotData.AssignCard(draggedCard, false);
			this.GraphicsM.MoveViewToSlot(draggedCard.CurrentSlot, false, false);
			MBSingleton<SoundManager>.Instance.PerformCardAppearanceSound(draggedCard.CardModel.WhenCreatedSounds);
			return;
		}
		if (this.GraphicsM.InspectedCard.HasInGameCardInInventory(draggedCard))
		{
			this.GraphicsM.MoveCardToSlot(draggedCard, new SlotInfo(SlotsTypes.Inventory, Mathf.Clamp(this.ParentSlotData.Index, 0, this.GraphicsM.InspectedCard.CardsInInventory.Count - 1)), true, true);
			return;
		}
		int indexForInventory = this.GraphicsM.InspectedCard.GetIndexForInventory(this.ParentSlotData.Index, draggedCard.CardModel, draggedCard.ContainedLiquidModel, draggedCard.CurrentWeight);
		this.GraphicsM.MoveCardToSlot(draggedCard, new SlotInfo(SlotsTypes.Inventory, Mathf.Max(0, indexForInventory)), true, true);
		MBSingleton<SoundManager>.Instance.PerformCardAppearanceSound(draggedCard.CardModel.WhenCreatedSounds);
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002808 File Offset: 0x00000A08
	public void OnPointerEnter(PointerEventData _Pointer)
	{
		if (!this.HighlightGraphics)
		{
			return;
		}
		this.HighlightGraphics.SetActive(false);
		if (!_Pointer.pointerDrag || _Pointer.button != PointerEventData.InputButton.Left || this.SlotType == SlotsTypes.Exploration)
		{
			return;
		}
		if (this.ParentSlotData == null)
		{
			return;
		}
		InGameDraggableCard draggedCard = GameManager.DraggedCard;
		if (!draggedCard)
		{
			return;
		}
		if (!draggedCard.CardModel)
		{
			return;
		}
		if (!draggedCard.CanBeDragged)
		{
			return;
		}
		if (this.ParentSlotData.AssignedCard && this.ParentSlotData.AssignedCard != draggedCard && (this.ParentSlotData.AssignedCard.CardModel != draggedCard.CardModel || !this.ParentSlotData.PileCompatible(draggedCard.CardModel)))
		{
			return;
		}
		this.HighlightGraphics.SetActive(this.ParentSlotData.CanReceiveCard(draggedCard, false));
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000028EC File Offset: 0x00000AEC
	public void OnPointerExit(PointerEventData _Pointer)
	{
		if (this.HighlightGraphics)
		{
			this.HighlightGraphics.SetActive(false);
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002908 File Offset: 0x00000B08
	private void Update()
	{
		if (!this.RectTr)
		{
			this.RectTr = base.GetComponent<RectTransform>();
		}
		float num = this.GraphicsM.InsertingActionMargin;
		if (this.LiquidText && this.SlotText)
		{
			this.SlotText.gameObject.SetActive(!this.LiquidText.gameObject.activeInHierarchy);
		}
		if (this.ParentSlotData != null)
		{
			if (this.ParentSlotData.AssignedCard && this.ParentSlotData.AssignedCard.HasAction)
			{
				num *= 0.5f;
			}
			if (this.SlotTitle)
			{
				this.SlotTitle.text = this.ParentSlotData.TitleText;
			}
			if (this.SlotText)
			{
				this.SlotText.text = this.ParentSlotData.SlotText;
				this.SlotText.color = this.ParentSlotData.SlotTextColor;
			}
			if (this.LiquidImage && this.LiquidImage.activeSelf != this.ParentSlotData.LiquidImageActive)
			{
				this.LiquidImage.SetActive(this.ParentSlotData.LiquidImageActive);
			}
			if (this.LiquidText)
			{
				this.LiquidText.text = this.ParentSlotData.LiquidText;
				this.LiquidText.color = this.ParentSlotData.LiquidTextColor;
			}
			if (this.SlotImage && this.ParentSlotData.AlternatingImages.Count != 0)
			{
				this.AlternatingImageTimer += Time.deltaTime;
				if (this.AlternatingImageTimer >= this.AlternatingImageDuration)
				{
					this.AlternatingImageIndex++;
					if (this.AlternatingImageIndex >= this.ParentSlotData.AlternatingImages.Count)
					{
						this.AlternatingImageIndex = 0;
					}
					this.SlotImage.overrideSprite = this.ParentSlotData.AlternatingImages[this.AlternatingImageIndex];
					this.AlternatingImageTimer -= this.AlternatingImageDuration;
				}
			}
			if (this.CookingBarObjects)
			{
				this.CookingBarObjects.SetActive((this.ParentSlotData.CookingProgressValue > 0f || this.ParentSlotData.CookingProgressRemainingTicks > 0) && !this.ParentSlotData.CookingHideProgress);
			}
			if (this.CookingBar)
			{
				this.CookingBar.fillAmount = this.ParentSlotData.CookingProgressValue;
			}
			if (this.CookingText)
			{
				this.CookingText.text = this.ParentSlotData.CookingCustomText;
			}
			GraphicsManager.SetActiveGroup(this.CookingBarPausedObjects, this.ParentSlotData.CookingProgressPaused);
		}
		this.CurrentRect = new Rect(base.transform.TransformPoint(this.RectTr.rect.position) + Vector3.right * num * 0.5f, base.transform.TransformVector(this.RectTr.rect.size) - Vector3.right * num);
		if (this.ExternalCardParent)
		{
			if (MBSingleton<GraphicsManager>.Instance.SlotsLerpSpeed > 0f)
			{
				this.ExternalCardParent.transform.position = Vector3.Lerp(this.ExternalCardParent.transform.position, base.transform.position, MBSingleton<GraphicsManager>.Instance.SlotsLerpSpeed * Time.deltaTime);
			}
			else
			{
				this.ExternalCardParent.transform.position = base.transform.position;
			}
		}
		if (!GameManager.DontRenameGOs && this.ExternalCardParent)
		{
			this.ExternalCardParent.name = base.name + "_CardParent";
		}
		if (this.ParentSlotData == null)
		{
			this.RaycastTarget = false;
			return;
		}
		if (!(this.ParentSlotData.AssignedCard != null))
		{
			this.RaycastTarget = true;
			return;
		}
		if (this.ParentSlotData.AssignedCard == GameManager.DraggedCard)
		{
			if (this.ParentSlotData.CardPileCount(true) <= 1)
			{
				this.RaycastTarget = this.ParentSlotData.AssignedCard.BlocksRaycasts;
				return;
			}
			this.RaycastTarget = this.ParentSlotData.GetCardAtIndex(1).BlocksRaycasts;
			return;
		}
		else
		{
			if (!this.ParentSlotData.AssignedCard.CurrentContainer)
			{
				this.RaycastTarget = this.ParentSlotData.AssignedCard.BlocksRaycasts;
				return;
			}
			if (this.ParentSlotData.AssignedCard.CurrentContainer.CardModel.CardType == CardTypes.Blueprint || this.ParentSlotData.AssignedCard.CurrentContainer.CardModel.CardType == CardTypes.EnvImprovement || this.ParentSlotData.AssignedCard.CurrentContainer.CardModel.CardType == CardTypes.EnvDamage)
			{
				this.RaycastTarget = true;
				return;
			}
			this.RaycastTarget = this.ParentSlotData.AssignedCard.BlocksRaycasts;
			return;
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002E24 File Offset: 0x00001024
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(this.CurrentRect.center, this.CurrentRect.size + Vector3.forward * 0.2f);
		Gizmos.color = Color.white;
	}

	// Token: 0x04000012 RID: 18
	public SlotsTypes SlotType;

	// Token: 0x04000013 RID: 19
	public CardFilter CompatibleCards;

	// Token: 0x04000014 RID: 20
	public bool CanHostPile;

	// Token: 0x04000015 RID: 21
	public int MaxPileCount;

	// Token: 0x04000016 RID: 22
	public bool CanPin;

	// Token: 0x04000017 RID: 23
	public DynamicLayoutSlot ParentSlotData;

	// Token: 0x04000019 RID: 25
	private RectTransform RectTr;

	// Token: 0x0400001A RID: 26
	private Transform ExternalCardParent;

	// Token: 0x0400001B RID: 27
	[SerializeField]
	private Transform CardParent;

	// Token: 0x0400001C RID: 28
	[SerializeField]
	private Image SlotImage;

	// Token: 0x0400001D RID: 29
	[SerializeField]
	private float AlternatingImageDuration;

	// Token: 0x0400001E RID: 30
	[SerializeField]
	private TextMeshProUGUI SlotTitle;

	// Token: 0x0400001F RID: 31
	[SerializeField]
	private TextMeshProUGUI SlotText;

	// Token: 0x04000020 RID: 32
	[SerializeField]
	private GameObject LiquidImage;

	// Token: 0x04000021 RID: 33
	[SerializeField]
	private TextMeshProUGUI LiquidText;

	// Token: 0x04000022 RID: 34
	[SerializeField]
	public bool ShowTextAboveCards;

	// Token: 0x04000023 RID: 35
	[SerializeField]
	private Image CookingBar;

	// Token: 0x04000024 RID: 36
	[SerializeField]
	private TextMeshProUGUI CookingText;

	// Token: 0x04000025 RID: 37
	[SerializeField]
	private GameObject CookingBarObjects;

	// Token: 0x04000026 RID: 38
	[SerializeField]
	private GameObject[] CookingBarPausedObjects;

	// Token: 0x04000027 RID: 39
	[SerializeField]
	private Graphic RaycastGraphic;

	// Token: 0x04000028 RID: 40
	public CardSwapButton CardSwap;

	// Token: 0x04000029 RID: 41
	[SerializeField]
	private GameObject HelpButton;

	// Token: 0x0400002A RID: 42
	[Header("Graphics")]
	public GameObject EmptyGraphics;

	// Token: 0x0400002B RID: 43
	public GameObject HighlightGraphics;

	// Token: 0x0400002C RID: 44
	public ParticleSystem CardLandParticles;

	// Token: 0x0400002D RID: 45
	private GraphicsManager GraphicsM;

	// Token: 0x0400002E RID: 46
	private GameManager GM;

	// Token: 0x0400002F RID: 47
	private Vector2 LocalLiquidImagePos;

	// Token: 0x04000030 RID: 48
	private Vector2 LocalSlotTextPos;

	// Token: 0x04000031 RID: 49
	private float AlternatingImageTimer;

	// Token: 0x04000032 RID: 50
	private int AlternatingImageIndex;
}
