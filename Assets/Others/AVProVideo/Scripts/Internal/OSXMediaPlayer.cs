#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IPHONE || UNITY_IOS || UNITY_TVOS
#if UNITY_5 || UNITY_5_4_OR_NEWER
#if !UNITY_5_0 && !UNITY_5_1
#define AVPROVIDEO_ISSUEPLUGINEVENT_UNITY52
#endif
#endif

using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using AOT;

//-----------------------------------------------------------------------------
// Copyright 2015-2017 RenderHeads Ltd.  All rights reserverd.
//-----------------------------------------------------------------------------

namespace RenderHeads.Media.AVProVideo
{
	public class OSXMediaPlayer : BaseMediaPlayer
	{
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
		private const string PluginName = "AVProVideo";
#elif UNITY_IPHONE || UNITY_IOS || UNITY_TVOS
		private const string PluginName = "__Internal";
#endif

		// Native Interface

		private enum AVPPluginEventType
		{
			PlayerRender,
			PlayerFreeResources,
		}

		private enum AVPPluginColorSpace
		{
			Uninitialized,
			Gamma,
			Linear
		};

		private enum AVPPlayerStatus
		{
			Failed = -1,
			Unknown,
			ReadyToPlay,
			Playing,
			Finished,
			Seeking,
			Buffering
		}

		private enum AVPLogFlag
		{
			Error	= 1 << 0,
			Warning	= 1 << 1,
			Info	= 1 << 2,
			Debug	= 1 << 3,
			Verbose	= 1 << 4,
		};

		private enum AVPLogLevel
		{
			Off		= 0,
			Error	= AVPLogFlag.Error,
			Warning	= AVPLogFlag.Error | AVPLogFlag.Warning,
			Info	= AVPLogFlag.Error | AVPLogFlag.Warning | AVPLogFlag.Info,
			Debug	= AVPLogFlag.Error | AVPLogFlag.Warning | AVPLogFlag.Info | AVPLogFlag.Debug,
			Verbose	= AVPLogFlag.Error | AVPLogFlag.Warning | AVPLogFlag.Info | AVPLogFlag.Debug | AVPLogFlag.Verbose,
			All		= -1
		};

		[StructLayout(LayoutKind.Sequential, Pack=4)]
		private struct AVPPlayerTextureInfo
		{
			public IntPtr native;
			public int width;
			public int height;
			public int format;
			public int flipped;
		};

		[DllImport(PluginName)]
		private static extern string AVPGetVersion();

#if AVPROVIDEO_ISSUEPLUGINEVENT_UNITY52
		[DllImport(PluginName)]
		private static extern IntPtr AVPGetRenderEventFunc();
#endif

		[DllImport(PluginName)]
		private static extern ErrorCode AVPPlayerGetLastError(IntPtr player);

		[DllImport(PluginName)]
		private static extern double AVPPlayerGetCurrentTime(IntPtr player);

		[DllImport(PluginName)]
		private static extern double AVPPlayerGetDuration(IntPtr player);

		[DllImport(PluginName)]
		private static extern int AVPPlayerGetFrameCount(IntPtr player);

		[DllImport(PluginName)]
		private static extern double AVPPlayerGetFrameRate(IntPtr player);

		[DllImport(PluginName)]
		private static extern long AVPPlayerGetFrameTimeStamp(IntPtr player);

		[DllImport(PluginName)]
		private static extern float AVPPlayerGetNominalFrameRate(IntPtr player);

		[DllImport(PluginName)]
		private static extern int AVPPlayerGetHandle(IntPtr player);

		[DllImport(PluginName)]
		private static extern AVPPlayerStatus AVPPlayerGetStatus(IntPtr player);

		[DllImport(PluginName)]
		private static extern float AVPPlayerGetBufferingProgress(IntPtr player);

		[DllImport(PluginName)]
		private static extern int AVPPlayerGetBufferedTimeRangeCount(IntPtr player);

		[DllImport(PluginName)]
		private static extern bool AVPPlayerGetBufferedTimeRange(IntPtr player, int index, out float start, out float end);

		[DllImport(PluginName)]
		private static extern bool AVPPlayerGetTextures(IntPtr player, [In, Out] AVPPlayerTextureInfo[] textures, ref int count);

		[DllImport(PluginName)]
		private static extern bool AVPPlayerGetTextureTransform(IntPtr player, [In, Out] float[] transform);

