using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio
{
	// Token: 0x020000AF RID: 175
	internal enum MmResult
	{
		// Token: 0x0400041D RID: 1053
		NoError,
		// Token: 0x0400041E RID: 1054
		UnspecifiedError,
		// Token: 0x0400041F RID: 1055
		BadDeviceId,
		// Token: 0x04000420 RID: 1056
		NotEnabled,
		// Token: 0x04000421 RID: 1057
		AlreadyAllocated,
		// Token: 0x04000422 RID: 1058
		InvalidHandle,
		// Token: 0x04000423 RID: 1059
		NoDriver,
		// Token: 0x04000424 RID: 1060
		MemoryAllocationError,
		// Token: 0x04000425 RID: 1061
		NotSupported,
		// Token: 0x04000426 RID: 1062
		BadErrorNumber,
		// Token: 0x04000427 RID: 1063
		InvalidFlag,
		// Token: 0x04000428 RID: 1064
		InvalidParameter,
		// Token: 0x04000429 RID: 1065
		HandleBusy,
		// Token: 0x0400042A RID: 1066
		InvalidAlias,
		// Token: 0x0400042B RID: 1067
		BadRegistryDatabase,
		// Token: 0x0400042C RID: 1068
		RegistryKeyNotFound,
		// Token: 0x0400042D RID: 1069
		RegistryReadError,
		// Token: 0x0400042E RID: 1070
		RegistryWriteError,
		// Token: 0x0400042F RID: 1071
		RegistryDeleteError,
		// Token: 0x04000430 RID: 1072
		RegistryValueNotFound,
		// Token: 0x04000431 RID: 1073
		NoDriverCallback,
		// Token: 0x04000432 RID: 1074
		MoreData,
		// Token: 0x04000433 RID: 1075
		WaveBadFormat = 32,
		// Token: 0x04000434 RID: 1076
		WaveStillPlaying,
		// Token: 0x04000435 RID: 1077
		WaveHeaderUnprepared,
		// Token: 0x04000436 RID: 1078
		WaveSync,
		// Token: 0x04000437 RID: 1079
		AcmNotPossible = 512,
		// Token: 0x04000438 RID: 1080
		AcmBusy,
		// Token: 0x04000439 RID: 1081
		AcmHeaderUnprepared,
		// Token: 0x0400043A RID: 1082
		AcmCancelled,
		// Token: 0x0400043B RID: 1083
		MixerInvalidLine = 1024,
		// Token: 0x0400043C RID: 1084
		MixerInvalidControl,
		// Token: 0x0400043D RID: 1085
		MixerInvalidValue
	}
}
