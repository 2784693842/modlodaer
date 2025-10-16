using System;
using System.IO;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis;
using UnityEngine;

namespace ModLoader.LoaderUtil
{
	// Token: 0x0200002A RID: 42
	public static class ResourceDataLoader
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x00009D08 File Offset: 0x00007F08
		public static AudioClip GetAudioClipFromWav(Stream raw_data, string clip_name)
		{
			WaveFileReader waveFileReader = new WaveFileReader(raw_data);
			WaveFormat waveFormat = waveFileReader.WaveFormat;
			AudioClip audioClip = AudioClip.Create(clip_name, (int)waveFileReader.SampleCount, waveFormat.Channels, waveFormat.SampleRate, false);
			audioClip.name = clip_name;
			audioClip.SetData(waveFileReader.ReadAllSamples(), 0);
			return audioClip;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00009D54 File Offset: 0x00007F54
		public static AudioClip GetAudioClipFromOgg(Stream raw_data, string clip_name)
		{
			VorbisReader vorbisReader = new VorbisReader(raw_data, false);
			AudioClip audioClip = AudioClip.Create(clip_name, (int)vorbisReader.TotalSamples, vorbisReader.Channels, vorbisReader.SampleRate, false);
			audioClip.name = clip_name;
			audioClip.SetData(vorbisReader.ReadAllSamples(), 0);
			return audioClip;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00009D98 File Offset: 0x00007F98
		public static AudioClip GetAudioClipFromMp3(Stream raw_data, string clip_name)
		{
			Mp3FileReader sourceProvider = new Mp3FileReader(raw_data);
			AudioClip audioClipFromWav;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				WaveFileWriter.WriteWavFileToStream(memoryStream, sourceProvider);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				audioClipFromWav = ResourceDataLoader.GetAudioClipFromWav(memoryStream, clip_name);
			}
			return audioClipFromWav;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00009DE8 File Offset: 0x00007FE8
		public static float[] ReadAllSamples(this WaveFileReader waveFileReader)
		{
			float[] array = new float[waveFileReader.SampleCount * (long)waveFileReader.WaveFormat.Channels];
			int num = 0;
			float[] array2;
			while (waveFileReader.SafeReadNextSampleFrame(out array2) && array2 != null)
			{
				foreach (float num2 in array2)
				{
					array[num] = num2;
					num++;
				}
			}
			return array;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00009E44 File Offset: 0x00008044
		public static float[] ReadAllSamples(this VorbisReader vorbisReader)
		{
			float[] array = new float[vorbisReader.TotalSamples * (long)vorbisReader.Channels];
			vorbisReader.ReadSamples(array.AsSpan<float>());
			return array;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00009E74 File Offset: 0x00008074
		private static bool SafeReadNextSampleFrame(this WaveFileReader waveFileReader, out float[] data)
		{
			bool result;
			try
			{
				data = waveFileReader.ReadNextSampleFrame();
				result = true;
			}
			catch (Exception)
			{
				data = Array.Empty<float>();
				result = false;
			}
			return result;
		}
	}
}