		[DllImport(PluginName)]
		private static extern float AVPPlayerGetVolume(IntPtr player);

		[DllImport(PluginName)]
		private static extern bool AVPPlayerHasAudio(IntPtr player);

		[DllImport(PluginName)]
		private static extern bool AVPPlayerHasVideo(IntPtr player);

		[DllImport(PluginName)]
		private static extern bool AVPPlayerHasMetaData(IntPtr player);

		[DllImport(PluginName)]
		private static extern bool AVPPlayerIsLooping(IntPtr player);

		[DllImport(PluginName)]
		private static extern void AVPPlayerSetLooping(IntPtr player, bool looping);

		[DllImport(PluginName)]
		private static extern bool AVPPlayerIsMuted(IntPtr player);

		[DllImport(PluginName)]
		private static extern void AVPPlayerSetMuted(IntPtr player, bool muted);

		[DllImport(PluginName)]
		private static extern void AVPPlayerSetVolume(IntPtr player, float volume);

		[DllImport(PluginName)]
		private static extern IntPtr AVPPlayerNew(bool useYuv);

		[DllImport(PluginName)]
		private static extern IntPtr AVPPlayerRelease(IntPtr player);

		[DllImport(PluginName)]
		private static extern bool AVPPlayerOpenFile(IntPtr player, string path);

		[DllImport(PluginName)]
		private static extern bool AVPPlayerOpenURL(IntPtr player, string url, string headers);

		[DllImport(PluginName)]
		private static extern void AVPPlayerClose(IntPtr player);

		[DllImport(PluginName)]
		private static extern void AVPPlayerPlay(IntPtr player);

		[DllImport(PluginName)]
		private static extern void AVPPlayerPause(IntPtr player);

		[DllImport(PluginName)]
		private static extern void AVPPlayerSeekToTime(IntPtr player, double time, bool fast);

		[DllImport(PluginName)]
		private static extern float AVPPlayerGetPlaybackRate(IntPtr player);

		[DllImport(PluginName)]
		private static extern void AVPPlayerSetPlaybackRate(IntPtr player, float rate);

		[DllImport(PluginName)]
		private static extern bool AVPPlayerUpdate(IntPtr player);

		[DllImport(PluginName)]
		private static extern int AVPPlayerGetAudioTrackCount(IntPtr player);

		[DllImport(PluginName)]
		private static extern string AVPPlayerGetAudioTrackId(IntPtr player, int index);

		[DllImport(PluginName)]
		private static extern int AVPPlayerGetCurrentAudioTrack(IntPtr player);

		[DllImport(PluginName)]
		private static extern string AVPPlayerGetCurrentAudioTrackId(IntPtr player);

		[DllImport(PluginName)]
		private static extern int AVPPlayerGetCurrentAudioTrackBitrate(IntPtr player);

		[DllImport(PluginName)]
		private static extern int AVPPlayerGetVideoTrackCount(IntPtr player);

		[DllImport(PluginName)]
		private static extern string AVPPlayerGetVideoTrackId(IntPtr player, int index);

		[DllImport(PluginName)]
		private static extern int AVPPlayerGetCurrentVideoTrack(IntPtr player);

		[DllImport(PluginName)]
		private static extern string AVPPlayerGetCurrentVideoTrackId(IntPtr player);

		[DllImport(PluginName)]
		private static extern int AVPPlayerGetCurrentVideoTrackBitrate(IntPtr player);

		[DllImport(PluginName)]
		private static extern void AVPPlayerSetAudioTrack(IntPtr player, int track);

		[DllImport(PluginName)]
		private static extern void AVPPlayerSetVideoTrack(IntPtr player, int track);

		[DllImport(PluginName)]
		private static extern int AVPPlayerExtractFrame(IntPtr player, [In, Out] AVPPlayerTextureInfo[] textures, ref int count, double timeout);

		[DllImport(PluginName)]
		private static extern void AVPPluginRegister();

		[DllImport(PluginName)]
		private static extern void AVPPluginInitialise(AVPPluginColorSpace colorSpace);

		[DllImport(PluginName)]
		private static extern void AVPPluginSetDebugLogFunction(IntPtr fn);

		// MediaPlayer Interface

		private static bool _initialised = false;
#if AVPROVIDEO_ISSUEPLUGINEVENT_UNITY52
		private static IntPtr _renderEventFunc = IntPtr.Zero;
#endif
		private static Regex _matchURLRegex = null;

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
#endif
		private delegate void DebugLogCallbackDelegate(int level, int flags, string str);

#if UNITY_IPHONE || UNITY_IOS || UNITY_TVOS
		[MonoPInvokeCallback(typeof(DebugLogCallbackDelegate))]
#endif
		private static void DebugLogCallback(int level, int flags, string str)
		{
			if ((flags & (int)AVPLogFlag.Error) == (int)AVPLogFlag.Error)
			{
				Debug.LogError(str);
			}
			else if ((flags & (int)AVPLogFlag.Warning) == (int)AVPLogFlag.Warning)
			{
				Debug.LogWarning(str);
			}
			else
			{
				Debug.Log(str);
			}
		}

		private static void IssuePluginEvent(AVPPluginEventType type, int param)
		{
			// Build eventId from the type and param.
			int eventId = 0x0FA60000 | ((int)type << 12);
			eventId |= param & 0xfff;

#if AVPROVIDEO_ISSUEPLUGINEVENT_UNITY52
			GL.IssuePluginEvent(_renderEventFunc, eventId);
#else
			GL.IssuePluginEvent(eventId);
#endif
		}

		static void Initialise()
		{
			if (!_initialised)
			{
				_initialised = true;

#if ((UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !UNITY_5) || (UNITY_IPHONE || UNITY_IOS || UNITY_TVOS)
				AVPPluginRegister();
#endif

				DebugLogCallbackDelegate callbackDelegate = new DebugLogCallbackDelegate(DebugLogCallback);
				IntPtr func = Marshal.GetFunctionPointerForDelegate(callbackDelegate);
				AVPPluginSetDebugLogFunction(func);

				AVPPluginColorSpace colorSpace = QualitySettings.activeColorSpace == ColorSpace.Linear ? AVPPluginColorSpace.Linear : AVPPluginColorSpace.Gamma;
				AVPPluginInitialise(colorSpace);

#if AVPROVIDEO_ISSUEPLUGINEVENT_UNITY52
				_renderEventFunc = AVPGetRenderEventFunc();
#endif

				_matchURLRegex = new Regex("^[a-zA-Z][a-zA-Z0-9+-.]*://.*$");
			}
		}

		private IntPtr _player = IntPtr.Zero;	// The native player instance.
		private int _handle = 0;	// Handle to the native player for use with IssuePluginEvent.
		private AVPPlayerStatus _status = AVPPlayerStatus.Unknown;
		private int _planeCount = 0;
		private Texture2D[] _texture = new Texture2D[2];
		private int _width = 0;
		private int _height = 0;
		private bool _flipped = false;
		private bool _isMetaDataReady = false;

		static OSXMediaPlayer()
		{
			Initialise();
		}

		public OSXMediaPlayer(bool useYuv420Textures = false)
		{
			_player = AVPPlayerNew(useYuv420Textures);
			_handle = AVPPlayerGetHandle(_player);
		}

		// Convenience method for calling OSXMediaPlayer.IssuePluginEvent.
		//
		private void IssuePluginEvent(AVPPluginEventType type)
		{
			OSXMediaPlayer.IssuePluginEvent(type, _handle);
		}

		// BaseMediaPlayer Overrides

		public override string GetVersion()
		{
			return AVPGetVersion();
		}

		public override bool OpenVideoFromFile(string path, long offset /* ignored */, string httpHeaderJson)
		{
			if (_matchURLRegex.IsMatch(path))
			{
				return AVPPlayerOpenURL(_player, path, httpHeaderJson);
			}
			else
			{
				return AVPPlayerOpenFile(_player, path);
			}
		}

		public override void CloseVideo()
		{
			AVPPlayerClose(_player);

			if (_texture[0] != null)
			{
				IssuePluginEvent(AVPPluginEventType.PlayerFreeResources);
				// Have to update with zero to release Metal textures!
				for (int i = 0; i < _planeCount; ++i)
				{
					_texture[i].UpdateExternalTexture(IntPtr.Zero);
					Texture2D.Destroy(_texture[i]);
					_texture[i] = null;
				}
			}

			_width = 0;
			_height = 0;
			_isMetaDataReady = false;
			_planeCount = 0;
		}

		public override bool IsLooping()
		{
			return AVPPlayerIsLooping(_player);
		}

		public override void SetLooping(bool looping)
		{
			AVPPlayerSetLooping(_player, looping);
		}

		public override bool HasAudio()
		{
			return AVPPlayerHasAudio(_player);
		}

		public override bool HasVideo()
		{
			return AVPPlayerHasVideo(_player);
		}

		public override bool HasMetaData()
		{
			return _isMetaDataReady;
		}

		public override bool CanPlay()
		{
			return _status >= AVPPlayerStatus.ReadyToPlay;
		}

		public override void Play()
		{
			AVPPlayerPlay(_player);
		}

		public override void Pause()
		{
			AVPPlayerPause(_player);
		}

		public override void Stop()
		{
			AVPPlayerPause(_player);
		}

		public override void Rewind()
		{
			AVPPlayerSeekToTime(_player, 0.0, true);
		}

		public override void Seek(float ms)
		{
			AVPPlayerSeekToTime(_player, ms / 1000.0, false);
		}

		public override void SeekFast(float ms)
		{
			AVPPlayerSeekToTime(_player, ms / 1000.0, true);
		}

		public override float GetCurrentTimeMs()
		{
			return (float)(AVPPlayerGetCurrentTime(_player) * 1000.0f);
		}

		public override void SetPlaybackRate(float rate)
		{
			AVPPlayerSetPlaybackRate(_player, rate);
		}

		public override float GetPlaybackRate()
		{
			return AVPPlayerGetPlaybackRate(_player);
		}

		public override float GetDurationMs()
		{
			return (float)(AVPPlayerGetDuration(_player) * 1000.0f);
		}

		public override int GetVideoWidth()
		{
			return _width;
		}

		public override int GetVideoHeight()
		{
			return _height;
		}

		public override float GetVideoDisplayRate()
		{
			return (float)AVPPlayerGetFrameRate(_player);
		}

		public override bool IsSeeking()
		{
			return _status == AVPPlayerStatus.Seeking;
		}

		public override bool IsPlaying()
		{
			return _status == AVPPlayerStatus.Playing;
		}

		public override bool IsPaused()
		{
			return _status == AVPPlayerStatus.ReadyToPlay;
		}

		public override bool IsFinished()
		{
			return _status == AVPPlayerStatus.Finished;
		}

		public override bool IsBuffering()
		{
			return _status == AVPPlayerStatus.Buffering;
		}

		public override float GetBufferingProgress()
		{
			return AVPPlayerGetBufferingProgress(_player);
		}

		public override int GetBufferedTimeRangeCount()
		{
			return AVPPlayerGetBufferedTimeRangeCount(_player);
		}

		public override bool GetBufferedTimeRange(int index, ref float startTimeMs, ref float endTimeMs)
		{
			return AVPPlayerGetBufferedTimeRange(_player, index, out startTimeMs, out endTimeMs);
		}

		public override bool WaitForNextFrame(Camera camera, int previousFrameCount)
		{
			int count = 2;
			AVPPlayerTextureInfo[] textures = new AVPPlayerTextureInfo[count];
			int ret = AVPPlayerExtractFrame(_player, textures, ref count, 0.0);
			if (ret == 0)
			{
				_planeCount = count;
				for (int i = 0; i < count; ++i)
				{
					if (_texture[i] == null || _texture[i].width != textures[i].width || _texture[i].height != textures[i].height || _texture[i].format != (TextureFormat)textures[i].format)
					{
						_texture[i] = Texture2D.CreateExternalTexture(textures[i].width, textures[i].height, (TextureFormat)textures[i].format, /*mipmap*/ false, /*linear*/ false, textures[i].native);
						if (i == 0)
						{
							_width = textures[i].width;
							_height = textures[i].height;
							_flipped = textures[i].flipped != 0;
						}
					}
					else
					{
						_texture[i].UpdateExternalTexture(textures[i].native);
					}
				}
				return true;
			}
			return false;
		}

		// IMediaProducer

		public override int GetTextureCount()
		{
			return _planeCount;
		}

		public override Texture GetTexture(int index)
		{
			return _texture[index];
		}

		public override int GetTextureFrameCount()
		{
			return AVPPlayerGetFrameCount(_player);
		}

		public override long GetTextureTimeStamp()
		{
			return AVPPlayerGetFrameTimeStamp(_player);
		}

		public override bool RequiresVerticalFlip()
		{
			return _flipped;
		}

		public override float[] GetTextureTransform()
		{
			float[] transform = new float[6];
			AVPPlayerGetTextureTransform(_player, transform);
			return transform;
		}

		//

		public override bool IsMuted()
		{
			return AVPPlayerIsMuted(_player);
		}

		public override void MuteAudio( bool bMute )
		{
			AVPPlayerSetMuted(_player, bMute);
		}

		public override void SetVolume(float volume)
		{
			AVPPlayerSetVolume(_player, volume);
		}

		public override float GetVolume()
		{
			return AVPPlayerGetVolume(_player);
		}

		public override int GetAudioTrackCount()
		{
			return AVPPlayerGetAudioTrackCount(_player);
		}

		public override string GetAudioTrackId(int index)
		{
			string identifier = AVPPlayerGetAudioTrackId(_player, index);
			if (identifier == null)
				identifier = base.GetAudioTrackId(index);
			return identifier;
		}

		public override int GetCurrentAudioTrack()
		{
			return AVPPlayerGetCurrentAudioTrack(_player);
		}

		public override void SetAudioTrack(int track)
		{
			AVPPlayerSetAudioTrack(_player, track);
		}

		public override string GetCurrentAudioTrackId()
		{
			return AVPPlayerGetCurrentAudioTrackId(_player);
		}

		public override int GetCurrentAudioTrackBitrate()
		{
			return AVPPlayerGetCurrentAudioTrackBitrate(_player);
		}

		public override int GetVideoTrackCount()
		{
			return AVPPlayerGetVideoTrackCount(_player);
		}

		public override int GetCurrentVideoTrack()
		{
			return AVPPlayerGetCurrentVideoTrack(_player);
		}

		public override void SetVideoTrack(int index)
		{
			AVPPlayerSetVideoTrack(_player, index);
		}

		public override string GetVideoTrackId(int index)
		{
			string identifier = AVPPlayerGetVideoTrackId(_player, index);
			if (identifier == null)
				identifier = base.GetVideoTrackId(index);
			return identifier;
		}

		public override string GetCurrentVideoTrackId()
		{
			return AVPPlayerGetCurrentVideoTrackId(_player);
		}

		public override int GetCurrentVideoTrackBitrate()
		{
			return AVPPlayerGetCurrentVideoTrackBitrate(_player);
		}

		public override float GetVideoFrameRate()
		{
			return AVPPlayerGetNominalFrameRate(_player);
		}

		public override void Render()
		{

		}

		public void UpdateTextures()
		{
			AVPPlayerTextureInfo[] textures = new AVPPlayerTextureInfo[2];
			int count = textures.Length;
			if (AVPPlayerGetTextures(_player, textures, ref count))
			{
				_planeCount = count;
				for (int i = 0; i < count; ++i)
				{
					if (_texture[i] == null || _texture[i].width != textures[i].width || _texture[i].height != textures[i].height || _texture[i].format != (TextureFormat)textures[i].format)
					{
						_texture[i] = Texture2D.CreateExternalTexture(textures[i].width, textures[i].height, (TextureFormat)textures[i].format, /*mipmap*/ false, /*linear*/ false, textures[i].native);
						if (i == 0)
						{
							_width = textures[i].width;
							_height = textures[i].height;
							_flipped = textures[i].flipped != 0;
						}
					}
					else
					{
						_texture[i].UpdateExternalTexture(textures[i].native);
					}
				}
			}
		}

		public override void Update()
		{
			_status = AVPPlayerGetStatus(_player);

			if (AVPPlayerUpdate(_player))
			{
				IssuePluginEvent(AVPPluginEventType.PlayerRender);
			}

			_lastError = AVPPlayerGetLastError(_player);
			UpdateTextures();

			UpdateSubtitles();

			// Check for meta data to become available
			if (!_isMetaDataReady)
			{
				if (AVPPlayerHasMetaData(_player) || CanPlay())
				{
					// MOZ - had to move this outside of HasVideo check
					_isMetaDataReady = true;

					if (HasVideo())
					{
						if (_width > 0 && _height > 0)
						{
							if (Mathf.Max(_width, _height) > SystemInfo.maxTextureSize)
							{
								Debug.LogError("[AVProVideo] Video dimensions larger than maxTextureSize");
							}
						}
						_playerDescription = "AVFoundation";
						Helper.LogInfo("Using playback path: " + _playerDescription + " (" + _width + "x" + _height + "@" + GetVideoFrameRate().ToString("F2") + ")");
					}
					else if (HasAudio())
					{

					}
				}
			}
		}

		public override void Dispose()
		{
			CloseVideo();
			AVPPlayerRelease(_player);
			_player = IntPtr.Zero;
		}

	}
}

#endif
